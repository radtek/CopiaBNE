//-- Data: 23/08/2016 10:36
//-- Autor: Marty Sroka

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class NotificacaoVip // Tabela: BNE_Notificacao_Vip
	{
        #region Consultas

        #region SpSelectAll
        private const string SpSelectAll = @"
            SELECT * FROM BNE_Notificacao_Vip WITH(NOLOCK) WHERE Flg_Inativo = 0
        ";
        #endregion

        #endregion

        #region Metodos

        #region RetornarTodos
        public static List<NotificacaoVip> RetornarTodos()
        {
            List<NotificacaoVip> notificacoes = new List<NotificacaoVip>();

            var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectAll, null);
            while (dr.Read())
                notificacoes.Add(SetInstanceNotDisposable(dr));

            return notificacoes;
        }
        #endregion

        #region SetInstanceNotDisposable
        private static NotificacaoVip SetInstanceNotDisposable(IDataReader dr)
        {
            try
            {
                var objNotificacaoVip = new NotificacaoVip();

                objNotificacaoVip._idNotificacaoVip = Convert.ToInt16(dr["Idf_Notificacao_Vip"]);
                objNotificacaoVip._descricaoNotificacaoVip = Convert.ToString(dr["Des_Notificacao_Vip"]);
                objNotificacaoVip._numeroDia = Convert.ToInt16(dr["Num_Dia"]);
                objNotificacaoVip._descricaoClasseResponsavel = Convert.ToString(dr["Des_Classe_Responsavel"]);
                objNotificacaoVip._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

                objNotificacaoVip._persisted = true;
                objNotificacaoVip._modified = false;

                return objNotificacaoVip;
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