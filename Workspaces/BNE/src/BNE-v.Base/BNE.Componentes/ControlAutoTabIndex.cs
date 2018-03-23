using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;
using System.Web.UI.HtmlControls;
using BNE.Componentes.Base;


namespace BNE.Componentes
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ControlAutoTabIndex : AjaxClientControlBase
    {
        HiddenField _hdnFoco = new HiddenField { ID = "hdnFoco" };

        private string _OnFocus = string.Empty;

        [ThreadStatic]
        private static ControlAutoTabIndex _control;

        public ControlAutoTabIndex()
        {
            _control = this;
        }

        internal static bool Focus(string idFocus)
        {
            if (_control != null)
                _control._OnFocus = idFocus;

            return _control != null;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.Controls.Add(_hdnFoco);
        }

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
        public override IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descripAutoTabIndex = new ScriptControlDescriptor("BNE.Componentes.AutoTabIndex", this.ClientID);

            descripAutoTabIndex.AddProperty("CampoFoco", _hdnFoco.ClientID);
            descripAutoTabIndex.AddProperty("FocoServer", _OnFocus);

            return new ScriptDescriptor[] { descripAutoTabIndex };
        }

        public override IEnumerable<ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);

            base.SetScriptReferences(references);

            references.Add(
                    new ScriptReference
                    {
                        Assembly = "BNE.Componentes",
                        Name = "BNE.Componentes.Content.js.AutoTabIndex.js"
                    }
                );

            return references;
        }
        #endregion
    }
}
