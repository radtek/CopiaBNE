//-- Data: 25/01/2011 09:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class RastreadorCurriculoIdioma // Tabela: BNE_Rastreador_Curriculo_Idioma
    {

        #region Consultas

        #region Spdeleteporrastreador
        private const string Spdeleteporrastreador = "DELETE FROM BNE_Rastreador_Curriculo_Idioma WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";
        #endregion

        #region Spselectidsidiomaporrastreador
        private const string Spselectidsidiomaporrastreador = @" 
        SELECT  Idf_Idioma, Idf_Nivel_Idioma
        FROM    BNE_Rastreador_Curriculo_Idioma
        WHERE   Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";
        #endregion

        #endregion

        #region DeletePorRastreador
        /// <summary>
        /// Método utilizado para excluir todas as VagaDisponibilidade ligadas a uma vaga dentro de uma transação
        /// </summary>
        /// <param name="objRastreadorCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
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

        #region ListarIdentificadoresIdiomaPorRastreador
        /// <summary>
        /// Método responsável por retornar uma lista com todas as instâncias de RastreadorCurriculoDisponibilidade 
        /// </summary>
        /// <param name="objRastreadorCurriculo">Código identificador do rastreador</param>
        /// <returns></returns>
        public static List<KeyValuePair<int, int>> ListarIdentificadoresIdiomaPorRastreador(RastreadorCurriculo objRastreadorCurriculo)
        {
            var lista = new List<KeyValuePair<int, int>>();

             var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objRastreadorCurriculo.IdRastreadorCurriculo }
                };

            using (var dr = ListarPorPesquisa(objRastreadorCurriculo))
            {
                while (dr.Read())
                    lista.Add(new KeyValuePair<int,int>(Convert.ToInt32(dr["Idf_Idioma"]),
                          (dr["idf_Nivel_idioma"] != DBNull.Value ? Convert.ToInt32(dr["idf_Nivel_idioma"]) : 0)));
            }

            return lista;
        }
        #endregion

        #region ListarPorPesquisaList
        private static IDataReader ListarPorPesquisa(RastreadorCurriculo objPesquisaCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.IdRastreadorCurriculo }
            };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectidsidiomaporrastreador, parms);
        }
        #endregion

        #region ListarIdiomaPorPesquisa
        public static List<RastreadorCurriculoIdioma> ListarIdiomaPorPesquisa(RastreadorCurriculo objPesquisaCurriculo)
        {
            var lista = new List<RastreadorCurriculoIdioma>();
            using (IDataReader dr = ListarPorPesquisa(objPesquisaCurriculo))
            {
                while (dr.Read())
                {
                    RastreadorCurriculoIdioma obj = new RastreadorCurriculoIdioma
                    {
                        Idioma = new Idioma(Convert.ToInt32(dr["idf_Idioma"])),
                        NivelIdioma = dr["idf_Nivel_Idioma"] != DBNull.Value ? new NivelIdioma(Convert.ToInt32(dr["idf_Nivel_Idioma"])) : null
                    };
                    lista.Add(obj);
                }


                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

    }
}