//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL.Common.Sitemap;

namespace BNE.BLL
{
    public partial class Origem // Tabela: TAB_Origem
    {

        #region Consultas

        private const string SPORIGEMPORFILIAL = @"
        SELECT  * 
        FROM    BNE.TAB_Origem O WITH(NOLOCK)
                JOIN BNE.TAB_Origem_Filial TOF WITH(NOLOCK) ON O.Idf_Origem = TOF.Idf_Origem
        WHERE   TOF.Idf_Filial = @Idf_Filial 
                AND TOF.Flg_Inativo = 0 
                AND O.Flg_Inativo = 0";

        #region SpRecuperarOrigensSiteMap
        private const string SpRecuperarOrigensSiteMap = @"
        SELECT  OrF.Des_Diretorio
        FROM    BNE.TAB_Origem O WITH (NOLOCK)
                INNER JOIN BNE.TAB_Origem_Filial OrF WITH (NOLOCK) ON O.Idf_Origem = OrF.Idf_Origem
        ORDER BY O.Idf_Origem
        ";
        #endregion

        #endregion

        #region OrigemPorFilial
        public static bool OrigemPorFilial(int idFilial, out Origem objOrigem)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Filial", SqlDbType.Int, 4));
            parms[0].Value = idFilial;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPORIGEMPORFILIAL, parms))
            {
                objOrigem = new Origem();
                if (SetInstance(dr, objOrigem))
                    return true;
                return false;
            }
        }
        #endregion

        #region RecuperarOrigensSiteMap
        public static IEnumerable<EmpresaSitemap> RecuperarOrigensSiteMap()
        {
            var listSiteMap = new List<EmpresaSitemap>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpRecuperarOrigensSiteMap, null))
            {
                while (dr.Read())
                {
                    listSiteMap.Add(new EmpresaSitemap
                                        {
                                            DesDiretorio = dr["Des_Diretorio"].ToString()
                                        });
                }
            }

            return listSiteMap;
        }
        #endregion

    }
}