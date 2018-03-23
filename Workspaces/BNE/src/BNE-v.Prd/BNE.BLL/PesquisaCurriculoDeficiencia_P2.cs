//-- Data: 05/02/2016 15:09
//-- Autor: Mailson

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class PesquisaCurriculoDeficiencia // Tabela: TAB_Pesquisa_Curriculo_Deficiencia
	{

        #region [Spselectporpesquisa]

        private const string Spselectporpesquisa = @"select idf_Deficiencia from BNE.TAB_Pesquisa_Curriculo_Deficiencia with(nolock)
                                                        where idf_pesquisa_curriculo = @Idf_Pesquisa_Curriculo";
        #endregion

        #region ListarIdsFuncaoPorPesquisa
        public static List<int> ListarIdentificadoresDeficienciaPorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var lista = new List<int>();

           
                using (IDataReader dr = ListarPorPesquisa(objPesquisaCurriculo))
                {
                    while (dr.Read())
                        lista.Add(Convert.ToInt32(dr["Idf_Deficiencia"]));

                    if (!dr.IsClosed)
                        dr.Close();
                }

            return lista;
        }
        #endregion

        #region ListarPorPesquisaList
        private static IDataReader ListarPorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.IdPesquisaCurriculo }
            };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporpesquisa, parms);
        }
        #endregion
    }
}