//-- Data: 16/01/2017 14:05
//-- Autor: Ramalho

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public partial class CartaVip // Tabela: TAB_Carta_VIP
    {

        public static Dictionary<Enumeradores.Carta, string> Recuperar(List<Enumeradores.Carta> cartas)
        {
            var parms = new List<SqlParameter>();
            var itens = new Dictionary<Enumeradores.Carta, string>();

            string query = "SELECT Idf_Carta_VIP, Vlr_Carta_VIP FROM TAB_Carta_VIP WITH(NOLOCK) WHERE Idf_Carta_VIP IN (";

            for (int i = 0; i < cartas.Count; i++)
            {
                string nomeConteudo = "@parm" + i;

                if (i > 0)
                    query += ", ";

                query += nomeConteudo;
                parms.Add(new SqlParameter(nomeConteudo, SqlDbType.Int, 4));
                parms[i].Value = Convert.ToInt32(cartas[i]);
            }

            query += ")";

            IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, parms);

            while (dr.Read())
            {
                var carta = (Enumeradores.Carta)Enum.Parse(typeof(Enumeradores.Carta), dr["Idf_Carta_VIP"].ToString());
                itens.Add(carta, Convert.ToString(dr["Vlr_Carta_VIP"]));
            }
            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return itens;
        }
    }
}