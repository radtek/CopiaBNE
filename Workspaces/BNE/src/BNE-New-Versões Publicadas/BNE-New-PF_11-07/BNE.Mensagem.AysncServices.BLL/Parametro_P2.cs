//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using BNE.Cache;
using BNE.EL;
using BNE.EL.Interface;

namespace BNE.Services.AsyncServices.BLL
{
    public partial class Parametro // Tabela: TAB_Parametro
    {

        private static readonly ILogger _logger = new DatabaseLogger();

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "AsyncServices.plataforma.TAB_Parametro";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Parametros
        private static Dictionary<Mensagem.AysncServices.BLL.Enumeradores.Parametro, string> Parametros
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarParametrosCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarParametrosCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static Dictionary<Mensagem.AysncServices.BLL.Enumeradores.Parametro, string> ListarParametrosCACHE()
        {
            var listaParametros = new List<Mensagem.AysncServices.BLL.Enumeradores.Parametro>();

            foreach (Mensagem.AysncServices.BLL.Enumeradores.Parametro parametro in Enum.GetValues(typeof(Mensagem.AysncServices.BLL.Enumeradores.Parametro)))
            {
                listaParametros.Add(parametro);
            }

            var parms = new List<SqlParameter>();
            var itensParametros = new Dictionary<Mensagem.AysncServices.BLL.Enumeradores.Parametro, string>();
            string query = "select Idf_Parametro, Vlr_Parametro from plataforma.TAB_Parametro WITH (NOLOCK) where Idf_Parametro in ( ";
            for (int i = 0; i < listaParametros.Count; i++)
            {
                string nomeParametro = "@parm" + i.ToString(CultureInfo.CurrentCulture);

                if (i > 0)
                    query += ", ";

                query += nomeParametro;
                parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                parms[i].Value = Convert.ToInt32(listaParametros[i]);
            }

            query += ")";

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, parms))
            {
                while (dr.Read())
                {
                    var parametro = (Mensagem.AysncServices.BLL.Enumeradores.Parametro)Enum.Parse(typeof(Mensagem.AysncServices.BLL.Enumeradores.Parametro), dr["Idf_Parametro"].ToString());
                    itensParametros.Add(parametro, Convert.ToString(dr["Vlr_Parametro"]));
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return itensParametros;
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string Spselvalor = @"
        SELECT  Vlr_Parametro
        FROM    plataforma.TAB_Parametro PAR WITH(NOLOCK)
        WHERE   PAR.Idf_Parametro = @idf_Parametro";
        #endregion

        #region Métodos

        #region RecuperaValorParametro
        /// <summary>
        /// Método que recupera o valor de um parâmetro a partir do id.
        /// </summary>
        /// <param name="parametro">Identificador do parâmetro.</param>
        /// <param name="trans">Transação </param>
        /// <returns>Valor do parâmetro.</returns>
        public static string RecuperaValorParametro(Mensagem.AysncServices.BLL.Enumeradores.Parametro parametro, SqlTransaction trans = null)
        {
            #region Cache
            if (HabilitaCache)
                return Parametros[parametro];
            #endregion

            var parms = new List<SqlParameter>
                {
                    new SqlParameter{ ParameterName = "@Idf_Parametro", SqlDbType = SqlDbType.Int, Size = 4, Value = Convert.ToInt32(parametro)}
                };

            Object retorno;

            if (trans != null)
                retorno = DataAccessLayer.ExecuteScalar(trans, CommandType.Text, Spselvalor, parms);
            else
                retorno = DataAccessLayer.ExecuteScalar(CommandType.Text, Spselvalor, parms);

            if (retorno != null)
                return retorno.ToString();

            return string.Empty;
        }
        #endregion

        #region ListarParametros
        /// <summary>
        /// Método que retorna uma lista de itens do dicionário de parâmetros do sistema.
        /// </summary>
        /// <param name="idsParametros">Lista de registros que devem ser recuperados do banco.</param>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        public static Dictionary<Mensagem.AysncServices.BLL.Enumeradores.Parametro, string> ListarParametros(List<Mensagem.AysncServices.BLL.Enumeradores.Parametro> idsParametros)
        {
            try
            {
                #region Cache
                if (HabilitaCache)
                    return idsParametros.ToDictionary(p => p, parametro => Parametros[parametro]);
                #endregion

                var parms = new List<SqlParameter>();
                var itensParametros = new Dictionary<Mensagem.AysncServices.BLL.Enumeradores.Parametro, string>();
                string query = "select Idf_Parametro, Vlr_Parametro from plataforma.TAB_Parametro WITH (NOLOCK) where Idf_Parametro in ( ";
                for (int i = 0; i < idsParametros.Count; i++)
                {
                    string nomeParametro = "@parm" + i.ToString(CultureInfo.CurrentCulture);

                    if (i > 0)
                        query += ", ";

                    query += nomeParametro;
                    parms.Add(new SqlParameter(nomeParametro, SqlDbType.Int, 4));
                    parms[i].Value = Convert.ToInt32(idsParametros[i]);
                }

                query += ")";

                using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, query, parms))
                {
                    while (dr.Read())
                    {
                        var parametro = (Mensagem.AysncServices.BLL.Enumeradores.Parametro)Enum.Parse(typeof(Mensagem.AysncServices.BLL.Enumeradores.Parametro), dr["Idf_Parametro"].ToString());
                        itensParametros.Add(parametro, Convert.ToString(dr["Vlr_Parametro"]));
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }

                return itensParametros;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erro ao buscar parametros.");
                throw;
            }
        }
        #endregion

        #region SalvarParametros
        /// <summary>
        /// Método que salva os itens do dicionário
        /// </summary>
        public static void SalvarParametros(Dictionary<Mensagem.AysncServices.BLL.Enumeradores.Parametro, string> dicionarioParametros)
        {
            var parms = new List<SqlParameter>();

            string query = String.Empty;
            int i = 1;
            foreach (KeyValuePair<Mensagem.AysncServices.BLL.Enumeradores.Parametro, string> kvp in dicionarioParametros)
            {
                string nomeParametro = "@parm" + i;
                string nomeParametroValor = "@parmValor" + i;

                var parmId = new SqlParameter(nomeParametro, SqlDbType.Int, 1);
                var parmValor = new SqlParameter(nomeParametroValor, SqlDbType.VarChar, 100);
                parms.Add(parmId);
                parms.Add(parmValor);

                parmId.Value = (int)kvp.Key;
                parmValor.Value = kvp.Value;

                string nomeCampo = String.Format("UPDATE plataforma.TAB_Parametro SET Vlr_Parametro = {0} WHERE Idf_Parametro = {1};", nomeParametroValor, nomeParametro);

                query += nomeCampo;
                i++;
            }

            DataAccessLayer.ExecuteNonQuery(CommandType.Text, query, parms);
        }
        #endregion

        #endregion

    }
}