using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.ComponentModel;
using System.Web.UI;
using System.Reflection;
using System.Collections;
using System.Data;
using Employer.Componentes.UI.Web.Util;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Controle base de modal de pesquisa com treeview
    /// </summary>
    public class ControlModalBuscaTreeView : CompositeControl
    {
        #region ModosExibicao
        /// <summary>
        /// Modos de exibição da modal
        /// </summary>
        public enum ModosExibicao
        {
            /// <summary>
            /// Exibição em tabela
            /// </summary>
            Tabela = 0,
            /// <summary>
            /// Exibição em árvore
            /// </summary>
            Estrutura = 1
        }
        #endregion 

        #region ExibicaoEstrutura
        /// <summary>
        /// Modo de exibição da estrutura
        /// </summary>
        public enum ExibicaoEstrutura
        {
            /// <summary>
            /// Estrutura Fechada
            /// </summary>
            Fechada = 0,
            /// <summary>
            /// Estrutura Aberta
            /// </summary>
            Aberta = 1
        }
        #endregion 

        #region Eventos
        /// <summary>
        /// Descritor do evento de pesquisar 
        /// </summary>
        /// <param name="BtnSender">O botão que disparou a busca</param>
        /// <param name="DadosBusca">Os dados retornados pela busca</param>
        public delegate void HandlerPesquisar(System.Web.UI.WebControls.Button BtnSender, out object DadosBusca);
        /// <summary>
        /// Descritor do evento que atualiza os nós da árvore
        /// </summary>
        /// <param name="dataKey">O objeto da fonte de dados representado pelo nó</param>
        public delegate void HandlerAtualizaNodes(Object dataKey);

        /// <summary>
        /// Evento Pesquisar
        /// </summary>
        public event HandlerPesquisar Pesquisar;
        /// <summary>
        /// Evento que ocorre após a criação dos controles filhos
        /// </summary>
        public event EventHandler PosCreateChildControls;
        /// <summary>
        /// Evento de Atualização das linhas da grid
        /// </summary>
        public event GridViewRowEventHandler AtualizaLinhasGrid;
        /// <summary>
        /// Evento disparado durante a atualização dos nós da árvore
        /// </summary>
        public event HandlerAtualizaNodes AtualizaNo;
        #endregion

        #region Atributos
        System.Web.UI.WebControls.Panel _pnlModal = new System.Web.UI.WebControls.Panel();
        System.Web.UI.WebControls.Label _lblTitulo = new System.Web.UI.WebControls.Label();
        ImageButton _btnFechar = new ImageButton();
        System.Web.UI.WebControls.Panel _pnlModo = new System.Web.UI.WebControls.Panel();
        System.Web.UI.WebControls.Label _lblModo = new System.Web.UI.WebControls.Label();
        ListaSugestoes _lsgModo = new ListaSugestoes();
        Label _lblLabel = new Label();
        TextBox _txtBusca = new TextBox();
        System.Web.UI.WebControls.Button _btnPesquisar = new System.Web.UI.WebControls.Button();
        HiddenField _hdnFechar = new HiddenField() { ID = "hdnFechar" };
        HiddenField _hdnModalAberta = new HiddenField() { ID = "hdnModalAberta" };
        EmployerGrid _GridBusca = new EmployerGrid();
        TreeView _tvBusca = new TreeView();
        System.Web.UI.WebControls.Button _btnLimpar = new System.Web.UI.WebControls.Button();
        //System.Web.UI.WebControls.Button _btnFechar = new System.Web.UI.WebControls.Button();
        //HiddenField _hdnTraget = new HiddenField();
        ModalPopupExtender _Modal = new ModalPopupExtender();
        UpdatePanel _upGrid = new UpdatePanel();
        UpdatePanel _upLimpar = new UpdatePanel();
        System.Web.UI.WebControls.Panel _pnlBotoesBaixo = new System.Web.UI.WebControls.Panel();
        System.Web.UI.WebControls.Panel _pnlBotoesTopo = new System.Web.UI.WebControls.Panel();
        System.Web.UI.WebControls.Panel _pnlTreeView = new System.Web.UI.WebControls.Panel();
        System.Web.UI.WebControls.Panel _pnlDadosBusca = new System.Web.UI.WebControls.Panel();
        #endregion

        #region Propriedades

        #region IdModalEstado
        internal string IdModalEstado
        {
            get { return _hdnModalAberta.ClientID; }
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

        #region DadosBuscaCss
        /// <summary>
        /// Classe css do painel de busca
        /// </summary>
        public string DadosBuscaCss
        {
            get { return _pnlDadosBusca.CssClass; }
            set { _pnlDadosBusca.CssClass = value; }
        }
        #endregion

        #region TituloCss
        /// <summary>
        /// Css do título da modal 
        /// </summary>
        public string TituloCss
        {
            get { return _lblTitulo.CssClass; }
            set { _lblTitulo.CssClass = value; }
        }
        #endregion 

        #region Titulo
        /// <summary>
        /// Titulo da modal
        /// </summary>
        public string Titulo
        {
            get { return _lblTitulo.Text; }
            set { _lblTitulo.Text = value; }
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

        #region TreeViewHeight
        /// <summary>
        /// Altura da Tree View
        /// </summary>
        public Unit TreeViewHeight
        {
            get { return this._pnlTreeView.Height; }
            set { this._pnlTreeView.Height = value; }
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
            get { return _btnFechar.CssClass; }
            set { _btnFechar.CssClass = value; }
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

        //public string BotaoFecharTexto
        //{
        //    get { return _btnFechar.Text; }
        //    set { _btnFechar.Text = value; }
        //}
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
            get {
                EnsureChildControls();
                return _Modal; 
            }
        }
        #endregion 

        #region LabelTexto
        /// <summary>
        /// O texto da label do campo de busca
        /// </summary>
        public string LabelTexto
        {
            get { return _lblLabel.Text; }
            set { _lblLabel.Text = String.Format("<p>{0}</p>", value); }
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

        #region LabelModoExibicaoCss
        /// <summary>
        /// Classe Css da label do modo de exibição
        /// </summary>
        public string LabelModoExibicaoCss
        {
            get { return _lblModo.CssClass; }
            set { _lblModo.CssClass = value; }
        }
        #endregion 

        #region TreeView Css 
        /// <summary>
        /// Classe css da treeview
        /// </summary>
        public string TreeViewCssClass
        {
            get { return _tvBusca.CssClass; }
            set { _tvBusca.CssClass = value; }
        }

        /// <summary>
        /// Classe css dos nós selecionados
        /// </summary>
        public string TreeViewSelectedNodeCssClass
        {
            get { return _tvBusca.SelectedNodeStyle.CssClass; }
            set { _tvBusca.SelectedNodeStyle.CssClass = value; }
        }
        #endregion

        #region DivModoExibicaoCss
        /// <summary>
        /// Classe Css da div de modo de exibição
        /// </summary>
        public string DivModoExibicaoCss
        {
            get { return _pnlModo.CssClass; }
            set { _pnlModo.CssClass = value; }
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
        /// Define se a grid permite ou não paginação
        /// </summary>
        public bool AllowSorting
        {
            get { return _GridBusca.AllowSorting; }
            set { _GridBusca.AllowSorting = value; }
        }
        #endregion 

        #region Ordenação
        /// <summary>
        /// Expressão de ordenação
        /// </summary>
        public string SortExpression
        {
            get { return ViewState["SortExpression"] != null ? (string)ViewState["SortExpression"] : string.Empty; }
            set { ViewState["SortExpression"] = value; }
        }

        /// <summary>
        /// Direção da ordenação
        /// </summary>
        public SortDirection SortDirection
        {
            get { return ViewState["SortDirection"] != null ? (SortDirection)ViewState["SortDirection"] : SortDirection.Ascending; }
            set { ViewState["SortDirection"] = value; }
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
            get 
            {
                EnsureChildControls();
                return _GridBusca; 
            }
        }
        #endregion 

        #region TreeView
        /// <summary>
        /// A Árvore de busca
        /// </summary>
        public TreeView TreeView
        {
            get { return _tvBusca; }
        }
        #endregion 

        #region TipoApresentacaoPesquisa
        /// <summary>
        /// Define qual é o modo de apresentação da modal de pesquisa
        /// Tabela: Modo de exibição em tabela
        /// Estrutura: Modo de exibição em árvore
        /// </summary>
        public ModosExibicao TipoApresentacaoPesquisa
        {
            get
            {
                if (String.IsNullOrEmpty(_lsgModo.Valor))
                    _lsgModo.ValorInt = ModosExibicao.Estrutura.GetHashCode();

                if (_lsgModo.ValorInt.HasValue)
                    return (ModosExibicao)_lsgModo.ValorInt;
                else
                {
                    _lsgModo.ValorInt = ModosExibicao.Estrutura.GetHashCode();
                    return (ModosExibicao)_lsgModo.ValorInt;
                }
            }
            set
            {
                _lsgModo.ValorInt = value.GetHashCode();

                if (value == ModosExibicao.Estrutura)
                {
                    _pnlTreeView.Visible = true;
                    _tvBusca.Visible = true;
                    _GridBusca.Visible = false;
                }
                else
                {
                    _pnlTreeView.Visible = false;
                    _tvBusca.Visible = false;
                    _GridBusca.Visible = true;
                }

            }
        }
        #endregion 

        #region TipoApresentacaoEstrutura
        /// <summary>
        /// Quando no modo estrutura define se a estrutura será apresentada:
        /// Fechada: Nós retraídos
        /// Aberta: Nós abertos
        /// </summary>
        public ExibicaoEstrutura TipoApresentacaoEstrutura
        {
            get
            {
                if (this.ViewState["TipoApresentacaoEstrutura"] == null)
                    this.ViewState["TipoApresentacaoEstrutura"] = ExibicaoEstrutura.Fechada.GetHashCode();
                return (ExibicaoEstrutura)this.ViewState["TipoApresentacaoEstrutura"];
            }
            set
            {
                this.ViewState["TipoApresentacaoEstrutura"] = value.GetHashCode();
            }
        }
        #endregion 

        #region ListaSugestaoCss
        /// <summary>
        /// Classe de css da lista de sugestão
        /// </summary>
        public String ListaSugestaoCss
        {
            get
            {
                return this._lsgModo.CssClass;
            }
            set
            {
                this._lsgModo.CssClass = value;
            }
        }
        #endregion 

        #endregion

        #region Procedimentos

        #region CreateChildControls
        /// <inheritdoc />
        protected override void CreateChildControls()
        {
            IniciaPainel();
            IniciaLabelModo();
            IniciaListaSugestao();
            IniciaLabel();
            IniciaTextBox();
            IniciaBotoes();
            IniciaGrid();
            IniciaTreeView();

            _pnlModal.Controls.Add(_lblTitulo);
            _pnlModal.Controls.Add(_btnFechar);
            _pnlModal.Controls.Add(_hdnFechar);
            _pnlModal.Controls.Add(_hdnModalAberta);
            _pnlModal.Controls.Add(_upGrid);
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
            _lsgModo.ValueChanged += new ValueChangedEvent(_lsgModo_ValueChanged);
            _btnPesquisar.Click += new EventHandler(_btnPesquisar_Click);
            _btnLimpar.Click += new EventHandler(_btnLimpar_Click);
            
            _GridBusca.RowDataBound += new GridViewRowEventHandler(_GridBusca_RowDataBound);
            _GridBusca.PageIndexChanging += new GridViewPageEventHandler(_GridBusca_PageIndexChanging);
            _GridBusca.PageSizeChanging += new EventHandler(_GridBusca_PageSizeChanging);
            _GridBusca.Sorting += new GridViewSortEventHandler(_GridBusca_Sorting);

            _tvBusca.TreeNodeDataBound += new TreeNodeEventHandler(_tvBusca_TreeNodeDataBound);
            _btnFechar.Click += new ImageClickEventHandler(_btnFechar_Click);

            base.OnInit(e);
        }

        #endregion
        /// <inheritdoc />
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (string.IsNullOrEmpty(_btnFechar.ImageUrl))
                _btnFechar.ImageUrl = Page.ClientScript.GetWebResourceUrl(
                    this.GetType(), "Employer.Componentes.UI.Web.Content.Images.botao_padrao_fechar.png");
        }

        #region OnPreRender
        /// <inheritdoc />
        protected override void OnPreRender(EventArgs e)
        {
            if (this.TipoApresentacaoPesquisa == ModosExibicao.Estrutura)
            {
                _pnlTreeView.Visible = true;
                _tvBusca.Visible = true;
                _GridBusca.Visible = false;
            }
            else
            {
                _pnlTreeView.Visible = false;
                _tvBusca.Visible = false;
                _GridBusca.Visible = true;
            }

            if (TipoApresentacaoEstrutura == ExibicaoEstrutura.Aberta)
                _tvBusca.ExpandAll();
            else
                _tvBusca.CollapseAll();

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
        public void Close()
        {
            _Modal.Hide();
            this.ModalAberta = false;
        }
        #endregion

        #region _GridBusca_Sorted

        void _GridBusca_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.SortDirection = this.SortDirection == System.Web.UI.WebControls.SortDirection.Ascending ? System.Web.UI.WebControls.SortDirection.Descending : System.Web.UI.WebControls.SortDirection.Ascending;
            this.SortExpression = e.SortExpression;

            GridBusca.MockPageIndex = 0;
            GridBusca.SelectedIndex = -1;

            object objDados = new object();
            Pesquisar(_btnPesquisar, out objDados);

            GridBusca.DataSource = objDados;
            GridBusca.DataBind();
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

        #region _tvBusca_TreeNodeDataBound
        /// <summary>
        /// Evento disparado quando a árvore está sendo populada
        /// </summary>
        /// <param name="sender">O Objeto que enviou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _tvBusca_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            DataRow row = ((DataRowView)e.Node.DataItem).Row;

            if (Convert.ToBoolean(row["selecionar"]))
                e.Node.SelectAction = TreeNodeSelectAction.Select;
            else
                e.Node.SelectAction = TreeNodeSelectAction.None;

            if (AtualizaNo != null)
            {
                AtualizaNo(e.Node);
            }
        }
        #endregion 

        #region _lsgModo_ValueChanged
        /// <summary>
        /// Evento disparado quando o valor da lista de sugestão muda
        /// </summary>
        /// <param name="sender">O Objeto que enviou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _lsgModo_ValueChanged(object sender, ValueChangedArgs e)
        {
            if (_lsgModo.ValorInt == 1)
            {
                GridBusca.Visible = false;
                _pnlTreeView.Visible = true;
                _tvBusca.Visible = true;                
            }
            else
            {
                GridBusca.Visible = true;
                _pnlTreeView.Visible = false;
                _tvBusca.Visible = false;                
            }
            _btnPesquisar_Click(this, null);

            if (_lsgModo.ValorInt == 1)
            {
                if (TipoApresentacaoEstrutura == ExibicaoEstrutura.Aberta)
                    _tvBusca.ExpandAll();
                else
                    _tvBusca.CollapseAll();
            }

            // Focus
            this._txtBusca.Focus();
            JavascriptHelper.FocusWithTimeout(this.Page, this._txtBusca, this._txtBusca.ClientID);
            
            _upGrid.Update();
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
            _pnlBotoesBaixo.Controls.Add(_upLimpar);
        }
        #endregion 

        #region IniciaLabelModo
        /// <summary>
        /// Inicia a label de modo de exibição
        /// </summary>
        private void IniciaLabelModo()
        {
            _lblModo.ID = "lbl_modo";
            _lblModo.EnableTheming = false;
            _lblModo.Text = "Modo Exibição ";
        }
        #endregion 

        #region IniciaListaSugestao
        /// <summary>
        /// Inicia a lista de sugestão de modo de exibição
        /// </summary>
        private void IniciaListaSugestao()
        {
            _lsgModo.ID = "lsgModo";
            _lsgModo.CampoChave = "Chave";
            _lsgModo.CampoDescricao = "Descricao";
            _lsgModo.Tamanho = 1;

            _lsgModo.DataSource = new Object[] {
                new { Chave="0", Descricao="Tabela"  },
                new { Chave="1", Descricao="Estrutura"  }
            };

            _lsgModo.TipoSugestao = ListaSugestaoBenesite.TipoListaSugestao.Numero;
            _lsgModo.DataBind();
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

            _btnPesquisar.EnableTheming = false;
            _btnLimpar.EnableTheming = false;
            _btnFechar.EnableTheming = false;

            _btnPesquisar.CausesValidation = false;
            _btnLimpar.CausesValidation = false;
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


            _pnlModo.Controls.Add(_lblModo);
            _pnlModo.Controls.Add(_lsgModo);

            //_pnlBotoesTopo.Controls.Add(_lblModo);
            //_pnlBotoesTopo.Controls.Add(_lsgModo);
            _pnlBotoesTopo.Controls.Add(_pnlModo);
            _pnlBotoesTopo.Controls.Add(_lblLabel);
            _pnlBotoesTopo.Controls.Add(_txtBusca);
            _pnlBotoesTopo.Controls.Add(_btnPesquisar);

            _upGrid.ContentTemplateContainer.Controls.Add(_pnlBotoesTopo);

            _pnlTreeView.Width = Unit.Percentage(100);
            /*_pnlTreeView.Height = Unit.Pixel(Convert.ToInt32(this.Width.Value) - 300);*/
            _pnlTreeView.Controls.Add(_tvBusca);
            _pnlTreeView.ScrollBars = ScrollBars.Auto;
            _pnlTreeView.Visible = false;
            _tvBusca.Width = Unit.Percentage(100);
            _tvBusca.Height = Unit.Percentage(100);

            _pnlDadosBusca.Controls.Add(_pnlTreeView);
            _pnlDadosBusca.Controls.Add(GridBusca);
            _upGrid.ContentTemplateContainer.Controls.Add(_pnlDadosBusca);

            _upLimpar.ID = "upLimpar";
            _upLimpar.RenderMode = UpdatePanelRenderMode.Inline;
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
        }
        #endregion 

        #region IniciaTreeView
        /// <summary>
        /// Inicia a Tree View
        /// </summary>
        private void IniciaTreeView()
        {
            _tvBusca.ID = "_tvBusca";
            _tvBusca.AutoGenerateDataBindings = false;
            _tvBusca.ShowLines = true;
        }
        #endregion 

        #endregion

        #region Botões

        #region _btnLimpar_Click
        /// <summary>
        /// Tratador do evento disparado quando se clica no botão de limpar
        /// </summary>
        /// <param name="sender">O Objeto que enviou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void _btnLimpar_Click(object sender, EventArgs e)
        {
            BuscaTexto = string.Empty;
            _GridBusca.MockPageIndex = 0;
            _GridBusca.SelectedIndex = -1;

            if (Pesquisar != null)
                _btnPesquisar_Click(sender, e);
            else
            {
                _GridBusca.DataSource = null;
                _GridBusca.DataBind();
                _tvBusca.DataSource = null;
                _tvBusca.DataBind();
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

                if (this.TipoApresentacaoPesquisa == ModosExibicao.Tabela)
                {
                    _GridBusca.DataSource = objDados;
                    _GridBusca.DataBind();
                }
                else
                {
                    _tvBusca.DataSource = objDados;
                    _tvBusca.DataBind();
                }
                _upGrid.Update();
            }
        }
        #endregion 

        #region _btnFechar_Click
        void _btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            this.Close();
        }
        #endregion

        #endregion

        #region Show
        /// <summary>
        /// Exibe a modal 
        /// </summary>
        public void Show()
        {
            _btnLimpar_Click(_btnLimpar, new EventArgs());
            this.Modal.Show();

            this.ModalAberta = true;
        }
        #endregion
        #endregion
    }
}
