using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Vagas.Code.Helpers.SEO
{
    public class LinkHelper
    {
        /// <summary>
        /// Método para obter um Objeto SEOLink para a página de Vagas por Função
        /// </summary>
        /// <param name="descricaoFuncao">Descrição da Função para a qual o link deve ser gerado</param>
        /// <param name="quantidadeVagas">Quantidade de vagas a ser exibida na descrição. Se não houver, ignorar esse parâmetro ou passar nulo.</param>
        /// <returns>SEO Link customizado com a descrição, titke e urk para a página de vaga por função.</returns>
        public static SEOLink ObterLinkVaga(string descricaoFuncao, int? quantidadeVagas = null)
        {
            if (quantidadeVagas.HasValue)
            {
                return new SEOLink()
                {
                    Descricao = String.Format("{0} Vagas para {1}", quantidadeVagas, descricaoFuncao),
                    Title = "Vagas de emprego para " + descricaoFuncao,
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncao(descricaoFuncao)
                };
            }
            else
            {
                return new SEOLink()
                {
                    Descricao = "Vagas para " + descricaoFuncao,
                    Title = "Vagas de emprego para " + descricaoFuncao,
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncao(descricaoFuncao)
                };
            }
        }
        /// <summary>
        /// Método para obter um Objeto SEOLink para a página de Vagas por Função
        /// </summary>
        /// <param name="descricaoFuncao">Descrição da Função para a qual o link deve ser gerado</param>
        /// <param name="quantidadeVagas">Quantidade de vagas a ser exibida na descrição. Se não houver, ignorar esse parâmetro ou passar nulo.</param>
        /// <returns>SEO Link customizado com a descrição, titke e urk para a página de vaga por função.</returns>
        public static SEOLink ObterLinkVagasFuncao(string descricaoFuncao, int? quantidadeVagas = null)
        {
            if (quantidadeVagas.HasValue)
            {
                return new SEOLink()
                {
                    Descricao = String.Format("{0} Vagas para {1}", quantidadeVagas, descricaoFuncao),
                    Title = "Vagas de emprego para " + descricaoFuncao,
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncao(descricaoFuncao)
                };
            }
            else
            {
                return new SEOLink()
                {
                    Descricao = "Vagas para " + descricaoFuncao,
                    Title = "Vagas de emprego para " + descricaoFuncao,
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncao(descricaoFuncao)
                };
            }
        }

        /// <summary>
        /// Método para obter um Objeto SEOLink para a página de Vagas por Cidade
        /// </summary>
        /// <param name="nomeCidade">Nome da cidade para a qual o link deve ser gerado</param>
        /// <param name="siglaEstado">Sigla do estado da cidade para a qual o link deve ser gerado</param>
        /// <param name="quantidadeVagas">Quantidade de vagas a ser exibida na descrição. Se não houver, ignorar esse parâmetro ou passar nulo.</param>
        /// <returns>SEO Link customizado com a descrição, titke e urk para a página de vaga por cidade.</returns>
        public static SEOLink ObterLinkVagasCidade(string nomeCidade, string siglaEstado, int? quantidadeVagas = null)
        {
            if (quantidadeVagas.HasValue)
            {
                return new SEOLink()
                {
                    Descricao = String.Format("{0} Vagas em {1}/{2}", quantidadeVagas, nomeCidade, siglaEstado),
                    Title = String.Format("Vagas de emprego em {0}/{1}", nomeCidade, siglaEstado),
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorCidade(nomeCidade, siglaEstado)
                };
            }
            else
            {
                return new SEOLink()
                {
                    Descricao = String.Format("Vagas em {0}/{1}", nomeCidade, siglaEstado),
                    Title = String.Format("Vagas de emprego em {0}/{1}", nomeCidade, siglaEstado),
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorCidade(nomeCidade, siglaEstado)
                };
            }
        }

        /// <summary>
        /// Método para obter um Objeto SEOLink para a página de Vagas por Função e Cidade
        /// </summary>
        /// <param name="descricaoFuncao">Descrição da Função para a qual o link deve ser gerado</param>
        /// <param name="nomeCidade">Nome da cidade para a qual o link deve ser gerado</param>
        /// <param name="siglaEstado">Sigla do estado da cidade para a qual o link deve ser gerado</param>
        /// <param name="quantidadeVagas">Quantidade de vagas a ser exibida na descrição. Se não houver, ignorar esse parâmetro ou passar nulo.</param>
        /// <returns>SEO Link customizado com a descrição, titke e urk para a página de vaga por função e cidade.</returns>
        public static SEOLink ObterLinkVagasFuncaoCidade(string descricaoFuncao, string nomeCidade, string siglaEstado, int? quantidadeVagas = null)
        {
            if (quantidadeVagas.HasValue)
            {
                return new SEOLink()
                {
                    Descricao = String.Format("{0} Vagas para {1} em {2}/{3}", quantidadeVagas, descricaoFuncao, nomeCidade, siglaEstado),
                    Title = String.Format("Vagas de emprego para {0} em {1}/{2}", descricaoFuncao, nomeCidade, siglaEstado),
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncaoCidade(descricaoFuncao, nomeCidade, siglaEstado)
                };
            }
            else
            {
                return new SEOLink()
                {
                    Descricao = String.Format("Vagas para {0} em {1}/{2}", descricaoFuncao, nomeCidade, siglaEstado),
                    Title = String.Format("Vagas de emprego para {0} em {1}/{2}", descricaoFuncao, nomeCidade, siglaEstado),
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncaoCidade(descricaoFuncao, nomeCidade, siglaEstado)
                };
            }
        }

        /// <summary>
        /// Método para obter um Objeto SEOLink para a página de Vagas por Area BNE.
        /// </summary>
        /// <param name="areaBNE">Descrição da Área para a qual o link deve ser gerado.</param>
        /// <param name="quantidadeVagas">Quantidade de vagas a ser exibida na descrição. Se não houver, ignorar esse parâmetro ou passar nulo.</param>
        /// <returns>SEO Link customizado com a descrição, titke e urk para a página de vaga por Área BNE.</returns>
        public static SEOLink ObterLinkVagasArea(string areaBNE, int? quantidadeVagas = null)
        {
            if (quantidadeVagas.HasValue)
            {
                return new SEOLink()
                {
                    Descricao = String.Format("{0} Vagas na área de {1}", quantidadeVagas, areaBNE),
                    Title = String.Format("Vagas de emprego para na área de {0}", areaBNE),
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorArea(areaBNE)
                };
            }
            else
            {
                return new SEOLink()
                {
                    Descricao = String.Format("Vagas na área de {0}", areaBNE),
                    Title = String.Format("Vagas de emprego para na área de {0}", areaBNE),
                    URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorArea(areaBNE)
                };
            }
        }
    }
}