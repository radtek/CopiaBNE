using System.Collections.Generic;

namespace BNE.JornalVagas
{
    public class MailMessage
    {
        public MailMessage(string key, string email, string subject, string message)
        {
            Email = email;
            Subject = subject;
            Message = message;
            Key = key;
        }

        protected MailMessage() //Newtonsoft
        {
        }

        public string Key { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
    }

    public class WebpushMessage
    {
        public WebpushMessage(string token)
        {
            Token = token;
        }

        protected WebpushMessage() //Newtonsoft
        {
        }

        public string Token { get; set; }
    }

    public class Message
    {
        public int Curriculo { get; set; }

        public Message(int curriculo, MailMessage mail)
        {
            Mail = mail;
            Curriculo = curriculo;
        }

        public Message(int curriculo, List<WebpushMessage> webpush)
        {
            Webpush = webpush;
            Curriculo = curriculo;
        }

        public Message(int curriculo, MailMessage mail, List<WebpushMessage> webpush)
        {
            Mail = mail;
            Webpush = webpush;
            Curriculo = curriculo;
        }

        protected Message() //Newtonsoft
        {
        }

        public MailMessage Mail { get; set; }
        public List<WebpushMessage> Webpush { get; set; }
    }
}