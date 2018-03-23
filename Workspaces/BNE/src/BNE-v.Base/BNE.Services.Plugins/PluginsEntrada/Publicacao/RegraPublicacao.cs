using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BNE.BLL;
using BNE.BLL.Custom;

namespace BNE.Services.Plugins.PluginsEntrada.Publicacao
{
    internal class RegraPublicacao
    {

        #region Propriedades

        public AcaoPublicacao AcaoPublicacao { get; set; }
        public string DescricaoCampo { get; set; }
        public string DescricaoPalavraPublicacao { get; set; }
        public string DescricaoPalavraSubstituicao { get; set; }
        public bool AplicarRegex { get; set; }

        #endregion

        #region Consultas

        #region Sprecuperarregrapublicacao
        private const string Sprecuperarregrapublicacao = @"
        /* Regras para um tipo específico de publicação  */
        SELECT  Des_Palavra_Publicacao, 
                Des_Palavra_Substituicao,
                Idf_Acao_Publicacao,
                Des_Campo_Publicacao,
                Flg_Aplicar_Regex
        FROM    BNE_Regra_Publicacao RP WITH(NOLOCK)
                LEFT JOIN BNE_Palavra_Publicacao PP WITH(NOLOCK) ON RP.Idf_Palavra_Publicacao = PP.Idf_Palavra_Publicacao
                INNER JOIN BNE_Regra_Campo_Publicacao RCP WITH(NOLOCK) ON RP.Idf_Regra_Publicacao = RCP.Idf_Regra_Publicacao
                INNER JOIN BNE_Campo_Publicacao CP WITH(NOLOCK) ON RCP.Idf_Campo_Publicacao = CP.Idf_Campo_Publicacao
        WHERE   ( PP.Flg_Inativo = 0 OR PP.Flg_Inativo IS NULL )
                AND RP.Flg_Inativo = 0
                AND RCP.Flg_Inativo = 0
                AND CP.Idf_Tipo_Publicacao = @Idf_Tipo_Publicacao
        ";
        #endregion

        #endregion

        #region Métodos

        #region RecuperarRegrasPublicacao
        /// <summary>
        /// Recuperar uma lista com todas as regras de publicação. Regra geral (para todo tipo de publicacao), regra só da publicação de vaga e regra específica de uma filial, uma regra não exclui outra
        /// </summary>
        /// <returns></returns>
        public static List<RegraPublicacao> RecuperarRegrasPublicacao(TipoPublicacao tipoPublicacao)
        {
            var parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Idf_Tipo_Publicacao", SqlDbType = SqlDbType.Int, Size = 4, Value = (int)tipoPublicacao }
                };

            var lista = new List<RegraPublicacao>();
            using (IDataReader dr = DataAccessLayer.ExecuteReader(CommandType.Text, Sprecuperarregrapublicacao, parms))
            {
                while (dr.Read())
                {
                    lista.Add(new RegraPublicacao
                    {
                        AcaoPublicacao = (AcaoPublicacao)Enum.Parse(typeof(AcaoPublicacao), dr["Idf_Acao_Publicacao"].ToString()),
                        DescricaoPalavraPublicacao = dr["Des_Palavra_Publicacao"] != null ? dr["Des_Palavra_Publicacao"].ToString() : string.Empty,
                        DescricaoPalavraSubstituicao = dr["Des_Palavra_Substituicao"] != null ? dr["Des_Palavra_Substituicao"].ToString() : string.Empty,
                        DescricaoCampo = dr["Des_Campo_Publicacao"].ToString(),
                        AplicarRegex = Convert.ToBoolean(dr["Flg_Aplicar_Regex"])
                    });
                }
            }

            return lista;
        }
        #endregion

        #region MontarRegex
        public static Regex MontarRegex(string regexPublicacao, Publicacao.RegraPublicacao regraPublicacao)
        {
            #region Regex
            string regexPalavra;
            if (regraPublicacao.AplicarRegex)
            {
                var parametros = new
                {
                    palavra = regraPublicacao.DescricaoPalavraPublicacao
                };
                regexPalavra = parametros.ToString(regexPublicacao);
            }
            else
                regexPalavra = regraPublicacao.DescricaoPalavraPublicacao;

            return new Regex(regexPalavra, RegexOptions.IgnoreCase);
        }
            #endregion

        #endregion

        #endregion

    }
}
