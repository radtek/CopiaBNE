//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL.Common.Sitemap;
using System;

namespace BNE.BLL
{
    public partial class Origem // Tabela: TAB_Origem
    {

        #region Consultas

        #region SpRecuperarOrigensSiteMap
        private const string SpRecuperarOrigensSiteMap = @"
        SELECT  OrF.Des_Diretorio
        FROM    BNE.TAB_Origem O WITH (NOLOCK)
                INNER JOIN BNE.TAB_Origem_Filial OrF WITH (NOLOCK) ON O.Idf_Origem = OrF.Idf_Origem
        ORDER BY O.Idf_Origem
        ";
        #endregion


        #region ORIGEM_POR_DESCRICAO_QRY
        private const string ORIGEM_POR_DESCRICAO_QRY = @" SELECT TOP 1 * FROM BNE.TAB_Origem WHERE Des_Origem = @Des_Origem ";
        #endregion

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


        #region RecuperarPorDescricao
        public bool RecuperarPorDescricao(string Descricao, SqlTransaction trans = null) 
        {
            try
            {
                List<SqlParameter> parms = new List<SqlParameter>()
                {
                    new SqlParameter(){ ParameterName = "Des_Origem", SqlDbType = SqlDbType.VarChar, Value = Descricao}
                };

                using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, ORIGEM_POR_DESCRICAO_QRY, parms))
                {
                    return SetInstance(dr, this);
                }
            }
            catch (Exception) { return false; }
        }
        #endregion
    }
}