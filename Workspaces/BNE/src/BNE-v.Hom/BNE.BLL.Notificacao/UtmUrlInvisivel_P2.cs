//-- Data: 05/08/2016 10:59
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
	public partial class UtmUrlInvisivel // Tabela: alerta.TAB_UtmUrl_Invisivel
    {
        public static Dictionary<Enumeradores.UtmUrl, string> Recuperar(List<Enumeradores.UtmUrl> utmsUrls)
        {
            var parms = new List<SqlParameter>();
            var itens = new Dictionary<Enumeradores.UtmUrl, string>();

            string query = "SELECT Idf_UtmUrl_Invisivel, Vlr_UtmUrl_Invisivel FROM TAB_UtmUrl_Invisivel WITH(NOLOCK) WHERE Idf_UtmUrl_Invisivel IN (";

            for (int i = 0; i < utmsUrls.Count; i++)
            {
                string nomeConteudo = "@parm" + i;

                if (i > 0)
                    query += ", ";

                query += nomeConteudo;
                parms.Add(new SqlParameter(nomeConteudo, SqlDbType.Int, 4));
                parms[i].Value = Convert.ToInt32(utmsUrls[i]);
            }

            query += ")";

            IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, parms);

            while (dr.Read())
            {
                var utmUrl = (Enumeradores.UtmUrl)Enum.Parse(typeof(Enumeradores.UtmUrl), dr["Idf_UtmUrl_Invisivel"].ToString());
                itens.Add(utmUrl, Convert.ToString(dr["Vlr_UtmUrl_Invisivel"]));
            }
            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return itens;
        }
    }
}