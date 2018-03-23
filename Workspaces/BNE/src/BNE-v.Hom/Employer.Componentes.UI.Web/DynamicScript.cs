using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using Employer.Componentes.UI.Web.Util;
using System.Web.UI;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente de script usado importar um Javascript informando a versão corrente do assembly para evitar cacheamento do navegador<br/>
    /// Este componente gera um parâmetro com a versão do projeto na url informada em Src.<br/>
    /// Desta forma o navegador interpreta a o arquivo como novo, toda vez que o sistema é publicado. <br/>
    /// Com esta técnica nunca será preciso limpar o cache do navegador do cliente.<br/>
    /// <note type="note">
    /// Recomendo usar em todos os arquivos js do projeto que não possuem número de versão no arquivo ou url.
    /// </note>
    /// <note type="note">
    /// Lembre sempre de deixar o AssemblyVersion na última semantica de versão como * ou trocar o número manualmente.
    /// </note>
    /// <note type="note">
    /// No processo padrão Employer de publicação a troca de versão é automática.
    /// </note>
    /// <example>
    /// <code title="Aspx" language="xml">
    /// &lt;Employer:DynamicScript runat=&quot;server&quot; Src=&quot;~/js/Framework/employer.design.js&quot; type=&quot;text/javascript&quot; /&gt;
    /// </code>
    /// </example>
    /// </summary>
    [ControlBuilder(typeof(HtmlEmptyTagControlBuilder))]
    public class DynamicScript : HtmlControl
    {
        #region Ctor
        /// <inheritdoc/>
        public DynamicScript()
            : base("script")
        { }
        #endregion 

        #region Propriedades

        #region Src
        /// <summary>
        /// O caminho para o javascript a ser importado
        /// </summary>
        [UrlProperty]
        [DefaultValue("")]
        public string Src
        {
            get { return this.Attributes["src"]; }
            set { this.Attributes["src"] = value; }
        }
        #endregion 

        #endregion 

        #region Métodos 

        #region RenderAttributes
        /// <inheritdoc />
        protected override void RenderAttributes(System.Web.UI.HtmlTextWriter writer)
        {
            string OldSrc = this.Attributes["src"];
            if (string.IsNullOrEmpty(this.Attributes["src"]) == false)
                this.Attributes["src"] = JavascriptHelper.ImportFile(this.ResolveClientUrl(this.Attributes["src"]), this.Page);

            base.RenderAttributes(writer);

            this.Attributes["src"] = OldSrc;
        }
        #endregion 

        #region Render
        /// <inheritdoc />
        protected override void Render(HtmlTextWriter writer)
        {
            writer.WriteBeginTag(this.TagName);
            this.RenderAttributes(writer);
            writer.Write(" ></script>");
        }
        #endregion

        #endregion 
    }
}
