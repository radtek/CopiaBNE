using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Componentes.Util;
using Telerik.Web.UI;

namespace BNE.Componentes
{
    /// <summary>
    ///     Controle que mostra um balão de informações "Saiba Mais" quando passa o mouse por cima
    /// </summary>
    [Category("Balão saiba mais")]
    public class BalaoSaibaMais : CompositeControl , ITextControl
    {
        private RadToolTip tooltip;
        private Label lblLabel;
        private Panel pnlTooltip;


        /// <summary>
        ///     O título da tooltip
        /// </summary>
        [Category("Employer - Balão saiba mais")]
        [DisplayName("Título tooltip")]
        public string ToolTipTitle
        {
            get
            {
                if (ViewState[Keys.General.ToolTipTitle.ToString()] == null)
                {
                    ViewState[Keys.General.ToolTipTitle.ToString()] = Resources.BalaoSaibaMaisLabel;
                }
                return (string) ViewState[Keys.General.ToolTipTitle.ToString()];
            }
            set { ViewState[Keys.General.ToolTipTitle.ToString()] = value; }
        }


        /// <summary>
        ///     O texto da tooltip
        /// </summary>
        [DisplayName("Texto tooltip")]
        public string ToolTipText
        {
            get
            {
                if (ViewState[Keys.General.ToolTipText.ToString()] == null)
                {
                    ViewState[Keys.General.ToolTipText.ToString()] = string.Empty;
                }
                return (string) ViewState[Keys.General.ToolTipText.ToString()];
            }
            set { ViewState[Keys.General.ToolTipText.ToString()] = value; }
        }


        /// <summary>
        ///     A classe css da label
        /// </summary>
        public string CssClassLabel
        {
            get
            {
                if (string.IsNullOrEmpty((string) ViewState[Keys.Stylesheet.CssClassLabel.ToString()]))
                {
                    ViewState[Keys.Stylesheet.CssClassLabel.ToString()] = Resources.BalaoSaibaMaisLabelCss;
                }
                return (string) ViewState[Keys.Stylesheet.CssClassLabel.ToString()];
            }
            set { ViewState[Keys.Stylesheet.CssClassLabel.ToString()] = value; }
        }


        [DisplayName("Controle alvo")]
        public string TargetControlID
        {
            get
            {
                if (ViewState[Keys.General.TargetContolID.ToString()] == null)
                {
                    ViewState[Keys.General.TargetContolID.ToString()] = string.Empty;
                }
                return (string) ViewState[Keys.General.TargetContolID.ToString()];
            }
            set { ViewState[Keys.General.TargetContolID.ToString()] = value; }
        }

        [DisplayName("Texto label")]
        public string Text
        {
            get
            {
                if (string.IsNullOrEmpty((string) ViewState[Keys.General.Text.ToString()]))
                {
                    ViewState[Keys.General.Text.ToString()] = Resources.BalaoSaibaMaisLabel;
                }
                return (string) ViewState[Keys.General.Text.ToString()];
            }
            set { ViewState[Keys.General.Text.ToString()] = value; }
        }

        [DisplayName("Posição tooltip")]
        public ToolTipPosition ToolTipPosition { get; set; } = ToolTipPosition.TopCenter;

        /// <summary>
        ///     Cria os controles filhos
        /// </summary>
        protected override void EnsureChildControls()
        {
            if (lblLabel != null)
            {
                return;
            }

            CssClass = Resources.Tooltip;

            lblLabel = new Label
            {
                ID = ID + "_lbl",
                CssClass = Resources.TooltipLabel,
                Text = "<i class='fa fa-question-circle'></i>"
            };
            Controls.Add(lblLabel);

            pnlTooltip = new Panel
            {
                ID = ID + "_panel_tooltip",
                CssClass = Resources.TooltipText
            };
            pnlTooltip.Controls.Add(new Literal
            {
                Text = ToolTipText
            });

            Controls.Add(pnlTooltip);

            tooltip = new RadToolTip();
            tooltip.CssClass = Resources.BalaoSaibaMaisCss;
            tooltip.ID = ID + "_tooltip";
            tooltip.TargetControlID = lblLabel.ID;
            tooltip.RelativeTo = ToolTipRelativeDisplay.Element;
            tooltip.HideEvent = ToolTipHideEvent.LeaveTargetAndToolTip;
            tooltip.Animation = ToolTipAnimation.None;
            tooltip.ManualCloseButtonText = Resources.TootipFechar;
            tooltip.AutoCloseDelay = 0;
            tooltip.AnimationDuration = 0;
            tooltip.ShowDelay = 0;
            tooltip.ShowCallout = true;
            tooltip.EnableEmbeddedBaseStylesheet = true;
            tooltip.EnableEmbeddedSkins = false;
            tooltip.EnableAjaxSkinRendering = true;
            tooltip.Skin = "BNE";
            if (DesignMode)
            {
                lblLabel.Text = Resources.BalaoSaibaMaisLabel;
            }
            Controls.Add(tooltip);
        }


        /// <summary>
        ///     Evento responsável por criar os controles filhos
        /// </summary>
        /// <param name="e">Os argumentos passados para o evento</param>
        protected override void OnLoad(EventArgs e)
        {
            EnsureChildControls();

            PageResourceManager.GetCurrent(Page).RegisterStyleSheetFromResource("bsmStsht", Resources.EstiloBalaoSaibaMais);

            base.OnLoad(e);
        }


        /// <summary>
        ///     Evento invocado na fase de renderização, para setar o texto dos controles filhos
        /// </summary>
        /// <param name="e">Os argumentos passados para o evento</param>
        protected override void OnPreRender(EventArgs e)
        {
            lblLabel.Text = Text;
            tooltip.Text = ToolTipText;
            tooltip.Title = ToolTipTitle;
            tooltip.Position = ToolTipPosition;
            if (!string.IsNullOrEmpty(TargetControlID))
            {
                lblLabel.Text = string.Empty;
                tooltip.TargetControlID = TargetControlID;
                tooltip.ShowEvent = ToolTipShowEvent.OnMouseOver;
                tooltip.IsClientID = true;
                tooltip.Enabled = false;
            }
            else
            {
                tooltip.ShowEvent = ToolTipShowEvent.OnClick;
            }
            base.OnPreRender(e);
        }
    }
}