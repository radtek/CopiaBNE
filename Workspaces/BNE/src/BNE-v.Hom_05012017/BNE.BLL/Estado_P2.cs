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

namespace BNE.BLL
{
    public partial class Estado // Tabela: TAB_Estado
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "plataforma.TAB_Estado";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Estados
        private static List<EstadoCache> Estados
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarEstadosCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarEstadosCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<EstadoCache> ListarEstadosCACHE()
        {
            var listaCidades = new List<EstadoCache>();

            const string spselecttodosestados = @"
            SELECT  Est.Idf_Estado,
                    Est.Nme_Estado,
                    Est.Sig_Estado 
            FROM    plataforma.TAB_Estado Est WITH(NOLOCK) 
            WHERE   Est.Flg_Inativo = 0
            ORDER BY Est.Nme_Estado asc
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodosestados, null))
            {
                while (dr.Read())
                {
                    listaCidades.Add(new EstadoCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Estado"]),
                        NomeEstado = dr["Nme_Estado"].ToString(),
                        SiglaEstado = dr["Sig_Estado"].ToString()
                    });
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return listaCidades;
        }
        #endregion

        #endregion

        #region CacheObjects

        #region EstadoCache
        private class EstadoCache
        {
            public int Identificador { get; set; }
            public string NomeEstado { get; set; }
            public string SiglaEstado { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Construtores
        public Estado(string siglaEstado)
        {
            _siglaEstado = siglaEstado;
            _persisted = true;
        }
        #endregion

        #region Consultas

        #region Spselect
        private const string Spselect = "SELECT Sig_Estado, Nme_Estado FROM plataforma.TAB_Estado WITH(NOLOCK) ORDER BY Nme_Estado ASC";
        #endregion

        #region Spselectsiglaestado
        private const string Spselectsiglaestado = "SELECT * FROM plataforma.TAB_Estado WITH(NOLOCK) WHERE Sig_Estado = @Sig_Estado";
        #endregion

        #region SpselectNomeEstado
        private const string SpselectNomeEstado = "SELECT * FROM plataforma.TAB_Estado WITH(NOLOCK) WHERE Nme_Estado = @Nme_Estado";
        #endregion

        #endregion

        #region Métodos

        #region Listar
        /// <summary>
        /// Método utilizado para retornar todos os registros de Estado do banco de dados.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Dictionary<string, string> Listar()
        {
            #region Cache
            if (HabilitaCache)
                return Estados.ToDictionary(e => e.SiglaEstado, e => e.NomeEstado);
            #endregion

            var dicionario = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselect, null))
            {
                while (dr.Read())
                {
                    dicionario.Add(dr["Sig_Estado"].ToString(), dr["Sig_Estado"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionario;
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Estado a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var estado = Estados.FirstOrDefault(e => e.SiglaEstado.NormalizarStringLINQ().Equals(this._siglaEstado.NormalizarStringLINQ()));

                if (estado != null)
                {
                    this.IdEstado = estado.Identificador;
                    this.NomeEstado = estado.NomeEstado;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(_siglaEstado))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #region LoadDataReader
        /// <summary>
        /// Método utilizado por retornar as colunas de um registro no banco de dados.
        /// </summary>
        /// <param name="siglaEstado">Sigla do Estado.</param>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static IDataReader LoadDataReader(string siglaEstado)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Sig_Estado", SqlDbType = SqlDbType.Char, Size = 2, Value = siglaEstado }
                };

            return DataAccessLayer.ExecuteReader(CommandType.Text, Spselectsiglaestado, parms);
        }
        #endregion

        #region CarregarPorSiglaEstado
        /// <summary>
        /// Carrega objeto Estado através da sigla do Estado
        /// </summary>
        /// <param name="siglaEstado"></param>
        /// <returns></returns>
        public static Estado CarregarPorSiglaEstado(string siglaEstado)
        {
            #region Cache
            if (HabilitaCache)
            {
                var estado = Estados.FirstOrDefault(e => e.SiglaEstado.NormalizarStringLINQ().Equals(siglaEstado.NormalizarStringLINQ()));
                if (estado != null)
                    return new Estado { IdEstado = estado.Identificador, NomeEstado = estado.NomeEstado, SiglaEstado = estado.SiglaEstado };

                return null;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Sig_Estado", SqlDbType = SqlDbType.Char, Size = 2, Value = siglaEstado }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectsiglaestado, parms))
            {
                var objEstado = new Estado();
                if (SetInstance(dr, objEstado))
                    return objEstado;
            }

            return null;
        }
        #endregion

        #region CarregarPorNome
        /// <summary>
        /// Carrega objeto Estado através da sigla do Estado
        /// </summary>
        /// <param name="siglaEstado"></param>
        /// <returns></returns>
        public static Estado CarregarPorNome(string nome)
        {
            #region Cache
            if (HabilitaCache)
            {
                var estado = Estados.FirstOrDefault(e => e.NomeEstado.NormalizarStringLINQ().Equals(nome.NormalizarStringLINQ()));
                if (estado != null)
                    return new Estado { IdEstado = estado.Identificador, NomeEstado = estado.NomeEstado, SiglaEstado = estado.SiglaEstado };

                return null;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Nme_Estado", SqlDbType = SqlDbType.VarChar, Size = 200, Value = nome }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpselectNomeEstado, parms))
            {
                var objEstado = new Estado();
                if (SetInstance(dr, objEstado))
                    return objEstado;
            }

            return null;
        }
        #endregion

        #endregion

    }
}