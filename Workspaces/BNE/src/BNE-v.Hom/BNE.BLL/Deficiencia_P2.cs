//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

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
	public partial class Deficiencia // Tabela: TAB_Deficiencia
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "plataforma.TAB_Deficiencia";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Deficiencias
        private static List<DeficienciaCache> Deficiencias
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarDeficienciasCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarDeficienciasCACHE
        /// <summary>
        /// Método que retorna uma lista de itens do dicionário de parâmetros do sistema.
        /// </summary>
        /// <returns>Lista com valores recuperados do banco.</returns>
        private static List<DeficienciaCache> ListarDeficienciasCACHE()
        {
            var lista = new List<DeficienciaCache>();

            const string spselecttodasdeficiencias = @"
            Select  Idf_Deficiencia,
                    Des_Deficiencia
            From    plataforma.Tab_Deficiencia D WITH (NOLOCK)
            WHERE   D.Flg_Inativo = 0
            ";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spselecttodasdeficiencias, null))
            {
                while (dr.Read())
                {
                    lista.Add(new DeficienciaCache
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Deficiencia"]),
                        Descricao = dr["Des_Deficiencia"].ToString()
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

        #region DeficienciaCache
        private class DeficienciaCache
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consulta

        private const string SELECTDEFICIENCIA = @" Select 
                                                        Idf_Deficiencia,
                                                        Des_Deficiencia
                                                    From
                                                    plataforma.Tab_Deficiencia WITH (NOLOCK)
                                                    order by Des_Deficiencia";

        private const string SpSelectPorDescricao = @"
                                                    Select Idf_Deficiencia
                                                          ,Des_Deficiencia
                                                          ,Cod_Caged
                                                          ,Flg_Inativo
                                                          ,Dta_Cadastro
                                                    From
                                                    plataforma.Tab_Deficiencia WITH (NOLOCK)
                                                    WHERE Des_Deficiencia LIKE @Descricao
                                                    order by Des_Deficiencia";

        #endregion

        #region Métodos

        #region Listar
        /// <summary>
        /// Método retorna Consulta para carregar DropDownList
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> Listar()
        {
            #region Cache
            if (HabilitaCache)
                return Deficiencias.OrderBy(e => e.Descricao).ToDictionary(e => e.Identificador.ToString(), e => e.Descricao);
            #endregion

            var dicionario = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECTDEFICIENCIA, null))
            {
                while (dr.Read())
                {
                    dicionario.Add(dr["Idf_Deficiencia"].ToString(), dr["Des_Deficiencia"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionario;
        }
        #endregion

        #region CarregarPorDescricao
        /// <summary>
        /// Carrega uma instancia de Deficiencia a partir de sua descricao
        /// </summary>
        /// <param name="descricao">Descricao da Deficiencia</param>
        /// <returns>Instancia de Deficiencia</returns>
        public static Deficiencia CarregarPorDescricao(string descricao)
        {
            #region Cache
            if (HabilitaCache)
            {
                var deficiencia = Deficiencias.FirstOrDefault(f => f.Descricao.NormalizarStringLINQ().Equals(descricao.NormalizarStringLINQ()));
                if (deficiencia != null)
                    return new Deficiencia { IdDeficiencia = deficiencia.Identificador, DescricaoDeficiencia = deficiencia.Descricao };

                return null;
            }
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Descricao", SqlDbType = SqlDbType.VarChar, Size = 50, Value = descricao }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpSelectPorDescricao, parms))
            {
                Deficiencia objDeficiencia = new Deficiencia();
                if (SetInstance(dr, objDeficiencia))
                    return objDeficiencia;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw new RecordNotFoundException(typeof(Deficiencia));
        }
        #endregion

        #region CompleteObject
        /// <summary>
        /// Método utilizado para completar uma instância de Deficiencia a partir do banco de dados.
        /// </summary>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject()
        {
            #region Cache
            if (HabilitaCache)
            {
                var deficiencia = Deficiencias.FirstOrDefault(d => d.Identificador == this._idDeficiencia);
                if (deficiencia != null)
                {
                    this.DescricaoDeficiencia = deficiencia.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idDeficiencia))
            {
                return SetInstance(dr, this);
            }
        }
        /// <summary>
        /// Método utilizado para completar uma instância de Deficiencia a partir do banco de dados, dentro de uma transação.
        /// </summary>
        /// <param name="trans">Transação existente no banco de dados.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public bool CompleteObject(SqlTransaction trans)
        {
            #region Cache
            if (HabilitaCache)
            {
                var deficiencia = Deficiencias.FirstOrDefault(d => d.Identificador == this._idDeficiencia);
                if (deficiencia != null)
                {
                    this.DescricaoDeficiencia = deficiencia.Descricao;
                    return true;
                }

                return false;
            }
            #endregion

            using (IDataReader dr = LoadDataReader(this._idDeficiencia, trans))
            {
                return SetInstance(dr, this);
            }
        }
        #endregion

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