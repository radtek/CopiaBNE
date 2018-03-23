//-- Data: 25/04/2014 15:33
//-- Autor: Francisco Ribas

using BNE.BLL.Custom;
using BNE.BLL.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;

namespace BNE.BLL
{
    public partial class CobrancaBoleto // Tabela: GLO_Cobranca_Boleto
    {

        #region Propriedades
        //Propriedades utilizadas na liberacao do boleto
        public Pagamento Pagamento { get; set; }
        #endregion

        #region Consultas
        private const string SP_BOLETOS_PARA_REGISTAR = @"SELECT cb.* FROM BNE.GLO_Cobranca_Boleto cb
                                                            JOIN BNE.BNE_Pagamento pag ON pag.Des_Identificador = cb.Num_Nosso_Numero
                                                            OUTER APPLY(SELECT	COUNT(Idf_Linha_Arquivo) AS num_registros_remessa 
					                                                            FROM BNE.BNE_Linha_Arquivo la 
					                                                            WHERE	la.Idf_Cobranca_Boleto = cb.Idf_Cobranca_Boleto 
							                                                            AND la.Idf_Tipo_Linha_Arquivo = 6 --Tipo RemessaRegistroBoletoHSBC
							                                                            AND SUBSTRING(Des_Conteudo,109,2) = '01' --Código de Ocorrência = Remessa
			                                                            ) AS remessa
                                                            WHERE 
	                                                            cb.Flg_Registra_Boleto = 1 --Marcado para registro
	                                                            AND pag.Idf_Pagamento_Situacao = 1 --Em aberto
	                                                            AND remessa.num_registros_remessa <= 0 --Sem registro de remessa";

        private const string SP_BOLETOS_PARA_CANCELAR = @"SELECT cb.* FROM BNE.GLO_Cobranca_Boleto cb
                                                            JOIN BNE.BNE_Pagamento pag ON pag.Des_Identificador = cb.Num_Nosso_Numero
                                                            OUTER APPLY(SELECT	COUNT(Idf_Linha_Arquivo) AS num_registros_remessa 
					                                                            FROM BNE.BNE_Linha_Arquivo la 
					                                                            WHERE	la.Idf_Cobranca_Boleto = cb.Idf_Cobranca_Boleto 
							                                                            AND la.Idf_Tipo_Linha_Arquivo = 6 --Tipo RemessaRegistroBoletoHSBC
							                                                            AND SUBSTRING(Des_Conteudo,109,2) = '01' --Código de Ocorrência = Remessa
			                                                            ) AS remessa
                                                            OUTER APPLY(SELECT	COUNT(Idf_Linha_Arquivo) AS num_registros_cancelamento 
					                                                            FROM BNE.BNE_Linha_Arquivo la 
					                                                            WHERE	la.Idf_Cobranca_Boleto = cb.Idf_Cobranca_Boleto 
							                                                            AND la.Idf_Tipo_Linha_Arquivo = 6 --Tipo RemessaRegistroBoletoHSBC
							                                                            AND SUBSTRING(Des_Conteudo,109,2) = '02' --Código de Ocorrência = Pedido de Baixa
			                                                            ) AS cancelamento
                                                            WHERE 
	                                                            cb.Flg_Registra_Boleto = 1 --Marcado para registro
	                                                            AND pag.Idf_Pagamento_Situacao = 3 --Cancelado
	                                                            AND remessa.num_registros_remessa >= 1 --Com registro de remessa
	                                                            AND cancelamento.num_registros_cancelamento <= 0 --Sem registro de cancelamento;";

        private const string SP_NOSSO_NUMERO = @"SELECT * FROM GLO_Cobranca_Boleto WHERE Num_Nosso_Numero = @NossoNumero";

        private const string SP_NOSSO_NUMERO_REGISTRADO_DE_PAGAMENTO_CANCELADO = @"SELECT TOP 1 cb.Num_Nosso_Numero FROM BNE.GLO_Cobranca_Boleto cb
	                                                                                JOIN BNE.BNE_Pagamento pag ON pag.Des_Identificador = cb.Num_Nosso_Numero
	                                                                                OUTER APPLY(SELECT	COUNT(Idf_Linha_Arquivo) AS num_registros_remessa 
						                                                                                FROM BNE.BNE_Linha_Arquivo la 
						                                                                                WHERE	la.Idf_Cobranca_Boleto = cb.Idf_Cobranca_Boleto 
								                                                                                AND la.Idf_Tipo_Linha_Arquivo = 6 --Tipo RemessaRegistroBoletoHSBC
								                                                                                AND SUBSTRING(Des_Conteudo,109,2) = '01' --Código de Ocorrência = Remessa
				                                                                                ) AS remessa
	                                                                                WHERE 
		                                                                                cb.Flg_Registra_Boleto = 1 --Marcado para registro
		                                                                                AND pag.Idf_Pagamento_Situacao = 3 --Cancelado
		                                                                                AND remessa.num_registros_remessa <= 0 --Sem registro de remessa";

