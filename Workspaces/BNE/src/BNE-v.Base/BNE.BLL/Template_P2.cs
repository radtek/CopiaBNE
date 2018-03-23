//-- Data: 21/02/2011 09:00
//-- Autor: Elias

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.EL;
namespace BNE.BLL
{
	public partial class Template // Tabela: BNE_Template
	{
        private const string SPSELECTPORNOME = "SELECT * FROM BNE_Template WHERE Nme_Template = @Nme_Template";


        #region CarregarPorNome
        /// <summary>   
        /// Método responsável por carregar uma instância de Template pelo nome do template
        /// </summary>
        /// <param name="strNomeTemplate">Nome do template</param>
        /// <returns>Objeto Template</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Template CarregarPorNome(String strNomeTemplate)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Nme_Template", SqlDbType.VarChar, 100));
            parms[0].Value = strNomeTemplate;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORNOME, parms))
            {
                Template objTemplate = new Template();
                if (SetInstance(dr, objTemplate))
                    return objTemplate;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw (new RecordNotFoundException(typeof(Template)));
        }
        /// <summary>   
        /// Método responsável por carregar uma instância de Template pelo nome do template
        /// </summary>
        /// <param name="strNomeTemplate">Nome do Template</param>
        /// <returns>Objeto Template</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorNome(String strNomeTemplate, out Template objTemplate)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Nme_Template", SqlDbType.VarChar, 100));
            parms[0].Value = strNomeTemplate;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORNOME, parms))
            {
                objTemplate = new Template();
                if (SetInstance(dr, objTemplate))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objTemplate = null;
            return false;
        }
        #endregion

    }
}