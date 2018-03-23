using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using BNE.Chat.Core;
using BNE.Chat.Core.Base;

namespace BNE.Test.App.Chat
{
    public class Global : System.Web.HttpApplication
    {
        private readonly MailyEventAppNotifier _mailyEventSessionEnd;

        public Global()
        {
            _mailyEventSessionEnd = new MailyEventAppNotifier();
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            ChatService.Configure((a, b, c, d) => new EmptyChatConsumer(a),
                (a, b, c) => new BNEChatProducerBase(a, b, c), (a) => new EmptyNotificationController(a),
                _mailyEventSessionEnd, _mailyEventSessionEnd, _mailyEventSessionEnd);

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
            _mailyEventSessionEnd.RaiseBeginRequest(sender, e);
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            _mailyEventSessionEnd.RaiseSessionEnd(sender, new BNE.Chat.Core.EventModel.CurrentSessionEventArgs { Current = new HttpSessionStateWrapper(Session) });
        }

        protected void Application_End(object sender, EventArgs e)
        {
            _mailyEventSessionEnd.RaiseEndOfApp(sender, e);

            ChatService.Instance.Dispose();
        }
    }
}