//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.AsyncServices
{
	public partial class PluginsCompatibilidade // Tabela: plataforma.TAB_Plugins_Compatibilidade
	{

        #region Consultas

        #region Spcarregarpormetadata
        private const String Spcarregarpormetadata = @"
        select
         pc.* 
        from plataforma.TAB_Plugins_Compatibilidade pc
          join plataforma.TAB_Plugin pe on (pc.Idf_Plugin_Entrada = pe.Idf_Plugin)
          join plataforma.TAB_Plugin ps on (pc.Idf_Plugin_Saida = ps.Idf_Plugin)
        where 
          pe.Des_Plugin_Metadata = @Des_Plugin_Metadata_Entrada
          and ps.Des_Plugin_Metadata = @Des_Plugin_Metadata_Saida";
        #endregion

        #endregion

        #region Métodos

        #region CarregarPorMetadata
        /// <summary>
        /// Carrega a relação de compatibilidade pelas metadatas
        /// </summary>
        /// <param name="metaPluginEntrada">Metadata do plugin de entrada</param>
        /// <param name="metaPluginSaida">Metadata do plugin de saída</param>
        /// <returns>A relação de compatibilidade</returns>
        public static PluginsCompatibilidade CarregarPorMetadata(String metaPluginEntrada, String metaPluginSaida)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Des_Plugin_Metadata_Entrada", SqlDbType = SqlDbType.VarChar, Value = metaPluginEntrada } ,
                    new SqlParameter { ParameterName = "@Des_Plugin_Metadata_Saida", SqlDbType = SqlDbType.VarChar, Value = metaPluginSaida }
                };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spcarregarpormetadata, parms))
            {
                var objCompatibilidade = new PluginsCompatibilidade();
                if (SetInstance(dr, objCompatibilidade))
                    return objCompatibilidade;

                return null;
            }
        }
        #endregion

        #endregion

	}
}