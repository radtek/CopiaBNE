using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Globalization;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class TipoPlano : BaseUserControl
    {

        #region Propriedades

        #region PageIndex - PageIndex
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int PageIndex
        {
            get
            {
                if (ViewState[Chave.Temporaria.PageIndex.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.PageIndex.ToString()].ToString());
                else
                    return 0;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.PageIndex.ToString(), value);
            }
        }
        #endregion

        #region IdPlano - Variavel1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdPlano
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
            }
        }
        #endregion

        #region StatusPlanos - Variavel
        private int? StatusPlanosFinanceiro
        {
            get
            {
                if (ViewState["StatusPlanosFinanceiro"] != null)
                    return Int32.Parse(ViewState["StatusPlanosFinanceiro"].ToString());
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add("StatusPlanosFinanceiro", value);
            }
        }
        #endregion

        #region TextoFiltro - Variavel
        private String TextoFiltro
        {
            get
            {
                if (ViewState["TextoFiltro"] != null)
                    return ViewState["TextoFiltro"].ToString();
                else
                    return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add("TextoFiltro", value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            ucConfirmacaoExclusao.Confirmar += ucConfirmacaoExclusao_Confirmar;            
        }
        #endregion

        #region ucConfirmacaoExclusao_Confirmar
        void ucConfirmacaoExclusao_Confirmar()
        {
            Excluir();
            LimparCampos();
            AjustarCampos();
            upCadastroPlano.Update();
            IdPlano = null;
            CarregarGrid();
            upGvPlanos.Update();
        }
        #endregion

        #region gvPlanos_PageIndexChanged
        protected void gvPlanos_PageIndexChanged(object source, GridPageChangedEventArgs e)
        {            
            PageIndex = e.NewPageIndex + 1;
            CarregarGrid();
        }
        #endregion

        #region gvPlanos_ItemCommand
        protected void gvPlanos_ItemCommand(object source, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                IdPlano = Convert.ToInt32(gvPlanos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Plano"]);
                PreencherCampos();
                AjustarCampos();
                upCadastroPlano.Update();
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                Label lblNomePlano = (Label)e.Item.FindControl("lblNomePlano");
                IdPlano = Convert.ToInt32(gvPlanos.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Plano"]);
                ucConfirmacaoExclusao.Inicializar("Confirmação de Exclusão", String.Format("Tem certeza que deseja excluir o plano <b>{0}</b>?", lblNomePlano.Text));
                ucConfirmacaoExclusao.MostrarModal();
            }
        }
        #endregion

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (rcbTipoContrato.SelectedValue.Equals("-1"))
                {
                    base.ExibirMensagem("Selecione um tipo de contrato!", Code.Enumeradores.TipoMensagem.Aviso);
                    return;
                }
                if (Convert.ToInt32(txtNumeroParcelas.Valor) > 0)
                {
                    Salvar();
                    LimparCampos();
                    AjustarCampos();
                    IdPlano = null;
                    CarregarGrid();
                }
                else
                    base.ExibirMensagem("O numero de parcelas deve ser no mínimo 1", Code.Enumeradores.TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region btnCancelar_Click
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            LimparCampos();
            AjustarCampos();
            IdPlano = null;
            txtNomePlano.Focus();
        }
        #endregion

        #region
        protected void rcbFiltrarTipoCliente_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            AjustarCampos();
        }
        #endregion

        #endregion

        #region Métodos

        #region CarregarGrid
        private void CarregarGrid()
        {
            int totalRegistros = 0;
            int? status;

            if (StatusPlanosFinanceiro != 2) //filtra por ativo ou inativo
                status = StatusPlanosFinanceiro;
            else
                status = null; //carrega Todos os Planos            

            rdLstStatusPlanos.SelectedValue = StatusPlanosFinanceiro.ToString(); //deixar o radioButton selecionado de acordo com os registros da grid
            upRdStatusPlanos.Update();

            UIHelper.CarregarRadGrid(gvPlanos, Plano.CarregarPelaDescricao(TextoFiltro, PageIndex, gvPlanos.PageSize, StatusPlanosFinanceiro, out totalRegistros), totalRegistros);
        }
        #endregion

        #region AjustarCampos
        private void AjustarCampos()
        {
            ckbPagamentoRecorrenteMensal.Enabled =
            lblQuantidadeSMS.Enabled = lblQuantidadeVisualizacoes.Enabled = lblQuantidadeCampanha.Enabled =
            txtQuantidadeSMS.Enabled = txtQuantidadeVisualizacoes.Enabled = txtQuantidadeCampanha.Enabled =
            txtQuantidadeSMS.Obrigatorio = txtQuantidadeVisualizacoes.Obrigatorio = txtQuantidadeCampanha.Obrigatorio = rcbFiltrarTipoCliente.SelectedValue.Equals(((int)Enumeradores.PlanoTipo.PessoaJuridica).ToString());
        }
        #endregion
        
        #region Excluir
        private void Excluir()
        {
            try
            {
                Plano objPlanoTipo = Plano.LoadObject(IdPlano.Value);
                objPlanoTipo.FlagInativo = true;
                objPlanoTipo.Save();
                base.ExibirMensagemConfirmacao("Confirmação de Cancelamento", "Plano cancelado com sucesso!", false);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos()
        {
            if (IdPlano.HasValue)
            {
                Plano objPlano = Plano.LoadObject(IdPlano.Value);

                rcbFiltrarTipoCliente.SelectedValue = objPlano.PlanoTipo.IdPlanoTipo.ToString(CultureInfo.CurrentUICulture);
                txtNomePlano.Valor = objPlano.DescricaoPlano;
                txtTempoAcesso.Valor = objPlano.QuantidadeDiasValidade.ToString(CultureInfo.CurrentUICulture);
                txtQuantidadeVisualizacoes.Valor = objPlano.QuantidadeVisualizacao.ToString(CultureInfo.CurrentUICulture);
                txtQuantidadeSMS.Valor = objPlano.QuantidadeSMS.ToString(CultureInfo.CurrentUICulture);
                txtQuantidadeCampanha.Valor = objPlano.QuantidadeCampanha.ToString(CultureInfo.CurrentUICulture);
                txtValor.Valor = objPlano.ValorBase;
                txtNumeroParcelas.Valor = objPlano.QuantidadeParcela.ToString(CultureInfo.CurrentUICulture);
                txtDesconto.Valor = objPlano.ValorDescontoMaximo.ToString(CultureInfo.CurrentUICulture);
                chkEnviaContrato.Checked = objPlano.FlagEnviarContrato;
                chkHabilitaVendaPersonalizada.Checked = objPlano.FlagHabilitaVendaPersonalizada;
                ckbLiberaUsuarioTanque.Checked = objPlano.FlagLiberaUsuariosTanque;
                ckbPagamentoRecorrenteMensal.Checked = objPlano.FlagRecorrente;

                if (objPlano.TipoContrato != null)
                    rcbTipoContrato.SelectedValue = objPlano.TipoContrato.IdTipoContrato.ToString(CultureInfo.CurrentUICulture);
            }
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            Plano objPlano;

            if (IdPlano.HasValue)
                objPlano = Plano.LoadObject(IdPlano.Value);
            else
            {
                objPlano = new Plano { DataInicio = DateTime.Now };
            }

            objPlano.DescricaoPlano = txtNomePlano.Valor;
            objPlano.QuantidadeDiasValidade = Convert.ToInt32(txtTempoAcesso.Valor);

            if (String.IsNullOrEmpty(txtQuantidadeVisualizacoes.Valor))
                objPlano.QuantidadeVisualizacao = 0;
            else
                objPlano.QuantidadeVisualizacao = Convert.ToInt32(txtQuantidadeVisualizacoes.Valor);

            if (String.IsNullOrEmpty(txtQuantidadeSMS.Valor))
                objPlano.QuantidadeSMS = 0;
            else
                objPlano.QuantidadeSMS = Convert.ToInt32(txtQuantidadeSMS.Valor);

            if (String.IsNullOrEmpty(txtQuantidadeCampanha.Valor))
                objPlano.QuantidadeCampanha = 0;
            else
                objPlano.QuantidadeCampanha = Convert.ToInt16(txtQuantidadeCampanha.Valor);

            objPlano.ValorBase = txtValor.Valor.Value;
            objPlano.QuantidadeParcela = Convert.ToInt32(txtNumeroParcelas.Valor);            
            objPlano.PlanoTipo = new PlanoTipo(Convert.ToInt32(rcbFiltrarTipoCliente.SelectedValue));
            objPlano.ValorDescontoMaximo = Convert.ToInt32(txtDesconto.Valor);
            objPlano.TipoContrato = new TipoContrato(Convert.ToInt32(rcbTipoContrato.SelectedValue));
            objPlano.FlagEnviarContrato = chkEnviaContrato.Checked;
            objPlano.FlagHabilitaVendaPersonalizada = chkHabilitaVendaPersonalizada.Checked;
            objPlano.FlagLiberaUsuariosTanque = ckbLiberaUsuarioTanque.Checked;
            objPlano.FlagRecorrente = ckbPagamentoRecorrenteMensal.Checked;

            if (ckbPagamentoRecorrenteMensal.Checked == false)
                objPlano.PlanoFormaPagamento = new PlanoFormaPagamento((int)Enumeradores.PlanoFormaPagamento.Total);
            else
                objPlano.PlanoFormaPagamento = new PlanoFormaPagamento((int)Enumeradores.PlanoFormaPagamento.Mensalidade);

            objPlano.Save();

            base.ExibirMensagemConfirmacao("Confirmação de Cadastro", IdPlano.HasValue ? "Plano atualizado com sucesso!" : "Plano cadastrado com sucesso!", false);
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            rcbFiltrarTipoCliente.SelectedValue = "1";
            txtNomePlano.Valor = String.Empty;
            txtTempoAcesso.Valor = String.Empty;
            txtQuantidadeVisualizacoes.Valor = String.Empty;
            txtQuantidadeSMS.Valor = String.Empty;
            txtQuantidadeCampanha.Valor = string.Empty;
            txtValor.Valor = null;
            txtNumeroParcelas.Valor = String.Empty;
            txtDesconto.Valor = String.Empty;
            rcbTipoContrato.SelectedValue = "-1";
            chkEnviaContrato.Checked = false;
            chkHabilitaVendaPersonalizada.Checked = false;
            ckbLiberaUsuarioTanque.Checked = false;
            ckbPagamentoRecorrenteMensal.Checked = false;
        }
        #endregion

        #region Inicializar
        public void Inicializar()
        {
            StatusPlanosFinanceiro = Convert.ToInt32(rdLstStatusPlanos.SelectedValue); 

            PageIndex = 1;
            gvPlanos.PageSize = 6;
            CarregarGrid();
            UIHelper.CarregarRadComboBox(rcbFiltrarTipoCliente, PlanoTipo.Listar(), "Idf_Plano_Tipo", "Des_Plano_Tipo");
            UIHelper.CarregarRadComboBox(rcbTipoContrato, TipoContrato.ListarTipoContrato(), new RadComboBoxItem("Selecione", "-1"));
            AjustarCampos();
        }
        #endregion

        #region btnFiltrarStatus_Click
        protected void btnFiltrarStatus_Click(object sender, EventArgs e)
        {
            StatusPlanosFinanceiro = Convert.ToInt32(rdLstStatusPlanos.SelectedValue);
            TextoFiltro = filtroDePlanos.Valor;
            int totalRegistros = 0;

            PageIndex = 1;
            gvPlanos.PageSize = 6;
            gvPlanos.CurrentPageIndex = 0; //zera a paginacao da grid

            CarregarGrid();
            
            upGvPlanos.Update();

            
            
        }
        #endregion

        #endregion

    }
}