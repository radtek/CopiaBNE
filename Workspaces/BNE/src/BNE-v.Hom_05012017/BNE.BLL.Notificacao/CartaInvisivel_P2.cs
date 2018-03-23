//-- Data: 15/02/2013 10:12
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.Notificacao
{
    public partial class CartaInvisivel // Tabela: TAB_Carta_Invisivel
    {

        public static Dictionary<Enumeradores.Carta, string> Recuperar(List<Enumeradores.Carta> cartas)
        {
            var parms = new List<SqlParameter>();
            var itens = new Dictionary<Enumeradores.Carta, string>();

            string query = "SELECT Idf_Carta_Invisivel, Vlr_Carta_Invisivel FROM TAB_Carta_Invisivel WITH(NOLOCK) WHERE Idf_Carta_Invisivel IN (";

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
                var carta = (Enumeradores.Carta)Enum.Parse(typeof(Enumeradores.Carta), dr["Idf_Carta_Invisivel"].ToString());
                itens.Add(carta, Convert.ToString(dr["Vlr_Carta_Invisivel"]));
            }
            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return itens;
        }
    }
}