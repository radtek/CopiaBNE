//-- Data: 01/10/2010 11:40
//-- Autor: Bruno Flammarion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
	public partial class PesquisaCurriculoIdioma // Tabela: TAB_Pesquisa_Curriculo_Idioma
    {

        private const string SPSELECTPORPESQUISA = @"
                                                    SELECT 
                                                        Idf_Pesquisa_Curriculo_Idioma, Idf_Idioma
                                                    FROM TAB_Pesquisa_Curriculo_Idioma (NOLOCK) PI
                                                    WHERE PI.Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";

        #region ListarPorPesquisaList
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static List<PesquisaCurriculoIdioma> ListarPorPesquisaList(int idPesquisaCurriculo)
        {
            List<PesquisaCurriculoIdioma> listPFI = new List<PesquisaCurriculoIdioma>();

            using (IDataReader dr = ListarPorPesquisa(idPesquisaCurriculo))
            {
                while (dr.Read())
                    listPFI.Add(PesquisaCurriculoIdioma.LoadObject(Convert.ToInt32(dr["Idf_Pesquisa_Curriculo_Idioma"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listPFI;
        }
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        private static IDataReader ListarPorPesquisa(int idPesquisaCurriculo)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idPesquisaCurriculo;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESQUISA, parms);
        }
        /// <summary>
        /// Método responsável por retornar uma string concatenada com , com todos os ids de idioma para determinada pesquisa de curriculo
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static string ListarIdentificadoresConcatenadosPorPesquisa(int idPesquisaCurriculo)
        {
            List<int> listIdentificadores = new List<int>();

            using (IDataReader dr = ListarPorPesquisa(idPesquisaCurriculo))
            {
                while (dr.Read())
                    listIdentificadores.Add(Convert.ToInt32(dr["Idf_Idioma"]));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return String.Join(",", listIdentificadores.Select(i => i.ToString()).ToArray());
        }
        #endregion

	}
}