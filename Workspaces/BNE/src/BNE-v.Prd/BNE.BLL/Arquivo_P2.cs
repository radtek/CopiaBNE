//-- Data: 22/05/2014 11:00
//-- Autor: Francisco Ribas

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Linq;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.EL;
namespace BNE.BLL
{
    public partial class Arquivo // Tabela: BNE_Arquivo
    {
        #region Atributos
        private List<LinhaArquivo> _linhas = new List<LinhaArquivo>();
        #endregion

        #region Propriedades
        public List<LinhaArquivo> Linhas
        {
            get { return _linhas; }
            set { _linhas = value; }
        }
        #endregion

        #region Consultas
        private const string SP_PROXIMO_ID = @"SELECT MAX(idf_Arquivo) + 1 FROM BNE.BNE_Arquivo WITH(NOLOCK)";
        private const string SP_COUNT_TIPO_ARQUIVO = @"SELECT COUNT(*) FROM BNE.BNE_Arquivo WHERE Idf_Tipo_Arquivo = @Idf_Tipo_Arquivo";
        #endregion Consultas

        #region Metodos

        #region GetBytes

        /// <summary>
        /// Converter o conte�do do arquivo em um array de bytes.
        /// </summary>
        /// <returns>Array de bytes com o conte�do do arquivo</returns>
        public byte[] GetBytes()
        {
            String sConteudo = this.GetString();
            return Encoding.Default.GetBytes(sConteudo);
        }
        #endregion GetBytes

        #region GetString

        /// <summary>
        /// Transforma o conte�do do arquivo em uma string.
        /// </summary>
        /// <returns>String com o conte�do do arquivo.</returns>
        public String GetString()
        {
            StringBuilder sb = new StringBuilder();
            this.Linhas.ForEach(l => sb.AppendLine(l.DescricaoConteudo));
            return sb.ToString();
        }

        #endregion GetString

        #region Gera��o de Arquivos
        /// <summary>
        /// Gera os arquivos a serem enviados aos bancos para registro de boletos e d�bito autom�tico.
        /// </summary>
        /// <param name="tipoArquivo">Tipo do arquivo a ser gerado</param>
        /// <returns>Instancia do tipo Arquivo com as linhas j� geradas para o arquivo informado.</returns>
        /// <exception cref="Exception">Lan�a uma exce��o caso a gera��o tipo do arquivo passado n�o seja de responsabilidade do BNE(pe, arquivos de retorno do banco)</exception>
        public static Arquivo GerarArquivo(Enumeradores.TipoArquivo tipoArquivo)
        {
            List<object> lstInclusao = new List<object>();
            List<object> lstCancelamento = new List<object>();

            switch (tipoArquivo)
            {
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoHSBC://REMESSA - Debito autom�tico HSBC
                    Transacao.CarregarTransacoesDebitoRecorrenteNaoRemetidas((int)Enumeradores.Banco.HSBC).ForEach(b => lstInclusao.Add(b));
                    Transacao.CarregarTransacoesDebitoRecorrenteACancelar((int)Enumeradores.Banco.HSBC).ForEach(b => lstCancelamento.Add(b));
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaDebitoBB://REMESSA - Debito autom�tico BB
                    Transacao.CarregarTransacoesDebitoRecorrenteNaoRemetidas((int)Enumeradores.Banco.BANCODOBRASIL).ForEach(b => lstInclusao.Add(b));
                    Transacao.CarregarTransacoesDebitoRecorrenteACancelar((int)Enumeradores.Banco.BANCODOBRASIL).ForEach(b => lstCancelamento.Add(b));
                    break;
                case BNE.BLL.Enumeradores.TipoArquivo.RemessaRegistroBoletos: //REMESSA - Boletos registrados
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoRegistroBoletos:
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoDebitoHSBC:
                case BNE.BLL.Enumeradores.TipoArquivo.RetornoCNR:
                default:
                    throw new Exception(String.Format("Tipo de arquivo {0} n�o pode ser gerado pelo sistema. � um arquivo de recebimento.", tipoArquivo.ToString()));
            }

            if (lstInclusao.Count <= 0 && lstCancelamento.Count <= 0)
            {
                return null;
            }

            return GerarArquivo(lstInclusao, lstCancelamento, tipoArquivo);
        }

