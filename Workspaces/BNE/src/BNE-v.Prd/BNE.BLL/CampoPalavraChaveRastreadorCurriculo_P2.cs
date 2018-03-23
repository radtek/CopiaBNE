//-- Data: 20/01/2016 16:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
namespace BNE.BLL
{
	public partial class CampoPalavraChaveRastreadorCurriculo // Tabela: BNE_Campo_Palavra_Chave_Rastreador_Curriculo
	{

        #region Consultas

        #region Spselectdescricaopalavrachavesolr
        private const string Spselectdescricaopalavrachavesolr = @" 
        SELECT  PC.Nme_Campo_Palavra_Chave_SOLR
        FROM    BNE_Campo_Palavra_Chave_Rastreador_Curriculo PCRastreador WITH(NOLOCK)
                INNER JOIN BNE_Campo_Palavra_Chave PC WITH(NOLOCK) ON PCRastreador.Idf_Campo_Palavra_Chave = PC.Idf_Campo_Palavra_Chave
        WHERE   Idf_Rastreador_Curriculo = @Idf_Rastreador_Curriculo";
        #endregion

        #endregion

        #region Métodos

        #region ListarPalavraChave
        /// <summary>
        /// Recupera uma lista com os nomes dos campos no Solr para efetuar uma busca
        /// </summary>
        /// <param name="objPesquisaCurriculo"></param>
        /// <returns></returns>
        public static List<string> ListarPalavraChave(RastreadorCurriculo objPesquisaCurriculo)
        {
            var lista = new List<string>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Rastreador_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.IdRastreadorCurriculo }
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, Spselectdescricaopalavrachavesolr, parms))
            {
                while (dr.Read())
                    lista.Add(dr["Nme_Campo_Palavra_Chave_SOLR"].ToString());

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion

        #endregion


	}
}