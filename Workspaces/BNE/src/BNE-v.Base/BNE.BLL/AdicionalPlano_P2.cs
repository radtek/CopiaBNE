//-- Data: 09/09/2011 09:39
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class AdicionalPlano // Tabela: BNE_Adicional_Plano
    {

        #region Consultas

        #region SPADICIONAISPORPLANOADQUIRIDO
        private const string SPADICIONAISPORPLANOADQUIRIDO = @"
        DECLARE @FirstRec INT
        DECLARE @LastRec INT
        DECLARE @iSelect VARCHAR(8000)
        DECLARE @iSelectCount VARCHAR(8000)
        DECLARE @iSelectPag VARCHAR(8000)
        
        SET @FirstRec = ( @CurrentPage * @PageSize + 1 )
        SET @LastRec = ( @CurrentPage * @PageSize ) + @PageSize
        
        --SET @FirstRec = ( @CurrentPage - 1 ) * @PageSize
        --SET @LastRec = ( @CurrentPage * @PageSize + 1 )

        SET @iSelect = '
        SELECT	
                ROW_NUMBER() OVER (ORDER BY AP.Idf_Adicional_Plano) AS RowID ,        
                AP.Idf_Adicional_Plano ,            
                TA.Des_Tipo_Adicional ,
                AP.Qtd_Adicional ,
                AP.Idf_Adicional_Plano_Situacao ,
                APS.Des_Adicional_Plano_Situacao ,
                PG.Idf_Pagamento ,
                PG.Vlr_Pagamento ,
                 CASE WHEN pg.Idf_Tipo_Pagamento = 2 THEN PG.Des_Identificador
                        ELSE cc.Cod_TID
                    END AS Des_Identificador ,
                PG.Dta_Emissao ,
                PG.Dta_Vencimento ,
                PS.Idf_Pagamento_Situacao ,
                CASE WHEN PG.Idf_Tipo_Pagamento = 2 THEN PS.Des_Pagamento_Situacao
                        ELSE ISNULL(RC.Des_Mensagem,'+CHAR(39)+'NULO'+CHAR(39)+')
                    END AS Des_Pagamento_Situacao  ,
                PG.Idf_Tipo_Pagamento ,
                TP.Des_Tipo_Pagamaneto
        FROM BNE_Adicional_Plano AP WITH (NOLOCK)
            INNER JOIN BNE_Tipo_Adicional TA WITH (NOLOCK) ON AP.Idf_Tipo_Adicional = TA.Idf_Tipo_Adicional
            INNER JOIN BNE_Pagamento PG WITH (NOLOCK) ON AP.Idf_Adicional_Plano = PG.Idf_Adicional_Plano
            INNER JOIN BNE_Adicional_Plano_Situacao APS WITH (NOLOCK) ON AP.Idf_Adicional_Plano_Situacao = APS.Idf_Adicional_Plano_Situacao
            LEFT JOIN BNE_Pagamento_Situacao PS WITH (NOLOCK) ON PG.Idf_Pagamento_Situacao = PS.Idf_Pagamento_Situacao
            LEFT JOIN BNE_Tipo_Pagamento TP WITH (NOLOCK) ON TP.Idf_Tipo_Pagamento = PG.Idf_Tipo_Pagamento
            LEFT JOIN GLO_Transacao TR WITH (NOLOCK) ON TR.Cod_GUID = PG.COD_Guid
            LEFT JOIN GLO_Cobranca_Cartao CC WITH (NOLOCK) ON TR.Idf_Transacao = CC.Idf_Transacao
            LEFT JOIN GLO_Mensagem_Retorno_Cartao RC WITH (NOLOCK) ON CC.Idf_Mensagem_Retorno_Cartao = RC.Idf_Mensagem_Retorno_Cartao
        WHERE	AP.Idf_Plano_Adquirido = ' + CONVERT(VARCHAR, @Idf_Plano_Adquirido) + ' 
        AND     AP.Flg_Inativo = 0'

        SET @iSelectCount = 'Select Count(*) From ( ' + @iSelect + ' ) As TblTempCount'
        SET @iSelectPag = 'Select * From ( ' + @iSelect + ' ) As TblTempPag	Where RowID >= ' + CONVERT(VARCHAR, @FirstRec) + ' And RowID <= ' + CONVERT(VARCHAR, @LastRec)
        EXECUTE (@iSelectCount)
        EXECUTE (@iSelectPag)";
        #endregion

        #endregion

        #region Métodos

        #region ListarAdicionaisPorPlanoAdquirido
        public static DataTable ListarAdicionaisPorPlanoAdquirido(PlanoAdquirido objPlanoAdquirido, int paginaAtual, int tamanhoPagina, out int totalRegistros)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@CurrentPage", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@PageSize", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Plano_Adquirido", SqlDbType.Int, 4));

            parms[0].Value = paginaAtual;
            parms[1].Value = tamanhoPagina;
            parms[2].Value = objPlanoAdquirido.IdPlanoAdquirido;

            DataTable dt = null;
            totalRegistros = 0;
            try
            {
                using (SqlDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPADICIONAISPORPLANOADQUIRIDO, parms))
                {
                    if (dr.Read())
                        totalRegistros = Convert.ToInt32(dr[0]);

                    dr.NextResult();
                    dt = new DataTable();
                    dt.Load(dr);
                }
            }
            finally
            {
                if (dt != null)
                    dt.Dispose();
            }

            return dt;
        }
        #endregion

        #region CriarPagamentoEPlanoAdicionalSMS
        /// <summary>
        /// Método responsável por criar um adicional para o plano adquirido e um pagamento para o plano adicional.
        /// </summary>
        /// <param name="objPlanoAdquirido"></param>
        /// <param name="valorTotal"></param>
        /// <param name="quantidadeAdquirida"></param>
        /// <param name="objUsuarioFilialPerfil"></param>
        /// <param name="tipoPagamento"></param>
        /// <param name="dtaEmissao"></param>
        /// <param name="dtaVencimento"></param>
        /// <param name="objPagamento"></param>
        public static void CriarPagamentoEPlanoAdicionalSMS(PlanoAdquirido objPlanoAdquirido, decimal valorTotal, int quantidadeAdquirida, UsuarioFilialPerfil objUsuarioFilialPerfil,
            Enumeradores.TipoPagamento tipoPagamento, DateTime dtaEmissao, DateTime dtaVencimento, out BLL.Pagamento objPagamento)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        //Criando um novo adicional para o plano adquirido
                        var objAdicionalPlano = new AdicionalPlano
                            {
                                PlanoAdquirido = objPlanoAdquirido,
                                TipoAdicional = new TipoAdicional((int)Enumeradores.TipoAdicional.SMSAdicional),
                                AdicionalPlanoSituacao = new AdicionalPlanoSituacao((int)Enumeradores.AdicionalPlanoSituacao.AguardandoLiberacao),
                                QuantidadeAdicional = quantidadeAdquirida,
                                FlagInativo = false
                            };
                        objAdicionalPlano.Save(trans);

                        objPagamento = new Pagamento
                            {
                                Filial = objPlanoAdquirido.Filial,
                                UsuarioFilialPerfil = objUsuarioFilialPerfil,
                                UsuarioGerador = objUsuarioFilialPerfil,
                                TipoPagamento = new TipoPagamento((int)tipoPagamento),
                                DataEmissao = dtaEmissao,
                                DataVencimento = dtaVencimento,
                                ValorPagamento = valorTotal,
                                FlagAvulso = false,
                                FlagInativo = false,
                                PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.EmAberto),
                                AdicionalPlano = objAdicionalPlano
                            };

                        objPagamento.Save(trans);

                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }
        #endregion

        #region LiberarPlanoAdicional
        /// <summary>
        /// Libera plano adicional e, caso seja de SMS, recarrega SMS
        /// </summary>
        public void LiberarPlanoAdicional()
        {
            using (SqlConnection conn = new SqlConnection(DataAccessLayer.CONN_STRING))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    try
                    {
                        this.LiberarPlanoAdicional(trans);
                        trans.Commit();
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Libera plano adicional e, caso seja de SMS, recarrega SMS
        /// </summary>
        /// <param name="trans"></param>
        public void LiberarPlanoAdicional(SqlTransaction trans)
        {
            this.CompleteObject(trans);

            if (this.TipoAdicional.IdTipoAdicional.Equals((int)Enumeradores.TipoAdicional.SMSAdicional))
            {
                PlanoQuantidade.RecarregarSMS(this, trans);
            }
            AdicionalPlanoSituacao = new AdicionalPlanoSituacao((int)Enumeradores.AdicionalPlanoSituacao.Liberado);
            Save(trans);
        }

        #endregion

        #region CancelarPlanoAdicional
        public void CancelarPlanoAdicional(SqlTransaction trans)
        {
            this.CompleteObject(trans);

            if (this.AdicionalPlanoSituacao.IdAdicionalPlanoSituacao.Equals((int)Enumeradores.AdicionalPlanoSituacao.Liberado)
                && this.TipoAdicional.IdTipoAdicional.Equals((int)Enumeradores.TipoAdicional.SMSAdicional))
            {
                // troca o sinal da quantidade adicional, para descarregar SMS (soma algébrica)
                this.QuantidadeAdicional *= (-1);
                PlanoQuantidade.RecarregarSMS(this, trans);
                this.QuantidadeAdicional *= (-1);
            }
            AdicionalPlanoSituacao = new AdicionalPlanoSituacao((int)Enumeradores.AdicionalPlanoSituacao.Cancelado);
            Save(trans);
        }
        #endregion

        #endregion

    }
}