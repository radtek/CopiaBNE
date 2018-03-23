using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using Employer.Componentes.UI.Web.Util;
using Employer.Componentes.UI.Web.Extensions;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente com mascara de Telefone
    /// </summary>
    public class ControlTelefone : ControlBaseTextBox
    {
        /// <summary>
        /// Tipo de telefone.
        /// </summary>
        public enum TipoTelefone
        {
            /// <summary>
            /// Telefone fixo
            /// </summary>
            Telefone = 0,
            /// <summary>
            /// Telefone móvel
            /// </summary>
            Celular = 1
        }

        #region Campos
        private CustomValidator _cvTelefone = new CustomValidator() { ID = "_cvValor" };
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel() { ID = "_pnlValidacao" };
        #endregion

        #region Propriedades

        #region ValidadorTexto
        /// <inheritdoc/>
        protected override System.Web.UI.WebControls.BaseValidator ValidadorTexto
        {
            get { return _cvTelefone; }
        }
        #endregion

        #region PanelValidador
        /// <inheritdoc/>
        protected override System.Web.UI.WebControls.Panel PanelValidador
        {
            get { return _pnlValidador; }
        }
        #endregion

        #region NumeroDDD
        /// <summary>
        /// Número do DDD
        /// </summary>
        public String NumeroDDD
        {
            get
            {
                if (String.IsNullOrEmpty(this.CampoTexto.Text))
                    return String.Empty;

                return Formatadores.SomenteNumeros(this.CampoTexto.Text).SubstringEmpty(0, 2);
            }
            set
            {
                this.CampoTexto.Text = Formatadores.FormatarTelefone(value.SubstringEmpty(0, 2), NumeroTelefone, (Tipo == TipoTelefone.Celular));
            }

        }
        #endregion

        #region NumeroTelefone
        /// <summary>
        /// Número de telefone sem o DDD
        /// </summary>
        public String NumeroTelefone
        {
            get
            {
                if (String.IsNullOrEmpty(this.CampoTexto.Text))
                    return String.Empty;

                return Formatadores.SomenteNumeros(this.CampoTexto.Text).SubstringEmpty(2);
            }
            set
            {
                String v = value.SubstringEmpty(0, 9);
                if (String.IsNullOrEmpty(v))
                    v = value.SubstringEmpty(0, 8);

                this.CampoTexto.Text = Formatadores.FormatarTelefone(NumeroDDD, v, (Tipo == TipoTelefone.Celular));
            }
        }
        #endregion

        #region Tipo
        /// <summary>
        /// Tipo de Telefone
        /// </summary>
        public TipoTelefone Tipo
        {
            get
            {
                if (this.ViewState["Tipo"] == null)
                    return TipoTelefone.Telefone;
                return (TipoTelefone)this.ViewState["Tipo"];
            }
            set
            {
                this.ViewState["Tipo"] = value;
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region GetScriptDescriptors
        /// <summary>
        /// Retorna os descritores de script
        /// </summary>
        /// <returns>Uma coleção dos descritores de script</returns>
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.ControlTelefone", this.ClientID);
            this.SetScriptDescriptors(descriptor);
            descriptor.AddProperty("MensagemErroFormato", this.MensagemErroFormato);
            descriptor.AddProperty("Tipo", this.Tipo.GetHashCode());
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
            reference.Name = "Employer.Componentes.UI.Web.Content.js.ControlTelefone.js";
            references.Add(reference);


            return references;
        }
        #endregion

        #region OnLoad
        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _cvTelefone.ServerValidate += new ServerValidateEventHandler(_cvTelefone_ServerValidate);
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
            _cvTelefone.ValidateEmptyText = true;
            _cvTelefone.ClientValidationFunction = "Employer.Componentes.UI.Web.ControlTelefone.ValidarTextBox";
        }
        #endregion

        #region CreateChildControls
        /// <inheritdoc/>
        protected override void CreateChildControls()
        {
            InicializarTextBox();
            InicializarCustomValidator();
            InicializarPainel();
            Controls.Add(CampoTexto);
            base.CreateChildControls();
        }
        #endregion

        #region OnPreRender
        /// <inheritdoc/>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (!String.IsNullOrEmpty(this.CampoTexto.Text))
                this.CampoTexto.Text = Formatadores.FormatarTelefone(this.NumeroDDD, this.NumeroTelefone, (Tipo == TipoTelefone.Celular));
        }
        #endregion

        #endregion

        #region Eventos

        #region _cvTelefone_ServerValidate
        private void _cvTelefone_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = true;
            if (this.Obrigatorio && String.IsNullOrEmpty(this.Text))
            {
                args.IsValid = false;
                _cvTelefone.IsValid = false;
                _cvTelefone.ErrorMessage = this.MensagemErroObrigatorio;
                return;
            }

            int iddd=0;
            int.TryParse(this.NumeroDDD, out iddd);
            bool ddd9Digitos = Formatadores.isDdds9Digitos(iddd);

            if (!String.IsNullOrEmpty(this.Text))
            {
                if (Formatadores.SomenteNumeros(this.Text).Length < ((this.Tipo == TipoTelefone.Celular && ddd9Digitos) ? 11 : 10))
                {
                    args.IsValid = false;
                    _cvTelefone.IsValid = false;
                    _cvTelefone.ErrorMessage = this.MensagemErroFormato;
                    return;
                }
            }
            if (!String.IsNullOrEmpty(this.NumeroTelefone))
            {
                // Número inválido da área 11
                if (this.Tipo == TipoTelefone.Celular && ddd9Digitos)
                {
                    if (this.NumeroTelefone.Length != 9)
                    {
                        args.IsValid = false;
                        _cvTelefone.IsValid = false;
                        _cvTelefone.ErrorMessage = this.MensagemErroFormato;
                        return;
                    }
                }

                int inicioNumero = 0;

                if (this.NumeroTelefone.Length > 1)
                    int.TryParse(NumeroTelefone.Substring(0, 1), out inicioNumero);

                // Número inválido
                if (inicioNumero == 1)
                {
                    args.IsValid = false;
                    _cvTelefone.IsValid = false;
                    _cvTelefone.ErrorMessage = this.MensagemErroFormato;
                    return;
                }

                //var ddd = fone.substr(0, 2);
                var preficoCelular = new List<int>(new []{ 2, 3, 4, 5 });
                var execao = iddd == 11 ? new List<int>(new[] { 5 }) : new List<int>(0);

                // Telefones fixos começam com 
                if (preficoCelular.Contains(inicioNumero))
                    args.IsValid = execao.Contains(inicioNumero) || this.Tipo == TipoTelefone.Telefone;
                else
                    args.IsValid = execao.Contains(inicioNumero) || this.Tipo == TipoTelefone.Celular;
            }

            if (!args.IsValid)
            {
                _cvTelefone.IsValid = false;
                _cvTelefone.ErrorMessage = this.MensagemErroFormato;
                return;
            }

            _cvTelefone.IsValid = true;
            _cvTelefone.ErrorMessage = String.Empty;
        }
        #endregion

        #endregion
    }
}
