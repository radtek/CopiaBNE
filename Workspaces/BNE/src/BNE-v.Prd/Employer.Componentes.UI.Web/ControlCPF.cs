using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text.RegularExpressions;
using System.Globalization;
using Employer.Componentes.UI.Web.Util;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Caixa de texto que formata CPF
    /// </summary>
    public class ControlCPF : ControlBaseTextBox
    {
        #region atributos
        private CustomValidator _cvValor = new CustomValidator();
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel();
        #endregion

        #region Propriedades
        #region ValidadorTexto
        /// <summary>
        /// O Validador
        /// </summary>
        protected override System.Web.UI.WebControls.BaseValidator ValidadorTexto
        {
            get 
            {
                EnsureChildControls();
                return _cvValor; 
            }
        }
        #endregion

        

        #region PanelValidador
        /// <summary>
        /// O painel que contém o validador
        /// </summary>
        protected override System.Web.UI.WebControls.Panel PanelValidador
        {
            get 
            {
                EnsureChildControls();
                return _pnlValidador; 
            }
        }
        #endregion

        #region ValorDecimal
        /// <summary>
        /// Retorna o valor decimal do CPF
        /// </summary>
        public decimal? ValorDecimal
        {
            get
            {
                EnsureChildControls();
                if (String.IsNullOrEmpty(this.Text))
                    return null;

                Regex r = new Regex("[^0-9]");
                return Convert.ToDecimal(
                    r.Replace(this.Text, String.Empty)
                );
            }
            set
            {
                EnsureChildControls();
                if (value.HasValue)
                {
                    this.Text = Formatadores.FormatarCPF(value.Value);
                }
                else
                    this.Text = String.Empty;
            }
        }
        #endregion

        #region Text
        /// <inheritdoc />
        public override string Text
        {
            get
            {
                EnsureChildControls();
                return base.Text;
            }
            set
            {
                EnsureChildControls();
                if (!String.IsNullOrEmpty(value))
                {
                    base.Text = Formatadores.FormatarCPF(value);
                    return;
                }
                base.Text = value;
            }
        }
        #endregion

        #region CssMensagemErro
        /// <summary>
        /// Classe css do validador
        /// </summary>
        public String CssMensagemErro
        {
            set 
            {
                EnsureChildControls();
                _cvValor.CssClass = value; 
            }
        }
        #endregion
        #endregion

        #region Metodos

        #region OnInit
        /// <inheritdoc/>
        protected override void OnInit(EventArgs e)
        {
            _cvValor.ServerValidate += new ServerValidateEventHandler(_cvValor_ServerValidate);
            base.OnInit(e);
        }        
        #endregion 

        #region _cvValor_ServerValidate
        /// <summary>
        /// Dispara a validação server-side do validador
        /// </summary>
        /// <param name="source">O controle que disparou a ação</param>
        /// <param name="args">Os argumentos do evento</param>
        private void _cvValor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (this.Obrigatorio)
            {
                args.IsValid = !String.IsNullOrEmpty(this.Text);
                this._cvValor.IsValid = args.IsValid;
                this.ValidadorTexto.ErrorMessage = this.MensagemErroObrigatorio;
            }

            if (!String.IsNullOrEmpty(this.Text))
            {
                args.IsValid = Validacao.ValidarCPF(this.Text);
                this._cvValor.IsValid = args.IsValid;
                this.ValidadorTexto.ErrorMessage = MensagemErroFormato;
            }
        }
        #endregion

        #region CreateChildControls
        /// <summary>
        /// Cria os controles filhos
        /// </summary>
        protected override void CreateChildControls()
        {
            InicializarTextBox();
            InicializarCustomValidator();
            InicializarPainel();
            Controls.Add(CampoTexto);
            base.CreateChildControls();
        }
        #endregion  
                    
        #region GetScriptDescriptors
        /// <summary>
        /// Retorna os descritores de script
        /// </summary>
        /// <returns>Uma coleção dos descritores de script</returns>
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.ControlCPF", this.ClientID);            
            this.SetScriptDescriptors(descriptor);
            return new ScriptControlDescriptor[] { descriptor };
        }
        #endregion

        #region GetScriptReferences
        /// <summary>
        /// Retorna as referencias dos scripts
        /// </summary>
        /// <returns>Uma coleção das referências</returns>
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.ControlCPF.js";
            references.Add(reference);

            return references;
        }
        #endregion

        #region InicializarCustomValidator
        /// <summary>
        /// Inicializa o validador
        /// </summary>
        private void InicializarCustomValidator()
        {
            base.InicializarValidator();
            ValidadorTexto.ErrorMessage = String.Empty;
            _cvValor.Display = ValidatorDisplay.Dynamic;
            _cvValor.EnableClientScript = true;            
            _cvValor.ValidateEmptyText = true;
            _cvValor.ID = "cvValor";
            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.ControlCPF.ValidarTextBox";
        }
        #endregion

        #region Validar
        /// <summary>
        /// Método para disparar o validador.
        /// </summary>
        public void Validar()
        {
            _cvValor.Validate();
        }
        #endregion

        #region OnPreRender
        /// <inheritdoc />
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(this.CampoTexto.ClientID))
            {
                this.Validar();
                // Seta como se a validaçao não tivesse sido disparada se o texto estiver vazio
                if (String.IsNullOrEmpty(this.CampoTexto.Text))
                {
                    this.ValidadorTexto.ErrorMessage = String.Empty;
                    this.ValidadorTexto.IsValid = true;
                }
                else
                {
                    if (!this.ValidadorTexto.IsValid && String.IsNullOrEmpty(this.ValidadorTexto.ErrorMessage))
                        this.ValidadorTexto.ErrorMessage = MensagemErroFormato;
                }
            }
        }
        #endregion
        #endregion
    }
}
