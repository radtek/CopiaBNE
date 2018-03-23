//-- Data: 14/10/2015 18:55
//-- Autor: Ribeiro

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PesquisaCurriculoFuncao // Tabela: TAB_Pesquisa_Curriculo_Funcao
    {
        #region Consultas
        private const string Spselecionafuncoesporpesquisa = "SELECT Idf_Funcao FROM TAB_Pesquisa_Curriculo_Funcao WHERE Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";
        private const string Spcountselecionafuncoesporpesquisa = "SELECT COUNT(Idf_Pesquisa_Curriculo_Funcao) FROM TAB_Pesquisa_Curriculo_Funcao WHERE Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";
        #endregion

        #region ListarIdsFuncaoPorPesquisa
        public static List<int> ListarIdentificadoresFuncaoPorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var lista = new List<int>();

            if (QuantidadeFuncoes(objPesquisaCurriculo) > 0)
            {
                using (IDataReader dr = ListarPorPesquisa(objPesquisaCurriculo))
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
       
        #region ListarPorPesquisa
        private static IDataReader ListarPorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.IdPesquisaCurriculo }
            };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselecionafuncoesporpesquisa, parms);
        }
        #endregion

        #region QuantidadeFuncoes
        private static int QuantidadeFuncoes(PesquisaCurriculo objPesquisaCurriculo)
        {
            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.IdPesquisaCurriculo } };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcountselecionafuncoesporpesquisa, parms));
        }
        #endregion

    }
}