        private const string SP_CARREGAR_ULTIMO_NOSSO_NUMERO_DE_REGISTRADO = @"SELECT MAX(CONVERT(DECIMAL, Num_Nosso_Numero)) 
                                                                                FROM BNE.GLO_Cobranca_Boleto cb
                                                                                WHERE cb.Flg_Registra_Boleto = 1
                                                                                AND ISNUMERIC(Num_Nosso_Numero) = 1";

        #endregion

        #region Metodos
        /// <summary>
        /// Carrega o boleto através do "Nosso Número" do Boleto
        /// </summary>
        /// <param name="nossoNumero">String com o Nosso Número a ser considerado na busca</param>
        /// <param name="objCobrancaBoleto">Variável onde o objeto será instanciado</param>
        /// <returns>True se um boleto foi encontrado.</returns>
        public static bool CarregarPeloNossoNumero(string nossoNumero, out CobrancaBoleto objCobrancaBoleto)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@NossoNumero", SqlDbType.VarChar, 20));
            parms[0].Value = nossoNumero;
            objCobrancaBoleto = new CobrancaBoleto();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_NOSSO_NUMERO, parms))
            {
                if (SetInstance(dr, objCobrancaBoleto))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Retorna o Nosso Número de um boleto cujo o pagamento tenha sido cancelado e que ainda não tenha submetido para registro. Utilizado para aproveitar todo o range.
        /// </summary>
        /// <returns>Nosso Número do boleto, se encontrado. Se nenum boleto não remetido com pagamento cancelado for encontrado, retorna null</returns>
        public static String CarregarNossoNumeroParaRegistradoDePagamentoCancelado()
        {
            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, SP_NOSSO_NUMERO_REGISTRADO_DE_PAGAMENTO_CANCELADO, null));
        }

        /// <summary>
        /// Retorna o Nosso Número de um boleto cujo o pagamento tenha sido cancelado e que ainda não tenha submetido para registro. Utilizado para aproveitar todo o range.
        /// </summary>
        /// <returns>Nosso Número do boleto, se encontrado. Se nenum boleto não remetido com pagamento cancelado for encontrado, retorna null</returns>
        public static String CarregarProximoNossoNumeroParaRegistrado()
        {
            //Tenta utilizar algum nosso número de um boleto registrado cujo o pagamento esteja cancelado e o registro ainda não tenha sido enviado.
            string nossoNumero = CarregarNossoNumeroParaRegistradoDePagamentoCancelado();
            if (!String.IsNullOrEmpty(nossoNumero))
            {
                return nossoNumero;
            }

            //Se não encontrou nenhum registro, busca o último utilizado    
            nossoNumero = Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, SP_CARREGAR_ULTIMO_NOSSO_NUMERO_DE_REGISTRADO, null));
            if (String.IsNullOrEmpty(nossoNumero))
            {
                //Se nenhum nosso número foi encontrado, retorna o número inicial do range informado pelo hsbc
                return AdicionaDigito("2219000000");
            }

            //Verificando se o range não foi excedido
            decimal iNossoNumero = Convert.ToDecimal(nossoNumero.Substring(0, nossoNumero.Length - 1)); //Convert para int, retirando o dígito verificador (último dígito à direita)
            if (iNossoNumero >= 2219099999)
            {
                //Range informado pelo HSBC foi excedido
                throw new Exception("O range de Nosso Número para registro de boletos foi excedido. Favor entrar em contato com o banco para obter outro range");
            }

            //Incrementa nosso número de 1 e adiciona o dígito verificador
            return AdicionaDigito((iNossoNumero + 1).ToString());
        }

        /// <summary>
        /// Calcula o dígito verificador do nosso número a ser enviado no boleto registrado
        /// </summary>
        /// <param name="nossoNumero">Nosso número para qual o digito deve ser calculado</param>
        /// <returns>Nosso número informado com o dígito verificador.</returns>
        private static String AdicionaDigito(String nossoNumero)
        {
            int cont = 5;
            int soma = 0;
            for (int i = 0; i < nossoNumero.Length; i++)
            {
                soma += (int)Char.GetNumericValue(nossoNumero[i]) * cont;
                if (--cont < 2)
                    cont = 7;
            }

            if (soma % 11 <= 1)
            {
                return nossoNumero + "0";
            }
            else
            {
                return nossoNumero + Convert.ToString(11 - (soma % 11));
            }
        }

        public string GerarRegistroRemessa(Int32 numeroLinha)
        {
            String linha = "";
            String temp = "";

            //Código do Registro
            linha += "1";
            //Código de Inscrição
            linha += "02"; //CNPJ
            //Número de Inscricao
            linha += Helper.NumericoParaTamanhoExato(this.NumeroCNPJCedente.Value, 14);
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
            linha += Helper.NumericoParaTamanhoExato(this.NumeroNossoNumero, 25);
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
            linha += Helper.NumericoParaTamanhoExato(1, 2); //Remessa
            //Seu Número
            linha += Helper.NumericoParaTamanhoExato(0, 10);
            //Vencimento
            linha += this.DataVencimento.Value.ToString("ddMMyy");
            //Valor do Titulo
            linha += Helper.MonetarioParaTamanhoExato(this.ValorBoleto.Value, 13);
            //Banco Cobrador
            linha += "399";
            //Agência Depositária
            linha += Helper.NumericoParaTamanhoExato(0, 5);
            //Espécia
            linha += Helper.NumericoParaTamanhoExato(10, 2);
            //Aceite
            linha += "A"; //Aceito
            //Data Emissão
            linha += this.DataEmissao.Value.ToString("ddMMyy");
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
            //Codigo de Inscricao
            if (this.NumeroCPFSacado.HasValue)
            {
                linha += Helper.NumericoParaTamanhoExato(1, 2);//CPF
            }
            else if (this.NumeroCNPJSacado.HasValue)
            {
                linha += Helper.NumericoParaTamanhoExato(2, 2);//CNPJ
            }
            else
            {
                linha += Helper.NumericoParaTamanhoExato(98, 2);//Não tem
            }
            //Número de Inscricao
            if (this.NumeroCPFSacado.HasValue)
            {
                linha += Helper.NumericoParaTamanhoExato(this.NumeroCPFSacado.Value, 14);//CPF
            }
            else if (this.NumeroCNPJSacado.HasValue)
            {
                linha += Helper.NumericoParaTamanhoExato(this.NumeroCNPJSacado.Value, 14);//CNPJ
            }
            else
            {
                linha += Helper.NumericoParaTamanhoExato(0, 14);//Não tem
            }
            //Nome do Pagador
            if (!String.IsNullOrEmpty(this.RazaoSocialSacado))
            {
                linha += Helper.StringParaTamanhoExato(this.RazaoSocialSacado, 40);
            }
            else
            {
                linha += Helper.StringParaTamanhoExato(this.NomePessoaSacado, 40);
            }
            //Endereço do pagador
            linha += Helper.StringParaTamanhoExato(this.DescricaoLogradouroSacado, 38);
            //Instruções de não recebimento do boleto
            linha += "  ";
            //Bairro do pagador
            linha += Helper.StringParaTamanhoExato(this.DescricaoBairroSacado, 12);
            //Cep do Pagador + Sufixo do pagador
            linha += Helper.NumericoParaTamanhoExato(Convert.ToDecimal(this.NumeroCepSacado), 8);
            //Cidade do Pagador
            if (String.IsNullOrEmpty(this.CidadeSacado.NomeCidade))
            {
                this.CidadeSacado.CompleteObject();
            }
            linha += Helper.StringParaTamanhoExato(this.CidadeSacado.NomeCidade, 15);
            //Sigla da UF
            linha += Helper.StringParaTamanhoExato(this.CidadeSacado.Estado.SiglaEstado, 2);
            //Sacador / Avalista
            linha += Helper.StringParaTamanhoExato(" ", 39);
            //Tipo de Boleto
            linha += " ";
            //Prazo de protesto
            linha += "  ";
            //Tipo de Moeda
            linha += "9";
            //Número Sequencial
            linha += Helper.NumericoParaTamanhoExato(numeroLinha, 6);

            return linha;
        }

        /// <summary>
        /// Lista todos os boletos marcados para registro
        /// </summary>
        /// <returns>Lista com os boletos para registro. Retorna lista vazia se nenhum boleto foi encontrado.</returns>
        public static List<CobrancaBoleto> ListaBoletosParaRegistro()
        {
            List<CobrancaBoleto> lstCobrancaBoleto = new List<CobrancaBoleto>();
            CobrancaBoleto objCobrancaBoleto = new CobrancaBoleto();

            SqlDataReader dr;

            dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_BOLETOS_PARA_REGISTAR, null);

            while (SetInstance_WithoutDispose(dr, objCobrancaBoleto))
            {
                lstCobrancaBoleto.Add(objCobrancaBoleto);
                objCobrancaBoleto = new CobrancaBoleto();
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();


            return lstCobrancaBoleto;
        }

        /// <summary>
        /// Lista todos os boletos a serem cancelados
        /// </summary>
        /// <returns>Lista com os boletos para o cancelamento do registro. Retorna lista vazia se nenhum boleto foi encontrado.</returns>
        public static List<CobrancaBoleto> ListaBoletosParaCancelamento()
        {
            List<CobrancaBoleto> lstCobrancaBoleto = new List<CobrancaBoleto>();
            CobrancaBoleto objCobrancaBoleto = new CobrancaBoleto();

            SqlDataReader dr;

            dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_BOLETOS_PARA_CANCELAR, null);

            while (SetInstance_WithoutDispose(dr, objCobrancaBoleto))
            {
                lstCobrancaBoleto.Add(objCobrancaBoleto);
                objCobrancaBoleto = new CobrancaBoleto();
            }

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();


            return lstCobrancaBoleto;
        }

        /// <summary>
        /// Faz a liquidação(pagamento) do boleto.
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Liquidar(SqlTransaction trans, DateTime dataPagamento, int idUsuarioLogado)
        {
            Pagamento objPagamento;

            if (Pagamento.CarregarPagamentoPorNossoNumeroBoleto(this.NumeroNossoNumero, out objPagamento))
            {
                //Informações adicionais utilizadas na grid de liberação
                this.Pagamento = objPagamento;

                #region Cancela demais boletos

                var listaPgMesmaParcela = Pagamento.RecuperarPagamentosMesmaParcela(objPagamento.PlanoParcela.IdPlanoParcela, objPagamento.IdPagamento, trans);

                foreach (int pagamento in listaPgMesmaParcela)
                {
                    var objPagamentoAux = BLL.Pagamento.LoadObject(pagamento);

                    objPagamentoAux.PagamentoSituacao = new PagamentoSituacao(Convert.ToInt32(Enumeradores.PagamentoSituacao.Cancelado));

                    objPagamentoAux.Save();
                }

                #endregion

                //se o objeto Pagamento ja estiver marcado como pago, não reefetua o pagamento
                if (!objPagamento.JaPago(trans))
                {
                    objPagamento.TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.BoletoBancario);
                    objPagamento.Liberar(trans, dataPagamento);

                }
                else
                {
                    objPagamento.PlanoParcela.DataPagamento = DateTime.Now.AddDays(-1);
                    objPagamento.PlanoParcela.PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago);
                    objPagamento.PlanoParcela.Save();
                }
            }
            else
            {
                try
                {
                    using (var objSine = new wsSine.AppClient())
                    {
                        UsuarioFilialPerfil objUsuario = UsuarioFilialPerfil.LoadObject(idUsuarioLogado);
                        objUsuario.PessoaFisica.CompleteObject();
                        objSine.LiberarDestaqueVaga(this.NumeroNossoNumero, objUsuario.PessoaFisica.NomeCompleto, objUsuario.PessoaFisica.CPF.ToString(), dataPagamento);
                    }
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    return false;
                }

            }

            return true;
        }

        #region SetInstance_WithoutDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objCobrancaBoleto">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstance_WithoutDispose(IDataReader dr, CobrancaBoleto objCobrancaBoleto)
        {
            try
            {
                if (dr.Read())
                {
                    objCobrancaBoleto._idCobrancaBoleto = Convert.ToInt32(dr["Idf_Cobranca_Boleto"]);
                    if (dr["Idf_Transacao"] != DBNull.Value)
                        objCobrancaBoleto._cobrancaBoletoTransacao = new CobrancaBoletoTransacao(Convert.ToInt32(dr["Idf_Transacao"]));
                    if (dr["Num_CNPJ_Cedente"] != DBNull.Value)
                        objCobrancaBoleto._numeroCNPJCedente = Convert.ToDecimal(dr["Num_CNPJ_Cedente"]);
                    if (dr["Num_CPF_Cedente"] != DBNull.Value)
                        objCobrancaBoleto._numeroCPFCedente = Convert.ToDecimal(dr["Num_CPF_Cedente"]);
                    if (dr["Num_Agencia_Bancaria"] != DBNull.Value)
                        objCobrancaBoleto._numeroAgenciaBancaria = Convert.ToString(dr["Num_Agencia_Bancaria"]);
                    if (dr["Num_Conta"] != DBNull.Value)
                        objCobrancaBoleto._numeroConta = Convert.ToString(dr["Num_Conta"]);
                    if (dr["Num_DV_Conta"] != DBNull.Value)
                        objCobrancaBoleto._numeroDVConta = Convert.ToString(dr["Num_DV_Conta"]);
                    if (dr["Raz_Social_Cedente"] != DBNull.Value)
                        objCobrancaBoleto._razaoSocialCedente = Convert.ToString(dr["Raz_Social_Cedente"]);
                    if (dr["Nme_Pessoa_Cedente"] != DBNull.Value)
                        objCobrancaBoleto._nomePessoaCedente = Convert.ToString(dr["Nme_Pessoa_Cedente"]);
                    if (dr["Idf_Banco"] != DBNull.Value)
                        objCobrancaBoleto._banco = new Banco(Convert.ToInt32(dr["Idf_Banco"]));
                    objCobrancaBoleto._flagRegistraBoleto = Convert.ToBoolean(dr["Flg_Registra_Boleto"]);
                    if (dr["Dta_Emissao"] != DBNull.Value)
                        objCobrancaBoleto._dataEmissao = Convert.ToDateTime(dr["Dta_Emissao"]);
                    if (dr["Dta_Vencimento"] != DBNull.Value)
                        objCobrancaBoleto._dataVencimento = Convert.ToDateTime(dr["Dta_Vencimento"]);
                    if (dr["Vlr_Boleto"] != DBNull.Value)
                        objCobrancaBoleto._valorBoleto = Convert.ToDecimal(dr["Vlr_Boleto"]);
                    if (dr["Num_Nosso_Numero"] != DBNull.Value)
                        objCobrancaBoleto._numeroNossoNumero = Convert.ToString(dr["Num_Nosso_Numero"]);
                    if (dr["Num_CPF_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._numeroCPFSacado = Convert.ToDecimal(dr["Num_CPF_Sacado"]);
                    if (dr["Num_CNPJ_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._numeroCNPJSacado = Convert.ToDecimal(dr["Num_CNPJ_Sacado"]);
                    if (dr["Nme_Pessoa_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._nomePessoaSacado = Convert.ToString(dr["Nme_Pessoa_Sacado"]);
                    if (dr["Raz_Social_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._razaoSocialSacado = Convert.ToString(dr["Raz_Social_Sacado"]);
                    if (dr["End_Email_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._enderecoEmailSacado = Convert.ToString(dr["End_Email_Sacado"]);
                    if (dr["Des_Logradouro_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._descricaoLogradouroSacado = Convert.ToString(dr["Des_Logradouro_Sacado"]);
                    if (dr["Num_Endereço_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._numeroEndereçoSacado = Convert.ToString(dr["Num_Endereço_Sacado"]);
                    if (dr["Des_Complemento_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._descricaoComplementoSacado = Convert.ToString(dr["Des_Complemento_Sacado"]);
                    if (dr["Idf_Cidade_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._cidadeSacado = new Cidade(Convert.ToInt32(dr["Idf_Cidade_Sacado"]));
                    if (dr["Num_Cep_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._numeroCepSacado = Convert.ToString(dr["Num_Cep_Sacado"]);
                    if (dr["Des_Bairro_Sacado"] != DBNull.Value)
                        objCobrancaBoleto._descricaoBairroSacado = Convert.ToString(dr["Des_Bairro_Sacado"]);
                    if (dr["Des_Instrucao_Caixa"] != DBNull.Value)
                        objCobrancaBoleto._descricaoInstrucaoCaixa = Convert.ToString(dr["Des_Instrucao_Caixa"]);
                    if (dr["Cod_Barras"] != DBNull.Value)
                        objCobrancaBoleto._codigoBarras = Convert.ToString(dr["Cod_Barras"]);
                    if (dr["Arq_Boleto"] != DBNull.Value)
                        objCobrancaBoleto._arquivoBoleto = Convert.ToString(dr["Arq_Boleto"]);
                    if (dr["Idf_Mensagem_Retorno_Boleto"] != DBNull.Value)
                        objCobrancaBoleto._mensagemRetornoBoleto = new MensagemRetornoBoleto(Convert.ToInt32(dr["Idf_Mensagem_Retorno_Boleto"]));

                    objCobrancaBoleto._persisted = true;
                    objCobrancaBoleto._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Geração de Boleto

        #region GerarBoleto
        /// <summary>
        /// Metodo responsável por gerar um boleto para determinado pagamento
        /// </summary>
        /// <param name="objPagamento">Uma pagamento para gerar um arquivo com um boleto</param>
        /// <returns></returns>
        public static string GerarBoleto(BLL.Pagamento objPagamento)
        {
            using (var conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        string retorno = GerarBoleto(objPagamento, trans);

                        trans.Commit();

                        return retorno; //string.Format("ArquivosTemporarios/{0}", fileName);
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
        public static string GerarBoleto(BLL.Pagamento objPagamento, SqlTransaction trans)
        {
            var imageArray = ProcessarBoleto(objPagamento, trans);

            string fileName = string.Format("boleto_{0}.jpg", DateTime.Now.Ticks);
            var diretorioAplicacao = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.DiretorioAplicacao);

#if DEBUG
            diretorioAplicacao = "C:\\tfs\\tfs.employer.com.br\\BNE\\src\\BNE-v.Prd\\BNE.Web";
           
#endif

            string filePath = string.Format("{0}\\ArquivosTemporarios\\{1}", diretorioAplicacao, fileName);

            nome_pdf = fileName;

            var ic = new ImageConverter();

            if (imageArray != null)
            {
                var convertedImage = (Image)ic.ConvertFrom(imageArray);
                if (convertedImage != null)
                {
                    var bmp = new Bitmap(convertedImage);

                    bmp.Save(filePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }

            return GetURL(filePath);
        }
        #endregion

        #region GerarBoleto
        /// <summary>
        /// Metodo responsável por gerar um arquivo com vários boletos para uma lista de pagamentos
        /// </summary>
        /// <param name="pagamentos">Uma lista de pagamentos para gerar apenas um arquivo com vários boletos</param>
        /// <returns></returns>

       public static string nome_pdf;

        public static string GerarBoleto(List<BLL.Pagamento> pagamentos, out byte[] pdfArray, out string pdfURL)
        {
            

            using (var conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var lista = new List<string>();
                        var listaBitmap = new List<Bitmap>();

                        var ic = new ImageConverter();
                        foreach (var objPagamento in pagamentos)
                        {
                            var imageArray = ProcessarBoleto(objPagamento, trans);
                            string fileName = string.Format("boleto_{0}.jpg", DateTime.Now.Ticks);

                            if (imageArray != null)
                            {
                                var convertedImage = (Image)ic.ConvertFrom(imageArray);
                                if (convertedImage != null)
                                {
                                    var bmp = new Bitmap(convertedImage);

                                    //Adicionando cada bitmpa para retornar apenas um no final
                                    listaBitmap.Add(bmp);

                                    lista.Add(string.Format("ArquivosTemporarios/{0}", fileName));
                                }
                            }
                        }

                        pdfArray = PDF.RecuperarPDFUsandoTextSharp(listaBitmap);

                        var fileNameCombined = String.Format("{0}\\{1}", System.Web.HttpContext.Current.Server.MapPath("ArquivosTemporarios"), string.Format("boletos_{0}.jpg", DateTime.Now.Ticks));

                        CombineBitmap(listaBitmap).Save(fileNameCombined, System.Drawing.Imaging.ImageFormat.Jpeg);

                        var fileNamePDF = string.Format("boletos_{0}.pdf", DateTime.Now.Ticks);
                        pdfURL = GerarLinkDownload(pdfArray, fileNamePDF);

                        nome_pdf = fileNamePDF;
                        
                        trans.Commit();
                        
                        return GetURL(fileNameCombined);
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
        #endregion

        #region ProcessarBoleto
        /// <summary>
        /// Metodo responsável por gerar o boleto para o pagamento
        /// </summary>
        /// <param name="objPagamento">Object</param>
        /// <returns></returns>
        public static byte[] ProcessarBoleto(BLL.Pagamento objPagamento)
        {
            using (var conn = new SqlConnection(BLL.DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        var retorno = ProcessarBoleto(objPagamento, trans);

                        trans.Commit();

                        return retorno;
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
        private static byte[] ProcessarBoleto(BLL.Pagamento objPagamento, SqlTransaction trans)
        {

            #region Variaveis

            PlanoAdquirido pa = PlanoAdquirido.CarregarPlanoAdquiridopDePagamento(objPagamento.IdPagamento);

            string strErro = null;
            byte[] byteImagem = null;

            String email = String.Empty;
            String descricaoLogradouro = String.Empty;
            String numeroEndereco = String.Empty;
            String descricaoComplemento = String.Empty;
            String numeroCEP = String.Empty;
            String descricaoBairro = String.Empty;
            String nossoNumero = String.Empty;
            String codigoBarras = String.Empty;
            decimal? CNPJ = null;
            decimal? CPF = null;
            String strNomeRazao = String.Empty;
            int idCidade = 0;
            Boolean registraBoleto = false;

            #endregion

            #region Informações do boleto
            if (objPagamento.Filial != null)
            {
                objPagamento.Filial.CompleteObject(trans);
                CNPJ = objPagamento.Filial.NumeroCNPJ.Value;
                strNomeRazao = objPagamento.Filial.RazaoSocial;

                UsuarioFilial objUsuarioFilial;
                if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objPagamento.UsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial))
                {
                    if (!String.IsNullOrEmpty(objUsuarioFilial.EmailComercial))
                        email = objUsuarioFilial.EmailComercial;
                }

                if (objPagamento.Filial.Endereco != null)
                {
                    objPagamento.Filial.Endereco.CompleteObject(trans);

                    idCidade = objPagamento.Filial.Endereco.Cidade.IdCidade;

                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoLogradouro))
                        descricaoLogradouro = objPagamento.Filial.Endereco.DescricaoLogradouro;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.NumeroEndereco))
                        numeroEndereco = objPagamento.Filial.Endereco.NumeroEndereco;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoComplemento))
                        descricaoComplemento = objPagamento.Filial.Endereco.DescricaoComplemento;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.NumeroCEP))
                        numeroCEP = objPagamento.Filial.Endereco.NumeroCEP;
                    if (!String.IsNullOrEmpty(objPagamento.Filial.Endereco.DescricaoBairro))
                        descricaoBairro = objPagamento.Filial.Endereco.DescricaoBairro;
                }
            }
            else
            {
                objPagamento.UsuarioFilialPerfil.CompleteObject(trans);
                objPagamento.UsuarioFilialPerfil.PessoaFisica.CompleteObject(trans);

                CPF = objPagamento.UsuarioFilialPerfil.PessoaFisica.CPF;
                strNomeRazao = objPagamento.UsuarioFilialPerfil.PessoaFisica.NomePessoa;

                if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.EmailPessoa))
                    email = objPagamento.UsuarioFilialPerfil.PessoaFisica.EmailPessoa;

                if (objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco != null)
                {
                    objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.CompleteObject(trans);

                    idCidade = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.Cidade.IdCidade;

                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoLogradouro))
                        descricaoLogradouro = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoLogradouro;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroEndereco))
                        numeroEndereco = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroEndereco;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoComplemento))
                        descricaoComplemento = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoComplemento;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroCEP))
                        numeroCEP = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.NumeroCEP;
                    if (!String.IsNullOrEmpty(objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoBairro))
                        descricaoBairro = objPagamento.UsuarioFilialPerfil.PessoaFisica.Endereco.DescricaoBairro;
                }
            }

            //objPagamento.TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.BoletoBancario);
            //objPagamento.Save(trans);
            #endregion

            //Carregando os parametros para o boleto
            List<Enumeradores.Parametro> parametros = new List<Enumeradores.Parametro>();
            parametros.Add(Enumeradores.Parametro.CobreBemXCodigoAgencia);
            parametros.Add(Enumeradores.Parametro.CobreBemXNumeroContaCorrente);
            parametros.Add(Enumeradores.Parametro.CobreBemXCodigoCedente);
            parametros.Add(Enumeradores.Parametro.CobreBemXCodBanco);
            parametros.Add(Enumeradores.Parametro.NumCnpjBNE);

            Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

            String codigoAgencia = valoresParametros[Enumeradores.Parametro.CobreBemXCodigoAgencia];
            String[] conta = valoresParametros[Enumeradores.Parametro.CobreBemXNumeroContaCorrente].Split('-');
            String codigoCedente = valoresParametros[Enumeradores.Parametro.CobreBemXCodigoCedente];
            String codigoBanco = valoresParametros[Enumeradores.Parametro.CobreBemXCodBanco];
            String cnpjBNE = valoresParametros[Enumeradores.Parametro.NumCnpjBNE];

            wsTRANSACAO.wsTRANSACAO objTransacao = new wsTRANSACAO.wsTRANSACAO();
            ServiceAuth.GerarHashAcessoWS(objTransacao);

            var textoRodapeBoleto = Parametro.RecuperaValorParametro(Enumeradores.Parametro.TextoRodapeBoleto);

            //Se o plano adquirido estiver configurado para registro de boleto e não é a primeira parcela do plano,
            //o boleto será registrado
            if (pa.FlagBoletoRegistrado && (objPagamento.PlanoParcela != null && objPagamento.PlanoParcela.NumeroParcela() > 1))
                registraBoleto = true;

            string strResult = objTransacao.CobrancaBoleto_Novo("bne",
                (int)Enumeradores.Sistema.BNE,
                textoRodapeBoleto,
                Convert.ToDecimal(cnpjBNE),
                codigoAgencia,
                codigoCedente,//convenio bancario
                conta[0],//numero conta
                conta[1],//digito verificador
                Convert.ToInt32(codigoBanco),
                "Bne - Banco Nacional de Empregos Ltda",
                registraBoleto,
                (DateTime)objPagamento.DataVencimento,
                objPagamento.ValorPagamento,
                strNomeRazao,
                email,
                "Boleto BNE",
                "",
                descricaoLogradouro,
                numeroEndereco,
                descricaoComplemento,
                idCidade,
                numeroCEP,
                descricaoBairro,
                "Todas as informações deste boleto são de exclusiva responsabilidade do cedente. Um dia útil após a confirmação do pagamento, você terá acesso ao serviço do BNE.",
                false,
                objPagamento.IdPagamento.ToString(),
                CPF,
                CNPJ,
                nossoNumero,
                out strErro,
                out byteImagem,
                out codigoBarras,
                out nossoNumero);

            objTransacao.RetornoTipoBoleto("bne", strResult, out nossoNumero, out codigoBarras);

            objPagamento.CodigoGuid = strResult;
            objPagamento.DescricaoIdentificador = nossoNumero;
            objPagamento.DescricaoDescricao = codigoBarras;
            objPagamento.Save(trans);

            return byteImagem;
        }
        #endregion

        #region RetornarBoleto
        public static byte[] RetornarBoleto(BLL.Pagamento objPagamento, bool gerarPDF)
        {
            var objTransacao = new wsTRANSACAO.wsTRANSACAO();
            ServiceAuth.GerarHashAcessoWS(objTransacao);

            #region GeraGuidPagamento
            if (string.IsNullOrWhiteSpace(objPagamento.CodigoGuid))
                ProcessarBoleto(objPagamento);
            #endregion

            return objTransacao.DevolveBoleto("bne", objPagamento.CodigoGuid, gerarPDF);
        }
        #endregion

        #region RetornarBoletoImagem
        public static string RetornarBoletoImagem(BLL.Pagamento objPagamento)
        {
            byte[] byteImagem = RetornarBoleto(objPagamento, false);

            string strNomeArquivo = string.Format("boleto_{0}.jpg", DateTime.Now.Ticks);
            return GerarLinkDownload(byteImagem, strNomeArquivo);
        }
        #endregion

        #region RetornarBoletoPDF
        public static string RetornarBoletoPDF(BLL.Pagamento objPagamento)
        {
            byte[] byteArray = RetornarBoleto(objPagamento, true);

            string strNomeArquivo = string.Format("boleto_{0}.pdf", DateTime.Now.Ticks);

            return GerarLinkDownload(byteArray, strNomeArquivo);
        }
        #endregion

        #region GerarLinkDownload
        /// <summary>
        /// Passa a string para gerar o arquivo para a tela de geração de arquivo (DownloadArquivo.aspx)
        /// </summary>
        /// <param name="strFinal">String de dados que devem estar no arquivo</param>
        /// <param name="nomeArquivo">Nome do arquivo a ser gerado</param>
        private static string GerarLinkDownload(byte[] arrayFinal, string nomeArquivo)
        {
            string caminhoArquivo = GetFilePath(nomeArquivo);

            FileStream fs = null;
            try
            {
                fs = new FileStream(caminhoArquivo, FileMode.CreateNew, FileAccess.Write);
                fs.Write(arrayFinal, 0, arrayFinal.Length);
                fs.Close();
            }
            finally
            {
                if (fs != null)
                    fs.Dispose();
            }

            return GetURL(caminhoArquivo);
        }
        #endregion

        private static string GetURL(string filePath)
        {
            return Helper.GetVirtualPath(filePath);
        }

        private static string GetFilePath(string fileName)
        {
            return String.Format("{0}\\{1}", System.Web.HttpContext.Current.Server.MapPath("ArquivosTemporarios"), fileName);
        }

        #endregion Geração de Boleto

        public static System.Drawing.Bitmap CombineBitmap(List<System.Drawing.Bitmap> images)
        {
            //read all images into memory
            System.Drawing.Bitmap finalImage = null;

            try
            {
                int width = 0;
                int height = 0;

                foreach (var image in images)
                {
                    //update the size of the final bitmap
                    width = image.Width > width ? image.Width : width;
                    height += image.Height; // > height ? image.Height : height
                }

                //create a bitmap to hold the combined image
                finalImage = new System.Drawing.Bitmap(width, height);

                //get a graphics object from the image so we can draw on it
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(finalImage))
                {
                    //set background color
                    g.Clear(System.Drawing.Color.Black);

                    //go through each image and draw it on the final image
                    int offset = 0;
                    foreach (Bitmap image in images)
                    {
                        g.DrawImage(image, new System.Drawing.Rectangle(0, offset, image.Width, image.Height));
                        offset += image.Height;
                    }
                }

                return finalImage;
            }
            catch (Exception ex)
            {
                if (finalImage != null)
                    finalImage.Dispose();

                throw ex;
            }
            finally
            {
                //clean up memory
                foreach (Bitmap image in images)
                {
                    image.Dispose();
                }
            }
        }

        #endregion

    }
}