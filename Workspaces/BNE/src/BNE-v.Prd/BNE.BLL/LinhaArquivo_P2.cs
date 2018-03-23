//-- Data: 22/05/2014 11:00
//-- Autor: Francisco Ribas

using BNE.BLL.Custom;
using BNE.EL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace BNE.BLL
{
    public partial class LinhaArquivo // Tabela: BNE_Linha_Arquivo
    {
        #region Propriedades

        #region DescricaoConteudo
        /// <summary>
        /// Tamanho do campo: 500.
        /// Campo obrigatório.
        /// Ao ser definido um valor, reconhece o tipo de linha, consistindo o conteúdo com o tipo do arquivo e carrega o objeto Transacao ou BoletoBancario relativo.
        /// </summary>
        public string DescricaoConteudo
        {
            get
            {
                return this._descricaoConteudo;
            }
            set
            {
                this._descricaoConteudo = value;
                this.ReconhecerTipoLinha();
                this.CarregarTransacoes();
                this._modified = true;
            }
        }
        #endregion DescricaoConteudo

        #endregion Propriedades

        #region Consultas
        private const String SP_ULTIMA_LINHA_DE_TRANSACAO_REMETIDA = @"SELECT TOP 1 la.* FROM BNE.BNE_Linha_Arquivo la
                                                                        INNER JOIN BNE.BNE_Arquivo a ON la.Idf_Arquivo = a.Idf_Arquivo
                                                                        INNER JOIN BNE.TAB_Tipo_Arquivo ta ON a.Idf_Tipo_Arquivo = ta.Idf_Tipo_Arquivo
                                                                        WHERE la.Idf_Transacao = @Idf_Transacao
		                                                                        AND ta.Flg_Remessa = 1
                                                                        ORDER BY a.Dta_Cadastro DESC";
        #endregion

        #region Metodos

        #region Geração de Arquivos

        /// <summary>
        /// Carrega a última linha remetida de uma transação
        /// </summary>
        /// <param name="idTransacao">Identificador da transação</param>
        /// <param name="objLinhaArquivo">Parametro que deverá ser preenchido com as informações da última linha</param>
        /// <returns>True se a transação foi encontrada</returns>
        public static bool CarregarUltimaLinhaRemetidaDeTransacao(int idTransacao, out LinhaArquivo objLinhaArquivo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Transacao", SqlDbType.Int, 4));
            parms[0].Value = idTransacao;
            objLinhaArquivo = new LinhaArquivo();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_ULTIMA_LINHA_DE_TRANSACAO_REMETIDA, parms))
            {
                if (SetInstance(dr, objLinhaArquivo))
                {
                    return true;
                }
            }

            return false;
        }

        #region Gerar Headers
        /// <summary>
        /// Gera o registro header (primeira linha)
        /// </summary>
        /// <param name="objArquivo">Arquivo no qual o Header deve ser gerado</param>
        /// <exception cref="Exception">Lança uma exceção caso não seja possível gerar o header.</exception>
        public static void GerarHeader(Arquivo objArquivo)
        {
            switch ((Enumeradores.TipoArquivo)objArquivo.TipoArquivo.IdTipoArquivo)
            {
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaRegistroBoletos:
                    GerarHeaderRegistroBoletoHSBC(objArquivo);
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoHSBC:
                    GerarHeaderRemessaDebitoHSBC(objArquivo);
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoBB:
                    GerarHeaderRemessaDebitoBB(objArquivo);
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoCNR:
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoRegistroBoletos:
                default:
                    throw new Exception(String.Format("Tipo de arquivo {0} não pode ser gerado pelo sistema. É um arquivo de recebimento.", ((Enumeradores.TipoArquivo)objArquivo.TipoArquivo.IdTipoArquivo).ToString()));
            }
        }
        /// <summary>
        /// Gera o header para o arquivo de Remessa de débito do Banco do Brasil
        /// </summary>
        /// <param name="objArquivo"></param>
        private static void GerarHeaderRemessaDebitoBB(BLL.Arquivo objArquivo)
        {
            //Sequencia do arquivo: total de arquivos para débito já gerados + 1
            int sequenciaArquivo = Arquivo.RecuperarNumeroDeArquivos(Enumeradores.TipoArquivo.RemessaDebitoHSBC) + 1;

            //LAYOUT DOS REGISTROS 
            //Registro “A”  -  Header 
            //Obrigatório em todos os arquivos.

            //A01-Código do Registro
            String sLinha = "A";
            //A02-Código de Remessa
            sLinha += "1"; //Código de Remessa
            // A03-Código do Convênio
            sLinha += Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.ConvenioDebitoBB)), 20);
            //A04-Nome da Empresa
            sLinha += Helper.StringParaTamanhoExato("Banco Nacional de Empregos Ltda", 20);
            //A05-Código do Banco
            sLinha += "001";//Código do Banco - fixo 001
            //A06-Nome do Banco
            sLinha += Helper.StringParaTamanhoExato("Banco do Brasil S.A.", 20);
            //A07-Data de Geração
            sLinha += DateTime.Now.ToString("yyyyMMdd");
            //A08-Número Seqüencial do Arquivo (NSA)
            sLinha += Helper.NumericoParaTamanhoExato(sequenciaArquivo, 6);
            //A09-Versão do Lay-out
            sLinha += "04";
            //A10-Identificação do Serviço
            sLinha += "DÉBITO AUTOMÁTICO";
            //A11-Reservado para o futuro
            //sLinha += Helper.StringParaTamanhoExato("", 52);
            sLinha += Helper.StringParaTamanhoExato("", 47);
            sLinha += Helper.StringParaTamanhoExato("TESTE", 5);

            objArquivo.Linhas.Add(new LinhaArquivo { Arquivo = objArquivo, DescricaoConteudo = sLinha, NumeroLinha = objArquivo.Linhas.Count + 1, TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header) });

        }

        /// <summary>
        /// Gera o header para o arquivo de registro de boleto do HSBC
        /// </summary>
        /// <param name="objArquivo">Arquivo no qual o Header deve ser gerado</param>
        private static void GerarHeaderRegistroBoletoHSBC(Arquivo objArquivo)
        {

            String sLinha = "0"; //Código do registro
            sLinha += "1"; //Código do arquivo
            sLinha += Helper.StringParaTamanhoExato("REMESSA", 7); // Literal Arquivo
            sLinha += "01"; //Código do Serviço
            sLinha += Helper.StringParaTamanhoExato("COBRANCA", 15); // Literal Serviço
            sLinha += "0"; //Zero
            sLinha += Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CobreBemXCodigoAgencia)), 4); // Agência Cedente
            sLinha += "55"; //Sub-conta
            // Conta Corrente
            String temp = Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CobreBemXCodigoAgencia)), 4);
            temp += Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CobreBemXNumeroContaCorrente).Replace("-", "")), 7);
            sLinha += Helper.NumericoParaTamanhoExato(Convert.ToInt32(temp), 11);
            //Uso do Banco
            sLinha += "  ";
            //Nome do Cliente
            sLinha += Helper.StringParaTamanhoExato("Bne - Banco Nacional de Empregos Ltda", 30);
            //Código do Banco
            sLinha += "399";
            //Nome do Banco
            sLinha += Helper.StringParaTamanhoExato("HSBC", 15);
            //Data Gravação
            sLinha += DateTime.Now.ToString("ddMMyy");
            //Densidade
            sLinha += "01600";
            //Literal Densidade
            sLinha += "BPI";
            //Uso do Banco
            sLinha += "  ";
            //Sigla Layout
            sLinha += "LANCV08";
            //Uso do Banco
            sLinha += Helper.StringParaTamanhoExato("", 277);
            //Numero Sequencial
            sLinha += Helper.NumericoParaTamanhoExato(objArquivo.Linhas.Count + 1, 6);

            objArquivo.Linhas.Add(new LinhaArquivo { Arquivo = objArquivo, DescricaoConteudo = sLinha, NumeroLinha = objArquivo.Linhas.Count + 1, TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header) });
        }

        /// <summary>
        /// Gera o header para o arquivo de Remessa de débito do HSBC
        /// </summary>
        /// <param name="objArquivo">Arquivo no qual o Header deve ser gerado</param>
        private static void GerarHeaderRemessaDebitoHSBC(Arquivo objArquivo)
        {
            //Sequencia do arquivo: total de arquivos para débito já gerados + 1
            int sequenciaArquivo = Arquivo.RecuperarNumeroDeArquivos(Enumeradores.TipoArquivo.RemessaDebitoHSBC) + 1;

            String sLinha = "A"; //Código do registro
            sLinha += "1"; //Código de Remessa
            // Código do Convênio (própria conta do HSBC)
            sLinha += Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.ConvenioDebitoHSBC)), 11);
            sLinha += Helper.StringParaTamanhoExato("", 9); //Brancos para completar o campo (seguindo documentacao)
            //Nome do Cliente Credor
            sLinha += Helper.StringParaTamanhoExato("Banco Nacional de Empregos Ltda", 20);
            //Código do Banco - fixo 399
            sLinha += "399";
            //Nome do Banco
            sLinha += Helper.StringParaTamanhoExato("HSBC", 20);
            //Data de geração do arquivo
            sLinha += DateTime.Now.ToString("yyyyMMdd");
            //Numero Sequencial do Arquivo
            sLinha += Helper.NumericoParaTamanhoExato(sequenciaArquivo, 6);
            //Versão do Layout - Fixo 04
            sLinha += "04";
            //Identificação do Serviço
            sLinha += Helper.StringParaTamanhoExato("DEBITO AUTOMATICO", 17);
            //Brancis
            sLinha += Helper.StringParaTamanhoExato("", 52);

            objArquivo.Linhas.Add(new LinhaArquivo { Arquivo = objArquivo, DescricaoConteudo = sLinha, NumeroLinha = objArquivo.Linhas.Count + 1, TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header) });
        }

        #endregion Gerar Headers

        #region Gerar Registros
        /// <summary>
        /// Gera um registro de remessa no objArquivo informado para o objeto tipo Transacao ou BoletoBancario informado
        /// </summary>
        /// <param name="objArquivo">Arquivo onde o registro deve ser gerado</param>
        /// <param name="objCobranca">Objeto do tipo Transacao ou BoletoBancario relativo ao qual o registro será gerado</param>
        /// <exception cref="Exception">Lança uma exceção caso não seja possível gerar o registro.</exception>
        /// <exception cref="Exception">Lança uma exceção caso o parâmetro objCobranca não seja do tipo BoletoBancario ou Transacao.</exception>
        public static void GerarRegistroRemessa(Arquivo objArquivo, object objCobranca, bool inclusao = true)
        {
            if (objCobranca.GetType() != typeof(BoletoBancario) && objCobranca.GetType() != typeof(Transacao))
            {
                throw new Exception(String.Format("Somente instancias das classes BoletoBancario ou Transacao são aceitas (objCobranca informado é do tipo {0})", typeof(Transacao).ToString()));
            }

            switch ((Enumeradores.TipoArquivo)objArquivo.TipoArquivo.IdTipoArquivo)
            {
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaRegistroBoletos:
                    GerarRegistroRegistroBoletoHSBC(objArquivo, (BoletoBancario)objCobranca, inclusao);
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoHSBC:
                    GerarRemessaDebitoHSBC(objArquivo, (Transacao)objCobranca, inclusao);
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoBB:
                    GerarRemessaDebitoBB(objArquivo, (Transacao)objCobranca, inclusao);
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoCNR:
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoRegistroBoletos:
                default:
                    throw new Exception(String.Format("Tipo de arquivo {0} não pode ser gerado pelo sistema. É um arquivo de recebimento.", ((Enumeradores.TipoArquivo)objArquivo.TipoArquivo.IdTipoArquivo).ToString()));
            }
        }

        /// <summary>
        /// Gera o registro para efetuar o registro de boletos no HSBC
        /// </summary>
        /// <param name="objArquivo">Objeto onde o registro deve ser gerado</param>
        /// <param name="objBoletoBancario">Objeto BoletoBancario para qual a linha deve ser gerada</param>
        private static void GerarRegistroRegistroBoletoHSBC(Arquivo objArquivo, BoletoBancario objBoletoBancario, bool inclusao = true)
        {
            String linha = "";
            String temp = "";

            //Código do Registro
            linha += "1";
            //Código de Inscrição
            linha += "02"; //CNPJ
            //Número de Inscricao
            linha += Helper.StringParaTamanhoExato(objBoletoBancario.CedenteNumCNPJCPF, 14);
            //Zero
            linha += "0";
            // Agência Cedente
            linha += Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CobreBemXCodigoAgencia)), 4);
            //Sub-conta
            linha += "55";
            // Conta Corrente
            temp = Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CobreBemXCodigoAgencia)), 4);
            temp += Helper.NumericoParaTamanhoExato(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CobreBemXNumeroContaCorrente).Replace("-", "")), 7);
            linha += Helper.NumericoParaTamanhoExato(Convert.ToInt32(temp), 11);
            //Brancos
            linha += Helper.StringParaTamanhoExato(" ", 2);
            //Controle do Participante
            linha += Helper.NumericoParaTamanhoExato(objBoletoBancario.NumeroNossoNumero, 25);
            //Nosso Número
            linha += Helper.NumericoParaTamanhoExato(0, 11); //Enviando zerado pois o banco gerará o nosso número.
            //Desconto Data (2)
            linha += "999999";
            //Valor do Desconto (2)
            linha += Helper.NumericoParaTamanhoExato(0, 11);
            //Desconto Data (3)
            linha += "999999";
            //Valor do Desconto (3)
            linha += Helper.MonetarioParaTamanhoExato(0, 11);
            //Carteira
            linha += Helper.NumericoParaTamanhoExato(1, 1);
            //Código da Ocorrência
            if (inclusao)
            {
                linha += Helper.NumericoParaTamanhoExato(1, 2); //Remessa
            }
            else
            {
                linha += Helper.NumericoParaTamanhoExato(2, 2); //Pedido de baixa
            }
            //Seu Número
            linha += Helper.NumericoParaTamanhoExato(0, 10);
            //Vencimento
            linha += objBoletoBancario.DataVencimento.Value.ToString("ddMMyy");
            //Valor do Titulo
            linha += Helper.MonetarioParaTamanhoExato(objBoletoBancario.ValorBoleto, 13);
            //Banco Cobrador
            linha += "399";
            //Agência Depositária
            linha += Helper.NumericoParaTamanhoExato(0, 5);
            //Espécia
            linha += Helper.NumericoParaTamanhoExato(10, 2);
            //Aceite
            linha += "A"; //Aceito
            //Data Emissão
            linha += objBoletoBancario.DataEmissao.Value.ToString("ddMMyy");
            //Instrução 1
            linha += "67";
            //Instrução 2
            linha += "00";
            //Juros de Mora
            linha += Helper.MonetarioParaTamanhoExato(0, 13);
            //Desconto Data
            linha += "999999";
            //Valor do Desconto
            linha += Helper.MonetarioParaTamanhoExato(0, 13);
            //Valor do IOF
            linha += Helper.MonetarioParaTamanhoExato(0, 13);
            //Valor do abatimento
            linha += Helper.MonetarioParaTamanhoExato(0, 13);

            if (string.IsNullOrEmpty(objBoletoBancario.SacadoNumCNPJCPF))
            {
                linha += Helper.NumericoParaTamanhoExato(objBoletoBancario.FlagEmpresa ? 2 : 1, 2);//Codigo de Inscricao
                linha += Helper.NumericoParaTamanhoExato(objBoletoBancario.SacadoNumCNPJCPF, 14);//Número de Inscricao
            }
            else
            {
                linha += Helper.NumericoParaTamanhoExato(98, 2);//Não tem
                linha += Helper.NumericoParaTamanhoExato(0, 14);//Não tem
            }

            //Nome do Pagador
            linha += Helper.StringParaTamanhoExato(objBoletoBancario.SacadoNome, 40);

            //Endereço do pagador
            linha += Helper.StringParaTamanhoExato(objBoletoBancario.SacadoEnderecoLogradouro, 38);
            //Instruções de não recebimento do boleto
            linha += "  ";
            //Bairro do pagador
            linha += Helper.StringParaTamanhoExato(objBoletoBancario.SacadoEnderecoBairro, 12);
            //Cep do Pagador + Sufixo do pagador
            linha += Helper.NumericoParaTamanhoExato(Convert.ToDecimal(objBoletoBancario.SacadoEnderecoCEP), 8);
            //Cidade do Pagador
            if (String.IsNullOrEmpty(objBoletoBancario.SacadoEnderecoCidade.NomeCidade))
                objBoletoBancario.SacadoEnderecoCidade.CompleteObject();
            linha += Helper.StringParaTamanhoExato(objBoletoBancario.SacadoEnderecoCidade.NomeCidade, 15);
            //Sigla da UF
            if (String.IsNullOrEmpty(objBoletoBancario.SacadoEnderecoCidade.Estado.SiglaEstado))
                objBoletoBancario.SacadoEnderecoCidade.Estado.CompleteObject();
            linha += Helper.StringParaTamanhoExato(objBoletoBancario.SacadoEnderecoCidade.Estado.SiglaEstado, 2);
            //Sacador / Avalista
            linha += Helper.StringParaTamanhoExato(" ", 39);
            //Tipo de Boleto
            linha += " ";
            //Prazo de protesto
            linha += "  ";
            //Tipo de Moeda
            linha += "9";
            //Número Sequencial
            linha += Helper.NumericoParaTamanhoExato(objArquivo.Linhas.Count + 1, 6);

            objArquivo.Linhas.Add(new LinhaArquivo
            {
                Arquivo = objArquivo,
                BoletoBancario = objBoletoBancario,
                DescricaoConteudo = linha,
                NumeroLinha = objArquivo.Linhas.Count + 1,
                TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC)
            });

        }

        /// <summary>
        /// Gera o registro para a inclusão ou cancelamento do débito automático
        /// </summary>
        /// <param name="objArquivo">Objeto onde o registro deve ser gerado</param>
        /// <param name="objTransacao">Objeto Transacao para qual a linha deve ser gerada</param>
        /// <param name="inclusao">Boolean que indica se o arquivo é de inclusão. Se verdadeiro, o registro é de inclusão.</param>
        private static void GerarRemessaDebitoBB(Arquivo objArquivo, Transacao objTransacao, bool inclusao = true)
        {
            String linha = "";

            //E01-Código do Registro 
            linha += "E";

            //E02-Identificação do Cliente na Empresa
            linha += Helper.NumericoParaTamanhoExato(objTransacao.IdTransacao, 25);

            //E03-Agência para Débito
            linha += Helper.NumericoParaTamanhoExato(objTransacao.DescricaoAgenciaDebito, 4);

            //E04-Identificação do Cliente no Banco
            linha += Helper.NumericoParaTamanhoExato(objTransacao.DescricaoContaCorrenteDebito, 14); //Conta corrente com DV

            //E05-Data do Vencimento
            Pagamento objPagamento;
            if (!Pagamento.CarregarPagamentoDeTransacao(objTransacao.IdTransacao, out objPagamento))
            {
                throw new Exception("Pagamento de transação não encontrado");
            }
            if (!objPagamento.DataVencimento.HasValue)
            {
                throw new Exception("Data de vencimento do pagamento não definida");
            }
            Int32 diasMinimoParaVencimento = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagamentoDebitoDiasMinimosVencimentoParaEnvio));
            DateTime dtaVencimento = objPagamento.DataVencimento.Value;
            if ((objPagamento.DataVencimento.Value - DateTime.Now).TotalDays < diasMinimoParaVencimento)
            {
                dtaVencimento = DateTime.Now.AddDays(diasMinimoParaVencimento);
            }
            linha += dtaVencimento.ToString("yyyyMMdd");

            //E06-Valor do Débito
            linha += Helper.MonetarioParaTamanhoExato(objTransacao.ValorDocumento, 15);

            //E07-Código da moeda
            linha += "03"; //Fixo 03 - Real

            //E08-Uso da Empresa
            //Não será tratada pelo banco
            linha += Helper.NumericoParaTamanhoExato(0, 49);
            //valor dos tributos - Lei n. 10.833
            linha += Helper.NumericoParaTamanhoExato(0, 10);
            //Se X na ultima posição = FIDC
            //Se Y na ultima posição = Lei n. 10.833
            linha += "X";
            //E09-Reservado para o futuro
            linha += Helper.StringParaTamanhoExato("", 20);
            //E10-Código do Movimento
            if (inclusao)
            {
                linha += "0"; //Debito Automatico
            }
            else
            {
                linha += "1"; //Cancelamento
            }


            objArquivo.Linhas.Add(new LinhaArquivo
            {
                Arquivo = objArquivo,
                Transacao = objTransacao,
                DescricaoConteudo = linha,
                NumeroLinha = objArquivo.Linhas.Count + 1,
                TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC)
            });

        }

        /// <summary>
        /// Gera o registro para a inclusão ou cancelamento do débito automático
        /// </summary>
        /// <param name="objArquivo">Objeto onde o registro deve ser gerado</param>
        /// <param name="objTransacao">Objeto Transacao para qual a linha deve ser gerada</param>
        /// <param name="inclusao">Boolean que indica se o arquivo é de inclusão. Se verdadeiro, o registro é de inclusão.</param>
        private static void GerarRemessaDebitoHSBC(Arquivo objArquivo, Transacao objTransacao, bool inclusao = true)
        {
            String linha = "";

            //Código do Registro
            linha += "E";
            //Identificação do Cliente Pagador - Será enviado o ID da transacao
            linha += Helper.NumericoParaTamanhoExato(objTransacao.IdTransacao, 20);
            //Brancos
            linha += Helper.StringParaTamanhoExato("", 5);
            //Agência do Banco para débito
            linha += Helper.NumericoParaTamanhoExato(objTransacao.DescricaoAgenciaDebito, 4);
            //Identificação do cliente consumidor no banco (conta corrente)
            linha += "399"; //conteúdo fixo, indicando o HSBC
            linha += Helper.NumericoParaTamanhoExato(objTransacao.DescricaoAgenciaDebito, 4); //Agência
            linha += Helper.NumericoParaTamanhoExato(objTransacao.DescricaoContaCorrenteDebito, 7); //Conta corrente com DV
            //Data do vencimento
            Pagamento objPagamento;
            if (!Pagamento.CarregarPagamentoDeTransacao(objTransacao.IdTransacao, out objPagamento))
            {
                throw new Exception("Pagamento de transação não encontrado");
            }
            if (!objPagamento.DataVencimento.HasValue)
            {
                throw new Exception("Data de vencimento do pagamento não definida");
            }
            //Data de vencimento tem que ser no mínimo 4 dias úteis. Para garantir o débito, enviaremos 1 dias para vencimento.
            Int32 diasMinimoParaVencimento = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagamentoDebitoDiasMinimosVencimentoParaEnvio));
            DateTime dtaVencimento = objPagamento.DataVencimento.Value;
            if ((objPagamento.DataVencimento.Value - DateTime.Now).TotalDays < diasMinimoParaVencimento)
            {
                dtaVencimento = DateTime.Now.AddDays(diasMinimoParaVencimento);
            }
            linha += dtaVencimento.ToString("yyyyMMdd");
            //Valor do Débito
            linha += Helper.MonetarioParaTamanhoExato(objTransacao.ValorDocumento, 15);
            //Código da Moeda
            linha += "03"; //Fixo 03 - Real
            //Uso do Cliente Credor
            linha += Helper.StringParaTamanhoExato("", 31);
            //CPF/CNPJ do devedor
            if (objTransacao.NumeroCPFTitularContaCorrenteDebito.HasValue)
            {
                linha += Helper.NumericoParaTamanhoExato(objTransacao.NumeroCPFTitularContaCorrenteDebito.Value, 14);
            }
            else if (objTransacao.NumeroCNPJTitularContaCorrenteDebito.HasValue)
            {
                linha += Helper.NumericoParaTamanhoExato(objTransacao.NumeroCNPJTitularContaCorrenteDebito.Value, 14);
            }
            else
            {
                throw new Exception("CPF ou CNPJ não indicados na transação");
            }
            //Valor Base de Cálculo IOF
            linha += Helper.NumericoParaTamanhoExato(0, 15); // enviar zerado - campo exclusivo para seguradoras
            //Tipo de Débito
            linha += "19"; //19 = Serviços diversos
            //Alíquota IOF
            linha += Helper.NumericoParaTamanhoExato(0, 7); // enviar zerado - campo exclusivo para seguradoras
            //Brancos
            linha += Helper.StringParaTamanhoExato("", 11);
            //Código do movimento
            if (inclusao)
            {
                linha += "0"; //Inclusão
            }
            else
            {
                linha += "1"; //Cancelamento
            }


            objArquivo.Linhas.Add(new LinhaArquivo
            {
                Arquivo = objArquivo,
                Transacao = objTransacao,
                DescricaoConteudo = linha,
                NumeroLinha = objArquivo.Linhas.Count + 1,
                TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC)
            });

        }

        #endregion Gerar Registros

        #region Gerar Footer

        /// <summary>
        /// Gera um registro de remessa no objArquivo informado para o objeto tipo Transacao ou BoletoBancario informado
        /// </summary>
        /// <param name="objArquivo">Arquivo onde o registro deve ser gerado</param>
        /// <param name="objCobranca">Objeto do tipo Transacao ou BoletoBancario relativo ao qual o registro será gerado</param>
        public static void GerarFooter(Arquivo objArquivo)
        {
            switch ((Enumeradores.TipoArquivo)objArquivo.TipoArquivo.IdTipoArquivo)
            {
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaRegistroBoletos:
                    GerarFooterRegistroBoletoHSBC(objArquivo);
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoHSBC:
                    GerarFooterRemessaDebitoHSBC(objArquivo);
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoBB:
                    GerarFooterRemessaDebitoBB(objArquivo);
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoCNR:
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoRegistroBoletos:
                default:
                    throw new Exception(String.Format("Tipo de arquivo {0} não pode ser gerado pelo sistema. É um arquivo de recebimento.", ((Enumeradores.TipoArquivo)objArquivo.TipoArquivo.IdTipoArquivo).ToString()));
            }
        }

        /// <summary>
        /// Gera o footer para o arquivo de Remessa de débito do Banco do Brasil
        /// </summary>
        /// <param name="objArquivo">Arquivo no qual o Header deve ser gerado</param>
        private static void GerarFooterRemessaDebitoBB(BLL.Arquivo objArquivo)
        {
            //Z01-Código do Registro
            String sLinha = "Z";

            //Z02-Total de registros do arquivo
            sLinha += Helper.NumericoParaTamanhoExato(objArquivo.Linhas.Count + 1, 6);

            //Z03-Valor total dos registros do arquivo
            List<LinhaArquivo> registros = new List<LinhaArquivo>();
            for (int i = 1; i < objArquivo.Linhas.Count; i++)
            {

                registros.Add(objArquivo.Linhas[i]);
            }
            sLinha += Helper.NumericoParaTamanhoExato(registros.Sum(item => Convert.ToDecimal(item.DescricaoConteudo.Substring(53, 14))), 17);

            //Z04-Reservado para o futuro
            sLinha += Helper.StringParaTamanhoExato("", 126);
            objArquivo.Linhas.Add(new LinhaArquivo { Arquivo = objArquivo, DescricaoConteudo = sLinha, NumeroLinha = objArquivo.Linhas.Count + 1, TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header) });
        }


        /// <summary>
        /// Gera o footer para o arquivo de registro de boleto do HSBC
        /// </summary>
        /// <param name="objArquivo">Arquivo no qual o Header deve ser gerado</param>
        private static void GerarFooterRegistroBoletoHSBC(Arquivo objArquivo)
        {
            //Código do registro
            String sLinha = "9";
            //Uso do Banco
            sLinha += Helper.StringParaTamanhoExato(" ", 393);
            //Numero Sequencial
            sLinha += Helper.NumericoParaTamanhoExato(objArquivo.Linhas.Count + 1, 6);

            objArquivo.Linhas.Add(new LinhaArquivo { Arquivo = objArquivo, DescricaoConteudo = sLinha, NumeroLinha = objArquivo.Linhas.Count + 1, TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header) });
        }

        /// <summary>
        /// Gera o footer para o arquivo de Remessa de débito do HSBC
        /// </summary>
        /// <param name="objArquivo">Arquivo no qual o Header deve ser gerado</param>
        private static void GerarFooterRemessaDebitoHSBC(Arquivo objArquivo)
        {
            String sLinha = "Z"; //Código do registro
            sLinha += Helper.NumericoParaTamanhoExato(objArquivo.Linhas.Count + 1, 6); //Total de registros no arquivo - Inclusive Header and Footer
            sLinha += Helper.MonetarioParaTamanhoExato(objArquivo.Linhas.Where(ln => ln.TipoLinhaArquivo.IdTipoLinhaArquivo == (int)Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC).Sum(ln => ln.Transacao.ValorDocumento), 17); //Valor Total dos registros no arquivo
            sLinha += Helper.NumericoParaTamanhoExato(0, 17);
            sLinha += Helper.StringParaTamanhoExato("", 109);
            objArquivo.Linhas.Add(new LinhaArquivo { Arquivo = objArquivo, DescricaoConteudo = sLinha, NumeroLinha = objArquivo.Linhas.Count + 1, TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Footer) });
        }

        #endregion Gerar Footer

        #endregion Geração de Arquivos

        #region Recepção de Arquivos
        /// <summary>
        /// Reconhece o tipo da linha através das carcterísticas do arquivo e linha.
        /// </summary>
        /// <exception cref="Exception">Lança uma exceção caso o tipo da linha não condiza com o tipo do arquivo.</exception>
        public void ReconhecerTipoLinha()
        {
            try
            {
                switch ((Enumeradores.TipoArquivo)this.Arquivo.TipoArquivo.IdTipoArquivo)
                {
                    case BNE.BLL.Enumeradores.TipoArquivo.RetornoCNR:
                        if (this.DescricaoConteudo[0] == '0')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header);
                        else if (this.DescricaoConteudo[0] == '9')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Footer);
                        else if (this.DescricaoConteudo[0] == '1')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.BoletoCNR);
                        else
                            throw new Exception(String.Format("Registro do tipo '{0}' não reconhecido para o layout de retorno de Boletos CNR", this.DescricaoConteudo[0].ToString()));
                        break;
                    case BNE.BLL.Enumeradores.TipoArquivo.RemessaRegistroBoletos:
                        if (this.DescricaoConteudo[0] == '0')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header);
                        else if (this.DescricaoConteudo[0] == '9')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Footer);
                        else if (this.DescricaoConteudo[0] == '1')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC);
                        else
                            throw new Exception(String.Format("Registro do tipo '{0}' não reconhecido para o layout de remessa de Registro de Boletos HSBC", this.DescricaoConteudo[0].ToString()));
                        break;
                    case BNE.BLL.Enumeradores.TipoArquivo.RetornoRegistroBoletos:
                        if (this.DescricaoConteudo[0] == '0')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header);
                        else if (this.DescricaoConteudo[0] == '9')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Footer);
                        else if (this.DescricaoConteudo[0] == '1')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC);
                        else
                            throw new Exception(String.Format("Registro do tipo '{0}' não reconhecido para o layout de retorno de Registro de Boletos HSBC", this.DescricaoConteudo[0].ToString()));
                        break;
                    case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoHSBC:
                        if (this.DescricaoConteudo[0] == 'A')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header);
                        else if (this.DescricaoConteudo[0] == 'Z')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Footer);
                        else if (this.DescricaoConteudo[0] == 'E')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC);
                        else
                            throw new Exception(String.Format("Registro do tipo '{0}' não reconhecido para o layout de remessa de Registro de Débito HSBC", this.DescricaoConteudo[0].ToString()));
                        break;
                    case BNE.BLL.Enumeradores.TipoArquivo.RetornoDebitoHSBC:
                        if (this.DescricaoConteudo[0] == 'A')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header);
                        else if (this.DescricaoConteudo[0] == 'Z')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Footer);
                        else if (this.DescricaoConteudo[0] == 'F')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC);
                        else if (this.DescricaoConteudo[0] == 'J')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC);
                        else
                            throw new Exception(String.Format("Registro do tipo '{0}' não reconhecido para o layout de retorno de Débito HSBC", this.DescricaoConteudo[0].ToString()));
                        break;
                    case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoBB:
                        if (this.DescricaoConteudo[0] == 'A')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Header);
                        else if (this.DescricaoConteudo[0] == 'Z')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.Footer);
                        else if (this.DescricaoConteudo[0] == 'E')
                            this.TipoLinhaArquivo = new TipoLinhaArquivo((int)Enumeradores.TipoLinhaArquivo.RemessaRegistroDebitoBB);

                        else
                            throw new Exception(String.Format("Registro do tipo '{0}' não reconhecido para o layout de retorno de Débito Banco do Brasil", this.DescricaoConteudo[0].ToString()));
                        break;

                    default:
                        throw new Exception("Tipo do arquivo ainda não definido");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível determinar o tipo do registro", ex);
            }
        }

        /// <summary>
        /// Carrega o objeto Transacao/BoletoBancario relativo à linha.
        /// </summary>
        public void CarregarTransacoes()
        {
            switch ((Enumeradores.TipoLinhaArquivo)this.TipoLinhaArquivo.IdTipoLinhaArquivo)
            {
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Header:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Footer:
                    return;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.BoletoCNR:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC:
                    String CodigoDoDocumento = this.DescricaoConteudo.Substring(37, 13).TrimStart(new Char[] { '0' });
                    BoletoBancario objBoletoBancario;
                    if (BoletoBancario.CarregarPeloNossoNumero(CodigoDoDocumento, out objBoletoBancario))
                    {
                        this.BoletoBancario = objBoletoBancario;
                    }
                    else
                    {
                        this.DescricaoMensagemLiberacao = String.Format("Boleto {0} não encontrado", CodigoDoDocumento);
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC:
                    string sTransacao = this.DescricaoConteudo.Substring(2, 21);
                    try
                    {
                        Int32 idTransacao = Convert.ToInt32(sTransacao);
                        this.Transacao = Transacao.LoadObject(idTransacao);
                    }
                    catch (RecordNotFoundException)
                    {
                        this.DescricaoMensagemLiberacao = String.Format("Transacao {0} não encontrada", sTransacao);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex, "Não foi possível reconhecer a transação: " + sTransacao);
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC:
                    //Não há linha. É o Retorno do processamento do arquivo.
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Verifica se alinha se trata de uma remessa de registro
        /// </summary>
        /// <returns>True se a linha se tratar de uma remessa de registro.</returns>
        public bool RemessaRegistro()
        {
            String s;
            switch ((Enumeradores.TipoLinhaArquivo)this.TipoLinhaArquivo.IdTipoLinhaArquivo)
            {
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC:
                    s = this.DescricaoConteudo.Substring(149, 1);
                    if (s == "0") //Código de Movimento de inclusão
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC:
                    s = this.DescricaoConteudo.Substring(108, 2);
                    if (s == "01") //Código de Ocorrência de inclusão
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Header:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Footer:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.BoletoCNR:
                default:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Verifica se alinha se trata de uma remessa de cancelamento de registro
        /// </summary>
        /// <returns>True se a linha se tratar de uma remessa de cancelamento de registro.</returns>
        public bool RemessaCancelamentoRegistro()
        {
            String s;
            switch ((Enumeradores.TipoLinhaArquivo)this.TipoLinhaArquivo.IdTipoLinhaArquivo)
            {
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC:
                    s = this.DescricaoConteudo.Substring(149, 1);
                    if (s == "1") //Código de Movimento de inclusão
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC:
                    s = this.DescricaoConteudo.Substring(108, 2);
                    if (s == "02") //Código de Ocorrência de inclusão
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Header:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Footer:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.BoletoCNR:
                default:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Verifica se alinha se trata de uma confirmação de pagamento
        /// </summary>
        /// <returns>True se a linha se tratar de uma confirmação de pagamento.</returns>
        public bool ConfirmacaoPagamento()
        {
            String s;
            List<String> CodigosDeLiquidacao;
            switch ((Enumeradores.TipoLinhaArquivo)this.TipoLinhaArquivo.IdTipoLinhaArquivo)
            {
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Header:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Footer:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC:
                    return false;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.BoletoCNR:
                    s = this.DescricaoConteudo.Substring(108, 2);
                    if (s == "06") //Retorno de liquidação
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC:
                    s = this.DescricaoConteudo.Substring(67, 2);
                    CodigosDeLiquidacao = new List<string> { "00", "31" };
                    if (CodigosDeLiquidacao.Contains(s)) //Retorno de liquidação
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC:
                    s = this.DescricaoConteudo.Substring(108, 2);
                    CodigosDeLiquidacao = new List<string> { "06", "07", "15", "16", "31", "32", "33", "36", "38", "39" };
                    if (CodigosDeLiquidacao.Contains(s)) //Retorno de liquidação
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Verifica se alinha se trata de uma confirmação de registro de pagamento
        /// </summary>
        /// <returns>True se a linha se tratar de uma confirmação de registro.</returns>
        public bool ConfirmacaoRegistro()
        {
            String s;
            switch ((Enumeradores.TipoLinhaArquivo)this.TipoLinhaArquivo.IdTipoLinhaArquivo)
            {
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Header:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Footer:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.BoletoCNR:
                    return false;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC:
                    s = this.DescricaoConteudo.Substring(67, 2);
                    if (s == "55") //Compromisso Agendado
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC:
                    s = this.DescricaoConteudo.Substring(108, 2);
                    if (s == "02") //Entrada Confirmada
                    {
                        return true;
                    }
                    break;
                default:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Verifica se alinha se trata de uma confirmação de cancelamento
        /// </summary>
        /// <returns>True se a linha se tratar de uma confirmação de cancelamento.</returns>
        public bool ConfirmacaoCancelamento()
        {
            String s;
            List<String> CodigosDeLiquidacao;
            switch ((Enumeradores.TipoLinhaArquivo)this.TipoLinhaArquivo.IdTipoLinhaArquivo)
            {
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC:
                    s = this.DescricaoConteudo.Substring(67, 2);
                    if (s == "99") //Cancelamento efetuado conforme solicitação
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC:
                    s = this.DescricaoConteudo.Substring(108, 2);
                    CodigosDeLiquidacao = new List<string> { "09", "10", "37" };
                    if (CodigosDeLiquidacao.Contains(s)) //Baixa de título
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Header:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Footer:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.BoletoCNR:
                default:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Verifica se alinha se trata de uma indicação de falha no pagamento
        /// </summary>
        /// <returns>True se a linha se tratar de uma confirmação de registro.</returns>
        public bool Falha(out String erro)
        {
            String s;
            Dictionary<String, String> CodigosDeFalha;
            erro = String.Empty;
            switch ((Enumeradores.TipoLinhaArquivo)this.TipoLinhaArquivo.IdTipoLinhaArquivo)
            {
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Header:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Footer:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.BoletoCNR:
                    return false;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC:
                    s = this.DescricaoConteudo.Substring(67, 2);
                    CodigosDeFalha = new Dictionary<string, string>();
                    CodigosDeFalha.Add("01", "Débito não efetuado - insuficiência de saldo. ");
                    CodigosDeFalha.Add("02", "Débito não efetuado - Conta Corrente não cadastrada. ");
                    CodigosDeFalha.Add("04", "Débito não efetuado - Outras restrições. ");
                    CodigosDeFalha.Add("05", "Débito não efetuado - Valor do débito excede o limite aprovado. ");
                    CodigosDeFalha.Add("10", "Débito não efetuado - Agência em regime de encerramento. ");
                    CodigosDeFalha.Add("12", "Débito não efetuado - Valor inválido. ");
                    CodigosDeFalha.Add("13", "Débito não efetuado - Data do lançamento inválida. ");
                    CodigosDeFalha.Add("14", "Débito não efetuado - Agência inválida. ");
                    CodigosDeFalha.Add("15", "Débito não efetuado - Dígito verificador da conta corrente inválido. ");
                    CodigosDeFalha.Add("18", "Débito não efetuado - Data do débito anterior ao processamento. ");
                    CodigosDeFalha.Add("50", "Débito não efetuado - Dados para o cálculo do IOF Inválidos. ");
                    CodigosDeFalha.Add("60", "Débito não efetuado - CPF/CNPJ do devedor divergente. ");
                    CodigosDeFalha.Add("97", "Cancelamento não efetuado - Compromisso não encontrado. ");
                    CodigosDeFalha.Add("99", "Cancelamento não efetuado - Fora do tempo hábil. ");
                    if (CodigosDeFalha.ContainsKey(s)) //Falha
                    {
                        erro = CodigosDeFalha[s];
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC:
                    s = this.DescricaoConteudo.Substring(108, 2);
                    if (s == "03") //Entrada Rejeitada
                    {
                        String complementoOcorrencia = this.DescricaoConteudo.Substring(301, 2);
                        erro = "Entrada Registrada - Codigo da ocorrência(verificar documentacao HSBC): " + complementoOcorrencia;
                        return true;
                    }
                    break;
                default:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Verifica se alinha se trata de uma inclusao de pagamento
        /// </summary>
        /// <returns>True se a linha se tratar de uma inclusao de pagamento.</returns>
        public bool InclusaoPagamento()
        {
            String s;
            switch ((Enumeradores.TipoLinhaArquivo)this.TipoLinhaArquivo.IdTipoLinhaArquivo)
            {
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC:
                    s = this.DescricaoConteudo.Substring(149, 1);
                    if (s == "0") //Inclusao de débito
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC:
                    s = this.DescricaoConteudo.Substring(108, 2);
                    if (s == "01") //Registro de Remessa
                    {
                        return true;
                    }
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Header:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Footer:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.BoletoCNR:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC:
                default:
                    return false;
            }

            return false;
        }

        /// <summary>
        /// Recupera a data do pagamento indicado na linha.
        /// </summary>
        /// <returns>Date time com a data indicada.</returns>
        /// <exception cref="Exception">Retorna exceção caso o tipo da linha não indique uma data de pagamento(Header e Footer, p.e.)</exception>
        public DateTime RecuperarDataPagamento()
        {
            DateTime retorno = DateTime.Now;
            String sData;
            Regex formato;
            Match m;

            switch ((Enumeradores.TipoLinhaArquivo)this.TipoLinhaArquivo.IdTipoLinhaArquivo)
            {
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.BoletoCNR:
                    sData = this.DescricaoConteudo.Substring(82, 6);
                    formato = new Regex(@"^(?<dia>\d{2})(?<mes>\d{2})(?<ano>\d{2})$"); //formato no arquivo - DDMMAA
                    m = formato.Match(sData);
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoDebitoHSBC:
                    sData = this.DescricaoConteudo.Substring(44, 8);
                    formato = new Regex(@"^(?<ano>\d{4})(?<mes>\d{2})(?<dia>\d{2})$"); //formato no arquivo - AAAAMMDD
                    m = formato.Match(sData);
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.ProcessamentoDebitoHSBC:
                    sData = this.DescricaoConteudo.Substring(38, 8);
                    formato = new Regex(@"^(?<ano>\d{4})(?<mes>\d{2})(?<dia>\d{2})$"); //formato no arquivo - AAAAMMDD
                    m = formato.Match(sData);
                    break;
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Header:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.Footer:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RegistroDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RemessaRegistroBoletoHSBC:
                case BNE.BLL.Enumeradores.TipoLinhaArquivo.RetornoRegistroBoletoHSBC:
                default:
                    throw new Exception("Tipo da linha não indica um pagamento");
            }

            int ano = Convert.ToInt32(m.Groups["ano"].Value);
            if (ano <= 2000)
            {
                ano += 2000;
            }

            return new DateTime(Convert.ToInt32(ano), Convert.ToInt32(m.Groups["mes"].Value), Convert.ToInt32(m.Groups["dia"].Value));
        }

        #endregion Recepção de Arquivos

        #endregion Metodos
    }
}