using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Sample.BLL
{
    public class Mail
    {

        #region [RetornarHtmlcamanha]
        /// <summary>
        /// Retorna apenas o html com as imagens
        /// </summary>
        /// <param name="html"></param>
        /// <param name="pastaCampanha"></param>
        /// <returns></returns>
        public static string RetornarHtmlcamanhaComImagens(string html, string pastaCampanha, string campanha)
        {
            var arq = 0;
            List<string> listaImg = new List<string>();
            var urlAmbiente = Helper.MontarUrlAmbiente();
            html = Helper.getImagensCampanha(html, out listaImg);


            foreach (var item in listaImg)
            {
                //Salva a imagem no diretorio.
                File.WriteAllBytes(string.Format(@"{0}\{1}.jpg", pastaCampanha, arq), Convert.FromBase64String(item));
                //cria url para colocar no e-mail
                var urlImg = string.Format(@"{0}/{1}/{2}.jpg", urlAmbiente, campanha, arq);
                html = html.Replace("{" + arq + "}", urlImg);
                arq++;
            }

            return html;
        }
        #endregion


        public static void Send(string to, string from, string subject, string message, List<KeyValuePair<string, byte[]>> arquivos)
        {
            #region [Salvar no Banco]


            #endregion
            string urlApi = "http://mailsender.bne.com.br/Mail";

            HttpWebRequest request = WebRequest.Create(urlApi) as HttpWebRequest;

            var json = string.Empty;
            string[] toList = to.Split(';');

            var Anexos = new Dictionary<string, string>();
            if (arquivos != null)
            {
                // Adiciona todos os anexos
                foreach (var item in arquivos)
                {
                    if (!item.Equals(default(KeyValuePair<string, byte[]>)))
                    {
                        Anexos.Add(item.Key, Convert.ToBase64String(item.Value));
                    }
                }
            }

            var enviar = new EmailSenderEnvio
            {
                Message = message,
                ProcessKey = "WU6AFXQA2KAF7QZK5CUL",
                To = toList,
                From = from,
                Subject = subject,
            };
            if (Anexos.Count > 0)
                enviar.Attachments = Anexos;

            json = JsonConvert.SerializeObject(enviar);
            request.Method = "POST";
            request.ContentType = "Application/json";
            request.Proxy = WebRequest.DefaultWebProxy;
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            byte[] byteArray = Encoding.UTF8.GetBytes(json);
            request.ContentLength = byteArray.Length;

            Stream dataStrem = request.GetRequestStream();
            dataStrem.Write(byteArray, 0, byteArray.Length);
            dataStrem.Close();

            WebResponse response = request.GetResponse();
            using (Stream streamre = response.GetResponseStream())
            {

                StreamReader reader = new StreamReader(streamre, Encoding.GetEncoding("iso-8859-1"));
                var simg = reader.ReadToEnd();
            }

        }

        #region [Private class]

        #region [EmailSenderEnvio]
        private class EmailSenderEnvio
        {
            public string ProcessKey { get; set; }
            public string From { get; set; }
            public string[] To { get; set; }
            public string[] CC { get; set; }
            public string[] BCC { get; set; }
            public string[] ReplyTo { get; set; }
            public string Subject { get; set; }
            public string Message { get; set; }
            public string TemplateId { get; set; }
            public Dictionary<string, string> Attachments { get; set; }
        }
        #endregion

        #endregion
    }
}