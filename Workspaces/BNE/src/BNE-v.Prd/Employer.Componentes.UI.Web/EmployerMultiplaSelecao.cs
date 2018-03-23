using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente base para controlar a multipla seleção de registros.
    /// </summary>
    public abstract class EmployerMultiplaSelecao : CompositeControl
    {
        /// <summary>
        /// Evento disparado antes de abrir a modal para adicionar os item.
        /// </summary>
        public event EventHandler AdicionarClick;

        #region Atributos
        ModalMultiplaSelecao _ModalASelecionar = new ModalMultiplaSelecao { ID = "ModalASelecionar" };
        ModalMultiplaSelecao _ModalSelecionados = new ModalMultiplaSelecao { ID = "ModalSelecionados" };
        
        ImageButton _btnAbrirModalAdd = new ImageButton { ID = "btnAbrirModalAdd" };
        System.Web.UI.WebControls.Button _btnAbrirModalAdicionados = new System.Web.UI.WebControls.Button { ID = "btnAbrirModalAdicionados", CausesValidation = false };
        #endregion

        #region Propriedades
        /// <summary>
        /// Lista com os ids dos itens selecionados
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
        /// Modal de itens novos que podem ser adicionados
        /// </summary>
        public ModalMultiplaSelecao ModalBuscaASelecionar
        {
            get { EnsureChildControls(); return _ModalASelecionar; }
        }

        /// <summary>
        /// Modal de itens já adicionados que podem ser removidos
        /// </summary>
        public ModalMultiplaSelecao ModalBuscaSelecionados
        {
            get { EnsureChildControls(); return _ModalSelecionados; }
        }

        /// <summary>
        /// Texto do botão que mostra a modal de visualização dos itens selecionados.
        /// </summary>
        public string TextoBotaoSelecionados
        {
            get 
            { 
                EnsureChildControls();
                if (ViewState["TextoBotaoSelecionados"] == null)
                    ViewState["TextoBotaoSelecionados"] = "{0} Itens selecionados";
                return (string) ViewState["TextoBotaoSelecionados"];
            }
        }

        /// <summary>
        /// Título da modal de itens a selecionar
        /// </summary>
        public string ModalBuscaASelecionarTitulo 
        {
            get { EnsureChildControls(); return ModalBuscaASelecionar.Titulo; }
            set { EnsureChildControls(); ModalBuscaASelecionar.Titulo = value;}
        }

        /// <summary>
        /// Título da modal se itens selecionados
        /// </summary>
        public string ModalBuscaSelecionadosTitulo
        {
            get { EnsureChildControls(); return ModalBuscaSelecionados.Titulo; }
            set { EnsureChildControls(); ModalBuscaSelecionados.Titulo = value; }
        }

        /// <summary>
        /// Texto da modal itens a selecionar
        /// </summary>
        public string ModalBuscaASelecionarLabelTexto
        {
            get { EnsureChildControls(); return ModalBuscaASelecionar.LabelTexto; }
            set { EnsureChildControls(); ModalBuscaASelecionar.LabelTexto = value; }
        }

        /// <summary>
        /// Texto da modal itens selecionados
        /// </summary>
        public string ModalBuscaSelecionadosLabelTexto
        {
            get { EnsureChildControls(); return ModalBuscaSelecionados.LabelTexto; }
            set { EnsureChildControls(); ModalBuscaSelecionados.LabelTexto = value; }
        }

        /// <summary>
        /// Indica se deve realizar valiação no botão adicionar da modal
        /// </summary>
        public bool CausesValidation
        {
            get { EnsureChildControls(); return _btnAbrirModalAdd.CausesValidation; }
            set { EnsureChildControls(); _btnAbrirModalAdd.CausesValidation = value; }
        }

        /// <summary>
        /// Grupo de validação do botão adiconar da modal
        /// </summary>
        public string ValidationGroup
        {
            get { EnsureChildControls(); return _btnAbrirModalAdd.ValidationGroup; }
            set { EnsureChildControls(); _btnAbrirModalAdd.ValidationGroup = value; }
        }

        /// <summary>
        /// Informa se utiliza a modal de itens a adicionar
        /// </summary>
        public bool UsarModarAdicionar
        {
            get { EnsureChildControls(); return ViewState["UsarModarAdicionar"] == null ? true : (bool)ViewState["UsarModarAdicionar"]; }
            set { EnsureChildControls(); ViewState["UsarModarAdicionar"] = value; }
        }

        /// <summary>
        /// Classe css do botão adicionar da modal
        /// </summary>
        public string BotaoAdiconarCss
        {
            get { EnsureChildControls(); return _btnAbrirModalAdd.CssClass; }
            set { EnsureChildControls(); _btnAbrirModalAdd.CssClass = value; }
        }

        /// <summary>
        /// Imagem do botão adicionar da modal
        /// </summary>
        public string BotaoAdiconarImagem
        {
            get { EnsureChildControls(); return _btnAbrirModalAdd.ImageUrl; }
            set { EnsureChildControls(); _btnAbrirModalAdd.ImageUrl = value; }
        }

        /// <summary>
        /// Classe css do botão adicionar da modal
        /// </summary>
        public string BotaoAdicionadosCss
        {
            get { EnsureChildControls(); return _btnAbrirModalAdicionados.CssClass; }
            set { EnsureChildControls(); _btnAbrirModalAdicionados.CssClass = value; }
        }
        
        #endregion

        #region Contrutores
        /// <inheritdoc/>
        public EmployerMultiplaSelecao()
        {
            _ModalASelecionar.RetornaDadosItens += new ModalMultiplaSelecao.HandlerRetornaDadosItens(_ModalASelecionar_RetornaDadosItens);
            _ModalASelecionar.AdiconarColunas += new ModalMultiplaSelecao.HandlerAdiconarColunas(_ModalASelecionar_AdiconarColunas);
            _ModalASelecionar.SelecionarItens += new EventHandler(_ModalASelecionar_SelecionarItens);

            _ModalASelecionar.GetKey += new ModalMultiplaSelecao.HandlerGetKey(_ModalASelecionar_GetKey);
            _ModalSelecionados.GetKey += new ModalMultiplaSelecao.HandlerGetKey(_ModalASelecionar_GetKey);

            _ModalSelecionados.AdiconarColunas += new ModalMultiplaSelecao.HandlerAdiconarColunas(_ModalSelecionados_AdiconarColunas);
            _ModalSelecionados.RetornaDadosItens += new ModalMultiplaSelecao.HandlerRetornaDadosItens(_ModalSelecionados_RetornaDadosItens);
            _ModalSelecionados.SelecionarItens += new EventHandler(_ModalSelecionados_SelecionarItens);
        }
        #endregion

        #region Métodos

        #region Sobrescritos
        /// <inheritdoc/>
        protected override void OnPreRender(EventArgs e)
        {
            _btnAbrirModalAdicionados.Text = string.Format(TextoBotaoSelecionados, this.ListaItensSelecionados.Count);

            base.OnPreRender(e);
        }

        /// <inheritdoc/>
        protected override void OnInit(EventArgs e)
        {
            _btnAbrirModalAdd.Click += new ImageClickEventHandler(_btnAbrirModalAdd_Click);
            _btnAbrirModalAdicionados.Click += new EventHandler(_btnAbrirModalAdicionados_Click);

            base.OnInit(e);
        }

        /// <inheritdoc/>
        protected override void CreateChildControls()
        {
            this.Controls.Add(_btnAbrirModalAdd);
            this.Controls.Add(_btnAbrirModalAdicionados);

            this.Controls.Add(_ModalASelecionar);
            _ModalASelecionar.TextoBotaoAdicionarSelecionados = "Adicionar Itens";

            this.Controls.Add(_ModalSelecionados);
            _ModalSelecionados.TextoBotaoAdicionarSelecionados = "Remover Itens";

            base.CreateChildControls();
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (string.IsNullOrEmpty(_btnAbrirModalAdd.ImageUrl))
            {
                _btnAbrirModalAdd.ImageUrl = this.Page.ClientScript.GetWebResourceUrl(
                        typeof(EmployerPIS), "Employer.Componentes.UI.Web.Content.Images.ico_adicionar.gif");
            }
        }

        #endregion

        #region Eventos

        void _btnAbrirModalAdicionados_Click(object sender, EventArgs e)
        {
            if (this.ListaItensSelecionados.Any())
            {
                ModalBuscaSelecionados.ListaItensSelecionados.Clear();
                ModalBuscaSelecionados.GridBusca.DataSource = null;
                ModalBuscaSelecionados.GridBusca.DataBind();

                ModalBuscaASelecionar.Close();
                ModalBuscaSelecionados.Show();
            }
        }

        void _btnAbrirModalAdd_Click(object sender, ImageClickEventArgs e)
        {
            if (AdicionarClick != null)
                AdicionarClick(sender, e);

            if (UsarModarAdicionar)
            {
                ModalBuscaASelecionar.ListaItensSelecionados.Clear();
                ModalBuscaASelecionar.GridBusca.DataSource = null;
                ModalBuscaASelecionar.GridBusca.DataBind();

                ModalBuscaSelecionados.Close();
                ModalBuscaASelecionar.Show();
            }
        }

        #region Modal
        void _ModalSelecionados_SelecionarItens(object sender, EventArgs e)
        {
            foreach (var id in _ModalSelecionados.ListaItensSelecionados)
            {
                if (this.ListaItensSelecionados.Contains(id))
                    this.ListaItensSelecionados.Remove(id);
            }
        }

        void _ModalASelecionar_SelecionarItens(object sender, EventArgs e)
        {
            foreach (var id in _ModalASelecionar.ListaItensSelecionados)
            {
                if (!this.ListaItensSelecionados.Contains(id))
                    this.ListaItensSelecionados.Add(id);
            }
        }

        object _ModalSelecionados_RetornaDadosItens(bool comPaginacao)
        {
            return RetornaDataSourceItensSelecionados(comPaginacao);                
        }

        void _ModalSelecionados_AdiconarColunas(EmployerGrid grid)
        {
            AdiconarColunasBuscaSelecionados(grid);
        }

        int _ModalASelecionar_GetKey(object DataItem)
        {
            return GetKey(DataItem);
        }

        void _ModalASelecionar_AdiconarColunas(EmployerGrid grid)
        {
            AdiconarColunasBuscaASelecionar(grid);
        }

        object _ModalASelecionar_RetornaDadosItens(bool comPaginacao)
        {
            return RetornaDataSourceItensASelecionar(comPaginacao);
        }
        #endregion

        #endregion

        #region Métodos abstratos
        /// <summary>
        /// Método que retorna os dados de itens selecionados
        /// </summary>
        /// <param name="comPaginacao">Le os dados somente da página ou de todas as páginas</param>
        /// <returns>Itens selecionados</returns>
        public abstract object RetornaDataSourceItensSelecionados(bool comPaginacao);

        /// <summary>
        /// Método usado para criar as colunas da grid de itens selecionados
        /// </summary>
        /// <param name="grid"></param>
        public abstract void AdiconarColunasBuscaSelecionados(EmployerGrid grid);

        /// <summary>
        /// Método que retorna os dados de dos itens a selecionar
        /// </summary>
        /// <param name="comPaginacao">Le os dados somente da página ou de todas as páginas</param>
        /// <returns>Itens selecionados</returns>
        public abstract object RetornaDataSourceItensASelecionar(bool comPaginacao);

        /// <summary>
        /// Método usado para criar as colunas da grid de itens a selecionar
        /// </summary>
        /// <param name="grid"></param>
        public abstract void AdiconarColunasBuscaASelecionar(EmployerGrid grid);

        #region GetKey
        /// <summary>
        /// Retorna a campo chave a partir de um objeto da fonte de dados
        /// </summary>
        /// <param name="DataItem">O objeto da fonte de dados</param>
        /// <returns>Um objeto representado o campo chave na fonte de dados</returns>
        public abstract int GetKey(object DataItem);
        #endregion

        #endregion

        #endregion

        #region Subclasses

        #region ColunaCheckBox
        /// <inheritdoc/>
        public class ColunaCheckBox : ITemplate
        {
            /// <inheritdoc/>
            public void InstantiateIn(Control container)
            {
                CheckBox chkSelecionar = new CheckBox { ID = "chkSelecionar" };
                container.Controls.Add(chkSelecionar);
            }
        }
        #endregion

        #endregion
    }
}
