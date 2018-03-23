//-- Data: 28/01/2016 18:54
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class RastreadorCurriculoResultado // Tabela: BNE_Rastreador_Curriculo_Resultado
    {

        #region Consultas

        #region Spdeleteporrastreador
        private const string Spdeleteporrastreador = @"
        DECLARE @SQL NVARCHAR(MAX)

        DECLARE @ParmDefinition NVARCHAR(1000)
        SET @ParmDefinition = N'@Idf_Rastreador_Curriculo INT, @Curriculos VARCHAR(MAX)';

        SET @SQL = 'DELETE FROM BNE_Rastreador_Curriculo_Resultado WHERE Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo AND Idf_Curriculo IN ( ' + @Curriculos + ' )'

        EXEC sp_executesql @SQL, @ParmDefinition, @Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo, @Curriculos = @Curriculos";
        #endregion

        #endregion

        #region Métodos
        
        #region DeletarCurriculosForaPerfil
        /// <summary>
        /// deleta os curriculos
        /// </summary>
        /// <param name="objRastreadorCurriculo"></param>
        /// <param name="curriculos"></param>
        /// <param name="trans"></param>
        public static void DeletarCurriculosForaPerfil(RastreadorCurriculo objRastreadorCurriculo, List<int> curriculos, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objRastreadorCurriculo.IdRastreadorCurriculo },
                    new SqlParameter { ParameterName = "@Curriculos", SqlDbType = SqlDbType.VarChar, Value = string.Join(", ",curriculos) }
                };

            DataAccessLayer.ExecuteNonQuery(trans, CommandType.Text, Spdeleteporrastreador, parms);
        }
        #endregion

        #region Inserção em Massa
        /// <summary>
        /// Crie uma tabela para inserir em massa.
        /// Passe um DataTable nulo para e a classe em populada. 
        /// Ela irá instanciar um novo DataTable já com colunas definidas apartir dos parâmetros sql definidos na classe.
        /// Os valores setados nas propriedades são transformados em uma linha na tabela.
        /// </summary>
        /// <param name="dt"></param>
        public void AddBulkTable(ref DataTable dt)
        {
            DataAccessLayer.AddBulkTable(ref dt, this);
        }
        /// <summary>
        /// Realiza inserção em massa.
        /// </summary>
        /// <param name="dt">Tabela criada pelo método AddBulkTable</param>
        /// <param name="trans">Transação</param>
        public static void SaveBulkTable(DataTable dt, SqlTransaction trans = null)
        {
            DataAccessLayer.SaveBulkTable(dt, "BNE_Rastreador_Curriculo_Resultado", trans);
        }
        #endregion

        #endregion

    }
}