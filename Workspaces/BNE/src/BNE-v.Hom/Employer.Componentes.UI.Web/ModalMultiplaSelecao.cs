using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Data;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Modal de multipla seleção de arquivos usado no EmployerMultiplaSelecao
    /// </summary>
    #pragma warning disable 1591
    public class ModalMultiplaSelecao : ControlModalBuscaBase
    {
        internal delegate object HandlerRetornaDadosItens(bool comPaginacao);
        internal delegate void HandlerAdiconarColunas(EmployerGrid grid);
        internal delegate int HandlerGetKey(object DataItem);

        internal event HandlerRetornaDadosItens RetornaDadosItens;
        internal event HandlerAdiconarColunas AdiconarColunas;
        internal event HandlerGetKey GetKey;

        public event EventHandler SelecionarItens;

        #region Atributos
        HiddenField _hdf = new HiddenField { ID = "hdnModal" };
        DropDownList dropListSelecionarItens = new DropDownList { ID = "dropListSelecionarItens", CausesValidation = false };
        System.Web.UI.WebControls.Button _btnAdicionarSelecionados = new System.Web.UI.WebControls.Button { ID = "btnAdicionarSelecionados", CausesValidation = false };

        //EmployerMultiplaSelecao _pai;
        private bool _LimparFechar = true;

        #endregion

        #region Propriedades
        /// <summary>
        /// Ids dos itens selecionados na modal
        /// </summary>
        public List<int> ListaItensSelecionados
        {
            get
            {
                EnsureChildControls();
                if (ViewState["ListaItensSelecionados"] == null)
                    ViewState["ListaItensSelecionados"] = new List<int>();
                return ViewState["ListaItensSelecionados"] as List<int>;
            }
            set { EnsureChildControls(); ViewState["ListaItensSelecionados"] = value; }
        }

        /// <summary>
        /// Texto do botão adicionar da modal
        /// </summary>
        public string TextoBotaoAdicionarSelecionados
        {
            get { EnsureChildControls(); return _btnAdicionarSelecionados.Text; }
            set { EnsureChildControls(); _btnAdicionarSelecionados.Text = value; }
        }
        #endregion

        /// <inheritdoc/>
        public ModalMultiplaSelecao() : base()
        {
            this.Pesquisar += new ControlModalBuscaBase.HandlerPesquisar(_ModalASelecionar_Pesquisar);
            this.GridBusca.RowDataBound += new GridViewRowEventHandler(GridBusca_RowDataBound);
            this.InformacaoFiltros += new ControlModalBuscaBase.PainelCustomizavel(_ModalASelecionar_InformacaoFiltros);
            this.PosCreateChildControls += new EventHandler(_ModalASelecionar_PosCreateChildControls);
            this.Fechar += new EventHandler(_ModalASelecionar_Fechar);
            this.Limpar += new EventHandler(_ModalASelecionar_Limpar);
            this.InformacaoPainelBotoesAntesBotaoLimpar += new ControlModalBuscaBase.HandlerContainer(_ModalASelecionar_InformacaoPainelBotoesAntesBotaoLimpar);
        }

        #region Métodos

        #region Invocado de evento

        #region da classe base
        void GridBusca_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var oKey = GetKey(e.Row.DataItem);

                var chkSelecionar = e.Row.FindControl("chkSelecionar") as CheckBox;
                chkSelecionar.Attributes["Codigo"] = oKey.ToString();

                chkSelecionar.Checked = ListaItensSelecionados.Contains(oKey);
            }
        }

        void _ModalASelecionar_Pesquisar(System.Web.UI.WebControls.Button BtnSender, out object DadosBusca)
        {
            LerGrid();

            DadosBusca = RetornaDadosItens(true);
        }

        void _ModalASelecionar_InformacaoPainelBotoesAntesBotaoLimpar(Control container)
        {
            container.Controls.Add(_btnAdicionarSelecionados);
        }

        void _ModalASelecionar_Limpar(object sender, EventArgs e)
        {
            ListaItensSelecionados.Clear();
            GridBusca.DataSource = null;
            GridBusca.DataBind();
        }

        void _ModalASelecionar_Fechar(object sender, EventArgs e)
        {
            if (_LimparFechar)
                ListaItensSelecionados.Clear();
        }

        void _ModalASelecionar_PosCreateChildControls(object sender, EventArgs e)
        {
            
        }

        void _ModalASelecionar_InformacaoFiltros(System.Web.UI.WebControls.Panel painel)
        {
            dropListSelecionarItens.AutoPostBack = true;

            dropListSelecionarItens.Items.Clear();
            dropListSelecionarItens.Items.Add(new ListItem { Text = "Selecione", Value = "0" });
            dropListSelecionarItens.Items.Add(new ListItem { Text = "Desmarcar todos", Value = "1" });
            dropListSelecionarItens.Items.Add(new ListItem { Text = "Marcar todos", Value = "2" });
            dropListSelecionarItens.Items.Add(new ListItem { Text = "Desmarcar todos desta página", Value = "3" });
            dropListSelecionarItens.Items.Add(new ListItem { Text = "Marcar todos desta página", Value = "4" });

            painel.Controls.Add(dropListSelecionarItens);
        }
        #endregion

        void _btnAdicionarSelecionados_Click(object sender, EventArgs e)
        {
            _LimparFechar = false;
            this.LerGrid();
            base.Close();

            SelecionarItens(this, e);

            this.ListaItensSelecionados.Clear();
        }

        void dropListSelecionarItens_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dropListSelecionarItens.SelectedValue == "0")
                return;

            //throw new NotImplementedException();
            if (dropListSelecionarItens.SelectedValue == "1" || dropListSelecionarItens.SelectedValue == "2")
            {
                ListaItensSelecionados.Clear();
                this.GridBusca.PageIndex = 0;
            }

            this.GridBusca.SelectedIndex = -1;

            if (dropListSelecionarItens.SelectedValue == "2")
            {
                var objDados = RetornaDadosItens(false);
                var dados = objDados is DataTable ? ((DataTable)objDados).Rows as IEnumerable : objDados as IEnumerable;

                foreach (var dado in dados)
                {
                    var id = GetKey(dado);
                    ListaItensSelecionados.Add(id);
                }
            }
            else if (dropListSelecionarItens.SelectedValue == "3")
            {
                List<int> listaPagina = new List<int>();
                LerGrid(listaPagina, false);

                listaPagina.ForEach(r => ListaItensSelecionados.
                    Remove(r));
            }
            else if (dropListSelecionarItens.SelectedValue == "4")
            {
                List<int> listaPagina = new List<int>();
                foreach (GridViewRow row in this.GridBusca.Rows)
                {
                    var chkSelecionar = row.FindControl("chkSelecionar") as CheckBox;
                    var id = int.Parse(chkSelecionar.Attributes["Codigo"]);
                    listaPagina.Add(id);
                }

                foreach (var id in listaPagina)
                {
                    if (!ListaItensSelecionados.Contains(id))
                        ListaItensSelecionados.Add(id);
                }
            }

            this.GridBusca.DataSource = RetornaDadosItens(true);
            this.GridBusca.DataBind();

            dropListSelecionarItens.SelectedValue = "0";
        }

        #endregion

        #region Sobrescritos
        /// <inheritdoc/>
        protected override void OnInit(EventArgs e)
        {
            //_Modal.RetornarAutoComplete += new ControlModalBuscaMultiplaSelecaoBase.HandlerAutoComplete(_Modal_RetornarAutoComplete);
            

            dropListSelecionarItens.SelectedIndexChanged += new EventHandler(dropListSelecionarItens_SelectedIndexChanged);
            _btnAdicionarSelecionados.Click += new EventHandler(_btnAdicionarSelecionados_Click);

            base.OnInit(e);
        }

        /// <inheritdoc/>
        protected override void CreateChildControls()
        {
            this.TargetControlID = _hdf.ID;

            base.CreateChildControls();

            this.Controls.Add(_hdf);

            AdiconarColunas(this.GridBusca);

            GridBusca.Columns.Add(new TemplateField { ItemTemplate = new Employer.Componentes.UI.Web.EmployerMultiplaSelecao.ColunaCheckBox() });
        }
        #endregion

        #region privados

        private void LerGrid()
        {
            LerGrid(ListaItensSelecionados);
        }

        private void LerGrid(List<int> lista, bool remover = true)
        {
            //Ler a grid
            foreach (GridViewRow row in this.GridBusca.Rows)
            {
                var chkSelecionar = row.FindControl("chkSelecionar") as CheckBox;
                var id = int.Parse(chkSelecionar.Attributes["Codigo"]);

                if (chkSelecionar.Checked && !lista.Contains(id))
                    lista.Add(id);
                else if (remover && !chkSelecionar.Checked && lista.Contains(id))
                    lista.Remove(id);
            }
        }

        #endregion

        #endregion
    }
}
