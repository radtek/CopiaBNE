//-- Data: 22/03/2014 17:09
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using BNE.Cache;

namespace BNE.BLL
{
    public partial class EmpresaHome // Tabela: BNE_Empresa_Home
    {

        #region Configuração de cache

        static readonly ICachingService Cache = CachingServiceProvider.Instance;

        #region Parametros
        private const string ObjectKey = "bne.BNE_Empresa_Home";
        private const double SlidingExpiration = 1440;
        private static readonly bool HabilitaCache = ConfigurationManager.AppSettings["HabilitaCache"] != null && Convert.ToBoolean(ConfigurationManager.AppSettings["HabilitaCache"]);
        #endregion

        #region Cached Objects

        #region EmpresasHome
        public static List<EmpresaHome> EmpresasHome
        {
            get
            {
                return Cache.GetItem(ObjectKey, ListarEmpresasHomeCache, SlidingExpiration);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region ListarEmpresasHomeCache
        internal static List<EmpresaHome> ListarEmpresasHomeCache()
        {
            var lista = new List<EmpresaHome>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarempresas, null))
            {
                while (dr.Read())
                {
                    var objEmpresaHome = new EmpresaHome();
                    if (SetInstance_NotDispose(dr, objEmpresaHome))
                        lista.Add(objEmpresaHome);
                }
            }
            return lista;
        }
        #endregion

        #endregion

        #endregion

        #region Consultas
        private const string Sprecuperarempresas = "SELECT * FROM BNE_Empresa_Home WITH(NOLOCK) WHERE Flg_Inativo = 0";
        #endregion

        #region Métodos

        #region ListarEmpresasHome
        public static List<EmpresaHome> ListarEmpresasHome()
        {
            #region Cache
            if (HabilitaCache)
                return EmpresasHome;
            #endregion

            var lista = new List<EmpresaHome>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarempresas, null))
            {
                while (dr.Read())
                {
                    var objEmpresaHome = new EmpresaHome();
                    if (SetInstance_NotDispose(dr, objEmpresaHome))
                        lista.Add(objEmpresaHome);
                }
            }
            return lista;
        }
        #endregion

        #region SetInstance
        /// <summary>
        /// Método auxiliar utilizado pelos métodos LoadObject e CompleteObject para percorrer um IDataReader e vincular as colunas com os atributos da classe.
        /// </summary>
        /// <param name="dr">Cursor de leitura do banco de dados.</param>
        /// <param name="objEmpresaHome">Instância a ser manipulada.</param>
        /// <returns>Verdadeiro ou falso informando se a operação foi executada com sucesso.</returns>
        /// <remarks>Gieyson Stelmak</remarks>
        private static bool SetInstance_NotDispose(IDataReader dr, EmpresaHome objEmpresaHome)
        {
            objEmpresaHome._filial = new Filial(Convert.ToInt32(dr["Idf_Filial"]));
            objEmpresaHome._descricaoNomeURL = Convert.ToString(dr["Des_Nome_URL"]);
            objEmpresaHome._descricaoCaminhoImagem = Convert.ToString(dr["Des_Caminho_Imagem"]);
            objEmpresaHome._flagInativo = Convert.ToBoolean(dr["Flg_Inativo"]);
            objEmpresaHome._dataCadastro = Convert.ToDateTime(dr["Dta_Cadastro"]);

            objEmpresaHome._persisted = true;
            objEmpresaHome._modified = false;

            return true;
        }
        #endregion

        #endregion
    }
}