//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
    public partial class CursoFonte // Tabela: TAB_Curso_Fonte
    {

        private const string Spselectcursofonteautocomplete = @"
        DECLARE @Query VARCHAR(8000)
        SET @Query = 
			'SELECT DISTINCT TOP (' + CONVERT(VARCHAR,@Count)  + ')
					CF.Des_Curso
			FROM    TAB_Curso_Fonte CF WITH(NOLOCK)
					INNER JOIN TAB_Curso C WITH(NOLOCK) ON C.Idf_Curso = CF.Idf_Curso
			WHERE   C.Des_Curso LIKE ''%' + CONVERT(VARCHAR,@Des_Curso) + '%''
					AND C.Flg_Inativo = 0
					AND C.Flg_Auditado = 1'
			
			IF( @Idf_Fonte IS NOT NULL)
				SET @Query = @Query + ' AND CF.Idf_Fonte = ' + CONVERT(VARCHAR,@Idf_Fonte) 
			
			IF( @Idf_Nivel_Curso IS NOT NULL)
				SET @Query = @Query + ' AND C.Idf_Nivel_Curso = ' + CONVERT(VARCHAR,@Idf_Nivel_Curso)
			
			SET @Query = @Query + ' ORDER BY CF.Des_Curso'			

        EXEC(@Query) ";

        private const string Spselectcursofonte = "SELECT * FROM TAB_Curso_Fonte WITH(NOLOCK) WHERE Idf_Fonte = @Idf_Fonte AND Idf_Curso = @Idf_Curso";

        #region LoadObject

        /// <summary>
        /// Método utilizado para retornar uma instância de CursoFonte a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFonte">Chave do registro.</param>
        /// <param name="idCurso">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <param name="objCursoFonte"> </param>
        /// <returns>Instância de CursoFonte.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool LoadObject(int idFonte, int idCurso, SqlTransaction trans, out CursoFonte objCursoFonte)
        {
            using (IDataReader dr = LoadDataReader(idFonte, idCurso, trans))
            {
                objCursoFonte = new CursoFonte();
                if (SetInstance(dr, objCursoFonte))
                    return true;
            }
            objCursoFonte = null;
            return false;
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFonte">Chave do registro.</param>
        /// <param name="idCurso">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(int idFonte, int idCurso, SqlTransaction trans)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ParameterName = "@Idf_Fonte", SqlDbType = SqlDbType.Int, Size = 4, Value = idFonte} ,
                    new SqlParameter{ParameterName = "@Idf_Curso", SqlDbType = SqlDbType.Int, Size = 4, Value = idCurso}
                };

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, Spselectcursofonte, parms);
        }
        #endregion

        #region RecuperarCursoFonte

        /// <summary>
        /// Método que retorna uma lista de funcoes
        /// </summary>
        /// <param name="nomeParcial">Parte do nome da funcao.</param>
        /// <param name="idFonte"> </param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <param name="idNivelCurso"> </param>
        /// <returns>Lista de nomes de funcoes.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static string[] RecuperarCursoFonte(string nomeParcial, int? idFonte, int numeroRegistros, int? idNivelCurso = null)
        {
            var cursos = new List<string>();

            using (IDataReader dr = ListarPorNomeParcial(nomeParcial, idFonte, numeroRegistros, idNivelCurso))
            {
                while (dr.Read())
                {
                    if (dr["Des_Curso"] != DBNull.Value)
                        cursos.Add(dr["Des_Curso"].ToString());
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return cursos.ToArray();
        }
        #endregion

        #region ListarPorNomeParcial
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader ListarPorNomeParcial(string nome, int? idFonte, int numeroRegistros, int? idNivelCurso = null)
        {
            object paramValueFonte = DBNull.Value;
            object paramValueNivelCurso  = DBNull.Value;
            
            if (idFonte.HasValue)
                paramValueFonte = idFonte;

            if (idNivelCurso.HasValue)
                paramValueNivelCurso = idNivelCurso;

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ParameterName = "@Des_Curso", SqlDbType = SqlDbType.VarChar, Size = 80, Value = nome} ,
                    new SqlParameter{ParameterName = "@Count", SqlDbType = SqlDbType.Int, Size = 4, Value = numeroRegistros} ,
                    new SqlParameter{ParameterName = "@Idf_Fonte", SqlDbType = SqlDbType.Int, Size = 4, Value = paramValueFonte } ,
                    new SqlParameter{ParameterName = "@Idf_Nivel_Curso", SqlDbType = SqlDbType.Int, Size = 4, Value = paramValueNivelCurso }
                };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcursofonteautocomplete, parms);
        }
        #endregion

    }
}