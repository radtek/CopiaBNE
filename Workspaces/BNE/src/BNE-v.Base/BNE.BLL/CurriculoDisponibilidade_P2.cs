//-- Data: 06/04/2011 17:26
//-- Autor: Vinicius Maciel

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class CurriculoDisponibilidade // Tabela: BNE_Curriculo_Disponibilidade
    {
        #region Querys

        private const string LISTARPORCURRICULO = @"SELECT * FROM BNE_Curriculo_Disponibilidade WHERE Idf_Curriculo = @Idf_Curriculo";

        private const string DELETEPORCURRICULO = @"DELETE BNE_Curriculo_Disponibilidade WHERE Idf_Curriculo = @Idf_Curriculo";

        #endregion

        #region Metodos

        #region Listar
        /// <summary>
        /// Lista CurriculoDisponibilidade por Curriculo
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <returns></returns>
        public static List<CurriculoDisponibilidade> Listar(int idCurriculo)
        {
            //TODO: Melhorar código, tirar load object interno
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idCurriculo;

            List<CurriculoDisponibilidade> lstCurriculoDisponibilidade = new List<CurriculoDisponibilidade>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, LISTARPORCURRICULO, parms))
            {
                while (dr.Read())
                    lstCurriculoDisponibilidade.Add(CurriculoDisponibilidade.LoadObject(Convert.ToInt32(dr["Idf_Curriculo_Disponibilidade"])));

                if (!dr.IsClosed)
                    dr.Dispose();

                dr.Close();
            }


            return lstCurriculoDisponibilidade;
        }

        #endregion

        #region DeletePorCurriculo
        /// <summary>
        /// Deleta Curriculo disponibilidade por id curriculo na transação
        /// </summary>
        /// <param name="idCurriculo"></param>
        /// <param name="trans"></param>
        public static void DeletePorCurriculo(int idCurriculo, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idCurriculo;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, DELETEPORCURRICULO, parms);
        }

        #endregion

        #endregion
    }
}