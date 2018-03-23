using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BNE.Core.ExtensionsMethods;

namespace BNE.PessoaFisica.Web.Helpers.SEO
{
    public class SitemapHelper
    {
        static readonly string UrlSite = "http://www.bne.com.br";
        static readonly string SitemapTituloVaga = "Vaga de Emprego de {Funcao} em {Cidade}/{SiglaEstado} - Vaga {Identificador} | BNE - Banco Nacional de Empregos";
        

        #region MontarTituloVaga
        /// <summary>
        /// Padronização de nome para o título das telas de vaga
        /// </summary>
        /// <param name="nomeFuncao">Descrição da Função</param>
        /// <param name="nomeAreaBNE">Nome da área no BNE</param>
        /// <param name="quantidadeVaga">Quantidade de Vagas</param>
        /// <param name="nomeCidade">Nome da Cidade</param>
        /// <param name="nomeEstado">Nome do Estado</param>
        /// <param name="siglaEstado">Sigla do Estado</param>
        /// <param name="identificadorVaga">Identificador da Vaga</param>
        /// <returns></returns>
        public static string MontarTituloVaga(string nomeFuncao, string nomeAreaBNE, short quantidadeVaga, string nomeCidade, string nomeEstado, string siglaEstado, int identificadorVaga)
        {
            var parametros = new
            {
                URLSite = UrlSite,
                Funcao = nomeFuncao,
                AreaBNE = nomeAreaBNE,
                Cidade = nomeCidade,
                Estado = nomeEstado,
                SiglaEstado = siglaEstado,
                QuantidadeVaga = quantidadeVaga,
                DescricaoQuantidadeVaga = quantidadeVaga.Equals(1) ? "vaga" : "vagas",
                Identificador = identificadorVaga
            };

            return parametros.ToString(SitemapTituloVaga);
        }
        #endregion
    }
}