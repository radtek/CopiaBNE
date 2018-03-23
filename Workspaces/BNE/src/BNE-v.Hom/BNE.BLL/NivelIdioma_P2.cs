//-- Data: 02/03/2010 09:20
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Caching;
namespace BNE.BLL
{
	public partial class NivelIdioma // Tabela: TAB_Nivel_Idioma
    {
        #region Consultas

        private const string SELECTNIVELIDIOMA = @" SELECT  
	                                                    Idf_Nivel_Idioma,
	                                                    Des_Nivel_Idioma
                                                    FROM    TAB_Nivel_Idioma
                                                    ORDER BY Idf_Nivel_Idioma";
        #endregion 

        #region Método 
        /// <summary>
        /// Método faz consulta e retorna o DataReader
        /// </summary>
        /// <returns>DataReader</returns>
        public static IDataReader Listar()
        {
            return DataAccessLayer.ExecuteReader(CommandType.Text, SELECTNIVELIDIOMA, null);
        }
        #endregion

        #region Listar() Dicionary
        /// <summary>
        /// Carregar no telerik
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> ListarDicionary()
        {
            var niveis = new Dictionary<string, string>();

            String cachekey = "TAB_Nivel_Idioma:";
            if (MemoryCache.Default[cachekey] != null)
            {
                return (Dictionary<string,string>)MemoryCache.Default.Get(cachekey);
            }
            var dr = Listar();
            while (dr.Read())
            {
                niveis.Add(dr["Idf_Nivel_Idioma"].ToString(), dr["Des_Nivel_Idioma"].ToString());
            }
            if (!dr.IsClosed)
                dr.Close();
            CacheItemPolicy policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.MinutosLimpezaCache)));
            MemoryCache.Default.Add(cachekey, niveis, policy);
            return niveis;
        }
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