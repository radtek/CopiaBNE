using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using BNE.Cache;
using BNE.EL;

namespace BNE.BLL
{
    public partial class TipoContrato
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "BNE_Tipo_Contrato";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region TiposContratos
        private static List<TipoContratoCACHE> TiposContratos
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarContratosCACHE, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarContratosCACHE
        /// <summary>
        /// Método que retorna uma lista de itens.
        /// </summary>
        /// <returns>Lista com valores recuperados do banco.</returns>
        private static List<TipoContratoCACHE> ListarContratosCACHE()
        {
            var lista = new List<TipoContratoCACHE>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpListartipoContrato, null))
            {
                while (dr.Read())
                {
                    lista.Add(new TipoContratoCACHE
                    {
                        Identificador = Convert.ToInt32(dr["Idf_Tipo_Contrato"]),
                        Descricao = dr["Des_Tipo_Contrato"].ToString()
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

        #region TipoContratoCACHE
        private class TipoContratoCACHE
        {
            public int Identificador { get; set; }
            public string Descricao { get; set; }
        }
        #endregion

        #endregion

        #endregion

        #region Consultas

        #region SpListartipoContrato
        private const string SpListartipoContrato = @"
        SELECT  Idf_Tipo_Contrato, 
                Des_Tipo_Contrato 
        FROM    plataforma.BNE_Tipo_Contrato WITH (NOLOCK)
        ORDER BY Des_Tipo_Contrato ASC";
        #endregion

//        #region Spselectpordescricao
//        private const string Spselectpordescricao = @"
//        SELECT  Idf_Tipo_Contrato, 
//                Des_Tipo_Contrato,
//                Dta_Cadastro 
//        FROM    BNE_Tipo_Contrato WITH (NOLOCK)
//        WHERE Des_Tipo_Contrato LIKE @Descricao
//        ORDER BY Des_Tipo_Contrato ASC";
//        #endregion
        #endregion

        #region Métodos

        #region ListarTipoContrato
        /// <summary>
        /// Método retorna Consulta para carregar DropDownList
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ListarTipoContrato()
        {
            #region Cache
            if (HabilitaCache)
                return TiposContratos.ToDictionary(tv => tv.Identificador.ToString(), tv => tv.Descricao);
            #endregion

            var dicionarioTipoContrato = new Dictionary<string, string>();

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpListartipoContrato, null))
            {
                while (dr.Read())
                {
                    dicionarioTipoContrato.Add(dr["Idf_Tipo_Contrato"].ToString(), dr["Des_Tipo_Contrato"].ToString());
                }
                if (!dr.IsClosed)
                    dr.Close();
            }

            return dicionarioTipoContrato;
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado para vincular as colunas com os atributos da classe, definindo se a leitura do DataReader e o Dispose devem ser excutado.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objTipoContrato">Instância a ser manipulada.</param>
        /// <param name="executeDispose">Define se o dispose no DataReader deve ser executado</param>
        /// <param name="executeRead">Define se o read no DataReader deve ser executado</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Francisco Ribas</remarks>
        internal static bool SetInstance(IDataReader dr, TipoContrato objTipoContrato, Boolean executeDispose, Boolean executeRead)
        {
            try
            {
                if (!executeRead || dr.Read())
                {
                    objTipoContrato._idfTipoContrato = Convert.ToInt32(dr["idf_Tipo_Contrato"]);
                    if (dr["Des_Tipo_Contrato"] != DBNull.Value)
                        objTipoContrato._descricaoTipoContrato = Convert.ToString(dr["Des_Tipo_Contrato"]);
                    objTipoContrato._flgInativo = Convert.ToBoolean(dr["flg_Inativo"]);

                    objTipoContrato._persisted = true;
                    objTipoContrato._modified = false;

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
                if (executeDispose)
                    dr.Dispose();
            }
        }
        #endregion

        #endregion

    }
}
