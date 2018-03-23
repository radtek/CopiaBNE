//-- Data: 25/01/2011 09:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class RastreadorCurriculoDisponibilidade // Tabela: BNE_Rastreador_Curriculo_Disponibilidade
    {

        #region Consultas

        private const string Spdeleteporrastreador = "DELETE FROM BNE_Rastreador_Curriculo_Disponibilidade WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";
        private const string Spselectidsdisponibilidadeporrastreador = @" 
        SELECT  Idf_Disponibilidade
        FROM    BNE_Rastreador_Curriculo_Disponibilidade
        WHERE   Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";

        #endregion

        #region M�todos

        #region DeletePorRastreador
        /// <summary>
        /// M�todo utilizado para excluir todas as VagaDisponibilidade ligadas a uma vaga dentro de uma transa��o
        /// </summary>
        /// <param name="objRastreadorCurriculo">Chave do registro.</param>
        /// <param name="trans">Transa��o existente no banco de dados.</param>
        /// <remarks>Gieyson</remarks>
        public static void DeletePorRastreador(RastreadorCurriculo objRastreadorCurriculo, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objRastreadorCurriculo.IdRastreadorCurriculo }
                };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spdeleteporrastreador, parms);
        }
        #endregion

        #region ListarIdentificadoresDisponibilidadePorRastreador
        /// <summary>
        /// M�todo respons�vel por retornar uma lista com todas as inst�ncias de RastreadorCurriculoDisponibilidade 
        /// </summary>
        /// <param name="objRastreadorCurriculo">C�digo identificador do rastreador</param>
        /// <returns></returns>
        public static List<int> ListarIdentificadoresDisponibilidadePorRastreador(RastreadorCurriculo objRastreadorCurriculo)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objRastreadorCurriculo.IdRastreadorCurriculo }
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectidsdisponibilidadeporrastreador, parms))
            {
                while (dr.Read())
                    lista.Add(Convert.ToInt32(dr["Idf_Disponibilidade"]));
            }

            return lista;
        }
        #endregion

        #endregion

    }
}