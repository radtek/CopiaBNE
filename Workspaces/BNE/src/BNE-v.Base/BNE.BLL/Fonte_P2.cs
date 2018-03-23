//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.BLL.Custom;
using BNE.BLL.wsSine;
using BNE.Cache;

namespace BNE.BLL
{
    public partial class Fonte // Tabela: TAB_Fonte
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "BNE.TAB_Fonte";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = false;// ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Fontes
        private static List<FonteCache> Fontes
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarFontesCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarFontesCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Lista com valores recuperados do banco.</returns>
        private static List<FonteCache> ListarFontesCACHE()
        {
            var lista = new List<FonteCache>();

            const string selectfontes = @"
            SELECT  F.Idf_Fonte,
                    F.Sig_Fonte,
                    F.Nme_Fonte,
                    F.Flg_Auditada
            FROM    TAB_Fonte F WITH(NOLOCK)
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, selectfontes, null))
            {
                while (dr.Read())
                {
                    lista.Add(new FonteCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Fonte"]),
                        Sigla = dr["Sig_Fonte"].ToString(),
                        Nome = dr["Nme_Fonte"].ToString(),
                        Auditada = Convert.ToBoolean(dr["Flg_Auditada"])
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

        #region FonteCache
        private class FonteCache
        {
            public int Identificador { get; set; }
            public string Nome { get; set; }
            public string Sigla { get; set; }
            public bool Auditada { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string Spselectpornome = "SELECT * FROM TAB_Fonte WITH(NOLOCK) WHERE Nme_Fonte = @Nme_Fonte";

        private const string Spselectporsiglanome = "SELECT * FROM TAB_Fonte WITH(NOLOCK) WHERE Sig_Fonte + ' - ' + Nme_Fonte = @Sig_Nome_Fonte";

        private const string Spselectporsiglanomeautocomplete = @"     
        SELECT  TOP(@Count) Idf_Fonte,
                F.Sig_Fonte + ' - ' +  F.Nme_Fonte AS Sig_Fonte  
        FROM    TAB_Fonte F WITH(NOLOCK)
        WHERE  ( F.Sig_Fonte LIKE '%' + @Sig_Fonte + '%' OR F.Nme_Fonte LIKE '%' + @Sig_Fonte + '%')AND
                F.Flg_Auditada = 1
        GROUP BY F.Sig_Fonte , F.Nme_Fonte , Idf_Fonte
        ";

        private const string Splistarfontepornivelcurso = @"
        DECLARE @Query VARCHAR(8000)
        SET @Query = 'SELECT DISTINCT TOP(' + CONVERT(VARCHAR,@Count) + ') F.*
        FROM    TAB_Fonte F WITH(NOLOCK)
                INNER JOIN TAB_Curso_Fonte CF WITH(NOLOCK) ON F.Idf_Fonte = CF.Idf_Fonte
                INNER JOIN TAB_Curso C WITH(NOLOCK) ON CF.Idf_Curso = C.Idf_Curso
        WHERE   1 = 1 AND  ( F.Sig_Fonte LIKE ''%' + @Sig_Fonte + '%'' 
	            OR F.Nme_Fonte LIKE ''%' + @Sig_Fonte + '%'')
                AND F.Flg_Auditada = 1'

        IF ( @Idf_Nivel_Curso IS NOT NULL ) 
            SET @Query = @Query + ' AND	C.Idf_Nivel_Curso ='
                + CONVERT(VARCHAR, @Idf_Nivel_Curso)
        
        EXEC(@Query)";

        #endregion

        #region CarregarPorNome
        /// <summary>
        /// Método responsável por carregar uma instância de Fonte pelo nome
        /// </summary>
        /// <returns>Objeto Pessoa Fisica</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorNome(string nomeFonte, out Fonte objFonte)
        {
            var retorno = false;
            objFonte = new Fonte();

            #region Cache
            if (HabilitaCache)
            {
                var fonte = Fontes.FirstOrDefault(f => f.Nome.NormalizarStringLINQ().Equals(nomeFonte.NormalizarStringLINQ()));
                if (fonte != null)
                {
                    objFonte = new Fonte { IdFonte = fonte.Identificador, NomeFonte = fonte.Nome, FlagAuditada = fonte.Auditada, SiglaFonte = fonte.Sigla };
                    retorno = true;
                }
                else
                    objFonte = null;

                return retorno;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Nme_Fonte", SqlDbType = SqlDbType.VarChar, Size = 100, Value = nomeFonte}
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectpornome, parms))
            {
                if (SetInstance(dr, objFonte))
                    retorno = true;
                else
                    objFonte = null;
                if (!dr.IsClosed)
                    dr.Close();
            }
            return retorno;
        }
        #endregion

        #region CarregarPorSiglaNome
        /// <summary>
        /// Método responsável por carregar uma instância de Fonte pela sigla e nome
        /// </summary>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorSiglaNome(string sigNomeFonte, out Fonte objFonte)
        {
            var retorno = false;
            objFonte = new Fonte();

            #region Cache
            if (HabilitaCache)
            {
                var fonte = Fontes.FirstOrDefault(f => string.Concat(f.Nome.NormalizarStringLINQ(), " - ", f.Sigla.NormalizarStringLINQ()).Equals(sigNomeFonte.NormalizarStringLINQ()));
                if (fonte != null)
                {
                    objFonte = new Fonte { IdFonte = fonte.Identificador, NomeFonte = fonte.Nome, FlagAuditada = fonte.Auditada, SiglaFonte = fonte.Sigla };
                    retorno = true;
                }
                else
                    objFonte = null;

                return retorno;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Sig_Nome_Fonte", SqlDbType = SqlDbType.VarChar,  Size = 120, Value = sigNomeFonte }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporsiglanome, parms))
            {
                if (SetInstance(dr, objFonte))
                    retorno = true;
                else
                    objFonte = null;

                if (!dr.IsClosed)
                    dr.Close();
            }
            return retorno;
        }
        #endregion

        #region BuscarPorDescricao
        /// <summary>
        /// Método responsável por busca uma instância de Fonte através da descricao informada
        /// </summary>
        /// <returns>Objeto Fonte</returns>
        /// <remarks>Francisco Ribas</remarks>
        public static bool BuscarPorDescricao(string descricao, out Fonte objFonte)
        {
            //Utilizando função do autocomplete para aproveitar o cache
            Dictionary<int, string> lstFuncao = RecuperarSiglaNomeFonte(descricao, 1);
            if (lstFuncao.Count > 0)
            {
                objFonte = Fonte.LoadObject(lstFuncao.First().Key);
                return true;
            }

            objFonte = null;
            return false;
        }
        #endregion BuscarPorDescricao

        #region RecuperarSiglaNomeFonte
        /// <summary>
        /// Método que retorna uma lista de funcoes
        /// </summary>
        /// <param name="siglaParcial">Parte do nome da funcao.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes de funcoes.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Dictionary<int, string> RecuperarSiglaNomeFonte(string siglaParcial, int numeroRegistros)
        {
            #region Cache
            if (HabilitaCache)
                return Fontes.OrderBy(f => f.Sigla).GroupBy(f => new { f.Sigla, f.Nome, f.Identificador, f.Auditada }).Where(f => f.Key.Auditada && (f.Key.Nome.NormalizarStringLINQ().Contains(siglaParcial.NormalizarStringLINQ()) || f.Key.Sigla.NormalizarStringLINQ().Contains(siglaParcial.NormalizarStringLINQ()))).Take(numeroRegistros).ToDictionary(f => f.Key.Identificador, f => string.Concat(f.Key.Sigla, " - ", f.Key.Nome)); 
            #endregion

            var funcoes = new Dictionary<int, string>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Sig_Fonte", SqlDbType = SqlDbType.VarChar, Size = 80, Value = siglaParcial},
                    new SqlParameter{ ParameterName = "@Count", SqlDbType = SqlDbType.Int, Size = 4, Value = numeroRegistros}
                };

            /* SELECT  TOP(@Count) Idf_Fonte,
                F.Sig_Fonte + ' - ' +  F.Nme_Fonte AS Sig_Fonte  
        FROM    TAB_Fonte F WITH(NOLOCK)
        WHERE  ( F.Sig_Fonte LIKE '%' + @Sig_Fonte + '%' OR F.Nme_Fonte LIKE '%' + @Sig_Fonte + '%')AND
                F.Flg_Auditada = 1
        GROUP BY F.Sig_Fonte , F.Nme_Fonte , Idf_Fonte*/
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectporsiglanomeautocomplete, parms))
            {
                while (dr.Read())
                    funcoes.Add(Convert.ToInt32(dr["Idf_Fonte"]), dr["Sig_Fonte"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return funcoes;
        }
        #endregion

        #region RecuperarSiglaNomeFonteNivelCurso
        /// <summary>
        /// Método que retorna uma lista de fonte de acordo com o nivel do curso
        /// </summary>
        /// <param name="nomeFonteSigla">Parte do nome da fonte.</param>
        /// <param name="idNivelCurso">Nivel do Curso </param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de fontes.</returns>
        /// <remarks>Vinicius Maciel</remarks>
        public static string[] RecuperarSiglaNomeFonteNivelCurso(string nomeFonteSigla, int? idNivelCurso, int numeroRegistros)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter("@Sig_Fonte", SqlDbType.VarChar, 80), 
                    new SqlParameter("@Idf_Nivel_Curso", SqlDbType.Int, 4), 
                    new SqlParameter("@Count", SqlDbType.Int, 4)
                };

            parms[0].Value = nomeFonteSigla;

            if (idNivelCurso.HasValue)
                parms[1].Value = idNivelCurso;
            else
                parms[1].Value = DBNull.Value;

            parms[2].Value = numeroRegistros;

            var lstNomeFonte = new List<string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Splistarfontepornivelcurso, parms))
            {
                while (dr.Read())
                    lstNomeFonte.Add(dr["Sig_Fonte"] + " - " + dr["Nme_Fonte"]);

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lstNomeFonte.ToArray();
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Fonte a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var fonte = Fontes.FirstOrDefault(f => f.Identificador == this._idFonte);

                if (fonte != null)
                {
                    this.NomeFonte = fonte.Nome;
                    this.SiglaFonte = fonte.Sigla;
                    this.FlagAuditada = fonte.Auditada;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idFonte))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Fonte a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var fonte = Fontes.FirstOrDefault(f => f.Identificador == this._idFonte);

                if (fonte != null)
                {
                    this.NomeFonte = fonte.Nome;
                    this.SiglaFonte = fonte.Sigla;
                    this.FlagAuditada = fonte.Auditada;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idFonte, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

    }
}