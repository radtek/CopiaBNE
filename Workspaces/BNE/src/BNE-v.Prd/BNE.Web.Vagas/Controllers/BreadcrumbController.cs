using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.UI;
using BNE.Web.Vagas.Models;
using BNE.BLL;
using BNE.BLL.Common;
using BNE.Web.Vagas.Code.Helpers.SEO;
using BNE.BLL.Custom;

namespace BNE.Web.Vagas.Controllers
{
    public class BreadcrumbController : Controller
    {
        //
        // GET: /Breadcrumb/

        public ActionResult Index()
        {
            RecuperarBreadcrumb();
            return View();
        }

        public PartialViewResult RecuperarBreadcrumb()
        {
            Breadcrumb oModel = new Breadcrumb();
            oModel.exibeBreadcrumb = true;
            StringBuilder resultado = new StringBuilder();            
            
            string dominio = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host + (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);            

            // As rotas são carregadas na classe Global.asax, pela chamada do RouteConfig.RegisterRoutes(RouteTable.Routes); (BNE.Web.Vagas / AppStart / RouteConfig.cs)
            
            if (TempData["temp_DesBreadcrumb"] == null || TempData["temp_DesBreadcrumbURL"] == null) //para não estourar erro na página caso não tenha dados
            {
                oModel.exibeBreadcrumb = false;
                return PartialView("_Breadcrumb", oModel);
            }

            string[] breadcrumbs = TempData["temp_DesBreadcrumb"].ToString().Split('|');
            string[] urlsBreadcrumbs = TempData["temp_DesBreadcrumbURL"].ToString().Split('|');           
            
            if (breadcrumbs.Length == 1) //se tiver só o diretório da home, não carregar breadcrumb
            {
                oModel.exibeBreadcrumb = false;
                return PartialView("_Breadcrumb", oModel);
            }

            //Monta os diretórios do breadcrumb
            int qtdDiretorios = breadcrumbs.Length;
            for (int i = 0; i < qtdDiretorios; i++)
            {
                if(urlsBreadcrumbs[i].Length > 1)
                    urlsBreadcrumbs[i] = urlsBreadcrumbs[i].NormalizarURL();
                
                int contador = i + 1;
                if (contador != qtdDiretorios)
                {
                    oModel.links = resultado.Append("<li itemprop='itemListElement' itemscope itemtype='http://schema.org/ListItem'><a itemprop='item' href='" + dominio + urlsBreadcrumbs[i].Trim() + "' title='" + breadcrumbs[i].Trim() + 
                        "' onclick=\"trackEvent(\'Master\', \'Click breadcrumb\',\'" + urlsBreadcrumbs[i].Trim() + "\');\"><span itemprop='name'>" + breadcrumbs[i].Trim() + "</span></a><meta itemprop='position' content='" + contador + "'></li>").ToString();
                }
                else
                {
                    oModel.nomePagina = "<li class='active' itemprop='itemListElement' itemscope itemtype='http://schema.org/ListItem'><a class='ultimoDiretorio' itemprop='item' href='" + dominio + urlsBreadcrumbs[i].Trim() + 
                        "'><span itemprop='name'>" + breadcrumbs[i].Trim() + "</span></a><meta itemprop='position' content='" + contador + "'></li>";
                }
            }
            oModel.exibeBreadcrumb = true;
            return PartialView("_Breadcrumb", oModel);
        }
    }
}