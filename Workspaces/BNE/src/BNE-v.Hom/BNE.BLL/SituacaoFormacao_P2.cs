//-- Data: 02/03/2010 09:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using BNE.Cache;

namespace BNE.BLL
{
	public partial class SituacaoFormacao // Tabela: BNE_Situacao_Formacao
	{

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "BNE_Situacao_Formacao";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region SituacoesFormacao
        private static List<SituacaoFormacaoCache> SituacoesFormacao
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarSituacaoFormacaoCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarSituacaoFormacaoCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<SituacaoFormacaoCache> ListarSituacaoFormacaoCACHE()
        {
            var lista = new List<SituacaoFormacaoCache>();

            const string spselecttodassituacoes = @"
            SELECT  Idf_Situacao_Formacao,
                    Des_Situacao_Formacao
            FROM    BNE_Situacao_Formacao WITH(NOLOCK)
            WHERE   Flg_Inativo = 0
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodassituacoes, null))
            {
                while (dr.Read())
                {
                    lista.Add(new SituacaoFormacaoCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Situacao_Formacao"]),
                        Descricao = dr["Des_Situacao_Formacao"].ToString()
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

        #region SituacaoFormacaoCache
        private class SituacaoFormacaoCache
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas

        private const string SpselectSituacaoFormacao = @"  
        SELECT  Idf_Situacao_Formacao,
                Des_Situacao_Formacao
        FROM BNE_Situacao_Formacao";
       
        #endregion

        #region Métodos

        #region Listar
        /// <summary>
        /// Retorna os registros da BNE_Situacao_Formacao
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> Listar()
        {
            #region Cache
            if (HabilitaCache)
                return SituacoesFormacao.ToDictionary(e => e.Identificador.ToString(), e => e.Descricao);
            #endregion

            var dicionario = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpselectSituacaoFormacao, null))
            {
                while (dr.Read())
                {
                    dicionario.Add(dr["Idf_Situacao_Formacao"].ToString(), dr["Des_Situacao_Formacao"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionario;
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de SituacaoFormacao a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var situacaoFormacao = SituacoesFormacao.FirstOrDefault(e => e.Identificador == this.IdSituacaoFormacao);

                if (situacaoFormacao != null)
                {
                    this.DescricaoSituacaoFormacao = situacaoFormacao.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idSituacaoFormacao))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de SituacaoFormacao a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var situacaoFormacao = SituacoesFormacao.FirstOrDefault(e => e.Identificador == this.IdSituacaoFormacao);

                if (situacaoFormacao != null)
                {
                    this.DescricaoSituacaoFormacao = situacaoFormacao.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idSituacaoFormacao, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

        #endregion

    }
}