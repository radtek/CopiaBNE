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
    public partial class Idioma // Tabela: TAB_Idioma
    {
        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "bne.Tab_Idioma";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null
                                                    && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects
        private class IdiomaCache
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }

        #region Idiomas
        private static List<IdiomaCache> Idiomas
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarIdiomaCache, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #endregion

        #region ListarDisponibilidadesCACHE
        /// <summary>
        /// Método que retorna uma lista de itens do sistema.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<IdiomaCache> ListarIdiomaCache()
        {
            var lista = new List<IdiomaCache>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECTIDIOMA, null))
            {
                while (dr.Read())
                {
                    lista.Add(new IdiomaCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Idioma"]),
                        Descricao = dr["Des_Idioma"].ToString()
                    });
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #region Consultas
        private const string SELECTIDIOMA = @"      
        SELECT  Idf_Idioma,
                Des_Idioma 
        FROM    TAB_Idioma WITH(NOLOCK)
        WHERE Flg_Inativo = 0
        ORDER BY Des_Idioma";

        private const string SPSELECTNOME = "SELECT * FROM TAB_Idioma WITH(NOLOCK) WHERE Des_Idioma = @Des_Idioma ";

        #endregion

        #region Métodos

        #region Listar
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public static Dictionary<string, string> Listar()
        {
            #region Cache
            if (HabilitaCache)
                return Idiomas.OrderBy(d => d.Identificador).ToDictionary(d => d.Identificador.ToString(), d => d.Descricao);
            #endregion

            var dicionarioIdioma = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECTIDIOMA, null))
            {
                while (dr.Read())
                {
                    dicionarioIdioma.Add(dr["Idf_Idioma"].ToString(), dr["Des_Idioma"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionarioIdioma;
        }
        #endregion

        #region CarregarPorNome
        /// <summary>
        /// Método utilizado para retornar uma instância de idioma a partir do banco de dados.
        /// </summary>
        /// <param name="nomeIdioma">Nome/Desc do idioma</param>
        /// <param name="objIdioma">Idioma</param>
        /// <returns>Resultado</returns>
        /// <remarks>Lennon Vidal</remarks>
        public static bool CarregarPorNome(string nomeIdioma, out Idioma objIdioma)
        {
            #region Cache
            if (HabilitaCache)
            {
                var idioma = Idiomas.FirstOrDefault(d => d.Descricao.NormalizarStringLINQ().Equals(nomeIdioma.NormalizarStringLINQ()));

                if (idioma != null)
                {
                    objIdioma = new Idioma(idioma.Identificador) { DescricaoIdioma = idioma.Descricao };
                    return true;
                }
                objIdioma = null;
                return false;
            }
            #endregion

            var parms = new List<SqlParameter>();
            parms.Add(new SqlParameter("@Des_Idioma", SqlDbType.VarChar, 80));
            parms[0].Value = nomeIdioma;

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SPSELECTNOME, parms))
            {
                objIdioma = new Idioma();
                if (SetInstance(dr, objIdioma))
                    return true;

                if (!dr.IsClosed)
                    dr.Close();
            }
            objIdioma = null;
            return false;
        }
        #endregion


        /// <summary>
        /// Método utilizado para retornar uma instância de Idioma a partir do banco de dados.
        /// </summary>
        /// <param name="idIdioma">Chave do registro.</param>
        /// <returns>Instância de Idioma.</returns>
        /// <remarks>Lennon Vidal</remarks>
        public static Idioma LoadObject(int idIdioma)
        {
            #region Cache
            if (HabilitaCache)
            {
                var idioma = Idiomas.FirstOrDefault(d => d.Identificador == idIdioma);

                if (idioma != null)
                    return new Idioma(idIdioma) { DescricaoIdioma = idioma.Descricao };

                throw new RecordNotFoundException(typeof(Disponibilidade));
            }
            #endregion

            using (var dr = LoadDataReader(idIdioma))
            {
                var objIdioma = new Idioma();
                if (SetInstance(dr, objIdioma))
                    return objIdioma;
            }
            throw (new RecordNotFoundException(typeof(Idioma)));
        }
        #endregion
    }
}