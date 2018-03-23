using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;

namespace Employer.Componentes.UI.Web.Util
{
    /// <summary>
    /// Gerenciador de help
    /// </summary>
    #pragma warning disable 1591
    public class HelpManager
    {
        #region HelpPath
        /// <summary>
        /// O caminho padrão dos arquivos de help
        /// </summary>
        public String HelpPath
        {
            get;
            private set;
        }
        #endregion


        #region Ctor
        public HelpManager(String helpPath)
        {
            if (String.IsNullOrEmpty(helpPath))
                HelpPath = "~/Ajuda";
            else
                HelpPath = helpPath;
        }
        public HelpManager(): this(String.Empty)
        {

        }

        #endregion
         
        #region GetHelp
        /// <summary>
        /// Retorna o caminho do help para a chave fornecida
        /// </summary>
        /// <param name="key">A chave de help fornecida</param>
        /// <returns>O caminho para o arquivo de help</returns>
        public String GetHelp(String key)
        {
            if (!(HttpContext.Current.CurrentHandler is Page))
                throw new InvalidOperationException("HelpManager.GetHelp deve ser chamado no contexto de uma Page");

            return (HttpContext.Current.CurrentHandler as Page)
                .ResolveUrl(String.Format("{0}/{1}.html", HelpPath, key));
        }
        /// <summary>
        /// Retorna o caminho do help para a página atual
        /// </summary>        
        /// <returns>O caminho para o arquivo de help</returns>
        public String GetHelp()
        {
            if (!(HttpContext.Current.CurrentHandler is Page))
                throw new InvalidOperationException("HelpManager.GetHelp deve ser chamado no contexto de uma Page");

            String[] url = HttpContext.Current.Request.RawUrl.Split('/');
            return GetHelp(url[url.Length - 1].Substring(0, url[url.Length - 1].IndexOf(".")));
        }
        #endregion

        #region RedirectToHelp
        /// <summary>
        /// Redireciona para a página de help para a chave fornecida
        /// </summary>
        /// <param name="key">A chave de help fornecida</param>
        public void RedirectToHelp(String key)
        {
            if (!(HttpContext.Current.CurrentHandler is Page))
                throw new InvalidOperationException("HelpManager.RedirectToHelp deve ser chamado no contexto de uma Page");

            HttpContext.Current.Response.Redirect(GetHelp(key));
        }
        /// <summary>
        /// Redireciona para a página de help da página atual
        /// </summary>
        public void RedirectToHelp()
        {
            if (!(HttpContext.Current.CurrentHandler is Page))
                throw new InvalidOperationException("HelpManager.RedirectToHelp deve ser chamado no contexto de uma Page");

            HttpContext.Current.Response.Redirect(GetHelp());
        }
        #endregion
    }
}
