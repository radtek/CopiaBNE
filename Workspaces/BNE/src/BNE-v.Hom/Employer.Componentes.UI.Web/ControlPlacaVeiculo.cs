// -----------------------------------------------------------------------
// <copyright file="ControlPlacaVeiculo.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Employer.Componentes.UI.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;
    using System.Web.UI;

    /// <summary>
    /// WebControl com mascara de Placa de Veículo
    /// </summary>
    public class ControlPlacaVeiculo : ControlBaseTextBox
    {
        #region atributos
        private CustomValidator _cvValor = new CustomValidator { EnableClientScript = true, ValidateEmptyText = true };
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel();
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
            CampoTexto.Columns = 8;

            base.CreateChildControls();
        }
        #endregion        

        #region InicializarCustomValidator
        private void InicializarCustomValidator()
        {
            base.InicializarValidator();

            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.ControlPlacaVeiculo.ValidarTextBox";
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
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.ControlPlacaVeiculo", this.ClientID);

            this.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("RegexValidacao", @"[A-w]{3}\-\d{4}");

            return new ScriptControlDescriptor[] { descriptor };
        }

        /// <inheritdoc />
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.ControlPlacaVeiculo.js";
            references.Add(reference);

            return references;
        }
        #endregion

        #endregion
    }
}
