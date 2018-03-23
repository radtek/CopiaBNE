using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BNE.BLL.Custom;

namespace BNE.Web.Vagas.Controllers
{
    public class BuscaVagaController : BaseController
    {
        //
        // GET: /BuscaVaga/

        public ActionResult Index()
        {
            List<BNE.Web.Vagas.Code.Helpers.SEO.SEOLink> lstLinks = new List<Code.Helpers.SEO.SEOLink>();

            foreach (var item in BLL.PesquisaVaga.RecuperarVagasPorEstado())
            {
                lstLinks.Add(new Code.Helpers.SEO.SEOLink
                {
                    Descricao = String.Format("{0} ({1} vagas)", item.Key, item.Value),
                    Title = String.Format("Vagas de emprego no estado do {0}", item.Key),
                    URL = BLL.Custom.SitemapHelper.MontarUrlBuscaDeVagasPorEstado(item.Key)
                });
            }

            ViewBag.linksEstado = lstLinks;

            List<List<BNE.Web.Vagas.Code.Helpers.SEO.SEOLink>> lstLinksArea = new List<List<Code.Helpers.SEO.SEOLink>>();
            foreach (var item in BLL.PesquisaVaga.RecuperarVagasPorArea())
            {
                lstLinksArea.Add(
                    new List<Code.Helpers.SEO.SEOLink>(){
                        new Code.Helpers.SEO.SEOLink
                        {
                            Descricao = String.Format("{0} ({1} vagas)", item.Key, item.Value),
                            Title = String.Format("Vagas de emprego na área de {0}", item.Key),
                            URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorArea(item.Key)
                        },
                        new Code.Helpers.SEO.SEOLink
                        {
                            Descricao = "Ver por função",
                            Title = String.Format("Vagas de emprego na área de {0} por função", item.Key),
                            URL = BLL.Custom.SitemapHelper.MontarUrlBuscaDeVagasPorArea(item.Key)
                        }
                    }
                );
            }

            ViewBag.linksArea = lstLinksArea;

            #region Viewbag
            ViewBag.H1 = "Busca de vagas de emprego";
            ViewBag.Title = "Busca de vagas de emprego";
            ViewBag.Description = "Busca de vagas por área e estado";
            #endregion

            return View();
        }

        public ActionResult VagasPorEstado()
        {
            if (RouteData.Values["estado"] == null)
                return HttpNotFound();

            String nomeEstado = RouteData.Values["estado"].ToString();
            BLL.Estado objEstado;
            Dictionary<string, int> facets = BLL.PesquisaVaga.RecuperarVagasPorCidadeDeEstado(nomeEstado.DesnormalizarURL(), out objEstado);

            if (objEstado == null)
                return HttpNotFound();

            List<List<BNE.Web.Vagas.Code.Helpers.SEO.SEOLink>> lstLinks = new List<List<Code.Helpers.SEO.SEOLink>>();
            foreach (var item in facets)
            {
                lstLinks.Add(
                    new List<Code.Helpers.SEO.SEOLink>(){
                        new Code.Helpers.SEO.SEOLink
                        {
                            Descricao = String.Format("{0} ({1} vagas)", item.Key, item.Value),
                            Title = String.Format("Vagas de emprego na cidade de {0}", item.Key),
                            URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorCidade(item.Key, objEstado.SiglaEstado)
                        },
                        new Code.Helpers.SEO.SEOLink
                        {
                            Descricao = String.Format("Ver por função", item.Key, item.Value),
                            Title = String.Format("Mais empregos por função na cidade de {0}", item.Key),
                            URL = BLL.Custom.SitemapHelper.MontarUrlBuscaDeVagasPorCidade(item.Key, objEstado.SiglaEstado)
                        }
                    }
                );
            }

            ViewBag.linksESublinks = lstLinks;

            #region Viewbag
            ViewBag.H1 = "Busca de vagas de emprego no estado do " + objEstado.NomeEstado;
            ViewBag.Title = "Busca de vagas de emprego no estado do " + objEstado.NomeEstado;
            ViewBag.Description = "Busca de vagas nas cidades do estado do " + objEstado.NomeEstado;
            ViewBag.NomeEstado = objEstado.NomeEstado;
            #endregion

            return View("Index");
        }

        public ActionResult VagasPorCidade()
        {
            if (RouteData.Values["cidade"] == null || RouteData.Values["siglaEstado"] == null)
                return HttpNotFound();

            String cidade = RouteData.Values["cidade"].ToString();
            String siglaEstado = RouteData.Values["siglaEstado"].ToString();
            BLL.Cidade objCidade;
            Dictionary<string, int> facets = BLL.PesquisaVaga.RecuperarVagasPorCidade(cidade.DesnormalizarURL(), siglaEstado.DesnormalizarURL(), out objCidade);

            if (objCidade == null)
                return HttpNotFound();

            List<BNE.Web.Vagas.Code.Helpers.SEO.SEOLink> lstLinks = new List<Code.Helpers.SEO.SEOLink>();
            foreach (var item in facets)
            {
                lstLinks.Add(
                        new Code.Helpers.SEO.SEOLink
                        {
                            Descricao = String.Format("{0}<br />({1} vagas)", item.Key, item.Value),
                            Title = String.Format("Vagas de emprego para {0} na cidade de {1} / {2}", item.Key, objCidade.NomeCidade, objCidade.Estado.SiglaEstado),
                            URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncaoCidade(item.Key, objCidade.NomeCidade, objCidade.Estado.SiglaEstado)
                        }
                );
            }

            ViewBag.links = lstLinks;

            #region Viewbag
            ViewBag.H1 = "Vagas de emprego para na cidade de " + objCidade.NomeCidade + " / " + objCidade.Estado.SiglaEstado;
            ViewBag.Title = "Busca de vagas de emprego na cidade de " + objCidade.NomeCidade + " / " + objCidade.Estado.SiglaEstado;
            ViewBag.Description = "Mais empregos na cidade de " + objCidade.NomeCidade + " / " + objCidade.Estado.SiglaEstado; ;
            ViewBag.NomeCidade = objCidade.NomeCidade;
            #endregion

            return View("Index");
        }

        public ActionResult VagasPorArea()
        {
            if (RouteData.Values["area"] == null)
                return HttpNotFound();

            String area = RouteData.Values["area"].ToString();
            BLL.AreaBNE objAreaBNE;
            Dictionary<string, int> facets = BLL.PesquisaVaga.RecuperarVagasAreaPorFuncao(area.DesnormalizarURL(), out objAreaBNE);

            if (objAreaBNE == null)
                return HttpNotFound();

            List<BNE.Web.Vagas.Code.Helpers.SEO.SEOLink> lstLinks = new List<Code.Helpers.SEO.SEOLink>();
            foreach (var item in facets)
            {
                lstLinks.Add(
                        new Code.Helpers.SEO.SEOLink
                        {
                            Descricao = String.Format("{0}<br />({1} vagas)", item.Key, item.Value),
                            Title = String.Format("Vagas de emprego na área de {0}", item.Key),
                            URL = BLL.Custom.SitemapHelper.MontarUrlVagasPorFuncao(item.Key)
                        }
                );
            }

            ViewBag.links = lstLinks;

            #region Viewbag
            ViewBag.H1 = "Vagas de emprego na área de " + objAreaBNE.DescricaoAreaBNE;
            ViewBag.Title = "Busca de vagas de emprego na área de " + objAreaBNE.DescricaoAreaBNE;
            ViewBag.Description = "Mais empregos na cidade na área de " + objAreaBNE.DescricaoAreaBNE;
            #endregion

            return View("Index");
        }

    }
}
