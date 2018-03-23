using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using JSONSharp;

[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.GeralControle.js", "text/javascript")]
[assembly: WebResource("Employer.Plataforma.Web.Componentes.js.AlfaNumerico.js", "text/javascript")]

namespace Employer.Plataforma.Web.Componentes
{
    [ToolboxData("<{0}:ListaSugestoes runat=server Obrigatorio=False ExibirSugestao=True PosicaoSugestao=Direita Tipo=Numerico ></{0}:ListaSugestoes>")]
    public class ListaSugestoes : CompositeControl
    {
        #region Posição
        public enum Posicao
        {
            Abaixo = 1,
            Acima = 2,
            Direita = 3,
            Esquerda = 4
        }
        #endregion

        #region Atributos
        private TextBox _txtValor;
        private Label _lblValor;
        private GridView _gvValor;
        private RequiredFieldValidator _rfValor;
        private RegularExpressionValidator _reValor;
        private AlfaNumerico.TipoAlfaNumerico _tipo;

        // Define se registra ou não os JavaScripts referentes ao comportamento do controle
        // Foi adicionado devido ao comportamento padrão o quel está registrando diversas vezes,
        // muitas delas desnecessárias, causando uma lentidão no carregamento de diversas instancias
        // Eduardo Ordine
        private Boolean _loadScripts = true;
        
        #endregion

        #region Propriedades

        #region ReadOnly
        /// <summary>
        /// Define se o campo é ReadOnly.
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("ReadOnly")
        ]
        public bool ReadOnly
        {
            get
            {
                return _txtValor.ReadOnly;
            }
            set
            {
                _txtValor.ReadOnly = value;
            }
        }

        #endregion

        #region SetFocusOnError
        /// <summary>
        /// Define se o campo é SetFocusOnError.
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("SetFocusOnError")
        ]
        public bool SetFocusOnError
        {
            get
            {
                return _reValor.SetFocusOnError;
            }
            set
            {
                _reValor.SetFocusOnError = value;
            }
        }

        #endregion

        #region Validation Group
        /// <summary>
        /// ValidationGroup
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Validation Group")
        ]
        public string ValidationGroup
        {
            get
            {
                return _reValor.ValidationGroup;
            }
            set
            {
                _rfValor.ValidationGroup = value;
                _reValor.ValidationGroup = value;
            }
        }
        #endregion

        #region HeaderValue
        /// <summary>
        /// Header do BoundField do Código
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Header Value"),
            Localizable(true)
        ]
        public string HeaderValue
        {
            get;
            set;
        }
        #endregion

        #region HeaderText
        /// <summary>
        /// Header do BoundField da Descrição
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Header Text"),
            Localizable(true)
        ]
        public string HeaderText
        {
            get
            {
                if (ViewState["HeaderText"] == null)
                    return null;
                return ViewState["HeaderText"].ToString();
            }
            set
            {
                ViewState["HeaderText"] = value;
            }
        }
        #endregion

        #region Obrigatorio
        /// <summary>
        /// Define se o Campo é obrigatório.
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Obrigatório")
        ]
        public bool Obrigatorio
        {
            get
            {
                return _rfValor.Enabled;
            }
            set
            {
                _rfValor.Enabled = value;
            }
        }
        #endregion

        #region Mensagem Erro Formato
        /// <summary>
        /// Mensagem de Erro Formato
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Mensagem Erro Formato"),
            Localizable(true)
        ]
        public string MensagemErroFormato
        {
            get
            {
                return _reValor.ErrorMessage;
            }
            set
            {
                _reValor.ErrorMessage = value;
            }
        }

        #endregion

        #region Mensagem Erro Obrigatorio
        /// <summary>
        /// Mensagem de Erro Obrigatorio
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Mensagem Erro Obrigatorio")
        ]
        public string MensagemErroObrigatorio
        {
            get
            {
                return _rfValor.ErrorMessage;
            }
            set
            {
                _rfValor.ErrorMessage = value;
            }
        }

        #endregion

        #region Mensagem Vazio
        /// <summary>
        /// Mensagem de Vazio
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Mensagem Vazio"),
            Localizable(true)
        ]
        public string MensagemVazio
        {
            get
            {
                if (ViewState["MensagemVazio"] == null)
                    return string.Empty;
                return ViewState["MensagemVazio"].ToString();
            }
            set
            {
                ViewState["MensagemVazio"] = value;
            }
        }

        #endregion

        #region TextBox - Columns
        /// <summary>
        /// TextBox
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Columns")
        ]
        public int Columns
        {
            get { return _txtValor.Columns; }
            set
            {
                _txtValor.Columns = value;
            }
        }
        #endregion

        #region TextBox - CssClass
        /// <summary>
        /// Classe CSS do TextBox
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass TextBox")
        ]
        public string CssClassTextBox
        {
            get
            {
                return _txtValor.CssClass;
            }
            set
            {
                _txtValor.CssClass = value;
            }
        }
        #endregion

        #region Label - CssClass
        /// <summary>
        /// Classe CSS do Label
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass Label")
        ]
        public string CssClassLabel
        {
            get
            {
                return _lblValor.CssClass;
            }
            set
            {
                _lblValor.CssClass = value;
            }
        }
        #endregion

        #region Descricao
        /// <summary>
        /// Descricao
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Descricao")
        ]
        public string Descricao
        {
            get
            {
                return _lblValor.Text;
            }
        }
        #endregion

        #region ExibirDescricao
        /// <summary>
        /// Exibir descrição
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Exibir Descrição")
        ]
        public bool ExibirDescricao
        {
            get
            {
                return _lblValor.Enabled;
            }
            set
            {
                _lblValor.Enabled = value;
            }
        }
        #endregion

        #region GridView - CssClass
        /// <summary>
        /// Classe CSS do GridView
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass GridView")
        ]
        public string CssClassGridView
        {
            get
            {
                return _gvValor.CssClass;
            }
            set
            {
                _gvValor.CssClass = value;
            }
        }
        #endregion

        #region GridView Header Value - CssClass
        /// <summary>
        /// Classe CSS do GridView Código
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass Header Value")
        ]
        public string CssClassHeaderValue
        {
            get;
            set;
        }
        #endregion

        #region GridView Header Text - CssClass
        /// <summary>
        /// Classe CSS do GridView Texto
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass Header Text")
        ]
        public string CssClassHeaderText
        {
            get;
            set;
        }
        #endregion

        #region GridView Códgio - CssClass
        /// <summary>
        /// Classe CSS do GridView Código
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass Código")
        ]
        public string CssClassCodigo
        {
            get;
            set;
        }
        #endregion

        #region GridView Descrição - CssClass
        /// <summary>
        /// Classe CSS do GridView Descrição
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass Descrição")
        ]
        public string CssClassDescricao
        {
            get;
            set;
        }
        #endregion

        #region GridView Vazio - CssClass
        /// <summary>
        /// Classe CSS do GridView Vazio
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass Vazio")
        ]
        public string CssClassVazio
        {
            get;
            set;
        }
        #endregion

        #region RequiredField - CssClass
        /// <summary>
        /// CssClass do RequiredField
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass RequiredField")
        ]
        public string CssClassRequiredField
        {
            get
            {
                return _rfValor.CssClass;
            }
            set
            {
                _rfValor.CssClass = value;
            }
        }
        #endregion

        #region RegularExpression - CssClass
        /// <summary>
        /// CssClass do CustomValidator
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("CssClass RegularExpression")
        ]
        public string CssClassRegularExpression
        {
            get
            {
                return _reValor.CssClass;
            }
            set
            {
                _reValor.CssClass = value;
            }
        }
        #endregion

        #region ExibirSugestao
        /// <summary>
        /// Exibir sugestão
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Exibir Sugestão")
        ]
        public bool ExibirSugestao
        {
            get
            {
                return _gvValor.Enabled;
            }
            set
            {
                _gvValor.Enabled = value;
            }
        }
        #endregion

        #region PosicaoSugestao
        /// <summary>
        /// Posição sugestão
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Posição da Sugestão"),
            Localizable(false)
        ]
        public Posicao PosicaoSugestao
        {
            get;
            set;
        }
        #endregion

        #region Tipo Campo
        /// <summary>
        /// Define o tipo do campo
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Tipo")
        ]
        public AlfaNumerico.TipoAlfaNumerico Tipo
        {
            get
            { return _tipo; }
            set
            { _tipo = value; }
        }
        #endregion

        #region Valor
        /// <summary>
        /// Get e Set do Componente
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("Valor")
        ]
        public string Valor
        {
            get
            {
                return _txtValor.Text;
            }
            set
            {
                _txtValor.Text = value;
                if (_lblValor.Enabled)
                {
                    _lblValor.Text = string.Empty;
                    if (!string.IsNullOrEmpty(value))
                    {
                        foreach (GridViewRow row in _gvValor.Rows)
                        {
                            if (row.Cells[0].Text.ToUpper().Equals(value.ToUpper()))
                            {
                                _lblValor.Text = row.Cells[1].Text;
                                break;
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region Dicionario
        /// <summary>
        /// Dictionary
        /// </summary>
        [Browsable(false)]
        public Dictionary<string, string> Dicionario
        {
            set
            {
                _gvValor.DataSource = value;
            }
        }
        #endregion

        #region ValorAlteradoClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("ValorAlteradoClient")
        ]
        public string ValorAlteradoClient
        {
            get;
            set;
        }
        #endregion

        #region OnFocusClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("OnFocusClient")
        ]
        public string OnFocusClient
        {
            get;
            set;
        }
        #endregion

        #region OnBlurClient
        /// <summary>
        /// Função javascript que deve ser executada ao alterar o valor.
        /// </summary>
        [
            Category("Employer - Lista de Sugestões"),
            DisplayName("OnBlurClient")
        ]
        public string OnBlurClient
        {
            get;
            set;
        }
        #endregion

        #region LoadScripts

        /// <summary>
        /// Define se registra ou não os JavaScripts referentes ao comportamento do controle
        /// Foi adicionado devido ao comportamento padrão o quel está registrando diversas vezes,
        /// muitas delas desnecessárias, causando uma lentidão no carregamento de diversas instancias
        /// </summary>
        /// <remarks>Eduardo Ordine</remarks>
        [Category("Employer - Valor Decimal"),
         DisplayName("LoadScripts"),
         DefaultValue("true")]
        public bool LoadScripts
        {
            get
            {
                return _loadScripts;
            }
            set
            {
                _loadScripts = value;
            }
        }

        #endregion

        #endregion

        #region Construtor
        public ListaSugestoes()
        {
            _txtValor = new TextBox();
            _lblValor = new Label();
            _gvValor = new GridView();
            _rfValor = new RequiredFieldValidator();
            _reValor = new RegularExpressionValidator();
        }
        #endregion

        #region Métodos

        #region AplicarAcoesComponente
        /// <summary>
        /// Método que instância o objeto de argumentos e registra a função
        /// valor alterado para ser invocada no Client.
        /// </summary>
        private void AplicarAcoesComponente()
        {
            Args args = new Args();
            args.NomeCampoValor = _txtValor.ClientID;
            List<string> nomeValidadores = new List<string>();
            if (_rfValor.Enabled)
                nomeValidadores.Add(_rfValor.ClientID);
            nomeValidadores.Add(_reValor.ClientID);
            args.Validadores = nomeValidadores.ToArray();

            _txtValor.Attributes.Add("OnKeyUp", "moveToNextElement(this, event, maxLength);");

            if (_lblValor.Enabled)
                _txtValor.Attributes.Add("OnFocus", "RemoverMascaraControle(this,null);ExibirComponenteInLine(this,'" + _lblValor.ClientID + "',false);");

            if (!String.IsNullOrEmpty(OnFocusClient))
                _txtValor.Attributes["OnFocus"] += OnFocusClient + "(" + new JSONReflector(args) + ");";

            if (_lblValor.Enabled)
            {
                _txtValor.Attributes.Add("OnBlur", "ExibirDescricao(this,'" + _lblValor.ClientID + "','" + _gvValor.ClientID + "');");
                _txtValor.Attributes["OnBlur"] += "ExibirComponenteInLine(this,'" + _lblValor.ClientID + "',true);";
            }

            if (_gvValor.Enabled)
            {
                _txtValor.Attributes["OnFocus"] += "ExibirComponente(this,'" + _gvValor.ClientID + "',true," + PosicaoSugestao.GetHashCode() + ");";
                _txtValor.Attributes["OnBlur"] += "ExibirComponente(this,'" + _gvValor.ClientID + "',false);";
            }

            _txtValor.Attributes["OnBlur"] += "ValidarAutomatico(" + new JSONReflector(args) + ");";

            if (!String.IsNullOrEmpty(ValorAlteradoClient))
                _txtValor.Attributes["OnChange"] += "ClientFunction(this," + ValorAlteradoClient + "," + new JSONReflector(args) + ");"; 

            if (ValorAlterado != null)
                _txtValor.Attributes["OnChange"] += "DoPostBack(this," + new JSONReflector(args) + ");";

            if (String.IsNullOrEmpty(this._txtValor.Attributes["OnBlur"]))
            {
                _txtValor.Attributes["OnBlur"] = "AplicarMascaraAlfaNumerico(this," + _tipo.GetHashCode() + "," + this._txtValor.MaxLength + ");";
            }

            if (this._txtValor.Attributes["OnBlur"].IndexOf("AplicarMascaraAlfaNumerico") == -1)
            {
                _txtValor.Attributes["OnBlur"] = "AplicarMascaraAlfaNumerico(this," + _tipo.GetHashCode() + "," + this._txtValor.MaxLength + ");" + _txtValor.Attributes["OnBlur"];
            }

            if (!String.IsNullOrEmpty(OnBlurClient))
                _txtValor.Attributes["OnBlur"] += OnBlurClient + "(" + new JSONReflector(args) + ");";
        }
        #endregion

        #region InicializarTextBox
        /// <summary>
        /// Inicializa o TextBox
        /// </summary>
        protected virtual void InicializarTextBox()
        {
            _txtValor.ID = "txtValor";
            _txtValor.AutoCompleteType = AutoCompleteType.Disabled;
        }


        #endregion

        #region InicializarLabel
        /// <summary>
        /// Inicializa o Label
        /// </summary>
        protected virtual void InicializarLabel()
        {
            _lblValor.ID = "lblValor";
        }
        #endregion

        #region InicializarGridView
        /// <summary>
        /// Inicializa o GridView
        /// </summary>
        protected virtual void InicializarGridView()
        {
            _gvValor.ID = "grvSugestoes";
            _gvValor.AutoGenerateColumns = false;
            _gvValor.GridLines = GridLines.None;
            _gvValor.ShowHeader = false;
            _gvValor.EmptyDataRowStyle.CssClass = CssClassVazio;
            _gvValor.EmptyDataText = string.IsNullOrEmpty(MensagemVazio) ? "Não há itens." : MensagemVazio;    
            _gvValor.Columns.Clear();

            BoundField bfCodigo = new BoundField();
            bfCodigo.DataField = "key";
            bfCodigo.ItemStyle.CssClass = CssClassCodigo;
            if (string.IsNullOrEmpty(_gvValor.EmptyDataRowStyle.CssClass))
                _gvValor.EmptyDataRowStyle.CssClass = CssClassCodigo;
            if (!string.IsNullOrEmpty(HeaderValue) || !string.IsNullOrEmpty(HeaderText))
            {
                bfCodigo.HeaderText = HeaderValue;
                bfCodigo.HeaderStyle.CssClass = CssClassHeaderValue;
                if (string.IsNullOrEmpty(_gvValor.EmptyDataRowStyle.CssClass))
                    _gvValor.EmptyDataRowStyle.CssClass = CssClassHeaderValue;
                _gvValor.ShowHeader = true;
            }
            _gvValor.Columns.Add(bfCodigo);

            BoundField bfDescricao = new BoundField();
            bfDescricao.DataField = "value";
            bfDescricao.ItemStyle.CssClass = CssClassDescricao;
            if (string.IsNullOrEmpty(_gvValor.EmptyDataRowStyle.CssClass))
                _gvValor.EmptyDataRowStyle.CssClass = CssClassDescricao;
            if (!string.IsNullOrEmpty(HeaderText) || !string.IsNullOrEmpty(HeaderValue))
            {
                bfDescricao.HeaderText = HeaderText;
                bfDescricao.HeaderStyle.CssClass = CssClassHeaderText;
                _gvValor.ShowHeader = true;
            }
            _gvValor.Columns.Add(bfDescricao);
        }
        #endregion

        #region InicializarRequiredField
        /// <summary>
        /// Inicializar RequiredField
        /// </summary>
        protected virtual void InicializarRequiredField()
        {
            _rfValor.ID = "rfValor";
            _rfValor.ForeColor = Color.Empty;
            _rfValor.ControlToValidate = _txtValor.ID;
            _rfValor.Display = ValidatorDisplay.Dynamic;
            _rfValor.SetFocusOnError = false;
        }
        #endregion

        #region InicializarRegularExpression
        /// <summary>
        /// Método de configuração das propriedades da Regular Expression.
        /// </summary>
        private void InicializarRegularExpression()
        {
            _reValor.ID = "reValor";
            _reValor.ForeColor = Color.Empty;
            _reValor.ControlToValidate = _txtValor.ID;
            _reValor.Display = ValidatorDisplay.Dynamic;
        }
        #endregion

        #endregion

        #region Eventos

        #region ValorAlterado
        /// <summary>
        /// Evento ao alterar valor
        /// </summary>
        public event EventHandler ValorAlterado;
        #endregion

        #region OnInit
        /// <summary>
        /// Especificar ao iniciar os javascripts a serem utilizados
        /// </summary>
        /// <param name="e">Argumento do evento</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Define se registra ou não os JavaScripts referentes ao comportamento do controle
            // Foi adicionado devido ao comportamento padrão o quel está registrando diversas vezes,
            // muitas delas desnecessárias, causando uma lentidão no carregamento de diversas instancias
            // ATENÇÃO! Estes IFs a seguir não adiantam em nada, pois eles estão retornando sempre 'false'
            // o que acaba fazendo o controle registrar um .JS para cada instancia do controle.
            // Em caso de dúvida ou descrédito, coloque um Breakpoint aqui e verifique!
            // Eduardo Ordine
            if (!LoadScripts)
                return;

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("GeralControle.js"))
                Page.ClientScript.RegisterClientScriptInclude("GeralControle.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.GeralControle.js"));

            if (!Page.ClientScript.IsClientScriptIncludeRegistered("AlfaNumerico.js"))
                Page.ClientScript.RegisterClientScriptInclude("AlfaNumerico.js", Page.ClientScript.GetWebResourceUrl(GetType(), "Employer.Plataforma.Web.Componentes.js.AlfaNumerico.js"));

            _gvValor.Style["display"] = "none";
        }
        protected override void CreateChildControls()
        {
            InicializarTextBox();
            InicializarGridView();
            InicializarLabel();
            InicializarRegularExpression();
            InicializarRequiredField();

            Panel pnlValidador = new Panel();
            pnlValidador.ID = "pnlValidador";
            pnlValidador.Controls.Add(_rfValor); //RequiredField
            pnlValidador.Controls.Add(_reValor); //RegularExpression
            Controls.Add(pnlValidador);
            Controls.Add(_txtValor);
            Controls.Add(_lblValor);
            Controls.Add(_gvValor);

            
            base.CreateChildControls();
        }
        #endregion

        #region OnPreRender
        /// <summary>
        /// Rederizar os componentes
        /// </summary>
        /// <param name="e">Argumento do evento</param>
        protected override void OnPreRender(EventArgs e)
        {
            AplicarAcoesComponente();

            if (this.ExibirDescricao)
            {
                _lblValor.Text = string.Empty;
                if (!string.IsNullOrEmpty(this._txtValor.Text))
                {
                    foreach (GridViewRow row in _gvValor.Rows)
                    {
                        if (row.Cells[0].Text.ToUpper().Equals(this._txtValor.Text.ToUpper()))
                        {
                            _lblValor.Text = row.Cells[1].Text;
                            break;
                        }
                    }
                }
            }

            base.OnPreRender(e);

            string eventArgument = Page.Request.Params["__EVENTARGUMENT"];
            if (!string.IsNullOrEmpty(eventArgument) && eventArgument.Equals(_txtValor.ClientID))
                _txtValor_TextChanged(_txtValor, null);
        }
        #endregion

        #region DataBind
        /// <summary>
        /// Atualizar controles
        /// </summary>
        public override void DataBind()
        {
            base.DataBind();

            _gvValor.DataBind();

            StringBuilder valores = new StringBuilder();
            int maxLength = 1;
            if (!_gvValor.Rows.Count.Equals(0))
            {
                List<object> items = new List<object>();
                foreach (GridViewRow row in _gvValor.Rows)
                    items.Add(row.Cells[0].Text);

                items.Sort();

                for (int i = items.Count - 1; i >= 0; i--)
                {
                    string key = items[i].ToString();
                    valores.Append("|(");

                    valores.Append(key[0]);
                    if (key.Length > 1)
                    {
                        if (key.Length > maxLength)
                            maxLength = key.Length;
                        for (int j = 1; j < key.Length; j++)
                        {
                            valores.Append('[');
                            valores.Append(key[j]);
                            valores.Append(']');
                        }
                    }

                    valores.Append(")");
                }

                /*
                for (int row = _gvValor.Rows.Count - 1; row >= 0; row--)
                {
                    string key = _gvValor.Rows[row].Cells[0].Text;
                    valores.Append("|(");

                    valores.Append(key[0]);
                    if (key.Length > 1)
                    {
                        if (key.Length > maxLength)
                            maxLength = key.Length;
                        for (int i = 1; i < key.Length; i++)
                        {
                            valores.Append('[');
                            valores.Append(key[i]);
                            valores.Append(']');
                        }
                    }

                    valores.Append(")");
                }
                */
                
                if (valores.Length > 0)
                    valores.Remove(0, 1);
            }
            _reValor.ValidationExpression = valores.ToString();

            _txtValor.MaxLength = maxLength;
            if (_txtValor.Columns.Equals(0))
                _txtValor.Columns = maxLength;

            _txtValor.Attributes.Add("OnKeyPress", "return ApenasTeclasAlfaNumerico(this, event, " + maxLength + ", " + _tipo.GetHashCode() + ");");

            if (string.IsNullOrEmpty(_txtValor.Attributes["OnBlur"]) == false && _txtValor.Attributes["OnBlur"].Contains("AplicarMascaraAlfaNumerico("))
                _txtValor.Attributes["OnBlur"] = _txtValor.Attributes["OnBlur"].Substring(_txtValor.Attributes["OnBlur"].IndexOf(");") + 2);

            _txtValor.Attributes["OnBlur"] = "AplicarMascaraAlfaNumerico(this," + _tipo.GetHashCode() + "," + maxLength + ");" + _txtValor.Attributes["OnBlur"];

            if (!_txtValor.Attributes["OnBlur"].Contains("ExibirDescricao(") && _lblValor.Enabled)
                _txtValor.Attributes["OnBlur"] += string.Format("ExibirDescricao(this,'{0}','{1}');", _lblValor.ClientID, _gvValor.ClientID);
        }
        #endregion

        #region _txtValor_TextChanged
        /// <summary>
        /// Metodo invocado quando o texto eh alterado no server-side
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _txtValor_TextChanged(object sender, EventArgs e)
        {
            if (_lblValor.Enabled)
            {
                foreach (GridViewRow row in _gvValor.Rows)
                {
                    if (row.Cells[0].Text.ToUpper().Equals(_txtValor.Text.ToUpper()))
                    {
                        _lblValor.Text = row.Cells[1].Text;
                        break;
                    }
                }
            }

            ValorAlterado(sender, e);
        }
        #endregion

        #endregion
    }
}