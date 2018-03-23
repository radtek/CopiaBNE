//-- Data: 20/01/2016 16:52
//-- Autor: Gieyson Stelmak

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace BNE.BLL
{
    public partial class CampoPalavraChavePesquisaCurriculo // Tabela: BNE_Campo_Palavra_Chave_Pesquisa_Curriculo
    {

        #region Consultas

        #region Spselectdescricaopalavrachavesolr
        private const string Spselectdescricaopalavrachavesolr = @" 
        SELECT  PC.Nme_Campo_Palavra_Chave_SOLR
        FROM    BNE_Campo_Palavra_Chave_Pesquisa_Curriculo PCPesquisa WITH(NOLOCK)
                INNER JOIN BNE_Campo_Palavra_Chave PC WITH(NOLOCK) ON PCPesquisa.Idf_Campo_Palavra_Chave = PC.Idf_Campo_Palavra_Chave
        WHERE   Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";
        #endregion

        #region SpselectdescricaopalavrachavesolrPorPesquisa
        private const string SpselectdescricaopalavrachavesolrPorPesquisa = @" 
        SELECT PCPesquisa.idf_campo_palavra_chave
        FROM    BNE_Campo_Palavra_Chave_Pesquisa_Curriculo PCPesquisa WITH(NOLOCK)
                INNER JOIN BNE_Campo_Palavra_Chave PC WITH(NOLOCK) ON PCPesquisa.Idf_Campo_Palavra_Chave = PC.Idf_Campo_Palavra_Chave
        WHERE   Idf_Pesquisa_Curriculo = @Idf_Pesquisa_Curriculo";
        #endregion


        #endregion

        #region Métodos

        #region ListarPalavraChave
        /// <summary>
        /// Recupera uma lista com os nomes dos campos no Solr para efetuar uma busca
        /// </summary>
        /// <param name="objPesquisaCurriculo"></param>
        /// <returns></returns>
        public static List<string> ListarPalavraChave(PesquisaCurriculo objPesquisaCurriculo)
        {
            var lista = new List<string>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.IdPesquisaCurriculo }
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

        #region ListarPalavraChavePorPesquisa
        /// <summary>
        /// Recupera uma lista com os nomes dos campos no Solr para efetuar uma busca
        /// </summary>
        /// <param name="objPesquisaCurriculo"></param>
        /// <returns></returns>
        public static List<int> ListarPalavraChavePorPesquisa(PesquisaCurriculo objPesquisaCurriculo)
        {
            var lista = new List<int>();

            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Pesquisa_Curriculo", SqlDbType = SqlDbType.Int, Size = 4, Value = objPesquisaCurriculo.IdPesquisaCurriculo }
                };

            using (var dr = DataAccessLayer.ExecuteReader(CommandType.Text, SpselectdescricaopalavrachavesolrPorPesquisa, parms))
            {
                while (dr.Read())
                    lista.Add(Convert.ToInt32(dr["idf_campo_palavra_chave"]));

                if (!dr.IsClosed)
                    dr.Close();
            }

            return lista;
        }
        #endregion
        #endregion

    }
}