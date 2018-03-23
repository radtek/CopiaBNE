//-- Data: 16/01/2012 11:03
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
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
        SELECT  *
        FROM    TAB_Pesquisa_Vaga_Tipo_Vinculo VTV WITH (NOLOCK)
        WHERE   VTV.Idf_Pesquisa_Vaga = @Idf_Pesquisa_Vaga";
        #endregion

        #endregion

        #region Métodos

        #region ListarPorPesquisaList
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de tipo vínculo da pesquisa vaga
        /// </summary>
        /// <param name="idPesquisaVaga">Código identificador de uma pesquisa vaga</param>
        /// <returns></returns>
        public static List<PesquisaVagaTipoVinculo> ListarPorPesquisaList(int idPesquisaVaga)
        {
            var list = new List<PesquisaVagaTipoVinculo>();
            using (IDataReader dr = ListarPorPesquisa(idPesquisaVaga))
            {
                var objPesquisaVagaTipoVinculo = new PesquisaVagaTipoVinculo();
                while (SetInstance_NonDispose(dr, objPesquisaVagaTipoVinculo))
                {
                    list.Add(objPesquisaVagaTipoVinculo);
                    objPesquisaVagaTipoVinculo = new PesquisaVagaTipoVinculo();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return list;
        }
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de tipo vínculo da pesquisa vaga 
        /// </summary>
        /// <param name="idPesquisaVaga">Código identificador de uma pesquisa vaga</param>
        /// <returns></returns>
        private static IDataReader ListarPorPesquisa(int idPesquisaVaga)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Pesquisa_Vaga", SqlDbType.Int, 4));
            parms[0].Value = idPesquisaVaga;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESQUISA, parms);
        }
        #endregion

        #region ListarIdentificadoresConcatenadosPorPesquisa
        /// <summary>
        /// Método responsável por retornar uma string concatenada com , com todos os ids de tipo de vinculo para determinada pesquisa de curriculo
        /// </summary>
        /// <param name="idPesquisaVaga">Código identificador da pesquisa de vaga</param>
        /// <returns></returns>
        public static string ListarIdentificadoresConcatenadosPorPesquisa(int idPesquisaVaga)
        {
            List<int> listIdentificadores = new List<int>();

            using (IDataReader dr = ListarPorPesquisa(idPesquisaVaga))
            {
                while (dr.Read())
                    listIdentificadores.Add(Convert.ToInt32(dr["Idf_Tipo_Vinculo"]));

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
        private static bool SetInstance_NonDispose(IDataReader dr, PesquisaVagaTipoVinculo objPesquisaVagaTipoVinculo)
        {
            try
            {
                if (dr.Read())
                {
                    objPesquisaVagaTipoVinculo._idPesquisaVagaTipoVinculo = Convert.ToInt32(dr["Idf_Pesquisa_Vaga_Tipo_Vinculo"]);
                    if (dr["Idf_Tipo_Vinculo"] != DBNull.Value)
                        objPesquisaVagaTipoVinculo._tipoVinculo = new TipoVinculo(Convert.ToInt32(dr["Idf_Tipo_Vinculo"]));
                    if (dr["Idf_Pesquisa_Vaga"] != DBNull.Value)
                        objPesquisaVagaTipoVinculo._pesquisaVaga = new PesquisaVaga(Convert.ToInt32(dr["Idf_Pesquisa_Vaga"]));

                    objPesquisaVagaTipoVinculo._persisted = true;
                    objPesquisaVagaTipoVinculo._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #endregion

    }
}