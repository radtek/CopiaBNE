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

namespace BNE.Mensagem.AsyncServices.BLL
{
    public partial class Parametro // Tabela: TAB_Parametro
    {

        #region Configura��o de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "AsyncServices.TAB_Parametro";
        private const double SlidingExpiration = 60;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region Parametros
        private static Dictionary<Enumeradores.Parametro, string> Parametros
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarParametrosCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region M�todos

        #region ListarParametrosCACHE
        /// <summary>
        /// M�todo que retorna uma lista de itens.
        /// </summary>
        /// <returns>Dicion�rio com valores recuperados do banco.</returns>
        private static Dictionary<Enumeradores.Parametro, string> ListarParametrosCACHE()
        {
            var listaParametros = new List<Enumeradores.Parametro>();

            foreach (Enumeradores.Parametro parametro in Enum.GetValues(typeof(Enumeradores.Parametro)))
            {
                listaParametros.Add(parametro);
            }

            var parms = new List<SqlParameter>();
            var itensParametros = new Dictionary<Enumeradores.Parametro, string>();
            string query = "select Idf_Parametro, Vlr_Parametro from atividade.TAB_Parametro WITH (NOLOCK) where Idf_Parametro in ( ";
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
                    var parametro = (Enumeradores.Parametro)Enum.Parse(typeof(Enumeradores.Parametro), dr["Idf_Parametro"].ToString());
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
        FROM    TAB_Parametro PAR WITH(NOLOCK)
        WHERE   PAR.Idf_Parametro = @idf_Parametro";
        #endregion

        #region M�todos

        #region RecuperaValorParametro
        /// <summary>
        /// M�todo que recupera o valor de um par�metro a partir do id.
        /// </summary>
        /// <param name="parametro">Identificador do par�metro.</param>
        /// <param name="trans">Transa��o </param>
        /// <returns>Valor do par�metro.</returns>
        public static string RecuperaValorParametro(Enumeradores.Parametro parametro, SqlTransaction trans = null)
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
        /// M�todo que retorna uma lista de itens do dicion�rio de par�metros do sistema.
        /// </summary>
        /// <param name="idsParametros">Lista de registros que devem ser recuperados do banco.</param>
        /// <returns>Dicion�rio com valores recuperados do banco.</returns>
        public static Dictionary<Enumeradores.Parametro, string> ListarParametros(List<Enumeradores.Parametro> idsParametros)
        {
            try
            {
                #region Cache
                if (HabilitaCache)
                    return idsParametros.ToDictionary(p => p, parametro => Parametros[parametro]);
                #endregion

                var parms = new List<SqlParameter>();
                var itensParametros = new Dictionary<Enumeradores.Parametro, string>();
                string query = "select Idf_Parametro, Vlr_Parametro from atividade.TAB_Parametro WITH (NOLOCK) where Idf_Parametro in ( ";
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
                        var parametro = (Enumeradores.Parametro)Enum.Parse(typeof(Enumeradores.Parametro), dr["Idf_Parametro"].ToString());
                        itensParametros.Add(parametro, Convert.ToString(dr["Vlr_Parametro"]));
                    }
                    if (!dr.IsClosed)
                        dr.Close();
                }

                return itensParametros;
            }
            catch (Exception ex)
            {
                new Services.AsyncServices.Base.Autofac().Logger.Error(ex, "Erro ao buscar parametros.");
                throw;
            }
        }
        #endregion

        #endregion

    }
}