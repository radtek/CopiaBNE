//-- Data: 01/10/2010 11:40
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
    public partial class PesquisaCurriculoDisponibilidade // Tabela: TAB_Pesquisa_Curriculo_Disponibilidade
    {

        private const string SPSELECTPORPESQUISA = @"
                                                    SELECT 
                                                        Idf_Pesquisa_Curriculo_Disponibilidade, Idf_Disponibilidade
                                                    FROM TAB_Pesquisa_Curriculo_Disponibilidade PD
                                                    WHERE PD.Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";

        #region ListarPorPesquisaList
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de PesquisaCurriculoDisponibilidade 
        /// </summary>
        /// <param name="objPesquisaCurriculo">Pesquisa de Currículo</param>
        /// <returns></returns>
        public static List<PesquisaCurriculoDisponibilidade> ListarPorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var listaPesquisaDisponibilidade = new List<PesquisaCurriculoDisponibilidade>();

            using (IDataReader dr = Listar(objPesquisaCurriculo))
            {
                while (dr.Read())
                    listaPesquisaDisponibilidade.Add(PesquisaCurriculoDisponibilidade.LoadObject(Convert.ToInt32(dr["Idf_Pesquisa_Curriculo_Disponibilidade"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listaPesquisaDisponibilidade;
        }
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de disponibilidades relacionadas a uma pesquisa de currículo
        /// </summary>
        /// <param name="objPesquisaCurriculo">Pesquisa de Currículo</param>
        /// <returns></returns>
        private static IDataReader Listar(PesquisaCurriculo objPesquisaCurriculo)
        {
            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Value = objPesquisaCurriculo.IdPesquisaCurriculo }
				};

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESQUISA, parms);
        }
        #endregion

        #region ListarIdentificadoresConcatenadosPorPesquisa
        /// <summary>
        /// Método responsável por retornar uma string concatenada com , com todos os ids de disponibilidade para determinada pesquisa de curriculo
        /// </summary>
        /// <param name="objPesquisaCurriculo">Pesquisa de Currículo</param>
        /// <returns></returns>
        public static string ListarIdentificadoresConcatenadosPorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            List<int> listIdentificadores = new List<int>();

            using (IDataReader dr = Listar(objPesquisaCurriculo))
            {
                while (dr.Read())
                    listIdentificadores.Add(Convert.ToInt32(dr["Idf_Disponibilidade"]));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return String.Join(",", listIdentificadores.Select(i => i.ToString()).ToArray());
        }
        #endregion

    }
}