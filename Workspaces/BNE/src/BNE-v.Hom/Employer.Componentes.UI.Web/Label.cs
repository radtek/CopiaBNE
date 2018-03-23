using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Employer.Componentes.UI.Web.Util;
using System.Web.UI;
using System.Reflection;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web;
using System.Configuration;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Label padrão.<br/>
    /// Este componente procura os controle informado em AssociatedControlID e verifica se é obrigatório. Se for coloca o * automaticamente.<br/>
    /// Também informa ao componente localizado os valores de MensagemErroFormatoSummary, MensagemErroObrigatorioSummary e MensagemErroInvalidoSummary 
    /// caso ele não possua valor definido desde que ele implemente a interface Employer.Componentes.UI.Web.Interface.IMensagemErro.
    /// </summary>
    #pragma warning disable 1591
    public class Label : System.Web.UI.WebControls.Label
    {
        #region Ctor
        /// <summary>
        /// Construtor da classe
        /// </summary>
        public Label()
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
                    return "O Campo {0} é Obrigatório.";
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

        public ModoRenderizacaoEnum ModoRenderizacao
        {
            get
            {
                var type = typeof(ModoRenderizacaoEnum);
                var value = ConfigurationManager.AppSettings["ModoRenderizacao"];
                var retValue = string.IsNullOrEmpty(value) ? false : Enum.IsDefined(type, value);
                return retValue ? (ModoRenderizacaoEnum)Enum.Parse(type, value) : ModoRenderizacaoEnum.Padrao;
            }
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

        #region AddAttributesToRender
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            object obj = this.ViewState["AssociatedControlNotInControlTree"];
            var ass = obj == null || (bool)obj;
            string associatedControlID = this.AssociatedControlID;
            if (associatedControlID.Length != 0)
            {
                if (ass)
                {
                    Control control = this.FindControl(associatedControlID);
                    if (control != null)                        
                    {
                        if (control is LocalizarBase)
                        {
                            var auto = control as LocalizarBase;
                            this.ViewState["AssociatedControlNotInControlTree"] = false;
                            this.AssociatedControlID = auto.CampoTexto.ClientID;
                        }
                        else if (control is ListaSugestaoBenesite)
                        {
                            var lista = control as ListaSugestaoBenesite;
                            this.ViewState["AssociatedControlNotInControlTree"] = false;
                            this.AssociatedControlID = lista.CampoTexto.ClientID;
                        }
                    }
                }
            }

            base.AddAttributesToRender(writer);

            this.ViewState["AssociatedControlNotInControlTree"] = ass;
        }
        #endregion

        #region Render
        /// <summary>
        /// Renderização dos controles na tela
        /// </summary>
        /// <param name="writer">Stream onde o controle será renderizado</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                writer.Write("Componente Label");
                return;
            }
            
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
                else {
                    this.Visible = c.Visible;
                }

                if (c is Employer.Componentes.UI.Web.Interface.IMensagemErro) {
                    Employer.Componentes.UI.Web.Interface.IMensagemErro componente = (Employer.Componentes.UI.Web.Interface.IMensagemErro) c;
                    if (String.IsNullOrEmpty(componente.MensagemErroFormatoSummary)) {                        
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
                    if (ModoRenderizacao == ModoRenderizacaoEnum.Bootstrap)
                    {
                        if (String.IsNullOrEmpty(this.MarcaObrigatorioCss))
                            this.Text = "<abbr title=\"Campo Obrigatório\">*</abbr>" + this.Text;
                        else
                            this.Text = string.Format(@"<abbr title=""Campo Obrigatório"" class=""{0}"">*</abbr>{1}", this.MarcaObrigatorioCss, this.Text);                                
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(this.MarcaObrigatorioCss))
                            this.Text = "<span>*</span>" + this.Text;
                        else
                            this.Text = "<span class=" + this.MarcaObrigatorioCss + ">*</span>" + this.Text;
                    }
                }
            }
            if(this.Visible)
                base.Render(writer);
        }
        #endregion
        #endregion
    }
}
