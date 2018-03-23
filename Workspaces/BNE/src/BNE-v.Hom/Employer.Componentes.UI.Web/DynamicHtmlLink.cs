using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.Design.WebControls;
using System.Web.UI.Design;
using System.IO;
using Employer.Componentes.UI.Web.Util;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente de link usado importar um css informando a versão corrente do assembly para evitar cacheamento do navegador<br/>
    /// Este componente gera um parâmetro com a versão do projeto na url informada em Href.<br/>
    /// Desta forma o navegador interpreta a o arquivo como novo, toda vez que o sistema é publicado. 
    /// Com esta técnica nunca será preciso limpar o cache do navegador do cliente.<br/>
    /// <note type="note">
    /// Recomendo usar em todos os arquivos css do projeto que não possuem número de versão no arquivo ou url.
    /// </note>
    /// <note type="note">
    /// Lembre sempre de deixar o AssemblyVersion na última semantica de versão como * ou trocar o número manualmente.
    /// </note>
    /// <note type="note">
    /// No processo padrão Employer de publicação a troca de versão é automática.
    /// </note>
    /// <example>
    /// <code title="Aspx" language="xml">
    /// &lt;Employer:DynamicHtmlLink runat=&quot;server&quot; href=&quot;~/css/global/geral.css&quot; rel=&quot;stylesheet&quot; type=&quot;text/css&quot; /&gt;
    /// </code>
    /// </example>
    /// </summary>
    [Designer(typeof(DynamicHtmlLinkDesigner))]
    public class DynamicHtmlLink : HtmlLink 
    {
        /// <inheritdoc/>
        [UrlProperty]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue("")]
        public override string Href
        {
            get
            {
                return JavascriptHelper.ImportFile(base.Href, this.Page);
            }
            set
            {
                base.Href = value;
            }
        }
      
        
    }

    /// <summary>
    /// Classe que define o a renderização dentro do visual studio do componente DynamicHtmlLink
    /// </summary>
    public class DynamicHtmlLinkDesigner : ControlDesigner
    {
        /// <inheritdoc/>
        public override string GetDesignTimeHtml()
        {
            StringWriter sb = new StringWriter();
            HtmlTextWriter hTxt = new HtmlTextWriter(sb);
            DynamicHtmlLink objLink = this.ViewControl as DynamicHtmlLink;

            objLink.RenderControl(hTxt);

            return sb.ToString();
        }
    }
}
