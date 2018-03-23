using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace BNE.BLL.Integracoes.Google
{
    public static class EncurtadorDeURL
    {
        public class GoogleShortenerResponse
        {
            public string kind { get; set; }
            public string id { get; set; }
            public string longUrl { get; set; }
        }

        public class EncurtadorDeURLException : Exception 
        {
            public EncurtadorDeURLException() { }
            public EncurtadorDeURLException(string message) : base(message) { }
            public EncurtadorDeURLException(string message, Exception inner) : base(message, inner) { }
        }

        /// <summary>Encurta uma url utilizando a API do Google. 
        /// <para>URL longa que será encurtada</para>
        /// <exception cref="BNE.BLL.Integracoes.Google.EncurtadorDeURLException">Lançada quando não é possível realizar o encurtamento.</exception>
        /// </summary> 
        public static string Encurtar(string url)
        {
            string str = "";

            WebRequest request = WebRequest.Create(string.Format("https://www.googleapis.com/urlshortener/v1/url?key={0}", 
                Parametro.RecuperaValorParametro(BNE.BLL.Enumeradores.Parametro.KeyAPIGoogleURLShortener)));
            request.Method = "POST";

            request.Proxy = WebRequest.DefaultWebProxy;
            request.Proxy.Credentials = CredentialCache.DefaultCredentials;

            string postData = "{" + string.Format("\"longUrl\": \"{0}\"", url) + "}";
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            try
            {
                WebResponse response = request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();

                GoogleShortenerResponse google = new JavaScriptSerializer().Deserialize<GoogleShortenerResponse>(responseFromServer);
                str = google.id;
                reader.Close();
                response.Close();
            }
            catch(Exception ex)
            {
                throw new EncurtadorDeURLException("Não foi possível realizar o encurtamento de URL", ex);
            }
            return str;
        }

    }
}
