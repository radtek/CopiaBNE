namespace BNE.PessoaFisica.Web.Helpers.SEO
{
    public class LinkHelper
    {
        #region ObterLinkVagasFuncao
        public static SEOLink ObterLinkVagasFuncao(string descricaoFuncao, string url)
        {
            return new SEOLink("Vagas para " + descricaoFuncao, "Vagas de emprego para " + descricaoFuncao, url);
        }
        #endregion

        #region ObterLinkVagasCidade
        /// <summary>
        ///     Método para obter um Objeto SEOLink para a página de Vagas por Cidade
        /// </summary>
        /// <param name="nomeCidade">Nome da cidade para a qual o link deve ser gerado</param>
        /// <param name="siglaEstado">Sigla do estado da cidade para a qual o link deve ser gerado</param>
        /// <param name="quantidadeVagas">
        ///     Quantidade de vagas a ser exibida na descrição. Se não houver, ignorar esse parâmetro ou
        ///     passar nulo.
        /// </param>
        /// <returns>SEO Link customizado com a descrição, titke e urk para a página de vaga por cidade.</returns>
        public static SEOLink ObterLinkVagasCidade(string nomeCidade, string siglaEstado, string url)
        {
            return new SEOLink(string.Format("Vagas em {0}/{1}", nomeCidade, siglaEstado), string.Format("Vagas de emprego em {0}/{1}", nomeCidade, siglaEstado), url);
        }
        #endregion

        #region ObterLinkVagasFuncaoCidade       
        /// <summary>
        ///     Método para obter um Objeto SEOLink para a página de Vagas por Função e Cidade
        /// </summary>
        /// <param name="descricaoFuncao">Descrição da Função para a qual o link deve ser gerado</param>
        /// <param name="nomeCidade">Nome da cidade para a qual o link deve ser gerado</param>
        /// <param name="siglaEstado">Sigla do estado da cidade para a qual o link deve ser gerado</param>
        /// <param name="quantidadeVagas">
        ///     Quantidade de vagas a ser exibida na descrição. Se não houver, ignorar esse parâmetro ou
        ///     passar nulo.
        /// </param>
        /// <returns>SEO Link customizado com a descrição, titke e urk para a página de vaga por função e cidade.</returns>
        public static SEOLink ObterLinkVagasFuncaoCidade(string descricaoFuncao, string nomeCidade, string siglaEstado, string url)
        {
            return new SEOLink(string.Format("Vagas para {0} em {1}/{2}", descricaoFuncao, nomeCidade, siglaEstado), string.Format("Vagas de emprego para {0} em {1}/{2}", descricaoFuncao, nomeCidade, siglaEstado), url);
        }
        #endregion

        #region ObterLinkVagasArea
        /// <summary>
        ///     Método para obter um Objeto SEOLink para a página de Vagas por Area BNE.
        /// </summary>
        /// <param name="areaBNE">Descrição da Área para a qual o link deve ser gerado.</param>
        /// <param name="quantidadeVagas">
        ///     Quantidade de vagas a ser exibida na descrição. Se não houver, ignorar esse parâmetro ou
        ///     passar nulo.
        /// </param>
        /// <returns>SEO Link customizado com a descrição, titke e urk para a página de vaga por Área BNE.</returns>
        public static SEOLink ObterLinkVagasArea(string areaBNE, string url)
        {
            return new SEOLink(string.Format("Vagas na área de {0}", areaBNE), string.Format("Vagas de emprego para na área de {0}", areaBNE), url);
        }
        #endregion
    }
}