//-- Data: 18/07/2016 15:05
//-- Autor: Mailson

using BNE.Cache;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class MotivoCancelamento // Tabela: BNE_Motivo_Cancelamento
	{
        #region Configuração de cache
        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        private static List<CacheObject> CachedObject
        {
            get
            {
                return Cache.GetItem(typeof(SituacaoFilial).ToString(), Load);
            }
        }
        private class CacheObject
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }
        private static List<CacheObject> Load()
        {
            var lista = new List<CacheObject>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, spListarVIP, null))
            {
                while (dr.Read())
                {
                    lista.Add(new CacheObject
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Motivo_Cancelamento"]),
                        Descricao = dr["Des_Motivo_Cancelamento"].ToString()
                    });
                }
            }

            return lista;
        }
        #endregion


        #region Consultas

        private const string spListarVIP = @"  
          SELECT  idf_motivo_cancelamento, des_motivo_cancelamento
        FROM    bne_motivo_cancelamento WITH ( NOLOCK )
        WHERE   Flg_Inativo = 0 and idf_tipo_motivo_cancelamento = 1 --VIP";

        #region [spListaCIA]
        private const string spListaCIA = @"  
          SELECT  idf_motivo_cancelamento, des_motivo_cancelamento
        FROM    bne_motivo_cancelamento WITH ( NOLOCK )
        WHERE   Flg_Inativo = 0 and idf_tipo_motivo_cancelamento = 2 --CIA";

        #endregion
        #endregion

        #region [ListarVIP]
        public static Dictionary<string, string> ListarVIP()
        {
           // if (CachedObject != null)
             //   return CachedObject.OrderBy(o => o.Identificador).ToDictionary(o => o.Identificador.ToString(), o => o.Descricao);

            var dicionario = new Dictionary<string, string>();

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spListarVIP, null))
            {
                while (dr.Read())
                    dicionario.Add(dr["Idf_Motivo_Cancelamento"].ToString(), dr["Des_Motivo_Cancelamento"].ToString());
            }
            return dicionario;
        }
        #endregion

        #region [ListaCIA]
        public static Dictionary<string, string> ListarCIA()
        {
            var dicionario = new Dictionary<string, string>();
            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, spListaCIA, null))
            {
                while (dr.Read())
                    dicionario.Add(dr["Idf_Motivo_Cancelamento"].ToString(), dr["Des_Motivo_Cancelamento"].ToString());
            }
            return dicionario;
        }
        #endregion
    }
}