using BNE.BLL.Common;
using BNE.BLL.Enumeradores;
using System;

namespace BNE.BLL.Custom
{

    public static class SitemapHelper
    {

        static readonly string UrlSite = string.Concat("http://", Helper.RecuperarURLAmbiente());
        static readonly string UrlSiteVaga = string.Concat("http://", Helper.RecuperarURLVagas());
        static readonly string SitemapTituloCurriculo = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SitemapTituloCurriculo);
        static readonly string SitemapTituloVaga = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SitemapTituloVaga);
        static readonly string SitemapURLCurriculoEspecifico = Rota.RecuperarURLRota(RouteCollection.Curriculo);
        static readonly string URLVisualizacaoCurriculo = Rota.RecuperarURLRota(RouteCollection.VisualizacaoCurriculo);
        static readonly string SitemapURLCurriculosFuncao = Rota.RecuperarURLRota(RouteCollection.CurriculosPorFuncao);
        static readonly string SitemapURLCurriculosCidade = Rota.RecuperarURLRota(RouteCollection.CurriculosPorCidade);
        static readonly string SitemapURLCurriculosFuncaoCidade = Rota.RecuperarURLRota(RouteCollection.CurriculosPorFuncaoCidade);
        static readonly string SitemapURLVagaEspecifica = Rota.RecuperarURLRota(RouteCollection.Vaga);
        static readonly string SitemapURLVagasFuncao = Rota.RecuperarURLRota(RouteCollection.VagasPorFuncao);
        static readonly string SitemapURLVagasCidade = Rota.RecuperarURLRota(RouteCollection.VagasPorCidade);
        static readonly string SitemapURLVagasFuncaoCidade = Rota.RecuperarURLRota(RouteCollection.VagasPorFuncaoCidade);
        static readonly string SitemapURLVagasPalavraChave = Rota.RecuperarURLRota(RouteCollection.VagasPorPalavraChave);
        static readonly string SitemapURLVagasPorArea = Rota.RecuperarURLRota(RouteCollection.VagasPorArea);
        static readonly string SitemapURLVagas = Rota.RecuperarURLRota(RouteCollection.Vagas);

        static readonly string SitemapURLBuscaDeVaga = Rota.RecuperarURLRota(RouteCollection.BuscaDeVagas);
        static readonly string SitemapURLBuscaDeVagasPorEstado = Rota.RecuperarURLRota(RouteCollection.BuscaDeVagasPorEstado);
        static readonly string SitemapURLBuscaDeVagasPorArea = Rota.RecuperarURLRota(RouteCollection.BuscaDeVagasPorArea);
        static readonly string SitemapURLBuscaDeVagasPorCidade = Rota.RecuperarURLRota(RouteCollection.BuscaDeVagasPorCidade);

        #region MontarUrlCurriculo
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="identificadorCurriculo"></param>
        /// <returns></returns>
        public static string MontarUrlCurriculo(string nomeFuncao, string nomeCidade, string siglaEstado, int identificadorCurriculo)
        {
            var parametros = new
            {
                URLSite = UrlSite,
                Funcao = nomeFuncao.NormalizarURL(),
                Cidade = nomeCidade.NormalizarURL(),
                SiglaEstado = siglaEstado.ToLower(),
                Identificador = identificadorCurriculo
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLCurriculoEspecifico));
        }
        #endregion

        #region MontarUrlVisualizacaoCurriculo
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="identificadorCurriculo"></param>
        /// <returns></returns>
        public static string MontarUrlVisualizacaoCurriculo(string nomeFuncao, string nomeCidade, string siglaEstado, int identificadorCurriculo)
        {
            return MontarUrlVisualizacaoCurriculo(nomeFuncao, nomeCidade, siglaEstado, identificadorCurriculo, null,null);
        }
        public static string MontarUrlVisualizacaoCurriculo(string nomeFuncao, string nomeCidade, string siglaEstado, int identificadorCurriculo, decimal? cpf, DateTime? dataNascimento)
        {
            var parametros = new
            {
                URLSite = UrlSite
            };
            if (cpf.HasValue && dataNascimento.HasValue)
            {
                string destino = parametros.ToString(string.Concat("{URLSite}", MontarCaminhoVisualizacaoCurriculo(nomeFuncao, nomeCidade, siglaEstado, identificadorCurriculo)));
                return BLL.Common.LoginAutomatico.GerarUrl(cpf.Value, dataNascimento.Value, destino);
            }
            else
                return parametros.ToString(string.Concat("{URLSite}", MontarCaminhoVisualizacaoCurriculo(nomeFuncao, nomeCidade, siglaEstado, identificadorCurriculo)));
        }
        #endregion

        #region MontarCaminhoVisualizacaoCurriculo
        public static string MontarCaminhoVisualizacaoCurriculo(string nomeFuncao, string nomeCidade, string siglaEstado, int identificadorCurriculo)
        {
            var parametros = new
            {
                Funcao = nomeFuncao.Replace("/","-").NormalizarURL(),
                Cidade = nomeCidade.NormalizarURL(),
                SiglaEstado = siglaEstado.ToLower(),
                Identificador = identificadorCurriculo
            };

            return parametros.ToString(string.Concat("/",URLVisualizacaoCurriculo));
        }
        #endregion

        #region MontarUrlCurriculosPorFuncao
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <returns></returns>
        public static string MontarUrlCurriculosPorFuncao(string nomeFuncao)
        {
            var parametros = new
            {
                URLSite = UrlSite,
                Funcao = nomeFuncao.NormalizarURL()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLCurriculosFuncao));
        }
        #endregion

        #region MontarUrlCurriculosPorCidade
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <returns></returns>
        public static string MontarUrlCurriculosPorCidade(string nomeCidade, string siglaEstado)
        {
            var parametros = new
            {
                URLSite = UrlSite,
                Cidade = nomeCidade.NormalizarURL(),
                SiglaEstado = siglaEstado.ToLower()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLCurriculosCidade));
        }
        #endregion

        #region MontarUrlCurriculosPorFuncaoCidade
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <returns></returns>
        public static string MontarUrlCurriculosPorFuncaoCidade(string nomeFuncao, string nomeCidade, string siglaEstado)
        {
            var parametros = new
            {
                URLSite = UrlSite,
                Funcao = nomeFuncao.NormalizarURL(),
                Cidade = nomeCidade.NormalizarURL(),
                SiglaEstado = siglaEstado.ToLower()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLCurriculosFuncaoCidade));
        }
        #endregion

        #region MontarTituloCurriculo
        /// <summary>
        /// Padronização de nome para o título das telas de currículo
        /// </summary>
        /// <param name="identificadorCurriculo"></param>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeCidade"></param>
        /// <param name="nomeEstado"> </param>
        /// <param name="siglaEstado"></param>
        /// <returns></returns>
        public static string MontarTituloCurriculo(string nomeFuncao, string nomeCidade, string nomeEstado, string siglaEstado, int identificadorCurriculo)
        {
            var parametros = new
            {
                UrlSite = UrlSite,
                Funcao = nomeFuncao,
                Cidade = nomeCidade,
                Estado = nomeEstado,
                SiglaEstado = siglaEstado,
                Identificador = identificadorCurriculo
            };

            return parametros.ToString(SitemapTituloCurriculo);
        }
        #endregion

        #region MontarUrlVagas
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeAreaBNE"></param>
        /// <returns></returns>
        public static string MontarUrlVagas()
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLVagas));
        }
        #endregion

        #region MontarUrlVagasPorFuncao
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeAreaBNE"></param>
        /// <returns></returns>
        public static string MontarUrlVagasPorFuncao(string nomeFuncao)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                Funcao = nomeFuncao.NormalizarURL()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLVagasFuncao));
        }
        #endregion

        #region MontarUrlVagasPorCidade
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <returns></returns>
        public static string MontarUrlVagasPorCidade(string nomeCidade, string siglaEstado)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                Cidade = nomeCidade.NormalizarURL(),
                SiglaEstado = siglaEstado.NormalizarURL()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLVagasCidade));
        }
        #endregion

        #region MontarUrlVagasPorFuncaoCidade
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <returns></returns>
        public static string MontarUrlVagasPorFuncaoCidade(string nomeFuncao, string nomeCidade, string siglaEstado)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                Funcao = nomeFuncao.NormalizarURL(),
                Cidade = nomeCidade.NormalizarURL(),
                SiglaEstado = siglaEstado.ToLower()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLVagasFuncaoCidade));
        }
        #endregion

        #region MontarUrlVagasPorPalavraChave
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="palavraChave"></param>
        /// <returns></returns>
        public static string MontarUrlVagasPorPalavraChave(string palavraChave)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                PalavraChave = palavraChave.NormalizarURL()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLVagasPalavraChave));
        }
        #endregion

        #region MontarUrlVagasPorArea
        /// <summary>
        /// Padronização da vaga
        /// </summary>
        /// <param name="palavraChave"></param>
        /// <returns></returns>
        public static string MontarUrlVagasPorArea(string AreaBNE)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                areaBNE = AreaBNE.NormalizarURL()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLVagasPorArea));
        }
        #endregion

        #region MontarUrlBuscaDeVaga
        /// <summary>
        /// Padronização da vaga
        /// </summary>
        /// <param name="palavraChave"></param>
        /// <returns></returns>
        public static string MontarUrlBuscaDeVaga()
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLVagasPorArea));
        }
        #endregion

        #region MontarUrlBuscaDeVagasPorEstado
        /// <summary>
        /// Padronização da vaga
        /// </summary>
        /// <param name="palavraChave"></param>
        /// <returns></returns>
        public static string MontarUrlBuscaDeVagasPorEstado(string estado)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                estado = estado.NormalizarURL()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLBuscaDeVagasPorEstado));
        }
        #endregion

        #region MontarUrlBuscaDeVagasPorArea
        /// <summary>
        /// Padronização da vaga
        /// </summary>
        /// <param name="palavraChave"></param>
        /// <returns></returns>
        public static string MontarUrlBuscaDeVagasPorArea(string area)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                area = area.NormalizarURL()
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLBuscaDeVagasPorArea));
        }
        #endregion

        #region MontarUrlBuscaDeVagasPorCidade
        /// <summary>
        /// Padronização da vaga
        /// </summary>
        /// <param name="palavraChave"></param>
        /// <returns></returns>
        public static string MontarUrlBuscaDeVagasPorCidade(string cidade, string siglaEstado)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                cidade = cidade.NormalizarURL(),
                siglaEstado = siglaEstado.NormalizarURL(),
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLBuscaDeVagasPorCidade));
        }
        #endregion

        #region MontarUrlVaga
        /// <summary>
        /// Padronização da url de vaga
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeAreaBNE"> </param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="identificadorVaga"></param>
        /// <returns></returns>
        public static string MontarUrlVaga(string nomeFuncao, string nomeAreaBNE, string nomeCidade, string siglaEstado, int identificadorVaga)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                Funcao = nomeFuncao.NormalizarURL(),
                AreaBNE = nomeAreaBNE.NormalizarURL(),
                Cidade = nomeCidade.NormalizarURL(),
                SiglaEstado = siglaEstado.ToLower(),
                Identificador = identificadorVaga
            };

            return parametros.ToString(string.Concat("{URLSite}/", SitemapURLVagaEspecifica));
        }
        #endregion

        #region MontarUrlVagaEstagio
        /// <summary>
        /// Padronização da url de vaga
        /// </summary>
        /// <param name="nomeFuncao"></param>
        /// <param name="nomeAreaBNE"> </param>
        /// <param name="nomeCidade"></param>
        /// <param name="siglaEstado"></param>
        /// <param name="identificadorVaga"></param>
        /// <returns></returns>
        public static string MontarUrlVagaEstagio(bool estagio, string nomeFuncao, string nomeAreaBNE, string nomeCidade, string siglaEstado, int identificadorVaga)
        {
            var parametros = new
            {
                URLSite = UrlSiteVaga,
                Funcao = nomeFuncao.NormalizarURL(),
                AreaBNE = nomeAreaBNE.NormalizarURL(),
                Cidade = nomeCidade.NormalizarURL(),
                SiglaEstado = siglaEstado.ToLower(),
                Identificador = identificadorVaga
            };

            return parametros.ToString(string.Concat("{URLSite}/", estagio ? "estagio-para-" + SitemapURLVagaEspecifica : SitemapURLVagaEspecifica));
        }
        #endregion

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

        #region MontarUrlEmpresa
        /// <summary>
        /// Padronização da url de empresa
        /// </summary>
        /// <param name="descricaoDiretorio"></param>
        /// <returns></returns>
        public static string MontarUrlEmpresa(string descricaoDiretorio)
        {
            const string urlSiteStc = "http://www.envieseucurriculo.com.br";
            return string.Format("{0}/{1}", urlSiteStc, descricaoDiretorio);
        }
        #endregion

        #region MontarUrl
        /// <summary>
        /// Padronização da url de currículo
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string MontarUrl(string url)
        {
            return string.Format("{0}/{1}", UrlSite, url);
        }
        #endregion
    }

    internal enum TipoSitemap
    {
        Curriculo,
        Vaga,
        Empresa
    }
}
