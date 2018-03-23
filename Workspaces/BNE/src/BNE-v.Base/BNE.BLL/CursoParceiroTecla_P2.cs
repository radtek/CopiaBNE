//-- Data: 16/04/2013 16:57
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BNE.BLL.Custom;

namespace BNE.BLL
{
	public partial class CursoParceiroTecla // Tabela: BNE_Curso_Parceiro_Tecla
    {

        #region Consultas

        #region Sprecuperarcursoparceiro
        private const string Sprecuperarcursoparceiro = @"
        SELECT  CPT.*
        FROM    BNE_Curso_Parceiro_Tecla CPT
        WHERE   CPT.Flg_Inativo = 0
                CPT.Idf_Curso_Tecla = @Idf_Curso
                AND CPT.Idf_Parceiro_Tecla = @Idf_Parceiro
        ";
        #endregion

        #region Sprecuperarcursos
        private const string Sprecuperarcursos = @"
        SELECT  CPT.*
        FROM    BNE_Curso_Parceiro_Tecla CPT WITH(NOLOCK)
        WHERE   CPT.Flg_Inativo = 0
        ";
        #endregion

        #region Sprecuperarcursosfuncao
        private const string Sprecuperarcursosfuncao = @"
        SELECT  CPT.*
        FROM    BNE_Curso_Parceiro_Tecla CPT WITH(NOLOCK)
                INNER JOIN BNE_Curso_Tecla CT WITH(NOLOCK) ON CPT.Idf_Curso_Tecla = CT.Idf_Curso_Tecla
                INNER JOIN BNE_Curso_Funcao_Tecla CFT WITH(NOLOCK) ON CT.Idf_Curso_Tecla = CFT.Idf_Curso_Tecla
        WHERE   CPT.Flg_Inativo = 0
                AND CT.Flg_Inativo = 0
                AND CFT.Flg_Inativo = 0
                AND CFT.Idf_Funcao = @Idf_Funcao
        ";
        #endregion

        #region Sprecuperaroutroscursosfuncao
        private const string Sprecuperaroutroscursosfuncao = @"
        SELECT  CPT.*
        FROM    BNE.BNE_Curso_Parceiro_Tecla CPT WITH(NOLOCK)
                LEFT JOIN BNE.BNE_Curso_Tecla CT WITH(NOLOCK) ON CPT.Idf_Curso_Tecla = CT.Idf_Curso_Tecla
                LEFT JOIN BNE.BNE_Curso_Funcao_Tecla CFT WITH(NOLOCK) ON CT.Idf_Curso_Tecla = CFT.Idf_Curso_Tecla
        WHERE   CPT.Flg_Inativo = 0
                AND CT.Flg_Inativo = 0
                AND (CFT.Flg_Inativo IS NULL OR CFT.Flg_Inativo = 0 )
                AND (CFT.Idf_Funcao IS NULL OR CFT.Idf_Funcao <> @Idf_Funcao)
        ";
        #endregion

        #region Sprecuperarnomecurso
        private const string Sprecuperarnomecurso = @"
        SELECT  CPT.Des_Titulo_Curso
        FROM    BNE_Curso_Parceiro_Tecla CPT
        WHERE   CPT.Flg_Inativo = 0
                AND CPT.Idf_Curso_Parceiro_Tecla = @Idf_Curso_Parceiro_Tecla
        ";
        #endregion

        #region Sprecuperarcursonomeurl
        private const string Sprecuperarcursonomeurl = @"
        SELECT  CPT.Idf_Curso_Parceiro_Tecla
        FROM    BNE_Curso_Parceiro_Tecla CPT
        WHERE   CPT.Flg_Inativo = 0
                AND LOWER(CPT.Des_Titulo_Curso) LIKE @Des_Titulo_Curso
        ";
        #endregion

        #endregion

        #region CarregarPorCursoParceiro
        public static CursoParceiroTecla CarregarPorCursoParceiro(CursoTecla objCursoTecla, ParceiroTecla objParceiroTecla)
	    {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curso", SqlDbType = SqlDbType.Int, Size = 4, Value = objCursoTecla.IdCursoTecla },
                    new SqlParameter { ParameterName = "@Idf_Parceiro", SqlDbType = SqlDbType.Int, Size = 4, Value = objParceiroTecla.IdParceiroTecla }
                };

            CursoParceiroTecla objCursoParceiroTecla;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarcursoparceiro, parms))
            {
                objCursoParceiroTecla = new CursoParceiroTecla();
                if (!SetInstance(dr, objCursoParceiroTecla))
                    objCursoParceiroTecla = null;
            }

            return objCursoParceiroTecla;
	    }
        #endregion

        #region CarregarCursos
        public static IDataReader CarregarCursos()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarcursos, null);
        }
        #endregion

        #region CarregarCursosPorFuncao
        /// <summary>
        /// Carrega os cursos específicos para determinada função
        /// </summary>
        /// <param name="idFuncao"></param>
        /// <returns></returns>
        public static IDataReader CarregarCursosPorFuncao(int idFuncao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = idFuncao }
                };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarcursosfuncao, parms);
        }
        #endregion

        #region CarregarOutrosCursos
        /// <summary>
        /// Recupera todos os cursos que não estão ligados aquela função
        /// </summary>
        /// <param name="idFuncao"></param>
        /// <returns></returns>
        public static IDataReader CarregarOutrosCursos(int idFuncao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = idFuncao }
                };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperaroutroscursosfuncao, parms);
        }
        #endregion


        #region RecuperarNomeCurso
        /// <summary>
        /// Recupera o nome do curso que sera usado na aplicação
        /// </summary>
        /// <param name="idCursoParceiro"></param>
        /// <returns></returns>
        public static string RecuperarNomeCurso(int idCursoParceiro)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curso_Parceiro_Tecla", SqlDbType = SqlDbType.Int, Size = 4, Value = idCursoParceiro }
                };

            Object ret = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarnomecurso, parms);
            if (ret != null)
                return ret.ToString();

            return string.Empty;
        }
        #endregion

        #region RecuperarNomeCursoURL
        /// <summary>
        /// Monta a URL a partir do nome do curso que sera usado na aplicação
        /// </summary>
        /// <param name="idCursoParceiro"></param>
        /// <returns></returns>
        public static string RecuperarNomeCursoURL(int idCursoParceiro)
	    {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Curso_Parceiro_Tecla", SqlDbType = SqlDbType.Int, Size = 4, Value = idCursoParceiro }
                };

            Object ret = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarnomecurso, parms);
            if (ret != null)
                return ret.ToString().NormalizarURL();

	        return string.Empty;
	    }
        #endregion

        #region RecuperarCursoPorNomeURL
        /// <summary>
        /// Monta a URL a partir do nome do curso que sera usado na aplicação
        /// </summary>
        /// <param name="nomeCurso"></param>
        /// <returns></returns>
        public static int RecuperarCursoPorNomeURL(string nomeCurso)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_Titulo_Curso", SqlDbType = SqlDbType.VarChar, Size = 150, Value = nomeCurso.DesnormalizarURL().ToLower() }
                };

            Object ret = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarcursonomeurl, parms);
            if (ret != null)
                return Convert.ToInt32(ret);

            return 0;
        }
        #endregion

    }
}