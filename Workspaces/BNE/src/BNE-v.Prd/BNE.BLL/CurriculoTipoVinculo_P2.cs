//-- Data: 31/01/2014 11:00
//-- Autor: Lennon Vidal

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    [Obsolete("Obtado por não utilizar/disponilizar.")]
    public partial class CurriculoTipoVinculo // Tabela: BNE_Curriculo_Tipo_Vinculo
    {
        #region Consultas
        private const string SpdeletePorCurriculo = "DELETE FROM BNE_Curriculo_Tipo_Vinculo WHERE Idf_Curriculo = @Idf_Curriculo";

        private const string SpSelectVinculoPorCurriculo = @"
        SELECT  Idf_Tipo_Vinculo
        FROM    BNE_Curriculo_Tipo_Vinculo WITH(NOLOCK)
        WHERE   Idf_Curriculo = @Idf_Curriculo";
        #endregion

        #region DeletePorVaga
        /// <summary>
        /// Método utilizado para excluir todos os tipo vinculo ligados a um curriculo dentro de uma transação
        /// </summary>
        /// <param name="idCurriculo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <remarks>Gieyson</remarks>
        [Obsolete]
        public static void DeletePorCurriculo(int idCurriculo, SqlTransaction trans)
        {
            throw new InvalidOperationException("Não disponível.");

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            if (trans != null)
                DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, SpdeletePorCurriculo, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SpdeletePorCurriculo, parms);
        }
        #endregion

        [Obsolete]
        public static List<CurriculoTipoVinculo> CarregarVinculoPretendidoPorCurriculo(int idCurriculo)
        {
            throw new InvalidOperationException("Não disponível.");

            var listVagaTipoVinculo = new List<CurriculoTipoVinculo>();

            using (IDataReader dr = ListarIdPorCurriculo(idCurriculo))
            {
                while (dr.Read())
                    listVagaTipoVinculo.Add(LoadObject(idCurriculo, Convert.ToInt32(dr["Idf_Tipo_Vinculo"])));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return listVagaTipoVinculo;
        }

        [Obsolete]
        private static IDataReader ListarIdPorCurriculo(int idCurriculo)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurriculo }
                };

            return DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectVinculoPorCurriculo, parms);
        }
    }
}