//-- Data: 12/01/2012 15:07
//-- Autor: kaio

using System;
using System.Collections;
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
        SELECT  *
        FROM    TAB_Pesquisa_Vaga_Disponibilidade VD WITH (NOLOCK)
        WHERE   VD.Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga";
        #endregion

        #endregion

        #region Métodos

        #region ListarPorPesquisaList
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static List<PesquisaVagaDisponibilidade> ListarPorPesquisaList(int idPesquisaVaga)
        {
            List<PesquisaVagaDisponibilidade> list = new List<PesquisaVagaDisponibilidade>();
            PesquisaVagaDisponibilidade objPesquisaVagaDisponibilidade;
            using (IDataReader dr = ListarPorPesquisa(idPesquisaVaga))
            {
                while (dr.Read())
                {
                    objPesquisaVagaDisponibilidade = new PesquisaVagaDisponibilidade();
                    if (SetInstance_NonDispose(dr, objPesquisaVagaDisponibilidade))
                        list.Add(objPesquisaVagaDisponibilidade);
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return list;
        }
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static IDataReader ListarPorPesquisa(int idPesquisaVaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));
            parms[0].Value = idPesquisaVaga;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESQUISA, parms);
        }
        #endregion

        #region ListarIdentificadoresConcatenadosPorPesquisa
        /// <summary>
        /// Método responsável por retornar uma string concatenada com , com todos os ids de disponibilidade para determinada pesquisa de curriculo
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static string ListarIdentificadoresConcatenadosPorPesquisa(int idPesquisaVaga)
        {
            List<int> listIdentificadores = new List<int>();

            using (IDataReader dr = ListarPorPesquisa(idPesquisaVaga))
            {
                while (dr.Read())
                    listIdentificadores.Add(Convert.ToInt32(dr["Idf_Disponibilidade"]));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return String.Join(",", listIdentificadores.Select(i => i.ToString()).ToArray());
        }
        #endregion

        #region SetInstance_NonDispose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objTABPesquisaVagaDisponibilidade">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>kaio</remarks>
        private static bool SetInstance_NonDispose(IDataReader dr, PesquisaVagaDisponibilidade objPesquisaVagaDisponibilidade)
        {
            
            objPesquisaVagaDisponibilidade._idPesquisaVagaDisponibilidade = Convert.ToInt32(dr["Idf_Pesquisa_Vaga_Disponibilidade"]);
            if (dr["Idf_Disponibilidade"] != DBNull.Value)
                objPesquisaVagaDisponibilidade._disponibilidade = new Disponibilidade(Convert.ToInt32(dr["Idf_Disponibilidade"]));
            if (dr["Idf_Pesquisa_Vaga"] != DBNull.Value)
                objPesquisaVagaDisponibilidade._pesquisaVaga = new PesquisaVaga(Convert.ToInt32(dr["Idf_Pesquisa_Vaga"]));

            objPesquisaVagaDisponibilidade._persisted = true;
            objPesquisaVagaDisponibilidade._modified = false;

            return true;
        }
        #endregion

        #endregion

    }
}