using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using Employer.Componentes.UI.Web.Interface;
using Employer.Componentes.UI.Web.Extensions;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Controle de lista de sugestões
    /// </summary>
    #pragma warning disable 1591
    [Bindable(true)]
    public class ListaSugestaoBenesite : AjaxClientDataBoundControlBase, IRequiredField, IMensagemErro
    {
        #region TipoListaSugestao
        /// <summary>
        /// Tipo da lista de sugestão
        /// </summary>
        public enum TipoListaSugestao
        {
            /// <summary>
            /// Aceita Texto
            /// </summary>
            Texto = 0,
            /// <summary>
            /// Aceita números
            /// </summary>
            Numero = 1,
            /// <summary>
            /// Aceita somente texto em maíusculo
            /// </summary>
            TextoMaiusculo = 2
        }
        #endregion

        #region Atributos
        private TextBox _txtValor = new TextBox();
        private CustomValidator _cvValor = new CustomValidator();
        private System.Web.UI.WebControls.Panel _pnlOculto = new System.Web.UI.WebControls.Panel();
        private System.Web.UI.WebControls.Panel _pnlValidador = new System.Web.UI.WebControls.Panel();
        private System.Web.UI.WebControls.Panel _pnlContainer = new System.Web.UI.WebControls.Panel();
        private Label _lblDescription = new Label();
        private LiteralControl _litTabela = new LiteralControl();
        #endregion

        #region Ctor
        /// <inheritdoc/>
        public ListaSugestaoBenesite()
        {
            this._txtValor.EnableTheming = false;
            this._pnlOculto.EnableTheming = false;
            this._pnlContainer.EnableTheming = false;
            this._lblDescription.EnableTheming = false;
        }
        #endregion

        #region Propriedades

        #region SemAutoTab
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

        #region Width
        /// <summary>
        /// Largura do componente de texto
        /// </summary>
        public override Unit Width
        {
            get
            {
                if (this._txtValor.Width == Unit.Empty)
                    this._txtValor.Width = Unit.Pixel(25);

                return this._txtValor.Width;
            }
            set
            {
                this._txtValor.Width = value;
            }
        }
        #endregion

        #region Descricao
        /// <summary>
        /// Texto da descrição
        /// </summary>
        [Browsable(false)]
        public String Descricao
        {
            get
            {
                if (this.MostrarDescricao)
                {
                    if (!string.IsNullOrEmpty(this._txtValor.Text))
                    {
                        if (this.Dicionario.ContainsKey(this._txtValor.Text))
                            return Dicionario[this._txtValor.Text];
                    }

                    return this._lblDescription.Text;
                }
                else
                    return String.Empty;
            }
            private set { this._lblDescription.Text = value; }
        }
        #endregion

        #region Obrigatorio
        /// <summary>
        /// Define se o campo é ou não obrigatório
        /// </summary>
        public Boolean Obrigatorio
        {
            get
            {
                if (this.ViewState["Obrigatorio"] == null)
                    this.ViewState["Obrigatorio"] = false;

                return (Boolean)this.ViewState["Obrigatorio"];
            }
            set { this.ViewState["Obrigatorio"] = value; }
        }
        #endregion

        #region MostrarDescricao
        /// <summary>
        /// Define se vai ou não exibir a descrição
        /// </summary>
        public Boolean MostrarDescricao
        {
            get
            {
                if (this.ViewState["MostrarDescricao"] == null)
                    this.ViewState["MostrarDescricao"] = true;

                return (Boolean)this.ViewState["MostrarDescricao"];
            }
            set { this.ViewState["MostrarDescricao"] = value; }
        }
        #endregion

        #region MostrarListaSugestao
        /// <summary>
        /// Define se vai ou não exibir a descrição
        /// </summary>
        public Boolean MostrarListaSugestao
        {
            get
            {
                if (this.ViewState["MostrarListaSugestao"] == null)
                    this.ViewState["MostrarListaSugestao"] = true;

                return (Boolean)this.ViewState["MostrarListaSugestao"];
            }
            set { this.ViewState["MostrarListaSugestao"] = value; }
        }
        #endregion

        #region MensagemErroObrigatorio
        /// <summary>
        /// Mensagem de Erro Obrigatorio
        /// </summary>
        [Category("Validador"), DisplayName("Mensagem Erro Obrigatorio"), Localizable(true)]
        public string MensagemErroObrigatorio
        {
            get
            {
                if (ViewState["MensagemErroObrigatorio"] == null)
                    return Resources.ControlBaseValidatorMensagemErroObrigatorio;
                return (string)ViewState["MensagemErroObrigatorio"];
            }
            set { ViewState["MensagemErroObrigatorio"] = value; }
        }
        #endregion

        #region MensagemErroInvalido
        /// <summary>
        /// Mensagem de Erro Inválidto
        /// </summary>
        [Category("Validador"), DisplayName("Mensagem Erro Obrigatorio"), Localizable(true)]
        public string MensagemErroInvalido
        {
            get
            {
                if (ViewState["MensagemErroInvalido"] == null)
                    return Resources.ControlBaseValidatorMensagemErroInvalido;
                return (string)ViewState["MensagemErroInvalido"];
            }
            set { ViewState["MensagemErroInvalido"] = value; }
        }
        #endregion

        #region MensagemErroInvalidoSummary
        public String MensagemErroInvalidoSummary
        {
            get
            {
                return Convert.ToString(this.ViewState["MensagemErroInvalidoSummary"]);
            }
            set
            {
                this.ViewState["MensagemErroInvalidoSummary"] = value;
            }
        }
        #endregion

        #region MensagemErroObrigatorioSummary
        public String MensagemErroObrigatorioSummary
        {
            get
            {
                return Convert.ToString(this.ViewState["MensagemErroObrigatorioSummary"]);
            }
            set
            {
                this.ViewState["MensagemErroObrigatorioSummary"] = value;
            }
        }
        #endregion

        #region MensagemErroFormatoSummary
        /// <summary>
        /// Interface obriga a implementar
        /// </summary>
        public string MensagemErroFormatoSummary
        {
            get
            {
                return String.Empty;
            }
            set
            {
            }
        }
        #endregion

        #region Dicionario
        /// <summary>
        /// Dicionário contendo os dados da lista
        /// </summary>
        protected Dictionary<String, String> Dicionario
        {
            get
            {
                if (this.ViewState["dic"] == null)
                    this.ViewState["dic"] = new Dictionary<String, String>();

                return (Dictionary<String, String>)this.ViewState["dic"];
            }
            set { this.ViewState["dic"] = value; }
        }
        #endregion

        #region CampoTexto
        /// <summary>
        /// O Campo de texto usado no controle
        /// </summary>
        internal TextBox CampoTexto
        {
            get
            {
                return this._txtValor;
            }
        }
        #endregion

        #region Valor
        /// <summary>
        /// O valor selecionado no controle
        /// </summary>        
        [Bindable(true)]
        public String Valor
        {
            get
            {
                //if (this.Dicionario.Count == 0)
                //    return String.Empty;

                return this._txtValor.Text;
            }
            set
            {
                this._txtValor.Text = Convert.ToString(value);
            }
        }
        #endregion

        #region ValorBooleano
        /// <summary>
        /// Retorna o valor selecionado como um booleano, true ou false
        /// </summary>
        public Boolean? ValorBooleano
        {
            get
            {
                if (this.Dicionario.Count == 0)
                    return null;

                if (String.IsNullOrEmpty(this._txtValor.Text))
                    return null;

                return "1".Equals(this._txtValor.Text, StringComparison.OrdinalIgnoreCase);
            }
            set
            {
                this._txtValor.Text = Convert.ToString(Convert.ToInt32(value));
            }
        }
        #endregion

        #region ValorInt
        /// <summary>
        /// Retorna o valor como um inteiro
        /// </summary>
        public int? ValorInt
        {
            get
            {
                //if (this.Dicionario.Count == 0)
                //    return null;

                if (String.IsNullOrEmpty(this._txtValor.Text))
                    return null;

                int dummy = 0;
                if (int.TryParse(this._txtValor.Text, out dummy))
                    return Convert.ToInt32(this._txtValor.Text);
                else
                    return null;
            }
            set { this._txtValor.Text = Convert.ToString(value); }
        }
        #endregion

        #region TipoSugestao
        /// <summary>
        /// O tipo de dado da lista de sugestão
        /// </summary>
        public TipoListaSugestao TipoSugestao
        {
            get
            {
                if (this.ViewState["TipoSugestao"] == null)
                    this.ViewState["TipoSugestao"] = TipoListaSugestao.Texto;

                return (TipoListaSugestao)this.ViewState["TipoSugestao"];
            }
            set { this.ViewState["TipoSugestao"] = value; }
        }
        #endregion

        #region CampoChave
        /// <summary>
        /// Campo chave do DataSource
        /// </summary>
        public String CampoChave
        {
            get
            {
                return Convert.ToString(this.ViewState["CampoChave"]);
            }
            set
            {
                this.ViewState["CampoChave"] = value;
            }
        }
        #endregion

        #region CampoDescricao
        /// <summary>
        /// Campo descrição do DataSource
        /// </summary>
        public String CampoDescricao
        {
            get
            {
                return Convert.ToString(this.ViewState["CampoDescricao"]);
            }
            set
            {
                this.ViewState["CampoDescricao"] = value;
            }
        }
        #endregion

        #region CssClassTextBox
        /// <summary>
        /// Css da TextBox
        /// </summary>
        public string CssClassTextBox
        {
            get
            {
                return this._txtValor.CssClass;
            }
            set
            {
                this._txtValor.CssClass = value;
            }
        }
        #endregion

        #region CssClassDescricao
        /// <summary>
        /// Css da descrição
        /// </summary>
        public string CssClassDescricao
        {
            get
            {
                return this._lblDescription.CssClass;
            }
            set
            {
                this._lblDescription.CssClass = value;
            }
        }
        #endregion

        #region CssClassSugestao
        /// <summary>
        /// Css da descrição
        /// </summary>
        public string CssClassSugestao
        {
            get
            {
                return this._pnlOculto.CssClass;
            }
            set
            {
                this._pnlOculto.CssClass = value;
            }
        }
        #endregion

        #region CssClassValidador
        /// <summary>
        /// Css class do validador
        /// </summary>
        public string CssClassValidador
        {
            get { return this._cvValor.CssClass; }
            set { this._cvValor.CssClass = value; }
        }
        #endregion

        #region IsValid
        /// <summary>
        /// Se o controle está validado
        /// </summary>
        public Boolean IsValid
        {
            get
            {
                return this._cvValor.IsValid;
            }
        }
        #endregion

        #region ValidationGroup
        /// <summary>
        /// O grupo de validação
        /// </summary>
        public String ValidationGroup
        {
            get
            {
                return Convert.ToString(this.ViewState["ValidationGroup"]);
            }
            set
            {
                this.ViewState["ValidationGroup"] = value;
                this._cvValor.ValidationGroup = value;
                this._txtValor.ValidationGroup = value;
            }
        }
        #endregion

        #region CausesValidation
        /// <summary>
        /// Determina se o controle causa validação
        /// </summary>
        public Boolean CausesValidation
        {
            get
            {
                return this._txtValor.CausesValidation;
            }
            set
            {
                this._txtValor.CausesValidation = value;
            }
        }
        #endregion

        #region Tamanho
        /// <summary>
        /// Quantidade de caracteres máximos na lista de sugestão
        /// </summary>
        public int Tamanho
        {
            get
            {
                return this._txtValor.MaxLength;
            }
            set
            {
                this._txtValor.MaxLength = value;
                MaxLengthOriginal = value;
            }

        }
        #endregion

        #region TabIndex
        /// <inheritdoc />        
        public override short TabIndex
        {
            get
            {
                return _txtValor.TabIndex;
            }
            set
            {
                _txtValor.TabIndex = value;
            }
        }
        #endregion

        #region MaxLength Definido pelo Usuário
        private int MaxLengthOriginal
        {
            get 
            {
                EnsureChildControls();
                if (ViewState["MaxLengthOriginal"] == null)
                    ViewState["MaxLengthOriginal"] = _txtValor.MaxLength;
                return (int)ViewState["MaxLengthOriginal"];
             }
            set {
                EnsureChildControls();
                ViewState["MaxLengthOriginal"] = value; 
            }
        }
        #endregion

        #endregion

        #region Métodos
        public override void Focus()
        {
            EnsureChildControls();

            if (!AutoTabIndex.Focus(this.CampoTexto.ClientID))
                ScriptManager.GetCurrent(this.Page).SetFocus(this.CampoTexto.ClientID);
        }

        #region CreateChildControls
        /// <summary>
        /// Cria os controles filhos do componente
        /// </summary>
        protected override void CreateChildControls()
        {
            _cvValor.ID = "cvValor";
            _txtValor.ID = "txtValor";
            _cvValor.ControlToValidate = string.Empty;

            if (this.DataSource == null && String.IsNullOrEmpty(this.DataSourceID) && !this.ChildControlsCreated)
            {
                InicializarTextBox();
                InicializarCustomValidator();
                InicializarPainel();
                InicializarPainelOculto();
                InicializarLabel();
                InicializarPainelContainer();

                _pnlValidador.Controls.Add(_cvValor);

                if (this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
                {   
                    Controls.Add(_pnlValidador);
                    Controls.Add(_txtValor);
                    Controls.Add(_pnlContainer);
                }
                else
                {
                    Controls.Add(_txtValor);
                    Controls.Add(_pnlValidador);
                    Controls.Add(_pnlContainer);
                }

                _cvValor.ControlToValidate = _txtValor.ID;
            }
            else
                base.CreateChildControls();
        }
        /// <summary>
        /// Cria os controles filhos do componente
        /// </summary>             
        protected override int CreateChildControls(System.Collections.IEnumerable dataSource, bool dataBinding)
        {
            _cvValor.ID = "cvValor";
            _txtValor.ID = "txtValor";
            _cvValor.ControlToValidate = string.Empty;

            if (this.Controls.Count == 0)
            {
                InicializarTextBox();
                InicializarCustomValidator();
                InicializarPainel();
                InicializarPainelOculto();
                InicializarLabel();
                InicializarPainelContainer();

                Controls.Add(_pnlValidador);
                Controls.Add(_txtValor);
                _pnlValidador.Controls.Add(_cvValor);
                Controls.Add(_pnlContainer);

                
                _cvValor.ControlToValidate = _txtValor.ID;
            }
            if (!this.DesignMode)
            {
                if (dataBinding)
                {
                    this.Dicionario = new Dictionary<string, string>();
                    if (dataSource != null)
                    {
                        foreach (Object obj in dataSource)
                        {
                            this.Dicionario.Add(
                                Convert.ToString(DataBinder.GetPropertyValue(obj, this.CampoChave)),
                                Convert.ToString(DataBinder.GetPropertyValue(obj, this.CampoDescricao)));
                        }
                    }

                    return Dicionario.Count;
                }
            }
            return 0;
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Inicializa os dados da Text Box
        /// </summary>
        private void InicializarTextBox()
        {
            
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;

            //_txtValor.Style.Add(HtmlTextWriterStyle.Position, "relative");
            //_txtValor.Style.Add(HtmlTextWriterStyle.Display, "inline");
            _txtValor.Style.Add("float", "left");
        }
        #endregion

        #region InicializarPainelOculto
        /// <summary>
        /// Incializa os dados do painel oculto
        /// </summary>
        private void InicializarPainelOculto()
        {
            //Adicionar Controles no WebForm
            _pnlOculto.ID = "pnlOculto";
            _pnlOculto.Controls.Clear();
            _pnlOculto.Controls.Add(this._litTabela);
            _pnlOculto.Style.Clear();
            _pnlOculto.Style.Add(HtmlTextWriterStyle.Display, "none");

            _pnlOculto.Style.Add("float", "left");
            _pnlOculto.Style.Add("position", "absolute");
            _pnlOculto.Style.Add("margin-left", "0px");
        }
        #endregion

        #region InicializarLabel
        /// <summary>
        /// Inicializa os dados da Label
        /// </summary>
        private void InicializarLabel()
        {
            this._lblDescription.ID = "lblDescricao";
        }
        #endregion

        #region InicializarCustomValidator
        /// <summary>
        /// Inicializa o validador
        /// </summary>
        private void InicializarCustomValidator()
        {
            _cvValor.ValidateEmptyText = true;
            _cvValor.Display = ValidatorDisplay.Static;
            
            _cvValor.ValidateEmptyText = true;
            _cvValor.ClientValidationFunction = "Employer.Componentes.UI.Web.ListaSugestao.ValidarTextBox";
            
        }
        #endregion

        #region InicializarPainel
        /// <summary>
        /// Inicializa o Painel do validador
        /// </summary>
        private void InicializarPainel()
        {
            //Adicionar Controles no WebForm
            _pnlValidador.ID = "pnlValidador";
            _pnlValidador.Style.Add(HtmlTextWriterStyle.Display, "block");
        }
        #endregion

        #region MontarTabela
        /// <summary>
        /// Monta a tabela com a grade de sugestões
        /// </summary>
        /// <returns>A String com o texto da tabela</returns>
        private void MontarTabela()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<table>");
            builder.Append("<tbody>");

            if (this.Dicionario.Count > 0)
            {
                foreach (String key in this.Dicionario.Keys)
                {
                    builder.Append("<tr>");
                    builder.AppendFormat("<td class='lsg_col1' key='{0}'>", key);
                    builder.Append(key);
                    builder.Append("</td>");
                    builder.AppendFormat("<td class='lsg_col2' key='{0}'>", key);
                    builder.Append(Dicionario[key]);
                    builder.Append("</td>");
                    builder.Append("</tr>");
                }

                if (this.MaxLengthOriginal == 0)
                {
                    _txtValor.MaxLength = this.Dicionario.Select(e => e.Key.Length).Max();
                }
            }
            else
            {
                builder.Append("<tr>");
                builder.Append("<td class='lsg_col2'>");
                builder.Append(Resources.ListaSugestaoVazia);
                builder.Append("</td>");
                builder.Append("</tr>");

                _txtValor.MaxLength = this.MaxLengthOriginal;
            }
            builder.Append("</tbody>");
            builder.Append("</table>");
            this._litTabela.Text = builder.ToString();
        }
        #endregion

        #region GetScriptDescriptors
        /// <summary>
        /// Regorna os descritores de script
        /// </summary>
        /// <returns>O descritor de scripts</returns>
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.ListaSugestao", this.ClientID);
            descriptor.AddProperty("Width", this.Width);
            descriptor.AddProperty("Dicionario", this.Dicionario);
            descriptor.AddProperty("Descricao", this.Descricao);
            descriptor.AddProperty("IsServerBlur", this.Blur != null);
            descriptor.AddProperty("IsPostBack", this.Blur != null || ValueChanged != null);            

            descriptor.AddProperty("MensagemErroInvalido", this.MensagemErroInvalido);
            descriptor.AddProperty("MensagemErroObrigatorio", this.MensagemErroObrigatorio);

            descriptor.AddProperty("MensagemErroInvalidoSummary", this.MensagemErroInvalidoSummary);
            descriptor.AddProperty("MensagemErroObrigatorioSummary", this.MensagemErroObrigatorioSummary);


            descriptor.AddProperty("Obrigatorio", this.Obrigatorio);
            descriptor.AddProperty("MostrarDescricao", this.MostrarDescricao);
            descriptor.AddProperty("MostrarListaSugestao", this.MostrarListaSugestao);
            descriptor.AddProperty("TipoSugestao", this.TipoSugestao);

            this.SetScriptDescriptors(descriptor);
            return new ScriptControlDescriptor[] { descriptor };
        }
        #endregion

        #region GetScriptReferences
        /// <summary>
        /// Retorna as referências do script
        /// </summary>
        /// <returns>A referência de scripts</returns>
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.ListaSugestao.js";
            references.Add(reference);

            return references;
        }
        #endregion

        #region RenderBeginTag
        /// <summary>
        /// Renderiza a tag de abertura 
        /// </summary>
        /// <param name="writer">O renderizador</param>
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.AddAttribute(HtmlTextWriterAttribute.Id, this.ClientID);
            if (!String.IsNullOrEmpty(this.CssClass))
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass);
            }
            writer.RenderBeginTag(HtmlTextWriterTag.Div);
        }
        #endregion

        #region InicializarPainelContainer
        /// <summary>
        /// Incializa os dados do painel container
        /// </summary>
        private void InicializarPainelContainer()
        {
            _pnlContainer.ID = "pnlContainer";
            _pnlContainer.Controls.Clear();
            _pnlContainer.Controls.Add(_lblDescription);
            _pnlContainer.Controls.Add(_pnlOculto);
            _pnlContainer.Style.Clear();
            _pnlContainer.Style.Add("float", "left");
        }
        #endregion
        #endregion

        #region Event Handlers

        /// <summary>
        /// Evento disparado quando o valor selecionado na lista é alterado
        /// </summary>
        public event ValueChangedEvent ValueChanged;

        /// <summary>
        /// Evento blur
        /// </summary>
        [Category("Action")]
        public event EventHandler Blur;

        #endregion

        #region Eventos
        #region OnLoad
        /// <summary>
        /// Evento que ocorre quando o componente é carregado
        /// </summary>
        /// <param name="e">Os argumentos do evento</param>
        protected override void OnLoad(EventArgs e)
        {
            //    if (String.IsNullOrEmpty(this._txtValor.Text))
            //    {
            //        this.Valor = this._txtValor.Text;
            //    }
            //    else
            //this._txtValor.Text = Convert.ToString(this.Valor);

            if (ValueChanged != null)
            {
                //this._txtValor.TextChanged += new EventHandler(_txtValor_TextChanged);
                //this._txtValor.AutoPostBack = true;
                //this._txtValor.Attributes["OnChange"] = "__doPostBack('" + this.ClientID + "','Change')";
            }

            _cvValor.ServerValidate += new ServerValidateEventHandler(_cvValor_ServerValidate);

            base.OnLoad(e);
        }
        #endregion

        #region OnInit
        /// <summary>
        /// Evento de Inicialização do componente
        /// </summary>
        /// <param name="e">Os argumentos do evento</param>
        protected override void OnInit(EventArgs e)
        {
            this.EnsureDataBound();
            base.OnInit(e);
        }
        #endregion

        #region OnPreRender
        /// <summary>
        /// Evento disparado na fase de pré-renderização
        /// </summary>
        /// <param name="e">Os argumentos do evento</param>
        protected override void OnPreRender(EventArgs e)
        {
            this._txtValor.Enabled = this.Enabled;
            this._cvValor.Enabled = this.Visible && this.Enabled;

            string eventTarget = Page.Request.Params["__EVENTTARGET"];
            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(eventTarget) && eventTarget.Equals(this.ClientID)
                && "Change".Equals(eventArgument))
            {
                _txtValor_TextChanged(_txtValor, null);
            }

           

            base.OnPreRender(e);
        }
        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.DesignMode)
            {
                writer.Write("Componente ListaSugestaoBenesite");
                return;
            }

            this.MontarTabela();

            //Deve realmente ficar aqui o controle ou este controle??? Na function de foco do js a visibilidade da lista é setada sem validação. Deve ficar lá?? ?
            if (this.MostrarListaSugestao)
                this._litTabela.Visible = true;
            else
                this._litTabela.Visible = false;


            if (this.MostrarDescricao)
            {
                if (this.Dicionario.ContainsKey(this._txtValor.Text))
                    this.Descricao = Dicionario[this._txtValor.Text];
                else
                    this.Descricao = String.Empty;
            }
            else
                this.Descricao = String.Empty;


            base.Render(writer);
        }

        #region _cvValor_ServerValidate
        /// <summary>
        /// Evento de validação do servidor
        /// </summary>
        /// <param name="source">O controle que disparou o evento</param>
        /// <param name="args">Os argumentos do evento</param>
        void _cvValor_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (this._txtValor.Text == String.Empty)
            {
                if (this.Obrigatorio)
                {
                    args.IsValid = false;
                    _cvValor.ErrorMessage = this.MensagemErroObrigatorio;
                    _cvValor.IsValid = false;
                    return;
                }
                else
                {
                    args.IsValid = true;
                    _cvValor.IsValid = true;
                    return;
                }
            }

            if (!this.Dicionario.ContainsKey(this._txtValor.Text))
            {
                args.IsValid = false;
                _cvValor.IsValid = false;
                _cvValor.ErrorMessage = this.MensagemErroInvalido;
                return;
            }

            args.IsValid = true;

        }
        #endregion

        #region _txtValor_TextChanged
        /// <summary>
        /// Evento de texto alterado da TextBox
        /// </summary>
        /// <param name="sender">O controle que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _txtValor_TextChanged(object sender, EventArgs e)
        {
            if (this.TipoSugestao == TipoListaSugestao.TextoMaiusculo)
                _txtValor.Text = _txtValor.Text.ToUpperInvariant();

            int dummy = 0;
            if (this.TipoSugestao == TipoListaSugestao.Numero)
                if (!int.TryParse(this._txtValor.Text, out dummy))
                    this._txtValor.Text = String.Empty;

            if (this.Blur != null)
                Blur(this, e);
            else
                ValueChanged(this, new ValueChangedArgs(this._txtValor.Text, this.Descricao));

            if (this.CausesValidation)
            {
                if (this.Page != null)
                {
                    if (String.IsNullOrEmpty(this.ValidationGroup))
                        this.Page.Validate();
                    else
                        this.Page.Validate(this.ValidationGroup);
                }
            }
        }
        #endregion
        #endregion
    }
}
