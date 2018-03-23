using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Componentes.Util;
using Telerik.Web.UI;

namespace BNE.Componentes
{
    /// <summary>
    /// Controle que mostra um balão de informações "Saiba Mais" quando passa o mouse por cima
    /// </summary>
    public class BalaoSaibaMais : CompositeControl//, ITextControl
    {

        #region Private
        //private RadToolTip tooltip = null;
        private Label lblLabel = null;
        private Panel pnlTooltip = null;
        //private ToolTipPosition position = ToolTipPosition.TopCenter;
        #endregion

        #region Properties

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
                if (this.ViewState[Keys.General.ToolTipTitle.ToString()] == null)
                    this.ViewState[Keys.General.ToolTipTitle.ToString()] = Resources.BalaoSaibaMaisLabel;
                return (String)this.ViewState[Keys.General.ToolTipTitle.ToString()];
            }
            set
            {
                this.ViewState[Keys.General.ToolTipTitle.ToString()] = value;
            }
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
                if (this.ViewState[Keys.General.ToolTipText.ToString()] == null)
                    this.ViewState[Keys.General.ToolTipText.ToString()] = String.Empty;
                return (String)this.ViewState[Keys.General.ToolTipText.ToString()];
            }
            set
            {
                this.ViewState[Keys.General.ToolTipText.ToString()] = value;
            }
        }
        #endregion

        //#region Value
        ///// <summary>
        ///// O texto da label do controle
        ///// </summary>
        //[
        //    Category("Employer - Balão saiba mais"),
        //    DisplayName("Texto label")
        //]
        //public string Text
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty((String)this.ViewState[Keys.General.Text.ToString()]))
        //            this.ViewState[Keys.General.Text.ToString()] = Resources.BalaoSaibaMaisLabel;
        //        return (String)this.ViewState[Keys.General.Text.ToString()];
        //    }
        //    set
        //    {
        //        this.ViewState[Keys.General.Text.ToString()] = value;
        //    }
        //}
        //#endregion

        //#region ShowOnMouseover
        ///// <summary>
        ///// Armazena se deve mostrar o tooltip no mouseover
        ///// </summary>
        //[
        //    Category("Employer - Balão saiba mais"),
        //    DisplayName("Controle alvo")
        //]
        //public bool ShowOnMouseover
        //{
        //    get
        //    {
        //        if (this.ViewState[Keys.General.ShowOnMouseover.ToString()] == null)
        //            this.ViewState[Keys.General.ShowOnMouseover.ToString()] = false;
        //        return Convert.ToBoolean(this.ViewState[Keys.General.ShowOnMouseover.ToString()]);
        //    }
        //    set
        //    {
        //        this.ViewState[Keys.General.ShowOnMouseover.ToString()] = value;
        //    }
        //}
        //#endregion

        //#region ToolTipPosition
        ///// <summary>
        ///// Posição da ToolTip. 
        ///// O valor padrão é: ToolTipPosition.TopCenter
        ///// </summary>
        //[
        //    Category("Employer - Balão saiba mais"),
        //    DisplayName("Posição tooltip")
        //]
        //public ToolTipPosition ToolTipPosition
        //{
        //    get { return this.position; }
        //    set { this.position = value; }
        //}
        //#endregion

        #region CssClassLabel
        /// <summary>
        /// A classe css da label
        /// </summary>
        public string CssClassLabel
        {
            get
            {
                if (String.IsNullOrEmpty((String)this.ViewState[Keys.Stylesheet.CssClassLabel.ToString()]))
                    this.ViewState[Keys.Stylesheet.CssClassLabel.ToString()] = Resources.BalaoSaibaMaisLabelCss;
                return (String)this.ViewState[Keys.Stylesheet.CssClassLabel.ToString()];
            }
            set
            {
                this.ViewState[Keys.Stylesheet.CssClassLabel.ToString()] = value;
            }
        }
        #endregion

        #endregion

        #region Methods

        #region EnsureChildControls
        /// <summary>
        /// Cria os controles filhos
        /// </summary>
        protected override void EnsureChildControls()
        {
            if (this.lblLabel != null)
                return;

            this.CssClass = Resources.Tooltip;

            this.lblLabel = new Label
            {
                ID = this.ID + "_lbl",
                CssClass = Resources.TooltipLabel,
                Text = "<i class='fa fa-question-circle'></i>"
            };
            this.Controls.Add(this.lblLabel);

            this.pnlTooltip = new Panel
            {
                ID = this.ID + "_tooltip",
                CssClass = Resources.TooltipText,
            };
            this.pnlTooltip.Controls.Add(new Literal
            {
                Text = this.ToolTipText
            });

            this.Controls.Add(this.pnlTooltip);

            //this.tooltip = new RadToolTip();

            //this.tooltip.CssClass = Resources.BalaoSaibaMaisCss;
            //this.tooltip.ID = this.ID + "_tooltip";
            //this.tooltip.TargetControlID = this.lblLabel.ID;
            //this.tooltip.RelativeTo = ToolTipRelativeDisplay.Element;
            //this.tooltip.HideEvent = ToolTipHideEvent.LeaveTargetAndToolTip;
            //this.tooltip.Animation = ToolTipAnimation.None;
            //this.tooltip.ManualCloseButtonText = Resources.TootipFechar;
            //this.tooltip.AutoCloseDelay = 0;
            //this.tooltip.AnimationDuration = 0;
            //this.tooltip.ShowDelay = 0;
            //this.tooltip.ShowCallout = true;
            //this.tooltip.EnableEmbeddedBaseStylesheet = true;
            //this.tooltip.EnableEmbeddedSkins = false;
            //this.tooltip.EnableAjaxSkinRendering = true;
            //this.tooltip.Skin = "BNE";

            if (this.DesignMode)
            {
                this.lblLabel.Text = Resources.BalaoSaibaMaisLabel;
            }

            //this.Controls.Add(this.tooltip);
        }
        #endregion

        #endregion

        #region Events

        #region OnLoad
        /// <summary>
        /// Evento responsável por criar os controles filhos
        /// </summary>
        /// <param name="e">Os argumentos passados para o evento</param>
        protected override void OnLoad(EventArgs e)
        {
            this.EnsureChildControls();

            PageResourceManager.GetCurrent(this.Page).RegisterStyleSheetFromResource("bsmStsht", Resources.EstiloBalaoSaibaMais);

            base.OnLoad(e);
        }
        #endregion

        /* #region OnPreRender
        /// <summary>
        /// Evento invocado na fase de renderização, para setar o texto dos controles filhos
        /// </summary>
        /// <param name="e">Os argumentos passados para o evento</param>
        protected override void OnPreRender(EventArgs e)
        {
            this.lblLabel.Text = this.Text;
            this.tooltip.Text = this.ToolTipText;
            this.tooltip.Title = this.ToolTipTitle;
            this.tooltip.Position = this.ToolTipPosition;

            if (ShowOnMouseover)
            {
                //this.lblLabel.Text = string.Empty;
                this.tooltip.TargetControlID = this.lblLabel.ClientID;
                this.tooltip.ShowEvent = ToolTipShowEvent.OnMouseOver;
                this.tooltip.IsClientID = true;
                this.tooltip.Enabled = false;
            }
            else
                this.tooltip.ShowEvent = ToolTipShowEvent.OnClick;

            base.OnPreRender(e);
        }
        #endregion*/

        #endregion

    }
}

