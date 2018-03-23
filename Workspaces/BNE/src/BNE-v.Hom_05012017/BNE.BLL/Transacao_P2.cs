//-- Data: 28/02/2013 11:38
//-- Autor: Gieyson Stelmak

using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.BLL.Integracoes.ArquivosBanco;
using BNE.BLL.Integracoes.Pagamento;
using BNE.Cryptography;
using BNE.EL;
using PayPal;
using PayPal.Enum;
using PayPal.ExpressCheckout;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;
using Uol.PagSeguro.Constants;
using Uol.PagSeguro.Domain;
using Uol.PagSeguro.Exception;
using Uol.PagSeguro.Service;

namespace BNE.BLL
{
    public partial class Transacao // Tabela: BNE_Transacao
    {

        private static readonly ICryptography Crypto = AES.GetInstance();

        #region Propriedades

        #region NumeroCartaoCredito
        /// <summary>
        /// Tamanho do campo: 88.
        /// Campo opcional.
        /// </summary>
        public string NumeroCartaoCredito
        {
            get
            {
                return Crypto.Decrypt(this._numeroCartaoCredito);
            }
            set
            {
                this._numeroCartaoCredito = Crypto.Encrypt(value);
                this._numeroVersaoCriptografia = Crypto.Version;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroMesValidadeCartaoCredito
        /// <summary>
        /// Tamanho do campo: 64.
        /// Campo opcional.
        /// </summary>
        public string NumeroMesValidadeCartaoCredito
        {
            get
            {
                return Crypto.Decrypt(this._numeroMesValidadeCartaoCredito);
            }
            set
            {
                this._numeroMesValidadeCartaoCredito = Crypto.Encrypt(value);
                this._numeroVersaoCriptografia = Crypto.Version;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroAnoValidadeCartaoCredito
        /// <summary>
        /// Tamanho do campo: 64.
        /// Campo opcional.
        /// </summary>
        public string NumeroAnoValidadeCartaoCredito
        {
            get
            {
                return Crypto.Decrypt(this._numeroAnoValidadeCartaoCredito);
            }
            set
            {
                this._numeroAnoValidadeCartaoCredito = Crypto.Encrypt(value);
                this._numeroVersaoCriptografia = Crypto.Version;
                this._modified = true;
            }
        }
        #endregion

        #region NumeroCodigoVerificadorCartaoCredito
        /// <summary>
        /// Tamanho do campo: 64.
        /// Campo opcional.
        /// </summary>
        public string NumeroCodigoVerificadorCartaoCredito
        {
            get
            {
                return Crypto.Decrypt(this._numeroCodigoVerificadorCartaoCredito);
            }
            set
            {
                this._numeroCodigoVerificadorCartaoCredito = Crypto.Encrypt(value);
                this._numeroVersaoCriptografia = Crypto.Version;
                this._modified = true;
            }
        }
        #endregion

        #endregion

        #region Consultas
        private const string SP_SELECT_ID_PLANO_ADQUIRIDO = "SELECT * FROM BNE_Transacao WITH(NOLOCK) WHERE Idf_Plano_Adquirido = @Idf_Plano_Adquirido";
        private const string SP_SELECT_ULTIMA_ID_PLANO_ADQUIRIDO = "SELECT TOP 1 * FROM BNE_Transacao WITH(NOLOCK) WHERE Idf_Plano_Adquirido = @Idf_Plano_Adquirido ORDER BY Idf_Transacao DESC";
        private const string SP_UPDATE_STATUS_TRANSACAO = "UPDATE BNE_Transacao SET Idf_Status_Transacao = @IdfStatusTransacao WHERE Idf_Transacao = @IdfTransacao;";

        private const string SP_CARREGAR_TRANSACAO_POR_DESCRICAO = @"SELECT TOP 1 * FROM BNE.BNE_Transacao WHERE Des_Transacao = @desIdentificador AND Idf_Status_Transacao IN (3,9)";

        private const string SP_SELECT_TRANSACOES_DEBITO_RECCORENTE_A_REMETER =
            @"SELECT t.* 
            FROM BNE.BNE_Transacao t 
			INNER JOIN BNE.BNE_Pagamento pag ON t.Idf_Pagamento = pag.Idf_Pagamento
            INNER JOIN BNE.BNE_Plano_Adquirido pa ON t.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
            WHERE t.Idf_Status_Transacao = 0 --Não Enviada
		            AND t.Idf_Tipo_Pagamento = 8 -- Débito Recorrente
		            AND t.Idf_Banco = @idBanco --HSBC
					AND pag.Idf_Pagamento_Situacao = 1 --Em Aberto
		            AND (pa.Idf_Plano_Situacao IN (0, 1) --Aguardando Liberação OU Liberado;
							OR ( pa.Idf_Plano_Situacao IN (2) AND Flg_Recorrente = 1))";

        private const string SP_SELECT_TRANSACOES_DEBITO_RECCORENTE_A_CANCELAR =
            @"SELECT t.* 
            FROM BNE.BNE_Transacao t 
			INNER JOIN BNE.BNE_Pagamento pag ON t.Idf_Pagamento = pag.Idf_Pagamento
            INNER JOIN BNE.BNE_Plano_Adquirido pa ON t.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
			OUTER APPLY (SELECT COUNT(*) AS num_registros_cancelamento FROM BNE.BNE_Linha_Arquivo LA 
							WHERE LA.Idf_Transacao = T.Idf_Transacao
								  AND SUBSTRING(la.Des_Conteudo,150,1) = '1' --Posicao 150 = 1, indicando cancelamento
								  ) AS cancelamento
            WHERE t.Idf_Status_Transacao IN (5,7) --Transacao Registrada ou enviada
		            AND t.Idf_Tipo_Pagamento = 8 -- Débito Recorrente
		            AND t.Idf_Banco = @idBanco --HSBC
					AND pag.Idf_Pagamento_Situacao = 3 --Cancelado
					AND cancelamento.num_registros_cancelamento <= 0";

        private const string SP_SELECT_TRANSACOES_APROVADAS_NAO_CAPTURADAS =
            @"SELECT * FROM BNE.BNE_Transacao WITH(NOLOCK) WHERE Idf_Transacao IN (
                select	MIN(t.Idf_Transacao)
                from [BNE].[BNE_Transacao] t WITH(NOLOCK)
                JOIN BNE.BNE_Pagamento pag WITH(NOLOCK) ON pag.Idf_Pagamento = t.Idf_Pagamento
                JOIN BNE.BNE_Plano_Parcela pp WITH(NOLOCK) ON pag.Idf_Plano_Parcela = pp.Idf_Plano_Parcela
                JOIN BNE.BNE_Plano_Adquirido pa WITH(NOLOCK) ON pp.Idf_Plano_Adquirido = pa.Idf_Plano_Adquirido
                where 1 = 1
                AND t.Idf_Status_Transacao = 2 -- Transação Aprovada
                AND t.Idf_Tipo_Pagamento = 1 --Cartão de Crédito
                AND pa.Idf_Plano_Situacao = 0 --Aguardando liberação
                GROUP BY pa.Idf_Usuario_Filial_Perfil)";

        private const string SP_ULTIMO_TRANSACAO_PAGAMENTO_PARCELA_PLANO_PAGO = @"
        SELECT  TOP 1 T.*
        FROM    BNE.BNE_Pagamento PG WITH ( NOLOCK )
                JOIN BNE.BNE_Plano_Parcela PP WITH ( NOLOCK ) ON PP.Idf_Plano_Parcela = PG.Idf_Plano_Parcela
		        JOIN BNE.BNE_Transacao T WITH(NOLOCK) ON T.Idf_Pagamento = PG.Idf_Pagamento
        WHERE
		        PG.Idf_Pagamento_Situacao = 2 -- 2=>Pago
		        AND PP.Idf_Plano_Parcela_Situacao = 2 -- 2=>Pago
		        AND T.Idf_Status_Transacao IN (3,9) -- 9 => Realizada 3 => Capturado
		        AND T.Idf_Tipo_Pagamento  IN (1,8) -- 8 => Debito Recorrente 1 => Cartao de credito
		        AND PP.Idf_Plano_Adquirido = @Idf_Plano_Adquirido
        ORDER BY
		        Dta_Pagamento DESC,T.Idf_Transacao DESC";

        private const string SP_SELECT_TRANSACOES_DEBITO_ONLINE_BRADESCO_NAO_APROVADAS = @"
            WITH    TRANSACAO_PARA_RETORNO
                      AS ( SELECT   T.Idf_Transacao,
                                    IDF_Usuario_Gerador ,
                                    Idf_Pessoa_Fisica
                           FROM     BNE.BNE_Transacao T WITH ( NOLOCK )
                                    JOIN BNE.BNE_Pagamento PG WITH ( NOLOCK ) ON PG.Idf_Pagamento = T.Idf_Pagamento
                                    JOIN BNE.TAB_Usuario_Filial_Perfil UFP ON UFP.Idf_Usuario_Filial_Perfil = PG.Idf_Usuario_Filial_Perfil
                           WHERE    T.Idf_Tipo_Pagamento = 5
                                    AND PG.Idf_Pagamento_Situacao = 1 -- 1 => Em Aberto
                                    AND T.Idf_Status_Transacao = 0 -- 0 => Não Enviado
                                    AND T.Idf_Banco = 237 -- 237 => Bradesco
                         )
                SELECT  T.Idf_Transacao ,
                        Vlr_Documento ,
                        IDF_Usuario_Gerador ,
                        Idf_Pessoa_Fisica
                FROM    TRANSACAO_PARA_RETORNO TR
                        JOIN BNE.BNE_Transacao T ON T.Idf_Transacao = TR.Idf_Transacao
                ORDER BY T.Idf_Transacao DESC";


        private const string SP_SELECT_TRANSACOES_DEBITO_ONLINE_NAO_APROVADAS =
                @"  WITH  TRANSACAO_PARA_RETORNO
                          AS ( SELECT   MAX(T.Idf_Transacao) Idf_Transacao,
										IDF_Usuario_Gerador,
										Idf_Pessoa_Fisica
                               FROM     BNE.BNE_Transacao T WITH ( NOLOCK )
                                        JOIN BNE.BNE_Pagamento PG WITH ( NOLOCK ) ON PG.Idf_Pagamento = T.Idf_Pagamento
										JOIN BNE.TAB_Usuario_Filial_Perfil UFP ON UFP.Idf_Usuario_Filial_Perfil = PG.Idf_Usuario_Filial_Perfil
                               WHERE    T.Idf_Tipo_Pagamento = 5
                                        AND PG.Idf_Pagamento_Situacao = 1
                                        AND T.Idf_Status_Transacao = @Idf_Status_Transacao
                                        AND T.Idf_Banco = 1
                               GROUP BY PG.Idf_Usuario_Filial_Perfil,
										IDF_Usuario_Gerador,
										Idf_Pessoa_Fisica
                             )
                    SELECT  T.Idf_Transacao ,
			                Vlr_Documento,
							IDF_Usuario_Gerador,
							Idf_Pessoa_Fisica
                    FROM    TRANSACAO_PARA_RETORNO TR
                            JOIN BNE.BNE_Transacao T ON T.Idf_Transacao = TR.Idf_Transacao";

        private const string SP_SELECT_DEBITO_ONLINE_A_CANCELAR = @"
            SELECT  T.Idf_Transacao
            FROM    BNE.BNE_Transacao T 
            JOIN BNE.BNE_Pagamento PG ON PG.Idf_Pagamento = T.Idf_Pagamento
            JOIN BNE.TAB_Usuario_Filial_Perfil UFP ON UFP.Idf_Usuario_Filial_Perfil = PG.Idf_Usuario_Filial_Perfil
            JOIN BNE.TAB_Pessoa_Fisica PF ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica  
            WHERE
		            PF.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
		            AND T.Dta_Cadastro < DATEADD(MINUTE, @intervalo * -1,GETDATE())
                    AND PG.Idf_Pagamento_Situacao = 1
		            AND T.Idf_Tipo_Pagamento = 5
		            AND T.Idf_Banco = 1";

        private const string SP_SELECT_DEBITO_ONLINE_INTERVALO_TRANSACAO_EXISTE = @"
            SELECT  T.*
            FROM    BNE.BNE_Transacao T 
            JOIN BNE.BNE_Pagamento PG ON PG.Idf_Pagamento = T.Idf_Pagamento
            JOIN BNE.TAB_Usuario_Filial_Perfil UFP ON UFP.Idf_Usuario_Filial_Perfil = PG.Idf_Usuario_Filial_Perfil
            JOIN BNE.TAB_Pessoa_Fisica PF ON PF.Idf_Pessoa_Fisica = UFP.Idf_Pessoa_Fisica  
            WHERE
		            PF.Idf_Pessoa_Fisica = @Idf_Pessoa_Fisica
		            AND T.Dta_Cadastro >= DATEADD(MINUTE, @intervalo *-1,GETDATE())
                    AND PG.Idf_Pagamento_Situacao = 1
		            AND T.Idf_Tipo_Pagamento = 5
                    AND T.Idf_Status_Transacao = 5
		            AND T.Idf_Banco = 1";

        private const string SP_UPDATE_DEBITO_ONLINE_ERRO_TRANSACAO = @"
            UPDATE BNE_Transacao 
                SET Des_Mensagem_Captura = @Des_Mensagem_Captura,
                Idf_Status_Transacao = @IdfStatusTransacao  
            WHERE Idf_Transacao = @IdfTransacao;";

        #endregion

        #region Metodos

        #region Metodos Comuns

        #region Liquidar
        /// <summary>
        /// Faz a liquidação(pagamento) da transacao.
        /// </summary>
        /// <param name="trans"></param>
        /// <returns></returns>
        public bool Liquidar(SqlTransaction trans, DateTime dataPagamento, int idUsuarioLogado)
        {
            Pagamento objPagamento;

            if (Pagamento.CarregarPagamentoDeTransacao(this.IdTransacao, out objPagamento))
            {
                this.Pagamento = objPagamento;

                objPagamento.PlanoParcela.CompleteObject(trans);
                objPagamento.PlanoParcela.PlanoAdquirido.CompleteObject(trans);

                #region Cancela demais pagamentos da parcela

                var listaPgMesmaParcela = Pagamento.RecuperarPagamentosMesmaParcela(objPagamento.PlanoParcela.IdPlanoParcela, objPagamento.IdPagamento, trans);

                foreach (int pagamento in listaPgMesmaParcela)
                {
                    var objPagamentoAux = BLL.Pagamento.LoadObject(pagamento, trans);

                    objPagamentoAux.PagamentoSituacao = new PagamentoSituacao(Convert.ToInt32(Enumeradores.PagamentoSituacao.Cancelado));

                    objPagamentoAux.Save(trans);
                }

                #endregion

                //se o objeto Pagamento ja estiver marcado como pago, não reefetua o pagamento
                if (!objPagamento.JaPago(trans))
                {
                    objPagamento.PlanoParcela.PlanoAdquirido.PlanoLiberadoNaoPago = true;
                    objPagamento.Liberar(trans, dataPagamento);
                    PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.CarregarPlanoAdquiridopDePagamento(objPagamento.IdPagamento);

                    //Atualiza a data do Fim do Plano e zera as visualizações
                    if ((objPlanoAdquirido.QuantidadeDeParcelasPagaPlanoAdquirido(trans) > 1) && 
                            ((objPlanoAdquirido != null && objPlanoAdquirido.FlagRecorrente) || (objPlanoAdquirido.Plano.CompleteObject() && objPlanoAdquirido.Plano.FlagRecorrente)))
                    {
                        PlanoAdquirido.SalvarNovaDataFimPlano(objPlanoAdquirido.IdPlanoAdquirido, trans);
                        FilialObservacao.SalvarCRM("Plano Recorrente " + objPlanoAdquirido.IdPlanoAdquirido + " Pagamento feito via Debito Recorrente",
                            objPlanoAdquirido.Filial, "Pagamento Debito Plano Recorrente.", trans);
                    }
                    
                }
                else
                {
                    objPagamento.PlanoParcela.CompleteObject(trans);
                    if (this.Pagamento.PlanoParcela.DataPagamento == null)
                        this.Pagamento.PlanoParcela.DataPagamento = DateTime.Now.AddDays(-1);
                    objPagamento.PlanoParcela.PlanoParcelaSituacao = new PlanoParcelaSituacao((int)Enumeradores.PlanoParcelaSituacao.Pago);
                    objPagamento.PlanoParcela.Save(trans);
                }
            }
            else
            {
                throw new Exception("Pagamento não encontrado para a transacao");
            }

            this.AtualizarStatus(Enumeradores.StatusTransacao.Realizada, trans);

            return true;
        }
        #endregion

        #region AtualizarStatus
        /// <summary>
        /// Método utilizado para atualizar o status da transacao
        /// </summary>
        /// <param name="statusTransacao">Novo status da transacao.</param>
        /// <remarks>Francisco Ribas</remarks>
        public void AtualizarStatus(Enumeradores.StatusTransacao statusTransacao, SqlTransaction trans = null)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@IdfStatusTransacao", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@IdfTransacao", SqlDbType.Int, 4));
            parms[0].Value = (int)statusTransacao;
            parms[1].Value = this.IdTransacao;
            if (trans == null)
            {
                if (DataAccessLayer.ExecuteNonQuery(CommandType.Text, SP_UPDATE_STATUS_TRANSACAO, parms) <= 0)
                {
                    throw new RecordNotFoundException(typeof(Transacao));
                }
            }
            else
            {
                if (DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SP_UPDATE_STATUS_TRANSACAO, parms) <= 0)
                {
                    throw new RecordNotFoundException(typeof(Transacao));
                }
            }
        }

