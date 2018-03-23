//-- Data: 21/01/2014 10:33
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using BNE.BLL.Common.Sitemap;

namespace BNE.BLL.CloudTag
{
    public partial class PalavraFuncaoVaga // Tabela: TAB_Palavra_Funcao_Vaga
    {

        #region Consultas

        #region SpRecuperarPalavrasSitemap
        private const string SpRecuperarPalavrasSitemap = @"SPRecuperarPalavrasSitemap";
        #endregion

        #endregion

        #region RecuperarPalavrasSitemap
        public static IEnumerable<VagaSitemap> RecuperarPalavrasSitemap()
        {
            var listSiteMap = new List<VagaSitemap>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, SpRecuperarPalavrasSitemap, null))
            {
                while (dr.Read())
                {
                    listSiteMap.Add(new VagaSitemap
                    {
                        PalavraChave = dr["Des_Palavra"].ToString().Trim()
                    });
                }
            }

            return listSiteMap;
        }
        #endregion

    }
}