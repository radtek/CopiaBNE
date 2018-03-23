using System;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Enumeradores;
using BNE.Common.Enumeradores;
using BNE.Common.Session;
using BNE.Web.Vagas.Models;
using Estatistica = BNE.Web.Vagas.Models.Estatistica;
using System.Collections.Generic;
using System.Reflection;
using BNE.BLL.Common;

namespace BNE.Web.Vagas.Controllers
{
    public class BaseController : Controller
    {
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            //Realizando tratamento das descrições na rota
            List<String> lstKeysFuncao = requestContext.RouteData.Values.Select(i => i.Key).ToList();
            foreach (var item in lstKeysFuncao)
                requestContext.RouteData.Values[item] = requestContext.RouteData.Values[item].ToString().Replace('-', ' ');

            base.Initialize(requestContext);
        }

        #region Cache

        #region Estatistica - Cache
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected BLL.Estatistica Estatistica
        {
            get
            {
                if (HttpRuntime.Cache["Estatistica"] == null)
                    Estatistica = BLL.Estatistica.RecuperarEstatistica();
                return (BLL.Estatistica)HttpRuntime.Cache["Estatistica"];
            }
            set
            {
                if (value != null)
                {
                    DateTime dataAtualizar = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 05, 00).AddDays(1);
                    HttpRuntime.Cache.Insert("Estatistica", value, null, dataAtualizar, Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                }
            }
        }
        #endregion

        #endregion

        #region Session

        public SessionVariable<int> IdCurriculo = new SessionVariable<int>(Chave.Permanente.IdCurriculo.ToString());
        public SessionVariable<int> IdPessoaFisicaLogada = new SessionVariable<int>(Chave.Permanente.IdPessoaFisicaLogada.ToString());
        public SessionVariable<int> IdVaga = new SessionVariable<int>(Chave.Permanente.IdVaga.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoCandidato = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoCandidato.ToString());
        public SessionVariable<int> IdUsuarioFilialPerfilLogadoEmpresa = new SessionVariable<int>(Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString());
        public SessionVariable<int> IdFilial = new SessionVariable<int>(Chave.Permanente.IdFilial.ToString());

        //public SessionVariable<string> FuncaoMaster = new SessionVariable<string>(Chave.Permanente.FuncaoMaster.ToString());
        //public SessionVariable<string> CidadeMaster = new SessionVariable<string>(Chave.Permanente.CidadeMaster.ToString());

        public SessionVariable<bool> STC = new SessionVariable<bool>(Chave.Permanente.STC.ToString());
        public SessionVariable<bool> STCUniversitario = new SessionVariable<bool>(Chave.Permanente.STCUniversitario.ToString());
        public SessionVariable<bool> STCComVIP = new SessionVariable<bool>(Chave.Permanente.STCComVIP.ToString());
        public SessionVariable<int> IdOrigem = new SessionVariable<int>(Chave.Permanente.IdOrigem.ToString());

