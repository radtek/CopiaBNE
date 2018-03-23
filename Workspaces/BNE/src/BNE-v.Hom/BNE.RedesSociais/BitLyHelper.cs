using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace BNE.RedesSociais
{
    /// <summary>
    /// Helper para uso da API do Bit.Ly
    /// </summary>
    public static class BitLyHelper
    {
        #region MinifyUrl
        /// <summary>
        /// Reduz uma url
        /// </summary>
        /// <param name="url">A url a ser reduzida</param>
        /// <param name="loginApiBitLy">Login bit.ly</param>
        /// <param name="senhaApiBitLy">Senha bit.ly</param>
        /// <returns>Uma string vazia, ou uma url reduzida</returns>
        public static String MinifyUrl(String url, String loginApiBitLy, String senhaApiBitLy)
        {
            String queryURL = String.Format("http://api.bit.ly/shorten?version=2.0.1&longUrl={0}&login={1}&apiKey={2}", url, loginApiBitLy, senhaApiBitLy);

            HttpWebRequest request = WebRequest.Create(queryURL) as HttpWebRequest;

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                String jsonResults = reader.ReadToEnd();

                var serializer = new JavaScriptSerializer();
                var values = serializer.Deserialize<IDictionary<string, object>>(jsonResults);
                var results = values["results"] as IDictionary<string, object>;
                var retorno = results[results.ElementAt(0).Key] as IDictionary<string, object>;

                return retorno["shortUrl"].ToString();
            }
        }
        #endregion 
    }
}
