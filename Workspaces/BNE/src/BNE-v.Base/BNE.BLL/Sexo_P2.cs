//-- Data: 02/03/2010 09:17
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
    public partial class Sexo : ICloneable // Tabela: TAB_Sexo
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "plataforma.TAB_Sexo";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Sexos
        private static List<SexoCache> Sexos
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarSexosCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarSexosCACHE
        /// <summary>
        /// Método que retorna uma lista de itens do sistema.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<SexoCache> ListarSexosCACHE()
        {
            var lista = new List<SexoCache>();

            const string spselecttodossexos = @"
            SELECT  Idf_Sexo,
                    Des_Sexo
            FROM    plataforma.TAB_Sexo WITH(NOLOCK)
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodossexos, null))
            {
                while (dr.Read())
                {
                    lista.Add(new SexoCache { Identificador = Convert.ToInt32(dr["Idf_Sexo"]), Descricao = dr["Des_Sexo"].ToString() });
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #endregion

        #region CacheObjects

        #region SexoCache
        private class SexoCache
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas
        private const string Spsexo = "SELECT Idf_Sexo, Des_Sexo FROM plataforma.TAB_Sexo ORDER BY Sig_Sexo";
        #endregion

        #region Métodos

        #region Listar
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Cursor de leitura do banco de dados.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Dictionary<string, string> Listar()
        {
            #region Cache
            if (HabilitaCache)
                return Sexos.OrderBy(s => s.Descricao).ToDictionary(s => s.Identificador.ToString(), s => s.Descricao);
            #endregion

            var dicionarioSexo = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spsexo, null))
            {
                while (dr.Read())
                {
                    dicionarioSexo.Add(dr["Idf_Sexo"].ToString(), dr["Des_Sexo"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionarioSexo;
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Sexo a partir do banco de dados.
        /// </summary>
        /// <param name="idSexo">Chave do registro.</param>
        /// <returns>Instância de Sexo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Sexo LoadObject(int idSexo)
        {
            #region Cache
            if (HabilitaCache)
            {
                var sexo = Sexos.FirstOrDefault(s => s.Identificador == idSexo);

                if (sexo != null)
                    return new Sexo { IdSexo = sexo.Identificador, DescricaoSexo = sexo.Descricao };

                throw (new RecordNotFoundException(typeof(Sexo)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idSexo))
            {
                Sexo objSexo = new Sexo();
                if (SetInstance(dr, objSexo))
                    return objSexo;
            }
            throw (new RecordNotFoundException(typeof(Sexo)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Sexo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idSexo">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Sexo.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static Sexo LoadObject(int idSexo, SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var sexo = Sexos.FirstOrDefault(s => s.Identificador == idSexo);

                if (sexo != null)
                    return new Sexo { IdSexo = sexo.Identificador, DescricaoSexo = sexo.Descricao };

                throw (new RecordNotFoundException(typeof(Sexo)));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idSexo, trans))
            {
                var objSexo = new Sexo();
                if (SetInstance(dr, objSexo))
                    return objSexo;
            }
            throw (new RecordNotFoundException(typeof(Sexo)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Sexo a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var sexo = Sexos.FirstOrDefault(s => s.Identificador == this._idSexo);

                if (sexo != null)
                {
                    this.DescricaoSexo = sexo.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idSexo))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Sexo a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var sexo = Sexos.FirstOrDefault(s => s.Identificador == this._idSexo);

                if (sexo != null)
                {
                    this.DescricaoSexo = sexo.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idSexo, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #endregion

    }
}