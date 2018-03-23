using System;
using System.Linq;
using System.Web.UI;
using System.Reflection;
using System.Web.UI.WebControls;
using BNE.Componentes.Interface;

namespace BNE.Componentes
{
    /// <summary>
    /// Label padrão
    /// </summary>
    public class ControlLabel : System.Web.UI.WebControls.Label
    {

        #region Construtor
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public ControlLabel()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Css do asterisco de obrigatório
        /// </summary>
        public String MarcaObrigatorioCss
        {
            get
            {
                return Convert.ToString(this.ViewState["MarcaObrigatorioCss"]);
            }
            set
            {
                this.ViewState["MarcaObrigatorioCss"] = value;
            }
        }

        public string MensagemErroFormatoSummary
        {
            get
            {
                var s = ViewState["MensagemErroFormatoSummary"] as string;
                if (string.IsNullOrEmpty(s))
                    return "O Campo {0} deve conter uma das sugestões.";
                return s;
            }
            set { ViewState["MensagemErroFormatoSummary"] = value; }
        }

        public string MensagemErroObrigatorioSummary
        {
            get
            {
                var s = ViewState["MensagemErroObrigatorioSummary"] as string;
                if (string.IsNullOrEmpty(s))
                    return "O Campo {0} é obrigatório.";
                return s;
            }
            set { ViewState["MensagemErroObrigatorioSummary"] = value; }
        }

        public string MensagemErroInvalidoSummary
        {
            get
            {
                var s = ViewState["MensagemErroInvalidoSummary"] as string;
                if (string.IsNullOrEmpty(s))
                    return "O Campo {0} deve conter uma das sugestões.";
                return s;
            }
            set { ViewState["MensagemErroInvalidoSummary"] = value; }
        }
        #endregion

        #region Methods

        #region ChecarObrigatorio
        private bool ChecarObrigatorio(Control control)
        {
            if (control == null)
                return false;

            foreach (PropertyInfo prop in control.GetType().GetProperties())
            {
                if (String.Equals("obrigatorio", prop.Name, StringComparison.OrdinalIgnoreCase))
                {
                    if (prop.GetValue(control, null) != null)
                        return Convert.ToBoolean(prop.GetValue(control, null));
                }
            }
            return false;
        }
        #endregion

        #region Render
        /// <summary>
        /// Renderização dos controles na tela
        /// </summary>
        /// <param name="writer">Stream onde o controle será renderizado</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            this.CssClass = this.CssClass;
            // Alterações feitas no evento Render não são persistidas na ViestateBag
            if (!String.IsNullOrEmpty(this.AssociatedControlID))
            {
                Control c = this.Parent.FindControl(this.AssociatedControlID);

                if (c == null)
                    return;


                if (c is WebControl)
                {
                    //Componente CNPJREceitaFederal "New Boolean Enabled". O comando (c as WebControl).Enabled pega o valor do base
                    //como o new cria uma nova propriedade somente para o nível hierárquico do componente, o Enabled vai com valor inconsistente.
                    PropertyInfo propriedade = c.GetType().GetProperty("Enabled");
                    if (propriedade != null)
                        this.Enabled = (Boolean)propriedade.GetValue(c, null);
                    else
                        this.Enabled = (c as WebControl).Enabled;

                    if (this.Enabled && c.GetType().GetProperties().FirstOrDefault(p => p.Name == "ReadOnly") != null)
                    {
                        propriedade = c.GetType().GetProperty("ReadOnly");
                        if (propriedade != null)
                            this.Enabled = !(Boolean)propriedade.GetValue(c, null);
                        else
                            this.Enabled = !(c as WebControl).Enabled;
                    }

                    PropertyInfo propriedadeVisible = c.GetType().GetProperty("Visible");
                    if (propriedadeVisible != null)
                        this.Visible = (Boolean)propriedadeVisible.GetValue(c, null);
                    else
                        this.Visible = (c as WebControl).Visible;

                    ////Insere a mensagem padrão para valor incorreto e campo obrigatório.
                    //PropertyInfo pObrigatorioSummary = c.GetType().GetProperty("MensagemErroObrigatorioSummary");
                    //PropertyInfo pInvalidoSummary = c.GetType().GetProperty("MensagemErroFormatoSummary");

                    //if (pObrigatorioSummary != null) {
                    //    var mensagem = pObrigatorioSummary.GetValue(c, null).ToString();
                    //    if (String.IsNullOrEmpty(mensagem)) {
                    //        pObrigatorioSummary.SetValue(c, this.Text + " é obrigatório(a)", null);
                    //    }
                    //}

                    //if (pInvalidoSummary != null)
                    //{
                    //    var mensagem = pInvalidoSummary.GetValue(c, null).ToString();
                    //    if (String.IsNullOrEmpty(mensagem))
                    //    {
                    //        pInvalidoSummary.SetValue(c, this.Text + " inválido(a)", null);
                    //    }
                    //}
                }
                else
                {
                    this.Visible = c.Visible;
                }

                if (c is IErrorMessage)
                {
                    IErrorMessage componente = (IErrorMessage)c;
                    if (String.IsNullOrEmpty(componente.MensagemErroFormatoSummary))
                    {
                        componente.MensagemErroFormatoSummary = string.Format(this.MensagemErroFormatoSummary, this.Text);
                    }

                    if (String.IsNullOrEmpty(componente.MensagemErroObrigatorioSummary))
                    {
                        componente.MensagemErroObrigatorioSummary = string.Format(this.MensagemErroObrigatorioSummary, this.Text);
                    }

                    if (String.IsNullOrEmpty(componente.MensagemErroInvalidoSummary))
                    {
                        componente.MensagemErroInvalidoSummary = string.Format(this.MensagemErroInvalidoSummary, this.Text);
                    }
                }


                Boolean obrigatorio = false;
                if (c is IRequiredField)
                {
                    obrigatorio = (c as IRequiredField).Obrigatorio;
                }
                else
                {
                    obrigatorio = this.ChecarObrigatorio(c);
                }

                if (obrigatorio)
                {
                    if (String.IsNullOrEmpty(this.MarcaObrigatorioCss))
                        this.Text = "<span>*</span>" + this.Text;
                    else
                        this.Text = "<span class=" + this.MarcaObrigatorioCss + ">*</span>" + this.Text;
                }
            }
            if (this.Visible)
                base.Render(writer);
        }
        #endregion

        #endregion

    }
}
