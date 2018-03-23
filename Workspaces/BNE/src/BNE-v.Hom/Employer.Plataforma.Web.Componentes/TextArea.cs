using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;

namespace Employer.Plataforma.Web.Controls
{
    /// <summary>
    /// Multiline TextBox com MaxLength
    /// </summary>
    public class TextArea: TextBox
    {
        protected override void OnPreRender(EventArgs e)
        {
            if (MaxLength > 0 && TextMode == TextBoxMode.MultiLine)
            {
                // Adicionar funções aos eventos para controlar o maxlength
                Attributes.Add("onkeypress","doKeypress(this);");

                Attributes.Add("onbeforepaste","doBeforePaste(this);");
                Attributes.Add("onpaste","doPaste(this);");

                // Adicionar a propriedade maxLength no Client-Side
                Attributes.Add("maxLength", this.MaxLength.ToString());
            }
            base.OnPreRender(e);
        }
    }
}
