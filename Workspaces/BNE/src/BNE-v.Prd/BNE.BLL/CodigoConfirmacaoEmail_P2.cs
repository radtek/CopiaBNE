//-- Data: 01/07/2014 11:10
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class CodigoConfirmacaoEmail // Tabela: TAB_Codigo_Confirmacao_Email
    {

        #region Consultas

        private const string SpCarregarPorCodigo = @"
        SELECT [Idf_Codigo_Confirmacao_Email]
              ,[Dta_Criacao]
              ,[Dta_Utilizacao]
              ,[Cod_Confirmacao]
              ,[Eml_Confirmacao]
        FROM  [BNE].[TAB_Codigo_Confirmacao_Email]
        WHERE [Cod_Confirmacao] LIKE @CodigoConfirmacao";

        #endregion

        #region CarregarPorCodigo
        /// <summary>
        /// Método utilizado para retornar uma instância de CodigoConfirmacaoEmail a partir do banco de dados.
        /// </summary>
        /// <param name="codigo">Codigo</param>
        /// <param name="trans">Transação</param>
        /// <returns>CodigoConfirmacaoEmail</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static CodigoConfirmacaoEmail CarregarPorCodigo(string codigo, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@CodigoConfirmacao", SqlDbType = SqlDbType.VarChar, Size = 100, Value = codigo }
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SpCarregarPorCodigo, parms))
            {
                var objCodigoConfirmacaoEmail = new CodigoConfirmacaoEmail();
                if (SetInstance(dr, objCodigoConfirmacaoEmail))
                    return objCodigoConfirmacaoEmail;
            }

            return null;
        }
        #endregion

    }
}