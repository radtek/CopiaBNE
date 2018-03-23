using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Web.UI;
using System.Reflection;
using System.Web.UI.HtmlControls;
using Employer.Componentes.UI.Web.Extensions;
using System.Configuration;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Controle base de modal de pesquisa
    /// </summary>
    #pragma warning disable 1591
    public class ControlModalBuscaBase : CompositeControl
    {
        #region Eventos
        /// <summary>
        /// Descritor do evento de pesquisar 
        /// </summary>
        /// <param name="BtnSender">O botão que disparou a busca</param>
        /// <param name="DadosBusca">Os dados retornados pela busca</param>
        public delegate void HandlerPesquisar(System.Web.UI.WebControls.Button BtnSender, out object DadosBusca);

        /// <summary>
        /// Descritor de evento para modificar um painel
        /// </summary>
        /// <param name="painel"></param>
        public delegate void PainelCustomizavel(System.Web.UI.WebControls.Panel painel);

        /// <summary>
        /// Descritor de evento para modificar um Control
        /// </summary>
        /// <param name="container"></param>
        public delegate void HandlerContainer(Control container);

        /// <summary>
        /// Evento Pesquisar
        /// </summary>
        public event HandlerPesquisar Pesquisar;
        /// <summary>
        /// Evento que ocorre após a criação dos controles filhos
        /// </summary>
        public event EventHandler PosCreateChildControls;

        /// <summary>
        /// Evento invocado ao fechar a modal
        /// </summary>
        public event EventHandler Fechar;

        /// <summary>
        /// Evento invocado ao limpar a modal
        /// </summary>
        public event EventHandler Limpar;

        /// <summary>
        /// Evento para atualizar os elementos de filtros customizáveis de uma modal
        /// </summary>
        protected event PainelCustomizavel CriarFiltroCustomizavel;

        /// <summary>
        /// Evento para atualizar os elementos de filtros de uma modal.
        /// </summary>
        public event PainelCustomizavel InformacaoFiltros;

        public event HandlerContainer InformacaoPainelBotoesAntesBotaoLimpar;

        /// <summary>
        /// Evento de Atualização das linhas da grid
        /// </summary>
        public event GridViewRowEventHandler AtualizaLinhasGrid;
        #endregion

        #region Atributos
        System.Web.UI.WebControls.Panel _pnlModal = new System.Web.UI.WebControls.Panel();
        System.Web.UI.WebControls.Panel _pnlInfoFiltro = new System.Web.UI.WebControls.Panel { Visible = false };
        System.Web.UI.WebControls.Panel _pnlGrid = new System.Web.UI.WebControls.Panel();

        System.Web.UI.WebControls.Label _lblTitulo = new System.Web.UI.WebControls.Label();
        HtmlGenericControl _TituloH3 = new HtmlGenericControl("h3");

        ImageButton _btnFechar = new ImageButton();
        LinkButton _lnkFechar = new LinkButton();

        HiddenField _hdnFechar = new HiddenField() { ID = "hdnFechar" };
        HiddenField _hdnModalAberta = new HiddenField() { ID = "hdnModalAberta" };

        Label _lblLabel = new Label();
        TextBox _txtBusca = new TextBox();
        System.Web.UI.WebControls.Button _btnPesquisar = new System.Web.UI.WebControls.Button();
        EmployerGrid _GridBusca = new EmployerGrid();
        System.Web.UI.WebControls.Button _btnLimpar = new System.Web.UI.WebControls.Button();
        //System.Web.UI.WebControls.Button _btnFechar = new System.Web.UI.WebControls.Button();
        //HiddenField _hdnTraget = new HiddenField();
        ModalPopupExtender _Modal = new ModalPopupExtender();
        UpdatePanel _upGrid = new UpdatePanel();
        UpdatePanel _upLimpar = new UpdatePanel();
        System.Web.UI.WebControls.Panel _pnlBotoesBaixo = new System.Web.UI.WebControls.Panel();
        System.Web.UI.WebControls.Panel _pnlBotoesTopo = new System.Web.UI.WebControls.Panel();
        #endregion

        #region Propriedades

        #region ModoRenderizacao
        /// <summary>
        /// Padrão de renderização do componente
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

        #region IdModalEstado
        internal string IdModalEstado
        {
            get { return _hdnModalAberta.ClientID; }
        }
        #endregion

        #region Width
        /// <inheritdoc />
        public override Unit Width
        {
            get { return _pnlModal.Width; }
            set { _pnlModal.Width = value; }
        }
        #endregion 

        #region Height
        /// <inheritdoc />
        public override Unit Height
        {
            get { return _pnlModal.Height; }
            set { _pnlModal.Height = value; }
        }
        #endregion

        #region GridSkinID
        /// <summary>
        /// SkinID da Grid de busca
        /// </summary>
        [Browsable(true)]
        [Themeable(true)]
        public string GridSkinID
        {
            get { return _GridBusca.SkinID; }
            set { _GridBusca.SkinID = value; }
        }
        #endregion 

        #region BotoesTopoCss
        /// <summary>
        /// Classe Css dos botões do topo
        /// </summary>
        public string BotoesTopoCss
        {
            get { return _pnlBotoesTopo.CssClass; }
            set { _pnlBotoesTopo.CssClass = value; }
        }
        #endregion 

        #region CssClass
        /// <inheritdoc />
        public override string CssClass
        {
            get { return _pnlModal.CssClass; }
            set { _pnlModal.CssClass = value; }
        }
        #endregion 

        #region TituloCss
        /// <summary>
        /// Css do título da modal 
        /// </summary>
        public string TituloCss
        {
            get { return this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao ? _lblTitulo.CssClass : _TituloH3.Attributes["class"]; }
            set  { _TituloH3.Attributes["class"] = _lblTitulo.CssClass = value; }
        }
        #endregion 

        #region Titulo
        /// <summary>
        /// Titulo da modal
        /// </summary>
        public string Titulo
        {
            get { return this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao ? _lblTitulo.Text : _TituloH3.InnerText; }
            set { _TituloH3.InnerText = _lblTitulo.Text = value; }
        }
        #endregion 

        #region BuscaCss
        /// <summary>
        /// Classe Css do campo texto
        /// </summary>
        public string BuscaCss
        {
            get { return _txtBusca.CssClass; }
            set { _txtBusca.CssClass = value; }
        }
        #endregion 

        #region BuscaTexto
        /// <summary>
        /// Texto digitado no campo de busca
        /// </summary>
        public string BuscaTexto
        {
            get { return _txtBusca.Text; }
            set { _txtBusca.Text = value; }
        }
        #endregion 

        #region BuscaWidth
        /// <summary>
        /// Largura do campo de busca
        /// </summary>
        public Unit BuscaWidth
        {
            get { return _txtBusca.Width; }
            set { _txtBusca.Width = value; }
        }
        #endregion 

        #region BotaoPesquisarCss
        /// <summary>
        /// Classe Css do botão pesquisar
        /// </summary>
        public string BotaoPesquisarCss
        {
            get { return _btnPesquisar.CssClass; }
            set { _btnPesquisar.CssClass = value; }
        }
        #endregion 

        #region BotaoPesquisarTexto
        /// <summary>
        /// Texto do botão de pesquisa
        /// </summary>
        public string BotaoPesquisarTexto
        {
            get { return _btnPesquisar.Text; }
            set { _btnPesquisar.Text = value; }
        }
        #endregion 

        #region BotaoLimparCss
        /// <summary>
        /// Css do botão de limpar
        /// </summary>
        public string BotaoLimparCss
        {
            get { return _btnLimpar.CssClass; }
            set { _btnLimpar.CssClass = value; }
        }
        #endregion 

        #region BotaoLimparTexto
        /// <summary>
        /// Texto do botão de limpar
        /// </summary>
        public string BotaoLimparTexto
        {
            get { return _btnLimpar.Text; }
            set { _btnLimpar.Text = value; }
        }
        #endregion 

        #region ValidationGroup
        /// <inheritdoc/>
        public string ValidationGroup
        {
            get { return _btnPesquisar.ValidationGroup; }
            set { _btnPesquisar.ValidationGroup = value; }
        }
        #endregion

        #region BotaoLimpar
        /// <summary>
        /// Botão de limpar
        /// </summary>
        public System.Web.UI.WebControls.Button BotaoLimpar
        {
            get { return _btnLimpar; }
        }
        #endregion 

        #region BotaoFecharCss
        /// <summary>
        /// Classe Css do botão de fechar
        /// </summary>
        public string BotaoFecharCss
        {
            get { return this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao ? _btnFechar.CssClass : _lnkFechar.CssClass; }
            set { _lnkFechar.CssClass = _btnFechar.CssClass = value; }
        }        
        #endregion 
        
        #region BotaoFecharImageUrl
        /// <summary>
        /// Caminho da imagem do botão de limpar
        /// </summary>
        public string BotaoFecharImageUrl
        {
            get { return _btnFechar.ImageUrl; }
            set { _btnFechar.ImageUrl = value; }
        }
        #endregion 
        
        #region PainelBotoesBaixoCss
        /// <summary>
        /// Classe Css do painel dos botões da parte de baixo da modal
        /// </summary>
        public string PainelBotoesBaixoCss
        {
            get { return _pnlBotoesBaixo.CssClass; }
            set { _pnlBotoesBaixo.CssClass = value; }
        }
        #endregion 

        #region DivInformacaoFiltrosCss
        /// <summary>
        /// Classe css da div do painel de informação
        /// </summary>
        public string DivInformacaoFiltrosCss
        {
            get { return _pnlInfoFiltro.CssClass; }
            set { _pnlInfoFiltro.CssClass = value; }
        }
        #endregion

        #region OnOkScript
        /// <summary>
        /// Javascript disparado quando se pressiona o botão OK
        /// </summary>
        public string OnOkScript
        {
            get { return _Modal.OnOkScript; }
            set { _Modal.OnOkScript = value; }
        }
        #endregion 

        #region OnCancelScript
        /// <summary>
        /// Javascript disparado quando se pressiona o botão Cancelar
        /// </summary>
        public string OnCancelScript
        {
            get { return _Modal.OnCancelScript; }
            set { _Modal.OnCancelScript = value; }
        }
        #endregion 

        #region FundoCssClass
        /// <summary>
        /// Classe Css do fundo da modal
        /// </summary>
        public string FundoCssClass
        {
            get { return _Modal.BackgroundCssClass; }
            set { _Modal.BackgroundCssClass = value; }
        }
        #endregion

        #region TargetControlID
        /// <summary>
        /// Controle a ser extendido pelo ModalPopupExtender
        /// </summary>
        public string TargetControlID
        {
            get { return _Modal.TargetControlID; }
            set { _Modal.TargetControlID = value; }
        }
        #endregion 

        #region Modal
        /// <summary>
        /// O ModalPopupExtender associado ao controle
        /// </summary>
        public ModalPopupExtender Modal
        {
            get { return _Modal; }
        }
        #endregion

        #region LabelTexto
        /// <summary>
        /// O texto da label do campo de busca
        /// </summary>
        public string LabelTexto
        {
            get { return _lblLabel.Text; }
            set { _lblLabel.Text = value; }
        }
        #endregion 
        
        #region LabelCss
        /// <summary>
        /// A classe css do label do campo de busca
        /// </summary>
        public string LabelCss
        {
            get { return _lblLabel.CssClass; }
            set { _lblLabel.CssClass = value; }
        }
        #endregion 

        #region Columns
        /// <summary>
        /// Colunas da grid de busca
        /// </summary>
        public DataControlFieldCollection Columns
        {
            get { return _GridBusca.Columns; }
        }
        #endregion 

        #region AllowPaging
        /// <summary>
        /// Define se a grid permite ou não paginação
        /// </summary>
        public bool AllowPaging
        {
            get { return _GridBusca.AllowPaging; }
            set { _GridBusca.AllowPaging = value; }
        }
        #endregion 

        #region AllowSorting
        /// <summary>
        /// Define se a grid permite ou não ordenação
        /// </summary>
        public bool AllowSorting
        {
            get { return _GridBusca.AllowSorting; }
            set { _GridBusca.AllowSorting = value; }
        }
        #endregion 

        #region AutoGenerateColumns
        /// <summary>
        /// Define ou não se a grid vai gerar automaticamente suas colunas
        /// </summary>
        public bool AutoGenerateColumns
        {
            get { return _GridBusca.AutoGenerateColumns; }
            set { _GridBusca.AutoGenerateColumns = value; }
        }
        #endregion 

        #region GridCss
        /// <summary>
        /// Classe Css da grid
        /// </summary>
        public string GridCss
        {
            get { return _GridBusca.CssClass; }
            set { _GridBusca.CssClass = value; }
        }
        #endregion

        #region GridPanelCss
        /// <summary>
        /// Classe css do painel que envolve a grid
        /// </summary>
        public string GridPanelCss
        {
            get { return _pnlGrid.CssClass; }
            set { _pnlGrid.CssClass = value; }
        }
        #endregion 

        #region GridGridLines
        /// <summary>
        /// As linhas da grid
        /// </summary>
        public GridLines GridGridLines
        {
            get { return _GridBusca.GridLines; }
            set { _GridBusca.GridLines = value; }
        }
        #endregion 

        #region GridHeaderCssClass
        /// <summary>
        /// Classe css do cabeçalho das linhas da grid
        /// </summary>
        public string GridHeaderCssClass
        {
            get { return _GridBusca.HeaderStyle.CssClass; }
            set { _GridBusca.HeaderStyle.CssClass = value; }
        }
        #endregion 

        #region AlternatingRowStyleCssClass
        /// <summary>
        /// Classe css alteranativa das linhas da grid
        /// </summary>
        public string AlternatingRowStyleCssClass
        {
            get { return _GridBusca.AlternatingRowStyle.CssClass; }
            set { _GridBusca.AlternatingRowStyle.CssClass = value; }
        }
        #endregion 

        #region GridBusca
        /// <summary>
        /// A grid de busca
        /// </summary>
        public EmployerGrid GridBusca
        {
            get { return _GridBusca; }
        }
        #endregion 

        #region LinhaCSSPesquisaModal
        /// <summary>
        /// Insere o css da linha de pesquisa da modal TextBox + Botao
        /// </summary>
        public string LinhaCSSPesquisaModal
        {
            get { return ViewState["LinhaCSSPesquisaModal"] != null ? ViewState["LinhaCSSPesquisaModal"].ToString() : ""; }
            set { ViewState["LinhaCSSPesquisaModal"] = value; }
        }
        #endregion

        #region LinhaCSSPesquisaModal
        /// <summary>
        /// Classe css da linha de campo do painel de filtro customizáveis
        /// </summary>
        public string LinhaCSSCamposModal
        {
            get { return ViewState["LinhaCSSCamposModal"] != null ? ViewState["LinhaCSSCamposModal"].ToString() : ""; }
            set { ViewState["LinhaCSSCamposModal"] = value; }
        }
        #endregion        

        #region Ordenação
        /// <summary>
        /// Expressão de ordenação
        /// </summary>
        public string SortExpression
        {
            get { return ViewState["SortExpression"] != null ? (string)ViewState["SortExpression"] : string.Empty; }
            set { ViewState["SortExpression"] = value;  }
        }

        /// <summary>
        /// Direção da ordenação.
        /// </summary>
        public SortDirection SortDirection
        {
            get { return ViewState["SortDirection"] != null ? (SortDirection)ViewState["SortDirection"] : SortDirection.Ascending; }
            set { ViewState["SortDirection"] = value; }
        }
        #endregion

        #region ModalAberta
        private bool ModalAberta
        {
            get
            {
                EnsureChildControls();
                bool bTemp = false;
                if (bool.TryParse(_hdnModalAberta.Value, out bTemp))
                    return bTemp;

                return false;
            }
            set 
            {
                EnsureChildControls();
                _hdnModalAberta.Value = value.ToString(); 
            }
        }
        #endregion

        #endregion

        #region Procedimentos

        #region CalculaPaginacao
        /// <summary>
        /// Realiza o calculo de paginação da grid presente na modal
        /// </summary>
        /// <param name="inicio">Registro inicial da paginação</param>
        /// <param name="fim">Registro final da paginação</param>
        public void CalculaPaginacao(out int inicio, out int fim)
        {
            int paginaCorrente = this.GridBusca.MockPageIndex + 1;
            int tamanhoPagina = this.GridBusca.PageSize;
            
            inicio = ((paginaCorrente - 1) * @tamanhoPagina) + 1;
            fim = paginaCorrente * tamanhoPagina;
        }
        #endregion

        #region CreateChildControls
        /// <inheritdoc />
        protected override void CreateChildControls()
        {
            IniciaPainel();
            IniciaLabel();
            IniciaTextBox();
            IniciaBotoes();
            IniciaGrid();

            _pnlModal.Controls.Add(_hdnFechar);
            _pnlModal.Controls.Add(_hdnModalAberta);

            if (this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
            {
                _pnlModal.Controls.Add(_lblTitulo);
                _pnlModal.Controls.Add(_btnFechar);
                _pnlModal.Controls.Add(_upGrid);
            }
            else
            {
                _pnlModal.Controls.Add(_lnkFechar);

                var _pnlPrimary = new Panel { CssClass = "panel panel-primary" };
                var _pnlHeader = new Panel { CssClass = "panel-heading" };
                var _pnlBody = new Panel { CssClass = "panel-body" };

                _pnlModal.Controls.Add(_pnlPrimary);

                _pnlPrimary.Controls.Add(_pnlHeader);
                _pnlPrimary.Controls.Add(_pnlBody);

                _pnlHeader.Controls.Add(_TituloH3);
                _pnlBody.Controls.Add(_upGrid);
            }            
            
            _pnlModal.Controls.Add(_pnlBotoesBaixo);

            IniciaUpdatePanel();
            IniciaModal();

            base.CreateChildControls();

            if (PosCreateChildControls != null)
                PosCreateChildControls(this, new EventArgs());
        }
        #endregion

        #region OnInit
        /// <inheritdoc />
        protected override void OnInit(EventArgs e)
        {
            _btnPesquisar.Click += new EventHandler(_btnPesquisar_Click);
            _btnLimpar.Click += new EventHandler(_btnLimpar_Click);
            _GridBusca.RowDataBound += new GridViewRowEventHandler(_GridBusca_RowDataBound);
            _GridBusca.PageIndexChanging += new GridViewPageEventHandler(_GridBusca_PageIndexChanging);
            _GridBusca.PageSizeChanging += new EventHandler(_GridBusca_PageSizeChanging);
            _GridBusca.Sorting += new GridViewSortEventHandler(_GridBusca_Sorting);

            if (this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
                _btnFechar.Click += new ImageClickEventHandler(_btnFechar_Click);
            else
                _lnkFechar.Click += _lnkFechar_Click;
        }
        #endregion

        #region OnLoad
        /// <inheritdoc />
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (string.IsNullOrEmpty(_btnFechar.ImageUrl))
                _btnFechar.ImageUrl = Page.ClientScript.GetWebResourceUrl(
                    this.GetType(), "Employer.Componentes.UI.Web.Content.Images.botao_padrao_fechar.png");
        }
        #endregion

        #region OnPreRender
        /// <inheritdoc />
        protected override void OnPreRender(EventArgs e)
        {
            _txtBusca.Attributes["onkeypress"] =
                String.Format("AjaxClientControlBase.SimulateClick(event,'{0}')",
                this._btnPesquisar.ClientID);

            base.OnPreRender(e);

            if (this.ModalAberta)
                _Modal.Show();
            else
                _Modal.Hide();
        }
        #endregion 

        #region Close
        /// <inheritdoc />
        public virtual void Close()
        {
            _Modal.Hide();
            this.ModalAberta = false;
        }
        #endregion

        #region _GridBusca_PageSizeChanging
        private void _GridBusca_PageSizeChanging(object sender, EventArgs e)
        {
            object objDados = new object();
            Pesquisar(_btnPesquisar, out objDados);

            _GridBusca.SelectedIndex = -1;
            _GridBusca.DataSource = objDados;
            _GridBusca.DataBind();
            _upGrid.Update();
        }
        #endregion

        #region _GridBusca_Sorted

        void _GridBusca_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.GridBusca.MockDirecaoOrdenacao =
            this.SortDirection = this.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending ? System.Web.UI.WebControls.SortDirection.Descending : System.Web.UI.WebControls.SortDirection.Ascending;

            this.GridBusca.MockColunaOrdenada =
            this.SortExpression = e.SortExpression;               

            GridBusca.MockPageIndex = 0;
            GridBusca.SelectedIndex = -1;

            object objDados = new object();
            Pesquisar(_btnPesquisar, out objDados);

            GridBusca.DataSource = objDados;
            GridBusca.DataBind();
        }                   
        #endregion

        #region _GridBusca_PageIndexChanging
        /// <summary>
        /// Tratador do evento de mudança de índice da grid
        /// </summary>
        /// <param name="sender">O Objeto que enviou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _GridBusca_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            _GridBusca.MockPageIndex = e.NewPageIndex;

            object objDados = new object();
            Pesquisar(_btnPesquisar, out objDados);

            _GridBusca.SelectedIndex = -1;
            _GridBusca.DataSource = objDados;
            _GridBusca.DataBind();
            _upGrid.Update();
        }
        #endregion

        #region _GridBusca_RowDataBound
        /// <summary>
        /// Tratador do evento de dados populados da grid
        /// </summary>
        /// <param name="sender">O Objeto que enviou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _GridBusca_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (AtualizaLinhasGrid != null)
                AtualizaLinhasGrid(sender, e);
        }
        #endregion 

        #region Iniciar Controles

        #region IniciaPainel
        /// <summary>
        /// Inicia o painel principal
        /// </summary>
        private void IniciaPainel()
        {
            _pnlModal.ID = "pnlModal";
            _pnlModal.Style[HtmlTextWriterStyle.Display] = "none";
            //_pnlModal.EnableTheming = false;
            this.Controls.Add(_pnlModal);

            _pnlBotoesBaixo.ID = "pnlBotoesBaixo";
            _pnlBotoesBaixo.EnableTheming = false;

            if (InformacaoPainelBotoesAntesBotaoLimpar != null)
                InformacaoPainelBotoesAntesBotaoLimpar(_pnlBotoesBaixo);

            _pnlBotoesBaixo.Controls.Add(_upLimpar);
        }
        #endregion 

        #region IniciaLabel
        /// <summary>
        /// Inicia a label de título
        /// </summary>
        private void IniciaLabel()
        {
            _lblTitulo.ID = "Titulo";
            _lblTitulo.EnableTheming = false;
        }
        #endregion 

        #region IniciaTextBox
        /// <summary>
        /// Inicia a textbox de busca
        /// </summary>
        private void IniciaTextBox()
        {
            _txtBusca.ID = "txtBusca";
            _txtBusca.EnableTheming = false;
        }
        #endregion 

        #region IniciaBotoes
        /// <summary>
        /// Inicia todos os botões
        /// </summary>
        private void IniciaBotoes()
        {
            _btnPesquisar.ID = "btnPesquisar";
            _btnLimpar.ID = "btnLimpar";
            _btnFechar.ID = "btnFechar";
            _lnkFechar.ID = "lnkFechar";

            _lnkFechar.EnableTheming =
            _btnPesquisar.EnableTheming = 
            _btnLimpar.EnableTheming = 
            _btnFechar.EnableTheming = false;

            _lnkFechar.CausesValidation =
            _btnPesquisar.CausesValidation =
            _btnLimpar.CausesValidation =
            _btnFechar.CausesValidation = false;

            _btnPesquisar.Text = Resources.ControlAutoCompleteBase_Pesquisar;
            _btnLimpar.Text = Resources.ControlAutoCompleteBase_Limpar;
            //_btnFechar.Text = Resources.ControlAutoCompleteBase_Fechar;
        }
        #endregion 
        
        #region IniciaModal
        /// <summary>
        /// Inicia a modal 
        /// </summary>
        private void IniciaModal()
        {
            _Modal.PopupControlID = _pnlModal.ID;
            _Modal.CancelControlID = _hdnFechar.ID;
            this.Controls.Add(_Modal);
        }
        #endregion 

        #region IniciaUpdatePanel
        /// <summary>
        /// Inicia o update panel
        /// </summary>
        private void IniciaUpdatePanel()
        {
            _upGrid.ID = "upGrid";
            _upGrid.UpdateMode = UpdatePanelUpdateMode.Conditional;

            if (CriarFiltroCustomizavel != null)
            {
                Panel divLinhaRelatorio = new Panel();
                divLinhaRelatorio.CssClass = LinhaCSSCamposModal;
                _pnlBotoesTopo.Controls.Add(divLinhaRelatorio);
                CriarFiltroCustomizavel(divLinhaRelatorio);

                divLinhaRelatorio = new Panel();
                divLinhaRelatorio.CssClass = LinhaCSSPesquisaModal;
                divLinhaRelatorio.Controls.Add(_btnPesquisar);
                _pnlBotoesTopo.Controls.Add(divLinhaRelatorio);
            }
            else
            {
                if (InformacaoFiltros != null)
                {
                    _pnlBotoesTopo.Controls.Add(_pnlInfoFiltro);
                    _pnlInfoFiltro.Visible = true;
                    InformacaoFiltros(_pnlInfoFiltro);
                }

                if (this.ModoRenderizacao == ModoRenderizacaoEnum.Padrao)
                    _pnlBotoesTopo.AdicionarLinhaPadrao(_lblLabel, _txtBusca, _btnPesquisar, _pnlBotoesTopo.CssClass, LinhaCSSPesquisaModal);                
                else
                {
                    _pnlBotoesTopo.Controls.Add(_lblLabel);
                    _pnlBotoesTopo.Controls.Add(_txtBusca);
                    _pnlBotoesTopo.Controls.Add(_btnPesquisar);
                }
            }

            _pnlGrid.ID = "pnlGrid";
            _pnlGrid.Controls.Add(_GridBusca);

            _upGrid.ContentTemplateContainer.Controls.Add(_pnlBotoesTopo);

            _upGrid.ContentTemplateContainer.Controls.Add(_pnlGrid);

            _upLimpar.ID = "upLimpar";
            _upLimpar.RenderMode = UpdatePanelRenderMode.Inline;
            _upLimpar.UpdateMode = UpdatePanelUpdateMode.Conditional;

            _upLimpar.ContentTemplateContainer.Controls.Add(_btnLimpar);
            //_pnlBotoesBaixo.Controls.Add(_btnFechar);
        }
        #endregion 

        #region IniciaGrid
        /// <summary>
        /// Inicia a datagrid
        /// </summary>
        private void IniciaGrid()
        {
            _GridBusca.CellSpacing = 2;
            _GridBusca.ID = "_GridBusca";
        }
        #endregion         

        #endregion

        #region Botões

        #region Fechar
        void _btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            _Modal.Hide();
            this.ModalAberta = false;
        }

        void _lnkFechar_Click(object sender, EventArgs e)
        {
            _Modal.Hide();
            this.ModalAberta = false;
        }   
        #endregion

        #region _btnLimpar_Click
        /// <summary>
        /// Tratador do evento disparado quando se clica no botão de limpar
        /// </summary>
        /// <param name="sender">O Objeto que enviou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _btnLimpar_Click(object sender, EventArgs e)
        {
            if (Limpar != null)
                Limpar(sender, e);

            BuscaTexto = string.Empty;
            _GridBusca.MockPageIndex = 0;
            _GridBusca.SelectedIndex = -1;

            if (Pesquisar != null)
                _btnPesquisar_Click(sender, e);
            else
            {
                _GridBusca.DataSource = null;
                _GridBusca.DataBind();
                _upGrid.Update();
            }            
        }
        #endregion 

        #region _btnPesquisar_Click
        /// <summary>
        /// Tratador do evento disparado quando se clica no botão de pesquisar
        /// </summary>
        /// <param name="sender">O Objeto que enviou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _btnPesquisar_Click(object sender, EventArgs e)
        {
            if (Pesquisar != null)
            {
                _GridBusca.MockPageIndex = 0;
                _GridBusca.SelectedIndex = -1;

                object objDados = new object();
                Pesquisar(_btnPesquisar, out objDados);

                _GridBusca.DataSource = objDados;
                _GridBusca.DataBind();
                _upGrid.Update();
            }
        }
        #endregion 

        #endregion

        #region Show
        /// <summary>
        /// Exibe a modal 
        /// </summary>
        public virtual void Show()
        {
            if (_txtBusca.Parent != null)
                _txtBusca.Focus();

            _btnLimpar_Click(_btnLimpar, new EventArgs());
            this.Modal.Show();

            this.ModalAberta = true;
        }
        #endregion
        #endregion
    }
}
