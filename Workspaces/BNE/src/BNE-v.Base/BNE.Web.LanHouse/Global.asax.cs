using System;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BNE.EL;
using BNE.Web.LanHouse.Code;
using BNE.Web.LanHouse.Models;
using FluentValidation.Mvc;
using BNE.Web.LanHouse.App_Start;

namespace BNE.Web.LanHouse
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        #region Helpers
        private static void RegisterModelValidators()
        {
            FluentValidationModelValidatorProvider.Configure();
        }

        private static void RegisterModelBinders(ModelBinderDictionary binder)
        {
            binder.Add(typeof(ModelAjaxLogarFacebook), new AliasModelBinder());
            binder.Add(typeof(ModelAjaxLogarCpf), new AliasModelBinder());
            binder.Add(typeof(ModelAjaxLogarCelular), new AliasModelBinder());

            binder.Add(typeof(ModelAjaxSextaTela), new AliasModelBinder());
            binder.Add(typeof(ModelAjaxQuintaTela), new AliasModelBinder());
            binder.Add(typeof(ModelAjaxTerceiraTela), new AliasModelBinder());
            binder.Add(typeof(ModelAjaxSegundaTela), new AliasModelBinder());

            binder.Add(typeof(ModelAjaxBuscarFilial), new AliasModelBinder());
            binder.Add(typeof(ModelHomeIndex), new AliasModelBinder());
            binder.Add(typeof(ModelAjaxCompanhia), new AliasModelBinder());
        }
        #endregion Helpers

        #region Application_Start
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AutomapperConfig.RegisterMaps();

            RegisterModelBinders(ModelBinders.Binders);
            RegisterModelValidators();
        }
        #endregion Application_Start

        protected void Application_Error(object sender, EventArgs e)
        {
            GerenciadorException.GravarExcecao(Server.GetLastError());
        }

    }
}