        #endregion

        #region CarregarPorPlanoAdquirido
        /// <summary>
        /// Método utilizado para retornar uma instância de Transacao a partir do banco de dados.
        /// </summary>
        /// <param name="idTransacao">Chave do registro.</param>
        /// <returns>Instância de Transacao.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Transacao CarregarPorPlanoAdquirido(int idPlanoAdquirido)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms[0].Value = idPlanoAdquirido;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_ID_PLANO_ADQUIRIDO, parms))
            {
                Transacao objTransacao = new Transacao();
                if (SetInstance(dr, objTransacao))
                    return objTransacao;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw new RecordNotFoundException(typeof(Transacao));
        }

        #endregion

        #region SetInstanceWithoutDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// Não dá o dispose no DataReader.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objTransacao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        private static bool SetInstanceWithoutDispose(IDataReader dr, Transacao objTransacao)
        {
            try
            {
                if (dr.Read())
                {
                    objTransacao._idTransacao = Convert.ToInt32(dr["Idf_Transacao"]);
                    objTransacao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    if (dr["Idf_Plano_Adquirido"] != DBNull.Value)
                        objTransacao._planoAdquirido = new PlanoAdquirido(Convert.ToInt32(dr["Idf_Plano_Adquirido"]));
                    if (dr["Idf_Tipo_Pagamento"] != DBNull.Value)
                        objTransacao._tipoPagamento = new TipoPagamento(Convert.ToInt32(dr["Idf_Tipo_Pagamento"]));
                    objTransacao._valorDocumento = Convert.ToDecimal(dr["Vlr_Documento"]);
                    if (dr["Num_Cartao_Credito"] != DBNull.Value)
                        objTransacao._numeroCartaoCredito = Convert.ToString(dr["Num_Cartao_Credito"]);
                    if (dr["Num_Mes_Validade_Cartao_Credito"] != DBNull.Value)
                        objTransacao._numeroMesValidadeCartaoCredito = Convert.ToString(dr["Num_Mes_Validade_Cartao_Credito"]);
                    if (dr["Num_Ano_Validade_Cartao_Credito"] != DBNull.Value)
                        objTransacao._numeroAnoValidadeCartaoCredito = Convert.ToString(dr["Num_Ano_Validade_Cartao_Credito"]);
                    if (dr["Num_Codigo_Verificador_Cartao_Credito"] != DBNull.Value)
                        objTransacao._numeroCodigoVerificadorCartaoCredito = Convert.ToString(dr["Num_Codigo_Verificador_Cartao_Credito"]);
                    if (dr["Des_IP_Comprador"] != DBNull.Value)
                        objTransacao._descricaoIPComprador = Convert.ToString(dr["Des_IP_Comprador"]);
                    if (dr["Idf_Operadora"] != DBNull.Value)
                        objTransacao._operadora = new Operadora(Convert.ToInt32(dr["Idf_Operadora"]));
                    if (dr["Num_Dia_Agendamento"] != DBNull.Value)
                        objTransacao._numeroDiaAgendamento = Convert.ToInt32(dr["Num_Dia_Agendamento"]);
                    if (dr["Num_Meses_Agendamento"] != DBNull.Value)
                        objTransacao._numeroMesesAgendamento = Convert.ToInt32(dr["Num_Meses_Agendamento"]);
                    if (dr["Num_Tentativas_Nao_Aprovado_Agendamento"] != DBNull.Value)
                        objTransacao._numeroTentativasNaoAprovadoAgendamento = Convert.ToInt32(dr["Num_Tentativas_Nao_Aprovado_Agendamento"]);
                    if (dr["Num_Dias_Entre_Tentativas_Agendamento"] != DBNull.Value)
                        objTransacao._numeroDiasEntreTentativasAgendamento = Convert.ToInt32(dr["Num_Dias_Entre_Tentativas_Agendamento"]);
                    if (dr["Idf_Banco"] != DBNull.Value)
                        objTransacao._banco = new Banco(Convert.ToInt32(dr["Idf_Banco"]));
                    if (dr["Des_Agencia_Debito"] != DBNull.Value)
                        objTransacao._descricaoAgenciaDebito = Convert.ToString(dr["Des_Agencia_Debito"]);
                    if (dr["Des_Conta_Corrente_Debito"] != DBNull.Value)
                        objTransacao._descricaoContaCorrenteDebito = Convert.ToString(dr["Des_Conta_Corrente_Debito"]);
                    if (dr["Nme_Titular_Conta_Corrente_Debito"] != DBNull.Value)
                        objTransacao._nomeTitularContaCorrenteDebito = Convert.ToString(dr["Nme_Titular_Conta_Corrente_Debito"]);
                    if (dr["Num_CPF_Titular_Conta_Corrente_Debito"] != DBNull.Value)
                        objTransacao._numeroCPFTitularContaCorrenteDebito = Convert.ToDecimal(dr["Num_CPF_Titular_Conta_Corrente_Debito"]);
                    if (dr["Des_Transacao"] != DBNull.Value)
                        objTransacao._descricaoTransacao = Convert.ToString(dr["Des_Transacao"]);
                    if (dr["Des_Mensagem_Captura"] != DBNull.Value)
                        objTransacao._descricaoMensagemCaptura = Convert.ToString(dr["Des_Mensagem_Captura"]);
                    objTransacao._statusTransacao = new StatusTransacao(Convert.ToInt32(dr["Idf_Status_Transacao"]));
                    if (dr["Num_CNPJ_Titular_Conta_Corrente_Debito"] != DBNull.Value)
                        objTransacao._numeroCNPJTitularContaCorrenteDebito = Convert.ToDecimal(dr["Num_CNPJ_Titular_Conta_Corrente_Debito"]);
                    if (dr["Idf_Pagamento"] != DBNull.Value)
                        objTransacao._pagamento = new Pagamento(Convert.ToInt32(dr["Idf_Pagamento"]));
                    if (dr["Num_Versao_Criptografia"] != DBNull.Value)
                        objTransacao._numeroVersaoCriptografia = Convert.ToString(dr["Num_Versao_Criptografia"]);
                    if (dr["Des_Codigo_token"] != DBNull.Value)
                        objTransacao._desCodigoToken = Convert.ToString(dr["Des_Codigo_token"]);

                    objTransacao._persisted = true;
                    objTransacao._modified = false;

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

        #region CriarTransacaoRedirecionamento
        private static Transacao CriarTransacaoRedirecionamento(ref Pagamento objPagamento, int idPlanoAdquirido, string descricaoIP)
        {
            return CriarTransacaoRedirecionamento(ref objPagamento, idPlanoAdquirido, descricaoIP, null);
        }
        private static Transacao CriarTransacaoRedirecionamento(ref Pagamento objPagamento, int idPlanoAdquirido, string descricaoIP, SqlTransaction trans)
        {
            try
            {
                var objTransacao = new Transacao
                {
                    PlanoAdquirido = new PlanoAdquirido(idPlanoAdquirido),
                    Pagamento = objPagamento,
                    TipoPagamento = objPagamento.TipoPagamento,
                    ValorDocumento = objPagamento.ValorPagamento,
                    DescricaoIPComprador = descricaoIP,
                    StatusTransacao = new StatusTransacao((int)Enumeradores.StatusTransacao.NaoEnviada)
                };

                if (trans != null)
                    objTransacao.Save(trans);
                else
                    objTransacao.Save();

                return objTransacao;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region CarregarUltimaPorPlanoAdquirido
        public static Transacao CarregarUltimaPorPlanoAdquirido(int idPlanoAdquirido)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));
            parms[0].Value = idPlanoAdquirido;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_ULTIMA_ID_PLANO_ADQUIRIDO, parms))
            {
                Transacao objTransacao = new Transacao();
                if (SetInstance(dr, objTransacao))
                    return objTransacao;
            }
            throw new RecordNotFoundException(typeof(Transacao));
        }
        #endregion

