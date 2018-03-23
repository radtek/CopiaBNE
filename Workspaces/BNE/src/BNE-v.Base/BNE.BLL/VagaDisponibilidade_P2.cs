//-- Data: 18/11/2010 13:23
//-- Autor: Vinicius Maciel

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class VagaDisponibilidade // Tabela: BNE_Vaga_Disponibilidade
	{

        #region Consultas
        private const string SPDELETEPORVAGA = "DELETE FROM BNE_Vaga_Disponibilidade WHERE Idf_Vaga = @Idf_Vaga";

        private const string SPSELECTIDPORVAGA = @"
        SELECT 
                Idf_Vaga_Disponibilidade
        FROM    BNE_Vaga_Disponibilidade WITH (NOLOCK)
        WHERE   Idf_Vaga = @Idf_Vaga";
        #endregion

        /// <summary>
        /// Método utilizado para excluir todas as VagaDisponibilidade ligadas a uma vaga dentro de uma transação
        /// </summary>
        /// <param name="idVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson</remarks>
        public static void DeletePorVaga(int idVaga, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4));

            parms[0].Value = idVaga;

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SPDELETEPORVAGA, parms);
        }

        #region ListarDisponibilidadesPorVaga
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de VagaDisponibilidade 
        /// </summary>
        /// <param name="idVaga">Código identificador da vaga</param>
        /// <returns></returns>
        public static List<VagaDisponibilidade> ListarDisponibilidadesPorVaga(int idVaga)
        {
            var listVagaDisponibilidade = new List<VagaDisponibilidade>();

            using (var dr = ListarIdPorVaga(idVaga))
            {
                while (dr.Read())
                    listVagaDisponibilidade.Add(VagaDisponibilidade.LoadObject(Convert.ToInt32(dr["Idf_Vaga_Disponibilidade"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listVagaDisponibilidade;
        }
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas os ids de vaga disponibilidade
        /// </summary>
        /// <param name="idVaga">Código identificador de uma vaga</param>
        /// <returns></returns>
        private static IDataReader ListarIdPorVaga(int idVaga)
        {
            var parms = new List<SqlParameter> {new SqlParameter("@Idf_Vaga", SqlDbType.Int, 4)};
            parms[0].Value = idVaga;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTIDPORVAGA, parms);
        }
        #endregion


	}
}