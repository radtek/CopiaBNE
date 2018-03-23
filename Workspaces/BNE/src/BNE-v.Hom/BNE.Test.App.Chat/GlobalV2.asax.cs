using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using BNE.Chat.Core.Base;
using BNE.Chat.Core.Interface;
using BNE.Chat.Core;

namespace BNE.Test.App.Chat
{
    public class Global : System.Web.HttpApplication
    {
        private readonly NotifySessionEnd _notifySessionEnd;
        private readonly NotifyAppBeginRequest _notifyAppBeginRequest;

        public Global()
        {
            _notifySessionEnd = new NotifySessionEnd();
            _notifyAppBeginRequest = new NotifyAppBeginRequest();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            ChatService.Configure((a, b, c) => new EmptyChatConsumer(a), (a, b) => new BNEChatProducerBase(a,b), _notifyAppBeginRequest, _notifySessionEnd);

            AreaRegistration.RegisterAllAreas();

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapHttpRoute(
                "Mensagem",
                "api/{controller}/{action}");

            RouteTable.Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Start", action = "Index", id = UrlParameter.Optional });

            RouteTable.Routes.MapRoute(
                "DefaulApitHelper",
                "Help/{action}",
                new { action = "Index" });
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            _notifyAppBeginRequest.RaiseBeginRequest(sender, e);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            _notifySessionEnd.RaiseSessionEnd(sender, e);
        }

        protected void Application_End(object sender, EventArgs e)
        {
            ChatService.Instance.Dispose();
        }
    }


}