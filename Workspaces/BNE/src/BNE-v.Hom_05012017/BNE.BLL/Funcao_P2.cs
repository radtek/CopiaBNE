//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.BLL.Custom;
using BNE.Cache;
using BNE.EL;

namespace BNE.BLL
{
    public partial class Funcao // Tabela: TAB_Funcao
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "plataforma.TAB_Funcao";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Funcoes
        private static List<FuncaoCache> Funcoes
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarFuncoesCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarFuncoesCACHE
        /// <summary>
        /// Método que retorna uma lista de itens do dicionário de parâmetros do sistema.
        /// </summary>
        /// <returns>Lista com valores recuperados do banco.</returns>
        private static List<FuncaoCache> ListarFuncoesCACHE()
        {
            var lista = new List<FuncaoCache>();

            const string spselecttodasfuncoes = @"
            SELECT  F.Idf_Funcao,
                    F.Des_Funcao,
                    F.Des_Job,
                    F.Des_Funcao_Pesquisa,
                    F.Idf_Area_BNE,
                    F.Idf_Funcao_Categoria
            FROM    plataforma.TAB_Funcao (NOLOCK) F
            WHERE   F.Flg_Inativo = 0
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodasfuncoes, null))
            {
                while (dr.Read())
                {
                    lista.Add(new FuncaoCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Funcao"]),
                        DescricaoFuncao = dr["Des_Funcao"].ToString(),
                        DescricaoJob = dr["Des_Job"].ToString(),
                        DescricaoFuncaoPesquisa = dr["Des_Funcao_Pesquisa"].ToString(),
                        IdAreaBNE = Convert.ToInt32(dr["Idf_Area_BNE"]),
                        IdFuncaoCategoria = Convert.ToInt32(dr["Idf_Funcao_Categoria"])
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

        #region FuncaoCache
        private class FuncaoCache
        {
            public int Identificador { get; set; }
            public string DescricaoFuncao { get; set; }
            public string DescricaoJob { get; set; }
            public string DescricaoFuncaoPesquisa { get; set; }
            public int IdAreaBNE { get; set; }
            public int IdFuncaoCategoria { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas

        private const string SPLISTAR = @"SELECT * FROM plataforma.TAB_Funcao (NOLOCK) ORDER BY Des_Funcao ASC";

        private const string SPSELECTDESCRICAO = @"
        SELECT  F.*
        FROM    plataforma.TAB_Funcao (NOLOCK) F
        WHERE   F.Des_Funcao = @Des_Funcao
                AND Flg_Inativo = 0";

        #region SPSELECTFUNCAOAUTOCOMPLETERHOFFICE
        private const string SPSELECTFUNCAOAUTOCOMPLETERHOFFICE =
        @"    
	        SELECT	TOP ( @Count ) F.Des_Funcao AS Des_Funcao
	        FROM    plataforma.TAB_Funcao F WITH(NOLOCK)
	        WHERE   F.Des_Funcao LIKE '%' + @Des_Funcao + '%'
			        AND F.Flg_Inativo = 0
	        ORDER BY CASE WHEN F.Des_Funcao LIKE @Des_Funcao + '%' THEN 0 ELSE 1 END , F.Des_Funcao
        ";
        #endregion

        #region SPSELECTFUNCAOIDAUTOCOMPLETERHOFFICE
        private const string SPSELECTFUNCAOIDAUTOCOMPLETERHOFFICE =
        @"SELECT	TOP ( @Count ) F.Des_Funcao AS Des_Funcao, F.Idf_Funcao
	        FROM    plataforma.TAB_Funcao F WITH(NOLOCK)
	        WHERE   F.Des_Funcao LIKE '%' + @Des_Funcao + '%'
			        AND F.Flg_Inativo = 0
	        ORDER BY CASE WHEN F.Des_Funcao LIKE @Des_Funcao + '%' THEN 0 ELSE 1 END , F.Des_Funcao";
        #endregion

        #region SPSELECTFUNCAOAUTOCOMPLETEPORAREA
        private const string SPSELECTFUNCAOAUTOCOMPLETEPORAREA = @"
DECLARE @query NVARCHAR(2000)
SET @query = ' 
SELECT 
	TOP ( @Count ) *
FROM (
	SELECT 
		F.Des_Funcao
	FROM    plataforma.TAB_Funcao ( NOLOCK ) F
	WHERE   
		F.Flg_Inativo = 0 
		AND F.Des_Funcao LIKE ''%'' + @Des_Funcao + ''%''
        AND (@Idf_Area_BNE IS NULL OR F.Idf_Area_BNE = @Idf_Area_BNE)
	UNION
	SELECT 
		F.Des_Funcao
	FROM    plataforma.TAB_Funcao ( NOLOCK ) F
	WHERE   
		F.Flg_Inativo = 0 
		AND CONTAINS (Des_Funcao, ''' + bne.BNE_Busca_MontaFormsOfComInflectional(@Des_Funcao) + ''')
        AND (@Idf_Area_BNE IS NULL OR F.Idf_Area_BNE = @Idf_Area_BNE)
    ) as temp
ORDER BY CASE WHEN Des_Funcao LIKE @Des_Funcao + ''%'' THEN 0 ELSE 1 END, Des_Funcao'

EXEC sp_executesql @query, N'@Count INT, @Des_Funcao VARCHAR(100), @Idf_Area_BNE INT', @Count = @Count, @Des_Funcao = @Des_Funcao, @Idf_Area_BNE = @Idf_Area_BNE";
        #endregion

        #region SPSELECTFUNCAOAUTOCOMPLETERHOFFICEPORAREA
        private const string SPSELECTFUNCAOAUTOCOMPLETERHOFFICEPORAREA = @"    
        IF (SELECT Flg_Todas_Funcoes FROM BNE.TAB_Origem_Filial OFil WITH(NOLOCK) WHERE OFil.Idf_Origem = @Idf_Origem) = 1
	        SELECT	TOP ( @Count ) F.Des_Funcao AS Des_Funcao
	        FROM    plataforma.TAB_Funcao F WITH(NOLOCK)
	        WHERE   F.Des_Funcao LIKE '%' + @Des_Funcao + '%'
			        AND F.Flg_Inativo = 0
                    AND (@Idf_Area_BNE IS NULL OR F.Idf_Area_BNE = @Idf_Area_BNE)
	        ORDER BY CASE WHEN F.Des_Funcao LIKE @Des_Funcao + '%' THEN 0 ELSE 1 END , F.Des_Funcao
        ELSE
            SELECT TOP ( @Count ) ISNULL (OFun.Des_Funcao, F.Des_Funcao) AS Des_Funcao
            FROM    plataforma.TAB_Funcao ( NOLOCK ) F
	                LEFT JOIN TAB_Origem_Filial_Funcao ( NOLOCK ) OFun ON F.Idf_Funcao = OFun.Idf_Funcao
	                LEFT JOIN TAB_Origem_Filial ( NOLOCK ) OFil ON OFil.Idf_Origem_Filial = OFun.Idf_Origem_Filial
            WHERE   ( F.Des_Funcao LIKE '%' + @Des_Funcao + '%' OR OFun.Des_Funcao LIKE '%' + @Des_Funcao + '%' )
                    AND F.Flg_Inativo = 0
                    AND (@Idf_Origem IS NULL OR OFil.Idf_Origem = @Idf_Origem)
                    AND (@Idf_Area_BNE IS NULL OR F.Idf_Area_BNE = @Idf_Area_BNE)
            ORDER BY CASE WHEN F.Des_Funcao LIKE @Des_Funcao + '%' THEN 0 ELSE 1 END , F.Des_Funcao ";
        #endregion

        #region SelectFuncaoBNEPorDescricaoSINE
        private const string SPSelectFuncaoBNEPorDescricaoSINE = @"
                            SELECT f.* FROM plataforma.TAB_De_Para dp WITH(NOLOCK)
	                             JOIN plataforma.TAB_Funcao f WITH(NOLOCK) ON f.Des_Funcao = dp.Des_Funcao
                            WHERE  dp.Des_Funcao_Nova = @Des_Funcao";
        #endregion

        private const string SPCONSULTAAMPLITUDESPORFUNCAO = @"SELECT 
AP.IDF_AMPLITUDE_SALARIAL,	
F.IDF_FUNCAO,
	F.DES_FUNCAO,
	AP.NR_POPULACAO,
	AP.VLR_MEDIANA,
	ISNULL(AP.VLR_AMPLITUDE_INFERIOR_ALTERADA,AP.VLR_AMPLITUDE_INFERIOR) AS VLR_AMPLITUDE_INFERIOR,
		ISNULL(AP.VLR_AMPLITUDE_SUPERIOR_ALTERADA,AP.VLR_AMPLITUDE_SUPERIOR) AS VLR_AMPLITUDE_SUPERIOR,
	AP.VLR_AMPLITUDE_INFERIOR_ALTERADA,
	AP.VLR_AMPLITUDE_SUPERIOR_ALTERADA,
    AP.Dta_Amostra
FROM PLATAFORMA.TAB_FUNCAO F
LEFT JOIN[BNE].[BNE_Amplitude_Salarial] AP ON AP.IDF_FUNCAO=F.IDF_FUNCAO
WHERE
 F.FLG_INATIVO=0
AND (@IDF_FUNCAO IS NULL OR F.IDF_FUNCAO=@IDF_FUNCAO)
AND (@Idf_Categoria IS NULL OR F.Idf_Funcao_Categoria = @IDF_CATEGORIA)";


        private const string SPINSERTAMPLITUDE =
            @"INSERT INTO BNE.BNE_AMPLITUDE_SALARIAL (IDF_FUNCAO,VLR_AMPLITUDE_INFERIOR_ALTERADA,VLR_AMPLITUDE_SUPERIOR_ALTERADA,DTA_AMOSTRA,DTA_CADASTRO)
        VALUES (@Idf_Funcao,@Vlr_Amplitude_Inferior,@Vlr_Amplitude_Superior,GetDate(),GetDate())";

        private const string SPUPDATEAMPLITUDE =
           @"UPDATE BNE.BNE_AMPLITUDE_SALARIAL SET 
                VLR_AMPLITUDE_INFERIOR_ALTERADA=@Vlr_Amplitude_Inferior,
                VLR_AMPLITUDE_SUPERIOR_ALTERADA=@Vlr_Amplitude_Superior,
                Dta_Amostra=Getdate()
                WHERE IDF_AMPLITUDE_SALARIAL=@Idf_Amplitude";


        #region Spvalidafuncaopororigem
        private const string Spvalidafuncaopororigem =
        @"
        IF ( (SELECT Flg_Todas_Funcoes FROM BNE.TAB_Origem_Filial OFil WITH(NOLOCK) WHERE OFil.Idf_Origem = @Idf_Origem) = 1 OR @Idf_Origem IS NULL )
	        SELECT	COUNT(Idf_Funcao)
	        FROM    plataforma.TAB_Funcao F WITH(NOLOCK)
	        WHERE   F.Des_Funcao = @Des_Funcao
			        AND F.Flg_Inativo = 0
        ELSE
            SELECT	COUNT(OFun.Idf_Funcao)
            FROM    plataforma.TAB_Funcao F WITH(NOLOCK)
			        LEFT JOIN BNE.TAB_Origem_Filial_Funcao OFun WITH(NOLOCK) ON F.Idf_Funcao = OFun.Idf_Funcao
			        LEFT JOIN BNE.TAB_Origem_Filial OFil WITH(NOLOCK) ON OFil.Idf_Origem_Filial = OFun.Idf_Origem_Filial
            WHERE   ( F.Des_Funcao = @Des_Funcao OR OFun.Des_Funcao = @Des_Funcao  )
                    AND F.Flg_Inativo = 0
                    AND OFil.Idf_Origem = @Idf_Origem
        ";

        private const string SpCarregaFuncaoPorOrigem =
        @"
        IF ( (SELECT Flg_Todas_Funcoes FROM BNE.TAB_Origem_Filial OFil WITH(NOLOCK) WHERE OFil.Idf_Origem = @Idf_Origem) = 1 OR @Idf_Origem IS NULL )
	        SELECT	F.*
	        FROM    plataforma.TAB_Funcao F WITH(NOLOCK)
	        WHERE   F.Des_Funcao = @Des_Funcao
			        AND F.Flg_Inativo = 0 
        ELSE
            SELECT	F.*
            FROM    plataforma.TAB_Funcao F WITH(NOLOCK)
			        LEFT JOIN BNE.TAB_Origem_Filial_Funcao OFun WITH(NOLOCK) ON F.Idf_Funcao = OFun.Idf_Funcao
			        LEFT JOIN BNE.TAB_Origem_Filial OFil WITH(NOLOCK) ON OFil.Idf_Origem_Filial = OFun.Idf_Origem_Filial
            WHERE   ( F.Des_Funcao = @Des_Funcao OR OFun.Des_Funcao = @Des_Funcao  )
                    AND F.Flg_Inativo = 0
                    AND OFil.Idf_Origem = @Idf_Origem
			        AND OFil.Idf_Filial IS NOT NULL
        ";
        #endregion

        #region Spselectdescricaojob
        private const string Spselectdescricaojob = @"
        SELECT Des_JOB FROM plataforma.TAB_Funcao F WITH(NOLOCK) WHERE Idf_Funcao = @Idf_Funcao
        ";
        #endregion

        #region Spselectareabne
        private const string Spselectareabne = @"
        SELECT Idf_Area_BNE FROM plataforma.TAB_Funcao F WITH(NOLOCK) WHERE Idf_Funcao = @Idf_Funcao
        ";
        #endregion

        #region SP_RecuperarFuncoesSinonimo
        private const string SP_RecuperarFuncoesSinonimo = @"
		SELECT f.* FROM plataforma.TAB_VW_Funcao_Sinonimo_Salariobr fSbr WITH(NOLOCK)
			JOIN plataforma.TAB_Funcao f WITH(NOLOCK) ON  f.Idf_Funcao = fSbr.Idf_Funcao_Antiga
		 WHERE fSbr.Idf_Funcao = @idfFuncao
		 AND f.Flg_Inativo = 0";
        #endregion

        #endregion

        #region ListarFuncoes
        /// <summary>
        /// Lista todas as funções.
        /// </summary>
        /// <returns></returns>
        public static List<Funcao> ListarFuncoes()
        {
            #region Cache
            if (HabilitaCache)
                return Funcoes.OrderBy(f => f.DescricaoFuncao).Select(f => new Funcao { IdFuncao = f.Identificador, DescricaoFuncao = f.DescricaoFuncao, DescricaoJob = f.DescricaoJob }).ToList();
            #endregion

            List<Funcao> retorno = new List<Funcao>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPLISTAR, null))
            {
                while (dr.Read())
                {
                    retorno.Add(SetInstanceNotDisposing(dr));
                }
            }

            return retorno;
        }
        #endregion

        #region CarregarPorDescricao
        /// <summary>
        /// Carrega um objeto da classe Função através da sua Descrição.
        /// </summary>
        /// <returns>objFuncao</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool CarregarPorDescricao(string descricaoFuncao, out Funcao objFuncao, SqlTransaction trans = null)
        {
            var retorno = false;
            objFuncao = new Funcao();

            #region Cache
            if (HabilitaCache)
            {
                var funcao = Funcoes.FirstOrDefault(f => f.DescricaoFuncao.NormalizarStringLINQ().Equals(descricaoFuncao.NormalizarStringLINQ()));
                if (funcao != null)
                {
                    objFuncao = new Funcao { IdFuncao = funcao.Identificador, DescricaoFuncao = funcao.DescricaoFuncao, DescricaoJob = funcao.DescricaoJob, AreaBNE = new AreaBNE(funcao.IdAreaBNE), FuncaoCategoria = new FuncaoCategoria(funcao.IdFuncaoCategoria) };
                    retorno = true;
                }
                else
                    objFuncao = null;

                return retorno;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_Funcao", SqlDbType = SqlDbType.VarChar, Size = 255, Value = descricaoFuncao }
                };

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTDESCRICAO, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTDESCRICAO, parms);

            if (SetInstance(dr, objFuncao))
                retorno = true;
            else
                objFuncao = null;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return retorno;
        }
        public static Funcao CarregarPorDescricao(string descricaoFuncao, SqlTransaction trans = null)
        {
            #region Cache
            if (HabilitaCache)
            {
                var funcao = Funcoes.FirstOrDefault(f => f.DescricaoFuncao.NormalizarStringLINQ().Equals(descricaoFuncao.NormalizarStringLINQ()));
                if (funcao != null)
                    return new Funcao { IdFuncao = funcao.Identificador, DescricaoFuncao = funcao.DescricaoFuncao, DescricaoJob = funcao.DescricaoJob, FuncaoCategoria = new FuncaoCategoria(funcao.IdFuncaoCategoria) };

                return null;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_Funcao", SqlDbType = SqlDbType.VarChar, Size = 255, Value = descricaoFuncao }
                };

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSELECTDESCRICAO, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTDESCRICAO, parms);

            var objFuncao = new Funcao();
            if (!SetInstance(dr, objFuncao))
                objFuncao = null;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return objFuncao;
        }
        #endregion

        #region RecuperarFuncoes
        /// <summary>
        /// Método que retorna uma lista de funcoes
        /// </summary>
        /// <param name="nomeParcial">Parte do nome da funcao.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes de funcoes.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static string[] RecuperarFuncoes(string nomeParcial, int numeroRegistros, int? idOrigem)
        {
            List<string> funcoes = new List<string>();

            using (IDataReader dr = Funcao.ListarPorNomeParcial(nomeParcial, numeroRegistros, idOrigem))
            {
                while (dr.Read())
                    funcoes.Add(dr["Des_Funcao"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return funcoes.ToArray();
        }

        #region RecuperarFuncoesComId
        /// <summary>
        /// Método que retorna uma lista de funcoes com ID
        /// </summary>
        /// <param name="nomeParcial">Parte do nome da funcao.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes de funcoes.</returns>
        /// <remarks>Luan Fernandes</remarks>
        public static Dictionary<int, string> RecuperarFuncoesComId(string nomeParcial, int numeroRegistros, int? idOrigem)
        {
            var funcoes = new Dictionary<int, string>();

            using (IDataReader dr = Funcao.ListarPorNomeParcial(nomeParcial, numeroRegistros))
            {
                while (dr.Read())
                    if (!funcoes.ContainsKey(Convert.ToInt32(dr["Idf_Funcao"])))
                        funcoes.Add(Convert.ToInt32(dr["Idf_Funcao"]), dr["Des_Funcao"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return funcoes;
        }

        #endregion

        #endregion

        #region RecuperarFuncoesPorArea
        /// <summary>
        /// Método que retorna uma lista de funcoes
        /// </summary>
        /// <param name="nomeParcial">Parte do nome da funcao.</param>
        /// <param name="numeroRegistros">Quantidade de registros a serem retornados.</param>
        /// <returns>Lista de nomes de funcoes.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static string[] RecuperarFuncoesPorArea(string nomeParcial, int numeroRegistros, int? idOrigem, int? idAreaBNE)
        {
            List<string> funcoes = new List<string>();

            using (IDataReader dr = Funcao.ListarPorNomeParcialArea(nomeParcial, numeroRegistros, idOrigem, idAreaBNE))
            {
                while (dr.Read())
                    funcoes.Add(dr["Des_Funcao"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return funcoes.ToArray();
        }
        #endregion

        #region ListarPorNomeParcial
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader ListarPorNomeParcial(string nome, int numeroRegistros, int? idOrigem)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 80),
                                new SqlParameter("@Count", SqlDbType.Int, 4)
                            };

            parms[0].Value = nome.Replace(",", string.Empty); //Removendo a vírgula para evitar erro na fulltext
            parms[1].Value = numeroRegistros;

            if (idOrigem.HasValue)
            {
                parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));
                parms[2].Value = idOrigem.Value;

                return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTFUNCAOAUTOCOMPLETERHOFFICE, parms);
            }

            return DataAccessLayer.ExecuteReader(CommandType.StoredProcedure, "TAG_SP_Palavra_Funcao", parms);
        }

        private static IDataReader ListarPorNomeParcial(string nome, int numeroRegistros)
        {
            var parms = new List<SqlParameter>
                            {
                                new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 80),
                                new SqlParameter("@Count", SqlDbType.Int, 4)
                            };

            parms[0].Value = nome.Replace(",", string.Empty); //Removendo a vírgula para evitar erro na fulltext
            parms[1].Value = numeroRegistros;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTFUNCAOIDAUTOCOMPLETERHOFFICE, parms);
        }
        #endregion

        #region ListarPorNomeParcialArea
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader ListarPorNomeParcialArea(string nome, int numeroRegistros, int? idOrigem, int? idAreaBNE)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Des_Funcao", SqlDbType.VarChar, 80));
            parms.Add(new SqlParameter("@Count", SqlDbType.Int, 4));
            parms.Add(new SqlParameter("@Idf_Area_BNE", SqlDbType.Int, 4));

            parms[0].Value = nome;
            parms[1].Value = numeroRegistros;

            if (idAreaBNE.HasValue)
                parms[2].Value = (int)idAreaBNE;
            else
                parms[2].Value = DBNull.Value;

            if (idOrigem.HasValue)
            {
                parms.Add(new SqlParameter("@Idf_Origem", SqlDbType.Int, 4));
                parms[3].Value = idOrigem.Value;

                return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTFUNCAOAUTOCOMPLETERHOFFICEPORAREA, parms);
            }
            else
                return DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTFUNCAOAUTOCOMPLETEPORAREA, parms);
        }
        #endregion

        #region CarregarAmplitudesPorFuncao
        public static DataTable CarregarAmplitudesPorFuncao(int? idfFuncao, int? idfCategoria)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Int));
            parms.Add(new SqlParameter("@Idf_Categoria", SqlDbType.Int));
            if (idfFuncao == null)
                parms[0].Value = DBNull.Value;
            else
                parms[0].Value = idfFuncao;

            if (idfCategoria == 0)
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = idfCategoria;

            DataSet ds = DataAccessLayer.ExecuteReaderDs(CommandType.Text, SPCONSULTAAMPLITUDESPORFUNCAO, parms);

            return ds.Tables[0];

        }
        #endregion

        #region SalvarAmplitude
        public bool SalvarAmplitude(int? idAmplitude, int idfFuncao, decimal? limiteInferior, decimal? limiteSuperior)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Vlr_Amplitude_Inferior", SqlDbType.Decimal));
            parms.Add(new SqlParameter("@Vlr_Amplitude_Superior", SqlDbType.Decimal));
            parms.Add(new SqlParameter("@Idf_Amplitude", SqlDbType.Decimal));
            parms.Add(new SqlParameter("@Idf_Funcao", SqlDbType.Decimal));

            if (limiteInferior == null)
                parms[0].Value = DBNull.Value;
            else
                parms[0].Value = limiteInferior;

            if (limiteSuperior == null)
                parms[1].Value = DBNull.Value;
            else
                parms[1].Value = limiteSuperior;

            if (idAmplitude == null)
                parms[2].Value = DBNull.Value;
            else
                parms[2].Value = idAmplitude;

            parms[3].Value = idfFuncao;

            if (idAmplitude == null)
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPINSERTAMPLITUDE, parms);
            else
                DataAccessLayer.ExecuteNonQuery(CommandType.Text, SPUPDATEAMPLITUDE, parms);

            return true;
        }
        #endregion

        #region ValidarFuncaoPorOrigem
        public static bool ValidarFuncaoPorOrigem(int? idOrigem, string desFuncao)
        {
            Object valueOrigem = DBNull.Value;

            if (idOrigem.HasValue && !idOrigem.Value.Equals((int)Enumeradores.Origem.BNE))
            {
                if (idOrigem.Value.Equals((int)Enumeradores.Origem.BNE))
                    valueOrigem = idOrigem;
            }

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Origem", SqlDbType = SqlDbType.Int, Size = 4, Value = valueOrigem } ,
                    new SqlParameter { ParameterName = "@Des_Funcao", SqlDbType = SqlDbType.VarChar, Size = 50, Value = desFuncao }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spvalidafuncaopororigem, parms)) > 0;
        }
        #endregion

        #region CarregarFuncaoPorOrigem
        public static bool CarregarFuncaoPorOrigem(int? idOrigem, string desFuncao, out Funcao objFuncao)
        {
            Object valueOrigem = DBNull.Value;

            if (idOrigem.HasValue && !idOrigem.Value.Equals((int)Enumeradores.Origem.BNE))
            {
                if (idOrigem.Value.Equals((int)Enumeradores.Origem.BNE))
                    valueOrigem = idOrigem;
            }

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Origem", SqlDbType = SqlDbType.Int, Size = 4, Value = valueOrigem } ,
                    new SqlParameter { ParameterName = "@Des_Funcao", SqlDbType = SqlDbType.VarChar, Size = 50, Value = desFuncao }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpCarregaFuncaoPorOrigem, parms))
            {
                objFuncao = new Funcao();
                if (SetInstance(dr, objFuncao))
                    return true;
            }

            return false;
        }
        #endregion

        #region ValidarFuncaoLimitacaoIntegracaoWebEstagios
        public static bool ValidarFuncaoLimitacaoIntegracaoWebEstagios(string desFuncao)
        {
            if (string.IsNullOrWhiteSpace(desFuncao))
                return false;

            if (StringComparer.OrdinalIgnoreCase.Equals(Helper.RemoverAcentos(desFuncao), "Estagiario"))
                return false;

            if (StringComparer.OrdinalIgnoreCase.Equals(desFuncao, "Aprendiz"))
                return false;

            return true;
        }
        #endregion

        #region RecuperarDescricaoJob
        public string RecuperarDescricaoJob()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFuncao }
                };

            return Convert.ToString(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectdescricaojob, parms));
        }
        #endregion

        #region RecuperarAreaBNE
        public int RecuperarAreaBNE()
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Funcao", SqlDbType = SqlDbType.Int, Size = 4, Value = _idFuncao }
                };

            return Convert.ToInt32(DataAccessLayer.ExecuteScalar(CommandType.Text, Spselectareabne, parms));
        }
        #endregion

        #region SetInstanceNotDisposing
        /// <summary>
        /// Método auxiliar para percorrer um IDataReader e vincular as colunas com os atributos da classe sem dar o dispose no DataReader.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objFuncao">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Vinicius Maciel</remarks>
        private static Funcao SetInstanceNotDisposing(IDataReader dr)
        {
            try
            {
                Funcao objFuncao = new Funcao();
                objFuncao._idFuncao = Convert.ToInt32(dr["Idf_Funcao"]);
                if (dr["Idf_Funcao_Agrupadora"] != DBNull.Value)
                    objFuncao._funcaoAgrupadora = new Funcao(Convert.ToInt32(dr["Idf_Funcao_Agrupadora"]));
                objFuncao._descricaoFuncao = Convert.ToString(dr["Des_Funcao"]);
                objFuncao._funcaoCategoria = new FuncaoCategoria(Convert.ToInt32(dr["Idf_Funcao_Categoria"]));
                if (dr["Idf_Classe_Salarial"] != DBNull.Value)
                    objFuncao._classeSalarial = new ClasseSalarial(Convert.ToInt32(dr["Idf_Classe_Salarial"]));
                if (dr["Idf_Escolaridade"] != DBNull.Value)
                    objFuncao._escolaridade = new Escolaridade(Convert.ToInt32(dr["Idf_Escolaridade"]));
                if (dr["Idf_Area_BNE"] != DBNull.Value)
                    objFuncao._areaBNE = new AreaBNE(Convert.ToInt32(dr["Idf_Area_BNE"]));
                if (dr["Flg_Menor_Aprendiz"] != DBNull.Value)
                    objFuncao._flagMenorAprendiz = Convert.ToBoolean(dr["Flg_Menor_Aprendiz"]);
                if (dr["Des_Job"] != DBNull.Value)
                    objFuncao._descricaoJob = Convert.ToString(dr["Des_Job"]);
                if (dr["Des_Experiencia"] != DBNull.Value)
                    objFuncao._descricaoExperiencia = Convert.ToString(dr["Des_Experiencia"]);
                if (dr["Vlr_Piso_Profissional"] != DBNull.Value)
                    objFuncao._valorPisoProfissional = Convert.ToDecimal(dr["Vlr_Piso_Profissional"]);
                if (dr["Des_Cursos"] != DBNull.Value)
                    objFuncao._descricaoCursos = Convert.ToString(dr["Des_Cursos"]);
                if (dr["Des_Competencias"] != DBNull.Value)
                    objFuncao._descricaoCompetencias = Convert.ToString(dr["Des_Competencias"]);
                objFuncao._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
                objFuncao._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                objFuncao._descricaoFuncaoPesquisa = Convert.ToString(dr["Des_Funcao_Pesquisa"]);
                if (dr["Cod_Funcao_Folha"] != DBNull.Value)
                    objFuncao._codigoFuncaoFolha = Convert.ToString(dr["Cod_Funcao_Folha"]);
                objFuncao._codigoCBO = Convert.ToString(dr["Cod_CBO"]);
                objFuncao._flagConselho = Convert.ToBoolean(dr["Flg_Conselho"]);
                objFuncao._flagCursoEspecializacao = Convert.ToBoolean(dr["Flg_Curso_Especializacao"]);
                if (dr["Flg_RAIS_Analfabeto"] != DBNull.Value)
                    objFuncao._flagRAISAnalfabeto = Convert.ToBoolean(dr["Flg_RAIS_Analfabeto"]);
                if (dr["Flg_RAIS_Nivel_Superior"] != DBNull.Value)
                    objFuncao._flagRAISNivelSuperior = Convert.ToBoolean(dr["Flg_RAIS_Nivel_Superior"]);
                objFuncao._flagCategoriaSindical = Convert.ToBoolean(dr["Flg_Categoria_Sindical"]);
                objFuncao._flagAuditada = Convert.ToBoolean(dr["Flg_Auditada"]);
                if (dr["Qtd_Carga_Horaria_Diaria"] != DBNull.Value)
                    objFuncao._quantidadeCargaHorariaDiaria = Convert.ToInt16(dr["Qtd_Carga_Horaria_Diaria"]);
                if (dr["Des_Local_Trabalho"] != DBNull.Value)
                    objFuncao._descricaoLocalTrabalho = Convert.ToString(dr["Des_Local_Trabalho"]);
                if (dr["Des_EPI"] != DBNull.Value)
                    objFuncao._descricaoEPI = Convert.ToString(dr["Des_EPI"]);
                if (dr["Des_PPRA"] != DBNull.Value)
                    objFuncao._descricaoPPRA = Convert.ToString(dr["Des_PPRA"]);
                if (dr["Des_PCMSO"] != DBNull.Value)
                    objFuncao._descricaoPCMSO = Convert.ToString(dr["Des_PCMSO"]);
                if (dr["Des_Equipamentos"] != DBNull.Value)
                    objFuncao._descricaoEquipamentos = Convert.ToString(dr["Des_Equipamentos"]);

                objFuncao._persisted = true;
                objFuncao._modified = false;

                return objFuncao;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region CarregarFuncaoBNEPorDescricaoSINE
        /// <summary>
        /// Processo que recupera a Função no BNE com base na função do SINE
        /// </summary>
        /// <param name="descricaoFuncao"></param>
        /// <param name="objFuncao"></param>
        /// <param name="trans"></param>
        /// <returns></returns>
        public static bool CarregarFuncaoBNEPorDescricaoSINE(string descricaoFuncao, out Funcao objFuncao, SqlTransaction trans = null)
        {
            var retorno = false;
            objFuncao = new Funcao();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_Funcao", SqlDbType = SqlDbType.VarChar, Size = 255, Value = descricaoFuncao }
                };

            IDataReader dr;
            if (trans != null)
                dr = DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSelectFuncaoBNEPorDescricaoSINE, parms);
            else
                dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSelectFuncaoBNEPorDescricaoSINE, parms);

            if (SetInstance(dr, objFuncao))
                retorno = true;
            else
                objFuncao = null;

            if (!dr.IsClosed)
                dr.Close();

            dr.Dispose();

            return retorno;
        }
        #endregion CarregarFuncaoBNEPorDescricaoSINE

        /// <summary>
        /// Busca a funcao através da descricao utilizando uma árvore B construída baseada nas descrições das funcoes cadastradas.
        /// </summary>
        /// <param name="descricaoFuncao">Descrição a ser pesquisada</param>
        /// <returns>Objeto Funcao detectado</returns>
        public static Funcao DetectarFuncao(String descricaoFuncao)
        {
            return Custom.ArvoreBuscaFuncao.GetInstance().Buscar(descricaoFuncao);
        }

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Funcao a partir do banco de dados.
        /// </summary>
        /// <param name="idFuncao">Chave do registro.</param>
        /// <returns>Instância de Funcao.</returns>
        /// <remarks>Vinicius Maciel</remarks>
        public static Funcao LoadObject(int idFuncao)
        {
                #if !DEBUG
                            #region Cache
                            if (HabilitaCache)
                            {
                                var funcao = Funcoes.FirstOrDefault(f => f.Identificador == idFuncao);

                                if (funcao != null)
                                    return new Funcao { IdFuncao = funcao.Identificador, DescricaoFuncao = funcao.DescricaoFuncao, DescricaoJob = funcao.DescricaoJob, DescricaoFuncaoPesquisa = funcao.DescricaoFuncaoPesquisa, AreaBNE = new AreaBNE(funcao.IdAreaBNE), FuncaoCategoria = new FuncaoCategoria(funcao.IdFuncaoCategoria) };

                                throw (new RecordNotFoundException(typeof(Funcao)));
                            }
                            #endregion
                #endif


            using (IDataReader dr = LoadDataReader(idFuncao))
            {
                Funcao objFuncao = new Funcao();
                if (SetInstance(dr, objFuncao))
                    return objFuncao;
            }
            throw (new RecordNotFoundException(typeof(Funcao)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Funcao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idFuncao">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Funcao.</returns>
        /// <remarks>Vinicius Maciel</remarks>
        public static Funcao LoadObject(int idFuncao, SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var funcao = Funcoes.FirstOrDefault(f => f.Identificador == idFuncao);

                if (funcao != null)
                    return new Funcao { IdFuncao = funcao.Identificador, DescricaoFuncao = funcao.DescricaoFuncao, DescricaoJob = funcao.DescricaoJob, DescricaoFuncaoPesquisa = funcao.DescricaoFuncaoPesquisa, AreaBNE = new AreaBNE(funcao.IdAreaBNE), FuncaoCategoria = new FuncaoCategoria(funcao.IdFuncaoCategoria) };

                throw (new RecordNotFoundException(typeof(Funcao)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idFuncao, trans))
            {
                Funcao objFuncao = new Funcao();
                if (SetInstance(dr, objFuncao))
                    return objFuncao;
            }
            throw (new RecordNotFoundException(typeof(Funcao)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Funcao a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Vinicius Maciel</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var funcao = Funcoes.FirstOrDefault(f => f.Identificador == this._idFuncao);
                if (funcao != null)
                {
                    this.DescricaoFuncao = funcao.DescricaoFuncao;
                    this.DescricaoJob = funcao.DescricaoJob;
                    this.DescricaoFuncaoPesquisa = funcao.DescricaoFuncaoPesquisa;
                    this.AreaBNE = new AreaBNE(funcao.IdAreaBNE);
                    this.FuncaoCategoria = new FuncaoCategoria(funcao.IdFuncaoCategoria);
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idFuncao))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Funcao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Vinicius Maciel</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var funcao = Funcoes.FirstOrDefault(f => f.Identificador == this._idFuncao);
                if (funcao != null)
                {
                    this.DescricaoFuncao = funcao.DescricaoFuncao;
                    this.DescricaoJob = funcao.DescricaoJob;
                    this.DescricaoFuncaoPesquisa = funcao.DescricaoFuncaoPesquisa;
                    this.AreaBNE = new AreaBNE(funcao.IdAreaBNE);
                    this.FuncaoCategoria = new FuncaoCategoria(funcao.IdFuncaoCategoria);
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idFuncao, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion


        #region RecuperarFuncoesSinonimo
        public static List<Funcao> RecuperarFuncoesSinonimo(int idFuncao)
        {
            List<Funcao> funcoes = new List<Funcao>();

            var parms = new List<SqlParameter>()
            {
                new SqlParameter{ ParameterName = "@idfFuncao", Size = 4, SqlDbType = SqlDbType.Int, SqlValue =  idFuncao}
            };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SP_RecuperarFuncoesSinonimo, parms))
            {
                while(dr.Read())
                {
                    Funcao objFuncao = SetInstanceNotDisposing(dr);

                    if (objFuncao != null)
                        funcoes.Add(objFuncao);
                }
            }

            return funcoes;
        }
        #endregion

    }
}