        #endregion

        #region Cartao de Credito

        #region CriarNovaTransacao
        /// <summary>
        /// Cria uma nova transação baseada em um transação prévia.
        /// </summary>
        /// <param name="objTransacao"></param>
        /// <param name="objPagamento"></param>
        /// <returns></returns>
        public static Transacao CriarNovaTransacao(Transacao objTransacao, ref Pagamento objPagamento)
        {
            return CriarNovaTransacao(objTransacao, ref objPagamento, null);
        }
        public static Transacao CriarNovaTransacao(Transacao objTransacao, ref Pagamento objPagamento, SqlTransaction trans)
        {
            return CriarTransacaoCartaoCredito(
                ref objPagamento,
                objTransacao.PlanoAdquirido.IdPlanoAdquirido,
                null,
                objTransacao.NumeroCartaoCredito.ToString(),
                Convert.ToInt32(objTransacao.NumeroMesValidadeCartaoCredito),
                Convert.ToInt32(objTransacao.NumeroAnoValidadeCartaoCredito),
                objTransacao.NumeroCodigoVerificadorCartaoCredito,
                (Enumeradores.Operadora)objTransacao.Operadora.IdOperadora,
                objTransacao.DesCodigoToken,
                trans);
        }
        #endregion

        #region CriarTransacaoCartaoCredito
        private static Transacao CriarTransacaoCartaoCredito(ref Pagamento objPagamento, int idPlanoAdquirido, string descricaoIP, string numeroCartao, int mesValidadeCartao, int anoValidadeCartao, string numeroDigitoVerificador, Enumeradores.Operadora bandeiraCartao, string desCodigoToken = null)
        {
            return CriarTransacaoCartaoCredito(ref objPagamento, idPlanoAdquirido, descricaoIP, numeroCartao, mesValidadeCartao, anoValidadeCartao, numeroDigitoVerificador, bandeiraCartao, desCodigoToken, null);
        }
        private static Transacao CriarTransacaoCartaoCredito(ref Pagamento objPagamento, int idPlanoAdquirido, string descricaoIP, string numeroCartao, int mesValidadeCartao, int anoValidadeCartao, string numeroDigitoVerificador, Enumeradores.Operadora bandeiraCartao, string desCodigoToken, SqlTransaction trans)
        {
            try
            {
                var objTransacao = new Transacao
                {
                    PlanoAdquirido = new PlanoAdquirido(idPlanoAdquirido),
                    Pagamento = objPagamento,
                    TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.CartaoCredito),
                    ValorDocumento = objPagamento.ValorPagamento,
                    NumeroCartaoCredito = Regex.Replace(numeroCartao, "[^0-9]", ""),
                    NumeroMesValidadeCartaoCredito = mesValidadeCartao.ToString(),
                    NumeroAnoValidadeCartaoCredito = anoValidadeCartao.ToString(),
                    NumeroCodigoVerificadorCartaoCredito = numeroDigitoVerificador,
                    DescricaoIPComprador = descricaoIP,
                    Operadora = new Operadora((int)bandeiraCartao),
                    StatusTransacao = new StatusTransacao((int)Enumeradores.StatusTransacao.NaoEnviada),
                    DesCodigoToken = desCodigoToken
                };

                if (trans != null)
                    objTransacao.Save(trans);
                else
                    objTransacao.Save();

                return objTransacao;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region ValidarPagamentoCartaoCredito
        public static bool ValidarPagamentoCartaoCredito(ref Pagamento objPagamento, int idPlanoAdquirido, string descricaoIP, String numeroCartao, int mesValidadeCartao, int anoValidadeCartao, string numeroDigitoVerificador, string desCodToken, out string erro)
        {
            erro = string.Empty;

            //Validando o cartão de crédito
            Enumeradores.Operadora? operadora;
            if (!Helper.ValidarCartaoCredito(numeroCartao, out operadora))
            {
                erro = "Falha ao validar os dados do cartão. Verifique os campos informados e tente novamente.";
                return false;
            }

            //Verificando se a operadora foi reconhecida
            if (!operadora.HasValue)
            {
                erro = "Falha ao reconhecer operadora. Informe um cartão Visa ou Mastercard e tente novamente.";
                return false;
            }

            objPagamento.Operadora = new Operadora((int)operadora.Value);
            objPagamento.Save();

            try
            {
                //Criando a transação 
                var objTransacao = CriarTransacaoCartaoCredito(ref objPagamento, idPlanoAdquirido, descricaoIP, numeroCartao, mesValidadeCartao, anoValidadeCartao, numeroDigitoVerificador, operadora.Value, desCodToken);

                //Recuperando o retorno
                CartaoCredito.RetornoCartaoCredito objRetorno = null;
                try
                {
                    objRetorno = EfetuarPagamentoCartaoDeCreditoCielo(objTransacao);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    erro = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                }

                if (objRetorno != null)
                {
                    //Atualizando a transação
                    objTransacao.DescricaoTransacao = objPagamento.DescricaoIdentificador = objRetorno.DescricaoTransacao;
                    objTransacao.DescricaoMensagemCaptura = objPagamento.DescricaoDescricao = objRetorno.CodigoAutorizacao;
                    objTransacao.Save();

                    if (objRetorno.Aprovado)
                    {
                        objTransacao.AtualizarStatus(Enumeradores.StatusTransacao.Aprovada);
                        //Se foi aprovado liberar o pagamento
                        if (objPagamento.Liberar(DateTime.Now))
                            objTransacao.AtualizarStatus(Enumeradores.StatusTransacao.Capturada);
                    }
                    else
                    {
                        objTransacao.AtualizarStatus(Enumeradores.StatusTransacao.NaoAprovada);
                        erro = objRetorno.DescricaoAutorizacao;
                    }

                    return objRetorno.Aprovado;
                }
                return false;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion


        #region EfetuarCancelamentoCartaoDeCreditoCielo
        public static bool EfetuarCancelamentoCartaoDeCreditoCielo(String desIdentificador)
        {
            Transacao objTransacao = Transacao.CarregarPelaDesIdentificador(desIdentificador);

            if (objTransacao == null) return false;
            try
            {
                String mid = ConfigurationManager.AppSettings["cielo.customer.id"];
                String key = ConfigurationManager.AppSettings["cielo.customer.key"];
                String url = ConfigurationManager.AppSettings["cielo.webservice.url"];

                //Instancia o servico
                BNE.Cielo.CieloService cielo = new Cielo.CieloService(mid, key, url);
                Cielo.Transaction transaction = cielo.cancellationRequest(objTransacao.DescricaoTransacao, Convert.ToInt32(objTransacao.ValorDocumento * 100));

                if (transaction.authorization.code == 9)
                {
                    objTransacao.AtualizarStatus(Enumeradores.StatusTransacao.Cancelada);
                    TransacaoResposta.SalvarResposta(objTransacao, false, "Transação Cancelada com Sucesso!", transaction.tid, transaction.authorization.arp, objTransacao.NumeroCartaoCredito.Replace(objTransacao.NumeroCartaoCredito.Substring(6, 6), "******"), null, null, null);
                }
                return true;
            }
            catch (Exception ex)
            {
                if (objTransacao != null)
                    TransacaoResposta.SalvarResposta(objTransacao, false, ex.InnerException != null ? ex.InnerException.Message + " - " + ex.Message : ex.Message, null, null, objTransacao.NumeroCartaoCredito.Replace(objTransacao.NumeroCartaoCredito.Substring(6, 6), "******"), null, null, null);
                return false;
            }
        }

        private static Transacao CarregarPelaDesIdentificador(string desIdentificador)
        {

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@DesIdentificador", SqlDbType.VarChar));
            parms[0].Value = desIdentificador;

            try
            {
                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_CARREGAR_TRANSACAO_POR_DESCRICAO, parms))
                {
                    Transacao objTransacao = new Transacao();
                    if (SetInstance(dr, objTransacao))
                        return objTransacao;
                }
                return null;
            }
            catch (SqlException ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                return null;
            }
        }
        #endregion

        #region EfetuarPagamentoCartaoDeCreditoCielo
        public static CartaoCredito.RetornoCartaoCredito EfetuarPagamentoCartaoDeCreditoCielo(Transacao objTransacao)
        {
            string mid = ConfigurationManager.AppSettings["cielo.customer.id"];
            string key = ConfigurationManager.AppSettings["cielo.customer.key"];
            string url = ConfigurationManager.AppSettings["cielo.webservice.url"];
            string urlRetorno = "www.bne.com.br/ConfirmacaoPagamento.aspx";

            Cielo.Holder holder = null;
            Cielo.PaymentMethod paymentMethod = null;

            switch (objTransacao.Operadora.IdOperadora)
            {
                case (int)Enumeradores.BandeiraCartao.Visa:
                    paymentMethod = new Cielo.PaymentMethod(Cielo.PaymentMethod.VISA, Cielo.PaymentMethod.CREDITO_A_VISTA);
                    break;
                case (int)Enumeradores.BandeiraCartao.MasterCard:
                    paymentMethod = new Cielo.PaymentMethod(Cielo.PaymentMethod.MASTERCARD, Cielo.PaymentMethod.CREDITO_A_VISTA);
                    break;
                case (int)Enumeradores.BandeiraCartao.Diners:
                    paymentMethod = new Cielo.PaymentMethod(Cielo.PaymentMethod.DINERS, Cielo.PaymentMethod.CREDITO_A_VISTA);
                    break;
                case (int)Enumeradores.BandeiraCartao.Amex:
                    paymentMethod = new Cielo.PaymentMethod(Cielo.PaymentMethod.AMEX, Cielo.PaymentMethod.CREDITO_A_VISTA);
                    break;
                case (int)Enumeradores.BandeiraCartao.Aura:
                    paymentMethod = new Cielo.PaymentMethod(Cielo.PaymentMethod.AURA, Cielo.PaymentMethod.CREDITO_A_VISTA);
                    break;
                case (int)Enumeradores.BandeiraCartao.JCB:
                    paymentMethod = new Cielo.PaymentMethod(Cielo.PaymentMethod.JCB, Cielo.PaymentMethod.CREDITO_A_VISTA);
                    break;
                case (int)Enumeradores.BandeiraCartao.ELO:
                    paymentMethod = new Cielo.PaymentMethod(Cielo.PaymentMethod.ELO, Cielo.PaymentMethod.CREDITO_A_VISTA);
                    break;
                case (int)Enumeradores.BandeiraCartao.Discover:
                    paymentMethod = new Cielo.PaymentMethod(Cielo.PaymentMethod.DISCOVER, Cielo.PaymentMethod.CREDITO_A_VISTA);
                    break;
            }

            //Instancia o servico
            Cielo.CieloService cielo = new Cielo.CieloService(mid, key, url);
            Cielo.Transaction.AuthorizationMethod authorizationMethod = Cielo.Transaction.AuthorizationMethod.AUTHORIZE_WITHOUT_AUTHENTICATION;
            Cielo.Order order = cielo.order(objTransacao.IdTransacao.ToString(), Convert.ToInt32(objTransacao.ValorDocumento * 100));

            objTransacao.PlanoAdquirido.CompleteObject();

            if (objTransacao.PlanoAdquirido.FlagRecorrente || PlanoAdquirido.PlanoEmpresaCobrancaCartao(objTransacao.PlanoAdquirido.IdPlanoAdquirido))
                authorizationMethod = Cielo.Transaction.AuthorizationMethod.RECURRENCE;

            if (!string.IsNullOrEmpty(objTransacao.DesCodigoToken))
                holder = cielo.holder(System.Web.HttpUtility.UrlEncode(objTransacao.DesCodigoToken));
            else
            {
                Cielo.Token token = cielo.tokenRequest(cielo.holder(objTransacao.NumeroCartaoCredito, (Convert.ToInt16(objTransacao.NumeroAnoValidadeCartaoCredito) + 2000).ToString(), objTransacao.NumeroMesValidadeCartaoCredito.Length == 1 ? objTransacao.NumeroMesValidadeCartaoCredito.Insert(0, "0") : objTransacao.NumeroMesValidadeCartaoCredito, objTransacao.NumeroCodigoVerificadorCartaoCredito));

                if (token != null)
                {
                    objTransacao.DesCodigoToken = token.code;
                    objTransacao.Save();

                    holder = cielo.holder(System.Web.HttpUtility.UrlEncode(token.code));
                }
                else
                {
                    holder = cielo.holder(objTransacao.NumeroCartaoCredito, (Convert.ToInt16(objTransacao.NumeroAnoValidadeCartaoCredito) + 2000).ToString(), objTransacao.NumeroMesValidadeCartaoCredito.Length == 1 ? objTransacao.NumeroMesValidadeCartaoCredito.Insert(0, "0") : objTransacao.NumeroMesValidadeCartaoCredito, objTransacao.NumeroCodigoVerificadorCartaoCredito);
                    urlRetorno = string.Empty;
                }

            }

            try
            {
                Cielo.Transaction transaction = cielo.transactionRequest(holder, order, paymentMethod, urlRetorno, authorizationMethod, true);

                if (transaction.authorization != null)
                {
                    if (transaction.authorization.code == 5 && transaction.token != null)
                    {
                        TransacaoResposta.SalvarResposta(objTransacao, false, "Possível troca de número de cartão e será tentada a recompra." + " Cod: " + transaction.authorization.lr, transaction.tid, transaction.authorization.arp, objTransacao.NumeroCartaoCredito.Replace(objTransacao.NumeroCartaoCredito.Substring(6, 6), "******"), null, null, null);
                        objTransacao.DesCodigoToken = transaction.token.code;
                        objTransacao.Save();

                        holder = cielo.holder(System.Web.HttpUtility.UrlEncode(transaction.token.code));
                        transaction = cielo.transactionRequest(holder, order, paymentMethod, urlRetorno, authorizationMethod, true);
                    }
                    TransacaoResposta.SalvarResposta(objTransacao, (transaction.authorization.code == 6) ? true : false, transaction.authorization.message.Replace("��", "çã") + " Cod: " + MensagemDeFalha(transaction.authorization.lr), transaction.tid, transaction.authorization.arp, objTransacao.NumeroCartaoCredito.Replace(objTransacao.NumeroCartaoCredito.Substring(6, 6), "******"), null, null, null);
                }

                return new CartaoCredito.RetornoCartaoCredito()
                {
                    Aprovado = (transaction.authorization.code == 6) ? true : false,
                    DescricaoTransacao = transaction.tid,
                    DescricaoAutorizacao = transaction.authorization.message.Replace("��", "çã") + " Cod: " + MensagemDeFalha(transaction.authorization.lr),
                    CodigoAutorizacao = transaction.authorization.nsu.ToString()
                };
            }
            catch (Exception ex)
            {
                TransacaoResposta.SalvarResposta(objTransacao, false, ex.Message, null, null, objTransacao.NumeroCartaoCredito.Replace(objTransacao.NumeroCartaoCredito.Substring(6, 6), "******"), null, null, null);

                return new CartaoCredito.RetornoCartaoCredito()
                {
                    Aprovado = false,
                    DescricaoAutorizacao = ex.Message
                };
            }
        }

        private static string MensagemDeFalha(int lr)
        {
            Dictionary<int, string> CodigosDeFalha = new Dictionary<int, string>();
            CodigosDeFalha.Add(1, "Referida pelo banco emissor");
            CodigosDeFalha.Add(4, "Existe algum tipo de restrição no cartão");
            CodigosDeFalha.Add(5, "Existe algum tipo de restrição no cartão");
            CodigosDeFalha.Add(6, "Falha na autorização");
            CodigosDeFalha.Add(7, "Existe algum tipo de restrição no cartão");
            CodigosDeFalha.Add(8, "Falha na autorização");
            CodigosDeFalha.Add(11, "Transação internacional aprovada com sucesso");
            CodigosDeFalha.Add(13, "Valor inválido");
            CodigosDeFalha.Add(14, "Digitação incorreta do número do cartão");
            CodigosDeFalha.Add(15, "Banco emissor indisponível");
            CodigosDeFalha.Add(21, "Cancelamento não localizado no banco emissor");
            CodigosDeFalha.Add(41, "Existe algum tipo de restrição no cartão");
            CodigosDeFalha.Add(51, "Saldo insuficiente");
            CodigosDeFalha.Add(54, "Cartão vencido");
            CodigosDeFalha.Add(57, "Existe algum tipo de restrição no cartão");
            CodigosDeFalha.Add(60, "Existe algum tipo de restrição no cartão");
            CodigosDeFalha.Add(62, "Existe algum tipo de restrição no cartão");
            CodigosDeFalha.Add(78, "Cartão não foi desbloqueado pelo portador ");
            CodigosDeFalha.Add(82, "Cartão inválido");
            CodigosDeFalha.Add(91, "Banco emissor indisponível");
            CodigosDeFalha.Add(96, "Falha no envio da autorização");

            
            
            if (CodigosDeFalha.ContainsKey(lr)) //Falha
                return lr +" - "+ CodigosDeFalha[lr];
            return lr.ToString();

        }
        #endregion


        #region CarregarTransacoesCreditoNaoCapturadas
        /// <summary>
        /// Retorna as transações de cartão de crédito aprovadas mas não capturadas
        /// </summary>
        /// <returns>Lista com as transações não capturadas.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static List<Transacao> CarregarTransacoesCreditoNaoCapturadas()
        {
            List<Transacao> lstRetorno = new List<Transacao>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_TRANSACOES_APROVADAS_NAO_CAPTURADAS, null))
            {
                Transacao objTransacao = new Transacao();
                while (SetInstanceWithoutDispose(dr, objTransacao))
                {
                    lstRetorno.Add(objTransacao);
                    objTransacao = new Transacao();
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lstRetorno;
        }
        #endregion

        #endregion

        #region Debito Online

        #region ValidarPagamentoDebitoOnline
        public static Transacao ValidarPagamentoDebitoOnline(ref BLL.Pagamento objPagamento, BLL.PlanoAdquirido objPlanoAdquirido, string descricaoIP, Enumeradores.Banco banco, out string erro)
        {
            erro = string.Empty;

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Criando a transação 
                        var objTransacao = CriarTransacaoDebitoOnline(ref objPagamento, objPlanoAdquirido, descricaoIP, banco, string.Empty, string.Empty, null, null, out erro, trans);
                        objPagamento.PlanoParcela.PlanoAdquirido = objPlanoAdquirido;

                        if (!String.IsNullOrEmpty(erro))
                        {
                            trans.Rollback();
                            return null;
                        }

                        objTransacao.Save(trans);
                        trans.Commit();



                        return objTransacao;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        #endregion

        #region CriarTransacaoDebitoOnline
        public static Transacao CriarTransacaoDebitoOnline(ref Pagamento objPagamento, PlanoAdquirido objPlanoAdquirido, string descricaoIP, Enumeradores.Banco banco, string agencia, string conta, decimal? cpf, decimal? cnpj, out string erro, SqlTransaction trans)
        {
            try
            {
                erro = String.Empty;
                var objTransacao = new Transacao
                {
                    PlanoAdquirido = objPlanoAdquirido,
                    Pagamento = objPagamento,
                    TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.DebitoOnline),
                    ValorDocumento = objPagamento.ValorPagamento,
                    DescricaoIPComprador = descricaoIP,
                    Banco = new Banco((int)banco),
                    DescricaoAgenciaDebito = agencia,
                    DescricaoContaCorrenteDebito = conta,
                    StatusTransacao = new StatusTransacao((int)Enumeradores.StatusTransacao.NaoEnviada),
                };
                objTransacao.Save(trans);

                return objTransacao;
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region CancelamentoDebitoOnline
        public void CancelamentoDebitoOnline(string erro, SqlTransaction trans = null)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@IdfTransacao", SqlDbType.Int));
            parms.Add(new SqlParameter("@Des_Mensagem_Captura", SqlDbType.VarChar, 200));
            parms.Add(new SqlParameter("@IdfStatusTransacao", SqlDbType.Int));
            parms[0].Value = this.IdTransacao;
            parms[1].Value = erro;
            parms[2].Value = (int)Enumeradores.StatusTransacao.Cancelada;

            if (trans == null)
            {
                if (DataAccessLayer.ExecuteNonQuery(CommandType.Text, SP_UPDATE_DEBITO_ONLINE_ERRO_TRANSACAO, parms) <= 0)
                {
                    throw new RecordNotFoundException(typeof(Transacao));
                }
            }
            else
            {
                if (DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SP_UPDATE_DEBITO_ONLINE_ERRO_TRANSACAO, parms) <= 0)
                {
                    throw new RecordNotFoundException(typeof(Transacao));
                }
            }
        }
        #endregion

        #region AtualizarStatusTransacaoDebitoOnline
        public static void AtualizarStatusTransacaoDebitoOnline(string refTran)
        {
            try
            {
                Transacao objTransacao = Transacao.LoadObject(Convert.ToInt32(refTran));
                objTransacao.AtualizarStatus(Enumeradores.StatusTransacao.Enviada);
            }
            catch (SqlException ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }

        }
        #endregion

        #region RecuperarPagamentoDebitoOnlineIntervalo
        public static bool ExisteTransacaoDebitoOnlineNoIntervalo(int idPessoaFisica, string intervaloDeTempo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int));
            parms.Add(new SqlParameter("@intervalo", SqlDbType.Int));
            parms[0].Value = idPessoaFisica;
            parms[1].Value = intervaloDeTempo;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_DEBITO_ONLINE_INTERVALO_TRANSACAO_EXISTE, parms).HasRows;
        }
        #endregion

        #region recuperarTransacoesDebitoOnlineACancelar
        public static IEnumerable<int> recuperarTransacoesDebitoOnlineACancelar(int idPessoaFisica, string intervaloDeTempo)
        {
            List<int> listTransacao = new List<int>();
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pessoa_Fisica", SqlDbType.Int));
            parms.Add(new SqlParameter("@intervalo", SqlDbType.Int));
            parms[0].Value = idPessoaFisica;
            parms[1].Value = intervaloDeTempo;


            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_DEBITO_ONLINE_A_CANCELAR, parms))
            {
                while (dr.Read())
                    listTransacao.Add(Convert.ToInt32(dr["Idf_Transacao"]));

            }
            return listTransacao;
        }
        #endregion

        #region ObterTransacoesDebitoOnlineNaoPagas
        public static IDataReader ObterTransacoesDebitoOnlineNaoConfirmados(Enumeradores.StatusTransacao statusTransacao)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Status_Transacao", SqlDbType.Int, 4));
            parms[0].Value = (int)statusTransacao;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_TRANSACOES_DEBITO_ONLINE_NAO_APROVADAS, parms);
        }

        #endregion

        #region ObterTransacoesDebitoOnlineNaoPagasBradesco
        public static IDataReader ObterTransacoesDebitoOnlineNaoPagasBradesco(SqlTransaction trans)
        {
            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SP_SELECT_TRANSACOES_DEBITO_ONLINE_BRADESCO_NAO_APROVADAS, null);
        }

        #endregion

        #endregion

        #region Debito Recorrente

        #region ValidarPagamentoDebito
        public static bool ValidarPagamentoDebito(ref Pagamento objPagamento, PlanoAdquirido objPlanoAdquirido, string descricaoIP, Enumeradores.Banco banco, string agencia, string conta, decimal? cpf, decimal? cnpj, out string erro)
        {
            erro = string.Empty;

            #region validacao da conta
            if (String.IsNullOrEmpty(agencia) || String.IsNullOrEmpty(conta))
            {
                erro = "Agencia e/ou conta não informada.";
                return false;
            }
            if (banco == Enumeradores.Banco.BANCODOBRASIL && !Helper.ValidarContaBancoDoBrasil(agencia, conta))
            {
                erro = "Agência/Conta Inválidos. Verifique os dados informados e tente novamente.";
                return false;
            }
            if (banco == Enumeradores.Banco.HSBC && !Helper.ValidarContaHSBC(agencia, conta))
            {
                erro = "Agência/Conta Inválidos. Verifique os dados informados e tente novamente.";
                return false;
            }
            #endregion validacao da conta

            using (var conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Criando a transação 
                        var objTransacao = CriarTransacaoDebito(ref objPagamento, objPlanoAdquirido, descricaoIP, banco, agencia, conta, cpf, cnpj, out erro, trans);
                        objPagamento.PlanoParcela.PlanoAdquirido = objPlanoAdquirido;

                        if (!String.IsNullOrEmpty(erro))
                        {
                            trans.Rollback();
                            return false;
                        }
                        //if (!Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagamentoDebitoLiberacaoNaConfirmacaoDoDebito, trans)))
                        //{
                        //    //Se não aguarda a confirmação do débito para liberar o plano
                        //    //Libera somente o plano, sem baixar o pagamento
                        //    if (objPlanoAdquirido.ParaPessoaFisica(trans))
                        //    {
                        //        Curriculo objCurriculo;
                        //        Curriculo.LiberarVIP(objPlanoAdquirido, out objCurriculo, trans);
                        //        objPagamento.PlanoParcela.CancelarOutrosPagamentos(objPagamento, trans);
                        //        objPagamento.PlanoParcela.CancelarOutrosPlanosAdquiridos(objCurriculo, trans);
                        //    }
                        //    else if (objPlanoAdquirido.ParaPessoaJuridica(trans))
                        //    {
                        //        UsuarioFilial objUsuarioFilial;
                        //        UsuarioFilial.LiberarCIA(objPlanoAdquirido, objPagamento.PlanoParcela, out objUsuarioFilial, trans);
                        //        objPagamento.PlanoParcela.CancelarOutrosPlanosAdquiridos(objPlanoAdquirido.Filial, trans);
                        //        objPagamento.PlanoParcela.RecarregarSMS(null, trans);    // libera SMSs da parcela
                        //    }
                        //    objPlanoAdquirido.LiberarPlanoAdquirido(null, trans);
                        //}

                        objTransacao.Save(trans);
                        trans.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
        }
        #endregion

        #region CriarTransacaoDebito
        public static Transacao CriarTransacaoDebito(int idPagamento, PlanoAdquirido objPlanoAdquirido, string descricaoIP, Enumeradores.Banco banco, string agencia, string conta, decimal? cpf, decimal? cnpj, out string erro, SqlTransaction trans)
        {
            Pagamento objPagamento = Pagamento.LoadObject(idPagamento, trans);
            return CriarTransacaoDebito(ref objPagamento, objPlanoAdquirido, descricaoIP, banco, agencia, conta, cpf, cnpj, out erro, trans);
        }

        public static Transacao CriarTransacaoDebito(ref Pagamento objPagamento, PlanoAdquirido objPlanoAdquirido, string descricaoIP, Enumeradores.Banco banco, string agencia, string conta, decimal? cpf, decimal? cnpj, out string erro, SqlTransaction trans)
        {
            try
            {
                erro = String.Empty;
                var objTransacao = new Transacao
                {
                    PlanoAdquirido = objPlanoAdquirido,
                    Pagamento = objPagamento,
                    TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.DebitoRecorrente),
                    ValorDocumento = objPagamento.ValorPagamento,
                    DescricaoIPComprador = descricaoIP,
                    Banco = new Banco((int)banco),
                    DescricaoAgenciaDebito = agencia,
                    DescricaoContaCorrenteDebito = conta,
                    StatusTransacao = new StatusTransacao((int)Enumeradores.StatusTransacao.NaoEnviada),
                    NumeroCNPJTitularContaCorrenteDebito = cnpj,
                    NumeroCPFTitularContaCorrenteDebito = cpf
                };
                objTransacao.Save(trans);

                return objTransacao;
            }
            catch (Exception ex)
            {
                erro = ex.Message;
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
        #endregion

        #region CarregarTransacoesDebitoRecorrenteNaoRemetidas
        /// <summary>
        /// Método utilizado para retornar uma lista de transações de débito a serem registradas (com pagamento em aberto e que o débito ainda não esteja registrado)
        /// </summary>
        /// <param name="idBanco">Identificador do banco das transações a serem registradas.</param>
        /// <returns>Lista com as transações a serem registradas.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static List<Transacao> CarregarTransacoesDebitoRecorrenteNaoRemetidas(int idBanco)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idBanco", SqlDbType.Int, 4));
            parms[0].Value = idBanco;

            List<Transacao> lstRetorno = new List<Transacao>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_TRANSACOES_DEBITO_RECCORENTE_A_REMETER, parms))
            {
                Transacao objTransacao = new Transacao();
                while (SetInstanceWithoutDispose(dr, objTransacao))
                {
                    lstRetorno.Add(objTransacao);
                    objTransacao = new Transacao();
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lstRetorno;
        }

        #endregion

        #region CarregarTransacoesDebitoRecorrenteACancelar
        /// <summary>
        /// Método utilizado para retornar uma lista de transações de débito a serem canceladas (com pagamento cancelados e que o débito esteja registrado)
        /// </summary>
        /// <param name="idBanco">Identificador do banco das transações a serem canceladas.</param>
        /// <returns>Lista com as transações a serem canceladas.</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static List<Transacao> CarregarTransacoesDebitoRecorrenteACancelar(int idBanco)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@idBanco", SqlDbType.Int, 4));
            parms[0].Value = idBanco;

            List<Transacao> lstRetorno = new List<Transacao>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_SELECT_TRANSACOES_DEBITO_RECCORENTE_A_CANCELAR, parms))
            {
                Transacao objTransacao = new Transacao();
                while (SetInstanceWithoutDispose(dr, objTransacao))
                {
                    lstRetorno.Add(objTransacao);
                    objTransacao = new Transacao();
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lstRetorno;
        }
        #endregion

        #region CarregarUltimaTransacaoPagaPorPlanoAdquirido
        public static Transacao CarregarUltimaTransacaoPagaPorPlanoAdquirido(int IdPlanoAdquirido, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Plano_Adquirido", SqlDbType = SqlDbType.Int, Size = 4, Value = IdPlanoAdquirido }
                };

            SqlDataReader dr;

            using (dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SP_ULTIMO_TRANSACAO_PAGAMENTO_PARCELA_PLANO_PAGO, parms))
            {
                Transacao objTransacao = new Transacao();
                if (SetInstance(dr, objTransacao))
                    return objTransacao;
            }
            return null;
        }
        #endregion

        #endregion

        #region PagSeguro

        public static string CriarTransacaoPagSeguro(Pagamento objPagamento, PlanoAdquirido objPlanoAdquirido, int idPessoaFisica, string descricaoIP, string urlRedirecionamento)
        {
            string urlRetorno = null;

            #region Carregando objetos necessários à execução
            if (objPlanoAdquirido.Plano == null || objPlanoAdquirido.Plano.IdPlano <= 0)
            {
                objPlanoAdquirido.CompleteObject();
            }

            Plano objPlano = objPlanoAdquirido.Plano;
            if (objPlano.DescricaoPlano == null)
            {
                objPlano.CompleteObject();
            }

            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(idPessoaFisica);
            #endregion Carregando objetos necessários à execução

            Transacao objTransacao = Transacao.CriarTransacaoRedirecionamento(ref objPagamento, objPlanoAdquirido.IdPlanoAdquirido, descricaoIP);

            try
            {
                Uol.PagSeguro.Domain.PaymentRequest payment = new Uol.PagSeguro.Domain.PaymentRequest();
                payment.Reference = objTransacao.IdTransacao.ToString();
                payment.Items.Add(new Item(objPlano.IdPlano.ToString(), objPlano.DescricaoPlano, 1, objPagamento.ValorPagamento));
                payment.Sender = new Sender(
                                            objPessoaFisica.NomeCompleto,
                                            objPessoaFisica.EmailPessoa,
                                            new Phone(
                                                        objPessoaFisica.NumeroDDDCelular.Trim(),
                                                        objPessoaFisica.NumeroCelular.Trim()
                                                     )
                                           );
                payment.Sender.Documents.Add(new SenderDocument(Documents.GetDocumentByType("CPF").Trim(), objPessoaFisica.NumeroCPF));

                payment.Currency = Currency.Brl;

                AccountCredentials credentials = new AccountCredentials(
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagSeguro_EmailCredencial),
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagSeguro_ChaveCredencial)
                );

