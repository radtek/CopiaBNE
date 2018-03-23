//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System;
using System.Linq;
using BNE.BLL.Custom;
using BNE.Cache;

namespace BNE.BLL
{
    public partial class Curso // Tabela: TAB_Curso
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "BNE.TAB_Curso";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = false;//ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Cursos
        private static List<CursoCache> Cursos
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarCursosCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarCursosCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Lista com valores recuperados do banco.</returns>
        private static List<CursoCache> ListarCursosCACHE()
        {
            var lista = new List<CursoCache>();

            const string selectcursos = @"
            SELECT  C.Idf_Curso,
                    C.Des_Curso,
                    C.Idf_Nivel_Curso
            FROM    TAB_Curso (NOLOCK) C
            WHERE   C.Flg_Inativo = 0
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, selectcursos, null))
            {
                while (dr.Read())
                {
                    lista.Add(new CursoCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Curso"]),
                        Descricao = dr["Des_Curso"].ToString(),
                        NivelCurso = Convert.ToInt32(dr["Idf_Nivel_Curso"])
                    });
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #endregion

        #region CacheObjects

        #region CursoCache
        private class CursoCache
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
            public int NivelCurso { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string Spselectpornome = "SELECT * FROM TAB_Curso WHERE Des_Curso = @Des_Curso AND Flg_Inativo = 0";

        private const string Spselectcursoautocomplete = @"
        SELECT  TOP(@Count) * 
        FROM    TAB_Curso C
        WHERE   C.Des_Curso LIKE + @Des_Curso + '%' AND
                C.Flg_Inativo = 0
        ORDER BY C.Des_Curso
        ";

        private const string SpselectBuscaCurso = @"
        SELECT  TOP(1) * 
        FROM    TAB_Curso C
        WHERE   C.Des_Curso LIKE '%' + @Des_Curso + '%' AND
                C.Flg_Inativo = 0
        ORDER BY C.Des_Curso
        ";

        #region Spverificacursoexistente
        private const string Spverificacursoexistente = @"
        SELECT  COUNT(*)
        FROM    TAB_Curso
        WHERE   Des_Curso LIKE @Des_Curso
                AND Flg_Inativo = 0
        ";
        #endregion

        #endregion

        #region CarregarPorNome
        /// <summary>
        /// Método responsável por carregar uma instância de Curso pelo nome
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorNome(string nome, out Curso objCurso, SqlTransaction trans = null)
        {
            var retorno = false;
            objCurso = new Curso();

            #region Cache
            if (HabilitaCache)
            {
                var curso = Cursos.FirstOrDefault(c => c.Descricao.NormalizarStringLINQ().Equals(nome.NormalizarStringLINQ()));
                if (curso != null)
                {
                    objCurso = new Curso { IdCurso = curso.Identificador, DescricaoCurso = curso.Descricao };
                    retorno = true;
                }
                else
                    objCurso = null;

                return retorno;
            }
            #endregion

            var parms = new List<SqlParameter> { new SqlParameter("@Des_Curso", SqlDbType.VarChar, 100) };
            parms[0].Value = nome;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(trans,CommandType.Text, Spselectpornome, parms))
            {
                if (SetInstance(dr, objCurso))
                    retorno = true;
                else
                    objCurso = null;

                if (!dr.IsClosed)
                    dr.Close();
            }
            return retorno;
        }
        #endregion

        #region 
        /// <summary>
        /// Método que retorna uma lista de funcoes
        /// </summary>
        /// <param name="nomeParcial">Parte do nome da funcao.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes de funcoes.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool BuscaCurso(string descricao, out Curso objCurso)
        {
            #region Cache
            if (HabilitaCache){
                CursoCache cache = Cursos.OrderBy(c => c.Descricao).Where(c => c.Descricao.NormalizarStringLINQ().Contains(descricao.NormalizarStringLINQ())).FirstOrDefault();
                if (cache != null)
                {
                    objCurso = Curso.LoadObject(cache.Identificador);
                    return true;
                }
            }
            #endregion

            var cursos = new List<string>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_Curso", SqlDbType = SqlDbType.VarChar, Size = 100, Value = descricao },
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpselectBuscaCurso, parms))
            {
                objCurso = new Curso();
                if (SetInstance(dr, objCurso))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }

            return false;
        }
        #endregion

        #region RecuperarCursos
        /// <summary>
        /// Método que retorna uma lista de funcoes
        /// </summary>
        /// <param name="nomeParcial">Parte do nome da funcao.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes de funcoes.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static string[] RecuperarCursos(string nomeParcial, int numeroRegistros)
        {
            #region Cache
            if (HabilitaCache)
                return Cursos.OrderBy(c => c.Descricao).Where(c => c.Descricao.NormalizarStringLINQ().StartsWith(nomeParcial.NormalizarStringLINQ())).Take(numeroRegistros).Select(c => c.Descricao).ToArray();
            #endregion

            var cursos = new List<string>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_Curso", SqlDbType = SqlDbType.VarChar, Size = 100, Value = nomeParcial },
                    new SqlParameter { ParameterName = "@Count", SqlDbType = SqlDbType.Int, Size = 4, Value = numeroRegistros }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectcursoautocomplete, parms))
            {
                while (dr.Read())
                    cursos.Add(dr["Des_Curso"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return cursos.ToArray();
        }
        public static string[] RecuperarCursos(string nomeParcial, int numeroRegistros, bool maiorIgualNivel4)
        {
            #region Cache
            if (HabilitaCache)
            {
                if (maiorIgualNivel4)
                    return Cursos.OrderBy(c => c.Descricao).Where(c => c.Descricao.NormalizarStringLINQ().Contains(nomeParcial.NormalizarStringLINQ()) && c.NivelCurso >= 4).Take(numeroRegistros).Select(c => c.Descricao).ToArray();

                return Cursos.OrderBy(c => c.Descricao).Where(c => c.Descricao.NormalizarStringLINQ().Contains(nomeParcial.NormalizarStringLINQ()) && c.NivelCurso <= 3).Take(numeroRegistros).Select(c => c.Descricao).ToArray();
            }
            #endregion

            var cursos = new List<string>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_Curso", SqlDbType = SqlDbType.VarChar, Size = 100, Value = nomeParcial },
                    new SqlParameter { ParameterName = "@Count", SqlDbType = SqlDbType.Int, Size = 4, Value = numeroRegistros }
                };

            string query = @"
            SELECT  TOP(@Count) * 
            FROM    TAB_Curso C WITH(NOLOCK)
            WHERE   C.Des_Curso LIKE + '%' + @Des_Curso + '%' 
                    AND C.Flg_Inativo = 0";

            if (maiorIgualNivel4)
                query += " AND C.Idf_Nivel_Curso >= 4";
            else
                query += " AND C.Idf_Nivel_Curso <= 3";

            query += "ORDER BY C.Des_Curso";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, parms))
            {
                while (dr.Read())
                    cursos.Add(dr["Des_Curso"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return cursos.ToArray();
        }
        #endregion

        #region VerificaCursoPorDescricao
        /// <summary>
        /// Verifica se existe um curso atraves da descricão do curso
        /// </summary>
        /// <param name="desCurso"></param>
        /// <returns></returns>
        public static bool VerificaCursoPorDescricao(string desCurso)
        {
            #region Cache
            if (HabilitaCache)
                return Cursos.Count(c => c.Descricao.NormalizarStringLINQ().Equals(desCurso.NormalizarStringLINQ())) > 0;
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_Curso", SqlDbType = SqlDbType.VarChar, Size = 100, Value = desCurso }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spverificacursoexistente, parms)) > 0;
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Curso a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var curso = Cursos.FirstOrDefault(c => c.Identificador == this._idCurso);

                if (curso != null)
                {
                    this.DescricaoCurso = curso.Descricao;
                    this.NivelCurso = new NivelCurso(curso.NivelCurso);
                    return true;
                }

                return false;
            }
            #endregion
            
            using (IDataReader dr = LoadDataReader(this._idCurso))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Curso a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var curso = Cursos.FirstOrDefault(c => c.Identificador == this._idCurso);

                if (curso != null)
                {
                    this.DescricaoCurso = curso.Descricao;
                    this.NivelCurso = new NivelCurso(curso.NivelCurso);
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idCurso, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region [ Migration Mapping ]
        /// <summary>
        /// Médodos e atributos auxiliares à migração de dados para o novo
        /// domínio.
        /// </summary>
        public DateTime MigrationDataCadastro
        {
            set
            {
                this._dataCadastro = value;
            }
            get { return this._dataCadastro; }
        }
        #endregion

    }
}
