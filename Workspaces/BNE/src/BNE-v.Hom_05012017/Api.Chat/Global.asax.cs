using BNE.Auth;
using BNE.Chat.Core;
using BNE.Chat.Core.Interface;
using BNE.Web;
using BNE.Web.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Api.Chat
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private static readonly MailyEventAppNotifier NotifierMainlyEvent;
        private static readonly IClientSimpleSecurity SimplifiedChatSecurity;
        private static readonly SessionAbandonRestoreMediator RestoreAbandonedSession;
        private static readonly NotifyInOutOfCandidate HandleInAndOutOfCandidate;


        static WebApiApplication()
        {
            NotifierMainlyEvent = new MailyEventAppNotifier();
            //SimplifiedChatSecurity = new ChatSecuritySelecionador(); // segurança do chat
            RestoreAbandonedSession = SessionAbandonRestoreMediator.Instance; // restaura variaveis de sessão utilizadas para outras finalidades após o logout
            HandleInAndOutOfCandidate = new NotifyInOutOfCandidate();
        }

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            ChatService.Configure((a, b, c, d) => new BNEChatConsumer(a, b, c, d),
                   (a, b, c) => new BNEChatProducer(a, b, c),
                   a => new BNEChatNotificationController(a),
                   NotifierMainlyEvent, NotifierMainlyEvent, NotifierMainlyEvent,
                   SimplifiedChatSecurity);
        }
    }
}
