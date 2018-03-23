using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BNE.Cdn
{
    public class CdnManager
    {
        private static CdnConfigSection _config = null;
        
        public static CdnConfigSection config {
            get{
                if (_config == null)
                    _config = System.Configuration.ConfigurationManager.GetSection("cdnConfig") as CdnConfigSection;

                if (_config == null)
                    throw new ConfigurationException("Configuração de CDN não definida.");

                return _config;
            }
        }

        /// <summary>
        /// Varre um html alterando os caminhos dos arquivos estáticos. Utilize no evento Render para tratar o HTML gerado.
        /// </summary>
        /// <param name="html">Html a ser tratado</param>
        /// <returns>Html com as urls alteradas</returns>
        public static string SetCdn(string html)
        {
            string retorno = html;

            retorno = SetImgCdn(retorno);
            retorno = SetCssCdn(retorno);
            retorno = SetJsCdn(retorno);

            return retorno;
        }

        /// <summary>
        /// Varre um html alterando os caminhos das imagens. Utilize no evento Render para tratar o HTML gerado.
        /// </summary>
        /// <param name="html">Html a ser tratado</param>
        /// <returns>Html com as urls alteradas</returns>
        public static string SetImgCdn(string html)
        {
            if (!config.Img.Enable) return html;

            return ApplyCdn("(<img.+?src=[\\\"'].+?[\\\"'].*?>)", "(?<=src=\").*(?=img/)", html, config.Img.Path);
        }

        /// <summary>
        /// Varre um html alterando os caminhos dos arquivos de estilo. Utilize no evento Render para tratar o HTML gerado.
        /// </summary>
        /// <param name="html">Html a ser tratado</param>
        /// <returns>Html com as urls alteradas</returns>
        public static string SetCssCdn(string html)
        {
            if (!config.Css.Enable) return html;

            return ApplyCdn("<link[^<]+?href=[\\\"'][^\\\"]+?\\.css[\\\"'].*?>", "(?<=href=\").*(?=css/)", html, config.Css.Path);
        }

        /// <summary>
        /// Varre um html alterando os caminhos dos arquivos javascript. Utilize no evento Render para tratar o HTML gerado.
        /// </summary>
        /// <param name="html">Html a ser tratado</param>
        /// <returns>Html com as urls alteradas</returns>
        public static string SetJsCdn(string html)
        {
            if (!config.Js.Enable) return html;

            return ApplyCdn("<script[^<]+?src=[\\\"'][^\\\"]+?\\.js[\\\"'].*?>", "(?<=src=\")[^\"']*(?=js/)", html, config.Img.Path);
        }

        /// <summary>
        /// Aplica as alterações nos caminhos dos arquivos estáticos para o CDN
        /// </summary>
        /// <param name="tagsPattern">Expressão regular para a extração das tags, presentes no HTML, a serem tratadas</param>
        /// <param name="urlPattern">Expressão regular para a identificação do caminho a ser alterado, via replace, nas tags extraídas via o parâmetro tagsPattern</param>
        /// <param name="html">HTML a ser tratado</param>
        /// <param name="path">Novo path a ser incluso</param>
        /// <returns>Html com as substituições efetuadas no caminho</returns>
        private static string ApplyCdn(String tagsPattern, String urlPattern, string html, string path)
        {
            string retorno = html;
            path = InsereBarra(path);

            //Tratando path das js
            foreach (Match match in Regex.Matches(retorno, tagsPattern, RegexOptions.IgnoreCase))
            {
                string urlAnt = Regex.Match(match.Value, urlPattern).Value;
                if (IsAnExternalUrl(urlAnt))
                    continue;

                //Get src
                string newTag = Regex.Replace(match.Value, urlPattern, path);
                retorno = retorno.Replace(match.Value, newTag);
            }

            return retorno;
        }

        /// <summary>
        /// Verifica se a url é uma url externa. Não aplica cdn se já aponta para outro servidor.
        /// </summary>
        /// <param name="url">Url a ser verificada</param>
        /// <returns>True se a url aponta para um servidor externo</returns>
        private static bool IsAnExternalUrl(String url)
        {
            return url.Contains("//");
        }

        /// <summary>
        /// Insere uma "/" no final do endereço se não houver.
        /// </summary>
        /// <param name="url">Url a ser tratada</param>
        /// <returns>Url com a "/" inserida no final</returns>
        private static String InsereBarra(String url)
        {
            if (url[url.Length - 1] != '/')
                url += "/";

            return url;
        }
    }
}
