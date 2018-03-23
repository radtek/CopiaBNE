using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using Employer.Componentes.UI.Web.Extensions;
using System.IO;
using System.Reflection;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Controla o registro e saída de Javascript e CSS na página
    /// </summary>
    [Obsolete()]
    public class PageResourceManager : Control
    {
        #region Fields
        private Dictionary<String, String> _registeredStyleSheets; // CSS registrados
        private Dictionary<String, String> _registeredJavascripts; // Javascript registrados
        #endregion

        #region ctor        
        /// <summary>
        /// Construtor
        /// </summary>
        public PageResourceManager()
        {
            // Sem viewstate
            this.EnableViewState = false;
        }
        #endregion

        #region Propriedades

        #region registeredStyleSheets
        /// <summary>
        /// As folhas de estilo registradas, elas devem estar sem as tags de estilo
        /// </summary>
        protected Dictionary<String, String> registeredStyleSheets
        {
            get
            {
                if (this._registeredStyleSheets == null)
                    this._registeredStyleSheets = new Dictionary<string, string>();
                return this._registeredStyleSheets;
            }
        }
        #endregion

        #region registeredJavascripts
        /// <summary>
        /// Os javascript registrados, eles devem estar sem as tags de javascript
        /// </summary>
        protected Dictionary<String, String> registeredJavascripts
        {
            get
            {
                if (this._registeredJavascripts == null)
                    this._registeredJavascripts = new Dictionary<string, string>();
                return this._registeredJavascripts;
            }
        }
        #endregion

        #endregion

        #region Métodos
                
        #region Render
        /// <summary>
        /// Renderiza o css e javascript registrados no controle.
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {
            // Print css
            //if (this.registeredStyleSheets.Count > 0)
            //{
            //    writer.Write("<style type=\"text/css\">");
            //    foreach (String css in this.registeredStyleSheets.Values)
            //    {
            //        // Escreve o css otimizado
            //        writer.Write(css);
            //    }
            //    writer.Write("</style>");            
            //}

            if (this.registeredStyleSheets.Count > 0)
            {                
                foreach (String css in this.registeredStyleSheets.Values)
                {
                    writer.Write(
                        String.Format("<link rel=\"stylesheet\" type=\"text/css\" href='{0}' />", css)
                        );
                }                
            }

            // Print javascript
            if (this.registeredJavascripts.Count > 0)
            {
                writer.Write("<script type=\"text/javascript\">");
                foreach (String js in this.registeredJavascripts.Values)
                {
                    // Escreve o javascript otimizado
                    writer.Write(js.Minify());
                }
                writer.Write("</script>");
            }
        }
        #endregion

        #region RegisterStyleSheet
        /// <summary>
        /// Registra o texto de uma folha de estilo. 
        /// </summary>
        /// <param name="key">A chave pela qual essa folha de estilo será reconhecida</param>
        /// <param name="css">O texto da folha de estilo</param>
        public void RegisterStyleSheet(String key, String css)
        {
            if (this.registeredStyleSheets.Keys.Contains(key))
                this.registeredStyleSheets[key] = css;
            else
                this.registeredStyleSheets.Add(key, css);
        }
        #endregion

        #region RegisterStyleSheetFromResource
        /// <summary>
        /// Registra o texto de uma folha de estilos carregada de um arquivo de recursos
        /// </summary>
        /// <param name="key">A chave pela qual essa folha de estilo será reconhecida</param>        
        /// <param name="resourceName">O nome do recurso</param>
        public void RegisterStyleSheetFromResource(String key, String resourceName)
        {
            //TextReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName));
            //String csstext = reader.ReadToEnd();
            String csstext = this.Page.ClientScript.GetWebResourceUrl(this.GetType(), resourceName);
            this.RegisterStyleSheet(key, csstext);
        }
        #endregion

        #region RegisterJavascript
        /// <summary>
        /// Registra o texto de um código javascript
        /// </summary>
        /// <param name="key">A chave pela qual o javascript será reconhecido</param>
        /// <param name="js">O texto javascript</param>
        public void RegisterJavascript(String key, String js)
        {
            if (this.registeredJavascripts.Keys.Contains(key))
                this.registeredJavascripts[key] = js;
            else
                this.registeredJavascripts.Add(key, js);
        }
        #endregion

        #region GetCurrent
        /// <summary>
        /// Retorna o PageResourceManager registrado na página. 
        /// Caso não haja um PageResourceManager resistrado, ele adiciona um novo PageResourceManager na página. 
        /// </summary>
        /// <param name="page">A instância da página atual</param>
        /// <returns>O PageResourceManager registrado</returns>
        public static PageResourceManager GetCurrent(Page page)
        {
            if (page == null)
                throw new ArgumentNullException("page");

            // Só verifica os controles em busca do PageResourceManager caso hajam controles na página
            if (page.Header.Controls.Count > 0)
            {
                foreach (Control c in page.Header.Controls)
                {
                    if (c is PageResourceManager)
                        return (PageResourceManager)c;
                }
            }

            // Não havendo controles, ou não havendo PageResourceManager registrado na página, 
            // cria uma nova instância e a registra
            PageResourceManager res = new PageResourceManager();
            page.Header.Controls.Add(res);
            return res;
        }
        #endregion
        
        #endregion
    }
}