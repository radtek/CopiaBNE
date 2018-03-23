using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace BNE.BLL.MailController
{
    public sealed class SendgridController
    {

        static SendgridControllerService _instance;

        private static SendgridControllerService Instance
        {
            get
            {
                return _instance ?? (_instance = new SendgridControllerService());
            }
        }

        public static void Send(string emailRemetente, string emailDestinatario, string assunto, string mensagem, Dictionary<string, byte[]> attachments = null, List<string> tags = null, int? timeoutMs = null)
        {
            Instance.Send(emailRemetente, emailDestinatario, assunto, mensagem, attachments, tags, timeoutMs);
        }

    }

    
}