        //Pagamento
        public SessionVariable<int> PagamentoIdentificadorPagamento = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPagamento.ToString());
        public SessionVariable<int> PagamentoIdentificadorPlano = new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPlano.ToString());

        #region Adicional

        #region PagamentoAdicionalValorTotal
        /// <summary>
        /// Utilizado quando não é a compra de um plano, mas de um adicional de plano.
        /// </summary>
        public SessionVariable<decimal> PagamentoAdicionalValorTotal = new SessionVariable<decimal>(Chave.Permanente.PagamentoAdicionalValorTotal.ToString());
        #endregion

        #region PagamentoAdicionalQuantidade
        /// <summary>
        /// Utilizado quando não é a compra de um plano, mas de um adicional de plano.
        /// </summary>
        public SessionVariable<int> PagamentoAdicionalQuantidade = new SessionVariable<int>(Chave.Permanente.PagamentoAdicionalQuantidade.ToString());
        #endregion

        #endregion

        public SessionVariable<string> PagamentoUrlRetorno = new SessionVariable<string>(Chave.Permanente.PagamentoUrlRetorno.ToString());
        public SessionVariable<int> PagamentoIdCodigoDesconto = new SessionVariable<int>(Chave.Permanente.PagamentoIdCodigoDesconto.ToString());

        #region LimparSession
        /// <summary>
        /// Método responsável por limpar os valores necessarios na identificação de um usuário.
        /// </summary>
        [Obsolete("Utilizar BNEAutenticacao")]
        public void LimparSession()
        {
            IdPessoaFisicaLogada.Clear();
            IdCurriculo.Clear();
            IdFilial.Clear();
            IdUsuarioFilialPerfilLogadoCandidato.Clear();
            IdUsuarioFilialPerfilLogadoEmpresa.Clear();

            //FuncaoMaster.Clear();
            //CidadeMaster.Clear();
        }
        #endregion

        #region LimparSessionPagamento
        /// <summary>
        /// Limpa as propriedades relacionado a compra de planos armazenadas na sessão do usuário
        /// </summary>
        public void LimparSessionPagamento()
        {
            PagamentoIdentificadorPagamento.Clear();
            PagamentoIdentificadorPlano.Clear();
            PagamentoUrlRetorno.Clear();
            PagamentoIdCodigoDesconto.Clear();
            PagamentoAdicionalValorTotal.Clear();
            PagamentoAdicionalQuantidade.Clear();
        }
        #endregion

        #region SessionDefault
        /// <summary>
        /// Método responsável por limpar os valores necessarios na identificação de um usuário.
        /// </summary>
        public void SessionDefault()
        {
            STC.Value = false;
            STCUniversitario.Value = false;
            STCComVIP.Value = false;
            IdOrigem.Value = 1; //BNE
        }
        #endregion

        #endregion

        #region RecuperarRodape
        public PartialViewResult RecuperarRodape()
        {
            return PartialView("_Rodape", new Rodape { STC = STC.ValueOrDefault });
        }
        #endregion

        #region RecuperarAtalhosTopo
        public PartialViewResult RecuperarAtalhosTopo()
        {
            return PartialView("_AtalhoTopo", new AtalhoTopo { UsuarioCandidatoLogado = IdUsuarioFilialPerfilLogadoCandidato.HasValue });
        }
        #endregion

        #region RecuperarEstatistica
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public PartialViewResult RecuperarEstatistica()
        {
            long totalCurriculo = Estatistica.QuantidadeCurriculo;
            long totalParametroContador = Convert.ToInt64(BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ContadorCurriculo));
            long total = totalCurriculo + totalParametroContador;

            return PartialView("_Estatistica", new Estatistica { QuantidadeCurriculo = total, QuantidadeEmpresa = Estatistica.QuantidadeEmpresa, QuantidadeVaga = Estatistica.QuantidadeVaga.Value });
        }
        #endregion

        #region RecuperarParametros
        public PartialViewResult RecuperarParametros()
        {
            return PartialView("_Parametros", new Parametros());
        }
        #endregion

        #region TesteDasCores
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult TesteDasCores()
        {
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/Utilitarios/Cores/default.asp");
            return RedirectPermanent(url);
        }
        #endregion

        #region TesteSistmars
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult TesteSistmars()
        {
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/Utilitarios/Sistmars/default.aspx");
            return RedirectPermanent(url);
        }
        #endregion

        #region OndeEstamos
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult OndeEstamos()
        {
            var urlDestino = Rota.RecuperarURLRota(RouteCollection.OndeEstamos);
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            return RedirectPermanent(url);
        }
        #endregion

        #region FaleComPresidente
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult FaleComPresidente()
        {
            var urlDestino = Rota.RecuperarURLRota(RouteCollection.FalePresidente);
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            return RedirectPermanent(url);
        }
        #endregion

        #region Agradecimentos
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult Agradecimentos()
        {
            var urlDestino = Rota.RecuperarURLRota(RouteCollection.Agradecimentos);
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            return RedirectPermanent(url);
        }
        #endregion

        #region CompreCVs
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult CompreCVs()
        {
            var urlDestino = Rota.RecuperarURLRota(RouteCollection.ProdutoCIA);
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            return RedirectPermanent(url);
        }
        #endregion

        #region CompreVip
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult CompreVip()
        {
            var urlDestino = Rota.RecuperarURLRota(RouteCollection.ProdutoVIP);
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            return RedirectPermanent(url);
        }
        #endregion

        #region RecrutamentoR1
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult RecrutamentoR1()
        {
            var urlDestino = Rota.RecuperarURLRota(RouteCollection.ApresentarR1);
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            return RedirectPermanent(url);
        }
        #endregion

        #region IndiqueBNE
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult IndiqueBNE()
        {
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/Default.aspx?indiquebne=true");
            return RedirectPermanent(url);
        }
        #endregion

        #region RetornarBNE
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult RetornarBNE()
        {
            Auth.BNEAutenticacao.DeslogarPadrao();
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente());
            return Redirect(url);
        }
        #endregion

        #region PesquisaVaga
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult PesquisaVaga()
        {
            var urlDestino = Rota.RecuperarURLRota(RouteCollection.PesquisaVagaAvancada);
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            return RedirectPermanent(url);
        }
        #endregion

        #region QuemMeViu
        public ActionResult QuemMeViu()
        {
            string url;

            if (IdUsuarioFilialPerfilLogadoCandidato.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)BLL.Enumeradores.TipoPerfil.Candidato))
            {
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.QuemMeViuVip);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }
            else
            {
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.LoginComercialCandidato);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }

            return Redirect(url);
        }
        #endregion

        #region AtualizarCurriculo
        public ActionResult AtualizarCurriculo()
        {
            string url;

            if ((IdUsuarioFilialPerfilLogadoCandidato.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)BLL.Enumeradores.TipoPerfil.Candidato)) || STC.ValueOrDefault)
            {
                //TODO: Setar session
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.CadastroCurriculoMini);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }
            else
            {
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.LoginComercialCandidato);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }

            return Redirect(url);
        }
        #endregion

        #region CadastrarCurriculo
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult CadastrarCurriculo()
        {
            var urlDestino = Rota.RecuperarURLRota(RouteCollection.CadastroCurriculoMini);
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            return RedirectPermanent(url);
        }
        #endregion

        #region SalaVip
        public ActionResult SalaVip()
        {
            string url;

            if (IdUsuarioFilialPerfilLogadoCandidato.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoCandidato.Value, (int)BLL.Enumeradores.TipoPerfil.Candidato))
            {
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.SalaVIP);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }
            else
            {
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.LoginComercialCandidato);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }

            return Redirect(url);
        }
        #endregion

        #region AtualizarEmpresa
        public ActionResult AtualizarEmpresa()
        {
            string url;

            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)BLL.Enumeradores.TipoPerfil.Empresa))
            {
                //TODO: Setar session
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.AtualizarDadosEmpresa);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }
            else
            {
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.LoginComercialEmpresa);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }

            return Redirect(url);
        }
        #endregion

        #region CadastrarEmpresa
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult CadastrarEmpresa()
        {
            var urlDestino = Rota.RecuperarURLRota(RouteCollection.AtualizarDadosEmpresa);
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            return RedirectPermanent(url);
        }
        #endregion

        #region SalaSelecionador
        public ActionResult SalaSelecionador()
        {
            string url;

            if (IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)BLL.Enumeradores.TipoPerfil.Empresa))
            {
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.SalaSelecionador);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }
            else
            {
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.LoginComercialEmpresa);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }

            return Redirect(url);
        }
        #endregion

        #region PesquisaCurriculo
        public ActionResult PesquisaCurriculo()
        {
            string url;

            if (IdPessoaFisicaLogada.HasValue)
            {
                var urlDestino = Rota.RecuperarURLRota(RouteCollection.PesquisaCurriculoAvancada);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }
            else
            {
                //TODO: Session
                //Session.Add(Chave.Temporaria.Variavel2.ToString(), "PesquisaCurriculoAvancada.aspx");

                var urlDestino = Rota.RecuperarURLRota(RouteCollection.LoginComercialEmpresa);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }

            return Redirect(url);
        }
        #endregion

        #region CvsRecebidos
        public ActionResult CvsRecebidos()
        {
            string url = string.Empty;

            if (IdPessoaFisicaLogada.HasValue && IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)BLL.Enumeradores.TipoPerfil.Empresa))
            {
                if (IdFilial.HasValue)
                {
                    if (!new Filial(IdFilial.Value).EmpresaBloqueada())
                    {
                        //TODO: Session
                        //Session.Add(Chave.Temporaria.Variavel13.ToString(), true);
                        var urlDestino = Rota.RecuperarURLRota(RouteCollection.VagasAnunciadas);
                        url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino );
                    }
                }
            }
            else
            {
                //TODO: Session
                //Session.Add(Chave.Temporaria.Variavel2.ToString(), "AnunciarVaga.aspx");

                var urlDestino = Rota.RecuperarURLRota(RouteCollection.LoginComercialEmpresa);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }

            return Redirect(url);
        }
        #endregion

        #region AnunciarVaga
        public ActionResult AnunciarVaga()
        {
            string url = string.Empty;

            if (IdPessoaFisicaLogada.HasValue && IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)BLL.Enumeradores.TipoPerfil.Empresa))
            {
                if (IdFilial.HasValue)
                {
                    if (!new Filial(IdFilial.Value).EmpresaBloqueada())
                    {
                        //TODO: Session
                        //UrlDestino.Value = "Default.aspx";

                        var urlDestino = Rota.RecuperarURLRota(RouteCollection.AnunciarVaga);
                        url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
                    }
                }
            }
            else
            {
                //TODO: Session
                //Session.Add(Chave.Temporaria.Variavel2.ToString(), "AnunciarVaga.aspx");

                var urlDestino = Rota.RecuperarURLRota(RouteCollection.LoginComercialEmpresa);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }

            return Redirect(url);
        }
        #endregion

        #region ExclusivoBancoCurriculo
        public ActionResult ExclusivoBancoCurriculo()
        {
            string url = string.Empty;

            if (IdPessoaFisicaLogada.HasValue && IdUsuarioFilialPerfilLogadoEmpresa.HasValue && UsuarioFilialPerfil.ValidarTipoPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, (int)BLL.Enumeradores.TipoPerfil.Empresa))
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/SiteTrabalheConoscoCriacao.aspx");
            else
            {
                //TODO: Session
                //Session.Add(Chave.Temporaria.Variavel2.ToString(), "SiteTrabalheConoscoCriacao.aspx");

                var urlDestino = Rota.RecuperarURLRota(RouteCollection.LoginComercialEmpresa);
                url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/", urlDestino);
            }

            return Redirect(url);
        }
        #endregion

        #region PesquisaCurriculoMasterPage
        public ActionResult PesquisaCurriculoMasterPage()
        {
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/Default.aspx?pesquisa=curriculo");
            return RedirectPermanent(url);
        }
        #endregion

        #region LogoEmpresa
        [OutputCache(Duration = 86400, VaryByParam = "cnpj")]
        public PartialViewResult LogoEmpresa(decimal cnpj)
        {

            var urlSite = string.Concat("http://", BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.URLAmbiente));
            var url = String.Format("{0}/Handlers/PessoaJuridicaLogo.ashx?cnpj={1}&Origem=Local", urlSite, cnpj);

            return PartialView("_LogoEmpresa", new { URL = url }.ToExpando());
        }
        #endregion

        #region Logar
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public ActionResult Logar()
        {
            return RedirectToAction("PesquisarVagas", "ResultadoPesquisaVaga");
        }
        #endregion

        #region Entrar
        [OutputCache(Duration = 86400, VaryByParam = "none")]
        public RedirectResult Entrar(string redirectURL)
        {
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/login.aspx?LogarVagas=true&RedirectURL=", redirectURL, "&ReferrerURL=", Request.UrlReferrer);
            return Redirect(url);
        }
        #endregion

        #region Redirecionar novo BNE
        public RedirectResult RedirecionarNovoBNE(string redirectURL)
        {
            return Redirect(redirectURL);
        }
        #endregion

        #region Sair
        public RedirectResult Sair()
        {
            Auth.BNEAutenticacao.DeslogarPadrao();
            var url = string.Concat("http://", Helper.RecuperarURLAmbiente(), "/login.aspx?Deslogar=true");
            return Redirect(url);
        }
        #endregion

        #region RedirectToAction

        /// <summary>
        /// Redirect to a specified action using the action name.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeParameters">The parameters for a route.</param>
        public RedirectToRouteResult RedirectToAction(string actionName, string controllerName, object routeParameters)
        {
            if (routeParameters == null)
                return RedirectToAction(actionName, controllerName);

            return base.RedirectToAction(actionName, controllerName, NormalizeRouteParameters(routeParameters));
        }

        /// <summary>
        /// Redirect to a specified action using the action name.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        public RedirectToRouteResult RedirectToAction(string actionName, string controllerName)
        {
            return base.RedirectToAction(actionName, controllerName);
        }

        /// <summary>
        /// Redirect to a specified action using the action name.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeParameters">The parameters for a route.</param>
        public RedirectToRouteResult RedirectToAction(string actionName, object routeParameters)
        {
            if (routeParameters == null)
                return RedirectToAction(actionName);

            return base.RedirectToAction(actionName, NormalizeRouteParameters(routeParameters));
        }

        /// <summary>
        /// Redirect to a specified action using the action name.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="routeParameters">The parameters for a route.</param>
        public RedirectToRouteResult RedirectToAction(string actionName, string controllerName, System.Web.Routing.RouteValueDictionary routeParameters)
        {
            if (routeParameters == null)
                return RedirectToAction(actionName, controllerName);

            return base.RedirectToAction(actionName, controllerName, NormalizeRouteParameters(routeParameters));
        }

        /// <summary>
        /// Redirect to a specified action using the action name.
        /// </summary>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="routeParameters">The parameters for a route.</param>
        public RedirectToRouteResult RedirectToAction(string actionName, System.Web.Routing.RouteValueDictionary routeParameters)
        {
            if (routeParameters == null)
                return RedirectToAction(actionName);

            return base.RedirectToAction(actionName, NormalizeRouteParameters(routeParameters));
        }

        /// <summary>
        /// Normalize the parameters to construct the URL correctly.
        /// </summary>
        /// <param name="routeParameters">The parameters for a route.</param>
        public static System.Web.Routing.RouteValueDictionary NormalizeRouteParameters(object routeParameters)
        {
            if (routeParameters == null)
                return null;

            System.Web.Routing.RouteValueDictionary r = new System.Web.Routing.RouteValueDictionary();
            PropertyInfo[] properties = routeParameters.GetType().GetProperties();
            foreach (var propertyInfo in properties)
            {
                object o = propertyInfo.GetValue(routeParameters, null);
                if (o == null)
                    continue;

                String s = o.ToString();
                s = s.NormalizarURL();
                r.Add(propertyInfo.Name, s);
            }

            return r;
        }

        /// <summary>
        /// Normalize the parameters to construct the URL correctly.
        /// </summary>
        /// <param name="routeParameters">The parameters for a route.</param>
        private static System.Web.Routing.RouteValueDictionary NormalizeRouteParameters(System.Web.Routing.RouteValueDictionary routeParameters)
        {
            if (routeParameters == null)
                return null;

            System.Web.Routing.RouteValueDictionary r = new System.Web.Routing.RouteValueDictionary();
            PropertyInfo[] properties = routeParameters.GetType().GetProperties();
            foreach (var param in routeParameters)
            {
                r.Add(param.Key, param.Value.ToString().NormalizarURL());
            }

            return r;
        }

        #endregion RedirectToAction

    }
}
