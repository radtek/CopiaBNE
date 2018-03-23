//-- Data: 21/05/2013 11:22
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.Cache;
using BNE.EL;
using System.Runtime.Caching;

namespace BNE.BLL
{
    public partial class Rota // Tabela: BNE_Rota
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "BNE_Rota";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Rotas
        private static List<RotaCache> Rotas
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarRotasCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarRotasCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<RotaCache> ListarRotasCACHE()
        {
            var lista = new List<RotaCache>();

            const string spselecttodasrotas = @"
            SELECT  Idf_Rota,
                    Nme_Rota,
                    Des_URL,
                    Des_Caminho_Fisico,
                    Num_Peso,
                    Flg_Ignore
            FROM BNE_Rota WITH(NOLOCK) 
            WHERE Flg_Inativo = 0 
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodasrotas, null))
            {
                while (dr.Read())
                {
                    lista.Add(new RotaCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Rota"]),
                        Nome = dr["Nme_Rota"].ToString(),
                        URL = dr["Des_URL"].ToString(),
                        CaminhoFisico = dr["Des_Caminho_Fisico"].ToString(),
                        Peso = Convert.ToInt32(dr["Num_Peso"]),
                        Ignore = Convert.ToBoolean(dr["Flg_Ignore"])
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

        #region RotaCache
        private class RotaCache
        {
            public int Identificador { get; set; }
            public string Nome { get; set; }
            public string URL { get; set; }
            public string CaminhoFisico { get; set; }
            public int Peso { get; set; }
            public bool Ignore { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string SPSelect_PorNomeRota = "SELECT * FROM BNE_Rota WITH(NOLOCK) WHERE Nme_Rota = @Nme_Rota";

        #region Sprecuperarrotas
        private const string Sprecuperarrotas = "SELECT * FROM BNE_Rota WHERE Flg_Inativo = 0 ORDER BY Num_Peso DESC";
        #endregion

        #region Sprecuperarurlrota
        private const string Sprecuperarurlrota = @"
        SELECT  Des_URL
        FROM    BNE_Rota R WITH(NOLOCK)
        WHERE   R.Idf_Rota = @Idf_Rota";
        #endregion

        #region spBuscaIdPorDesUrl
        private const string spBuscaIdPorDesUrl = @"
                    select rv.url_rota_video, rv.des_url_img_video, rv.Nme_Video from bne_rota r with(nolock)
                    join bne_rota_video rv with(nolock) on rv.idf_rota = r.idf_rota
                     where r.Des_Url = @Des_Url and r.flg_inativo = 0";
        #endregion
        #endregion

        #region Métodos

        #region RecuperarRotas
        /// <summary>
        /// Recupera todas as rotas ativa do site
        /// </summary>
        public static List<Rota> RecuperarRotas()
        {
            #region Cache
            if (HabilitaCache)
                return Rotas.OrderByDescending(r => r.Peso).Select(r => new Rota { IdRota = r.Identificador, NomeRota = r.Nome, DescricaoURL = r.URL, DescricaoCaminhoFisico = r.CaminhoFisico, NumeroPeso = r.Peso, FlagIgnore = r.Ignore }).ToList();
            #endregion

            var lista = new List<Rota>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarrotas, null))
            {
                while (dr.Read())
                {
                    var objRota = new Rota();
                    if (SetInstance_NonDipose(dr, objRota))
                        lista.Add(objRota);
                }
            }

            return lista;
        }
        #endregion

        #region SetInstance_NonDipose
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objRota">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NonDipose(IDataReader dr, Rota objRota)
        {
            objRota._idRota = Convert.ToInt32(dr["Idf_Rota"]);
            objRota._nomeRota = Convert.ToString(dr["Nme_Rota"]);
            objRota._descricaoURL = Convert.ToString(dr["Des_URL"]);
            objRota._descricaoCaminhoFisico = Convert.ToString(dr["Des_Caminho_Fisico"]);
            objRota._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            objRota._numeroPeso = Convert.ToInt32(dr["Num_Peso"]);
            objRota._flagIgnore = Convert.ToBoolean(dr["Flg_Ignore"]);

            objRota._persisted = true;
            objRota._modified = false;

            return true;
        }
        #endregion

        #region RecuperarURLRota
        /// <summary>
        /// Método que recupera o valor de uma rota a partir do id.
        /// </summary>
        /// <param name="enumeradorRota">Identificador da rota.</param>
        /// <returns>Valor do parâmetro.</returns>
        public static string RecuperarURLRota(Enumeradores.RouteCollection enumeradorRota)
        {
            #region Cache
            if (HabilitaCache)
            {
                var rota = Rotas.FirstOrDefault(r => r.Identificador.Equals((int)enumeradorRota));

                if (rota != null)
                    return rota.URL;

                return string.Empty;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Rota", SqlDbType = SqlDbType.Int, Size = 4, Value = Convert.ToInt32(enumeradorRota)}
                };

            var retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, Sprecuperarurlrota, parms);

            if (!Convert.IsDBNull(retorno))
                return retorno.ToString();

            return string.Empty;
        }
        #endregion

        #region BuscarPorNomeRoute
        /// <summary>   
        /// Método responsável por buscar uma instância de ParametrosSEO pelo parâmetro desRouteName.
        /// </summary>
        /// <param name="desRouteName">Nome do Route</param>
        /// <returns>Objeto Parametros SEO</returns>
        public static Rota BuscarPorRouteName(string nomeRota)
        {
            String cacheKey = String.Format("parametroSEO:{0}", nomeRota);

            if (MemoryCache.Default[cacheKey] != null)
            {
                return (Rota)MemoryCache.Default.Get(cacheKey);
            }

            Rota objRotaSEO = Rota.LoadObject_PorNomeRota(nomeRota);

            if (objRotaSEO == null)
            {
                objRotaSEO = new Rota();
                objRotaSEO.NomeRota = nomeRota;
            }

            CacheItemPolicy policy = new CacheItemPolicy();

            policy.AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MinutosLimpezaCache)));

            MemoryCache.Default.Add(cacheKey, objRotaSEO, policy);

            return objRotaSEO;
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de ParametrosSEO a partir do banco de dados.
        /// </summary>
        /// <param name="desRouteName">Chave do registro.</param>
        /// <returns>Instância de ParametrosSEO.</returns>
        public static Rota LoadObject_PorNomeRota(string nomeRota)
        {
            using (IDataReader dr = LoadDataReader_PorNomeRota(nomeRota))
            {
                Rota objRotaSEO = new Rota();
                if (SetInstance(dr, objRotaSEO))
                    return objRotaSEO;
            }

            return null;
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de ParametrosSEO a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="desRouteName">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de ParametrosSEO.</returns>
        public static Rota LoadObject_PorNomeRota(string nomeRota, SqlTransaction trans)
        {
            using (IDataReader dr = LoadDataReader_PorNomeRota(nomeRota, trans))
            {
                Rota objRotaSEO = new Rota();
                if (SetInstance(dr, objRotaSEO))
                    return objRotaSEO;
            }
            throw (new RecordNotFoundException(typeof(Rota)));
        }
        #endregion

        #region LoadDataReader_PorNomeRota
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="desRouteName">Chave do registro.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        private static IDataReader LoadDataReader_PorNomeRota(string nomeRota)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Nme_Rota", SqlDbType.VarChar, 200));

            parms[0].Value = nomeRota;

            return DataAccessLayer.ExecuteReader(CommandType.Text, SPSelect_PorNomeRota, parms);
        }
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="desRouteName">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        private static IDataReader LoadDataReader_PorNomeRota(string nomeRota, SqlTransaction trans)
        {
            List<SqlParameter> parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Nme_Rota", SqlDbType.VarChar, 50));

            parms[0].Value = nomeRota;

            return DataAccessLayer.ExecuteReader(trans, CommandType.Text, SPSelect_PorNomeRota, parms);
        }
        #endregion


        #region [Busca por Des_Url]
        public static string BuscaVideoPorDesUrl(string DesUrl, out string urlImg, out string NmeVideo)
        {
            try
            {
                DesUrl = DesUrl.Replace("/", "");
                string cacheKey = String.Format("RotaVideo:{0}", DesUrl);
                string cacheKey2 = String.Format("ImgVideo:{0}", DesUrl);
                string cacheKey3 = String.Format("NmeVideo:{0}", DesUrl);
                string urlVideo = string.Empty;
                NmeVideo = string.Empty;
                urlImg = string.Empty;
                if (MemoryCache.Default[cacheKey] != null)
                {
                    urlImg = (string)MemoryCache.Default.Get(cacheKey2);
                    NmeVideo = (string)MemoryCache.Default.Get(cacheKey3);
                    return (string)MemoryCache.Default.Get(cacheKey);
                }

                var parms = new List<SqlParameter>
            {
                new SqlParameter {ParameterName = "@Des_Url", SqlDbType = SqlDbType.VarChar, Size =60, Value = DesUrl}
            };

                using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spBuscaIdPorDesUrl, parms))
                {
                    if (dr.Read())
                    {
                        urlVideo = dr["Url_Rota_Video"].ToString();
                        urlImg = dr["Des_Url_Img_Video"].ToString();
                        NmeVideo = dr["Nme_Video"].ToString();
                    }

                }
                CacheItemPolicy policy = new CacheItemPolicy();
                policy.AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MinutosLimpezaCache)));
                MemoryCache.Default.Add(cacheKey, urlVideo, policy);
                MemoryCache.Default.Add(cacheKey2, urlImg, policy);
                MemoryCache.Default.Add(cacheKey3, NmeVideo, policy);
                return urlVideo;
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Metodo BuscaIdPorDesUrl");
                urlImg = string.Empty;
                NmeVideo = string.Empty;
                return string.Empty;
            }

        }
        #endregion


        #endregion
    }
}