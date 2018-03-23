using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Sample.BLL
{
    public class Helper
    {

        public static string  getImagensCampanha(string html, out List<string> lista)
        {
            lista = new List<string>();
            var count = 0;
            var tipopng = "data:image/png;base64,";
            var tipojpg = "data:image/jpeg;base64,";
            var datafile = "data-filename=\"";

            while (html.Contains(tipojpg)) 
            {
                var InicioHash = html.IndexOf(tipojpg, 0) + tipojpg.Length;//inicio da hash da imagem
                var FimHash = html.IndexOf('"', InicioHash);

                lista.Add(html.Substring(InicioHash, FimHash - InicioHash));
                html = html.Replace(html.Substring(InicioHash, FimHash - InicioHash), "");
                html = html.Replace("\"data:image/jpeg;base64,\"", "\"{"+ count + "}\"");
                count++;
            }

            while (html.Contains(datafile))
            {
                var InicioHash = html.IndexOf(datafile, 0) + datafile.Length;//inicio da hash da imagem
                var FimHash = html.IndexOf('"', InicioHash)+1;
                html = html.Replace(html.Substring(InicioHash, FimHash - InicioHash), "");
                html = html.Replace(datafile, "");
            }

            while (html.Contains(tipopng))
            {
                var InicioHash = html.IndexOf(tipopng, 0) + tipopng.Length;//inicio da hash da imagem
                var FimHash = html.IndexOf('"', InicioHash);

                lista.Add(html.Substring(InicioHash, FimHash - InicioHash));
                html = html.Replace(html.Substring(InicioHash, FimHash - InicioHash), "");
                html = html.Replace("\"data:image/png;base64,\"", "\"{" + count + "}\"");
                count++;
            }
            return html;
        }

        #region [MontarUrlAmbiente]
        public static string MontarUrlAmbiente()
        {
#if DEBUG
            var url = "localhost:64117";
#else
            var url = ConfigurationManager.AppSettings["urlAmbiente"];
#endif

            if (!url.Contains("http") && !url.Contains("https"))
            {
#if DEBUG
                url = string.Concat("http://", url);
#else
                url = String.Concat("https://", url);
#endif
            }

            return url;
            #endregion


        }

        #region RetornarPrimeiroNome
        /// <param name="nomeCompleto">String nome completo</param>
        /// <returns>Primeiro nome</returns>
        public static string RetornarPrimeiroNome(string nomeCompleto)
        {
            if (string.IsNullOrWhiteSpace(nomeCompleto))
                return string.Empty;

            if (nomeCompleto.IndexOf(' ') != -1)
                return nomeCompleto.Substring(0, nomeCompleto.IndexOf(' '));

            return nomeCompleto;
        }
        #endregion
    }
}