using System;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;

namespace BNE.Componentes.Base
{
    /// <summary>
    /// Classe base para outros webcontros com TextBox e Validator
    /// </summary>
    public abstract class ControlBaseTextBox : ControlBaseValidator
    {

        #region TipoAlfaNumerico
        /// <summary>
        /// TipoAlfaNumerico
        /// </summary>
        public enum TipoAlfaNumerico
        {
            Padrao = 0,
            LetraMaiuscula = 1,
            LetraMinuscula = 2,
            Numeros = 3,
            Letras = 4
        }
        #endregion

        #region Atributos
        private TextBox _txtValor = new TextBox { ID = "txtValor" };
        #endregion

        #region Eventos
        /// <summary>
        /// Evento de valor alterado
        /// </summary>
        [Category("Action")]
        public event EventHandler ValorAlterado;

        /// <summary>
        /// Evento blur
        /// </summary>
        [Category("Action")]
        public event EventHandler Blur;
        #endregion

        #region Propriedades

        #region Primeiro
        public bool Primeiro
        {
            get
            {
                EnsureChildControls();
                return this.CampoTexto.Attributes["autotab"] == "primeiro";
            }
            set
            {
                EnsureChildControls();
                if (value)
                    this.CampoTexto.Attributes["autotab"] = "primeiro";
                else
                    this.CampoTexto.Attributes.Remove("autotab");
            }
        }
        #endregion

        #region CampoTexto
        /// <summary>
        /// O campo texto do controle
        /// </summary>
        protected override TextBox CampoTexto
        {
            get  
            {
                EnsureChildControls();
                return _txtValor; 
            }
        }
        #endregion

        #region Width
        /// <summary>
        /// Largura do campo texto
        /// </summary>
        public override Unit Width
        {
            get
            {
                EnsureChildControls();
                return _txtValor.Width;
            }
            set
            {
                EnsureChildControls();
                _txtValor.Width = value;
            }
        }
        #endregion

        #region Height
        /// <summary>
        /// Altura do campo texto
        /// </summary>
        public override Unit Height
        {
            get
            {
                EnsureChildControls();
                return _txtValor.Height;
            }
            set
            {
                EnsureChildControls();
                _txtValor.Height = value;
            }
        }
        #endregion

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [Category("TextBox"), DisplayName("ReadOnly")]
        public bool ReadOnly
        {
            get 
            {
                EnsureChildControls();
                return CampoTexto.ReadOnly; 
            }
            set 
            {
                EnsureChildControls();
                CampoTexto.ReadOnly = value;
                if (value)
                {
                    ViewState["TabIndex"] = CampoTexto.TabIndex;
                    CampoTexto.TabIndex = 1000;
                }
                else if (ViewState["TabIndex"] != null)
                    CampoTexto.TabIndex = (short)ViewState["TabIndex"];
            }
        }
        #endregion

        #region Tamanho
        /// <summary>
        /// Tamanho do campo
        /// </summary>
        [Category("TextBox"), DisplayName("Tamanho")]
        public int Tamanho
        {
            get 
            {
                EnsureChildControls();
                return CampoTexto.MaxLength; 
            }
            set 
            {
                EnsureChildControls();
                CampoTexto.MaxLength = value; 
            }
        }
        #endregion

        #region Columns
        /// <summary>
        /// Tamanho do campo
        /// </summary>
        [Category("TextBox"), DisplayName("Columns")]
        public int Columns
        {
            get 
            {
                EnsureChildControls();
                return CampoTexto.Columns; 
            }

            set 
            {
                EnsureChildControls();
                CampoTexto.Columns = value; 
            }
        }
        #endregion

        #region ValorAlteradoClient
        /// <summary>
        /// Define uma função em javascript disparada ao sair do campo somente quando um valor é alterado.
        /// </summary>
        [Category("JavaScript"), DisplayName("Valor Alterado Client")]
        public string ValorAlteradoClient
        {
            get
            {
                if (ViewState["ValorAlteradoClient"] == null)
                    return null;
                return (string)ViewState["ValorAlteradoClient"];
            }
            set { ViewState["ValorAlteradoClient"] = value; }
        }
        #endregion

