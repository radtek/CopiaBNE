//-- Data: 18/11/2010 13:28
//-- Autor: Vinicius Maciel

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class VagaTipoVinculo // Tabela: BNE_Vaga_Tipo_Vinculo
    {

        #region Consultas
        private const string Spdeleteporvaga = "DELETE FROM BNE_Vaga_Tipo_Vinculo WHERE Idf_Vaga = @Idf_Vaga";

        private const string Spselectidporvaga = @"
        SELECT  Idf_Vaga_Tipo_Vinculo
        FROM    BNE_Vaga_Tipo_Vinculo WITH(NOLOCK)
        WHERE   Idf_Vaga = @Idf_Vaga";

        private const string Spcountporvagatipovinculo = @"SELECT count(Idf_Vaga_Tipo_Vinculo) FROM BNE_Vaga_Tipo_Vinculo WITH(NOLOCK) WHERE Idf_Vaga = @Idf_Vaga AND Idf_Tipo_Vinculo = @Idf_Tipo_Vinculo";
        #endregion

        #region DeletePorVaga
        /// <summary>
        /// Método utilizado para excluir todas as VagaTipoVinculo ligadas a uma vaga dentro de uma transação
        /// </summary>
        /// <param name="idVaga">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson</remarks>
        public static void DeletePorVaga(int idVaga, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spdeleteporvaga, parms);
        }
        #endregion

        #region ListarDisponibilidadesPorVaga
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas as instâncias de VagaTipoVinculo 
        /// </summary>
        /// <param name="idVaga">Código identificador de uma vaga</param>
        /// <returns></returns>
        public static List<VagaTipoVinculo> ListarTipoVinculoPorVaga(int idVaga, SqlTransaction trans = null)
        {
            var listVagaTipoVinculo = new List<VagaTipoVinculo>();

            using (IDataReader dr = ListarIDPorVaga(idVaga, trans))
            {
                while (dr.Read())
                    listVagaTipoVinculo.Add(LoadObject(Convert.ToInt32(dr["Idf_Vaga_Tipo_Vinculo"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listVagaTipoVinculo;
        }
        /// <summary>
        /// Método responsável por retornar uma IDataReader com todas os ids de VagaTipoVinculo
        /// </summary>
        /// <param name="idVaga">Código identificador de uma vaga</param>
        /// <returns></returns>
        private static IDataReader ListarIDPorVaga(int idVaga, SqlTransaction trans = null)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = idVaga }
                };

            return trans == null ? DataAccessLayer.ExecuteReader(CommandType.Text, Spselectidporvaga, parms)
                : DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectidporvaga, parms);
        }
        #endregion

        #region PossuiVinculo
        /// <summary>
        /// Retorna um booleano indicando se a vaga possui o tipo vinculo indicado ou não
        /// </summary>
        /// <returns></returns>
        public static bool PossuiVinculo(Vaga objVaga, Enumeradores.TipoVinculo tipoVinculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Vaga", SqlDbType = SqlDbType.Int, Size = 4, Value = objVaga.IdVaga },
                    new SqlParameter { ParameterName = "@Idf_Tipo_Vinculo", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)tipoVinculo }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spcountporvagatipovinculo, parms)) > 0;
        }
        #endregion

    }
}