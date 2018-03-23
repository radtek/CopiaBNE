using System.Web.Mvc;
using System.Web.Routing;
using BNE.Web.Vagas.Code;

namespace BNE.Web.Vagas
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("cep/api");

            #region Pesquisa de vaga

            routes.MapRoute(
                name: "PesquisaVagaAvancada",
                url: "resultado-pesquisa-avancada-de-vagas/{Identificador}",
                defaults: new { controller = "ResultadoPesquisaVaga", action = "PesquisaVagaAvancada" }
            ).DataTokens["RouteName"] = "PesquisaVagaAvancada";

            routes.MapRoute(
                name: "Vaga",
                url: "vaga-de-emprego-na-area-{AreaBNE}-em-{Cidade}-{SiglaEstado}/{Funcao}/{Identificador}",
                defaults: new { controller = "Vaga", action = "VisualizarVaga" }
            ).DataTokens["RouteName"] = "Vaga";

            routes.MapRoute(
                name: "VagasPorFuncaoCidadecomPesquisa",
                url: "vagas-de-emprego-para-{Funcao}-em-{Cidade}-{SiglaEstado}/{IdPesquisa}",
                defaults: new { controller = "ResultadoPesquisaVaga", action = "PesquisarVagasFuncaoCidade" }
            ).DataTokens["RouteName"] = "VagasPorFuncaoCidadecomPesquisa";

            routes.MapRoute(
                name: "VagasPorFuncaoCidade",
                url: "vagas-de-emprego-para-{Funcao}-em-{Cidade}-{SiglaEstado}",
                defaults: new { controller = "ResultadoPesquisaVaga", action = "PesquisarVagasFuncaoCidade" }
            ).DataTokens["RouteName"] = "VagasPorFuncaoCidade";

            routes.MapRoute(
                name: "VagasPorCidade",
                url: "vagas-de-emprego-em-{Cidade}-{SiglaEstado}",
                defaults: new { controller = "ResultadoPesquisaVaga", action = "PesquisarVagasFuncaoCidade" }
            ).DataTokens["RouteName"] = "VagasPorCidade";

            routes.MapRoute(
                name: "VagasPorFuncao",
                url: "vagas-de-emprego-para-{Funcao}",
                defaults: new { controller = "ResultadoPesquisaVaga", action = "PesquisarVagasFuncaoCidade" }
            ).DataTokens["RouteName"] = "VagasPorFuncao";

            routes.MapRoute(
                name: "VagasPorArea",
                url: "vagas-de-emprego-na-area-de-{areaBNE}",
                defaults: new { controller = "ResultadoPesquisaVaga", action = "PesquisarVagasAreaBNE" }
            ).DataTokens["RouteName"] = "VagasPorArea";

            routes.MapRoute(
              name: "VagasPorPalavraChave",
              url: "vagas-de-emprego-{PalavraChave}",
              defaults: new { controller = "ResultadoPesquisaVaga", action = "PesquisarVagasPalavraChave" }
           ).DataTokens["RouteName"] = "VagasPorPalavraChave";

            routes.MapRoute(
                name: "VagasPorEmpresa",
                url: "vagas-de-emprego-na-empresa-{Empresa}/{CodigoEmpresa}",
                defaults: new { controller = "ResultadoPesquisaVaga", action = "PesquisarVagasEmpresa" }
            ).DataTokens["RouteName"] = "VagasPorEmpresa";

            routes.MapRoute(
                name: "PesquisaVaga",
                url: "vagas-de-emprego",
                defaults: new { controller = "ResultadoPesquisaVaga", action = "PesquisarVagas" }
            ).DataTokens["RouteName"] = "PesquisaVaga";


            #endregion Pesquisa de vaga

            #region Busca de vagas
            routes.MapRoute(
                name: "BuscaDeVagas",
                url: "busca-de-vagas",
                defaults: new { controller = "BuscaVaga", action = "Index" }
            ).DataTokens["RouteName"] = "BuscaDeVagas";
            #endregion Busca de vagas

            #region Mapeamento para busca de vaga - Links para landing pages

            routes.MapRoute(
                name: "BuscaDeVaga",
                url: "busca-de-vagas",
                defaults: new { controller = "BuscaVaga", action = "Index" }
            ).DataTokens["RouteName"] = "BuscaDeVaga";

            routes.MapRoute(
                name: "BuscaDeVagasPorEstado",
                url: "busca-de-vagas-no-estado-do-{estado}",
                defaults: new { controller = "BuscaVaga", action = "VagasPorEstado" }
            ).DataTokens["RouteName"] = "BuscaDeVagasPorEstado";

            routes.MapRoute(
                name: "BuscaDeVagasPorArea",
                url: "busca-de-vagas-na-area-de-{area}",
                defaults: new { controller = "BuscaVaga", action = "VagasPorArea" }
            ).DataTokens["RouteName"] = "BuscaDeVagasPorArea";

            routes.MapRoute(
                name: "BuscaDeVagasPorCidade",
                url: "busca-de-vagas-na-cidade-de-{cidade}-{siglaEstado}",
                defaults: new { controller = "BuscaVaga", action = "VagasPorCidade" }
            ).DataTokens["RouteName"] = "BuscaDeVagasPorCidade";

            #endregion

            routes.MapRoute(
                name: "Sistmars",
                url: "sistmars",
                defaults: new { controller = "Base", action = "TesteSistmars" }
            ).DataTokens["RouteName"] = "Sistmars";

            routes.MapRoute(
                name: "Cores",
                url: "testedascores",
                defaults: new { controller = "Base", action = "TesteDasCores" }
            ).DataTokens["RouteName"] = "Cores";

            routes.MapRoute(
                name: "OndeEstamos",
                url: "onde-estamos",
                defaults: new { controller = "Base", action = "OndeEstamos" }
            ).DataTokens["RouteName"] = "OndeEstamos";

            routes.MapRoute(
                name: "FaleComPresidente",
                url: "fale-com-presidente",
                defaults: new { controller = "Base", action = "FaleComPresidente" }
            ).DataTokens["RouteName"] = "FaleComPresidente";

            routes.MapRoute(
                name: "Agradecimentos",
                url: "agradecimentos",
                defaults: new { controller = "Base", action = "Agradecimentos" }
            ).DataTokens["RouteName"] = "Agradecimentos";

            routes.MapRoute(
                name: "CompreCv",
                url: "cia",
                defaults: new { controller = "Base", action = "CompreCVs" }
            ).DataTokens["RouteName"] = "CompreCv";

            routes.MapRoute(
                name: "R1",
                url: "R1",
                defaults: new { controller = "Base", action = "RecrutamentoR1" }
            ).DataTokens["RouteName"] = "R1";

            routes.MapRoute(
                name: "IndiqueBNE",
                url: "indique-bne",
                defaults: new { controller = "Base", action = "IndiqueBNE" }
            ).DataTokens["RouteName"] = "IndiqueBNE";

            routes.MapRoute(
                name: "RetornarBNE",
                url: "bne",
                defaults: new { controller = "Base", action = "RetornarBNE" }
            ).DataTokens["RouteName"] = "RetornarBNE";

            routes.MapRoute(
                name: "PesquisaVagasAvancada",
                url: "pesquisa-de-vagas",
                defaults: new { controller = "Base", action = "PesquisaVaga" }
            ).DataTokens["RouteName"] = "PesquisaVagasAvancada";

            routes.MapRoute(
                name: "QuemMeViu",
                url: "quem-me-viu",
                defaults: new { controller = "Base", action = "QuemMeViu" }
            ).DataTokens["RouteName"] = "QuemMeViu";

            routes.MapRoute(
                name: "AtualizarCurriculo",
                url: "atualizar-curriculo",
                defaults: new { controller = "Base", action = "AtualizarCurriculo" }
            ).DataTokens["RouteName"] = "AtualizarCurriculo";

            routes.MapRoute(
                name: "CadastrarCurriculo",
                url: "cadastrar-curriculo",
                defaults: new { controller = "Base", action = "CadastrarCurriculo" }
            ).DataTokens["RouteName"] = "CadastrarCurriculo";

            routes.MapRoute(
               name: "SalaVip",
               url: "sala-vip",
               defaults: new { controller = "Base", action = "SalaVip" }
            ).DataTokens["RouteName"] = "SalaVip";

            routes.MapRoute(
               name: "CompreVip",
               url: "vip",
               defaults: new { controller = "Base", action = "CompreVip" }
            ).DataTokens["RouteName"] = "CompreVip";

            routes.MapRoute(
                name: "AtualizarEmpresa",
                url: "atualizar-empresa",
                defaults: new { controller = "Base", action = "AtualizarEmpresa" }
            ).DataTokens["RouteName"] = "AtualizarEmpresa";

            routes.MapRoute(
                name: "CadastrarEmpresa",
                url: "cadastrar-empresa",
                defaults: new { controller = "Base", action = "CadastrarEmpresa" }
            ).DataTokens["RouteName"] = "CadastrarEmpresa";

            routes.MapRoute(
                name: "SalaSelecionador",
                url: "sala-selecionadora",
                defaults: new { controller = "Base", action = "SalaSelecionador" }
            ).DataTokens["RouteName"] = "SalaSelecionador";

            routes.MapRoute(
                name: "PesquisarCv",
                url: "pesquisar-cv",
                defaults: new { controller = "Base", action = "PesquisaCurriculo" }
            ).DataTokens["RouteName"] = "PesquisarCv";

            routes.MapRoute(
                name: "CvsRecebidos",
                url: "cvs-recebidos",
                defaults: new { controller = "Base", action = "CvsRecebidos" }
            ).DataTokens["RouteName"] = "CvsRecebidos";

            routes.MapRoute(
                name: "AnunciarVagas",
                url: "anunciar-vagas",
                defaults: new { controller = "Base", action = "AnunciarVaga" }
            ).DataTokens["RouteName"] = "AnunciarVagas";

            routes.MapRoute(
                name: "STC",
                url: "exclusivo-banco-curriculos",
                defaults: new { controller = "Base", action = "ExclusivoBancoCurriculo" }
            ).DataTokens["RouteName"] = "STC";

            routes.MapRoute(
               name: "Logar",
               url: "logar",
               defaults: new { controller = "Base", action = "Logar" }
           ).DataTokens["RouteName"] = "Logar";

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            ).DataTokens["RouteName"] = "Default";
        }
    }
}