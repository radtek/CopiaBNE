// -----------------------------------------------------------------------
// <copyright file="EmployerPIS.cs" company="">
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
    using System.ComponentModel;
    using Employer.Componentes.UI.Web.Util;

    /// <summary>
    /// Componente texto com mascara de PIS.
    /// </summary>
    public class EmployerPIS : ControlBaseTextBox
    {
        #region enums
        /// <summary>
        /// Tipo do formato do valor
        /// </summary>
        public enum TipoValor
        {
            /// <inheritdoc/>
            Pasep,
            /// <inheritdoc/>
            PIS,
            /// <inheritdoc/>
            NIT,
            /// <inheritdoc/>
            SUS
        }
        #endregion

        #region atributos
        private CustomValidator _cvValor = new CustomValidator { EnableClientScript = true, ValidateEmptyText = true };
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel();
        private Label _lblValor = new Label { ID = "lblValor" };
        #endregion

        #region Propriedades
        /// <summary>
        /// Classe css da label que descreve o tipo
        /// </summary>
        public string CssClassDescricao
        {
            get
            {
                EnsureChildControls();
                return _lblValor.CssClass;
            }
            set
            {
                EnsureChildControls();
                _lblValor.CssClass = value;
            }
        }

        /// <summary>
        /// Exibe a descrição do tipo.
        /// </summary>
        public bool ExibirDescricao
        {
            get
            {
                EnsureChildControls();
                return _lblValor.Enabled;
            }
            set
            {
                EnsureChildControls();
                _lblValor.Enabled = value;
            }
        }

        /// <inheritdoc/>
        public override string Text
        {
            get
            {
                EnsureChildControls();

                var valor = Validacao.LimparMascaraPis(CampoTexto.Text);
                if (Validacao.ValidarFormatoPIS(valor))
                    return valor;
                return string.Empty;
            }
            set
            {
                EnsureChildControls();

                CampoTexto.Text = AtualizaTipo(value);
            }
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
            Controls.Add(_lblValor);

            CampoTexto.Columns = 15;
            CampoTexto.MaxLength = 11;

            base.CreateChildControls();
        }
        #endregion

        #region InicializarCustomValidator
        private void InicializarCustomValidator()
        {
            base.InicializarValidator();

            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.EmployerPIS.ValidarTextBox";
        }
        #endregion

        #region ControlBaseTextBox
        /// <inheritdoc/>
        protected override System.Web.UI.WebControls.BaseValidator ValidadorTexto
        {
            get 
            {
                EnsureChildControls();
                return _cvValor; 
            }
        }

        /// <inheritdoc/>
        protected override System.Web.UI.WebControls.Panel PanelValidador
        {
            get 
            {
                EnsureChildControls();
                return _pnlValidador; 
            }
        }

        #region Script
        /// <inheritdoc/>
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.EmployerPIS", this.ClientID);
            
            this.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("ExibirDescricao", this.ExibirDescricao);

            return new ScriptControlDescriptor[] { descriptor };
        }

        /// <inheritdoc/>
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.EmployerPIS.js";
            references.Add(reference);

            return references;
        }
        #endregion

        #endregion

        private string AtualizaTipo(string value)
        {
            try
            {
                 _lblValor.Text = string.Empty;
                if (!string.IsNullOrEmpty(value))
                {
                    value = value.Trim();
                    if (Validacao.ValidarPIS(value))
                    {
                        value = Validacao.LimparMascaraPis(value);
                        value = string.Format("{0}.{1}.{2}-{3}", value.Substring(0, 3), value.Substring(3, 5), value.Substring(8, 2), value.Substring(10));
                        _lblValor.Text = RecuperarTipo(value);
                    }
                }                
            }
            catch { }

            return value;
        }

        /// <inheritdoc/>
        protected override void OnPreRender(EventArgs e)
        {
            CampoTexto.Text = AtualizaTipo(CampoTexto.Text);

            base.OnPreRender(e);
        }
        
        #region RecuperarTipo
        /// <summary>
        /// Retorna a descrição do tipo
        /// </summary>
        /// <param name="valor">Número do PIS</param>
        /// <returns>Tipo do PIS</returns>
        public static string RecuperarTipo(string valor)
        {
            valor = Validacao.LimparMascaraPis(valor);
            string tipo = null;
            if (Validacao.ValidarFormatoPIS(valor))
            {
                if (Validacao.ValidarCalculoPis(valor))
                {
                    Int64 vlr = Convert.ToInt64(valor.Substring(0, 10));

                    if ((vlr >= 1000000000 && vlr <= 1019999999) || (vlr >= 1700000000 && vlr <= 1909999999))
                        tipo = TipoValor.Pasep.ToString();
                    else if ((vlr >= 1020000000 && vlr <= 1089999999) || (vlr >= 1200000000 && vlr <= 1669999999) || (vlr >= 1690000000 && vlr <= 1699999999))
                        tipo = TipoValor.PIS.ToString();
                    else if ((vlr >= 1090000000 && vlr <= 1199999999) || (vlr >= 1670000000 && vlr <= 1689999999) || (vlr >= 2670000000 && vlr <= 2679999999))
                        tipo = TipoValor.NIT.ToString();
                    //else if (vlr >= 2000000000 && vlr <= 2999999999)
                    else if ((vlr >= 2000000000 && vlr <= 2669999999) || (vlr >= 2680000000 && vlr <= 2999999999))
                        tipo = TipoValor.PIS.ToString();
                }
            }
            return tipo;
        }
        #endregion

        

        
    }
}
