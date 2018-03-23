using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Text.RegularExpressions;
using Employer.Componentes.UI.Web.Util;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Caixa de texto que formata CNPJ
    /// </summary>
    public class ControlCNPJ : ControlBaseTextBox
    {
        #region atributos
        private CustomValidator _cvValor = new CustomValidator { ValidateEmptyText = true, ID = "cvValor" };
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel { ID = "pnlValidador" };
        #endregion

        #region Propriedades
        #region ValidadorTexto
        /// <summary>
        /// O Validador
        /// </summary>
        protected override System.Web.UI.WebControls.BaseValidator ValidadorTexto
        {
            get { return _cvValor; }
        }
        #endregion

        #region PanelValidador
        /// <summary>
        /// O painel que contém o validador
        /// </summary>
        protected override System.Web.UI.WebControls.Panel PanelValidador
        {
            get { return _pnlValidador; }
        }
        #endregion

        #region ValorDecimal
        /// <summary>
        /// Retorna o valor decimal do CNPJ
        /// </summary>
        public decimal? ValorDecimal
        {
            get
            {
                if (String.IsNullOrEmpty(this.Text))
                    return null;

                Regex r = new Regex("[^0-9]");
                return Convert.ToDecimal(
                    r.Replace(this.Text, String.Empty)
                );
            }
            set
            {
                if (value.HasValue)
                {
                    this.Text = Formatadores.FormatarCNPJ(value.Value);
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
                return base.Text;
            }
            set
            {
                if (!String.IsNullOrEmpty(value))
                {
                    base.Text = Formatadores.FormatarCNPJ(value);
                    return;
                }
                base.Text = value;
            }
        }
        #endregion


        #endregion

        #region Metodos       

        #region OnInit
        /// <inheritdoc />
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
                args.IsValid = Validacao.ValidarCNPJ(this.Text);
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
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.ControlCNPJ", this.ClientID);
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
            reference.Name = "Employer.Componentes.UI.Web.Content.js.ControlCNPJ.js";
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
            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.ControlCNPJ.ValidarTextBox";
        }
        #endregion

        #region Validar
        /// <summary>
        /// Invoca o validador
        /// </summary>
        public void Validar()
        {
            _cvValor.Validate();
        }
        #endregion
        #endregion
    }
}
