using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using BNE.BLL;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code;
using BNE.BLL.Security;
using BNE.BLL.Custom;
using BNE.CEP;

namespace BNE.Web.UserControls
{
    public partial class BuscarCEP : BaseUserControl
    {

        #region Propriedades

        #region Habilitar
        /// <summary>
        /// Propriedade utilizada para habilitar ou desabilitar o botão de busca de cep.
        /// </summary>
        public bool Habilitar
        {
            get
            {
                return this.btnBuscarCEP.Enabled;
            }
            set
            {
                this.btnBuscarCEP.Enabled = value;
            }
        }

        public bool UfObrigatorio { get; set; }
        #endregion

        #endregion

        #region Métodos

        #region LimparCampos
        /// <summary>
        /// Metodo responsavel por limpar os campos ao retornar o CEP
        /// </summary>
        private void LimparCampos()
        {

            txtBairro.Valor = string.Empty;
            txtLogradouro.Valor = string.Empty;
            lsUF.Valor = string.Empty;
            txtMunicipio.Text = string.Empty;
            gvwCEP.DataSource = null;
            gvwCEP.DataBind();

            //aqui
            PaginacaoGrid.LimparPaginacao();

            lsUF.Focus();
        }
        #endregion

        #region CarregarGrid
        /// <summary>
        /// Metodo responsavel por Efetuar a busca de CEP
        /// </summary>
        private void CarregarGrid()
        {
            try
            {
                this.PaginacaoGrid.TamanhoPagina = 10;
                int totalRegistros;

                var lista = Cep.ConsultaCEPPaginacao(string.Empty,
                    Helper.TratarPesquisaLogradouro(txtLogradouro.Valor),
                    string.Empty,
                    txtBairro.Valor,
                    lsUF.Valor,
                    txtMunicipio.Text.Split('/')[0],
                    CEP.Colunas.Cidade,
                    CEP.DirecaoOrdenacao.ASC,
                    PaginacaoGrid.PaginaCorrente,
                    PaginacaoGrid.TamanhoPagina,
                    out totalRegistros);

                gvwCEP.DataSource = lista;
                gvwCEP.DataBind();

                this.PaginacaoGrid.TotalResultados = totalRegistros;
                this.PaginacaoGrid.AjustarPaginacao();

                PaginacaoGrid.Visible = true;

                MPE.Show();
            }
            catch(Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "");
            }
        }

        /// <summary>
        /// Metodo responsavel por Efetuar a busca de CEP
        /// </summary>
        private void CarregarGrid(string pNumCep)
        {
            try
            {
                this.PaginacaoGrid.TamanhoPagina = 10;
                int totalRegistros;

                var lista = Cep.ConsultaCEPPaginacao(pNumCep, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, CEP.Colunas.Cidade, CEP.DirecaoOrdenacao.ASC, PaginacaoGrid.PaginaCorrente, 8, out totalRegistros);
                gvwCEP.DataSource = lista;
                gvwCEP.DataBind();

                this.PaginacaoGrid.TotalResultados = totalRegistros;
                this.PaginacaoGrid.AjustarPaginacao();

                PaginacaoGrid.Visible = true;

                MPE.Show();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "");
            }
        }
        #endregion

        #region HabilitarModal
        /// <summary>
        /// Metodo responsavel por habilitar a modal.
        /// </summary>
        public void CarregarPorCep(string pNumCEP)
        {
            pnlModal.Enabled = true;
            LimparCampos();
            MPE.Show();

            CarregarGrid(pNumCEP);
        }
        #endregion

        #region FecharModal
        public void FecharModal()
        {
            MPE.Hide();
            if (Fechar != null)
                Fechar();
        }
        #endregion
        
        #endregion

        #region Eventos

        #region CepSelecionado
        public delegate void delegateCepSelecionado(string cep);
        public event delegateCepSelecionado CepSelecionado;
        public delegate void delegateLogradouroSelecionado(string cep, string logradouro);
        public event delegateLogradouroSelecionado LogradouroSelecionado;
        public delegate void delegateCancelar();
        public event delegateCancelar Cancelar;

        #endregion

        #region VoltarFoco
        public delegate void delegateVoltarFoco();
        public event delegateVoltarFoco VoltarFoco;
        #endregion

        #region Page
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //pnlModal.Enabled = false;
                UIHelper.CarregarListaSugestao(lsUF, Estado.Listar());
                lsUF.Obrigatorio = UfObrigatorio;
            }

            this.PaginacaoGrid.MudaPagina += PaginacaoGrid_MudaPagina;
        }
        #endregion

        #region PaginacaoGrid
        /// <summary>
        /// Evento disparado pelo Componente de Paginacao ao trocar de pagina
        /// </summary>
        void PaginacaoGrid_MudaPagina()
        {
            CarregarGrid();
        }
        #endregion

        #region gvwCEP
        /// <summary>
        /// Metodo executado ao selecionar um CEP no Grid
        /// </summary>
        protected void gvwCEP_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string cep = gvwCEP.DataKeys[e.NewEditIndex].Value.ToString();
            string logradouro = gvwCEP.Rows[e.NewEditIndex].Cells[3].Text;

            LimparCampos();
            //pnlModal.Enabled = false;
            MPE.Hide();

            //Modo antigo
            if (CepSelecionado != null)
                CepSelecionado(cep);

            //Utilizar este evento.
            if (LogradouroSelecionado != null)
                LogradouroSelecionado(cep, logradouro.Equals("&nbsp;") ? string.Empty : logradouro);
        }
        #endregion

        #region bntLocalizar
        /// <summary>
        /// Metodo executado ao Clicar no botao visualizar
        /// </summary>
        protected void bntLocalizar_Click(object sender, EventArgs e)
        {
            PaginacaoGrid.PaginaCorrente = 1;
            CarregarGrid();
            lsUF.Focus();
        }
        #endregion

        #region bntCancelar
        /// <summary>
        /// Metodo executado ao Clicar no botao Cancelar
        /// </summary>
        protected void bntCancelar_Click(object sender, EventArgs e)
        {
            //pnlModal.Enabled = false;

            if (VoltarFoco != null)
                VoltarFoco.Invoke();
            MPE.Hide();
            Cancelar();

        }
        #endregion

        #region btnBuscarCEP_Click
        protected void btnBuscarCEP_Click(object sender, EventArgs e)
        {
            pnlModal.Enabled = true;
            LimparCampos();
            MPE.Show();
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void fechar();
        public event fechar Fechar;
        #endregion

    }
}