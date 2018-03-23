//-- Data: 16/01/2012 11:03
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
    public partial class PesquisaVagaTipoVinculo // Tabela: TAB_Pesquisa_Vaga_Tipo_Vinculo
    {
        #region Consultas

        #region SPSELECTPORPESQUISA
        private const string SPSELECTPORPESQUISA = @"
        SELECT  Idf_Tipo_Vinculo
        FROM    TAB_Pesquisa_Vaga_Tipo_Vinculo VTV WITH (NOLOCK)
        WHERE   VTV.Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga";
        #endregion

        #endregion

        #region M�todos

        #region ListarPorPesquisa
        /// <summary>
        /// M�todo respons�vel por retornar uma IDataReader com todas as inst�ncias de tipo v�nculo da pesquisa vaga 
        /// </summary>
        /// <param name="objPesquisaVaga">C�digo identificador de uma pesquisa vaga</param>
        /// <returns></returns>
        private static IDataReader ListarPorPesquisa(PesquisaVaga objPesquisaVaga)
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
        /// M�todo respons�vel por retornar uma string concatenada com , com todos os ids de tipo de vinculo para determinada pesquisa de curriculo
        /// </summary>
        /// <param name="objPesquisaVaga">C�digo identificador da pesquisa de vaga</param>
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
                    identificadores.Add(Convert.ToInt32(dr["Idf_Tipo_Vinculo"]));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return identificadores;
        }
        #endregion

    }
}