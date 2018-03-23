using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using BNE.Auth;
using BNE.Auth.EventArgs;
using BNE.Auth.HttpModules;
using BNE.BLL;
using BNE.Bridge;
using BNE.Chat.Core;
using BNE.Chat.Core.EventModel;
using BNE.Chat.Core.Interface;
using BNE.Chat.UserControls;
using BNE.Common.Session;
using BNE.EL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Properties;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;
using System.IO;

namespace BNE.Web
{
    public class Global : HttpApplication
    {
        private static readonly MailyEventAppNotifier NotifierMainlyEvent;
        private static readonly IClientSimpleSecurity SimplifiedChatSecurity;
        private static readonly SessionAbandonRestoreMediator RestoreAbandonedSession;

        static Global()
        {
            NotifierMainlyEvent = new MailyEventAppNotifier();
            SimplifiedChatSecurity = new ChatSecuritySelecionador(); // segurança do chat
            RestoreAbandonedSession = SessionAbandonRestoreMediator.Instance; // restaura variaveis de sessão utilizadas para outras finalidades após o logout
        }

        #region Session

        #region Propriedades
        public SessionVariable<bool> STC = new SessionVariable<bool>(Chave.Permanente.STC.ToString());
        public SessionVariable<int> IdOrigem = new SessionVariable<int>(Chave.Permanente.IdOrigem.ToString());
        public SessionVariable<string> Tema = new SessionVariable<string>(Chave.Permanente.Theme.ToString());
        public SessionVariable<TipoBuscaMaster> TipoBusca = new SessionVariable<TipoBuscaMaster>(Chave.Permanente.TipoBuscaMaster.ToString());
        #endregion

        #region SessionDefault
        /// <summary>
        /// Método responsável por limpar os valores necessarios na identificação de um usuário.
        /// </summary>
        public void SessionDefault()
        {
            Tema.Value = string.Empty;
            STC.Value = false;
            IdOrigem.Value = 1; //BNE
            TipoBusca.Value = TipoBuscaMaster.Vaga;
        }
        #endregion

        #endregion

        protected void Application_Start(object sender, EventArgs e)
        {
            // Configurando Log4Net
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config");
            FileInfo finfo = new FileInfo(logFilePath);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(finfo);

            RegisterRoutes(RouteTable.Routes);
            //Necessário para controlar a quantidade de usuários logados por empresa
            PessoaFisica.ZerarDataInteracaoTodosUsuarios();

            RenderizarChat.PathDefinitionToRenderCss.Value = @"css/chat";
            RenderizarChat.PathDefinitionToRenderImg.Value = @"img/chat";
            RenderizarChat.PathDefinitionToRenderJs.Value = @"js/chat";
            RenderizarChat.PathDefinitionToTargetLink.Value = Settings.Default.BNE_WEB_CurriculoLinkHandler;
            RenderizarChat.PathDefinitionToRenderContactThumb.Value = Settings.Default.BNE_WEB_CurriculoThumbFotoHandler;

            var jsonSerializer = new JsonNetSerializer(new JsonSerializerSettings
            {
                DateParseHandling = DateParseHandling.DateTime,
                DateFormatHandling = DateFormatHandling.IsoDateFormat,
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                NullValueHandling = NullValueHandling.Ignore
            });

            GlobalHost.DependencyResolver.Register(typeof(IJsonSerializer), () => jsonSerializer);

            ChatService.Configure((a, b, c, d) => new BNEChatConsumer(a, b, c, d),
                                  (a, b, c) => new BNEChatProducer(a, b, c),
                                  a => new BNEChatNotificationController(a),
                                  NotifierMainlyEvent, NotifierMainlyEvent, NotifierMainlyEvent,
                                  SimplifiedChatSecurity);

            try
            {
                ChatService.Instance.IsValid();
            }
            catch (Exception)
            {
                throw;
            }

            AreaRegistration.RegisterAllAreas();

            // Para evitar erro no chat, manter o registro de rotas customizadas após inicialização.
            RegisterRoutesCustomizadas(RouteTable.Routes);
            
            if (HttpRuntime.UsingIntegratedPipeline)
                if (this.Modules.AllKeys.Contains("Session") && this.Modules.AllKeys.Contains("BNEAuthModule"))
                    AuthEventAggregator.Instance.NewSessionWithAutheticatedLogin += BNEAuthModule_NewSessionWithAutheticatedLogin;
                else
                    AuthEventAggregator.Instance.NewSessionWithAutheticatedLogin += BNEAuthModule_NewSessionWithAutheticatedLogin;

            AuthEventAggregator.Instance.SessionEndByTimeoutWithUserAuth += Instance_SessionEndByTimeoutWithUserAuth;
            AuthEventAggregator.Instance.ClosingManuallySessionWithUserAuth += Instance_ClosingManuallySessionWithUserAuth;

            RestoreAbandonedSession.Start();
        }

        void Instance_ClosingManuallySessionWithUserAuth(object sender, BNELogoffAuthControlEventArgs e) //logout manually 
        {
            NotifierMainlyEvent.RaiseSessionEnd(sender, new CurrentSessionEventArgs { Current = e.Session });
            RestoreAbandonedSession.RaiseSessionCloseManually(e.Session);
        }

        void Instance_SessionEndByTimeoutWithUserAuth(object sender, BNEAuthEventArgs e) //logout by timeout
        {
            NotifierMainlyEvent.RaiseSessionEnd(sender, new CurrentSessionEventArgs { Current = e.Session });
        }

