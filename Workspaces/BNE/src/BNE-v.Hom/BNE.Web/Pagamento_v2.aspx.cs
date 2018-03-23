using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using BNE.BLL.Common;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom;
using BNE.Web.Master;
using System.Configuration;

namespace BNE.Web
{
    public partial class Pagamento_v2 : BasePagePagamento
    {
        public static readonly string UtilizaCielo = ConfigurationManager.AppSettings["UtilizaCielo"];
        #region Propriedades
        public static int HabilitarApenasCartaoCredito { get; set; }
        public static int HabilitarFormasPagamentoPlanosRecorrentes { get; set; }
        public static int DesabilitarTodosOsPagamentos { get; set; }
        public static Boolean FormaDePagamentoBoleto { get; set; }
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

        #region JustificativaVendedor
        /// <summary>
        /// Propriedade que armazena e recupera o Codigo de Desconto
        /// </summary>
        public string JustificativaVendedor
        {
            get
            {
                return base.PagamentoJustificativaAbaixoMinimo.HasValue ? base.PagamentoJustificativaAbaixoMinimo.Value : null;
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

        #region BancoDebitoOnline
        /// <summary>
        /// Propriedade que armazena e recupera o valor do banco selecionado 
        /// </summary>
        public Enumeradores.Banco BancoDebitoOnline
        {
            get;
            set;
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            var master = (Principal)Page.Master;

            if (master != null)
                master.LoginEfetuadoSucesso += master_LoginEfetuadoSucesso;

            bool flgRecorrente = false;

            if (!IsPostBack)
                Inicializar(ref flgRecorrente);
            Validacoes(flgRecorrente);





        }

        private void master_LoginEfetuadoSucesso()
        {
            if (!base.IdUsuarioFilialPerfilLogadoCandidato.HasValue && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                base.ExibirLogin();
            else
            {
                bool recorrente = false;

                Inicializar(ref recorrente);
                Validacoes(recorrente);
            }

        }
        #endregion

        #region Evento Click - PayPal
        protected void btnPayPal_Click(object sender, EventArgs e)
        {
            if (!ValidaFormaDePagamentoAssinatura(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.PayPal))
            {
                base.ExibirMensagem("Não é uma forma de Pagamento Válida!", TipoMensagem.Aviso);
                return;
            }

            PagamentoPayPal();
        }
        #endregion

        #region Evento Click - Cartao Credito
        protected void btnFinalizarCartaoCredito_Click(object sender, EventArgs e)
        {
            if (!chkTermoAceita.Checked)
            {
                base.ExibirMensagem("Termo de Aceite é obrigatorio!", TipoMensagem.Aviso);
                return;
            }

            base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.CartaoCredito;
            ValidarPagamento();
        }
        #endregion

        #region Evento Click - Pagamento Boleto
        protected void btnPagamentoBoleto_Click(object sender, EventArgs e)
        {
            if (!ValidaFormaDePagamentoAssinatura(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.BoletoBancario))
            {
                base.ExibirMensagem("Não é uma forma de Pagamento Válida!", TipoMensagem.Aviso);
                return;
            }
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
            if (!ValidaFormaDePagamentoAssinatura(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.PagSeguro))
            {
                base.ExibirMensagem("Não é uma forma de Pagamento Válida!", TipoMensagem.Aviso);
                return;
            }
            PagamentoPagSeguro();
        }
        #endregion

        #region Evento Click - Tela Pagamento HSBC
        //protected void btPagamentoHSBC_Click(object sender, EventArgs e)
        //{
        //    opcaoDebito.Visible = false;
        //    debitoHSBC.Visible = true;
        //}
        #endregion

        #region Evento Click - Debito Recorrente HSBC
        protected void btnFinalizarDebito_Click(object sender, EventArgs e)
        {
            base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.DebitoRecorrente;
            ValidarPagamento();
        }
        #endregion

        #region Evento Click - Voltar Pagamento Debito
        //protected void bntVoltarDebito_Click(object sender, EventArgs e)
        //{
        //    debitoHSBC.Visible = false;
        //    opcaoDebito.Visible = true;
        //}
        #endregion

        #region Evento Click - Debito Online Bradesco
        protected void btPagamentoBradesco_Click(object sender, EventArgs e)
        {
            if (!ValidaFormaDePagamentoAssinatura(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.DebitoOnline))
            {
                base.ExibirMensagem("Não é uma forma de Pagamento Válida!", TipoMensagem.Aviso);
                return;
            }

            base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.DebitoOnline;
            BancoDebitoOnline = Enumeradores.Banco.BRADESCO;

            ValidarPagamento();
        }
        #endregion

        #region Evento Click - Debito Online Banco do Brasil
        protected void ButtonBB_Click(object sender, EventArgs e)
        {
            if (!ValidaFormaDePagamentoAssinatura(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.DebitoOnline))
            {
                base.ExibirMensagem("Não é uma forma de Pagamento Válida!", TipoMensagem.Aviso);
                return;
            }

            base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.DebitoOnline;
            BancoDebitoOnline = Enumeradores.Banco.BANCODOBRASIL;
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

        #region Inicializar
        private void Inicializar(ref bool flgRecorrente)
        {
            CancelarEvento = false;


            Plano objPlano = null;
            BLL.Pagamento objPagamento;
            PlanoAdquirido objPlanoAdquirido;

            //Tratamento especial para compra de plano do relatório do salario BR - plano adquirido é criado aqui
            if (!base.PagamentoIdentificadorPlano.HasValue)
            {
                if (!string.IsNullOrEmpty(Request.QueryString["IdPlano"]))
                {
                    CarregarParamentrosDeCampanha(out objPlano, out objPlanoAdquirido);
                }
            }
            else if (base.PagamentoIdentificadorPlano.HasValue && !string.IsNullOrEmpty(Request.QueryString["IdPlano"]))
            {
                CarregarParamentrosDeCampanha(out objPlano, out objPlanoAdquirido);
            }

            // Carrega Plano de acordo com plano escolhido na session
            if (base.PagamentoIdentificadorPlano.HasValue && objPlano == null)
                objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);

            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                txtCodigoCredito.Text = CodigoDesconto;
                ValidarCodigoDesconto();  // valida novamente codigo de credito
            }

            int anoAtual = DateTime.Now.Year % 100;
            for (int i = 0; i < 10; i++)
            {
                ddlAnoVencimento.Items.Add((anoAtual + i).ToString());
            }

             if (!base.IdFilial.HasValue)//pessoa fisica só pode comprar com cartao
            {
                pnlOutrasFormasPagamento.Visible = false;
            }

            if (!string.IsNullOrEmpty(Request.QueryString["IdPlanoAdquirido"]))
                base.PagamentoIdentificadorPlanoAdquirido.Value = Convert.ToInt32(Request.QueryString["IdPlanoAdquirido"].ToString());

            CarregarPlanoAdquirido(out objPagamento, out objPlanoAdquirido);

            if (objPlanoAdquirido != null)
            {
                flgRecorrente = objPlanoAdquirido.Plano.FlagRecorrente;

                if (objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                {
                    ltValorPlano.Text = (base.ValorBasePlano.HasValue == true ? base.ValorBasePlano.Value.ToString("C") : objPlano.ValorBase.ToString("C"));

                    if (objPlanoAdquirido.Plano.IdPlano.Equals(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoCandidaturaPremium))))
                    {
                        //evitar drop se resolverem mudar o texto ¬¬'
                        lblPlano.Visible = false;
                        lblPremium.Visible = true;
                        divBreadCrumb.Visible = false;
                    }
                    if (objPlano.QuantidadeDiasValidade == 0)
                        ltNomePlano.Text = string.Empty;
                    else if (objPlano.FlagRecorrente)
                        ltNomePlano.Text = "Assinatura";
                    else
                        ltNomePlano.Text = objPlano.QuantidadeDiasValidade + " dias";
                }
                else
                {
                    divBreadCrumb.Visible = false;
                    ltValorPlano.Text = objPlanoAdquirido.ValorBase.ToString("C");
                    ltNomePlano.Text = objPlanoAdquirido.Plano.DescricaoPlano;
                }

                FormaDePagamentoBoleto = objPlano.FlagBoletoRecorrente && objPlano.FlagRecorrente;

                updTextAcesso.Update();

                updTermoUso.Visible = objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo == (int)BLL.Enumeradores.PlanoTipo.PessoaFisica;

            }
        }

        private void CarregarParamentrosDeCampanha(out Plano objPlano, out PlanoAdquirido objPlanoAdquirido)
        {
            PlanoAdquirido objPlanoAdquiridoAuxiliar = null;
            //Seta o plano na sessão
            base.PagamentoIdentificadorPlano.Value = Convert.ToInt32(Request.QueryString["IdPlano"].ToString());

            //Cria o plano adquirido
            objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);


            if (objPlano.ParaPessoaJuridica())
            {
                var objFilial = Filial.LoadObject(base.IdFilial.Value);
                var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

                UsuarioFilial objUsuarioFilial;
                UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial);

                objPlanoAdquiridoAuxiliar = PlanoAdquirido.CriarPlanoAdquiridoPJ(objUsuarioFilialPerfil, objFilial, objUsuarioFilial, objPlano, 0);

                base.PagamentoIdentificadorPlanoAdquirido.Value = objPlanoAdquiridoAuxiliar.IdPlanoAdquirido;
            }
            else
            {
                if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                {
                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoCandidato.Value);

                    UsuarioFilial objUsuarioFilial;
                    UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(objUsuarioFilialPerfil.IdUsuarioFilialPerfil, out objUsuarioFilial);

                    objPlanoAdquiridoAuxiliar = PlanoAdquirido.CriarPlanoAdquiridoPF(objUsuarioFilialPerfil, objPlano, 1);

                    base.PagamentoIdentificadorPlanoAdquirido.Value = objPlanoAdquiridoAuxiliar.IdPlanoAdquirido;
                }
                else
                {
                    base.PagamentoUrlRetorno.Value = Request.Url.AbsoluteUri;
                    base.ExibirLogin();
                }

            }
            objPlanoAdquirido = objPlanoAdquiridoAuxiliar;

        }

        #endregion

        #region Validacoes
        private void Validacoes(bool flgRecorrente)
        {
            //Validação se já existe compra
            if (base.PagamentoIdentificadorPlanoAdquirido.HasValue)
            {
                PlanoAdquirido objPlanoAdquiridoValidacao = PlanoAdquirido.LoadObject(base.PagamentoIdentificadorPlanoAdquirido.Value);

                TratarVisibilidadeFormasPagamento(flgRecorrente);

                //Trataticas Pessoas Júridicas
                if (objPlanoAdquiridoValidacao.ParaPessoaJuridica())
                {
                    if (PlanoAdquirido.ExistePlanoAdquridoEnviadoOuNaoEnviadoDebitoHSBC(new Filial(base.IdFilial.Value)))
                    {
                        DesabilitarTodosOsPagamentos = 1;
                        ExibirMensagem("Existe um Pagamento Sendo Processado! Aguarde a Confirmação do Pagamento!", TipoMensagem.Aviso, true);
                    }
                    else
                        DesabilitarTodosOsPagamentos = 0;
                }
                else
                {
                    if (base.IdPessoaFisicaLogada.HasValue)
                    {
                        if (PlanoAdquirido.ExistePlanoAdquridoEnviadoOuNaoEnviadoDebitoHSBC(new PessoaFisica(base.IdPessoaFisicaLogada.Value)))
                        {
                            DesabilitarTodosOsPagamentos = 1;
                            ExibirMensagem("Existe um Pagamento Sendo Processado! Aguarde a Confirmação do Pagamento!", TipoMensagem.Aviso, true);
                        }
                        else if (PlanoAdquirido.ExisteVipPlanoLiberadoVencimentoEmXDias(new PessoaFisica(base.IdPessoaFisicaLogada.Value), 5))
                        {
                            DesabilitarTodosOsPagamentos = 1;
                            ExibirMensagem("Você Possui um Plano Liberado!", TipoMensagem.Aviso, true);
                        }
                        else
                            DesabilitarTodosOsPagamentos = 0;
                    }
                }
            }
            else
            {

                if (!base.PagamentoIdentificadorPlano.HasValue)
                    Redirect(GetRouteUrl(Enumeradores.RouteCollection.LoginComercialCandidato.ToString(), null));
            }
        }

        #endregion

        #region Pagamentos

        #region ValidaFormaDePagamentoAssinatura
        private bool ValidaFormaDePagamentoAssinatura(int idPlanoAdquirido, int tipoFormaDePagamento)
        {
            PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
            objPlanoAdquirido.Plano.CompleteObject();

            if (objPlanoAdquirido.Plano.FlagRecorrente && (tipoFormaDePagamento == (int)BNE.BLL.Enumeradores.TipoPagamento.CartaoCredito || tipoFormaDePagamento == (int)BNE.BLL.Enumeradores.TipoPagamento.DebitoRecorrente || tipoFormaDePagamento == (int)BLL.Enumeradores.TipoPagamento.BoletoBancario))
                return true;
            else if (!objPlanoAdquirido.Plano.FlagRecorrente)
                return true;
            else
                return false;
        }
        #endregion


        #region Pagamento - PayPal
        private void PagamentoPayPal()
        {

            if (!ValidaFormaDePagamentoAssinatura(base.PagamentoIdentificadorPlanoAdquirido.Value, (int)Enumeradores.TipoPagamento.PayPal))
            {
                base.ExibirMensagem("Não é uma forma de Pagamento Válida!", TipoMensagem.Aviso);
                return;
            }


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

            if (objPlanoAdquirido.QtdParcela.HasValue && objPlanoAdquirido.QtdParcela > 1 &&
                (int)Enumeradores.TipoPagamento.CartaoCredito == idTipoPagamento && objPlanoAdquirido.FlagRecorrente)
            {
                objPlanoAdquirido.DataFimPlano = objPlanoAdquirido.DataInicioPlano.AddMonths(1);
                objPlanoAdquirido.Save();
            }

            objPlanoAdquirido.Plano.CompleteObject();
            objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();

            if (objPlanoAdquirido.Plano.EhPlanoParaImpulsionarVaga())
            {
                int idVaga = 0;

                if (!string.IsNullOrEmpty(Request.QueryString["Idf_Vaga"]))
                    idVaga = Convert.ToInt32(Request.QueryString["Idf_Vaga"]);

                PlanoAdquiridoDetalhes.CriarPladoAdDetalhesPlanoSmsVaga(objPlanoAdquirido, idVaga);
            }

            base.PagamentoFormaPagamento.Value = idTipoPagamento;

            CodigoDesconto objCodigoDesconto = null;
            if (base.PagamentoIdCodigoDesconto.HasValue)
                objCodigoDesconto = new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);

            if (objPlanoAdquirido.ParaPessoaFisica())
            {
                TipoPessoaFisica = (int)Enumeradores.PlanoTipo.PessoaFisica;

                if (!PlanoParcela.ExisteParcelaCriada(objPlanoAdquirido))
                    objPlanoAdquirido.CriarParcelas(new TipoPagamento(base.PagamentoFormaPagamento.Value), objCodigoDesconto, objPlanoAdquirido.QuantidadePrazoBoleto, objPlanoAdquirido.QtdParcela);
                else
                    objPlanoAdquirido.AjustarParcelas(new TipoPagamento(base.PagamentoFormaPagamento.Value), objCodigoDesconto, objPlanoAdquirido.QuantidadePrazoBoleto);
            }
            else
            {
                objPlanoAdquirido.Filial.CompleteObject();
                TipoPessoaFisica = (int)Enumeradores.PlanoTipo.PessoaJuridica;
                //Notificar o comercial da tentativa de compra de um plano
                NotificarVendaPlanoEmpresa(idTipoPagamento, objPlanoAdquirido.ValorBase);

                if (!PlanoParcela.ExisteParcelaCriada(objPlanoAdquirido))
                    objPlanoAdquirido.CriarParcelas(new TipoPagamento(base.PagamentoFormaPagamento.Value), objCodigoDesconto, objPlanoAdquirido.QuantidadePrazoBoleto, objPlanoAdquirido.QtdParcela);
                else
                    objPlanoAdquirido.AjustarParcelas(new TipoPagamento(base.PagamentoFormaPagamento.Value), objCodigoDesconto, objPlanoAdquirido.QuantidadePrazoBoleto);
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
            else if (objPlano.IdPlano == Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoRelatorioSalarialEmpresasIdentificador))) //Compra de plano do relatório do salario BR redireciona para a tela do relatório
            {
                //Resgatar a função para exibição do relatório
                var idFuncao = Request.QueryString["idFuncao"].ToString();
                string siglaEstado = Request.QueryString["SiglaEstado"].ToString();
                Redirect("/RelatorioSalarioBR.aspx?idFuncao=" + idFuncao + "&SiglaEstado=" + siglaEstado + "&IdPlanoAdquirido=" + IdPlanoAdquirido);
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

        #region Pagamento - Debito Online

        #region BRADESCO

        #region Debito Online - Pagamento via Bradesco
        private bool PagamentoDebitoOnlineViaBradesco(Pagamento objPagamento, PlanoAdquirido objPlanoAdquirido, out string msgErro)
        {
            Transacao objTransacao = Transacao.ValidarPagamentoDebitoOnline(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), Enumeradores.Banco.BRADESCO, out msgErro);
            objTransacao.PlanoAdquirido = objPlanoAdquirido;

            if (objTransacao != null)
                return EnviaPostBradesco(objTransacao, objPagamento);
            else
            {
                msgErro = "Ocorreu um erro ao tentar validar sua compra, tente novamente!";
                return false;
            }
        }

        #endregion

        #region Debito Online - POST - Bradesco
        private bool EnviaPostBradesco(Transacao objTransacao, Pagamento objPagamento)
        {
            //Criação do POST de Envio para o BB
            string numConvenio = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.ConvenioDebitoBradesco);
            string dadosDaRequisicao = string.Format(Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLDebitoOnlineBradesco) + "&descritivo={3}&quantidade=1&unidade=UN&valor={4}"
                    , numConvenio
                    , numConvenio
                    , objTransacao.IdTransacao
                    , objTransacao.PlanoAdquirido.Plano.CompleteObject() ? objTransacao.PlanoAdquirido.Plano.DescricaoPlano : ""
                    , Convert.ToString(objPagamento.ValorPagamento).Replace(",", "").TrimStart(new Char[] { '0' })
                    );

            Redirect(dadosDaRequisicao);
            return true;

        }
        #endregion

        #endregion

        #region BANCO DO BRASIL

        #region Debito Online - Pagamento via Branco do Brasil
        private bool PagamentoDebitoOnlineViaBancoDoBrasil(Pagamento objPagamento, PlanoAdquirido objPlanoAdquirido, out string msgErro)
        {
            //Se existe transações do cliente dentro de um intervalo, não o deixe criar uma nova
            if (Transacao.ExisteTransacaoDebitoOnlineNoIntervalo(base.IdPessoaFisicaLogada.Value, Parametro.RecuperaValorParametro(Enumeradores.Parametro.IntervaloTempoSondaDebitoOnlineBB)))
            {
                msgErro = "Existe um pagamento a ser processado, favor aguardar!";
                return false;
            }

            Transacao objTransacao = Transacao.ValidarPagamentoDebitoOnline(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), Enumeradores.Banco.BANCODOBRASIL, out msgErro);

            if (objTransacao != null)
                return EnviaPostBancoDoBrasil(objTransacao, objPagamento);
            else
            {
                msgErro = "Ocorreu um erro ao tentar validar sua compra, tente novamente!";
                return false;
            }
        }
        #endregion

        #region Debito Online - POST - Banco do Brasil
        private bool EnviaPostBancoDoBrasil(Transacao objTransacao, Pagamento objPagamento)
        {
            //Criação do POST de Envio para o BB
            string dadosDaRequisicao = string.Format("idConv={0}&refTran={1}&valor={2}&dtVenc={3}&tpPagamento={4}&urlRetorno={5}&urlInforma={6}",
                                        Parametro.RecuperaValorParametro(Enumeradores.Parametro.ConvenioDebitoBB),
                                        Convert.ToString(objTransacao.IdTransacao).PadLeft(17, '0'),
                                        Convert.ToString(objPagamento.ValorPagamento).Replace(",", "").TrimStart(new Char[] { '0' }),
                                        objPagamento.DataVencimento.Value.ToString("ddMMyyyy"),
                                        Parametro.RecuperaValorParametro(Enumeradores.Parametro.DebitoEmContaViaInternetPFePJ),
                                        Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLRetornoDebitoOnline),
                                        Parametro.RecuperaValorParametro(Enumeradores.Parametro.URLInformaDebitoOnline)
                                        );

            Redirect("PagamentoDebitoOnlineBB.aspx?" + dadosDaRequisicao);

            return true;
        }
        #endregion

        #endregion

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
                if (!base.IdUsuarioFilialPerfilLogadoCandidato.HasValue && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                {
                    base.ExibirLogin();
                    return;
                }

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
                    Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamentoErro.ToString(), null));
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
            string nomeCartaoCredito = txtNomeCartao.Valor;

            string resultadoTransacaoResposta = string.Empty;

            Enumeradores.GerenciadoraTransacao gerenciadoraCartao;
            gerenciadoraCartao = UtilizaCielo == "True" ? Enumeradores.GerenciadoraTransacao.Cielo : Enumeradores.GerenciadoraTransacao.PagarMe;

            var retorno = Transacao.ValidarPagamentoCartaoCredito(ref objPagamento,
             IdPlanoAdquirido, PageHelper.RecuperarIP(), numeroCartao,
             mesValidadeCartao, anoValidadeCartao, numeroDigitoVerificador, nomeCartaoCredito, null, gerenciadoraCartao, out erro, out resultadoTransacaoResposta);

            if (!retorno && base.IdCurriculo.HasValue)
            {
                CurriculoObservacao.SalvarCRM("Pagamento Cartão do plano: " + objPlanoAdquirido.IdPlanoAdquirido +
                     " não realizado, motivo: " + erro + " - " + resultadoTransacaoResposta, new Curriculo(base.IdCurriculo.Value),
                     "ControleParcelas -> Renovacao Recorrencia");
            }


            return retorno;
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
            decimal? cpf = null;
            decimal? cnpj = null;

            switch (banco)
            {
                case Enumeradores.Banco.HSBC:
                    agencia = txtAgenciaDebitoHSBC.Text;
                    conta = txtContaCorrenteDebitoHSBC.Text + txtDigitoDebitoHSBC.Text;
                    cpf = string.IsNullOrEmpty(txtCPFDebitoHSBC.Valor) ? cpf : Convert.ToDecimal(txtCPFDebitoHSBC.Valor);
                    cnpj = string.IsNullOrEmpty(txtCNPJDebitoHSBC.Valor) ? cpf : Convert.ToDecimal(txtCNPJDebitoHSBC.Valor);
                    break;
            }

            return Transacao.ValidarPagamentoDebito(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), banco, agencia, conta, cpf, cnpj, out erro);
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

            #region Validações dos Bancos

            bool sucesso = true;
            msgErro = string.Empty;

            switch (BancoDebitoOnline)
            {
                case Enumeradores.Banco.BANCODOBRASIL:
                    sucesso = PagamentoDebitoOnlineViaBancoDoBrasil(objPagamento, objPlanoAdquirido, out msgErro);
                    break;
                case Enumeradores.Banco.BRADESCO:
                    sucesso = PagamentoDebitoOnlineViaBradesco(objPagamento, objPlanoAdquirido, out msgErro);
                    break;
            }
            return sucesso;

            #endregion
        }
        #endregion

        #region Validar - Ja se candidatou a vaga
        private bool CandidatouVaga(PlanoAdquirido objPlanoAd)
        {

            objPlanoAd.UsuarioFilialPerfil.CompleteObject();
            PlanoAdquiridoDetalhes objAdquiridoDetalhe = new PlanoAdquiridoDetalhes();
            PlanoAdquiridoDetalhes.CarregarPorPlanoAdquirido(objPlanoAd, out objAdquiridoDetalhe);
            Curriculo objCurriculo = new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(objPlanoAd.UsuarioFilialPerfil.PessoaFisica));

            if (VagaCandidato.CurriculoJaCandidatouVaga(objCurriculo, objAdquiridoDetalhe.Vaga))
            {
                lblPlano.Text = " <b style='color:red;'>ATENÇÃO:</b> você já se candidatou a esta vaga.";
                return true;
            }
            return false;
        }

        #endregion

        #endregion Validações

        #region Outros

        #region NotificarVendaPlanoEmpresa
        /// <summary>
        /// Fluxo acionado quando um vendedor realiza uma venda de plano abaixo do mínimo configurado para aquele plano, respeitando a coluna Vlr_Desconto_Maximo
        /// </summary>
        private void NotificarVendaPlanoEmpresa(int idTipoPagamento, decimal valorBase)
        {
            string descricao = string.Empty, nomeVendedor = string.Empty;
            BLL.CodigoDesconto objCodigoDesconto = null;

            var objFilial = Filial.LoadObject(base.IdFilial.Value);
            var objPlano = new Plano(base.PagamentoIdentificadorPlano.Value);
            var objTipoPagamento = new TipoPagamento(idTipoPagamento);
            var vendedor = objFilial.Vendedor();

            objFilial.Endereco.CompleteObject();
            objFilial.Endereco.Cidade.CompleteObject();
            objTipoPagamento.CompleteObject();



            if (base.PagamentoIdCodigoDesconto.HasValue)
            {
                BLL.CodigoDesconto.CarregarPorCodigo(CodigoDesconto, out objCodigoDesconto);

                if (objCodigoDesconto.UsuarioFilialPerfil != null)
                {
                    objCodigoDesconto.UsuarioFilialPerfil.CompleteObject();
                    nomeVendedor = objCodigoDesconto.UsuarioFilialPerfil.PessoaFisica.NomeCompleto;
                }
            }

            //ENVIO DE EMAIL
            var valoresDestinatarios = Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailVendaCiaAbaixoMinimo);
            var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);

            if (!string.IsNullOrWhiteSpace(valoresDestinatarios))
            {
                string assunto;
                var carta = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.VendaPlanoVendedorAbaixoMinimo, out assunto);

                var parametros = new
                {
                    Vendedor = string.IsNullOrEmpty(nomeVendedor) ? "Venda feita pelo SITE" : nomeVendedor,
                    DescricaoPlano = objPlano.DescricaoPlano,
                    CodigoPlano = objPlano.IdPlano,
                    NomeEmpresa = string.Format("{0} - {1}", objFilial.CNPJ, objFilial.NomeFantasia),
                    ValorPago = valorBase,
                    Justificativa = string.IsNullOrEmpty(JustificativaVendedor) ? "Não houve justificativa" : JustificativaVendedor,
                    ValorBase = objPlano.RecuperarValor(),
                    Cidade = UIHelper.FormatarCidade(objFilial.Endereco.Cidade.NomeCidade, objFilial.Endereco.Cidade.Estado.SiglaEstado),
                    FormaPagamento = objTipoPagamento.DescricaoTipoPagamaneto,
                    CarteiraCliente = vendedor.NomeVendedor
                };

                //CASO A VENDA SEJA POR VENDEDOR ENVIAR INFORMAÇÃO PELO CRM
                if (objCodigoDesconto != null && objCodigoDesconto.UsuarioFilialPerfil != null)
                {
                    if (!string.IsNullOrEmpty(JustificativaVendedor))
                        descricao =
                            parametros.ToString(
                                "O vendedor {Vendedor} solicitou a venda do plano {DescricaoPlano} (Código - {CodigoPlano}) pelo valor de R$ {ValorPago} e sua justificativa foi {Justificativa}.");
                    else
                        descricao =
                            parametros.ToString(
                                "O vendedor {Vendedor} solicitou a venda do plano {DescricaoPlano} (Código - {CodigoPlano}) pelo valor de R$ {ValorPago}.");

                    FilialObservacao.SalvarCRM(descricao, objFilial,
                        new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value));
                }

                if (!string.IsNullOrWhiteSpace(valoresDestinatarios))
                {
                    //Quando o vendedor vende abaixo do mínimo para o plano escolhido (Vlr_Plano_Base), é enviado um e-mail para informar o financeiro
                    assunto = string.Concat("Aviso de venda:  ", parametros.Vendedor);
                    //Ajustando conteúdo
                    carta = parametros.ToString(carta);

                    MensagemCS.EnvioDeEmailComValidacao(TipoEnviadorEmail.Fila, assunto, carta, Enumeradores.CartaEmail.VendaPlanoVendedorAbaixoMinimo, emailRemetente, valoresDestinatarios);
                }

            }
            LimparSessionJustificativa();
        }
        #endregion NotificarVendaPlanoEmpresa

        private void TratarVisibilidadeFormasPagamento(bool flgRecorrente)
        {
            try
            {
                HabilitarApenasCartaoCredito = 0;
                HabilitarFormasPagamentoPlanosRecorrentes = 0;

                //Validação para mostrar apenas cartão de credito quando o plano é de pesquisa salarial do salario br para empresas
                if (base.PagamentoIdentificadorPlano.Value == Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoRelatorioSalarialEmpresasIdentificador))
                    || base.PagamentoIdentificadorPlano.Value == Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoCandidaturaPremium)))
                    HabilitarApenasCartaoCredito = 1;
                else
                {

                }
                if (flgRecorrente) //Verificar se Plano selecionado é do tipo Recorrente, habilitar apenas Cartões de Crédito e Debito Hsbc
                    HabilitarFormasPagamentoPlanosRecorrentes = 1;


            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

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

                    if (objPlanoParcela != null)
                    {
                        var objListPagamentosPorParcela = BLL.Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela);

                        if (objListPagamentosPorParcela != null && objListPagamentosPorParcela.Count > 0)
                        {
                            if (base.PagamentoFormaPagamento.HasValue)
                            {
                                objPagamento = objListPagamentosPorParcela.FirstOrDefault(p => (p.TipoPagamento == null || p.TipoPagamento.IdTipoPagamento == base.PagamentoFormaPagamento.Value) && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false);
                                if (objPagamento != null)
                                {
                                    base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                                }
                            }
                            IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                        }
                    }
                }
                else if (objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica))
                {
                    PlanoParcela objPlanoParcela = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquirido(objPlanoAdquirido);
                    if (objPlanoParcela != null)
                    {
                        var objListPagamentosPorParcela = BLL.Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela);

                        if (objListPagamentosPorParcela != null && objListPagamentosPorParcela.Count > 0)
                        {
                            // Se existir algum pagamento com o tipo==null e ativo entao popula o objPagamento
                            if (base.PagamentoFormaPagamento.HasValue)
                            {
                                objPagamento = objListPagamentosPorParcela.OrderBy(p => p.IdPagamento).FirstOrDefault(p => p.TipoPagamento == null || p.TipoPagamento.IdTipoPagamento == base.PagamentoFormaPagamento.Value && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false);
                                if (objPagamento != null)
                                {
                                    base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                                }
                            }
                            IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                        }
                    }
                }
            }
            else if (base.PagamentoAdicionalValorTotal.HasValue && base.PagamentoAdicionalQuantidade.HasValue)
            {
                PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(new Filial(base.IdFilial.Value), (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido);

                if (objPlanoAdquirido != null)
                {
                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

                    if (base.PagamentoFormaPagamento.HasValue)
                    {
                        AdicionalPlano.CriarPagamentoEPlanoAdicionalSMS(objPlanoAdquirido, base.PagamentoAdicionalValorTotal.Value, base.PagamentoAdicionalQuantidade.Value, objUsuarioFilialPerfil, (Enumeradores.TipoPagamento)base.PagamentoFormaPagamento.Value, DateTime.Now, DateTime.Today, out objPagamento);
                        if (objPagamento != null)
                        {
                            base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                        }
                    }
                    IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
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

        #region Limpar Sessao - Codigo de Desconto
        private void LimparSessionJustificativa()
        {
            base.PagamentoJustificativaAbaixoMinimo.Clear();
        }
        #endregion





        protected void radbtnCPF_OR_CNPJ_CheckedChanged(object sender, EventArgs e)
        {
            if (radbtnCPF.Checked)
            {
                txtCPFDebitoHSBC.Visible = true;
                txtCNPJDebitoHSBC.Visible = false;
                lblTextCPFouCNPJ.Text = "CPF do titular:";
                lblTextCPFouCNPJ.CssClass = "text_g";
            }
            else
            {
                txtCNPJDebitoHSBC.Visible = true;
                txtCPFDebitoHSBC.Visible = false;
                lblTextCPFouCNPJ.Text = "CNPJ do titular:";
                lblTextCPFouCNPJ.CssClass = "text_g2";
            }
            UpdatePanel1.Update();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "inicializa_debito_HSBC", "inicializa_debito_HSBC()", true);

        }

        #endregion Outros

        #endregion Métodos

    }
}