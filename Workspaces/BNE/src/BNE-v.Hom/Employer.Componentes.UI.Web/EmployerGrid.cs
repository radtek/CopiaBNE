using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using System.Drawing.Design;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente de Grid com paginação customizada, grupo de validação entre outras funcionalidades.<br/>
    /// <example>
    /// Exemplo de utilização da grid com estilo e paginação:<br/>
    /// <code language="C#">
    /// protected void Page_Load(object sender, EventArgs e)
    /// {
    ///     if (!IsPostBack)
    ///         CarregarGrid();
    /// }
    /// 
    /// private void CarregarGrid()
    /// {
    ///     gv.DataSource = dataSource; //Substitua dataSource pela usa consulta na dados
    ///     gv.MockItemCount = itemCount; //Substitua itemCount pelo valor total retornado da base de dados
    ///     gv.DataBind();
    /// }
    /// 
    /// protected void gv_CarregarGrid(object sender, EventArgs e)
    /// {
    ///     CarregarGrid()
    /// }
    /// </code>
    /// No exemplo acima é informado o total de registros atravez da propriedade MockItemCount para a grid gv<br/>
    /// <code title="Aspx" language="xml">
    /// &lt;Employer:EmployerGrid id=&quot;gv&quot; runat=&quot;server&quot; OnCarregarGrid=&quot;gv_CarregarGrid&quot; AllowSorting=&quot;True&quot; AllowPaging=&quot;True&quot;&gt;
    /// &lt;/Employer:EmployerGrid&gt;
    /// </code>
    /// O evento CarregarGrid foi criado para facilitar a paginação, pois com apenas 1 evento realizar o trabalho que precisaria dos eventos RowCommand, Sorting, PageIndexChanging e RowCreated.<br/>
    /// <br/>
    /// Arquivo de thema
    /// <code title="Skin" language="xml">
    /// &lt;Employer:EmployerGrid  runat=&quot;server&quot; 
    ///CssClass=&quot;tabela&quot; 
    ///GridLines=&quot;None&quot;
    ///CssClassHeaderOrderAsc=&quot;HeaderOrderAsc&quot; 
    ///CssClassHeaderOrderDesc=&quot;HeaderOrderDesc&quot;
    ///CellSpacing=&quot;2&quot;
    ///RowStyle-CssClass=&quot;linha_ponteiro&quot;&gt;
    ///&lt;HeaderStyle CssClass = &quot; cabecalho_tabela&quot; /&gt;
    ///&lt;AlternatingRowStyle CssClass = &quot; even&quot; /&gt;
    ///&lt;PagerStyle CssClass = &quot; pag_tab&quot;/&gt;
    ///&lt;EmployerPaginacaoStyle
    ///    DivPaginacaoCss = &quot; paginacao&quot; 
    ///    DivPaginacaoInfoCss=&quot;pagin_info&quot;
    ///    DivPaginacaoPesquisaCss=&quot;pagin_itens&quot;
    ///    DivPaginacaoPesquisaBotaoOkCss=&quot;pagin_pesq&quot;
    ///    DivPaginacaoQuantidadePaginasCss=&quot;qts_itens_pagina&quot;
    ///    SpanPaginacaoInfoCss=&quot;btn_paginacao&quot;
    ///    LinkSelecionadoPaginacaoInfoCss=&quot;selected&quot;
    ///    PrimeiroImageUrl=&quot;/img/icn_ff.png&quot;
    ///    AnteriorImageUrl=&quot;/img/icn_f.png&quot;
    ///    ProximoImageUrl=&quot;/img/icn_rw.png&quot;
    ///    UltimoImageUrl=&quot;/img/icn_rrw.png&quot;
    ///    BotaoOkCss=&quot;btn_ok&quot;
    ///    BotaoOkImageUrl=&quot;/img/btn_ok.png&quot;
    ///    TextBoxIrParaCss=&quot;pag_texto&quot;
    ///    BotoesPaginacaoCss=&quot;btnPaginacaoGrid&quot;
    ///    /&gt;
    ///&lt;/Employer:EmployerGrid&gt;
    /// </code>
    /// 
    /// </example>
    /// </summary>
    public class EmployerGrid : GridView, IPostBackContainer, IPostBackEventHandler, ICallbackContainer, ICallbackEventHandler, IPersistedSelector
    {
        #region Eventos
        /// <summary>
        /// Evento disparado ao recarregar a grid.
        /// </summary>
        public event EventHandler CarregarGrid;

        /// <summary>
        /// Evento disparado somente ao trocar de página chamado depois de CarregarGrid
        /// </summary>
        public event EventHandler PageSizeChanging;
        #endregion

        #region Atributos
        private int? _mockItemCount;
        private int? _mockPageIndex;
        #endregion

        #region Propriedades
        /// <summary>
        /// Grupo de validação
        /// </summary>
        public string ValidationGroup
        {
            set
            {
                //EnsureChildControls();
                ViewState["ValidationGroup"] = value;
            }
            get
            {
                //EnsureChildControls();
                return ViewState["ValidationGroup"] as string;
            }
        }

        #region EmployerPaginacaoStyle
        EmployerPaginacaoGridStyle _EmployerPaginacaoGridStyle;

        /// <summary>
        /// Estilo da paginação
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [NotifyParentProperty(true)]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public EmployerPaginacaoGridStyle EmployerPaginacaoStyle
        {
            get
            {
                if (IsTrackingViewState)
                    ((IStateManager)_EmployerPaginacaoGridStyle).TrackViewState();

                return _EmployerPaginacaoGridStyle;
            }
            set
            {
                _EmployerPaginacaoGridStyle = value;

                if (IsTrackingViewState)
                    ((IStateManager)_EmployerPaginacaoGridStyle).TrackViewState();
            }
        }
        #endregion

        #region MockItemCount
        ///<summary>
        /// Informe a quantidade de registros para a grid calcular a paginação
        ///</summary>
        public int MockItemCount
        {
            get
            {
                if (_mockItemCount == null)
                {
                    if (ViewState["MockItemCount"] == null)
                        MockItemCount = Rows.Count;
                    else
                        MockItemCount = (int)ViewState["MockItemCount"];
                }
                return _mockItemCount.Value;
            }
            set
            {
                _mockItemCount = value;
                ViewState["MockItemCount"] = value;
            }
        }
        #endregion

        #region MockPageIndex
        ///<summary>
        /// Propriedade usada para indicar a página corrente.
        ///</summary>
        public int MockPageIndex
        {
            get
            {
                if (_mockPageIndex == null)
                {
                    if (ViewState["MockPageIndex"] == null)
                        MockPageIndex = PageIndex;
                    else
                        MockPageIndex = (int)ViewState["MockPageIndex"];
                }
                return _mockPageIndex.Value;
            }
            set
            {
                _mockPageIndex = value;
                ViewState["MockPageIndex"] = value;
            }
        }
        #endregion

        #region MockColunaOrdenada
        /// <summary>
        /// Nome da coluna ordenada.<br/>
        /// Caso não tenha valor retorna a primeira coluna que tenha SortExpression
        /// </summary>
        public String MockColunaOrdenada
        {
            get
            {
                if (ViewState["MockColunaOrdenada"] == null)
                {
                    foreach (var coluna in this.Columns)
                    {
                        if (!String.IsNullOrEmpty(((DataControlField)coluna).SortExpression))
                        {
                            return ((DataControlField)coluna).SortExpression;
                        }
                    }
                    return String.Empty;
                }
                return ViewState["MockColunaOrdenada"].ToString();
            }
            set
            {
                ViewState["MockColunaOrdenada"] = value;
            }
        }
        #endregion

        #region MockDirecaoOrdenacao
        /// <summary>
        /// Direção da ordenação da grid
        /// </summary>
        public SortDirection MockDirecaoOrdenacao
        {
            get
            {
                if (ViewState["MockDirecaoOrdenacao"] == null)
                    return SortDirection.Ascending;
                return (SortDirection)ViewState["MockDirecaoOrdenacao"];
            }
            set
            {
                ViewState["MockDirecaoOrdenacao"] = value;
            }
        }
        #endregion

        #region CssHeaderOrderAsc
        /// <summary>
        /// Classe css utilizada no cabeçalho selecinado da gridView na ordem ascendente
        /// </summary>
        public String CssClassHeaderOrderAsc
        {
            private get
            {
                if (ViewState["CssClassHeaderOrderAsc"] == null)
                    return String.Empty;
                return ViewState["CssClassHeaderOrderAsc"].ToString();
            }
            set
            {
                ViewState["CssClassHeaderOrderAsc"] = value;
            }
        }
        #endregion

        #region CssHeaderOrderDesc
        /// <summary>
        /// Classe css utilizada no cabeçalho selecinado da gridView na ordem descendente
        /// </summary>
        public String CssClassHeaderOrderDesc
        {
            private get
            {
                if (ViewState["CssClassHeaderOrderDesc"] == null)
                    return String.Empty;
                return ViewState["CssClassHeaderOrderDesc"].ToString();
            }
            set
            {
                ViewState["CssClassHeaderOrderDesc"] = value;
            }
        }
        #endregion

        #endregion

        #region Metodos

        #region Construtor
        /// <summary>
        /// Construtor padrão.<br/>
        /// Cria um PagerTemplate do tipo EmployerPaginacaoGrid vinculado a grid.
        /// </summary>
        public EmployerGrid()
            : base()
        {
            if (this._EmployerPaginacaoGridStyle == null)
                this._EmployerPaginacaoGridStyle = new EmployerPaginacaoGridStyle();



            if (this.PagerTemplate == null)
                this.PagerTemplate = new EmployerPaginacaoGrid(this);
        }
        #endregion

        #region OnInit
        /// <inheritdoc />
        protected override void OnInit(EventArgs e)
        {
            if (this.PagerTemplate is EmployerPaginacaoGrid)
            {
                string sJs = "Employer.Componentes.UI.Web.Content.js.AjaxClientControlBase.js";
                Type t = typeof(AjaxClientControlBase);
                if (!Page.ClientScript.IsClientScriptBlockRegistered(t, sJs))
                    Page.ClientScript.RegisterClientScriptInclude(sJs, Page.ClientScript.GetWebResourceUrl(t, sJs));

                this.RowDataBound += new GridViewRowEventHandler(EmployerGrid_RowDataBound);
                this.RowCreated += new GridViewRowEventHandler(EmployerGrid_RowCreated);
                this.PageIndexChanging += new GridViewPageEventHandler(EmployerGrid_PageIndexChanging);
                this.Sorting += new GridViewSortEventHandler(EmployerGrid_Sorting);
                this.RowCommand += new GridViewCommandEventHandler(EmployerGrid_RowCommand);
            }

            base.OnInit(e);
        }

        void EmployerGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ("PageQtd".Equals(e.CommandName, StringComparison.OrdinalIgnoreCase))
            {
                this.PageSize = Convert.ToInt32(e.CommandArgument);
                this.MockPageIndex = 0;
                if (PageSizeChanging != null)
                    PageSizeChanging(this, new EventArgs());

                if (CarregarGrid != null)
                    CarregarGrid(this, new EventArgs());
            }
        }

        void EmployerGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (MockColunaOrdenada.Equals(e.SortExpression) && MockDirecaoOrdenacao == SortDirection.Ascending)
            {
                MockColunaOrdenada = e.SortExpression;
                MockDirecaoOrdenacao = SortDirection.Descending;
            }
            else
            {
                MockColunaOrdenada = e.SortExpression;
                MockDirecaoOrdenacao = SortDirection.Ascending;
            }

            //this.DataBind();

            if (CarregarGrid != null)
                CarregarGrid(this, new EventArgs());
        }

        void EmployerGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.MockPageIndex = e.NewPageIndex;
            if (CarregarGrid != null)
                CarregarGrid(this, new EventArgs());
        }

        void EmployerGrid_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (!string.IsNullOrEmpty(this.ValidationGroup))
                {
                    foreach (TableCell cel in e.Row.Cells)
                    {
                        if (cel.Controls.Count > 0 && cel.Controls[0] is LinkButton)
                        {
                            var btn = cel.Controls[0] as LinkButton;
                            if (string.IsNullOrEmpty(btn.OnClientClick))
                                btn.OnClientClick = string.Format("return Page_ClientValidate('{0}')", this.ValidationGroup);
                        }
                    }
                }

                int sortColumnIndex = GetIndiceColunaOrdenar();
                if (sortColumnIndex != -1)
                {
                    if (this.MockDirecaoOrdenacao == SortDirection.Ascending)
                    {
                        if (!String.IsNullOrEmpty(this.CssClassHeaderOrderAsc))
                        {
                            e.Row.Cells[sortColumnIndex].CssClass = this.CssClassHeaderOrderAsc;
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(this.CssClassHeaderOrderDesc))
                        {
                            e.Row.Cells[sortColumnIndex].CssClass = this.CssClassHeaderOrderDesc;
                        }
                    }
                }
            }
        }
        #endregion

        #region EmployerGrid_RowDataBound
        /// <summary>
        /// Evento disparado quando a linha é preenchida
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        void EmployerGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Pager)
            {
                if (this.PagerTemplate is EmployerPaginacaoGrid)
                    ((EmployerPaginacaoGrid)this.PagerTemplate).RowCreated(sender, e);
            }
        }
        #endregion

        #region RaisePostBackEvent
        /// <inheritdoc />
        protected override void RaisePostBackEvent(string eventArgument)
        {
            if (this.PagerTemplate is EmployerPaginacaoGrid)
                ((EmployerPaginacaoGrid)this.PagerTemplate).CustonRaisePostBackEvent(ref eventArgument);

            base.RaisePostBackEvent(eventArgument);
        }
        #endregion

        #region InitializePager
        ///<summary>
        ///Initializes the pager row displayed when the paging feature is enabled.
        ///</summary>
        ///
        ///<param name="columnSpan">The number of columns the pager row should span. </param>
        ///<param name="row">A <see cref="T:System.Web.UI.WebControls.GridViewRow"></see> that represents the pager row to initialize. </param>
        ///<param name="pagedDataSource">A <see cref="T:System.Web.UI.WebControls.PagedDataSource"></see> that represents the data source. </param>
        protected override void InitializePager(GridViewRow row, int columnSpan, PagedDataSource pagedDataSource)
        {
            if (pagedDataSource.IsPagingEnabled && (MockItemCount != pagedDataSource.VirtualCount))
            {
                pagedDataSource.AllowCustomPaging = true;
                pagedDataSource.VirtualCount = MockItemCount;
                pagedDataSource.CurrentPageIndex = MockPageIndex;
                //pagedDataSource.PageCount = (int)Math.Ceiling((decimal)this.MockItemCount / (decimal)this.PageSize);
            }

            base.InitializePager(row, columnSpan, pagedDataSource);

            EmployerPaginacaoGrid objTemplate = this.PagerTemplate as EmployerPaginacaoGrid;
            if (objTemplate != null)
                objTemplate.AjustaPaginacao();
        }
        #endregion

        #region CreateChildControls
        /// <inheritdoc />
        protected override int CreateChildControls
           (System.Collections.IEnumerable dataSource, bool dataBinding)
        {

            PageIndex = MockPageIndex;

            if (!String.IsNullOrEmpty(base.RowStyle.CssClass))
            {
                base.AlternatingRowStyle.CssClass += (String.IsNullOrEmpty(base.AlternatingRowStyle.CssClass) ? "" : " ") + base.RowStyle.CssClass;
            }

            return base.CreateChildControls(dataSource, dataBinding);
        }
        #endregion

        #endregion

        #region GetIndiceColunaOrdenar
        /// <summary>
        /// Retorna o indice da coluna a ordenar
        /// </summary>
        /// <returns>Indice da coluna de ordenação</returns>
        private int GetIndiceColunaOrdenar()
        {
            if (!String.IsNullOrEmpty(MockColunaOrdenada))
            {
                foreach (DataControlField field in this.Columns)
                {
                    if (field.SortExpression.Equals(MockColunaOrdenada))
                        return this.Columns.IndexOf(field);
                }
            }
            return -1;
        }
        #endregion
    }

    #region EmployerPaginacaoGrid
    /// <summary>
    /// Template do paginador
    /// </summary>
    public class EmployerPaginacaoGrid : ITemplate
    {
        #region Atributos
        System.Web.UI.WebControls.Panel _DivPaginacao = null;
        // Div quantidade
        System.Web.UI.WebControls.Panel _divQuantidade = null;
        System.Web.UI.WebControls.DropDownList _ddlQuantidade = null;
        Literal _liExibir = null;
        Literal _liPorPagina = null;

        //_DivPaginacaoInfo
        System.Web.UI.WebControls.Panel _DivPaginacaoInfo = null;
        System.Web.UI.WebControls.Literal _liInfo = null;

        //_DivPaginacaoItens
        System.Web.UI.WebControls.Panel _DivPaginacaoItens = null;
        DataControlLinkButton _btnPrimeiro = null;//new System.Web.UI.WebControls.ImageButton();
        DataControlLinkButton _btnAnterior = null;//new System.Web.UI.WebControls.ImageButton();
        System.Web.UI.WebControls.Label _lblItens = null;
        DataControlLinkButton _btnProximo = null;// new DataControlImageButton(
        DataControlLinkButton _btnUltimo = null;//new System.Web.UI.WebControls.ImageButton();

        //_DivPaginacaoPesq
        System.Web.UI.WebControls.Panel _DivPaginacaoPesq = null;
        //System.Web.UI.WebControls.Label _lblIrPagina = new System.Web.UI.WebControls.Label();
        System.Web.UI.HtmlControls.HtmlGenericControl _lblIrPagina = null;
        TextBox _txtPagina = null;
        DataControlLinkButton _btnOk = null;//new System.Web.UI.WebControls.ImageButton();

        private EmployerGrid _grid = null;
        #endregion

        #region Métodos

        #region CustonRaisePostBackEvent
        /// <summary>
        /// Raise postback customizado
        /// </summary>
        /// <param name="eventArgument">Os argumentos do evento</param>
        internal void CustonRaisePostBackEvent(ref string eventArgument)
        {
            if (eventArgument != null && eventArgument.Equals("PageOk$", StringComparison.InvariantCultureIgnoreCase))
            {
                int iTemp = 0;
                if (int.TryParse(this._txtPagina.Text, out iTemp) == false)
                    iTemp = 1;
                eventArgument = "Page$" + iTemp.ToString();
            }

            if (eventArgument != null && eventArgument.IndexOf("PageQtd", StringComparison.OrdinalIgnoreCase) > -1)
            {
                eventArgument = "PageQtd$" + _ddlQuantidade.SelectedValue;
            }
        }
        #endregion

        #region EmployerPaginacaoGrid
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="grid">Um objeto EmployerGrid</param>
        public EmployerPaginacaoGrid(EmployerGrid grid)
        {
            _grid = grid;
        }
        #endregion

        #region AjustaCss
        /// <summary>
        /// Realiza ajustes no Css
        /// </summary>
        private void AjustaCss()
        {
            EmployerGrid eGrid = _grid;

            if (eGrid.EmployerPaginacaoStyle == null)
                return;

            _btnPrimeiro.ImageUrl = eGrid.EmployerPaginacaoStyle.PrimeiroImageUrl;
            _btnAnterior.ImageUrl = eGrid.EmployerPaginacaoStyle.AnteriorImageUrl;
            _btnProximo.ImageUrl = eGrid.EmployerPaginacaoStyle.ProximoImageUrl;
            _btnUltimo.ImageUrl = eGrid.EmployerPaginacaoStyle.UltimoImageUrl;

            _btnPrimeiro.CssClass =
            _btnAnterior.CssClass =
            _btnProximo.CssClass =
            _btnUltimo.CssClass = eGrid.EmployerPaginacaoStyle.BotoesPaginacaoCss;

            _DivPaginacao.CssClass = eGrid.EmployerPaginacaoStyle.DivPaginacaoCss;
            _DivPaginacaoInfo.CssClass = eGrid.EmployerPaginacaoStyle.DivPaginacaoInfoCss;
            _DivPaginacaoItens.CssClass = eGrid.EmployerPaginacaoStyle.DivPaginacaoPesquisaCss;
            _DivPaginacaoPesq.CssClass = eGrid.EmployerPaginacaoStyle.DivPaginacaoPesquisaBotaoOkCss;
            _divQuantidade.CssClass = eGrid.EmployerPaginacaoStyle.DivPaginacaoQuantidadePaginasCss;

            _lblItens.CssClass = eGrid.EmployerPaginacaoStyle.SpanPaginacaoInfoCss;

            _btnOk.CssClass = eGrid.EmployerPaginacaoStyle.BotaoOkCss;
            _btnOk.ImageUrl = eGrid.EmployerPaginacaoStyle.BotaoOkImageUrl;

            _ddlQuantidade.CssClass = eGrid.EmployerPaginacaoStyle.DropDownPaginasCss;

            _txtPagina.CssClass = eGrid.EmployerPaginacaoStyle.TextBoxIrParaCss;

            foreach (WebControl e in _lblItens.Controls)
            {
                if (e is HyperLink)
                    e.CssClass = eGrid.EmployerPaginacaoStyle.LinkSelecionadoPaginacaoInfoCss;
            }
        }
        #endregion



        #region VinculaPaginacao
        /// <summary>
        /// Vincula um botão a uma página
        /// </summary>
        /// <param name="btn">Botão</param>
        /// <param name="Prefixo">Prefixo usado na vinculação</param>
        /// <param name="iPagina">Id da página a ser vinculada</param>
        private void VinculaPaginacao(DataControlLinkButton btn, string Prefixo, int iPagina)
        {
            btn.ID = Prefixo + "Pagina_" + iPagina.ToString();
            //_btnPrimeiro.Text = iPagina.ToString();
            btn.CommandArgument = iPagina.ToString();
            btn.CommandName = "Page";
        }
        #endregion

        #region AddPagina
        /// <summary>
        /// Adiciona uma página
        /// </summary>
        /// <param name="iPagina">Id da página</param>
        /// <param name="cCurrentpage">True se for a página atual</param>
        private void AddPagina(int iPagina, bool cCurrentpage)
        {
            WebControl lnkPagina;
            if (cCurrentpage)
            {
                lnkPagina = new HyperLink();
                HyperLink lnkPagina2 = lnkPagina as HyperLink;
                lnkPagina2.ID = "Pagina_" + iPagina.ToString();
                lnkPagina2.Text = iPagina.ToString();
            }
            else
            {
                lnkPagina = new DataControlLinkButton(_grid);
                //((LinkButton)lnkPagina).CausesValidation = false;

                LinkButton lnkPagina2 = lnkPagina as LinkButton;

                lnkPagina2.ID = "Pagina_" + iPagina.ToString();
                lnkPagina2.Text = iPagina.ToString();
                lnkPagina2.CommandArgument = iPagina.ToString();
                lnkPagina2.CommandName = "Page";

                if (!string.IsNullOrEmpty(_grid.ValidationGroup))
                {
                    lnkPagina2.OnClientClick =
                        string.Format("return Page_ClientValidate('{0}')", _grid.ValidationGroup);
                }
            }

            _lblItens.Controls.Add(lnkPagina);
        }
        #endregion

        #region CriaDivPaginacaoPesq
        /// <summary>
        /// Cria div da paginação de pesquisa
        /// </summary>
        private void CriaDivPaginacaoPesq()
        {
            _lblIrPagina.ID = "lblIrPagina";
            _lblIrPagina.InnerText = "Ir para a página";

            _btnOk.ID = "btnOk";
            _btnOk.CommandName = "PageOk";

            if (!string.IsNullOrEmpty(_grid.ValidationGroup))
            {
                _btnOk.OnClientClick =
                    string.Format("return Page_ClientValidate('{0}')", _grid.ValidationGroup);
            }

            _txtPagina.ID = "txtPagina";
            _txtPagina.ToolTip = "Insira o Número da Página";

            _DivPaginacaoPesq.Controls.Add(_lblIrPagina);
            _DivPaginacaoPesq.Controls.Add(_txtPagina);
            _DivPaginacaoPesq.Controls.Add(_btnOk);
        }
        #endregion

        #region CriaDivPaginacaoInfo
        /// <summary>
        /// Cria div da paginação de informação
        /// </summary>
        private void CriaDivPaginacaoInfo()
        {
            _DivPaginacaoInfo.Controls.Add(_liInfo);
            _liInfo.ID = "liInfo";
        }
        #endregion

        #region CriaDivQuantidade
        private void CriaDivQuantidade()
        {
            _divQuantidade.ID = "divQuantidade";
            _ddlQuantidade.ID = "ddlQuantidade";

            _ddlQuantidade.Items.Add("5");
            _ddlQuantidade.Items.Add("10");
            if (15 < _grid.MockItemCount)
                _ddlQuantidade.Items.Add("15");
            if (20 < _grid.MockItemCount)
                _ddlQuantidade.Items.Add("20");
            if (25 < _grid.MockItemCount)
                _ddlQuantidade.Items.Add("25");
            if (50 < _grid.MockItemCount)
                _ddlQuantidade.Items.Add("50");
            if (75 < _grid.MockItemCount)
                _ddlQuantidade.Items.Add("75");
            if (100 < _grid.MockItemCount)
                _ddlQuantidade.Items.Add("100");

            if (_ddlQuantidade.Items.FindByValue(Convert.ToString(_grid.PageSize)) == null)
            {
                _ddlQuantidade.SelectedValue = "10";
            }
            else
            {
                _ddlQuantidade.SelectedValue = Convert.ToString(_grid.PageSize);
            }

            if (!string.IsNullOrEmpty(_grid.ValidationGroup))
                _ddlQuantidade.Attributes["onchange"] =
                     string.Format("javascript:return (Page_ClientValidate('{1}') ? __doPostBack('{0}','PageQtd$') : false)",
                        _grid.UniqueID, _grid.ValidationGroup);
            else
                _ddlQuantidade.Attributes["onchange"] =
                string.Format("javascript:__doPostBack('{0}','PageQtd$')", _grid.UniqueID);

            _liExibir.ID = "liExibir";
            _liExibir.Text = "Exibir ";

            _liPorPagina.ID = "liPorPag";
            _liPorPagina.Text = " por página";

            _divQuantidade.Controls.Add(_liExibir);
            _divQuantidade.Controls.Add(_ddlQuantidade);
            _divQuantidade.Controls.Add(_liPorPagina);
        }
        #endregion

        #region CriaDivPaginacao
        /// <summary>
        /// Cria div da paginação
        /// </summary>
        private void CriaDivPaginacao()
        {
            _DivPaginacao.ID = "DivPaginacao";
            _DivPaginacaoInfo.ID = "DivPaginacaoInfo";
            _DivPaginacaoItens.ID = "DivPaginacaoItens";
            _DivPaginacaoPesq.ID = "DivPaginacaoPesq";

            _DivPaginacao.EnableTheming = false;

            _DivPaginacao.Controls.Add(_divQuantidade);
            _DivPaginacao.Controls.Add(_DivPaginacaoInfo);
            _DivPaginacao.Controls.Add(_DivPaginacaoItens);
            _DivPaginacao.Controls.Add(_DivPaginacaoPesq);
        }
        #endregion

        #region CriaDivPaginacaoItens
        /// <summary>
        /// Cria div dos ítens da paginação
        /// </summary>
        private void CriaDivPaginacaoItens()
        {
            _btnPrimeiro.ID = "btnPrimeiro";
            _btnAnterior.ID = "btnAnterior";
            _btnProximo.ID = "btnProximo";
            _btnUltimo.ID = "btnUltimo";

            _btnPrimeiro.CommandName =
            _btnAnterior.CommandName =
            _btnProximo.CommandName =
            _btnUltimo.CommandName = "Page";

            _btnPrimeiro.EnableTheming =
            _btnAnterior.EnableTheming =
            _btnProximo.EnableTheming =
            _btnUltimo.EnableTheming = false;

            if (!string.IsNullOrEmpty(_grid.ValidationGroup))
            {
                _btnPrimeiro.OnClientClick =
                    _btnAnterior.OnClientClick =
                    _btnProximo.OnClientClick =
                    _btnUltimo.OnClientClick = string.Format("return Page_ClientValidate('{0}')", _grid.ValidationGroup);
            }

            _DivPaginacaoItens.Controls.Add(_btnPrimeiro);
            _DivPaginacaoItens.Controls.Add(_btnAnterior);
            _DivPaginacaoItens.Controls.Add(_lblItens);
            _DivPaginacaoItens.Controls.Add(_btnProximo);
            _DivPaginacaoItens.Controls.Add(_btnUltimo);
        }
        #endregion

        #region AjustaPaginacao
        /// <summary>
        /// Ajusta a paginação
        /// </summary>
        internal void AjustaPaginacao()
        {
            int CurrentPageNumber = _grid.MockPageIndex;
            int PageNumber = CurrentPageNumber;
            int PageListNumber = PageNumber;
            int NumberOfPages = (int)Math.Ceiling((decimal)_grid.MockItemCount / (decimal)_grid.PageSize);
            int NumberOfPagesGrid = NumberOfPages;
            int PageListSize = 3;


            _liInfo.Text = string.Format("Exibindo página {0} de {1}",
                CurrentPageNumber + 1,
                NumberOfPagesGrid);


            int FirstPageListNumber = 0;
            int NumberOfPageLists = (int)Math.Ceiling(((decimal)NumberOfPages) / ((decimal)PageListSize - 1));
            for (int i = 1; i <= NumberOfPageLists; i++)
            {
                int MaxPageListNumber = PageListSize * i;
                if (PageNumber < MaxPageListNumber - i)
                {
                    PageListNumber = i;
                    FirstPageListNumber = (PageListNumber * (PageListSize - 1)) - (PageListSize - 1) + 1;
                    break;
                }
            }

            if (FirstPageListNumber == NumberOfPages)
                FirstPageListNumber -= PageListSize - 1;

            //bool AddPrevDots = false;
            //bool AddNextDots = false;
            //bool AddNextText = false;
            //bool AddPrevText = false;

            //Get first number in page list   

            if (FirstPageListNumber <= 0)
                FirstPageListNumber = 1;

            //Get last number in pages list 
            int LastPageListNumber = FirstPageListNumber + (PageListSize - 1);
            //Check to see if its beyond the first page list and add Prev. dots if it is. 
            //Also check to see of there are more pages ahead of the current page list and add Next dots if there is.
            int PageCount = 0;
            if (LastPageListNumber > NumberOfPages)
            {
                PageCount = NumberOfPages - FirstPageListNumber;
                //if (PageListNumber != 1)
                //    AddPrevDots = true;
            }
            else
            {
                PageCount = PageListSize - 1;
                //if (PageListNumber != 1)
                //    AddPrevDots = true;
                //if (LastPageListNumber < NumberOfPages)
                //    if (NumberOfPages > PageListSize)
                //        AddNextDots = true;
            }


            _lblItens.Controls.Clear();


            int PageNumberLabel;
            for (int i = 0; i <= PageCount; i++)
            {
                PageNumberLabel = FirstPageListNumber + i;
                AddPagina(PageNumberLabel, PageNumberLabel == CurrentPageNumber + 1);
            }

            CurrentPageNumber += 1;
            int OldPage = CurrentPageNumber - 1 > 1 ? CurrentPageNumber - 1 : 1;
            int NexPage = CurrentPageNumber + 1 < NumberOfPagesGrid ? CurrentPageNumber + 1 : NumberOfPagesGrid;

            VinculaPaginacao(_btnPrimeiro, "Primeiro", 1);
            VinculaPaginacao(_btnAnterior, "Anterior", OldPage);
            VinculaPaginacao(_btnProximo, "Proximo", NexPage);
            VinculaPaginacao(_btnUltimo, "Ultimo", NumberOfPagesGrid);

            string sValOk = Guid.NewGuid().ToString();

            _txtPagina.Attributes["onkeyup"] = string.Format("AjaxClientControlBase.RestringeNumeroUp(this,1,{0});", NumberOfPagesGrid);

            _btnOk.ValidationGroup = sValOk;

            AjustaCss();
        }
        #endregion

        #region RowCreated
        /// <summary>
        /// Evento disparado quando a linha é criada
        /// </summary>
        /// <param name="sender">O objeto que disparou o evento</param>
        /// <param name="e">Os argumentos do evento</param>
        public void RowCreated(object sender, GridViewRowEventArgs e)
        {
            _txtPagina.Attributes["onkeypress"] = "AjaxClientControlBase.SoNumero(event,$get('" + _btnOk.ClientID + "'));";
        }
        #endregion

        #region InstantiateIn
        /// <summary>
        /// Instancia o template no container parametrizado
        /// </summary>
        /// <param name="container">O container parametrizado</param>
        public void InstantiateIn(Control container)
        {
            //_DivPaginacao.Parent
            _DivPaginacao = new System.Web.UI.WebControls.Panel();
            _DivPaginacaoInfo = new System.Web.UI.WebControls.Panel();
            _DivPaginacaoItens = new System.Web.UI.WebControls.Panel();
            _lblItens = new System.Web.UI.WebControls.Label();
            _DivPaginacaoPesq = new System.Web.UI.WebControls.Panel();
            _lblIrPagina = new System.Web.UI.HtmlControls.HtmlGenericControl("label");
            _txtPagina = new TextBox();
            _liInfo = new Literal();
            _liExibir = new Literal();
            _liPorPagina = new Literal();

            _divQuantidade = new System.Web.UI.WebControls.Panel();
            _ddlQuantidade = new DropDownList();

            _btnPrimeiro = new DataControlLinkButton(_grid);
            _btnAnterior = new DataControlLinkButton(_grid);
            _btnProximo = new DataControlLinkButton(_grid);
            _btnUltimo = new DataControlLinkButton(_grid);
            _btnOk = new DataControlLinkButton(_grid);

            CriaDivPaginacao();
            CriaDivQuantidade();
            CriaDivPaginacaoInfo();
            CriaDivPaginacaoItens();
            CriaDivPaginacaoPesq();

            container.Controls.Add(_DivPaginacao);
        }


        #endregion

        #endregion

        #region Botões Especiais
        [SupportsEventValidation]
        internal sealed class DataControlImageButton : ImageButton
        {
            // Fields
            private string _callbackArgument;
            private IPostBackContainer _container;
            private bool _enableCallback;

            // Methods
            //[System.Runtime.TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            internal DataControlImageButton(IPostBackContainer container)
            {
                this._container = container;
            }

            internal void EnableCallback(string argument)
            {
                this._enableCallback = true;
                this._callbackArgument = argument;
            }

            protected sealed override PostBackOptions GetPostBackOptions()
            {
                if (this._container != null)
                {
                    return this._container.GetPostBackOptions(this);
                }
                return base.GetPostBackOptions();
            }

            protected override void Render(HtmlTextWriter writer)
            {
                this.SetCallbackProperties();
                base.Render(writer);
            }

            private void SetCallbackProperties()
            {
                if (this._enableCallback)
                {
                    ICallbackContainer container = this._container as ICallbackContainer;
                    if (container != null)
                    {
                        string callbackScript = container.GetCallbackScript(this, this._callbackArgument);
                        if (!string.IsNullOrEmpty(callbackScript))
                        {
                            this.OnClientClick = callbackScript;
                        }
                    }
                }
            }

            // Properties
            public override bool CausesValidation
            {
                get
                {
                    return false;
                }
                set
                {
                    //throw new NotSupportedException("CannotSetValidationOnDataControlButtons");
                }
            }
        }

        [SupportsEventValidation]
        internal sealed class DataControlLinkButton : LinkButton
        {
            // Fields
            private string _callbackArgument;
            private IPostBackContainer _container;
            private bool _enableCallback;

            public string ImageUrl
            {
                get { return ViewState["ImageUrl"] != null ? (string)ViewState["ImageUrl"] : string.Empty; }
                set { ViewState["ImageUrl"] = value; }
            }

            // Methods
            //[System.Runtime.TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            internal DataControlLinkButton(IPostBackContainer container)
            {
                this._container = container;
            }

            internal void EnableCallback(string argument)
            {
                this._enableCallback = true;
                this._callbackArgument = argument;
            }

            protected override void CreateChildControls()
            {
                this.Controls.Clear();

                if (!string.IsNullOrEmpty(ImageUrl))
                {
                    Image img = new Image { ImageUrl = ImageUrl };
                    this.Controls.Add(img);
                }

                base.CreateChildControls();
            }

            protected sealed override PostBackOptions GetPostBackOptions()
            {
                if (this._container != null)
                {
                    return this._container.GetPostBackOptions(this);
                }
                return base.GetPostBackOptions();
            }

            protected override void Render(HtmlTextWriter writer)
            {
                this.SetCallbackProperties();
                base.Render(writer);
            }

            private void SetCallbackProperties()
            {
                if (this._enableCallback)
                {
                    ICallbackContainer container = this._container as ICallbackContainer;
                    if (container != null)
                    {
                        string callbackScript = container.GetCallbackScript(this, this._callbackArgument);
                        if (!string.IsNullOrEmpty(callbackScript))
                        {
                            this.OnClientClick = callbackScript;
                        }
                    }
                }
            }

            // Properties
            public override bool CausesValidation
            {
                get
                {
                    return false;
                }
                set
                {
                    //throw new NotSupportedException("CannotSetValidationOnDataControlButtons");
                }
            }
        }
        [SupportsEventValidation]
        internal sealed class DataControTextBox : LinkButton
        {
            // Fields
            private string _callbackArgument;
            private IPostBackContainer _container;
            private bool _enableCallback;

            // Methods
            //[System.Runtime.TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
            internal DataControTextBox(IPostBackContainer container)
            {
                this._container = container;
            }

            internal void EnableCallback(string argument)
            {
                this._enableCallback = true;
                this._callbackArgument = argument;
            }

            protected sealed override PostBackOptions GetPostBackOptions()
            {
                if (this._container != null)
                {
                    return this._container.GetPostBackOptions(this);
                }
                return base.GetPostBackOptions();
            }

            protected override void Render(HtmlTextWriter writer)
            {
                this.SetCallbackProperties();
                base.Render(writer);
            }

            private void SetCallbackProperties()
            {
                if (this._enableCallback)
                {
                    ICallbackContainer container = this._container as ICallbackContainer;
                    if (container != null)
                    {
                        string callbackScript = container.GetCallbackScript(this, this._callbackArgument);
                        if (!string.IsNullOrEmpty(callbackScript))
                        {
                            this.OnClientClick = callbackScript;
                        }
                    }
                }
            }

            // Properties
            public override bool CausesValidation
            {
                get
                {
                    return false;
                }
                set
                {
                    //throw new NotSupportedException("CannotSetValidationOnDataControlButtons");
                }
            }
        }
        #endregion
    }
    #endregion

    #region EmployerPaginacaoGridStyle
    /// <summary>
    /// Classe que define o estilo da paginação
    /// </summary>
    [Serializable]
    public class EmployerPaginacaoGridStyle : TableItemStyle
    {
        #region DivPaginacaoCss
        /// <summary>
        /// Classe css da div de paginação
        /// </summary>
        public string DivPaginacaoCss
        {
            get { return this.ViewState["DivPaginacaoCss"] as string; }
            set { this.ViewState["DivPaginacaoCss"] = value; }
        }
        #endregion

        #region DivPaginacaoInfoCss
        /// <summary>
        /// Classe css da div de informações de paginação
        /// </summary>
        public string DivPaginacaoInfoCss
        {
            get { return this.ViewState["DivPaginacaoInfoCss"] as string; }
            set { this.ViewState["DivPaginacaoInfoCss"] = value; }
        }
        #endregion

        #region DivPaginacaoPesquisaCss
        /// <summary>
        /// Classe css da div de pesquisa de paginação
        /// </summary>
        public string DivPaginacaoPesquisaCss
        {
            get { return this.ViewState["DivPaginacaoPesquisaCss"] as string; }
            set { this.ViewState["DivPaginacaoPesquisaCss"] = value; }
        }
        #endregion

        #region DivPaginacaoPesquisaBotaoOkCss
        /// <summary>
        /// Classe css do botão de pesquisa
        /// </summary>
        public string DivPaginacaoPesquisaBotaoOkCss
        {
            get { return this.ViewState["DivPaginacaoPesquisaBotaoOkCss"] as string; }
            set { this.ViewState["DivPaginacaoPesquisaBotaoOkCss"] = value; }
        }
        #endregion

        #region DivPaginacaoQuantidadePaginas
        /// <summary>
        /// Div da quantidade de páginas
        /// </summary>
        public string DivPaginacaoQuantidadePaginasCss
        {
            get { return this.ViewState["DivPaginacaoQuantidadePaginasCss"] as string; }
            set { this.ViewState["DivPaginacaoQuantidadePaginasCss"] = value; }
        }
        #endregion

        #region BotaoOkCss
        /// <summary>
        /// Classe css do botão "OK"
        /// </summary>
        public string BotaoOkCss
        {
            get { return this.ViewState["BotaoOkCss"] as string; }
            set { this.ViewState["BotaoOkCss"] = value; }
        }
        #endregion

        #region BotaoOkQuantidadeCss
        /// <summary>
        /// Classe css do botão "OK"
        /// </summary>
        public string BotaoOkQuantidadeCss
        {
            get { return (this.ViewState["BotaoOkQuantidadeCss"] == null) ? BotaoOkCss : Convert.ToString(this.ViewState["BotaoOkQuantidadeCss"]); }
            set { this.ViewState["BotaoOkQuantidadeCss"] = value; }
        }
        #endregion

        #region TextBoxIrParaCss
        /// <summary>
        /// Classe css do textbox
        /// </summary>
        public string TextBoxIrParaCss
        {
            get { return this.ViewState["TextBoxIrParaCss"] as string; }
            set { this.ViewState["TextBoxIrParaCss"] = value; }
        }
        #endregion

        #region DropDownPaginasCss
        /// <summary>
        /// Css class da drop down
        /// </summary>
        public string DropDownPaginasCss
        {
            get { return this.ViewState["DropDownPaginasCss"] as string; }
            set { this.ViewState["DropDownPaginasCss"] = value; }
        }
        #endregion

        #region BotaoOkImageUrl
        /// <summary>
        /// Caminho para a imagem do botão ok
        /// </summary>
        [DefaultValue("")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Bindable(true)]
        public string BotaoOkImageUrl
        {
            get { return this.ViewState["BotaoOkImageUrl"] as string; }
            set { this.ViewState["BotaoOkImageUrl"] = value; }
        }
        #endregion

        #region SpanPaginacaoInfoCss
        /// <summary>
        /// Classe css da span de informações de paginação
        /// </summary>
        public string SpanPaginacaoInfoCss
        {
            get { return this.ViewState["SpanPaginacaoInfoCss"] as string; }
            set { this.ViewState["SpanPaginacaoInfoCss"] = value; }
        }
        #endregion

        #region LinkSelecionadoPaginacaoInfoCss
        /// <summary>
        /// Classe css do link selecionado
        /// </summary>
        public string LinkSelecionadoPaginacaoInfoCss
        {
            get { return this.ViewState["LinkSelecionadoPaginacaoInfoCss"] as string; }
            set { this.ViewState["LinkSelecionadoPaginacaoInfoCss"] = value; }
        }
        #endregion

        #region BotoesPaginacaoCss
        /// <summary>
        /// Classe css dos botões de paginação
        /// </summary>
        public string BotoesPaginacaoCss
        {
            get { return this.ViewState["BotoesPaginacaoCss"] as string; }
            set { this.ViewState["BotoesPaginacaoCss"] = value; }
        }
        #endregion

        #region PrimeiroImageUrl
        /// <summary>
        /// Caminho para a imagem do botão "primeiro"
        /// </summary>
        [DefaultValue("")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Bindable(true)]
        public string PrimeiroImageUrl
        {
            get { return this.ViewState["PrimeiroImageUrl"] as string; }
            set { this.ViewState["PrimeiroImageUrl"] = value; }
        }
        #endregion

        #region AnteriorImageUrl
        /// <summary>
        /// Caminho para a imagem do botão "anterior"
        /// </summary>
        [DefaultValue("")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Bindable(true)]
        public string AnteriorImageUrl
        {
            get { return this.ViewState["AnteriorImageUrl"] as string; }
            set { this.ViewState["AnteriorImageUrl"] = value; }
        }
        #endregion

        #region ProximoImageUrl
        /// <summary>
        /// Caminho para a imagem do botão "próximo"
        /// </summary>
        [DefaultValue("")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Bindable(true)]
        public string ProximoImageUrl
        {
            get { return this.ViewState["ProximoImageUrl"] as string; }
            set { this.ViewState["ProximoImageUrl"] = value; }
        }
        #endregion

        #region UltimoImageUrl
        /// <summary>
        /// Caminho para a imagem do botão "último"
        /// </summary>
        [DefaultValue("")]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Bindable(true)]
        public string UltimoImageUrl
        {
            get { return this.ViewState["UltimoImageUrl"] as string; }
            set { this.ViewState["UltimoImageUrl"] = value; }
        }
        #endregion



    }
    #endregion
}
