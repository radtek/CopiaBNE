//-- Data: 08/04/2010 11:17
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using BNE.Cache;

namespace BNE.BLL
{
    public partial class SituacaoFilial : ICloneable // Tabela: TAB_Situacao_Filial
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

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECTSITUACAOFILIAL, null))
            {
                while (dr.Read())
                {
                    lista.Add(new CacheObject
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Situacao_Filial"]),
                        Descricao = dr["Des_Situacao_Filial"].ToString()
                    });
                }
            }

            return lista;
        }
        #endregion

        #region Clone
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        #endregion

        #region Consultas

        private const string SELECTSITUACAOFILIAL = @"  
        SELECT  Idf_Situacao_Filial, 
                Des_Situacao_Filial 
        FROM    TAB_Situacao_Filial WITH ( NOLOCK )
        WHERE   Flg_Inativo = 0";

        #endregion

        #region Metodos

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objSituacaoFilial">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        public static bool SetInstance(IDataReader dr, SituacaoFilial objSituacaoFilial)
        {
            try
            {
                if (dr.Read())
                {
                    objSituacaoFilial._idSituacaoFilial = Convert.ToInt32(dr["Idf_Situacao_Filial"]);
                    objSituacaoFilial._descricaoSituacaoFilial = Convert.ToString(dr["Des_Situacao_Filial"]);
                    objSituacaoFilial._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);
                    objSituacaoFilial._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);

                    objSituacaoFilial._persisted = true;
                    objSituacaoFilial._modified = false;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                dr.Dispose();
            }
        }
        #endregion

        #region Listar
        public static Dictionary<string, string> Listar()
        {
            if (CachedObject != null)
                return CachedObject.OrderBy(o => o.Identificador).ToDictionary(o => o.Identificador.ToString(), o => o.Descricao);

            var dicionario = new Dictionary<string, string>();

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SELECTSITUACAOFILIAL, null))
            {
                while (dr.Read())
                    dicionario.Add(dr["Idf_Situacao_Filial"].ToString(), dr["Des_Situacao_Filial"].ToString());
            }
            return dicionario;
        }
        #endregion

        #endregion

    }
}