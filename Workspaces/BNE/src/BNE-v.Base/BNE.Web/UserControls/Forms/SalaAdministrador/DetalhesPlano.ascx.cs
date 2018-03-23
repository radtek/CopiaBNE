using System;
using System.Collections.Generic;
using System.Globalization;
using BNE.BLL.Custom;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.BLL;
using Resources;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Data.SqlClient;
using System.Web.UI;

namespace BNE.Web.UserControls.Forms.SalaAdministrador
{
    public partial class DetalhesPlano : BaseUserControl
    {

        #region Propriedades

        #region IdPlanoAdquirido - Variavel 1
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        public int IdPlanoAdquirido
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel1.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region IdPagamento - Variavel 2
        /// <summary>
        /// Propriedade que armazena e recupera o ID
        /// </summary>
        protected int? IdPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel2.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());

                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel2.ToString());

            }
        }
        #endregion

        #region Permissoes - Variável Permissoes
        /// <summary>
        /// Propriedade que armazena e recupera as Permissoes
        /// </summary>
        protected List<int> Permissoes
        {
            get
            {
                return (List<int>)ViewState[Chave.Temporaria.Permissoes.ToString()];
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Permissoes.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region btnSalvar_Click
        protected void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                Salvar();
                CarregarGridParcelasPlano();
                CarregarGridParcelasAdicional();
                LimparCampos();
                IdPagamento = null;
                ExibirMensagem(MensagemAviso._100001, TipoMensagem.Aviso);
            }
            catch (Exception ex)
            {
                base.ExibirMensagemErro(ex);
            }
        }
        #endregion

        #region gvParcelas

        #region gvParcelas_ItemCommand
        protected void gvParcelas_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                IdPagamento = Convert.ToInt32(gvParcelas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]);
                string parcela = gvParcelas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Parcela"].ToString();
                PreencherCampos(parcela);
            }
            else if (e.CommandName.Equals("Visualizar"))
            {
                try
                {
                    IdPagamento = Convert.ToInt32(gvParcelas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]);
                    BLL.Pagamento objPagamento = BLL.Pagamento.LoadObject(IdPagamento.Value);

                    if (string.IsNullOrEmpty(gvParcelas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Des_Identificador"].ToString()))
                        objPagamento.CodigoGuid = null;

                    hlBoleto.NavigateUrl = CobrancaBoleto.RetornarBoletoImagem(objPagamento);
                    hlPDF.NavigateUrl = CobrancaBoleto.RetornarBoletoPDF(objPagamento);

                    CarregarGridParcelasPlano();
                    CarregarGridParcelasAdicional();
                    LimparCampos();

                    upDados.Update();
                    mpeVisualizacaoBoleto.Show();
                }
                catch (Exception ex)
                {
                    base.ExibirMensagemErro(ex);
                }
            }
            else if (e.CommandName.Equals("EmitirNF"))
            {
                IdPagamento = Convert.ToInt32(gvParcelas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]);

                var objPagamento = BLL.Pagamento.LoadObject(IdPagamento.Value);

                if (base.IdPerfil.Value != (int)Enumeradores.Perfil.Financeiro)
                    ExibirMensagem("Perfil Usuário sem permissão para emitir Nota Fiscal", TipoMensagem.Aviso);
                else
                    if (objPagamento.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)
                        if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null))
                            ExibirMensagem("Nota Fiscal emitida com sucesso !", TipoMensagem.Aviso);
                        else
                            ExibirMensagem("Erro ao emitir Nota fiscal", TipoMensagem.Aviso);
                    else
                        if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null))
                            ExibirMensagem("Nota Fiscal emitida com sucesso !", TipoMensagem.Aviso);
                        else
                            ExibirMensagem("Erro ao emitir Nota fiscal", TipoMensagem.Aviso);
            }
        }
        #endregion

        #region gvParcelas_PageIndexChanged
        protected void gvParcelas_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            gvParcelas.CurrentPageIndex = e.NewPageIndex;
            CarregarGridParcelasPlano();
        }
        #endregion

        #endregion

        #region gvParcelasAdicional

        #region gvParcelasAdicional_ItemCommand
        protected void gvParcelasAdicional_ItemCommand(object source, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                IdPagamento = Convert.ToInt32(gvParcelasAdicional.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]);
                //string parcela = gvParcelas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Parcela"].ToString();
                PreencherCampos(String.Empty);
            }
            else if (e.CommandName.Equals("Visualizar"))
            {
                try
                {
                    IdPagamento = Convert.ToInt32(gvParcelasAdicional.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]);
                    BLL.Pagamento objPagamento = BLL.Pagamento.LoadObject(IdPagamento.Value);

                    hlBoleto.NavigateUrl = CobrancaBoleto.RetornarBoletoImagem(objPagamento);
                    hlPDF.NavigateUrl = CobrancaBoleto.RetornarBoletoPDF(objPagamento);

                    upDados.Update();
                    mpeVisualizacaoBoleto.Show();
                }
                catch (Exception ex)
                {
                    base.ExibirMensagemErro(ex);
                }
            }
            else if (e.CommandName.Equals("EmitirNF"))
            {
                IdPagamento = Convert.ToInt32(gvParcelasAdicional.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]);

                var objPagamento = BLL.Pagamento.LoadObject(IdPagamento.Value);

                if (base.IdPerfil.Value != (int)Enumeradores.Perfil.Financeiro)
                    ExibirMensagem("Perfil Usuário sem permissão para emitir Nota Fiscal", TipoMensagem.Aviso);
                else
                    if (objPagamento.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)
                        if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null))
                            ExibirMensagem("Nota Fiscal emitida com sucesso !", TipoMensagem.Aviso);
                        else
                            ExibirMensagem("Erro ao emitir Nota fiscal", TipoMensagem.Aviso);
                    else
                        if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, null))
                            ExibirMensagem("Nota Fiscal emitida com sucesso !", TipoMensagem.Aviso);
                        else
                            ExibirMensagem("Erro ao emitir Nota fiscal", TipoMensagem.Aviso);

                objPagamento.Save();
            }
        }
        #endregion

        #region gvParcelasAdicional_PageIndexChanged
        protected void gvParcelasAdicional_PageIndexChanged(object source, Telerik.Web.UI.GridPageChangedEventArgs e)
        {
            gvParcelasAdicional.CurrentPageIndex = e.NewPageIndex;
            CarregarGridParcelasAdicional();
        }
        #endregion

        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (Voltar != null)
                Voltar();
        }
        #endregion

        #region chkLiberarSMS_CheckedChanged

        protected void chkLiberarSMS_CheckedChanged(object sender, EventArgs e)
        {
            PlanoParcela.LiberarOuBloquearSaldoTotalSMSDoPlano(IdPlanoAdquirido, chkLiberarSMS.Checked);
            txtQtdeSMSLiberada.Valor = chkLiberarSMS.Checked ? txtQtdeSMSTotal.Valor : String.Empty;
            txtQtdeSMSLiberada.Enabled = !chkLiberarSMS.Checked;
            CarregarGridParcelasPlano();
        }

        #endregion

        #endregion

        #region Métodos

        #region CarregarGridParcelasPlano
        private void CarregarGridParcelasPlano()
        {
            int totalRegistros;
            UIHelper.CarregarRadGrid(gvParcelas, PlanoParcela.ListaParcelasPorPlanoAdquirido(IdPlanoAdquirido, gvParcelas.CurrentPageIndex, gvParcelas.PageSize, out totalRegistros), totalRegistros);
            upGvParcelas.Update();
        }
        #endregion

        #region CarregarGridParcelasAdicional
        private void CarregarGridParcelasAdicional()
        {
            int totalRegistros;
            PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquirido);
            UIHelper.CarregarRadGrid(gvParcelasAdicional, AdicionalPlano.ListarAdicionaisPorPlanoAdquirido(objPlanoAdquirido, gvParcelasAdicional.CurrentPageIndex, gvParcelasAdicional.PageSize, out totalRegistros), totalRegistros);
            upGvParcelasAdicional.Update();
        }
        #endregion

        #region InicializarModal
        public void InicializarModal()
        {
            CarregarEAjustarPermissoes();

            gvParcelas.PageSize = 6;
            gvParcelasAdicional.PageSize = 6;
            gvParcelas.CurrentPageIndex = 0;
            gvParcelasAdicional.CurrentPageIndex = 0;
            CarregarGridParcelasPlano();
            CarregarGridParcelasAdicional();
            LimparCampos();
            //txtDataEnvio.DataMinima = DateTime.Today;
            //txtDataVencimento.DataMinima = DateTime.Today;
            UIHelper.CarregarRadComboBox(rcbSituacao, PagamentoSituacao.Listar(), "Idf_Pagamento_Situacao", "Des_Pagamento_Situacao");
        }
        #endregion

        #region Salvar
        private void Salvar()
        {
            try
            {
                bool gerarNovoPagamento = false; //Utilizado para controlar se deve abrir um novo pagamento

                BLL.Pagamento objPagamento = BLL.Pagamento.LoadObject(IdPagamento.Value);

                if (Convert.ToInt32(rcbSituacao.SelectedValue) == (int)Enumeradores.PagamentoSituacao.EmAberto && (!objPagamento.DataVencimento.Equals(txtDataVencimento.ValorDatetime) || !objPagamento.ValorPagamento.Equals(txtValor.Valor.Value)))
                    gerarNovoPagamento = true;

                objPagamento.PagamentoSituacao = new PagamentoSituacao(Convert.ToInt32(rcbSituacao.SelectedValue));
                objPagamento.DataEmissao = txtDataEnvio.ValorDatetime;
                objPagamento.DataVencimento = txtDataVencimento.ValorDatetime;
                objPagamento.ValorPagamento = txtValor.Valor.Value;
                objPagamento.NumeroNotaFiscal = txtNumeroNota.Valor;

                int qtdeSMSTotal;
                int qtdeSMSLiberada;
                Int32.TryParse(txtQtdeSMSLiberada.Valor, out qtdeSMSLiberada);
                Int32.TryParse(txtQtdeSMSTotal.Valor, out qtdeSMSTotal);

                switch (Convert.ToInt32(rcbSituacao.SelectedValue))
                {
                    case (int)Enumeradores.PagamentoSituacao.Cancelado:
                        BNE.BLL.Pagamento.CancelarPagamento(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);
                        break;
                    case (int)Enumeradores.PagamentoSituacao.Pago:
                        objPagamento.Liberar(DateTime.Now);
                        break;
                }

                if (gerarNovoPagamento)
                {
                    var objPagamentoNovo = new BLL.Pagamento
                    {
                        TipoPagamento = objPagamento.TipoPagamento,
                        PlanoParcela = objPagamento.PlanoParcela,
                        DataEmissao = objPagamento.DataEmissao,
                        DataVencimento = objPagamento.DataVencimento,
                        UsuarioFilialPerfil = objPagamento.UsuarioFilialPerfil,
                        ValorPagamento = objPagamento.ValorPagamento,
                        PagamentoSituacao = objPagamento.PagamentoSituacao,
                        Filial = objPagamento.Filial,
                        AdicionalPlano = objPagamento.AdicionalPlano,
                        NumeroNotaFiscal = objPagamento.NumeroNotaFiscal,
                        UrlNotaFiscal = objPagamento.UrlNotaFiscal
                    };

                    objPagamentoNovo.Salvar(qtdeSMSTotal, qtdeSMSLiberada);

                    try
                    {
                        hlBoleto.NavigateUrl = CobrancaBoleto.RetornarBoletoImagem(objPagamentoNovo);
                        hlPDF.NavigateUrl = CobrancaBoleto.RetornarBoletoPDF(objPagamentoNovo);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    upDados.Update();
                    mpeVisualizacaoBoleto.Show();
                }
                else
                    objPagamento.Save();
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region PreencherCampos
        private void PreencherCampos(string parcela)
        {
            BLL.Pagamento objPagamento = BLL.Pagamento.LoadObject(IdPagamento.Value);

            litNumeroParcela.Text = parcela;
            rcbSituacao.SelectedValue = objPagamento.PagamentoSituacao.IdPagamentoSituacao.ToString(CultureInfo.CurrentCulture);
            txtDataEnvio.ValorDatetime = objPagamento.DataEmissao;
            txtDataVencimento.ValorDatetime = objPagamento.DataVencimento;
            txtValor.Valor = objPagamento.ValorPagamento;
            txtNumeroNota.Valor = objPagamento.NumeroNotaFiscal;
            pnlQtdeSMS.Visible = false;

            if (null != objPagamento.PlanoParcela)
            {
                pnlQtdeSMS.Visible = true;

                objPagamento.PlanoParcela.CompleteObject();
                txtQtdeSMSTotal.Valor = objPagamento.PlanoParcela.QuantidadeSMSTotal.ToString();
                txtQtdeSMSLiberada.Valor =
                    objPagamento.PlanoParcela.QuantidadeSMSLiberada.HasValue ?
                        objPagamento.PlanoParcela.QuantidadeSMSLiberada.Value.ToString() : String.Empty;
                chkLiberarSMS.Checked = PlanoParcela.EstaLiberadoSMS(IdPlanoAdquirido);
                txtQtdeSMSLiberada.Enabled = !chkLiberarSMS.Checked;
            }

            upPnlEditarParcela.Update();
        }
        #endregion

        #region LimparCampos
        private void LimparCampos()
        {
            litNumeroParcela.Text = String.Empty;
            txtDataEnvio.ValorDatetime = null;
            txtDataVencimento.ValorDatetime = null;
            txtValor.Valor = null;
            txtNumeroNota.Valor = null;
            rcbSituacao.SelectedValue = "1";
            txtQtdeSMSTotal.Valor = null;
            txtQtdeSMSLiberada.Valor = null;
            pnlQtdeSMS.Visible = false;

            upPnlEditarParcela.Update();
        }
        #endregion

        #region AjustarVisibilidadeEditarParcela
        /// <summary>
        /// Metodo resposavel por ajustar a visibilidade do botão editar parcela
        /// </summary>
        /// <param name="idPagamentoSituacao"></param>
        /// <param name="cortesia"></param>
        /// <returns>visible</returns>
        public bool AjustarVisibilidadeEditarParcela(int idPagamentoSituacao, bool cortesia)
        {
            if (cortesia)
                return false;

            if (idPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto)
                return true;

            return false;
        }
        #endregion

        #region AjustarVisibilidadeVisualizarBoleto
        /// <summary>
        /// Metodo resposavel por ajustar a visibilidade do botão visualzar boleto
        /// </summary>
        /// <param name="idTipoPagamento"></param>
        /// <param name="cortesia"></param>
        /// <returns></returns>
        public bool AjustarVisibilidadeVisualizarBoleto(int idTipoPagamento, bool cortesia)
        {
            if (cortesia)
                return false;

            if (idTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)
                return true;

            return false;
        }
        #endregion

        #region AjustarTamanhoEmail
        protected string AjustarTamanhoEmail(string descricaoEmail)
        {
            return descricaoEmail.Truncate(20);
        }
        #endregion

        #region CarregarEAjustarPermissoes
        /// <summary>
        /// Método responsável por ajustar as permissões da tela de acordo com o susuário filial perfil logado.
        /// </summary>
        private void CarregarEAjustarPermissoes()
        {
            if (base.IdUsuarioFilialPerfilLogadoUsuarioInterno.HasValue)
            {
                Permissoes = UsuarioFilialPerfil.CarregarPermissoes(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value, Enumeradores.CategoriaPermissao.Administrador);

                if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroSalvarPlano))
                    btnSalvar.Enabled = false;
            }
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void delegateVoltar();
        public event delegateVoltar Voltar;
        #endregion

    }
}
