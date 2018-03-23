//-- Data: 22/06/2010 12:18
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System;
namespace BNE.BLL
{
    public partial class RastreadorCurriculo // Tabela: BNE_Rastreador_Curriculo
    {

        #region Consultas

        #region Spselectcandidatosporrastreador
        private const string Spselectcandidatosporrastreador = @" SELECT Idf_Curriculo FROM BNE_Rastreador_Curriculo WITH(NOLOCK) WHERE Idf_Rastreador = @Idf_Rastreador";
        #endregion

        #region Spselectporrastreadorcurriculo
        private const string Spselectporrastreadorcurriculo = @"
            SELECT * FROM BNE_Rastreador_Curriculo WITH(NOLOCK) WHERE Idf_Rastreador = @Idf_Rastreador AND Idf_Curriculo = @Idf_Curriculo";
        #endregion

        private const string Spdeleteporrastreador = "DELETE FROM BNE_Rastreador_Curriculo WHERE Idf_Rastreador = @Idf_Rastreador";

        #endregion

        #region ListarCurriculosPorRastreador
        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static List<int> ListarCurriculosPorRastreador(Rastreador objRastreador)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4)
                };
            parms[0].Value = objRastreador.IdRastreador;

            var idfsCurriculos = new List<int>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcandidatosporrastreador, parms))
            {
                while (dr.Read())
                    idfsCurriculos.Add(Convert.ToInt32(dr["Idf_Curriculo"]));
            }

            return idfsCurriculos;
        }
        #endregion

        #region CarregarPorRastreadorCurriculo
        /// <summary>   
        /// Método responsável por carregar uma instância de PessoaFisica pelo parâmetro CPF.
        /// </summary>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorRastreadorCurriculo(int idRastreador, int idCurriculo, out RastreadorCurriculo objRastreadorCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4),
                    new SqlParameter("@Idf_Curriculo", SqlDbType.Int, 4)
                };
            parms[0].Value = idRastreador;
            parms[1].Value = idCurriculo;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporrastreadorcurriculo, parms))
            {
                objRastreadorCurriculo = new RastreadorCurriculo();
                if (SetInstance(dr, objRastreadorCurriculo))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objRastreadorCurriculo = null;
            return false;
        }
        #endregion

        #region DeletePorRastreador
        /// <summary>
        /// Método utilizado para excluir todas as VagaDisponibilidade ligadas a uma vaga dentro de uma transação
        /// </summary>
        /// <param name="idRastreador"> </param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson</remarks>
        public static void DeletePorRastreador(int idRastreador, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4)
                };

            parms[0].Value = idRastreador;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spdeleteporrastreador, parms);
        }
        #endregion

	}
}