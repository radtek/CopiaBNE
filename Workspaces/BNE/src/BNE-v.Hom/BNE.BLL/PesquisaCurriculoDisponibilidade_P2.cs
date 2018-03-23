//-- Data: 01/10/2010 11:40
//-- Autor: Bruno Flammarion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class PesquisaCurriculoDisponibilidade // Tabela: TAB_Pesquisa_Curriculo_Disponibilidade
    {

        #region Consultas
        private const string Spselectidsdisponibilidadeporrastreador = @" SELECT Idf_Disponibilidade FROM TAB_Pesquisa_Curriculo_Disponibilidade PD WHERE PD.Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";

        #region [Spselectdisponibilidadeporrastreador]
        private const string Spselectdisponibilidadeporrastreador = @"SELECT cd.Idf_Disponibilidade, d.Des_Disponibilidade FROM TAB_Pesquisa_Curriculo_Disponibilidade cd with(nolock)
                    join bne.Tab_Disponibilidade d with(nolock) on cd.Idf_Disponibilidade = d.Idf_Disponibilidade
                    where cd.Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";

        #endregion

        private const string SPDELETEDisponibilidade = "DELETE FROM TAB_Pesquisa_Curriculo_Disponibilidade WHERE Idf_Pesquisa_Curriculo_Disponibilidade = @Idf_Pesquisa_Curriculo_Disponibilidade and Idf_Disponibilidade = @Idf_Disponibilidade";
        #endregion

        #region Métodos

        #region ListarIdentificadoresDisponibilidadePorPesquisa
        public static List<int> ListarIdentificadoresDisponibilidadePorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
				{
					new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Value = objPesquisaCurriculo.IdPesquisaCurriculo }
				};

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectidsdisponibilidadeporrastreador, parms))
            {
                while (dr.Read())
                    lista.Add(Convert.ToInt32(dr["Idf_Disponibilidade"]));
            }

            return lista;
        }

        public static Dictionary<int,string> ListarDisponibilidadePorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var lista = new Dictionary<int,string>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Value = objPesquisaCurriculo.IdPesquisaCurriculo }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectdisponibilidadeporrastreador, parms))
            {
                while (dr.Read())
                    lista.Add(Convert.ToInt32(dr["Idf_Disponibilidade"]),dr["Des_Disponibilidade"].ToString());
            }

            return lista;
        }
        #endregion


        #endregion

    }
}