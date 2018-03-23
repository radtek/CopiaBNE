//-- Data: 01/07/2015 15:40
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using iTextSharp.text;

namespace BNE.BLL
{
    public partial class TemplateContrato // Tabela: plataforma.TAB_Template_Contrato
    {

        #region Consultas

        #region Spcontratovigente
        private const string Spcontratovigente = @"
        SELECT  TC.*
        FROM    plataforma.TAB_Template_Contrato TC
        WHERE   TC.Flg_Inativo = 0
        AND		TC.Idf_Template_Contrato =  @idf_Template_Contrato";
        #endregion

        #endregion

        #region M�todos

        #region RecuperarContratoVigente
        /// <summary>
        /// Retorna o template vigente.
        /// </summary>
        /// <returns></returns>
        public static TemplateContrato RecuperarContratoVigente(Enumeradores.TemplateContrato templateContrato, SqlTransaction trans = null)
        {
            var objTemplateContrato = new TemplateContrato();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                new SqlParameter("@idf_template_contrato", SqlDbType.Int, 4)
            };
            parametros[0].Value = (int) templateContrato;

            using (var dr = trans != null ? DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spcontratovigente, parametros) : DataAccessLayer.ExecuteReader(CommandType.Text, Spcontratovigente, parametros))
            {
                if (!SetInstance(dr, objTemplateContrato))
                    objTemplateContrato = null;
            }

            return objTemplateContrato;
        }
        #endregion

     
        #endregion

    }
}