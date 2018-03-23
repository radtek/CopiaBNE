using BNE.Web.Vagas.Code.Helpers.SEO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom.Maps;
using System.Data;

namespace BNE.Web.Vagas.Controllers
{
    public class MaisVagasController : Controller
    {
        //
        // GET: /MaisVagas/

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult CarregarLinksMaisVagas(int? idFuncao, int? idCidade, string cidadeEstado = null)
        {
            List<SEOLink> model = new List<SEOLink>();
            int qtdeMaxRetorno = 0;

            string nomeCidade = string.Empty;
            string siglaEstado = string.Empty;
            if (!string.IsNullOrEmpty(cidadeEstado))
            {
                var arrayCidadeEstado = cidadeEstado.Split('/');
                nomeCidade = arrayCidadeEstado[0].Trim();
                siglaEstado = arrayCidadeEstado[1].Trim();
            }            

            if (idFuncao == null && cidadeEstado == null)
            {
                var arrayCidadeEstadoDefault = Parametro.RecuperaValorParametro(Enumeradores.Parametro.SeoMaisVagasParametrosDefault).Split('|');

                foreach (var cidadeEstadoDefault in arrayCidadeEstadoDefault)
                {
                    nomeCidade = cidadeEstadoDefault.Split('/')[0].Trim();
                    siglaEstado = cidadeEstadoDefault.Split('/')[1].Trim();

                    model.Add(LinkHelper.ObterLinkVagasCidade(nomeCidade, siglaEstado));
                }
            }
            else if (idFuncao != null && cidadeEstado != null)
            {
                qtdeMaxRetorno = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SeoMaisVagasQtdeFuncoesSimilaresPorCidade));
                DataTable dtFuncoesSimilares = FuncaoPretendida.CarregarFuncoesSimilaresPorFuncaoECidade(qtdeMaxRetorno, idFuncao, idCidade);

                foreach (DataRow funcaoSimilar in dtFuncoesSimilares.Rows)
                {
                    model.Add(LinkHelper.ObterLinkVagasFuncaoCidade(funcaoSimilar["Des_Funcao"].ToString(), nomeCidade, siglaEstado));
                }
            }
            else if (idFuncao != null)
            {
                qtdeMaxRetorno = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SeoMaisVagasQtdeFuncoesSimilares));
                DataTable dtFuncoesSimilares = FuncaoPretendida.CarregarFuncoesSimilaresPorFuncaoECidade(qtdeMaxRetorno, idFuncao);

                foreach (DataRow funcaoSimilar in dtFuncoesSimilares.Rows)
                {
                    model.Add(LinkHelper.ObterLinkVagasFuncao(funcaoSimilar["Des_Funcao"].ToString()));
                }                
            }
            else if (idCidade != null)
            {
                qtdeMaxRetorno = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SeoMaisVagasQtdeCidadesProximas));
                DataTable dtCidadesProximas = Cidade.CarregarCidadesProximasPorGeolocalizacao(qtdeMaxRetorno, idCidade);

                foreach (DataRow cidadeProxima in dtCidadesProximas.Rows)
                {
                    model.Add(LinkHelper.ObterLinkVagasCidade(cidadeProxima["Nme_Cidade"].ToString(), cidadeProxima["Sig_Estado"].ToString()));
                }
            }

            return PartialView("~/Views/ResultadoPesquisaVaga/_MaisVagas.cshtml", model);
        }
    }
}
