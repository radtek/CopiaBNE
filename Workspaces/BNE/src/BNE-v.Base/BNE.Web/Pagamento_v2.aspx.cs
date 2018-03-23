using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class Pagamento_v2 : BasePagePagamento
    {
        #region Propriedades

        #region ContadorTentativas
        private byte ContadorTentativas
        {
            get
            {
                if (ViewState["ContadorTentativas"] == null)
                    ViewState["ContadorTentativas"] = 0;

                return Convert.ToByte(ViewState["ContadorTentativas"]);
            }
            set
            {
                ViewState["ContadorTentativas"] = value;
            }
        }
        #endregion

        #region CancelarEvento
        public bool CancelarEvento
        {
            get;
            set;
        }
        #endregion

        #region IdPlanoAdquirido
        /// <summary>
        /// Propriedade que armazena e recupera o valor pago pelo plano
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

        #region TipoPessoaFisica
        /// <summary>
        /// Propriedade que armazena e recupera o valor pago pelo plano
        /// </summary>
        public int TipoPessoaFisica
        {
            get
            {
                return Int32.Parse(ViewState[Chave.Temporaria.Variavel3.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }
        #endregion

        #region ValorPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        public Decimal ValorPagamento
        {
            get
            {
                return Decimal.Parse(ViewState[Chave.Temporaria.Variavel2.ToString()].ToString());
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #region CodigoDesconto
        /// <summary>
        /// Propriedade que armazena e recupera o Codigo de Desconto
        /// </summary>
        public string CodigoDesconto
        {
            get
            {
                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    CodigoDesconto codigoDesconto =
                        BLL.CodigoDesconto.LoadObject(base.PagamentoIdCodigoDesconto.Value);

                    return codigoDesconto.DescricaoCodigoDesconto;
                }
                else
                {
                    return null;
                }
            }
        }
        #endregion

        #region PercentualDesconto
        /// <summary>
        /// Propriedade que armazena e recupera o percentual de desconto
        /// </summary>
        public decimal PercentualDesconto
        {
            get
            {
                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    CodigoDesconto codigoDesconto =
                        BLL.CodigoDesconto.LoadObject(base.PagamentoIdCodigoDesconto.Value);

                    if (codigoDesconto.TipoCodigoDesconto != null)
                    {
                        codigoDesconto.TipoCodigoDesconto.CompleteObject();
                        return codigoDesconto.TipoCodigoDesconto.NumeroPercentualDesconto;
                    }
                }

                return Decimal.Zero;
            }
        }
        #endregion

        #region QuantidadePrazoBoleto - Variavel 6
        public int QuantidadePrazoBoleto
        {
            get
            {
                return Convert.ToInt32(Session[Chave.Temporaria.Variavel6.ToString()]);
            }
            set
            {
                Session.Add(Chave.Temporaria.Variavel6.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            CancelarEvento = false;

            if (!IsPostBack)
            {
                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    txtCodigoCredito.Text = CodigoDesconto;
                    btnValidarCodigoCredito_Click(sender, e);   // valida novamente codigo de credito
                }

                int anoAtual = DateTime.Now.Year % 100;
                for (int i = 0; i < 10; i++)
                {
                    ddlAnoVencimento.Items.Add((anoAtual + i).ToString());
                }

                if (!string.IsNullOrEmpty(Request.QueryString["IdPlanoAdquirido"]))
                    base.PagamentoIdentificadorPlanoAdquirido.Value = Convert.ToInt32(Request.QueryString["IdPlanoAdquirido"].ToString());

            }

            if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, GetType().ToString());
            else
                InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, GetType().ToString());
        }
        #endregion

        #region Evento Click - PayPal
        protected void btnPayPal_Click(object sender, EventArgs e)
        {
            PagamentoPayPal();
        }
        #endregion   

        #region Evento Click - Cartao Credito
        protected void btnFinalizarCartaoCredito_Click(object sender, EventArgs e)
        {
            base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.CartaoCredito;
            ValidarPagamento();
        }
        #endregion

        #region Evento Click - Pagamento Boleto
        protected void btnPagamentoBoleto_Click(object sender, EventArgs e)
        {
            PagamentoBoleto();
        }
        #endregion

        #region Evento Click - Codigo de Desconto
        protected void btnValidarCodigoCredito_Click(object sender, EventArgs e)
        {
            ValidarCodigoDesconto();
        }
        #endregion

        #region Evento Click - Pagamento PagSeguro
        protected void btnPagSeguro_Click(object sender, EventArgs e)
        {
            PagamentoPagSeguro();
        }
        #endregion

        #region Evento Click - Tela Pagamento HSBC
        protected void btPagamentoHSBC_Click(object sender, EventArgs e)
        {
            opcaoDebito.Visible = false;
            debitoHSBC.Visible = true;
        }
        #endregion

        #region Evento Click - Debito Recorrente HSBC
        protected void btnFinalizarDebito_Click(object sender, EventArgs e)
        {

            base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.DebitoRecorrente;
            ValidarPagamento();
        }
        #endregion

        #region Evento Click - Voltar Pagamento Debito
        protected void bntVoltarDebito_Click(object sender, EventArgs e)
        {
            debitoHSBC.Visible = false;
            opcaoDebito.Visible = true;
        }
        #endregion

        #region Evento Click - Debito Online Banco do Brasil
        protected void ButtonBB_Click(object sender, EventArgs e)
        {
            base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.DebitoOnline;
            ValidarPagamento();
        }

        #endregion       

        #region Evento TextChanged - Validar Codigo Desconto
        protected void txtCodigoCredito_TextChanged(object sender, EventArgs e)
        {
            ValidarCodigoDesconto();
        }
        #endregion

        #endregion

        #region Métodos

        #region Pagamentos

        #region Pagamento - PayPal
        private void PagamentoPayPal()
        {
            ProcessoCompra(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.PayPal);

            BLL.Pagamento objPagamento;
            PlanoAdquirido objPlanoAdquirido;

            CarregarPlanoAdquirido(out objPagamento, out objPlanoAdquirido);
            String paymentRedirectUri = null;

            #region CriandoRequisicaoPayPal
            try
            {
                paymentRedirectUri = Transacao.CriarTransacaoPayPal(objPagamento, objPlanoAdquirido, base.IdPessoaFisicaLogada.Value, PageHelper.RecuperarIP(), UIHelper.GetAbsoluteUrl(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamentoIntermediadores.ToString(), new { Intermediador = IntermediadorPagamento.PayPal.ToString(), IdfPlanoAdquirido = base.PagamentoIdentificadorPlanoAdquirido.Value })));
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            #endregion CriandoRequisicaoPayPal

            if (paymentRedirectUri != null)
            {
                Redirect(paymentRedirectUri);
            }
        }
        #endregion

        #region Pagamento - PagSeguro
        private void PagamentoPagSeguro()
        {
            ProcessoCompra(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.PagSeguro);

            BLL.Pagamento objPagamento;
            PlanoAdquirido objPlanoAdquirido;

            CarregarPlanoAdquirido(out objPagamento, out objPlanoAdquirido);
            String paymentRedirectUri = null;

            #region CriandoRequisicaoPagseguro
            try
            {
                paymentRedirectUri = Transacao.CriarTransacaoPagSeguro(objPagamento, objPlanoAdquirido, base.IdPessoaFisicaLogada.Value, PageHelper.RecuperarIP(), UIHelper.GetAbsoluteUrl(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamentoIntermediadores.ToString(), new { Intermediador = IntermediadorPagamento.PagSeguro.ToString(), IdfPlanoAdquirido = base.PagamentoIdentificadorPlanoAdquirido.Value })));
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
            #endregion CriandoRequisicaoPagseguro

            if (paymentRedirectUri != null)
            {
                Redirect(paymentRedirectUri);
            }
        }
        #endregion

        #region Pagamento - Boleto Bancario
        private void PagamentoBoleto()
        {
            ProcessoCompra(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.BoletoBancario);

            base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.BoletoBancario;
            base.PagamentoValorPago.Value = ValorPagamento;

            if (TipoPessoaFisica.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
            {
                Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamento.ToString(), null));
            }
            Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamentoCIA.ToString(), null));
        }
        #endregion

        #region Pagamento - Cartao de Credito
        private void PagamentoCartao()
        {
            ProcessoCompra(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.CartaoCredito);
        }
        #endregion

        #region Pagamento - Debito Recorrente
        private void PagamentoDebito()
        {
            ProcessoCompra(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.DebitoRecorrente);
        }
        #endregion

        #region Pagamento - Processo de Compra
        public void ProcessoCompra(int idPlanoAdquirido, int idTipoPagamento)
        {
            // Carrega Plano Adquirido
            var objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
            objPlanoAdquirido.Plano.CompleteObject();
            objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();

            if (objPlanoAdquirido.Plano.IdPlano.Equals(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.VendaPlanoCIA_PlanoSmsEmailVagaIdentificador))))
            {
                int idVaga = 0;

                if (!string.IsNullOrEmpty(Request.QueryString["Idf_Vaga"]))
                    idVaga = Convert.ToInt32(Request.QueryString["Idf_Vaga"]);

                PlanoAdquiridoDetalhes.CriarPladoAdDetalhesPlanoSmsVaga(objPlanoAdquirido, idVaga);
            }

            base.PagamentoFormaPagamento.Value = idTipoPagamento;

            if (objPlanoAdquirido.ParaPessoaFisica())
            {
                TipoPessoaFisica = (int)Enumeradores.PlanoTipo.PessoaFisica;

                CodigoDesconto objCodigoDesconto = null;
                if (base.PagamentoIdCodigoDesconto.HasValue)
                    objCodigoDesconto = new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);

                if (!PlanoParcela.ExisteParcelaCriada(objPlanoAdquirido))
                    objPlanoAdquirido.CriarParcelas(new TipoPagamento(base.PagamentoFormaPagamento.Value), objCodigoDesconto, objPlanoAdquirido.QuantidadePrazoBoleto);
                else
                    objPlanoAdquirido.AjustarParcelas(new TipoPagamento(base.PagamentoFormaPagamento.Value), objCodigoDesconto, objPlanoAdquirido.QuantidadePrazoBoleto);

                pnlCodigoCredito.Visible = true;
            }
            else
            {
                objPlanoAdquirido.Filial.CompleteObject();
                TipoPessoaFisica = (int)Enumeradores.PlanoTipo.PessoaJuridica;

                if (!PlanoParcela.ExisteParcelaCriada(objPlanoAdquirido))
                    objPlanoAdquirido.CriarParcelas(new TipoPagamento(base.PagamentoFormaPagamento.Value), null, objPlanoAdquirido.QuantidadePrazoBoleto);
                else
                    objPlanoAdquirido.AjustarParcelas(new TipoPagamento(base.PagamentoFormaPagamento.Value), null, objPlanoAdquirido.QuantidadePrazoBoleto);
            }

            AtualizaValorPagarNaTela();
        }
        #endregion

        #region Pagamento - Retorno e  Redirecionamento
        private void trataRedirecionamento(Pagamento objPagamento, bool sucesso, string msgErro)
        {
            if (!sucesso) return;

            StatusTransacaoCartao = true;

            base.PagamentoValorPago.Value = objPagamento.ValorPagamento;
            base.PagamentoIdentificadorPlanoAdquirido.Value = IdPlanoAdquirido;

            Plano objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);
            if (objPlano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
            {
                Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamento.ToString(), null));
            }
            Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamentoCIA.ToString(), null));


            //Apagando campos informados pelo usuário
            //Obrigatório para validação de segurança da Cielo
            txtNumeroCartao.Valor = String.Empty;
            ddlMesVencimento.SelectedValue = "00";
            ddlAnoVencimento.SelectedValue = "00";
            txtCodigoVerificadorCartao.Valor = String.Empty;
            upCartaoCredito.Update();
            ScriptManager.RegisterClientScriptBlock(upCartaoCredito, upCartaoCredito.GetType(), "inicializa_cartao_credito", "inicializa_cartao_credito();", true);
            ScriptManager.RegisterClientScriptBlock(upCartaoCredito, upCartaoCredito.GetType(), "inicializa_debito_HSBC", "inicializa_debito_HSBC();", true);
        }
        #endregion

        #region Pagamento - Atualiza o Valor de Pagamento na Tela
        protected void AtualizaValorPagarNaTela()
        {
            decimal valorPagamento = Decimal.Zero;
            if (base.PagamentoIdentificadorPlanoAdquirido.HasValue)
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(base.PagamentoIdentificadorPlanoAdquirido.Value);

                IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                // calcula o valor a pagar, usando o desconto
                valorPagamento = objPlanoAdquirido.ValorBase;

                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    var objCodigoDesconto = new BLL.CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);
                    objCodigoDesconto.CalcularDesconto(ref valorPagamento);
                }
            }
            else if (base.PagamentoAdicionalValorTotal.HasValue && base.PagamentoAdicionalQuantidade.HasValue) //Plano adicional
            {
                valorPagamento = base.PagamentoAdicionalValorTotal.Value;
            }

            //Atualizando valor do TrackingEcommece
            ValorPagamento = valorPagamento;
        }
        #endregion

        #endregion

        #region Validações

        #region Validar - Codigo de Desconto
        private void ValidarCodigoDesconto()
        {
            if (CancelarEvento)
                return;

            CancelarEvento = true;

            LimparSessionDesconto();
            AtualizaValorPagarNaTela();

            if (++ContadorTentativas > 100)     // impedir que bots descubram codigos
            {
                DeslogarUsuario();
                return;
            }

            if (string.IsNullOrEmpty(txtCodigoCredito.Text))
                return;

            CodigoDesconto codigo;
            if (!BLL.CodigoDesconto.CarregarPorCodigo(txtCodigoCredito.Text, out codigo))
            {
                ExibirMensagem("Código promocional inválido", TipoMensagem.Erro);
                return;
            }

            if (codigo.JaUtilizado())
            {
                ExibirMensagem("Código promocional já utilizado", TipoMensagem.Erro);
                return;
            }

            if (!codigo.DentroValidade())
            {
                ExibirMensagem("Código promocional fora da validade", TipoMensagem.Erro);
                txtCodigoCredito.Text = String.Empty;
                return;
            }

            TipoCodigoDesconto tipoCodigoDesconto;
            if (!codigo.TipoDescontoDefinido(out tipoCodigoDesconto))
            {
                ExibirMensagem("Código promocional inválido", TipoMensagem.Erro);
                txtCodigoCredito.Text = String.Empty;
                return;
            }

            List<Plano> planosVinculados;
            if (codigo.HaPlanosVinculados(out planosVinculados))
            {
                if (planosVinculados.Any(plano =>
                        plano.IdPlano == base.PagamentoIdentificadorPlano.Value))
                {
                    // o plano está vinculado corretamente
                }
                else if (planosVinculados.Count > 0)
                {
                    // existem planos vinculados, mas nenhum corresponde ao plano escolhido para pagamento
                    ExibirMensagem("Esse código promocional não serve para o plano escolhido! "
                        + "Favor escolher outro plano ou trocar o código promocional", TipoMensagem.Erro);
                    return;
                }
            }

            // seta sessao com as informacoes sobre o desconto
            base.PagamentoIdCodigoDesconto.Value = codigo.IdCodigoDesconto;
            ExibirMensagem(null, TipoMensagem.Erro);

            // se o codigo de credito for de 100%, concede desconto integral imediatamente
            string erro = null;
            if (PercentualDesconto >= 99.9m)
            {
                try
                {
                    if (BLL.Pagamento.ConcederDescontoIntegral(
                        new Curriculo(base.IdCurriculo.Value),
                        new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoCandidato.Value),
                        new Plano(base.PagamentoIdentificadorPlano.Value),
                        new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value),
                        out erro))
                    {
                        base.StatusTransacaoCartao = true;
                        string urlRedirect = base.PagamentoUrlRetorno.Value;
                        base.LimparSessionPagamento();

                        Redirect(urlRedirect);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    if (!(ex is ThreadAbortException))
                        EL.GerenciadorException.GravarExcecao(ex);

                    erro = ex.Message;
                }
                finally
                {
                    if (!string.IsNullOrEmpty(erro))
                        ExibirMensagem("Erro durante concessão de crédito: " + erro, TipoMensagem.Erro);
                }
            }

            AtualizaValorPagarNaTela();
        }
        #endregion

        #region Validar - Todos os Pagamentos
        protected void ValidarPagamento()
        {
            try
            {
                bool sucesso = false;
                string msgErro = String.Empty;
                BLL.Pagamento objPagamento = null;

                switch ((Enumeradores.TipoPagamento)base.PagamentoFormaPagamento.Value)
                {
                    case BNE.BLL.Enumeradores.TipoPagamento.CartaoCredito:
                        sucesso = ValidarPagamentoCartaoCredito(out objPagamento, out msgErro);
                        trataRedirecionamento(objPagamento, sucesso, msgErro);
                        break;

                    case BNE.BLL.Enumeradores.TipoPagamento.BoletoBancario:
                        break;

                    case BNE.BLL.Enumeradores.TipoPagamento.DepositoIdentificado:
                        break;

                    case BNE.BLL.Enumeradores.TipoPagamento.Parceiro:
                        break;

                    case BNE.BLL.Enumeradores.TipoPagamento.DebitoOnline:
                        sucesso = ValidarPagamentoDebitoOnline(out msgErro);
                        break;

                    case BNE.BLL.Enumeradores.TipoPagamento.PagSeguro:
                        break;

                    case BNE.BLL.Enumeradores.TipoPagamento.PayPal:
                        break;

                    case BNE.BLL.Enumeradores.TipoPagamento.DebitoRecorrente:
                        sucesso = ValidarPagamentoDebito(out objPagamento, out msgErro);
                        trataRedirecionamento(objPagamento, sucesso, msgErro);
                        break;

                    default:
                        break;
                }

                if (!sucesso)
                {
                    if (string.IsNullOrEmpty(msgErro))
                        ExibirMensagem("Ocorreu um erro ao tentar validar sua compra, tente novamente!", TipoMensagem.Erro);
                    else
                        ExibirMensagem(msgErro, Web.Code.Enumeradores.TipoMensagem.Erro);
                }

            }
            catch (Exception ex)
            {
                ExibirMensagem("Ocorreu um erro ao tentar validar sua compra, tente novamente!", TipoMensagem.Erro);
                if (!(ex is ThreadAbortException))
                    EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region Validar - Pagamento Cartao Credito
        private bool ValidarPagamentoCartaoCredito(out BLL.Pagamento objPagamento, out string erro)
        {
            ProcessoCompra(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.CartaoCredito);

            PlanoAdquirido objPlanoAdquirido;
            CarregarPlanoAdquirido(out objPagamento, out objPlanoAdquirido);

            string numeroCartao = txtNumeroCartao.Valor;
            int mesValidadeCartao = Convert.ToInt32(ddlMesVencimento.SelectedValue);
            int anoValidadeCartao = Convert.ToInt32(ddlAnoVencimento.SelectedValue);
            string numeroDigitoVerificador = txtCodigoVerificadorCartao.Valor;

            return Transacao.ValidarPagamentoCartaoCredito(ref objPagamento,
            IdPlanoAdquirido, PageHelper.RecuperarIP(), numeroCartao,
            mesValidadeCartao, anoValidadeCartao, numeroDigitoVerificador, out erro);
        }
        #endregion

        #region Validar - Pagamento Debito Recorrente
        private bool ValidarPagamentoDebito(out BLL.Pagamento objPagamento, out string erro)
        {
            ProcessoCompra(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.DebitoRecorrente);

            PlanoAdquirido objPlanoAdquirido;
            CarregarPlanoAdquirido(out objPagamento, out objPlanoAdquirido);

            if (objPagamento == null || objPlanoAdquirido == null)
            {
                //Se o pagamento ou o plano adquiridos não foram carregados, redireciona o usuário à tela de escolha de plano.
                Redirect(GetRouteUrl("EscolhaPlano", null));
            }

            //Banco Selecionado
            Enumeradores.Banco banco = Enumeradores.Banco.HSBC;

            //Informações da conta
            string agencia = string.Empty;
            string conta = string.Empty;
            decimal cpf = decimal.Zero;



            switch (banco)
            {
                case Enumeradores.Banco.HSBC:
                    agencia = txtAgenciaDebitoHSBC.Text;
                    conta = txtContaCorrenteDebitoHSBC.Text + txtDigitoDebitoHSBC.Text;
                    cpf = Convert.ToDecimal(txtCPFDebitoHSBC.Valor);
                    break;
            }


            return Transacao.ValidarPagamentoDebito(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), banco, agencia, conta, cpf, null, out erro);
        }
        #endregion      

        #region Validar - Pagamento Debito Online Banco do Brasil
        private bool ValidarPagamentoDebitoOnline(out string msgErro)
        {
            Pagamento objPagamento;
            PlanoAdquirido objPlanoAdquirido;
            
            //Processamento da Compra
            ProcessoCompra(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.DebitoOnline);
            CarregarPlanoAdquirido(out objPagamento, out objPlanoAdquirido);

            //Se o pagamento ou o plano adquiridos não foram carregados, redireciona o usuário à tela de escolha de plano.
            if (objPagamento == null || objPlanoAdquirido == null)
                Redirect(GetRouteUrl("EscolhaPlano", null));

            //Se existe transações do cliente dentro de um intervalo, não o deixe criar uma nova
            if (Transacao.ExisteTransacaoDebitoOnlineNoIntervalo(base.IdPessoaFisicaLogada.Value, Parametro.RecuperaValorParametro(Enumeradores.Parametro.IntervaloTempoSondaDebitoOnlineBB)))
            {
                msgErro = "Existe um pagamento a ser processado, favor aguardar!";
                return false;
            }

            Transacao objTransacao = Transacao.ValidarPagamentoDebitoOnline(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), Enumeradores.Banco.BANCODOBRASIL, out msgErro);

            if (objTransacao != null)
            {
                //Criação do POST de Envio para o BB
                string dadosDaRequisicao = string.Format("idConv={0}&refTran={1}&valor={2}&dtVenc={3}&tpPagamento={4}&urlRetorno={5}",
                                            Parametro.RecuperaValorParametro(Enumeradores.Parametro.ConvenioDebitoBB),
                                            Convert.ToString(objTransacao.IdTransacao).PadLeft(17, '0'),
                                            Convert.ToString(objPagamento.ValorPagamento).Replace(",", ""),
                                            objPagamento.DataVencimento.Value.ToString("ddMMyyyy"),
                                            Parametro.RecuperaValorParametro(Enumeradores.Parametro.DebitoEmContaViaInternetPFePJ),
                                            Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLRetornoDebitoOnline)
                                            );

                Redirect("PagamentoDebitoOnline.aspx?" + dadosDaRequisicao);

                return true;
            }
            else
            {
                msgErro = "Ocorreu um erro ao tentar validar sua compra, tente novamente!";
                return false;
            }
        }
        #endregion

        #endregion 

        #region Outros

        #region Deslogar - Usuario
        private void DeslogarUsuario()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
                new PessoaFisica(IdPessoaFisicaLogada.Value).ZerarDataInteracaoUsuario();

            //BNE.Auth.BNEAutenticacao.DeslogarPadrao();
            BNE.Auth.BNEAutenticacao.DeslogarPagamento();
        }
        #endregion

        #region Carregar - PlanoAdquirido
        private void CarregarPlanoAdquirido(out BLL.Pagamento objPagamento, out PlanoAdquirido objPlanoAdquirido)
        {
            objPagamento = null;
            objPlanoAdquirido = null;

            if (base.PagamentoIdentificadorPlanoAdquirido.HasValue)
            {
                objPlanoAdquirido = PlanoAdquirido.LoadObject(base.PagamentoIdentificadorPlanoAdquirido.Value);
                objPlanoAdquirido.Plano.CompleteObject();

                if (base.ValorBasePlano.HasValue)
                    objPlanoAdquirido.Plano.ValorBase = base.ValorBasePlano.Value;

                if (objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
                {
                    PlanoParcela objPlanoParcela = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquirido(objPlanoAdquirido);
                    var objListPagamentosPorParcela = BLL.Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela);

                    if (objListPagamentosPorParcela != null && objListPagamentosPorParcela.Count > 0)
                    {
                        objPagamento = objListPagamentosPorParcela.FirstOrDefault(p => (p.TipoPagamento == null || p.TipoPagamento.IdTipoPagamento == base.PagamentoFormaPagamento.Value) && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false);

                        IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                        base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                    }
                }
                else if (objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica))
                {
                    PlanoParcela objPlanoParcela = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquirido(objPlanoAdquirido);
                    var objListPagamentosPorParcela = BLL.Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela);

                    if (objListPagamentosPorParcela != null && objListPagamentosPorParcela.Count > 0)
                    {
                        // Se existir algum pagamento com o tipo==null e ativo entao popula o objPagamento
                        objPagamento = objListPagamentosPorParcela.OrderBy(p => p.IdPagamento).FirstOrDefault(p => p.TipoPagamento == null || p.TipoPagamento.IdTipoPagamento == base.PagamentoFormaPagamento.Value && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false);

                        IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                        base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                    }
                }
            }
            else if (base.PagamentoAdicionalValorTotal.HasValue && base.PagamentoAdicionalQuantidade.HasValue)
            {
                PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(new Filial(base.IdFilial.Value), (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido);

                if (objPlanoAdquirido != null)
                {
                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

                    AdicionalPlano.CriarPagamentoEPlanoAdicionalSMS(objPlanoAdquirido, base.PagamentoAdicionalValorTotal.Value, base.PagamentoAdicionalQuantidade.Value, objUsuarioFilialPerfil, (Enumeradores.TipoPagamento)base.PagamentoFormaPagamento.Value, DateTime.Now, DateTime.Today, out objPagamento);

                    IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                    base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                }
            }

            if (objPlanoAdquirido != null)
            {
                base.PagamentoIdentificadorPlanoAdquirido.Value = objPlanoAdquirido.IdPlanoAdquirido;
            }

            if (objPagamento != null)
            {
                base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
            }
        }
        #endregion

        #region Limpar Sessao - Codigo de Desconto
        private void LimparSessionDesconto()
        {
            base.PagamentoIdCodigoDesconto.Clear();
        }
        #endregion

        

        #endregion


        #endregion

    }
}