//-- Data: 04/02/2014 10:47
//-- Autor: Lennon Vidal

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
    [Obsolete("Obtado por não utilizar/disponilizar.")]
    public partial class PesquisaCurriculoTipoVinculo // Tabela: TAB_Pesquisa_Curriculo_Tipo_Vinculo
    {
        private const string SPSELECTPORPESQUISA = @"
                                                    SELECT Idf_Tipo_Vinculo
                                                    FROM TAB_Pesquisa_Curriculo_Tipo_Vinculo as TV
                                                    WHERE TV.Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";

        #region ListarPorPesquisaList
        /// <summary>
        /// Método responsável por retornar uma lsita com todas as instâncias de PesquisaCurriculoTipoVinculo 
        /// </summary>
        /// <param name="idPesquisaCurriculo">Código identificador de um curriculio</param>
        /// <returns></returns>
        public static List<PesquisaCurriculoTipoVinculo> ListarPorPesquisaList(int idPesquisaCurriculo)
        {
            var listPFI = new List<PesquisaCurriculoTipoVinculo>();

            using (IDataReader dr = ListarPorPesquisa(idPesquisaCurriculo))
            {
                while (dr.Read())
                    listPFI.Add(PesquisaCurriculoTipoVinculo.LoadObject(idPesquisaCurriculo, Convert.ToInt32(dr["Idf_Tipo_Vinculo"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listPFI;
        }

        public static string ListarIdentificadoresConcatenadosPorPesquisa(int idPesquisaCurriculo)
        {
            var listIdentificadores = new List<int>();

            using (IDataReader dr = ListarPorPesquisa(idPesquisaCurriculo))
            {
                while (dr.Read())
                    listIdentificadores.Add(Convert.ToInt32(dr["Idf_Tipo_Vinculo"]));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return String.Join(",", listIdentificadores.Select(i => i.ToString()).ToArray());
        }

        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="idPesquisaCurriculo">Código identificador de um currículo</param>
        /// <returns></returns>
        private static IDataReader ListarPorPesquisa(int idPesquisaCurriculo)
        {
            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Curriculo", SqlDbType.Int, 4));
            parms[0].Value = idPesquisaCurriculo;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESQUISA, parms);
        }
        #endregion


    }
}