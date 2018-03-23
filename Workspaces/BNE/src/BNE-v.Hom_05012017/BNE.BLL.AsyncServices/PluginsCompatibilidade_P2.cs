//-- Data: 05/10/2012 14:44
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL.AsyncServices
{
    public partial class PluginsCompatibilidade // Tabela: TAB_Plugins_Compatibilidade
    {

        #region Consultas

        #region Spcarregarportipoatividade
        private const String Spcarregarportipoatividade = @"
        SELECT  *
        FROM    TAB_Plugins_Compatibilidade
        WHERE   Idf_Tipo_Atividade = @Idf_Tipo_Atividade";
        #endregion

        #endregion

        #region Métodos

        #region CarregarPorTipoAtividade
        /// <summary>
        /// Carrega a relação de compatibilidade pelas metadatas
        /// </summary>
        /// <param name="tipoAtividade">Tipo da atividade</param>
        /// <returns>A relação de compatibilidade</returns>
        public static PluginsCompatibilidade CarregarPorTipoAtividade(Enumeradores.TipoAtividade tipoAtividade)
        {
            var parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@Idf_Tipo_Atividade", SqlDbType = SqlDbType.Int, Value = (int)tipoAtividade } 
            };

            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spcarregarportipoatividade, parms))
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