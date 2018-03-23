//-- Data: 18/01/2016 16:18
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
	public partial class RastreadorCurriculoFuncao // Tabela: BNE_Rastreador_Curriculo_Funcao
	{

        #region Consultas

        #region Spselectidsfuncaoporrastreador
        private const string Spselectidsfuncaoporrastreador = @" 
        SELECT  Idf_Funcao
        FROM    BNE_Rastreador_Curriculo_Funcao
        WHERE   Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";
        #endregion

        #region Spselectcountfuncaoporrastreador
        private const string Spselectcountfuncaoporrastreador = @" 
        SELECT  count(Idf_Funcao)
        FROM    BNE_Rastreador_Curriculo_Funcao
        WHERE   Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";
        #endregion

        #endregion

        #region Métodos

        #region ListarIdentificadoresFuncaoPorRastreador
        /// <summary>
        /// Método responsável por retornar uma lista com os ids das funcoes do rastreador
        /// </summary>
        /// <param name="objRastreadorCurriculo">Código identificador do rastreador</param>
        /// <returns></returns>
        public static List<int> ListarIdentificadoresFuncaoPorRastreador(RastreadorCurriculo objRastreadorCurriculo)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objRastreadorCurriculo.IdRastreadorCurriculo }
                };

            if (QuantidadeFuncoes(objRastreadorCurriculo) > 0)
            {
                using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectidsfuncaoporrastreador, parms))
                {
                    while (dr.Read())
                        lista.Add(Convert.ToInt32(dr["Idf_Funcao"]));

                    if (!dr.IsClosed)
                        dr.Close();
                }
            }

            return lista;
        }
        #endregion

        #region QuantidadeFuncoes
        private static int QuantidadeFuncoes(RastreadorCurriculo objRastreadorCurriculo)
        {
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objRastreadorCurriculo.IdRastreadorCurriculo } };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectcountfuncaoporrastreador, parms));
        }
        #endregion

        #endregion

	}
}