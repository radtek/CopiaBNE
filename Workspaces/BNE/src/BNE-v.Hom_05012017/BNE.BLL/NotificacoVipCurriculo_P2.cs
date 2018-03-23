//-- Data: 23/08/2016 10:36
//-- Autor: Marty Sroka
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class NotificacaoVipCurriculo // Tabela: BNE_Notificacao_Vip_Curriculo
	{
        #region Consultas

        #region SpSelectPorCurriculo
        private const string SpSelectPorCurriculo = @"
            SELECT 
                * 
            FROM BNE_Notificacao_Vip_Curriculo WITH(NOLOCK) 
            WHERE 
                Idf_Curriculo = @Idf_Curriculo

        ";
        #endregion

        #region SpSelectQtdMesmaNotificacoes
        private const string SpSelectQtdMesmaNotificacoes = @"
            SELECT 
                COUNT(*) 
            FROM 
                BNE.BNE_Notificacao_Vip_Curriculo WITH(NOLOCK)
            WHERE 
                    Idf_Curriculo = @Idf_Curriculo 
                AND Idf_Notificacao_Vip = @Idf_Notificacao_Vip
                AND Idf_Status_Notificacao_VIP = 4
        ";
        #endregion


        #endregion

        #region Metodos

        #region RetornarTodos
        public static List<NotificacaoVipCurriculo> RetornarTodosPorCurriculo(int idCurriculo)
        {
            List<NotificacaoVipCurriculo> notificacoesCV = new List<NotificacaoVipCurriculo>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idCurriculo;

            var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectPorCurriculo, parms);
            while (dr.Read())
                notificacoesCV.Add(SetInstanceNotDisposable(dr));

            return notificacoesCV;
        }
        #endregion

        #region RetornarQuantidadeEnvioMesmaNotificacao
        public static int RetornarQuantidadeEnvioMesmaNotificacao(int idCurriculo, int idNotificacao)
        {
            List<NotificacaoVipCurriculo> notificacoesCV = new List<NotificacaoVipCurriculo>();

            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter(){ ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo});
            parms.Add(new SqlParameter() { ParameterName = "@Idf_Notificacao_Vip", SqlDbType = SqlDbType.Int, Size = 4, Value = idNotificacao });


            var quantidade = DataAccessLayer.ExecuteScalar(CommandType.Text, SpSelectQtdMesmaNotificacoes, parms);
            if (quantidade != null)
                return Convert.ToInt32(quantidade);

            return 0;
        }
        #endregion

        #region GravarNotificacao
        public static void GravarNotificacao(int idCurriculo, short idNotificacao, short idStatusNotificacao, string observacoes = null)
        {
            var novaNotificacao = new NotificacaoVipCurriculo()
            {
                StatusNotificacaoVIP = new StatusNotificacaoVIP(idStatusNotificacao),
                NotificacaoVip = new NotificacaoVip(idNotificacao),
                Curriculo = new Curriculo(idCurriculo),
                DescricaoObservacao = observacoes
            };
            novaNotificacao.Save();
        }
        #endregion

        #region SetInstanceNotDisposable
        private static NotificacaoVipCurriculo SetInstanceNotDisposable(IDataReader dr)
        {
            try
            {
                var objNotificacaoVipCurriculo = new NotificacaoVipCurriculo();

                objNotificacaoVipCurriculo._idNotificacaoVipCurriculo = Convert.ToInt32(dr["Idf_Notificacao_Vip_Curriculo"]);
                objNotificacaoVipCurriculo._curriculo = new Curriculo(Convert.ToInt32(dr["Idf_Curriculo"]));
                objNotificacaoVipCurriculo._notificacaoVip = new NotificacaoVip(Convert.ToInt16(dr["Idf_Notificacao_Vip"]));
                objNotificacaoVipCurriculo._statusNotificacaoVIP = new StatusNotificacaoVIP(Convert.ToInt16(dr["Idf_Status_Notificacao_VIP"]));
                objNotificacaoVipCurriculo._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

                if (dr["Dta_Cadastro"] != DBNull.Value)
                    objNotificacaoVipCurriculo._descricaoObservacao = dr["Des_Observacao"].ToString();

                objNotificacaoVipCurriculo._persisted = true;
                objNotificacaoVipCurriculo._modified = false;

                return objNotificacaoVipCurriculo;
            }
            catch
            {
                throw;
            }

        }
        #endregion

        #endregion
	}
}