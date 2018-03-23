using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;

[assembly: WebResource("Employer.Componentes.UI.Web.Content.js.EmployerHora.js", "text/javascript")]

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente de texto com mascará de hora
    /// </summary>
    public class EmployerHora : ControlBaseTextBox
    {
        #region atributos
        private CustomValidator _cvValor = new CustomValidator { EnableClientScript = true, ValidateEmptyText = true };
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel();
        #endregion

        #region Propriedades

        #region ClientOnBlur
        /// <summary>
        /// Função a ser chamada ao executar a função Blur.<br/>
        ///  Ex: <code>functionName(parameter)</code>
        /// </summary>
        public String ClientOnBlur
        {
            private get
            {
                if (ViewState["ClientOnBlur"] == null)
                    return String.Empty;
                return (String)ViewState["ClientOnBlur"];
            }
            set
            {
                ViewState["ClientOnBlur"] = value;
            }
        }
        #endregion

        #region Text
        /// <inheritdoc/>
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = AplicarMascara(value);
            }
        }
        #endregion

        #endregion

        #region AplicarMascara
        private String AplicarMascara(String valor)
        {
            if (!String.IsNullOrEmpty(valor))
            {
                valor = valor.PadRight(4, '0');
                return String.Format("{0}:{1}", valor.Substring(0, 2), valor.Substring(2, 2));
            }
            return String.Empty;
        }
        #endregion

        #region CreateChildControls
        /// <inheritdoc />
        protected override void CreateChildControls()
        {
            InicializarTextBox();
            InicializarCustomValidator();
            InicializarPainel();
            //CampoTexto.Enabled = this.Enabled;
            Controls.Add(CampoTexto);

            CampoTexto.Columns = 5;
            CampoTexto.MaxLength = 5;

            base.CreateChildControls();
        }
        #endregion   
     
        #region InicializarCustomValidator
        private void InicializarCustomValidator()
        {
            base.InicializarValidator();

            if (string.IsNullOrEmpty(this.MensagemErroFormato))
                this.MensagemErroFormato = Resources.EmployerHoraMensagemErroFormato;

            if (string.IsNullOrEmpty(this.MensagemErroFormatoSummary))
                this.MensagemErroFormatoSummary = Resources.EmployerHoraMensagemErroFormatoSummary;

            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.EmployerHora.ValidarTextBox";
        }
        #endregion

        #region ControlBaseTextBox
        /// <inheritdoc />
        protected override System.Web.UI.WebControls.BaseValidator ValidadorTexto
        {
            get { return _cvValor; }
        }

        /// <inheritdoc />
        protected override System.Web.UI.WebControls.Panel PanelValidador
        {
            get { return _pnlValidador; }
        }

        #region Script
        /// <inheritdoc />
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.EmployerHora", this.ClientID);

            this.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("ClientOnBlur", this.ClientOnBlur);

            return new ScriptControlDescriptor[] { descriptor };
        }

        /// <inheritdoc />
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.EmployerHora.js";
            references.Add(reference);

            return references;
        }
        #endregion

        #endregion
    }
}
