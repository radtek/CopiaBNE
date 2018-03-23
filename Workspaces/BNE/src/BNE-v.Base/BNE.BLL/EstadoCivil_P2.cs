//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.Cache;
using BNE.EL;

namespace BNE.BLL
{
    public partial class EstadoCivil // Tabela: TAB_Estado_Civil
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "plataforma.TAB_Estado_Civil";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region EstadosCivis
        private static List<EstadoCivilCACHE> EstadosCivis
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarEstadosCivisCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarEstadosCivisCACHE
        /// <summary>
        /// Método que retorna uma lista de itens do sistema.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<EstadoCivilCACHE> ListarEstadosCivisCACHE()
        {
            var lista = new List<EstadoCivilCACHE>();

            const string spselecttodosestadoscivis = @"
            SELECT  Idf_Estado_Civil,
                    Des_Estado_Civil
            FROM    plataforma.TAB_Estado_Civil WITH(NOLOCK)
            ORDER BY Des_Estado_Civil
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodosestadoscivis, null))
            {
                while (dr.Read())
                {
                    lista.Add(new EstadoCivilCACHE { Identificador = Convert.ToInt32(dr["Idf_Estado_Civil"]), Descricao = dr["Des_Estado_Civil"].ToString() });
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #endregion

        #region CacheObjects

        #region EstadoCivilCACHE
        private class EstadoCivilCACHE
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string Spestadocivl = @"
        SELECT  Idf_Estado_Civil,
                Des_Estado_Civil
        FROM    plataforma.TAB_Estado_Civil
        ORDER BY Des_Estado_Civil";
        #endregion

        #region Método

        #region Listar
        /// <summary>
        /// Método retorna Consulta para carregar DropDownList
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> Listar()
        {
            #region Cache
            if (HabilitaCache)
                return EstadosCivis.OrderBy(e => e.Descricao).ToDictionary(e => e.Identificador.ToString(), e => e.Descricao);
            #endregion

            var dicionario = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spestadocivl, null))
            {
                while (dr.Read())
                {
                    dicionario.Add(dr["Idf_Estado_Civil"].ToString(), dr["Des_Estado_Civil"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionario;
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de EstadoCivil a partir do banco de dados.
        /// </summary>
        /// <param name="idEstadoCivil">Chave do registro.</param>
        /// <returns>Instância de EstadoCivil.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static EstadoCivil LoadObject(int idEstadoCivil)
        {
            #region Cache
            if (HabilitaCache)
            {
                var estadoCivil = EstadosCivis.FirstOrDefault(s => s.Identificador == idEstadoCivil);

                if (estadoCivil != null)
                    return new EstadoCivil { IdEstadoCivil = estadoCivil.Identificador, DescricaoEstadoCivil = estadoCivil.Descricao };

                throw (new RecordNotFoundException(typeof(EstadoCivil)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idEstadoCivil))
            {
                EstadoCivil objEstadoCivil = new EstadoCivil();
                if (SetInstance(dr, objEstadoCivil))
                    return objEstadoCivil;
            }
            throw (new RecordNotFoundException(typeof(EstadoCivil)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de EstadoCivil a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idEstadoCivil">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de EstadoCivil.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static EstadoCivil LoadObject(int idEstadoCivil, SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var estadoCivil = EstadosCivis.FirstOrDefault(s => s.Identificador == idEstadoCivil);

                if (estadoCivil != null)
                    return new EstadoCivil { IdEstadoCivil = estadoCivil.Identificador, DescricaoEstadoCivil = estadoCivil.Descricao };

                throw (new RecordNotFoundException(typeof(EstadoCivil)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idEstadoCivil, trans))
            {
                EstadoCivil objEstadoCivil = new EstadoCivil();
                if (SetInstance(dr, objEstadoCivil))
                    return objEstadoCivil;
            }
            throw (new RecordNotFoundException(typeof(EstadoCivil)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de EstadoCivil a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var estadoCivil = EstadosCivis.FirstOrDefault(s => s.Identificador == this._idEstadoCivil);

                if (estadoCivil != null)
                {
                    this.DescricaoEstadoCivil = estadoCivil.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idEstadoCivil))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de EstadoCivil a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var estadoCivil = EstadosCivis.FirstOrDefault(s => s.Identificador == this._idEstadoCivil);

                if (estadoCivil != null)
                {
                    this.DescricaoEstadoCivil = estadoCivil.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idEstadoCivil, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #endregion

    }
}