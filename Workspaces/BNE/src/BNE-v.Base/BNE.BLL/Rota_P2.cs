//-- Data: 21/05/2013 11:22
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
                    Num_Peso
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
                        Peso = Convert.ToInt32(dr["Num_Peso"])
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
        }
        #endregion

        #endregion

        #endregion

        #region Consultas

        #region Sprecuperarrotas
        private const string Sprecuperarrotas = "SELECT * FROM BNE_Rota WHERE Flg_Inativo = 0 ORDER BY Num_Peso DESC";
        #endregion

        #region Sprecuperarurlrota
        private const string Sprecuperarurlrota = @"
        SELECT  Des_URL
        FROM    BNE_Rota R WITH(NOLOCK)
        WHERE   R.Idf_Rota = @Idf_Rota";
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
                return Rotas.OrderByDescending(r => r.Peso).Select(r => new Rota { IdRota = r.Identificador, NomeRota = r.Nome, DescricaoURL = r.URL, DescricaoCaminhoFisico = r.CaminhoFisico, NumeroPeso = r.Peso }).ToList();
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

        #endregion

    }
}