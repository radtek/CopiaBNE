using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Employer.Componentes.UI.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Configuration;
using Employer.Componentes.UI.Web.Interface;
using System.Drawing.Design;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente base para usar autocomplete com código e modal de busca.
    /// </summary>
    #pragma warning disable 1591
    public abstract class LocalizarBase : CompositeControl, IRequiredField, IMensagemErro // AjaxClientControlBase
    {
        #region Eventos
        /// <summary>
        /// Evento para atualizar os elementos de filtros da modal do autocomplete.
        /// </summary>
        public event ControlModalBuscaBase.PainelCustomizavel InformacaoFiltros
        {
            add { _ModalBusca.InformacaoFiltros += value; }
            remove { _ModalBusca.InformacaoFiltros -= value; }
        }

        /// <summary>
        /// Evento disparado ao clicar no botão de cadastro
        /// </summary>
        public event EventHandler CadastroClick
        {
            add { _AutoComplete.CadastroClick += value; }
            remove { _AutoComplete.CadastroClick -= value; }
        }
        #endregion

        #region Atritudos
        ControlAutoCompleteBase _AutoComplete = new ControlAutoCompleteBase { ID = "AutoComplete" };
        ControlModalBuscaBase _ModalBusca = new ControlModalBuscaBase { ID = "ModalBusca" };
        HiddenField _hdf = new HiddenField { ID = "hdnModal" };
        #endregion

        #region Propriedades

        #region ModalBusca

        #region ModoRenderizacao
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

        #region SemAutoTab
        /// <summary>
        /// Indica se deve desligar o AutoTabIndex somente para o componente.<br/>
        /// Valor padrão falso.
        /// </summary>
        public bool SemAutoTab
        {
            get { return AutoComplete.SemAutoTab; }
            set { AutoComplete.SemAutoTab = value; }
        }
        #endregion

        /// <summary>
        /// A janela modal de consulta
        /// </summary>
        public ControlModalBuscaBase ModalBusca
        {
            get 
            {
                EnsureChildControls();
                return _ModalBusca; 
            }
        }
        #endregion

        #region AutoComplete
        /// <summary>
        /// O campo de auto complete
        /// </summary>
        public ControlAutoCompleteBase AutoComplete
        {
            get 
            {
                EnsureChildControls();
                return _AutoComplete; 
            }
        }
        #endregion

        #region AutoWidth
        /// <summary>
        /// A largura do campo de auto complete
        /// </summary>
        public Unit AutoWidth
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.Width; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.Width = value; 
            }
        }
        #endregion

        #region AutoHeight
        /// <summary>
        /// A altura do campo de auto complete
        /// </summary>
        public Unit AutoHeight
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.Height; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.Height = value; 
            }
        }
        #endregion

        #region AutoColumns
        /// <summary>
        /// A quantidade de colunas do campo de auto complete
        /// </summary>
        public int AutoColumns
        {
            get
            {
                EnsureChildControls();
                return AutoComplete.Columns; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.Columns = value; 
            }
        }
        #endregion

        #region ModalWidth
        /// <summary>
        /// A largura da modal de consulta
        /// </summary>
        public Unit ModalWidth
        {
            get { return ModalBusca.Width; }
            set { ModalBusca.Width = value; }
        }
        #endregion

        #region ModalHeight
        /// <summary>
        /// A altura da modal de consulta
        /// </summary>
        public Unit ModalHeight
        {
            get { return ModalBusca.Height; }
            set { ModalBusca.Height = value; }
        }
        #endregion

        #region ModalBuscaWidth
        /// <summary>
        /// A largura do campo texto da modal de consulta
        /// </summary>
        public Unit ModalBuscaWidth
        {
            get { return ModalBusca.BuscaWidth; }
            set { ModalBusca.BuscaWidth = value; }
        }
        #endregion

        #region Texto
        /// <summary>
        /// O texto atualmente no auto complete
        /// </summary>
        public string Texto
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.Text; 
            }
            set 
            {
                EnsureChildControls();
                AutoComplete.Text = value; 
            }
        }
        #endregion

        #region LabelVisivel
        /// <summary>
        /// Define a visibilidade do label do auto complete
        /// </summary>
        public bool LabelVisivel
        {
            get { return AutoComplete.LabelVisivel; }
            set { AutoComplete.LabelVisivel = value; }
        }
        #endregion

        #region LabelTexto
        /// <summary>
        /// Define o texto do label do auto complete
        /// </summary>
        public string LabelTexto
        {
            get { return AutoComplete.LabelTexto; }
            set { AutoComplete.LabelTexto = value; }
        }
        #endregion

        #region LabelCssBotaoPesquisarModal
        /// <summary>
        /// Define o texto do label de busca da modal
        /// </summary>
        public string LabelCssBotaoPesquisarModal
        {
            get { return _ModalBusca.BotaoPesquisarCss; }
            set { _ModalBusca.BotaoPesquisarCss = value; }
        }
        #endregion

        #region TextBoxCssPesquisarModal
        /// <summary>
        /// Define o texto do textBox de pesquisa da modal
        /// </summary>
        public string TextBoxCssPesquisarModal
        {
            get { return _ModalBusca.BuscaCss; }
            set { _ModalBusca.BuscaCss = value; }
        }
        #endregion

        #region LinhaCSSPesquisaModal
        /// <summary>
        /// Insere o css da linha de pesquisa da modal TextBox + Botao
        /// </summary>
        public string LinhaCSSPesquisaModal
        {
            get { return _ModalBusca.LinhaCSSPesquisaModal; }
            set { _ModalBusca.LinhaCSSPesquisaModal = value; }
        }
        #endregion

        #region DivCSSPesquisaModal
        /// <summary>
        /// Insere o css da linha de pesquisa da modal TextBox + Botao
        /// </summary>
        public string DivCSSPesquisaModal
        {
            get { return _ModalBusca.BotoesTopoCss; }
            set { _ModalBusca.BotoesTopoCss = value; }
        }
        #endregion

        #region DivInformacaoFiltrosCss
        /// <summary>
        /// Classe css da div do painel de informação da modal
        /// </summary>
        public string DivInformacaoFiltrosCss
        {
            get { return ModalBusca.DivInformacaoFiltrosCss; }
            set { ModalBusca.DivInformacaoFiltrosCss = value; }
        }
        #endregion

        #region Obrigatorio
        /// <summary>
        /// Define se o campo é obrigatório ou não
        /// </summary>
        public bool Obrigatorio
        {
            get
            {
                EnsureChildControls();
                if (ViewState["Obrigatorio"] == null)
                    return false;
                return (Boolean)ViewState["Obrigatorio"];
            }
            set 
            {
                EnsureChildControls();

                ViewState["Obrigatorio"] = value;
                _AutoComplete.Obrigatorio = value;
            }
        }
        #endregion 

        #region ValidationGroup
        /// <summary>
        /// Define o grupo de validação
        /// </summary>
        public string ValidationGroup
        {
            get 
            {
                EnsureChildControls();
                return AutoComplete.ValidationGroup; 
            }
            set
            {
                EnsureChildControls();
                AutoComplete.ValidationGroup = value; 
            }
        }
        #endregion 

        #region ValorAlteradoClient
        /// <summary>
        /// Evento javascript disparado quando o conteúdo do controle muda
        /// </summary>
        public string ValorAlteradoClient
        {
            get { return AutoComplete.ValorAlteradoClient; }
            set { AutoComplete.ValorAlteradoClient = value; }
        }
        #endregion 

        #region ValorAlterado
        /// <summary>
        /// Evento de valor alterado
        /// </summary>
        public event EventHandler ValorAlterado
        {
            add { AutoComplete.ValorAlterado += value; }
            remove { AutoComplete.ValorAlterado -= value; }
        }
        #endregion 

        #region AutoCustomServerValidate
        /// <summary>
        /// Evento disparado pelo custom validator
        /// </summary>
        public event ServerValidateEventHandler AutoCustomServerValidate
        {
            add { AutoComplete.CustomServerValidate += value; }
            remove { AutoComplete.CustomServerValidate -= value; }
        }
        #endregion 

        #region AutoCustomClientValidate
        /// <summary>
        /// Evento cliente disparado pelo custom validator
        /// </summary>
        public string AutoCustomClientValidate
        {
            get { return AutoComplete.CustomClientValidate; }
            set { AutoComplete.CustomClientValidate = value; }
        }
        #endregion 

        #region Cadastro
        /// <summary>
        /// Endereço da tela de cadastro.
        /// </summary>
        public string CadastroUrl
        {
            get { return this.AutoComplete.CadastroUrl; }
            set { this.AutoComplete.CadastroUrl = value; }
        }

        /// <summary>
        /// Nome do parâmetro de sessão para passar o valor informado no campo texto do autocomplete para e tela de cadastro.
        /// </summary>
        public string CadastroParametroSessao
        {
            get { return this.AutoComplete.CadastroParametroSessao; }
            set { this.AutoComplete.CadastroParametroSessao = value; }
        }

        /// <summary>
        /// Nome do parâmetro da url para passar o valor informado no campo texto do autocomplete para e tela de cadastro.
        /// </summary>
        public string CadastroParametroUrl
        {
            get { return this.AutoComplete.CadastroParametroUrl; }
            set { this.AutoComplete.CadastroParametroUrl = value; }
        }

        /// <summary>
        /// Nome do parâmetro de sessão para passar do código informado no autocomplete para a tela de cadastro.
        /// </summary>
        public string CadastroParametroSessaoCodigo
        {
            get { return this.AutoComplete.CadastroParametroSessaoCodigo; }
            set { this.AutoComplete.CadastroParametroSessaoCodigo = value; }
        }

        /// <summary>
        /// Nome do parâmetro da url para passar do código informado no autocomplete para a tela de cadastro.
        /// </summary>
        public string CadastroParametroUrlCodigo
        {
            get { return this.AutoComplete.CadastroParametroUrlCodigo; }
            set { this.AutoComplete.CadastroParametroUrlCodigo = value; }
        }

        /// <summary>
        /// Caminho da imagem para o botão que ativa a modal de busca
        /// </summary>
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Bindable(true)]
        public string ImagemBotaoCadastro
        {
            get { return this.AutoComplete.CadastroBotao.ImageUrl; }
            set { this.AutoComplete.CadastroBotao.ImageUrl = value; }
        }

        /// <summary>
        /// Classe Css do botão que ativa a modal de busca
        /// </summary>
        public string CssBotaoCadastro
        {
            get { return this.AutoComplete.CadastroBotao.CssClass; }
            set { this.AutoComplete.CadastroBotao.CssClass = value; }
        }
        #endregion

        #region SkinIDModalBusca
        /// <summary>
        /// ID do Skin da modal de pesquisa
        /// </summary>
        [Browsable(true)]
        [Themeable(true)]
        public string SkinIDModalBusca
        {
            get { return ModalBusca.SkinID; }
            set { ModalBusca.SkinID = value; }
        }
        #endregion 

        #region SkinIDAutoComplete
        /// <summary>
        /// ID do Skin do auto complete
        /// </summary>
        [Browsable(true)]
        [Themeable(true)]
        public string SkinIDAutoComplete
        {
            get { return AutoComplete.SkinID; }
            set { AutoComplete.SkinID = value; }
        }
        #endregion 

        #region GridSkinID
        /// <summary>
        /// ID do Skin da grid de pesquisa
        /// </summary>
        [Browsable(true)]
        [Themeable(true)]
        public string GridSkinID
        {
            get { return ModalBusca.SkinID; }
            set { ModalBusca.SkinID = value; }
        }
        #endregion 

        #region BotaoBuscaVisivel
        /// <summary>
        /// Define a visibilidade do botão de busca
        /// </summary>
        public Boolean BotaoBuscaVisivel
        {
            get 
            {
                EnsureChildControls();
                return _AutoComplete.BotaoBuscaVisivel; 
            }
            set 
            {
                EnsureChildControls();
                this._AutoComplete.BotaoBuscaVisivel = value; 
            }
        }
        #endregion 

        #region MensagemErroFormato
        /// <summary>
        /// Define a mensagem de erro de formato inválido
        /// </summary>
        public String MensagemErroFormato
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroFormato;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroFormato = value;
            }
        }
        #endregion

        #region MensagemErroObrigatorio
        /// <summary>
        /// Define a mensagem de erro de campo obrigatório
        /// </summary>
        public String MensagemErroObrigatorio
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroObrigatorio;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroObrigatorio = value;
            }
        }
        #endregion

        #region MensagemErroFormatoSummary
        /// <summary>
        /// Define a mensagem de erro de formato inválido
        /// </summary>
        public String MensagemErroFormatoSummary
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroFormatoSummary;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroFormatoSummary = value;
            }
        }
        #endregion

        #region MensagemErroObrigatorioSummary
        /// <summary>
        /// Define a mensagem de erro de campo obrigatório
        /// </summary>
        public String MensagemErroObrigatorioSummary
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroObrigatorioSummary;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroObrigatorioSummary = value;
            }
        }
        #endregion

        #region MensagemErroInvalidoSummary
        /// <summary>
        /// Sem funcionalidade. Obrigado a implementar por causa da Interface.
        /// </summary>
        public String MensagemErroInvalidoSummary
        {
            get
            {
                EnsureChildControls();
                return this.AutoComplete.MensagemErroInvalidoSummary;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.MensagemErroInvalidoSummary = value;
            }
        }
        #endregion

        #region CssTextBoxAutoComplete
        /// <summary>
        /// Altera o CSS do textbox autocomplete.
        /// </summary>
        public String CssTextBoxAutoComplete
        {
            set
            {
                EnsureChildControls();
                _AutoComplete.CampoTexto.CssClass = value;
            }
        }
        #endregion

        #region Width
        public override Unit Width
        {
            get
            {
                EnsureChildControls();
                return AutoComplete.CampoTexto.Width;
            }
            set
            {
                EnsureChildControls();
                AutoComplete.CampoTexto.Width = value;
            }
        }
        #endregion

        #region TabIndex
        /// <inheritdoc/>
        public override short TabIndex
        {
            get
            {
                EnsureChildControls();
                return _AutoComplete.TabIndex;
            }
            set
            {
                EnsureChildControls();
                _AutoComplete.TabIndex = value;
            }
        }
        #endregion

        #region Enabled
        /// <inheritdoc/>
        public override bool Enabled
        {
            get
            {
                EnsureChildControls();
                return _AutoComplete.Enabled;
            }
            set
            {
                EnsureChildControls();
                _AutoComplete.Enabled = value;
                //base.Enabled = value;
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
                return _AutoComplete.ReadOnly; 
            }
            set
            {
                EnsureChildControls();
                _AutoComplete.ReadOnly = value;
            }
        }
        #endregion

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
                return this.AutoComplete.Primeiro;
            }
            set
            {
                EnsureChildControls();
                this.AutoComplete.Primeiro = value;
            }
        }
        #endregion

        #region IContemTextBox
        /// <summary>
        /// TextBox do componente
        /// </summary>
        public TextBox CampoTexto
        {
            get
            {
                EnsureChildControls();
                return this._AutoComplete.CampoTexto;
            }
        }
        #endregion

        #endregion

        #region Construtor
        /// <inheritdoc/>
        public LocalizarBase()
        {
            _AutoComplete.PosCreateChildControls += new EventHandler(_AutoComplete_PosCreateChildControls);
            _ModalBusca.PosCreateChildControls += new EventHandler(_ModalBusca_PosCreateChildControls);
        }
        #endregion

        #region Metodos        

        #region Focus
        /// <inheritdoc />
        public override void Focus()
        {
            AutoComplete.Focus();
        }
        #endregion 

        #region ExecutarValorAlterado
        public void ExecutarValorAlterado()
        {
            this.AutoComplete.ExecutarValorAlterado();

        }
        #endregion

        #region LimparCampos
        /// <summary>
        /// Limpa o campo do auto complete
        /// </summary>
        public void LimparCampos()
        {
            AutoComplete.Codigo = string.Empty;
            AutoComplete.Text = string.Empty;
        }
        #endregion 

        #region CreateChildControls
        /// <inheritdoc />
        protected override void CreateChildControls()
        {            
            this.Controls.Add(_AutoComplete);
            
            this.Controls.Add(_ModalBusca);
            
            this.Controls.Add(_hdf);

            //_AutoComplete.AutoPostBack = true;

            base.CreateChildControls();
        }
        #endregion 

        #region GetKey
        /// <summary>
        /// Retorna a campo chave a partir de um objeto da fonte de dados
        /// </summary>
        /// <param name="DataItem">O objeto da fonte de dados</param>
        /// <returns>Um objeto representado o campo chave na fonte de dados</returns>
        public abstract object GetKey(object DataItem);
        #endregion 

        #region GetValue
        /// <summary>
        /// Retorna o campo valor a partir de um objeto da fonte de dados
        /// </summary>
        /// <param name="DataItem">O objeto da fonte de dados</param>
        /// <returns>Um objeto representando o campo valor na fonte de dados</returns>
        public abstract object GetValue(object DataItem);
        #endregion 

        #region LoadByKey
        /// <summary>
        /// Carrega um registro a partir do código informado ao autocomplete
        /// </summary>
        protected abstract void LoadByKey();
        #endregion

        #region OnInit
        /// <inheritdoc />
        protected override void OnInit(EventArgs e)
        {
            _ModalBusca.AtualizaLinhasGrid += new GridViewRowEventHandler(_ModalBusca_AtualizaLinhasGrid);
            _AutoComplete.CarregarPorCodigo += new EventHandler(_AutoComplete_CarregarPorCodigo);

            if (this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
                _AutoComplete.BotaoBusca.Click += new ImageClickEventHandler(BotaoBusca_Click);
            else
                _AutoComplete.LinkBusca.Click += LinkBusca_Click;

            base.OnInit(e);
        }        
        #endregion 

        protected override void Render(HtmlTextWriter writer)
        {
            if (DesignMode)
                writer.Write("Componente LocalizarBase - "+this.ID);
            else
                base.Render(writer);
        }

        #region BotaoBusca_Click
        /// <summary>
        /// Tratador do evento disparado quando o usuário clica no botão que exibe a modal.  
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void BotaoBusca_Click(object sender, ImageClickEventArgs e)
        {
            _ModalBusca.Show();
        }

        void LinkBusca_Click(object sender, EventArgs e)
        {
            _ModalBusca.Show();
        }
        #endregion

        #region _AutoComplete_CarregarPorCodigo
        /// <summary>
        /// Tratador do evento disparado disparado ao informar o código do ítem a ser carregado.  
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _AutoComplete_CarregarPorCodigo(object sender, EventArgs e)
        {
            this.LoadByKey();
        }
        #endregion         

        #region _ModalBusca_AtualizaLinhasGrid
        /// <summary>
        /// Tratador do evento disparado quando está atualizando as linhas da grid.  
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _ModalBusca_AtualizaLinhasGrid(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                object oKey = this.GetKey(e.Row.DataItem);
                string sValue = System.Web.HttpUtility.HtmlEncode(this.GetValue(e.Row.DataItem).ToString());

                foreach (TableCell c in e.Row.Cells)
                {
                    c.Attributes["onclick"] = string.Format("javascript:$find('{0}').CarregarPorCodigo('{1}','{2}')",
                    _AutoComplete.ClientID,
                    oKey, sValue);
                }
            }
        }
        #endregion 

        #region _ModalBusca_PosCreateChildControls
        /// <summary>
        /// Tratador do evento disparado disparado após criar os controles filhos da modal de pesquisa.  
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _ModalBusca_PosCreateChildControls(object sender, EventArgs e)
        {
            _AutoComplete.IdModalAjax = _ModalBusca.Modal.ClientID;
            _AutoComplete.IdModalEstado = _ModalBusca.IdModalEstado;
        }
        #endregion 

        #region _AutoComplete_PosCreateChildControls
        /// <summary>
        /// Tratador do evento disparado disparado após criar os controles filhos do campo auto complete.  
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        private void _AutoComplete_PosCreateChildControls(object sender, EventArgs e)
        {
            ModalBusca.TargetControlID = _hdf.ID;
        }
        #endregion

        #endregion

        
    }
}
