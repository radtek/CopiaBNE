using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BNE.PessoaFisica.Web.Helpers
{
    public static class HtmlHelpers
    {
        private static string subdir = "/vagas-de-emprego";

        #region GetApplicationSubfolder
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString RecuperarURLAmbiente(this HtmlHelper html)
        {
            return new MvcHtmlString(ConfigurationManager.AppSettings["EnderecoBNE"]);
        }
        #endregion

        #region GetApplicationSubfolder
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static MvcHtmlString GetApplicationSubfolder(this HtmlHelper html)
        {
#if DEBUG
            return new MvcHtmlString(String.Empty);
#endif
            return new MvcHtmlString(subdir);
        }
        #endregion

        #region HyperLink
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="url"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static MvcHtmlString HyperLink(this HtmlHelper html, string url, string text, Boolean nofollow = false)
        {
            if (nofollow)
                return new MvcHtmlString(String.Format("<a href=\"{0}\" rel=\"nofollow\">{1}</a>", url, text));

            return new MvcHtmlString(String.Format("<a href=\"{0}\">{1}</a>", url, text));
        }
        #endregion

        #region HyperLink
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="url"></param>
        /// <param name="text"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static MvcHtmlString HyperLink(this HtmlHelper html, string url, string text, string target, Boolean nofollow = false)
        {
            if (nofollow)
                return new MvcHtmlString(String.Format("<a href=\"{0}\" target=\"{1}\" rel=\"nofollow\">{2}</a>", url, target, text));
            return new MvcHtmlString(String.Format("<a href=\"{0}\" target=\"{1}\">{2}</a>", url, target, text));
        }
        #endregion

        #region IsMobileDevice
        public static bool IsMobileDevice()
        {
            System.Web.HttpBrowserCapabilities myBrowserCaps = HttpContext.Current.Request.Browser;
            return ((System.Web.Configuration.HttpCapabilitiesBase)myBrowserCaps).IsMobileDevice;
        }
        #endregion

    }
}