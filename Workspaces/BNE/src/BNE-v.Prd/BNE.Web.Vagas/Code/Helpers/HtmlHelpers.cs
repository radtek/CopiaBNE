using BNE.BLL.Custom;
using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace BNE.Web.Vagas.Code
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
            return new MvcHtmlString(Helper.RecuperarURLAmbiente());
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
            if(nofollow)
                return new MvcHtmlString(String.Format("<a href=\"{0}\" target=\"{1}\" rel=\"nofollow\">{2}</a>", url, target, text));
            return new MvcHtmlString(String.Format("<a href=\"{0}\" target=\"{1}\">{2}</a>", url, target, text));
        }
        #endregion
    }
}