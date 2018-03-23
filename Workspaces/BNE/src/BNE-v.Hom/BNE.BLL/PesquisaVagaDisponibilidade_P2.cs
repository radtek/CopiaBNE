//-- Data: 12/01/2012 15:07
//-- Autor: kaio

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
    public partial class PesquisaVagaDisponibilidade // Tabela: BNE.TAB_Pesquisa_Vaga_Disponibilidade
    {
        #region Consultas

        #region SPSELECTPORPESQUISA
        private const string SPSELECTPORPESQUISA = @"
        SELECT  Idf_Disponibilidade
        FROM    TAB_Pesquisa_Vaga_Disponibilidade VD WITH (NOLOCK)
        WHERE   VD.Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga";
        #endregion

        #endregion

        #region Métodos

        #region ListarPorPesquisa
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="objPesquisaVaga">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static IDataReader ListarPorPesquisa(PesquisaVaga objPesquisaVaga)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Idf_Pesquisa_Vaga", SqlDbType = SqlDbType.Int, Size=4, Value = objPesquisaVaga.IdPesquisaVaga}
            };

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESQUISA, parms);
        }
        #endregion

        #region ListarIdentificadoresConcatenadosPorPesquisa
        /// <summary>
        /// Método responsável por retornar uma string concatenada com , com todos os ids de disponibilidade para determinada pesquisa de curriculo
        /// </summary>
        /// <param name="objPesquisaVaga">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static string ListarIdentificadoresConcatenadosPorPesquisa(PesquisaVaga objPesquisaVaga)
        {
            return String.Join(",", ListarIdentificadores(objPesquisaVaga).Select(i => i.ToString()).ToArray());
        }
        #endregion

        public static List<int> ListarIdentificadores(PesquisaVaga objPesquisaVaga)
        {
            List<int> identificadores = new List<int>();

            using (IDataReader dr = ListarPorPesquisa(objPesquisaVaga))
            {
                while (dr.Read())
                    identificadores.Add(Convert.ToInt32(dr["Idf_Disponibilidade"]));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return identificadores;
        }

        #endregion

    }
}