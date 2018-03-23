using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
//using Telerik.Web.UI;
using System.Reflection;

namespace Employer.Componentes.UI.Web.Util
{
    /// <summary>
    /// Classe de utilitários para javascript
    /// </summary>
    public static class JavascriptHelper
    {
        #region Constants
        /// <summary>
        /// Javascript responsável por focar um componente. Com delay de 200 ms.
        /// </summary>
        public static String JS_FOCUS_WITH_TIMEOUT = "setTimeout(function(){ $(\"[id$='{controlID}']\").focus(); $(\"[id$='{controlID}']\").select();},200);";
        /// <summary>
        /// Javascript responsável por focar um componente.
        /// </summary>
        public static String JS_FOCUS = "$(\"[id$='{controlID}']\").focus(); $(\"[id$='{controlID}']\").select();";
        //public static String JS_FOCUS_WITH_TIMEOUT = "focar(\"{controlID}\");";
        
        #endregion

        #region Methods
        #region FocusWithTimeout
        /// <summary>
        /// Registra um javascript para focar um componente com delay de 200 ms.
        /// </summary>
        /// <param name="page">A página a ser registrado</param>
        /// <param name="ctrl">O Controle que está emitindo o foco</param>
        /// <param name="controlID">O Id do controle a ser focado</param>
        /// <returns>True se conseguiu setar o foco</returns>
        public static Boolean FocusWithTimeout(Page page, Control ctrl, String controlID)
        {
            String js = JS_FOCUS_WITH_TIMEOUT.Replace("{controlID}", controlID);

            // Volta o foco para a combobox
            if (ScriptManager.GetCurrent(page) != null)
            {
                ScriptManager.
                     RegisterClientScriptBlock(ctrl,
                     page.GetType(),
                     "setfocus__",
                     js,
                     true);
                return true;
            }
            else
                if (page.ClientScript != null)
                {

                    page.ClientScript.RegisterClientScriptBlock(ctrl.GetType(), "setfocus__", js, true);
                    return true;
                }

            return false;
        }
        /// <summary>
        /// Registra um javascript para focar um componente.
        /// </summary>
        /// <param name="page">A página a ser registrado</param>
        /// <param name="ctrl">O Controle que está emitindo o foco</param>
        /// <param name="controlID">O Id do controle a ser focado</param>
        /// <returns>True se conseguiu setar o foco</returns>
        public static Boolean Focus(Page page, Control ctrl, String controlID)
        {
            String js = JS_FOCUS.Replace("{controlID}", controlID);

            // Volta o foco para a combobox
            if (ScriptManager.GetCurrent(page) != null)
            {
                ScriptManager.
                     RegisterClientScriptBlock(ctrl,
                     page.GetType(),
                     "setfocus__",
                     js,
                     true);
                return true;
            }
            else
                if (page.ClientScript != null)
                {

                    page.ClientScript.RegisterClientScriptBlock(ctrl.GetType(), "setfocus__", js, true);
                    return true;
                }

            return false;
        }
        #endregion

        #region Find
        /// <summary>
        /// Cria um javascript find
        /// </summary>
        /// <param name="controlID">O Id do controle a ser focado</param>
        /// <returns>O Javascript find</returns>
        public static String Find(String controlID)
        {
            return "$find(\"" + controlID + "\")";
        }
        #endregion

        /// <summary>
        /// Retorna o caminho do js ou css colocando a versão da compilação em um parâmetro da url para o navegador não fazer cache.
        /// </summary>
        /// <param name="sFile"></param>
        /// <param name="pagina"></param>
        /// <returns></returns>
        internal static string ImportFile(string sFile,Page pagina)
        {
            Version objVersao = GetVersion(pagina);

            string sFileImport = sFile;// this.Page.ResolveClientUrl(sFile);
            string sE = sFile.Contains("?") ? "&" : "?";
            string sVersao = string.Format("{0}_{1}_{2}_{3}",
                            objVersao.Major,
                            objVersao.Minor,
                            objVersao.Build,
                            objVersao.Revision);

            return string.Format("{0}{1}versao={2}",
                sFileImport,
                sE,
                sVersao);
        }

        /// <summary>
        /// Retorna e versão da Dll do projeto web
        /// </summary>
        /// <returns></returns>
        private static Version GetVersion(Page pagina)
        {
            Assembly ass = pagina.GetType().BaseType.Assembly;
            if (ass == null)
                ass = pagina.GetType().Assembly;
            if (ass != null)
                return ass.GetName().Version;
            return null;
        }
        #endregion

        #region Extensions
        #region Js
        /// <summary>
        /// Concatena um código Javascript
        /// </summary>
        /// <param name="script">A instância a ser extendida</param>
        /// <param name="js">O javascript a ser concatenado</param>
        /// <returns>O javascript concatenado</returns>
        public static String Js(this String script, String js)
        {
            return String.Concat(script, js);
        }
        #endregion
        #endregion
    }
}
