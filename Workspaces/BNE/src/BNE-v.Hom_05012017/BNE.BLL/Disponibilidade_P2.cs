//-- Data: 01/10/2010 11:40
//-- Autor: Bruno Flammarion

using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using BNE.BLL.Custom;
using BNE.Cache;
using BNE.EL;

namespace BNE.BLL
{
	public partial class Disponibilidade // Tabela: Tab_Disponibilidade
	{

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "bne.Tab_Disponibilidade";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Disponibilidades
        private static List<DisponibilidadeCache> Disponibilidades
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarDisponibilidadesCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarDisponibilidadesCACHE
        /// <summary>
        /// Método que retorna uma lista de itens do sistema.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<DisponibilidadeCache> ListarDisponibilidadesCACHE()
        {
            var lista = new List<DisponibilidadeCache>();

            const string selectdisponibilidades = @"
            SELECT  D.Idf_Disponibilidade,
				    D.Des_Disponibilidade
		    FROM    Tab_Disponibilidade D WITH(NOLOCK)
		    WHERE   D.Flg_Inativo = 0
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, selectdisponibilidades, null))
            {
                while (dr.Read())
                {
                    lista.Add(new DisponibilidadeCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Disponibilidade"]),
                        Descricao = dr["Des_Disponibilidade"].ToString()
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

        #region DisponibilidadeCache
        private class DisponibilidadeCache
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string Selectdisponibilidade = @"
        SELECT  Idf_Disponibilidade,
				Des_Disponibilidade
		FROM    Tab_Disponibilidade WITH(NOLOCK)
		WHERE   Flg_Inativo = 0
		ORDER   BY Idf_Disponibilidade ASC";

        private const string SpSelectPorDescricao = @"
        SELECT   Idf_Disponibilidade
                ,Des_Disponibilidade
                ,Dta_Cadastro
                ,Flg_Inativo
		FROM    Tab_Disponibilidade WITH(NOLOCK)
		WHERE   Des_Disponibilidade LIKE @Descricao
		ORDER   BY Idf_Disponibilidade ASC";
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
                return Disponibilidades.OrderBy(d => d.Identificador).ToDictionary(d => d.Identificador.ToString(), d => d.Descricao);
            #endregion

            var dicionarioDisponibilidade = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Selectdisponibilidade, null))
            {
                while (dr.Read())
                    dicionarioDisponibilidade.Add(dr["Idf_Disponibilidade"].ToString(), dr["Des_Disponibilidade"].ToString());
            }

            return dicionarioDisponibilidade;
        }
        #endregion

        #region CarregarPorDescricao
        /// <summary>
        /// Carrega uma instancia de Disponibilidade a partir de sua descricao
        /// </summary>
        /// <param name="descricao">Sigla de Disponibilidade</param>
        /// <returns>Instancia de Disponibilidade</returns>
        public static Disponibilidade CarregarPorDescricao(string descricao)
        {
            #region Cache
            if (HabilitaCache)
            {
                var disponibilidade = Disponibilidades.FirstOrDefault(d => d.Descricao.NormalizarStringLINQ().Equals(descricao.NormalizarStringLINQ()));

                if (disponibilidade != null)
                    return new Disponibilidade { IdDisponibilidade = disponibilidade.Identificador, DescricaoDisponibilidade = disponibilidade.Descricao };

                throw new RecordNotFoundException(typeof(Disponibilidade));
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Descricao", SqlDbType = SqlDbType.VarChar, Size = 50, Value = descricao }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectPorDescricao, parms))
            {
                var objDisponibilidade = new Disponibilidade();
                if (SetInstance(dr, objDisponibilidade))
                    return objDisponibilidade;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw new RecordNotFoundException(typeof(Disponibilidade));
        }
        #endregion

        #region LoadObject
        /// <summary>
        /// Método utilizado para retornar uma instância de Disponibilidade a partir do banco de dados.
        /// </summary>
        /// <param name="idDisponibilidade">Chave do registro.</param>
        /// <returns>Instância de Disponibilidade.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        public static Disponibilidade LoadObject(int idDisponibilidade)
        {
            #region Cache
            if (HabilitaCache)
            {
                var disponibilidade = Disponibilidades.FirstOrDefault(d => d.Identificador == idDisponibilidade);

                if (disponibilidade != null)
                    return new Disponibilidade { IdDisponibilidade = disponibilidade.Identificador, DescricaoDisponibilidade = disponibilidade.Descricao };

                throw new RecordNotFoundException(typeof(Disponibilidade));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idDisponibilidade))
            {
                Disponibilidade objDisponibilidade = new Disponibilidade();
                if (SetInstance(dr, objDisponibilidade))
                    return objDisponibilidade;
            }
            throw (new RecordNotFoundException(typeof(Disponibilidade)));
        }
        /// <summary>
        /// Método utilizado para retornar uma instância de Disponibilidade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="idDisponibilidade">Chave do registro.</param>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Instância de Disponibilidade.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        public static Disponibilidade LoadObject(int idDisponibilidade, SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var disponibilidade = Disponibilidades.FirstOrDefault(d => d.Identificador == idDisponibilidade);

                if (disponibilidade != null)
                    return new Disponibilidade { IdDisponibilidade = disponibilidade.Identificador, DescricaoDisponibilidade = disponibilidade.Descricao };

                throw new RecordNotFoundException(typeof(Disponibilidade));
            }
            #endregion

            using (IDataReader dr = LoadDataReader(idDisponibilidade, trans))
            {
                Disponibilidade objDisponibilidade = new Disponibilidade();
                if (SetInstance(dr, objDisponibilidade))
                    return objDisponibilidade;
            }
            throw (new RecordNotFoundException(typeof(Disponibilidade)));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Disponibilidade a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var disponibilidade = Disponibilidades.FirstOrDefault(d => d.Identificador == this._idDisponibilidade);

                if (disponibilidade != null)
                {
                    this.DescricaoDisponibilidade = disponibilidade.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idDisponibilidade))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Disponibilidade a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Bruno Flammarion</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var disponibilidade = Disponibilidades.FirstOrDefault(d => d.Identificador == this._idDisponibilidade);

                if (disponibilidade != null)
                {
                    this.DescricaoDisponibilidade = disponibilidade.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idDisponibilidade, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #endregion

    }
}