        #region Enabled        
        public override bool Enabled
        {
            get 
            {
                EnsureChildControls();
                return CampoTexto.Enabled; 
            }
            set
            {
                EnsureChildControls();
                ValidadorTexto.Enabled = value;
                CampoTexto.Enabled = value;
                //base.Enabled = value;
            }
        }
        #endregion

        #region CausesValidation
        /// <inheritdoc />        
        public virtual bool CausesValidation
        {
            get 
            {
                EnsureChildControls();
                return this._txtValor.CausesValidation; 
            }
            set 
            {
                EnsureChildControls();
                this._txtValor.CausesValidation = value; 
            }
        }
        #endregion

        #region TabIndex
        /// <inheritdoc />    
        public override short TabIndex
        {
            get
            {
                EnsureChildControls();
                return _txtValor.TabIndex;
            }
            set
            {
                EnsureChildControls();
                _txtValor.TabIndex = value;
            }
        }
        #endregion

        #region AutoPostBack
        /// <summary>
        /// Define se o componente terá autopostback
        /// </summary>
        public Boolean AutoPostBack
        {
            get
            {
                if (ViewState["AutoPostBack"] == null)
                    return false;
                return (Boolean)ViewState["AutoPostBack"];
            }
            set { ViewState["AutoPostBack"] = value; }
        }
        #endregion

        #region Attributes
        public System.Web.UI.AttributeCollection Attributes
        {
            get 
            {
                EnsureChildControls();
                return this.CampoTexto.Attributes; 
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region CreateChildControls
        /// <inheritdoc/>        
        protected override void CreateChildControls() 
        {

            base.CreateChildControls();
        }
        #endregion

        #region ExecutarValorAlterado
        public void ExecutarValorAlterado()
        {
            if (this.ValorAlterado != null)
                this.ValorAlterado(this._txtValor, new EventArgs());

        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Método de configuração das propriedades do campo Valor Decimal.
        /// </summary>
        protected void InicializarTextBox()
        {
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;
            _txtValor.EnableTheming = false;
        }
        #endregion

        #region SetScriptDescriptors
        /// <summary>
        /// Registra as propriedades para o objeto em javascript
        /// </summary>
        /// <param name="descriptor"></param>
        protected override void SetScriptDescriptors(ScriptControlDescriptor descriptor)
        {
            base.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("ValorAlteradoClient", this.ValorAlteradoClient);
            descriptor.AddProperty("isValorAlterado", this.ValorAlterado != null && this.Blur == null);
            descriptor.AddProperty("isPostBack", this.Blur != null || ValorAlterado != null || this.AutoPostBack);
        }
        #endregion

        #region OnPreRender
        /// <inheritdoc />
        protected override void OnPreRender(EventArgs e)
        {
            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            string eventTarget = Page.Request.Params["__EVENTTARGET"];
            if (ValorAlterado != null || Blur != null)
            {
                bool disparaEvento = false;

                ///SE o conteudo for nulo, não valida
                ///permitir postback com conteúdo vazio.
                if (String.IsNullOrEmpty(eventArgument) && !String.IsNullOrEmpty(eventTarget) && eventTarget.Equals(_txtValor.ClientID))                
                    disparaEvento = this.ValidadorTexto.IsValid;

                if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(_txtValor.ClientID) && this.ValidadorTexto != null)
                { 
                    this.ValidadorTexto.Validate();
                    disparaEvento = this.ValidadorTexto.IsValid;
                }

                if (disparaEvento)
                {
                    if (Blur != null)
                        Blur(_txtValor, null);
                    else
                        ValorAlterado(_txtValor, null);
                }
            }

            base.OnPreRender(e);
        }
        #endregion

        #endregion

    }
}
