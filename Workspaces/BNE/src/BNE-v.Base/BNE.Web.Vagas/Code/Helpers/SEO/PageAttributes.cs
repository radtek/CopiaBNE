using BNE.BLL.Enumeradores;
using BNE.BLL.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace BNE.Web.Vagas.Code.Helpers.SEO
{
    public class PageAttributes
    {
        public String Title { get; set; }
        public String Description { get; set; }
        public List<String> Keywords { get; set; }
        public String H1 { get; set; }

        public static PageAttributes Get()
        {
            return new PageAttributes()
            {
                Title = "Vagas de emprego, currículos e candidatos em Todo Brasil | BNE",
                Description = "Milhares de vagas e empregos surgindo a todo momento com diversas empresas, de recursos humanos (rh) e outros, procurando por currículos.",
                Keywords = new List<string>() { "Vagas de emprego", "emprego", "currículos", "candidatos" },
                H1 = "Vagas de emprego, currículos e candidatos em Todo Brasil"
            };
        }

        public static PageAttributes Get(RouteData routeData, object routeParameters = null)
        {
            PageAttributes retorno = Get();

            String RouteName = routeData.DataTokens["RouteName"].ToString();

            if (routeParameters == null)
                routeParameters = new { };

            BNE.BLL.Enumeradores.RouteCollection route;
            if (Enum.TryParse<BNE.BLL.Enumeradores.RouteCollection>(RouteName, out route))
            {

                switch (route)
                {
                    case BNE.BLL.Enumeradores.RouteCollection.VagasPorFuncao:
                        retorno.Title = routeParameters.ToString("Vagas de emprego para {DescricaoFuncao} | BNE");
                        retorno.Description = routeParameters.ToString("{TotalRegistros} Vagas de Emprego para {DescricaoFuncao} e muito mais empregos e vagas para você. Cadastre seu currículo grátis no BNE.");
                        retorno.Keywords = new List<string>() { routeParameters.ToString("{DescricaoFuncao}"), "Vagas de emprego", "emprego", routeParameters.ToString("Vaga de emprego para {DescricaoFuncao}") };
                        retorno.H1 = routeParameters.ToString("{TotalRegistros} Vagas de Emprego para {DescricaoFuncao}");
                        break;
                    case BNE.BLL.Enumeradores.RouteCollection.VagasPorCidade:
                        retorno.Title = routeParameters.ToString("Vagas de emprego em {NomeCidade} / {SiglaEstado} | BNE");
                        retorno.Description = routeParameters.ToString("{TotalRegistros} Vagas de Emprego em {NomeCidade} / {SiglaEstado} e muito mais empregos e vagas para você. Cadastre seu currículo grátis no BNE.");
                        retorno.Keywords = new List<string>() { routeParameters.ToString("{NomeCidade} / {SiglaEstado} "), "Vagas de emprego", "emprego", routeParameters.ToString("Vaga de emprego em {NomeCidade} / {SiglaEstado} ") };
                        retorno.H1 = routeParameters.ToString("{TotalRegistros} Vagas de Emprego em {NomeCidade} / {SiglaEstado}");
                        break;
                    case BNE.BLL.Enumeradores.RouteCollection.VagasPorFuncaoCidade:
                        retorno.Title = routeParameters.ToString("Vagas de emprego para {DescricaoFuncao} em {NomeCidade} / {SiglaEstado} | BNE");
                        retorno.Description = routeParameters.ToString("{TotalRegistros} Vagas de Emprego para {DescricaoFuncao} em {NomeCidade} / {SiglaEstado} e muito mais empregos e vagas para você. Cadastre seu currículo grátis no BNE.");
                        retorno.Keywords = new List<string>() { routeParameters.ToString("{DescricaoFuncao} em {NomeCidade} / {SiglaEstado}"), "Vagas de emprego", "emprego", routeParameters.ToString("Vaga de emprego para {DescricaoFuncao} em {NomeCidade} / {SiglaEstado}"), routeParameters.ToString("{NomeCidade} / {SiglaEstado} "), routeParameters.ToString("Vaga de emprego em {NomeCidade} / {SiglaEstado} ") };
                        retorno.H1 = routeParameters.ToString("{TotalRegistros} Vagas de Emprego para {DescricaoFuncao} em {NomeCidade} / {SiglaEstado}");
                        break;
                    case BNE.BLL.Enumeradores.RouteCollection.VagasPorEmpresa:
                        retorno.Title = routeParameters.ToString("Vagas de emprego na empresa {RazaoSocial} | BNE");
                        retorno.Description = routeParameters.ToString("{TotalRegistros} Vagas de Emprego na empresa {RazaoSocial} e muito mais empregos e vagas para você. Cadastre seu currículo grátis no BNE.");
                        retorno.Keywords = new List<string>() { routeParameters.ToString("{RazaoSocial}"), "Vagas de emprego", "emprego", routeParameters.ToString("Vaga de emprego na empresa {RazaoSocial}") };
                        retorno.H1 = routeParameters.ToString("{TotalRegistros} Vagas de Emprego na empresa {RazaoSocial}");
                        break;
                    case BNE.BLL.Enumeradores.RouteCollection.VagasPorArea:
                        retorno.Title = routeParameters.ToString("Vagas de emprego na área de {DescricaoAreaBNE} | BNE");
                        retorno.Description = routeParameters.ToString("{TotalRegistros} Vagas de Emprego na área de {DescricaoAreaBNE} e muito mais empregos e vagas para você. Cadastre seu currículo grátis no BNE.");
                        retorno.Keywords = new List<string>() { routeParameters.ToString("{DescricaoAreaBNE}"), "Vagas de emprego", "emprego", routeParameters.ToString("Vaga de emprego na área de {DescricaoAreaBNE}") };
                        retorno.H1 = routeParameters.ToString("{TotalRegistros} Vagas de Emprego na área {DescricaoAreaBNE}");
                        break;
                    case BNE.BLL.Enumeradores.RouteCollection.PesquisaVaga:
                    case BNE.BLL.Enumeradores.RouteCollection.PesquisaVagaAvancada:
                        retorno.Title = routeParameters.ToString("Pesquisa avançada de Vagas de emprego | BNE");
                        retorno.Description = routeParameters.ToString("Mais Empregos e vagas em todo o Brasil. Cadastre seu currículo grátis no BNE.");
                        retorno.Keywords = new List<string>() { routeParameters.ToString("{DescricaoAreaBNE}"), "Vagas de emprego", "emprego", routeParameters.ToString("Vaga de emprego na área de {DescricaoAreaBNE}") };
                        retorno.H1 = routeParameters.ToString("Vagas de Emprego");
                        break;
                    default:
                        break;
                }
            }
            return retorno;
        }
    }
}