// -----------------------------------------------------------------------
// <copyright file="EmployerToolTip.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Web.UI;
[assembly: WebResource("Employer.Componentes.UI.Web.Content.js.EmployerToolTip.js", "text/javascript")]

namespace Employer.Componentes.UI.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.ComponentModel;
    using System.Web.UI;

    /// <summary>
    /// Componente que implementa ToolTip
    /// </summary>
    public class EmployerToolTip : AjaxClientControlBase
    {
        #region Atributos
        private Label lblLabel = new Label();
        //private EmployerToolTipPosition position = EmployerToolTipPosition.TopCenter;
        #endregion

        #region Propriedades

        #region ToolTipTitle
        /// <summary>
        /// O título da tooltip
        /// </summary>
        [
            Category("Employer - Balão saiba mais"),
            DisplayName("Título tooltip")
        ]
        public String ToolTipTitle
        {
            get
            {
                EnsureChildControls();
                if (this.ViewState["ToolTipTitle"] == null)
                    this.ViewState["ToolTipTitle"] = Resources.BalaoSaibaMaisLabel;
                return (String)this.ViewState["ToolTipTitle"];
            }
            set
            {
                EnsureChildControls();
                this.ViewState["ToolTipTitle"] = value;
            }
        }

        /// <summary>
        /// Classe css do titulo
        /// </summary>
        [Category("Employer - Balão saiba mais"), DisplayName("Css título tooltip")]
        public String ToolTipTitleCss
        {
            get
            {
                EnsureChildControls();
                if (this.ViewState["ToolTipTitleCss"] == null)
                    this.ViewState["ToolTipTitleCss"] = String.Empty;
                return (String)this.ViewState["ToolTipTitleCss"];
            }
            set { EnsureChildControls(); this.ViewState["ToolTipTitleCss"] = value; }
        }
        #endregion

        #region ToolTipText
        /// <summary>
        /// O texto da tooltip
        /// </summary>
        [
            Category("Employer - Balão saiba mais"),
            DisplayName("Texto tooltip")
        ]
        public String ToolTipText
        {
            get
            {
                EnsureChildControls();
                if (this.ViewState["ToolTipText"] == null)
                    this.ViewState["ToolTipText"] = String.Empty;
                return (String)this.ViewState["ToolTipText"];
            }
            set
            {
                EnsureChildControls();
                this.ViewState["ToolTipText"] = value;
            }
        }

        /// <summary>
        /// Classe css do texto
        /// </summary>
        [Category("Employer - Balão saiba mais"), DisplayName("Css texto tooltip")]
        public String ToolTipTextCss
        {
            get
            {
                EnsureChildControls();
                if (this.ViewState["ToolTipTextCss"] == null)
                    this.ViewState["ToolTipTextCss"] = String.Empty;
                return (String)this.ViewState["ToolTipTextCss"];
            }
            set { EnsureChildControls(); this.ViewState["ToolTipTextCss"] = value; }
        }
        #endregion

        #region ToolTipPosition
        /// <summary>
        /// Posição da ToolTip. 
        /// O valor padrão é: ToolTipPosition.TopCenter
        /// </summary>
        [
            Category("Employer - Balão saiba mais"),
            DisplayName("Posição tooltip")
        ]
        public EmployerToolTipPosition ToolTipPosition
        {
            get { EnsureChildControls(); return this.ViewState["ToolTipPosition"] == null ? EmployerToolTipPosition.TopCenter : (EmployerToolTipPosition)this.ViewState["ToolTipPosition"]; }
            set { EnsureChildControls(); this.ViewState["ToolTipPosition"] = value; }
        }
        #endregion

        #region CssClassLabel
        /// <summary>
        /// Classe css da label
        /// </summary>
        public string CssClassLabel
        {
            get {
                EnsureChildControls();
                return this.lblLabel.CssClass; }
            set {
                EnsureChildControls();
                this.lblLabel.CssClass = value; }
        }
        #endregion

        #region Text
        /// <summary>
        /// Texto da label
        /// </summary>
        public string Text
        {
            get {
                EnsureChildControls();
                return this.lblLabel.Text; }
            set {
                EnsureChildControls();
                this.lblLabel.Text = value; }
        }
        #endregion

        #endregion
        /// <inheritdoc/>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.lblLabel.ID = this.ID + "_lbl";
            //Obrigatório sem o componente não funciona
            this.lblLabel.Attributes["title"] = "title";
            this.Controls.Add(this.lblLabel);

            if (this.DesignMode)
            {
                this.lblLabel.Text = Resources.BalaoSaibaMaisLabel;
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.EmployerToolTip", this.lblLabel.ClientID);

            this.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("ToolTipTitle", this.ToolTipTitle);
            descriptor.AddProperty("ToolTipText", this.ToolTipText);
            descriptor.AddProperty("ToolTipPosition", this.ToolTipPosition);
            descriptor.AddProperty("ToolTipTextCss", this.ToolTipTextCss);
            descriptor.AddProperty("ToolTipTitleCss", this.ToolTipTitleCss);

            return new ScriptControlDescriptor[] { descriptor };
        }

        /// <inheritdoc/>
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.EmployerToolTip.js";
            references.Add(reference);

            return references;
        }
    }

    /// <summary>
    /// Posição do ToolTip
    /// </summary>
    public enum EmployerToolTipPosition
    {
        /// <inheritdoc/>
        TopLeft = 11,
        /// <inheritdoc/>
        TopCenter = 12,
        /// <inheritdoc/>
        TopRight = 13,
        /// <inheritdoc/>
        MiddleLeft = 21,
        /// <inheritdoc/>
        Center = 22,
        /// <inheritdoc/>
        MiddleRight = 23,
        /// <inheritdoc/>
        BottomLeft = 31,
        /// <inheritdoc/>
        BottomCenter = 32,
        /// <inheritdoc/>
        BottomRight = 33,
    }
}