        /// <summary>
        /// Gera o arquivo do tipo informado com os registros de inclus�o e cancelamento informados.
        /// </summary>
        /// <param name="lstInclusao">Lista de objetos do tipo BoletoBancario ou Transacao para gerar os registros de inclusao</param>
        /// <param name="lstCancelamento">Lista de objetos do tipo BoletoBancario ou Transacao para gerar os registros de cancelamento</param>
        /// <returns></returns>
        private static Arquivo GerarArquivo(List<object> lstInclusao, List<object> lstCancelamento, Enumeradores.TipoArquivo tipoArquivo)
        {
            Arquivo objArquivo = new Arquivo();

            #region Gerando Registro Arquivo
            objArquivo.NomeArquivo = String.Format("{0}.txt", RecuperarProximoId().ToString("D8"));
            objArquivo.TipoArquivo = new TipoArquivo((int)tipoArquivo);
            #endregion Gerando Registro Arquivo

            try
            {
                //Gerando Primeira Linha
                LinhaArquivo.GerarHeader(objArquivo);

                foreach (object objBoletoBancario in lstInclusao)
                {
                    LinhaArquivo.GerarRegistroRemessa(objArquivo, objBoletoBancario);
                }

                foreach (object objBoletoBancario in lstCancelamento)
                {
                    LinhaArquivo.GerarRegistroRemessa(objArquivo, objBoletoBancario, false);
                }

                //Gerando �ltima linha
                LinhaArquivo.GerarFooter(objArquivo);

                //Gravando arquivo no banco
                objArquivo.PersistirArquivo();

                return objArquivo;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Recupera o pr�ximo ID da tabela BNE_Arquivo. Utilizado para determinar o nome do arquivo no processo de gera��o.
        /// </summary>
        /// <returns>Int com o pr�ximo ID.</returns>
        private static int RecuperarProximoId()
        {
            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SP_PROXIMO_ID, null));
        }

