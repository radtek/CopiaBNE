//-- Data: 13/07/2015 17:00
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using BNE.Cache;

namespace BNE.BLL
{
	public partial class TipoRetornoCampanhaRecrutamento // Tabela: BNE_Tipo_Retorno_Campanha_Recrutamento
	{

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region TiposRetorno
        private static List<TipoRetornoCampanhaRecrutamento> TiposRetorno
        {
            get
            {
                return Cache.GetItem(typeof(TipoRetornoCampanhaRecrutamento).ToString(), ListarTipoRetornoCampanhaRecrutamentoCACHE);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarTipoRetornoCampanhaRecrutamentoCACHE
        /// <summary>
        /// Método que retorna uma lista de itens do sistema.
        /// </summary>
        /// <returns>Dicionário com valores recuperados do banco.</returns>
        private static List<TipoRetornoCampanhaRecrutamento> ListarTipoRetornoCampanhaRecrutamentoCACHE()
        {
            var lista = new List<TipoRetornoCampanhaRecrutamento>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectall, null))
            {
                var objTipoRetornoCampanhaRecrutamento = new TipoRetornoCampanhaRecrutamento();

                while (SetInstance(dr, objTipoRetornoCampanhaRecrutamento, false))
                {
                    lista.Add(objTipoRetornoCampanhaRecrutamento);
                    objTipoRetornoCampanhaRecrutamento = new TipoRetornoCampanhaRecrutamento();
                }

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #endregion

        #endregion

        #region Consultas

        #region Spselectall
        private const string Spselectall = @"
        SELECT  Idf_Tipo_Retorno_Campanha_Recrutamento,
                Des_Tipo_Retorno_Campanha_Recrutamento
        FROM    BNE_Tipo_Retorno_Campanha_Recrutamento WITH(NOLOCK)";
        #endregion

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
                return TiposRetorno.OrderBy(r => r.IdTipoRetornoCampanhaRecrutamento).ToDictionary(s => s.IdTipoRetornoCampanhaRecrutamento.ToString(), s => s.DescricaoTipoRetornoCampanhaRecrutamento);
            #endregion

            var dicionario = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectall, null))
            {
                while (dr.Read())
                {
                    dicionario.Add(dr["Idf_Tipo_Retorno_Campanha_Recrutamento"].ToString(), dr["Des_Tipo_Retorno_Campanha_Recrutamento"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionario;
        }
        #endregion

        #endregion

	}
}