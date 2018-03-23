//-- Data: 04/05/2015 16:56
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PlanoAdquiridoDetalhesCurriculo // Tabela: BNE_Plano_Adquirido_Detalhes_Curriculo
    {

        #region Consultas

        #region SpRecebeuCampanhaVagaPerfil
        private const string SpRecebeuCampanhaVagaPerfil = @"
        SELECT COUNT(*) 
        FROM BNE_Plano_Adquirido_Detalhes_Curriculo PADC 
            INNER JOIN  BNE_Plano_Adquirido_Detalhes PAD ON PADC.Idf_Plano_Adquirido_Detalhes = PAD.Idf_Plano_Adquirido_Detalhes
        WHERE Idf_Curriculo = @Idf_Curriculo AND Idf_Vaga = @Idf_Vaga";
        #endregion

        #endregion

        #region Métodos

        #region RecebeuCampanhaVagaPerfil
        /// <summary>
        /// Verifica se o curriculo em questão recebeu campanha por estar no perfil de uma vaga. Processo de compra de campanha para curriculo.
        /// </summary>
        /// <param name="objCurriculo"></param>
        /// <param name="objVaga"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static bool RecebeuCampanhaVagaPerfil(BLL.Curriculo objCurriculo, Vaga objVaga, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objCurriculo.IdCurriculo },
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga }
                };

            if (trans != null)
                return Convert.ToInt32(DataAccessLayer.ExecuteScalar(trans, CommandType.Text, SpRecebeuCampanhaVagaPerfil, parms)) > 0;

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, SpRecebeuCampanhaVagaPerfil, parms)) > 0;
        }
        #endregion

        #endregion

    }
}