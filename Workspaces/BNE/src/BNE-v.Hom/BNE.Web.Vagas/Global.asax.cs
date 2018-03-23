using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BNE.Common.Enumeradores;
using BNE.Common.Session;
using BNE.Web.Vagas.App_Start;
using System.Linq;
using System;
using BNE.Auth.EventArgs;
using System.IO;

namespace BNE.Web.Vagas
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public MvcApplication()
        {
        }

        #region Session

        #region Propriedades
        public SessionVariable<bool> STC = new SessionVariable<bool>(Chave.Permanente.STC.ToString());
        public SessionVariable<bool> STCUniversitario = new SessionVariable<bool>(Chave.Permanente.STCUniversitario.ToString());
        public SessionVariable<bool> STCComVIP = new SessionVariable<bool>(Chave.Permanente.STCComVIP.ToString());
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
            STCUniversitario.Value = false;
            STCComVIP.Value = false;
            IdOrigem.Value = 1; //BNE
            TipoBusca.Value = TipoBuscaMaster.Vaga;
        }
        #endregion

        #endregion

        protected void Application_Start()
        {
            // Configurando Log4Net
            string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log4Net.config");
            FileInfo finfo = new FileInfo(logFilePath);
            log4net.Config.XmlConfigurator.ConfigureAndWatch(finfo);

            AreaRegistration.RegisterAllAreas();

            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutomapperConfig.RegisterMaps();

            //RegisterApplicationNameForSessionShare();

            if (HttpRuntime.UsingIntegratedPipeline)
            {
                if (this.Modules.AllKeys.Contains("Session"))
                {
                    if (this.Modules.AllKeys.Contains("BNEAuthSessionCheckerModule"))
                    {
                        BNE.Auth.AuthEventAggregator.Instance.NewSessionWithAutheticatedLogin += BNEAuthModule_NewSessionWithAutheticatedLogin;
                        BNE.Auth.AuthEventAggregator.Instance.UserAuthenticatedWithDifferentSession += BNEAuthModule_UserAuthenticatedWithDifferentSession;
                    }
                    else if (this.Modules.AllKeys.Contains("BNEAuthModule"))
                    {
                        BNE.Auth.AuthEventAggregator.Instance.NewSessionWithAutheticatedLogin += BNEAuthModule_NewSessionWithAutheticatedLogin;
                    }
                }
            }
            else
            {
                BNE.Auth.AuthEventAggregator.Instance.NewSessionWithAutheticatedLogin += BNEAuthModule_NewSessionWithAutheticatedLogin;
                BNE.Auth.AuthEventAggregator.Instance.UserAuthenticatedWithDifferentSession += BNEAuthModule_UserAuthenticatedWithDifferentSession;
            }

        }

        void BNEAuthModule_NewSessionWithAutheticatedLogin(object sender, BNEAuthStartSessionControlArgs e)
        {
            BNE.Bridge.BNESessaoLogin.PreencherDadosSessao(e.Context, e.Identity);
        }

        void BNEAuthModule_UserAuthenticatedWithDifferentSession(object sender, BNEAuthEventArgs e)
        {
            e.Context.Session.Abandon();
        }

        //#region RegisterApplicationNameForSessionShare
        //public void RegisterApplicationNameForSessionShare()
        //{
        //    const string applicationName = "bne.com.br";

        //    // Change the Application Name in runtime.
        //    var runtimeInfo = typeof(HttpRuntime).GetField("_theRuntime", BindingFlags.Static | BindingFlags.NonPublic);
        //    if (runtimeInfo != null)
        //    {
        //        var theRuntime = (HttpRuntime)runtimeInfo.GetValue(null);
        //        var appNameInfo = typeof(HttpRuntime).GetField("_appDomainAppId", BindingFlags.Instance | BindingFlags.NonPublic);

        //        if (appNameInfo != null) appNameInfo.SetValue(theRuntime, applicationName);
        //    }
        //}
        //#endregion

        protected void Session_Start()
        {
            SessionDefault();
        }
    }
}