        void BNEAuthModule_NewSessionWithAutheticatedLogin(object sender, BNEAuthStartSessionControlArgs e) //login by forms (persistent or normal cookie)
        {
            var result = BNESessaoLogin.PreencherDadosSessao(e.Context, e.Identity);
            if (result.Value != BNESessaoLoginResultType.OK)
                return;

            AuthEventAggregator.Instance.OnUserEnterSuccessfully(sender, new BNEAuthLoginControlEventArgs(e.Identity, e.Context) { PersistentWay = true });

            try
            {
                BNELoginProcess.SalvarNovaSessaoBanco(e, result);
                if (e.EventType == BNEAuthModule.SessionStartEventControlType.NewSessionAuthenticatedUsingRememberMeLogin)
                {
                    BNELoginProcess.RegistrarBLLProcess(e, result);
                }
            }
            catch (Exception ex)
            {
                BNEAutenticacao.DeslogarPadrao(e.Context);
                GerenciadorException.GravarExcecao(ex);
            }
        }

        protected void RegisterRoutes(RouteCollection routes)
        {
            routes.Ignore("{resource}.axd/{*pathInfo}");

            //Procedimentos para ignorar todos os arquivos que estiverem na pasta informada
            routes.Ignore("{folder}/{*pathInfo}", new { folder = "WebServices" });
            routes.Ignore("{folder}/{*pathInfo}", new { folder = "img" });
            routes.Ignore("{folder}/{*pathInfo}", new { folder = "css" });
            routes.Ignore("{folder}/{*pathInfo}", new { folder = "js" });
            routes.Ignore("{folder}/{*pathInfo}", new { folder = "Resources" });

            //Procedimentos para ignorar todos os arquivos que tiverem com a extensão informada
            routes.Ignore("{*fileExtension}", new { fileExtension = @".*\.(jpg|gif|jpeg|png|ashx|resx|js|css|htm|html|htc|caprf)$" });

            ModelBinders.Binders.Add(typeof(DateTime),
                                    new DateModelBinderPtBr(con => con.Controller != null
                                                            && typeof(EntrarCandidatoController).IsAssignableFrom(con.Controller.GetType())));


            RouteTable.Routes.MapRoute("EntrarCandidatoHashPadrao",
                             "entrar/hash",
                             new { controller = "EntrarCandidato", action = "Hash" });

            RouteTable.Routes.MapRoute("EntrarCandidatoHashParam",
                             "entrar/hash/{inHash}",
                             new { controller = "EntrarCandidato", action = "Hash" });

            RouteTable.Routes.MapRoute("EntrarCandidatoReTentar",
                               "entrar/retentar",
                               new { controller = "EntrarCandidato", action = "ReTentar" });

            RouteTable.Routes.MapRoute("EntrarCandidatoUsando",
                                    "entrar/usando",
                                    new { controller = "EntrarCandidato", action = "Usando" });

            RouteTable.Routes.MapRoute("EntrarCandidatoEmpresa",
                                       "entrar/empresa",
                                       new { Controller = "EntrarCandidato", action = "Empresa" });

            RouteTable.Routes.MapRoute("EntrarCandidatoForcar",
                                     "entrar/redirec",
                                     new { controller = "EntrarCandidato", action = "Redirec" });

            RouteTable.Routes.MapRoute("EntrarCandidatoComParam",
                                    "entrar/{inParam}",
                                    new { controller = "EntrarCandidato", action = "Com" });

            RouteTable.Routes.MapRoute("EntrarCandidatoComGenerico",
                                    "entrar/{*uri}",
                                    new { controller = "EntrarCandidato", action = "Com" });
            RouteTable.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}");

        }

        private static void RegisterRoutesCustomizadas(RouteCollection routes)
        {
            var rotas = Rota.RecuperarRotas();
            foreach (var rota in rotas)
            {
                if (rota.FlagIgnore)
                    routes.Ignore(rota.DescricaoURL);
                else
                {
                    //Efetuando replace das rotas para que o web forms trabalhe com uma rota diferenciada
                    string rotaWebForm = rota.DescricaoURL.Replace("vagas-de-emprego", "vagas").Replace("vaga-de-emprego", "vaga");
                    //routes.MapPageRoute(rota.NomeRota, rotaWebForm, rota.DescricaoCaminhoFisico); //routes.MapPageRoute anterior

                    RouteValueDictionary defaults = default(RouteValueDictionary);
                    RouteValueDictionary constraints = default(RouteValueDictionary);
                    RouteValueDictionary dataTokens = default(RouteValueDictionary);

                    if (dataTokens == null)
                        dataTokens = new RouteValueDictionary();

                    dataTokens.Add("RouteName", rota.NomeRota);
                    routes.MapPageRoute(rota.NomeRota, rotaWebForm, rota.DescricaoCaminhoFisico, true, defaults, constraints, dataTokens);
                }
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            SessionDefault();
            RestoreAbandonedSession.RaiseSessionStarted(new HttpSessionStateWrapper(Session));
            Session.Add("DateTime_Inicio", DateTime.Now);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            NotifierMainlyEvent.RaiseBeginRequest(sender, e);
            HttpApplication application = (HttpApplication)sender;
            HttpRequest request = application.Context.Request;
            request.InsertEntityBody();
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            GerenciadorException.GravarExcecao(Server.GetLastError());
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // not working with session state server
        }

        protected void Application_End(object sender, EventArgs e)
        {
            NotifierMainlyEvent.RaiseEndOfApp(sender, e);
            //ChatService.Instance.Dispose();
        }
    }
}