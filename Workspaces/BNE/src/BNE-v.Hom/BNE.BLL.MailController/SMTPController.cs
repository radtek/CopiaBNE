using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using BNE.BLL.AsyncServices.Enumeradores;
using BNE.EL;

namespace BNE.Services.AsyncExecutor
{
    public sealed class SMTPController
    {
        public static void Send(string emailRemetente, string emailDestinatario, string assunto, string mensagem, Dictionary<string, byte[]> attachments = null, List<string> tags = null, int? timeoutMs = null)
        {
            var service = new SMTPControllerService();
            service.Send(emailRemetente, emailDestinatario, assunto, mensagem, attachments, tags, timeoutMs);
        }
    }

    
}