using System;
using System.Web.Mvc;

namespace BNE.PessoaFisica.Web.Controllers
{
    public class LinkController : Controller
    {
        private static readonly string URLBNE = System.Web.HttpContext.Current.Application[Enumeradores.ApplicationKeys.EnderecoBNE.ToString()].ToString();
        private static readonly string URLPattern = "{0}://{1}/{2}";

        #region AtualizarCurriculo
        public string AtualizarCurriculo()
        {
            return RecuperarURL(URLBNE, "cadastro-de-curriculo-gratis");
        }
        #endregion

        #region CadastrarCurriculo
        public string CadastrarCurriculo()
        {
            return RecuperarURL(URLBNE, "cadastro-de-curriculo-gratis");
        }
        #endregion

        #region SalaVIP
        public string SalaVIP()
        {
            return RecuperarURL(URLBNE, "sala-vip");
        }
        #endregion

        #region UltimasVagas
        public string UltimasVagas()
        {
            return RecuperarURL(URLBNE, "vagas-de-emprego");
        }
        #endregion

        #region BuscaVaga
        public string BuscaVaga()
        {
            return RecuperarURL(URLBNE, "busca-de-vagas");
        }
        #endregion

        #region ComprarVIP
        public string ComprarVIP()
        {
            return RecuperarURL(URLBNE, "vip");
        }
        #endregion

        #region QuemMeViu
        public string QuemMeViu()
        {
            return RecuperarURL(URLBNE, "quem-me-viu");
        }
        #endregion

        #region PesquisaVaga
        public string PesquisaVaga()
        {
            return RecuperarURL(URLBNE, "pesquisa-de-vagas");
        }
        #endregion

        #region ExcluirCurriculo
        public string ExcluirCurriculo()
        {
            return RecuperarURL(URLBNE, "fale-com-presidente?action=Excluir");
        }
        #endregion

        #region AnunciarVaga
        public string AnunciarVaga()
        {
            return RecuperarURL(URLBNE, "anunciar-vaga-gratis");
        }
        #endregion

        #region PesquisaCurriculo
        public string PesquisaCurriculo()
        {
            return RecuperarURL(URLBNE, "pesquisa-de-curriculo");
        }
        #endregion

        #region CurriculosRecebidos
        public string CurriculosRecebidos()
        {
            return RecuperarURL(URLBNE, "sala-selecionador-vagas-anunciadas");
        }
        #endregion

        #region SalaSelecionador
        public string SalaSelecionador()
        {
            return RecuperarURL(URLBNE, "sala-selecionador");
        }
        #endregion

        #region ComprarCIA
        public string ComprarCIA()
        {
            return RecuperarURL(URLBNE, "cia");
        }
        #endregion

        #region RecrutamentoR1
        public string RecrutamentoR1()
        {
            return RecuperarURL(URLBNE, "recrutamento-r1");
        }
        #endregion

        #region CadastroCIA
        public string CadastroCIA()
        {
            return RecuperarURL(URLBNE, "cadastro-de-empresa-gratis");
        }
        #endregion

        #region AtualizarCIA
        public string AtualizarCIA()
        {
            return RecuperarURL(URLBNE, "cadastro-de-empresa-gratis");
        }
        #endregion

        #region Home
        public string Home()
        {
            return RecuperarURL(URLBNE, String.Empty);
        }
        #endregion

        #region PesquisaSalarial
        public string PesquisaSalarial()
        {
            return RecuperarURL("www.salariobr.com", String.Empty);
        }
        #endregion

        #region TesteSistmars
        public string TesteSistmars()
        {
            return RecuperarURL(URLBNE, "Utilitarios/Sistmars/default.aspx");
        }
        #endregion

        #region TesteCores
        public string TesteCores()
        {
            return RecuperarURL(URLBNE, "Utilitarios/Cores/default.asp");
        }
        #endregion

        #region OndeEstamos
        public string OndeEstamos()
        {
            return RecuperarURL(URLBNE, "onde-estamos");
        }
        #endregion

        #region FalePresidente
        public string FalePresidente()
        {
            return RecuperarURL(URLBNE, "fale-com-presidente");
        }
        #endregion

        #region Agradecimentos
        public string Agradecimentos()
        {
            return RecuperarURL(URLBNE, "agradecimentos");
        }
        #endregion

        #region Employer
        public string Employer()
        {
            return RecuperarURL("www.employer.com.br", string.Empty);
        }
        #endregion

        #region IndiqueBNE
        public string IndiqueBNE()
        {
            return RecuperarURL(URLBNE, "default.aspx?indiquebne=true");
        }
        #endregion

        #region RecuperarURL
        public string RecuperarURL(string prefix, string destino)
        {
            if (Request.Url != null)
            {
                string url = string.Format(URLPattern, Request.Url.Scheme, prefix, destino);

                return url;
            }

            return string.Empty;
        }
        #endregion

        public static string EnderecoApiPessoaFisica()
        {
            return "http://" + System.Web.HttpContext.Current.Application[Enumeradores.ApplicationKeys.EnderecoApiPessoaFisica.ToString()];
        }

    }
}