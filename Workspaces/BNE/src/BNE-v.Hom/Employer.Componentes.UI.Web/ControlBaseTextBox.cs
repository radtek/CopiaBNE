using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using Employer.Componentes.UI.Web.Extensions;
using System.Configuration;

namespace Employer.Componentes.UI.Web
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
            /// <summary>
            /// Não restringe nenhum tipo de valor
            /// </summary>
            Padrao = 0,
            /// <summary>
            /// Aceita somente letras maiúsculas
            /// </summary>
            LetraMaiuscula = 1,
            /// <summary>
            /// Aceita somente letras minúsculas
            /// </summary>
            LetraMinuscula = 2,
            /// <summary>
            /// Aceita Somente números
            /// </summary>
            Numeros = 3,
            /// <summary>
            /// Aceita Somente letras
            /// </summary>
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
        /// <summary>
        /// Propriedade usada para informar se o controle é o primeiro campo na tela para o controle de AutoTabIndex se localizar melhor.<br/>
        /// Valor padrão verdadeiro.
        /// </summary>
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

        #region SemAutoTab
        /// <summary>
        /// Indica se deve desligar o AutoTabIndex somente para o componente.<br/>
        /// Valor padrão falso.
        /// </summary>
        public bool SemAutoTab
        {
            get
            {
                EnsureChildControls();
                bool semtab = false;
                return bool.TryParse(this.CampoTexto.Attributes["semtab"], out semtab) ? semtab : false;
            }
            set
            {
                EnsureChildControls();
                if (value)
                    this.CampoTexto.Attributes["semtab"] = "true";
                else
                    this.CampoTexto.Attributes.Remove("semtab");
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
        /// <inheritdoc/>  
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
        public new virtual bool CausesValidation
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
        /// <inheritdoc/>
        public new System.Web.UI.AttributeCollection Attributes
        {
            get 
            {
                EnsureChildControls();
                return this.CampoTexto.Attributes; 
            }
        }
        #endregion

        #region ModoRenderizacao
        /// <summary>
        /// Modifica a geração do html do componente
        /// </summary>
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

        #endregion
        /// <summary>
        /// Contrutor padrão
        /// </summary>
        public ControlBaseTextBox()
        {
            _txtValor.EnableTheming = false;
        }

        #region Métodos

        #region CreateChildControls
        /// <inheritdoc/>        
        protected override void CreateChildControls() 
        {

            base.CreateChildControls();
        }
        #endregion

        #region ExecutarValorAlterado
        /// <summary>
        /// Dispara o evento de valor alterado
        /// </summary>
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

                //SE o conteudo for nulo, não valida
                //permitir postback com conteúdo vazio.
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
