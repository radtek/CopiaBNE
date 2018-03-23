using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;

[assembly: WebResource("Employer.Componentes.UI.Web.Content.js.AutoTabIndex.js", "text/javascript")]

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Este componente quando adicionado em tela muda o comportamento de todos os demais componentes desta
    /// biblioteca pisparando a tecla de tab quando o valor digitado atinge o tamanho máximo de campo.<br/>
    /// Esta implementação tem o objetivo de facilitar o preenchimento.
    /// </summary>
    public class AutoTabIndex : AjaxClientControlBase
    {
        HiddenField _hdnFoco = new HiddenField { ID = "hdnFoco" };

        private string _OnFocus = string.Empty;

        [ThreadStatic]
        private static AutoTabIndex _control;

        /// <inheritdoc/>
        public AutoTabIndex()
        {
            _control = this;
        }

        internal static bool Focus(string idFocus)
        {
            if (_control != null)
                _control._OnFocus = idFocus;

            return _control != null;
        }

        /// <inheritdoc/>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.Controls.Add(_hdnFoco);
        }

        /// <inheritdoc/>
        protected override void Render(HtmlTextWriter writer)
        {
            bool focoPost = this.Page.ClientScript.IsStartupScriptRegistered(
                typeof(HtmlForm), "Focus");

            string evTarg = Page.Request.Params["__EVENTTARGET"];

            if (focoPost || !string.IsNullOrEmpty(_OnFocus))
                _hdnFoco.Value = string.Empty;
            else if (!string.IsNullOrEmpty(evTarg))
                _hdnFoco.Value = evTarg;

            //string ajax = "MicrosoftAjax";

            //this.Page.ClientScript.IsStartupScriptRegistered(
            //    typeof(HtmlForm), ajax)

            base.Render(writer);
        }

        #region IScriptControl
        /// <inheritdoc/>
        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descripAutoTabIndex = new ScriptControlDescriptor("Employer.Componentes.UI.Web.AutoTabIndex", this.ClientID);

            descripAutoTabIndex.AddProperty("CampoFoco", _hdnFoco.ClientID);
            descripAutoTabIndex.AddProperty("FocoServer", _OnFocus);

            return new ScriptDescriptor[] { descripAutoTabIndex };
        }

        /// <inheritdoc/>
        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);

            base.SetScriptReferences(references);

            references.Add(
                    new ScriptReference
                    {
                        Assembly = "Employer.Componentes.UI.Web",
                        Name = "Employer.Componentes.UI.Web.Content.js.AutoTabIndex.js"
                    }
                );

            return references;
        }
        #endregion
    }
}
