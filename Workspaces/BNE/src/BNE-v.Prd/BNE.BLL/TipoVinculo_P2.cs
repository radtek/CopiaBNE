//-- Data: 16/11/2010 14:07
//-- Autor: Vinicius Maciel

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
    public partial class TipoVinculo // Tabela: BNE_Tipo_Vinculo
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "BNE_Tipo_Vinculo";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region TiposVinculos
        private static List<TipoVinculoCACHE> TiposVinculos
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarVinculosCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarVinculosCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Lista com valores recuperados do banco.</returns>
        private static List<TipoVinculoCACHE> ListarVinculosCACHE()
        {
            var lista = new List<TipoVinculoCACHE>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpListartipovinculo, null))
            {
                while (dr.Read())
                {
                    lista.Add(new TipoVinculoCACHE
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Tipo_Vinculo"]),
                        Descricao = dr["Des_Tipo_Vinculo"].ToString()
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

        #region TipoVinculoCACHE
        private class TipoVinculoCACHE
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas

        #region SpListartipovinculo
        private const string SpListartipovinculo = @"
        SELECT  Idf_Tipo_Vinculo, 
                Des_Tipo_Vinculo 
        FROM    BNE_Tipo_Vinculo WITH (NOLOCK)
        ORDER BY Cod_Grau_Tipo_Vinculo ASC";
        #endregion

        #region Spselectpordescricao
        private const string Spselectpordescricao = @"
        SELECT  Idf_Tipo_Vinculo, 
                Des_Tipo_Vinculo,
                Dta_Cadastro,
                Cod_Grau_Tipo_Vinculo
        FROM    BNE_Tipo_Vinculo WITH (NOLOCK)
        WHERE Des_Tipo_Vinculo LIKE @Descricao
        ORDER BY Des_Tipo_Vinculo ASC";
        #endregion
        
        #endregion

        #region Métodos

        #region ListarTipoVinculo
        /// <summary>
        /// Método retorna Consulta para carregar DropDownList
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ListarTipoVinculo()
        {
            #region Cache
            if (HabilitaCache)
                return TiposVinculos.ToDictionary(tv => tv.Identificador.ToString(), tv => tv.Descricao);
            #endregion

            var dicionarioTipoVinculo = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpListartipovinculo, null))
            {
                while (dr.Read())
                {
                    dicionarioTipoVinculo.Add(dr["Idf_Tipo_Vinculo"].ToString(), dr["Des_Tipo_Vinculo"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionarioTipoVinculo;
        }
        #endregion

        #region CarregarPorDescricao
        /// <summary>
        /// Carrega uma instancia de TipoVinculo a partir de sua descricao
        /// </summary>
        /// <param name="descricao">Descricao de Disponibilidade</param>
        /// <returns>Instancia de TipoVinculo</returns>
        public static TipoVinculo CarregarPorDescricao(string descricao)
        {
            #region Cache
            if (HabilitaCache)
            {
                var tipoVinculo = TiposVinculos.OrderBy(tv => tv.Descricao).FirstOrDefault(tv => tv.Descricao.NormalizarStringLINQ().Equals(descricao.NormalizarStringLINQ()));

                if (tipoVinculo != null)
                    return new TipoVinculo { IdTipoVinculo = tipoVinculo.Identificador, DescricaoTipoVinculo = tipoVinculo.Descricao };

                throw (new RecordNotFoundException(typeof(TipoVinculo)));
            }
            #endregion

            var parms = new List<SqlParameter> { new SqlParameter { ParameterName = "@Descricao", SqlDbType = SqlDbType.VarChar, Size = 50, Value = descricao } };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectpordescricao, parms))
            {
                var objTipoVinculo = new TipoVinculo();
                if (SetInstance(dr, objTipoVinculo))
                    return objTipoVinculo;

                if (!dr.IsClosed)
                    dr.Close();
            }
            throw new RecordNotFoundException(typeof(TipoVinculo));
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