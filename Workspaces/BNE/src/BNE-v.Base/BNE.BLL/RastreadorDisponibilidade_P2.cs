//-- Data: 25/01/2011 09:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace BNE.BLL
{
	public partial class RastreadorDisponibilidade // Tabela: BNE_Rastreador_Disponibilidade
	{

        private const string SPDELETEPORVAGA = "DELETE FROM BNE_Rastreador_Disponibilidade WHERE Idf_Rastreador = @Idf_Rastreador";

        private const string SPSELECTPORPESQUISA = @"
                                                    SELECT 
                                                        Idf_Rastreador_Disponibilidade, Idf_Disponibilidade
                                                    FROM BNE_Rastreador_Disponibilidade RD
                                                    WHERE RD.Idf_Rastreador = @Idf_Rastreador";

        #region DeletePorRastreador
        /// <summary>
        /// Método utilizado para excluir todas as VagaDisponibilidade ligadas a uma vaga dentro de uma transação
        /// </summary>
        /// <param name="idVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson</remarks>
        public static void DeletePorRastreador(int idRastreador, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4));

            parms[0].Value = idRastreador;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETEPORVAGA, parms);
        }
        #endregion

        #region ListarPorPesquisaList
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static List<RastreadorDisponibilidade> ListarPorRastreadorList(int idRastreador)
        {
            List<RastreadorDisponibilidade> listPFI = new List<RastreadorDisponibilidade>();

            using (IDataReader dr = ListarPorRastreador(idRastreador))
            {
                while (dr.Read())
                    listPFI.Add(RastreadorDisponibilidade.LoadObject(Convert.ToInt32(dr["Idf_Rastreador_Disponibilidade"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listPFI;
        }
        #endregion

        #region ListarPorRastreador
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de Formacao 
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static IDataReader ListarPorRastreador(int idRastreador)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Rastreador", SqlDbType.Int, 4));
            parms[0].Value = idRastreador;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTPORPESQUISA, parms);
        }
        #endregion

        #region ListarIdentificadoresConcatenadosPorRastreador
        /// <summary>
        /// Método responsável por retornar uma string concatenada com , com todos os ids de disponibilidade para determinada pesquisa de curriculo
        /// </summary>
        /// <param name="idPessoaFisica">Código identificador de uma pessoa física</param>
        /// <returns></returns>
        public static string ListarIdentificadoresConcatenadosPorRastreador(Rastreador objRastreador)
        {
            List<int> listIdentificadores = new List<int>();

            using (IDataReader dr = ListarPorRastreador(objRastreador.IdRastreador))
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