#if !DEBUG
                //PagSeguro não aceita redirecionamento para Localhost
                payment.RedirectUri = new Uri(urlRedirecionamento);
#endif

                urlRetorno = payment.Register(credentials).AbsoluteUri;
            }
            catch (PagSeguroServiceException exception)
            {

                Console.WriteLine(exception.StatusCode);

                if (exception.StatusCode == HttpStatusCode.Unauthorized)
                {
                    EL.GerenciadorException.GravarExcecao(exception, "Unauthorized: please verify if the credentials used in the web service call are correct.");
                }

                foreach (ServiceError error in exception.Errors)
                {
                    EL.GerenciadorException.GravarExcecao(exception, error.Message);
                }

            }

            return urlRetorno;
        }

        public static void AtualizarSituacaoPagSeguro(string idNotificacaoPagSeguro)
        {
            #region Consultando status no PagSeguro
            try
            {
                // Inicializando credenciais  
                AccountCredentials credentials = new AccountCredentials(
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagSeguro_EmailCredencial),
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagSeguro_ChaveCredencial)
                );

                // Realizando uma consulta da notificacao a partir do código da notificação
                // para obter o objeto Transaction  
                Transaction transaction = NotificationService.CheckTransaction(
                                            credentials,
                                            idNotificacaoPagSeguro
                                        );

                Transacao objTransacao;
                AtualizarSituacaoPagSeguro(transaction, out objTransacao);
            }
            catch (PagSeguroServiceException exception)
            {

                Console.WriteLine(exception.StatusCode);

                if (exception.StatusCode == HttpStatusCode.Unauthorized)
                {
                    EL.GerenciadorException.GravarExcecao(exception, "Unauthorized: please verify if the credentials used in the web service call are correct.");
                }

                foreach (ServiceError error in exception.Errors)
                {
                    EL.GerenciadorException.GravarExcecao(exception, error.Message);
                }

            }
            #endregion Consultando status no PagSeguro
        }

        public static void AtualizarSituacaoPagSeguro(string idTransacaoPagSeguro, out Transacao objTransacao)
        {
            Transaction transaction = null;

            #region Consultando status no PagSeguro
            try
            {
                // Inicializando credenciais  
                AccountCredentials credentials = new AccountCredentials(
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagSeguro_EmailCredencial),
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagSeguro_ChaveCredencial)
                );

                // Realizando uma consulta de transação a partir do código identificador   
                // para obter o objeto Transaction  
                transaction = TransactionSearchService.SearchByCode(
                    credentials,
                    idTransacaoPagSeguro
                );
            }
            catch (PagSeguroServiceException exception)
            {

                Console.WriteLine(exception.StatusCode);

                if (exception.StatusCode == HttpStatusCode.Unauthorized)
                {
                    EL.GerenciadorException.GravarExcecao(exception, "Unauthorized: please verify if the credentials used in the web service call are correct.");
                }

                foreach (ServiceError error in exception.Errors)
                {
                    EL.GerenciadorException.GravarExcecao(exception, error.Message);
                }

            }
            #endregion Consultando status no PagSeguro

            AtualizarSituacaoPagSeguro(transaction, out objTransacao);
        }

        public static void AtualizarSituacaoPagSeguro(Transaction transaction, out Transacao objTransacao)
        {
            //Erro se não foi possível obter a transação
            if (transaction == null || String.IsNullOrEmpty(transaction.Reference))
            {
                EL.GerenciadorException.GravarExcecao(new Exception("Não foi possível obter a transação do pagseguro"));
            }

            //Recuperando Transação do BNE
            objTransacao = Transacao.LoadObject(Convert.ToInt32(transaction.Reference));

            //Gravando id da transação no pag seguro
            if (String.IsNullOrEmpty(objTransacao.DescricaoTransacao))
            {
                objTransacao.DescricaoTransacao = transaction.Code;
                objTransacao.Save();
            }

            bool transacaoAprovada = false;
            bool transacaoCancelada = false;
            String DescNaoAprovacao = null;
            String DescAprovacao = null;

            switch (transaction.TransactionStatus)
            {
                case 1: //Aguardando Pagamento
                    DescNaoAprovacao = "PagSeguro retornou o status AGUARDANDO LIBERAÇÃO";
                    break;
                case 2: //Em Análise
                    DescNaoAprovacao = "PagSeguro retornou o status EM ANÁLISE";
                    break;
                case 3: //Paga
                    DescAprovacao = "PagSeguro retornou o status PAGA";
                    transacaoAprovada = true;
                    break;
                case 4: //Disponível
                    DescAprovacao = "PagSeguro retornou o status DISPONÍVEL";
                    transacaoAprovada = true;
                    break;
                case 5: //Em disputa
                    DescNaoAprovacao = "PagSeguro retornou o status EM DISPUTA";
                    //Eviando e-mails de alerta
                    String assunto;

                    string emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);
                    string emailDestinatario = Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagSeguro_EmailAberturaDeDisputa);
                    String carta = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.PagSeguro_AvisoDeDisputa, out assunto);

                    objTransacao.PlanoAdquirido.CompleteObject();
                    objTransacao.PlanoAdquirido.Plano.CompleteObject();
                    objTransacao.PlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                    objTransacao.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CompleteObject();
                    carta = carta.Replace("{NomeUsuario}", objTransacao.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.NomeCompleto);
                    carta = carta.Replace("{CPFUsuario}", objTransacao.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.CPF.ToString());
                    carta = carta.Replace("{DescricaoPlano}", objTransacao.PlanoAdquirido.Plano.DescricaoPlano);
                    carta = carta.Replace("{CodigoPlano}", objTransacao.PlanoAdquirido.Plano.IdPlano.ToString());
                    carta = carta.Replace("{DataInicioPlano}", objTransacao.PlanoAdquirido.DataInicioPlano.ToString("d/MM/yyyy"));
                    carta = carta.Replace("{DataTerminoPlano}", objTransacao.PlanoAdquirido.DataFimPlano.ToString("d/MM/yyyy"));
                    carta = carta.Replace("{ValorPago}", objTransacao.ValorDocumento.ToString("N2", CultureInfo.CreateSpecificCulture("pt-BR")));

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(assunto, carta,Enumeradores.CartaEmail.PagSeguro_AvisoDeDisputa, emailRemetente, emailDestinatario);
                    break;
                case 6: //Devolvida
                    DescNaoAprovacao = "PagSeguro retornou o status DEVOLVIDA";
                    transacaoCancelada = true;
                    break;
                case 7: //Cancelada
                    DescNaoAprovacao = "PagSeguro retornou o status CANCELADA";
                    transacaoCancelada = true;
                    break;
                default:
                    break;
            }

            objTransacao.GravarRetorno(transacaoAprovada, DescNaoAprovacao, DescAprovacao);

            if (transacaoAprovada)
            {
                objTransacao.AtualizarStatus(Enumeradores.StatusTransacao.Aprovada);
                //Liberar plano
                objTransacao.PlanoAdquirido.CompleteObject();
                if (objTransacao.PlanoAdquirido.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.AguardandoLiberacao)
                {
                    //Se o plano adquirido ainda está aguardando liberação
                    objTransacao.Pagamento.CompleteObject();
                    objTransacao.Pagamento.DescricaoIdentificador = objTransacao.DescricaoTransacao;
                    objTransacao.Pagamento.Liberar(DateTime.Now);
                }
                else
                {
                    //Caso o plano esteja com a situação liberado, faz alterações para garantir a consistência da base
                    objTransacao.PlanoAdquirido.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado);
                    objTransacao.PlanoAdquirido.Plano.CompleteObject();
                    if (objTransacao.PlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                    {
                        objTransacao.PlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                        Curriculo objCurriculo;
                        Curriculo.CarregarPorPessoaFisica(objTransacao.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo);
                        objCurriculo.FlagVIP = true;
                        objCurriculo.Save();
                    }
                }
            }
            if (transacaoCancelada)
            {
                objTransacao.AtualizarStatus(Enumeradores.StatusTransacao.Cancelada);

                objTransacao.Pagamento.CompleteObject();
                if (objTransacao.Pagamento.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.Pago)
                {
                    objTransacao.PlanoAdquirido.CompleteObject();

                    int? idCurriculo = null;

                    objTransacao.PlanoAdquirido.Plano.CompleteObject();
                    if (objTransacao.PlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                    {
                        objTransacao.PlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                        Curriculo objCurriculo;
                        Curriculo.CarregarPorPessoaFisica(objTransacao.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo);
                        idCurriculo = objCurriculo.IdCurriculo;
                    }

                    objTransacao.PlanoAdquirido.CancelarPlanoAdquirido(null, "Transação Cancelada/Devolvida pelo PagSeguro", true, idCurriculo);
                }
            }
        }

        #region GravarRetorno
        public void GravarRetorno(bool Aprovada, String DescMotivoNaoFinalizada, String DescAutorizacao)
        {
            TransacaoRetorno objTransacaoRetorno = new TransacaoRetorno
            {
                DataStatus = DateTime.Now,
                DescricaoMotivoNaoFinalizada = DescMotivoNaoFinalizada,
                DescricaoAutorizacao = DescAutorizacao,
                FlagAprovada = Aprovada,
                Transacao = this
            };

            objTransacaoRetorno.Save();
        }
        #endregion

        #endregion PagSeguro

        #region PayPal

        public static string CriarTransacaoPayPal(Pagamento objPagamento, PlanoAdquirido objPlanoAdquirido, int idPessoaFisica, string descricaoIP, string urlRedirecionamento)
        {
            string urlRetorno = null;

            #region Carregando objetos necessários à execução
            if (objPlanoAdquirido.Plano == null || objPlanoAdquirido.Plano.IdPlano <= 0)
            {
                objPlanoAdquirido.CompleteObject();
            }

            Plano objPlano = objPlanoAdquirido.Plano;
            if (objPlano.DescricaoPlano == null)
            {
                objPlano.CompleteObject();
            }

            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(idPessoaFisica);
            #endregion Carregando objetos necessários à execução

            Transacao objTransacao = Transacao.CriarTransacaoRedirecionamento(ref objPagamento, objPlanoAdquirido.IdPlanoAdquirido, descricaoIP);

            try
            {
                SetExpressCheckoutOperation SetExpressCheckout = PayPalApiFactory.instance.ExpressCheckout(
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PayPal_Usuario),
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PayPal_Senha),
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PayPal_Assinatura)
                ).SetExpressCheckout(urlRedirecionamento, urlRedirecionamento);

                SetExpressCheckout.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
                SetExpressCheckout.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;
                SetExpressCheckout.NoShipping = true;
                SetExpressCheckout.AllowNote = false;
                SetExpressCheckout.Email = objPessoaFisica.EmailPessoa;

                SetExpressCheckout.PaymentRequest(0).addItem(objPlano.DescricaoPlano, 1, Convert.ToDouble(objPagamento.ValorPagamento));
                SetExpressCheckout.PaymentRequest(0).CurrencyCode = CurrencyCode.BRAZILIAN_REAL;
                SetExpressCheckout.PaymentRequest(0).Amount = Convert.ToDouble(objPagamento.ValorPagamento);
                SetExpressCheckout.PaymentRequest(0).Custom = objTransacao.IdTransacao.ToString();
                SetExpressCheckout.PaymentRequest(0).NotifyUrl = urlRedirecionamento;

                String urlDominio = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente);
                if (urlRedirecionamento.Contains("dev.bne.com.br")
                    || urlRedirecionamento.Contains("teste.bne.com.br")
                    || urlRedirecionamento.Contains("localhost"))
                {
                    SetExpressCheckout.sandbox().execute(); //Executa a operação no Sandbox
                }
                else
                {
                    SetExpressCheckout.execute(); //Executa a operação em produção
                }


                objTransacao.DescricaoTransacao = SetExpressCheckout.Token;
                objTransacao.Save();

                urlRetorno = SetExpressCheckout.RedirectUrl;
            }
            catch (Exception exception)
            {
                EL.GerenciadorException.GravarExcecao(exception);
            }

            return urlRetorno;
        }

        public static void AtualizarSituacaoTransacaoPayPal(int idTransacao, String paymentStatus, out Transacao objTransacao)
        {
            objTransacao = null;
            try
            {

                objTransacao = Transacao.LoadObject(idTransacao);
                String idTransacaoPayPal = objTransacao.DescricaoTransacao;

                AtualizarSituacaoTransacaoPayPal(paymentStatus, ref objTransacao);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

        public static void AtualizarSituacaoPayPal(string idTransacaoPayPal, out Transacao objTransacao)
        {
            objTransacao = null;

            #region Consultando status no PayPal
            try
            {
                ExpressCheckoutApi ec = PayPalApiFactory.instance.ExpressCheckout(
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PayPal_Usuario),
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PayPal_Senha),
                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.PayPal_Assinatura)
                );

                String urlDominio = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente);
                if (urlDominio.Contains("dev.bne.com.br")
                    || urlDominio.Contains("teste.bne.com.br")
                    || urlDominio.Contains("localhost"))
                {
                    ec = ec.sandbox(); //Executa a operação no Sandbox
                }

                AtualizarSituacaoPayPal(ec, idTransacaoPayPal, out objTransacao);
            }
            catch (Exception exception)
            {
                EL.GerenciadorException.GravarExcecao(exception);
            }
            #endregion Consultando status no PayPal
        }

        public static void AtualizarSituacaoPayPal(ExpressCheckoutApi ec, String token, out Transacao objTransacao)
        {
            GetExpressCheckoutDetailsOperation getExpressCheckoutDetailsOperation = new GetExpressCheckoutDetailsOperation(ec, token);

            String urlDominio = Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLAmbiente);
            if (urlDominio.Contains("dev.bne.com.br")
                || urlDominio.Contains("teste.bne.com.br")
                || urlDominio.Contains("localhost"))
            {
                getExpressCheckoutDetailsOperation.sandbox().execute(); //Executa a operação no Sandbox
            }
            else
            {
                getExpressCheckoutDetailsOperation.execute();
            }

            //Erro se não foi possível obter a transação
            if (getExpressCheckoutDetailsOperation == null || String.IsNullOrEmpty(getExpressCheckoutDetailsOperation.Token))
            {
                EL.GerenciadorException.GravarExcecao(new Exception("Não foi possível obter a transação do PayPal"));
            }

            //Recuperando Transação do BNE
            objTransacao = Transacao.LoadObject(Convert.ToInt32(getExpressCheckoutDetailsOperation.PaymentRequest(0).Custom));
            objTransacao.Pagamento.CompleteObject();

            //Gravando id da transação no pag seguro
            if (String.IsNullOrEmpty(objTransacao.DescricaoTransacao))
            {
                objTransacao.DescricaoTransacao = getExpressCheckoutDetailsOperation.Token;
                objTransacao.Save();
            }

            DoExpressCheckoutPaymentOperation doExpressCheckout = new DoExpressCheckoutPaymentOperation(ec, token, getExpressCheckoutDetailsOperation.PayerId, PaymentAction.ORDER);

            doExpressCheckout.LocaleCode = LocaleCode.BRAZILIAN_PORTUGUESE;
            doExpressCheckout.CurrencyCode = CurrencyCode.BRAZILIAN_REAL;
            doExpressCheckout.PaymentRequest(0).Amount = Convert.ToDouble(objTransacao.Pagamento.ValorPagamento);
            doExpressCheckout.PaymentRequest(0).Action = PaymentAction.SALE;

            if (urlDominio.Contains("dev.bne.com.br")
                || urlDominio.Contains("teste.bne.com.br")
                || urlDominio.Contains("localhost"))
            {
                doExpressCheckout.sandbox().execute(); //Executa a operação no Sandbox
            }
            else
            {
                doExpressCheckout.execute();
            }

            AtualizarSituacaoTransacaoPayPal(doExpressCheckout.PaymentInfo(0).PaymentStatus.ToString(), ref objTransacao);
        }

        public static void AtualizarSituacaoTransacaoPayPal(String paymentStatus, ref Transacao objTransacao)
        {
            bool transacaoAprovada = false;
            bool transacaoCancelada = false;
            String DescAprovacao = String.Empty;
            String DescNaoAprovacao = String.Empty;

            switch (paymentStatus)
            {
                case "None":
                    DescNaoAprovacao = "PayPal retornou o status NÃO DEFINIDO para o pagamento";
                    break;
                case "CanceledReversal":
                    DescNaoAprovacao = "PayPal retornou o status REEMBOLSO CANCELADO para o pagamento";
                    transacaoAprovada = true;
                    break;
                case "Completed":
                    DescNaoAprovacao = "PayPal retornou o status COMPLETO para o pagamento";
                    transacaoAprovada = true;
                    break;
                case "Denied":
                    DescNaoAprovacao = "PayPal retornou o status NEGADO para o pagamento";
                    transacaoCancelada = true;
                    break;
                case "Expired":
                    DescNaoAprovacao = "PayPal retornou o status EXPIRADO para o pagamento";
                    transacaoCancelada = true;
                    break;
                case "Failed ":
                    DescNaoAprovacao = "PayPal retornou o status FALHA para o pagamento";
                    transacaoCancelada = true;
                    break;
                case "InProgress":
                    DescNaoAprovacao = "PayPal retornou o status EM PROGRESSO para o pagamento";
                    break;
                case "PartiallyRefunded":
                    DescNaoAprovacao = "PayPal retornou o status REEMBOLSADO PARCIALMENTE para o pagamento";
                    break;
                case "Refunded":
                    DescNaoAprovacao = "PayPal retornou o status REEMBOLSADO para o pagamento";
                    transacaoCancelada = true;
                    break;
                case "Reversed":
                    DescNaoAprovacao = "PayPal retornou o status REVERTIDO para o pagamento";
                    transacaoCancelada = true;
                    break;
                case "Processed":
                    DescNaoAprovacao = "PayPal retornou o status PROCESSADO para o pagamento";
                    break;
                case "Voided":
                    DescNaoAprovacao = "PayPal retornou o status NEGADO para o pagamento";
                    transacaoCancelada = true;
                    break;
                case "CompletedFundsHeld":
                    DescNaoAprovacao = "PayPal retornou o status SALDO LIBERADO para o pagamento";
                    transacaoAprovada = true;
                    break;
                default:
                    break;
            }

            objTransacao.GravarRetorno(transacaoAprovada, DescNaoAprovacao, DescAprovacao);

            if (transacaoAprovada)
            {
                //Liberar plano
                objTransacao.PlanoAdquirido.CompleteObject();
                if (objTransacao.PlanoAdquirido.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.AguardandoLiberacao)
                {
                    //Se o plano adquirido ainda está aguardando liberação
                    objTransacao.Pagamento.CompleteObject();
                    objTransacao.Pagamento.DescricaoIdentificador = objTransacao.DescricaoTransacao;
                    objTransacao.Pagamento.Liberar(DateTime.Now);
                    objTransacao.AtualizarStatus(Enumeradores.StatusTransacao.Aprovada);
                }
                else if (objTransacao.PlanoAdquirido.PlanoSituacao.IdPlanoSituacao != (int)Enumeradores.PlanoSituacao.Liberado)
                {
                    //Caso o plano não esteja aguardando liberação e nem liberado, faz alterações na base para garantir a persistência.
                    objTransacao.PlanoAdquirido.PlanoSituacao = new PlanoSituacao((int)Enumeradores.PlanoSituacao.Liberado);
                    objTransacao.PlanoAdquirido.Plano.CompleteObject();
                    if (objTransacao.PlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                    {
                        objTransacao.PlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                        Curriculo objCurriculo;
                        Curriculo.CarregarPorPessoaFisica(objTransacao.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo);
                        objCurriculo.FlagVIP = true;
                        objCurriculo.Save();
                    }
                }
            }
            if (transacaoCancelada)
            {
                objTransacao.AtualizarStatus(Enumeradores.StatusTransacao.Cancelada);

                objTransacao.PlanoAdquirido.CompleteObject();

                if (objTransacao.PlanoAdquirido.PlanoSituacao.IdPlanoSituacao == (int)Enumeradores.PlanoSituacao.Liberado)
                {
                    objTransacao.Pagamento.CompleteObject();

                    if (objTransacao.Pagamento.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.Pago)
                    {
                        objTransacao.PlanoAdquirido.CompleteObject();

                        int? idCurriculo = null;

                        objTransacao.PlanoAdquirido.Plano.CompleteObject();
                        if (objTransacao.PlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                        {
                            objTransacao.PlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                            Curriculo objCurriculo;
                            Curriculo.CarregarPorPessoaFisica(objTransacao.PlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo);
                            idCurriculo = objCurriculo.IdCurriculo;
                        }

                        objTransacao.PlanoAdquirido.CancelarPlanoAdquirido(null, "Transação Cancelada/Devolvida pelo PayPal", true, idCurriculo);
                    }

                }
            }
        }


        #endregion PayPal

        #endregion

    }
}