        /// <summary>
        /// Recupera o n�mero arquivos gerados por tipo.
        /// </summary>
        /// <param name="tipoArquivo">Tipo do arquivo a ser considerado na contagem</param>
        /// <returns>Int com o n�mero de arquivos gerados.</returns>
        public static int RecuperarNumeroDeArquivos(Enumeradores.TipoArquivo tipoArquivo)
        {
            List<SqlParameter> param = new List<SqlParameter>();
            param.Add(new SqlParameter("@Idf_Tipo_Arquivo", SqlDbType.Int, 4));
            param[0].Value = (int)tipoArquivo;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SP_COUNT_TIPO_ARQUIVO, param));
        }

        #endregion Gera��o de Arquivos

        #region Recep��o de Arquivos

        /// <summary>
        /// Recebe um arquivo. Efetua a leitura, interpreta, persiste no banco e libera os planos das linhas identificadas como paga.
        /// </summary>
        /// <param name="sArquivo">Stream com o arquivo a ser recebido</param>
        /// <param name="fileName">Nome do arquivo.</param>
        /// <param name="idUsuarioLogado">Identificador do usu�rio logado para gera��o de NF.</param>
        /// <returns>Inst�ncia da classe Arquivo com o arquivo interpretado.</returns>
        public static Arquivo ReceberArquivo(Stream sArquivo, String fileName, int idUsuarioLogado)
        {
            try
            {
                Arquivo objArquivo = InterpretarArquivo(sArquivo);
                objArquivo.NomeArquivo = fileName;
                objArquivo.PersistirArquivo();

                objArquivo.ConsistirRetorno(idUsuarioLogado);

                return objArquivo;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao receber o arquivo");
                throw (ex);
            }
        }

        /// <summary>
        /// Efetua a leitura do arquivo de retorno do banco
        /// </summary>
        /// <param name="sArquivo">Stream do arquivo a ser importado</param>
        /// <returns>Inst�ncia da classe arquivo, representando o arquivo lido</returns>
        public static Arquivo InterpretarArquivo(Stream sArquivo)
        {
            try
            {
                Arquivo objArquivo = new Arquivo();
                StreamReader sr = new StreamReader(sArquivo);
                string linhaAtual;

                #region Interpretando Header
                if ((linhaAtual = sr.ReadLine()) == null)
                    throw new Exception("N�o foi possivel ler o header. Verifique se o ponteiro da stream est� no in�cio do arquivo.");

                objArquivo.ReconhecerTipoArquivo(linhaAtual);
                objArquivo.AdicionarLinha(linhaAtual);
                #endregion

                while ((linhaAtual = sr.ReadLine()) != null)
                {
                    objArquivo.AdicionarLinha(linhaAtual);
                }

                return objArquivo;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao interpretar o arquivo");
                throw (ex);
            }
        }

        /// <summary>
        /// Valida��o de linhas antes serem exibidas
        /// </summary>
        /// <returns></returns>
        private bool isLinhaValida(string linhaAtual)
        {
            string[] reject = { "01", "02", "04", "05", "10", "12", "13", "14", "15", "18", "50", "60" };
            //Verifica se existe alguma excess�o na linha para n�o listar
            if (string.IsNullOrEmpty(linhaAtual)) return false;
            else if (reject.Contains(linhaAtual.Substring(67, 2))) return false;
            else return true;
        }

        /// <summary>
        /// Salva o objeto arquivo e as linhas na propriedade Linhas.
        /// </summary>
        public void PersistirArquivo()
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.Save(trans);

                        foreach (LinhaArquivo linha in this.Linhas)
                        {
                            linha.Save(trans);
                            //Se a linha contiver uma transa��o e essa for uma linha de inclus�o de pagamento
                            //marca a transa��o como Enviada
                            if (linha.Transacao != null && linha.InclusaoPagamento())
                            {
                                linha.Transacao.AtualizarStatus(Enumeradores.StatusTransacao.Enviada, trans);
                            }
                        }

                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Reconhece o tipo do arquivo atrav�s do header presente na lista de linhas.
        /// </summary>
        /// <exception cref="Exception">Retorna exce��o se o tipo n�o for reconhecido</exception>
        private void ReconhecerTipoArquivo(String header)
        {
            String s;

            #region Reconhecimento de arquivos de boleto (CNAB 400)
            if (header[0] == '0')
            {
                s = header.Substring(11, 15).Trim();
                switch (s)
                {
                    case "COBRANCA CNR":
                        this.TipoArquivo = new BLL.TipoArquivo((int)Enumeradores.TipoArquivo.RetornoCNR);
                        return;
                    case "COBRANCA":
                        this.TipoArquivo = new BLL.TipoArquivo((int)Enumeradores.TipoArquivo.RetornoRegistroBoletos);
                        return;
                    default:
                        break;
                }
            }

            #endregion Reconhecimento de arquivos de boleto (CNAB 400)

            #region Reconhecimento do layout de recebimento autom�tico HSBC (150 posi��es)
            if (header[0] == 'A')
            {
                s = header.Substring(81, 17).Trim();
                switch (s)
                {
                    case "DEBITO AUTOMATICO":
                        if (header[1] == '1')
                        {
                            this.TipoArquivo = new BLL.TipoArquivo((int)Enumeradores.TipoArquivo.RemessaDebitoHSBC);
                            return;
                        }
                        this.TipoArquivo = new BLL.TipoArquivo((int)Enumeradores.TipoArquivo.RetornoDebitoHSBC);
                        return;
                    default:
                        break;
                }
            }
            #endregion Reconhecimento do layout de recebimento autom�tico HSBC (150 posi��es)

            throw new Exception("Tipo arquivo n�o identificado.");
        }

        /// <summary>
        /// Adiciona uma linha, efetuando as interpreta��es e carregando os objetos vinculados a essa linha
        /// </summary>
        /// <param name="linha">String com a linha a ser interpretada</param>
        /// <returns>Inst�ncia da classe LinhaArquivo que representa a linha interpretada</returns>
        private LinhaArquivo AdicionarLinha(String linha)
        {
            LinhaArquivo objLinhaArquivo = new LinhaArquivo();
            objLinhaArquivo.Arquivo = this;
            //Ao definir a descricao do conteudo, o tipo da linha j� � reconhecida e o objeto Transacao/BoletoBancario j� � carregado.
            objLinhaArquivo.DescricaoConteudo = linha;

            if (this.Linhas == null)
            {
                this.Linhas = new List<LinhaArquivo>();
            }
            objLinhaArquivo.NumeroLinha = this.Linhas.Count + 1;
            this.Linhas.Add(objLinhaArquivo);

            return objLinhaArquivo;
        }

        /// <summary>
        /// Verifica linha a linha do arquivo e libera os pagamentos confirmados.
        /// </summary>
            String erro;
        private void ConsistirRetorno(int idUsuarioLogado)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();              
                int count = this.Linhas.Count;

                foreach (LinhaArquivo linha in this.Linhas)
                {
                    //Verificando se na linha existe algo a ser consistido
                    if (linha.BoletoBancario == null && linha.Transacao == null)
                    {
                        if (this.Linhas.Count == count || count == 1)//HEADER E FOOTER IGNORE 
                        {
                            count--;
                            continue;
                        }

                        try
                        {
                            string numeroNossoNumero = linha.DescricaoConteudo.Substring(37, 13).TrimStart(new Char[] { '0' });

                            if (string.IsNullOrEmpty(numeroNossoNumero)) continue;

                            using (var objSine = new wsSine.AppClient())
                            {
                                UsuarioFilialPerfil objUsuario = UsuarioFilialPerfil.LoadObject(idUsuarioLogado);
                                objUsuario.PessoaFisica.CompleteObject();
                                objSine.LiberarDestaqueVaga(numeroNossoNumero, objUsuario.PessoaFisica.NomeCompleto, objUsuario.PessoaFisica.CPF.ToString(), DateTime.Now);
                            }
                        }
                        catch (Exception ex)
                        {
                            EL.GerenciadorException.GravarExcecao(ex);
                        }
                        count--;
                        continue;
                    }

                    using (SqlTransaction trans = conn.BeginTransaction())
                    {
                        try
                        {
                            //Linhas relativas a boletos
                            if (linha.BoletoBancario != null)
                            {
                                //Verifica se a linha � uma confirma��o de cancelamento
                                if (linha.ConfirmacaoCancelamento())
                                {
                                    linha.DescricaoMensagemLiberacao = "Cancelamento de registro Realizado com Sucesso";
                                    linha.Save(trans);
                                }
                                else
                                { 
                                    //Verifica se a linha � uma confirma��o de pagamento
                                    if (linha.ConfirmacaoPagamento())
                                    {
                                        decimal vlrPagamento = decimal.Parse(linha.DescricaoConteudo.Substring(152, 13).TrimStart(new Char[] { '0' })) / 100;
                                        linha.BoletoBancario.Liquidar(trans, DateTime.Now, idUsuarioLogado, vlrPagamento);


                                        if (linha.BoletoBancario.Pagamento.PlanoParcela != null)
                                        {
                                           var parcelaFinal =
                                           PlanoParcela.CarregarUltimaParcelaPorPlanoAdquirido(
                                               linha.BoletoBancario.Pagamento.PlanoParcela.PlanoAdquirido
                                                   .IdPlanoAdquirido, trans);


                                            if (parcelaFinal != null && linha.BoletoBancario.Pagamento.PlanoParcela.PlanoAdquirido.Plano != null
                                                && parcelaFinal.PlanoParcelaSituacao.IdPlanoParcelaSituacao == (int)BLL.Enumeradores.PlanoParcelaSituacao.Pago
                                                && linha.BoletoBancario.Pagamento.PlanoParcela.PlanoAdquirido.Plano.FlagRecorrente
                                                && linha.BoletoBancario.Pagamento.PlanoParcela.PlanoAdquirido.Plano.FlagBoletoRecorrente
                                                )
                                            {
                                                GerarNovoPagamento(linha, trans);
                                            }

                                            linha.DescricaoMensagemLiberacao = "Pagamento Realizado com Sucesso";
                                            linha.Save(trans);
                                        }
                                       
                                    }
                                    else
                                    { 
                                        //Verifica se a linha � uma confirma��o de registro
                                        if (linha.ConfirmacaoRegistro())
                                        {
                                            linha.DescricaoMensagemLiberacao = "Registro Efetuado";
                                            linha.Save(trans);
                                        }
                                        else
                                        {
                                            //Verifica se a linha � uma indica��o de falha
                                            if (linha.Falha(out erro))
                                            {
                                                linha.DescricaoMensagemLiberacao = "Falha: " + erro;
                                                linha.Save(trans);
                                            }
                                        }
                                            
                                    }
                                }
                            }
                            //Linhas relativas a transa��es (D�bito)
                            if (linha.Transacao != null)
                            {
                                //Verifica se a linha � uma confirma��o de cancelamento
                                if (linha.ConfirmacaoCancelamento())
                                {
                                    linha.Transacao.AtualizarStatus(Enumeradores.StatusTransacao.Cancelada, trans);
                                    PlanoAdquirido.DerrubarPlanoLiberadoPagamentoEmAberto(linha.Transacao, trans);
                                    linha.DescricaoMensagemLiberacao = "Cancelamento de registro Realizado com Sucesso";
                                    linha.Save(trans);
                                }
                                else
                                {
                                    //Verifica se a linha � uma confirma��o de pagamento
                                    if (linha.ConfirmacaoPagamento())
                                    {
                                        linha.Transacao.Liquidar(trans, linha.RecuperarDataPagamento(), idUsuarioLogado);
                                        linha.DescricaoMensagemLiberacao = "D�bito Realizado com Sucesso";
                                        linha.Save(trans);

                                        linha.Transacao.CompleteObject(trans);
                                    }
                                    else
                                    {
                                        //Verifica se a linha � uma confirma��o de registro
                                        if (linha.ConfirmacaoRegistro())
                                        {
                                            linha.Transacao.AtualizarStatus(Enumeradores.StatusTransacao.Registrada, trans);
                                            linha.DescricaoMensagemLiberacao = "D�bito agendado";
                                            linha.Save(trans);
                                        }
                                        else
                                        { 
                                            //Verifica se a linha � uma indica��o de falha
                                            if (linha.Falha(out erro))
                                            {
                                                LinhaArquivo objUltimaLinhaRemetida;
                                                if (LinhaArquivo.CarregarUltimaLinhaRemetidaDeTransacao(linha.Transacao.IdTransacao, out objUltimaLinhaRemetida))
                                                {
                                                    //Verifica se a �ltima linha enviada se trata de uma inclusao de registro
                                                    if (objUltimaLinhaRemetida.RemessaRegistro())
                                                    {
                                                        linha.Transacao.AtualizarStatus(Enumeradores.StatusTransacao.Rejeitada, trans);
                                                    }
                                                }
                                                PlanoAdquirido.DerrubarPlanoLiberadoPagamentoEmAberto(linha.Transacao, trans);

                                                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.RecuperaPlanoAdquiridoPelaTransacao(linha.Transacao);

                                                objPlanoAdquirido.Plano.CompleteObject();
                                                objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                                                objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject();

                                                EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar("Cancelamento do Plano ",
                                                    objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomePessoa + ", ocorreu o seguinte problema com o seu pagamento junto ao Banco HSBC: " + erro + "\r\n Favor entrar em contato no 0800-41-2400",null,
                                                    Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail),
                                                    objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.EmailPessoa);
                                                
                                                //(string to, string from, string subject, string message, Dictionary<string, byte[]> attachments, SaidaSMTP saidaSMTP = SaidaSMTP.Mandrill, List<string> tags = null )
                                                if (objPlanoAdquirido.ParaPessoaFisica())
                                                {
                                                    EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar("Cancelamento do Plano D�bito HSBC", "Segue os dados do cliente: \n Nome: " + objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomeCompleto + "\n CPF: " + objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroCPF + "\n Tel: (" + objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroDDDTelefone + ")" + objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NumeroTelefone + "\n Plano Solicitado: " + objPlanoAdquirido.Plano.DescricaoPlano,null, Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail), "financeiro@bne.com.br");
                                                }
                                                else
                                                {
                                                    objPlanoAdquirido.Filial.CompleteObject();
                                                    EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar("Cancelamento do Plano D�bito HSBC", "Segue os dados do cliente: \n Empresa: " + objPlanoAdquirido.Filial.NomeFantasia + "\n CNPJ: " + objPlanoAdquirido.Filial.NumeroCNPJ + "\n Tel: (" + objPlanoAdquirido.Filial.NumeroDDDComercial + ")" + objPlanoAdquirido.Filial.NumeroComercial + "\n Plano Solicitado: " + objPlanoAdquirido.Plano.DescricaoPlano,null, Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContaPadraoEnvioEmail), string.IsNullOrEmpty(objPlanoAdquirido.Filial.Vendedor().EmailVendedor) ? "andrianogoncalves@bne.com.br" : objPlanoAdquirido.Filial.Vendedor().EmailVendedor);
                                                }

                                                linha.DescricaoMensagemLiberacao = "Falha: " + erro;
                                                linha.Save(trans);
                                            }
                                        }
                                    }
                                }
                            }
                            count--;
                            trans.Commit();
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            String guid = EL.GerenciadorException.GravarExcecao(ex);
                            linha.DescricaoMensagemLiberacao = String.Format("Ocorreu um erro na liquida��o do boleto: [{0}]", guid);
                        }
                    }
                }
            }
        }

        #endregion Recep��o de Arquivos
        /// <summary>
        /// Gera novo pagamento caso boleto seja recorrente e esteja na ultima parcela
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="trans"></param>
        public void GerarNovoPagamento(LinhaArquivo linha, SqlTransaction trans)
        {
            try
            {
                //criar uma nova parcela
                var objPlanoParcela = PlanoParcela.CriarParcelaRecorrenciaPeloPlanoAdquirido(linha.BoletoBancario.Pagamento.PlanoParcela.PlanoAdquirido,null, trans);
                //criar um novo pagamento
                DateTime dataVencimento = linha.BoletoBancario.Pagamento.DataVencimento.HasValue ? linha.BoletoBancario.Pagamento.DataVencimento.Value.AddMonths(1) : DateTime.Today.AddDays(5);

                DateTime dataVenciamentoValida = dataVencimento.AddDays(Feriado.RetornarDiaUtilVencimento(dataVencimento));
                var pagamento = Pagamento.CriarPagamentoBoletoRecorrencia(objPlanoParcela,
                   linha.BoletoBancario.Pagamento.PlanoParcela.PlanoAdquirido,
                   dataVenciamentoValida, linha.BoletoBancario.Pagamento.PlanoParcela.ValorParcela,
                   trans);
                //criar Boleto para atualizar a tabela de pagamento
                PagarMeOperacoes.GerarBoleto(pagamento);
            }
            catch (Exception ex)
            {
                String guid = EL.GerenciadorException.GravarExcecao(ex);
                linha.DescricaoMensagemLiberacao = String.Format("Ocorreu um erro na geracao do boleto: [{0}]", guid);
            }
           
        }

        #endregion Metodos
    }
}