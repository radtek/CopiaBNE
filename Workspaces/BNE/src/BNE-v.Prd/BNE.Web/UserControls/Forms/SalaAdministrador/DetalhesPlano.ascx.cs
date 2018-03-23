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
using BNE.BLL.Assincronos;
using BNE.BLL.Common;

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
                if (!Salvar())
                {
                    ExibirMensagem(MensagemAviso._505709, TipoMensagem.Aviso);
                    return;
                }
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

                    if (objPagamento.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.CartaoCredito)
                    {
                        objPagamento.TipoPagamento = new TipoPagamento((int)Enumeradores.TipoPagamento.BoletoBancario);
                        objPagamento.Save();
                    }
                    string img = string.Empty;

                    hlPDFNormal.NavigateUrl = BoletoBancario.RetornarBoleto(objPagamento, out img);
                    hlPDFCobranca.NavigateUrl = img;

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
                    if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value))
                        ExibirMensagem("Nota Fiscal emitida com sucesso !", TipoMensagem.Aviso);
                    else
                        ExibirMensagem("Erro ao emitir Nota fiscal", TipoMensagem.Aviso);
                else
                        if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value))
                    ExibirMensagem("Nota Fiscal emitida com sucesso !", TipoMensagem.Aviso);
                else
                    ExibirMensagem("Erro ao emitir Nota fiscal", TipoMensagem.Aviso);
            }
            else if (e.CommandName.Equals("Estornar"))
            {
                IdPagamento = Convert.ToInt32(gvParcelas.MasterTableView.DataKeyValues[e.Item.ItemIndex]["Idf_Pagamento"]);

                var objPagamento = BLL.Pagamento.LoadObject(IdPagamento.Value);

                BNE.BLL.Pagamento.CancelarPagamento(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);

                objPagamento.PagamentoSituacao = new PagamentoSituacao((int)Enumeradores.PagamentoSituacao.Cancelado);
                objPagamento.Save();

                string descricaoCRM = string.Concat("Pagamento ", objPagamento.IdPagamento, " cancelado!");

                var transacao = BLL.Transacao.CarregaTransacaoPorPagamento(objPagamento.IdPagamento);

                if (transacao.GerenciadoraTransacao == (int) Enumeradores.GerenciadoraTransacao.Cielo)
                {
                    descricaoCRM +=
                        Transacao.EfetuarCancelamentoCartaoDeCreditoCielo(objPagamento.DescricaoIdentificador)
                            ? descricaoCRM + " Estorno Realizado TID: " + objPagamento.DescricaoIdentificador
                            : descricaoCRM + " Falha ao realizar o estorno TID: " +
                              objPagamento.DescricaoIdentificador;
                }
                else
                {
                    descricaoCRM += PagarMeOperacoes.CancelarTransacaoPagarMe(objPagamento)
                        ? descricaoCRM + " Estorno Realizado TID: " + objPagamento.DescricaoIdentificador
                        : descricaoCRM + " Falha ao realizar o estorno TID: " +  objPagamento.DescricaoIdentificador;
                }

                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.CarregarPlanoAdquiridopDePagamento(objPagamento.IdPagamento);

                if (objPlanoAdquirido.ParaPessoaFisica())
                {
                    Curriculo objCurriculo;
                    objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();
                    Curriculo.CarregarPorPessoaFisica(objPlanoAdquirido.UsuarioFilialPerfil.PessoaFisica.IdPessoaFisica, out objCurriculo);
                    CurriculoObservacao.SalvarCRM(descricaoCRM, objCurriculo, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
                }
                else
                {
                    FilialObservacao.SalvarCRM(descricaoCRM, objPagamento.Filial, new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value), null);
                }
                ExibirMensagem(descricaoCRM, TipoMensagem.Aviso);

                upGvParcelas.Update();
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

        #region chkNotaAntecipada_CheckedChanged
        protected void chkNotaAntecipada_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNotaAntecipada.Checked)
            {
                divDataNFAntecipada.Visible = true;
                txtDataNFAntecipada.Obrigatorio = true;
            }
            else
            {
                divDataNFAntecipada.Visible = false;
                txtDataNFAntecipada.Obrigatorio = false;
            }
        }
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

                    string img = string.Empty;

                    hlPDFNormal.NavigateUrl = BoletoBancario.RetornarBoleto(objPagamento, out img);
                    hlPDFCobranca.NavigateUrl = img;

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
                    if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value))
                        ExibirMensagem("Nota Fiscal emitida com sucesso !", TipoMensagem.Aviso);
                    else
                        ExibirMensagem("Erro ao emitir Nota fiscal", TipoMensagem.Aviso);
                else
                        if (BLL.PlanoParcela.EmitirNF(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value))
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
        private bool Salvar()
        {
            try
            {
                bool modificacao = false; //Utilizado para controlar se deve abrir um novo pagamento

                if (!IdPagamento.HasValue)
                {
                    PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquirido);

                    if (objPlanoAdquirido != null && objPlanoAdquirido.FlagRecorrente)
                    {
                        var objPlanoParcela = PlanoParcela.CriarParcelaRecorrenciaPeloPlanoAdquirido(objPlanoAdquirido, null);
                        var objPagamentoBoleto = Pagamento.CriarPagamentoBoletoRecorrencia(objPlanoParcela, objPlanoAdquirido, txtDataVencimento.ValorDatetime, txtValor.Valor.Value);

                        IdPagamento = objPagamentoBoleto.IdPagamento;
                    }
                }

                Pagamento objPagamento = BLL.Pagamento.LoadObject(IdPagamento.Value);
                Pagamento objPagamentoOld = objPagamento.Clone() as Pagamento;

                PlanoParcela objPlanoParcelaOld = null;
                PlanoAdquirido objPlanoAdquiridoOld = null;

                if (Convert.ToInt32(rcbSituacao.SelectedValue) == (int)Enumeradores.PagamentoSituacao.EmAberto 
                                && (!objPagamento.DataVencimento.Value.Date.Equals(txtDataVencimento.ValorDatetime.Value.Date) || !objPagamento.ValorPagamento.Equals(txtValor.Valor.Value)) || !objPagamento.FlagJuros.Equals(chkJuros.Checked))
                    modificacao = true;
                
               //valorJuros = ValorJuros(chkJuros.Checked, txtValor.Valor.Value, objPagamento);

                objPagamento.FlagJuros = chkJuros.Checked;
                objPagamento.PagamentoSituacao = new PagamentoSituacao(Convert.ToInt32(rcbSituacao.SelectedValue));
                objPagamento.DataEmissao = txtDataEnvio.ValorDatetime;

                //Vai ser Adicicionar a nova data no venciment quando for criar o boleto
                //objPagamento.DataVencimento = txtDataVencimento.ValorDatetime;

                objPagamento.ValorPagamento = txtValor.Valor.Value;

                objPagamento.NumeroNotaFiscal = txtNumeroNota.Valor;
                objPagamento.UrlNotaFiscal = txtUrlNota.Text;
                objPagamento.DesOrdemDeCompra = txtOrdemDeCompra.Valor;
                

                if (objPagamento.AdicionalPlano == null)
                {
                    objPagamento.PlanoParcela.CompleteObject();
                    objPlanoParcelaOld = objPagamento.PlanoParcela;

                    objPagamento.PlanoParcela.ValorParcela = txtValor.Valor.Value;
                    objPagamento.PlanoParcela.EmailEnvioBoleto = txtEmailBoleto.Text;
                    if (chkNotaAntecipada.Checked)
                    {
                        if (txtDataNFAntecipada.ValorDatetime > objPagamento.DataVencimento.Value.Date)
                        {
                            //ExibirMensagemAlerta("Data Nota Antecipada", "Data de Envio de Nota Antecipada é maior que a data de vencimento do boleto");
                            return false;
                        }
                        objPagamento.PlanoParcela.DataEmissaoNotaAntecipada = txtDataNFAntecipada.ValorDatetime;
                    }
                    else
                        objPagamento.PlanoParcela.DataEmissaoNotaAntecipada = null;
                    objPagamento.PlanoParcela.PlanoAdquirido.CompleteObject();
                    objPlanoAdquiridoOld = objPagamento.PlanoParcela.PlanoAdquirido;
                    objPagamento.PlanoParcela.PlanoAdquirido.FlgNotaAntecipada = chkNotaAntecipada.Checked;
                }

                int qtdeSMSTotal;
                int qtdeSMSLiberada;
                Int32.TryParse(txtQtdeSMSLiberada.Valor, out qtdeSMSLiberada);
                Int32.TryParse(txtQtdeSMSTotal.Valor, out qtdeSMSTotal);

                var descricaoCRM = string.Empty;
                UsuarioFilialPerfil objUsuarioGerador = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);

                switch (Convert.ToInt32(rcbSituacao.SelectedValue))
                {
                    case (int)Enumeradores.PagamentoSituacao.Cancelado:
                        BNE.BLL.Pagamento.CancelarPagamento(objPagamento, base.IdUsuarioFilialPerfilLogadoUsuarioInterno.Value);
                        descricaoCRM = string.Concat("Pagamento ", objPagamento.IdPagamento, " cancelado!");
                        break;
                    case (int)Enumeradores.PagamentoSituacao.Pago:
                        PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(IdPlanoAdquirido);
                        var objPlano = Plano.CarregarPlanoDePagamento(objPagamento.IdPagamento);
                        if (objPagamento.Liberar(DateTime.Now) && (objPlano.FlagRecorrente || objPlano.FlagBoletoRecorrente))
                            PlanoAdquirido.SalvarNovaDataFimPlano(objPlanoAdquirido);

                        descricaoCRM = string.Concat("Pagamento ", objPagamento.IdPagamento, " Liberado Manualmente!");
                        break;
                }

                string descricao = string.Empty;

                if (objPagamento.AdicionalPlano == null)
                {
                    objPagamento.PlanoParcela.Save();
                    if (!objPagamento.PlanoParcela.PlanoAdquirido.FlgNotaAntecipada)
                        objPagamento.PlanoParcela.PlanoAdquirido.FlgNotaAntecipada = PlanoParcela.ExisteParcelaDataEmissaoNotaAntecipadaPlanoAdquirido(objPagamento.PlanoParcela.PlanoAdquirido.IdPlanoAdquirido);
                    objPagamento.PlanoParcela.PlanoAdquirido.Save();

                    descricao += objPlanoAdquiridoOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objPlanoAdquiridoOld, objPagamento.PlanoParcela.PlanoAdquirido, objPlanoAdquiridoOld.GetType())) : string.Empty;
                    descricao += objPlanoParcelaOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objPlanoParcelaOld, objPagamento.PlanoParcela, objPlanoParcelaOld.GetType())) : string.Empty;
                }

                if (modificacao)
                {
                    objPagamento.Salvar(qtdeSMSTotal, qtdeSMSLiberada);

                    try
                    {
                        string img = string.Empty;
                        //Se Editar parcela é criado um boleto novo.
                        hlPDFNormal.NavigateUrl = BoletoBancario.RetornarBoleto(objPagamento, out img, true, txtDataVencimento.ValorDatetime);
                        hlPDFCobranca.NavigateUrl = img;
                      
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

                ///CRM
                descricao += objPagamentoOld != null ? MensagemAssincronoLogCRM.StringListaDeItensModificados(MensagemAssincronoLogCRM.DiffProperties(objPagamentoOld, objPagamento, objPagamentoOld.GetType())) : string.Empty;

                if (!string.IsNullOrEmpty(descricao))
                {
                    PlanoAdquirido pa = PlanoAdquirido.CarregarPlanoAdquiridopDePagamento(objPagamento.IdPagamento);
                    if (pa.ParaPessoaFisica())
                    {
                        if (objUsuarioGerador != null)
                            MensagemAssincronoLogCRM.SalvarModificacoesCurriculoCRM(string.Concat(descricaoCRM, descricao), new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(objPagamento.UsuarioFilialPerfil.PessoaFisica)), objUsuarioGerador, null);
                        else
                            MensagemAssincronoLogCRM.SalvarModificacoesCurriculoCRM(string.Concat(descricaoCRM, descricao), new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(objPagamento.UsuarioFilialPerfil.PessoaFisica)), null, "DetalhesPlano");
                    }
                    else
                    {
                        if (objUsuarioGerador != null)
                            MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(string.Concat(descricaoCRM, "Tela Detalhes do Plano: <br/><br/>", descricao), objPagamento.Filial, objUsuarioGerador, null);
                        else
                            MensagemAssincronoLogCRM.SalvarModificacoesFilialCRM(string.Concat(descricaoCRM, "Tela Detalhes do Plano: <br/><br/>", descricao), objPagamento.Filial, null, "DetalhesPlano");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                BNE.EL.GerenciadorException.GravarExcecao(ex);
                return true;
            }
        }

        private decimal RetornaValorPagamento(bool juros, Pagamento objPagamento)
        {
            


            return 0;
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
            txtUrlNota.Text = objPagamento.UrlNotaFiscal;
            pnlQtdeSMS.Visible = false;
            txtOrdemDeCompra.Valor = objPagamento.DesOrdemDeCompra;
            chkJuros.Checked = objPagamento.FlagJuros;
            

            if (objPagamento.PlanoParcela != null)
            {
                pnlQtdeSMS.Visible = true;

                objPagamento.PlanoParcela.CompleteObject();
                txtQtdeSMSTotal.Valor = objPagamento.PlanoParcela.QuantidadeSMSTotal.ToString();
                txtQtdeSMSLiberada.Valor =
                    objPagamento.PlanoParcela.QuantidadeSMSLiberada.HasValue ?
                        objPagamento.PlanoParcela.QuantidadeSMSLiberada.Value.ToString() : String.Empty;
                chkLiberarSMS.Checked = PlanoParcela.EstaLiberadoSMS(IdPlanoAdquirido);
                txtQtdeSMSLiberada.Enabled = !chkLiberarSMS.Checked;
                txtEmailBoleto.Text = PlanoAdquiridoDetalhes.RetornaEmailNotas(objPagamento.IdPagamento);//string.IsNullOrEmpty(objPagamento.PlanoParcela.EmailEnvioBoleto) ? planoAdiquiridoDetalhes.EmailEnvioBoleto : objPagamento.PlanoParcela.EmailEnvioBoleto;
                if (objPagamento.PlanoParcela.DataEmissaoNotaAntecipada.HasValue)
                {
                    chkNotaAntecipada.Checked = true;
                    divDataNFAntecipada.Visible = true;
                    txtDataNFAntecipada.Obrigatorio = true;
                    txtDataNFAntecipada.ValorDatetime = objPagamento.PlanoParcela.DataEmissaoNotaAntecipada;
                }
                else
                {
                    chkNotaAntecipada.Checked = false;
                    divDataNFAntecipada.Visible = false;
                    txtDataNFAntecipada.Obrigatorio = false;
                    txtDataNFAntecipada.ValorDatetime = DateTime.Today.AddDays(1);
                }

            }
            else
            {
                divNotaAntecipada.Visible = false;
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
            txtEmailBoleto.Text = null;
            txtNumeroNota.Valor = null;
            txtUrlNota.Text = null;
            rcbSituacao.SelectedValue = "1";
            txtQtdeSMSTotal.Valor = null;
            txtQtdeSMSLiberada.Valor = null;
            pnlQtdeSMS.Visible = false;
            txtOrdemDeCompra.Valor = null;
            chkJuros.Checked = false;
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
        public bool AjustarVisibilidadeVisualizarBoleto(int idTipoPagamento, bool cortesia, int IdfPagamentoSituacao)
        {
            if (cortesia)
                return false;

            if (idTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario)
                return true;
            else if (idTipoPagamento == (int)Enumeradores.TipoPagamento.CartaoCredito && IdfPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto)
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

        #region AjustarVisibilidadeEstorno

        public bool AjustarVisibilidadeEstorno(int idTipoPagamento, int idPagamentoSituacao)
        {
            if (!Permissoes.Contains((int)Enumeradores.Permissoes.Administrador.FinanceiroEstornoPagamento))
                btnSalvar.Enabled = false;
            if (idPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.Pago && idTipoPagamento == (int)Enumeradores.TipoPagamento.CartaoCredito)
                return true;
            return false;
        }
        #endregion

        #endregion

        #region Delegates
        public delegate void delegateVoltar();
        public event delegateVoltar Voltar;
        #endregion

        /// <summary>
        /// Retorna o valor a ser pago com a insidência de juros
        /// </summary>
        /// <param name="flgJuros"></param>
        /// <param name="objPagamento"></param>F
        public decimal ValorJuros(Boolean flgJurosTela, decimal valor, Pagamento pagamento)
        {
            decimal valorNovo = 0;
            if (flgJurosTela && !pagamento.FlagJuros) {
                int percentual = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.TaxaJurosAtraso));
                valorNovo = valor * (Convert.ToDecimal(percentual) / 100);
                return Convert.ToDecimal(valorNovo);
            }
            if (!flgJurosTela && pagamento.FlagJuros)
            {
                valorNovo = pagamento.ValorJuros * (-1);
                return valorNovo;
            }
            return valorNovo;
        }
    }
}
