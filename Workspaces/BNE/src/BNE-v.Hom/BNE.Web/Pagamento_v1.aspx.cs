using System;
using System.Threading;
using System.Web.UI;
using System.Linq;
using System.Collections.Generic;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Globalization;

namespace BNE.Web
{
    public partial class Pagamento : BasePagePagamento
    {
        #region Propriedades

        public string IdPlano;
        public string NomePlano;
        public string TipoPlano;
        public string VlrPlano;
        public string NmeCidade;
        public string NmeEstado;
        public string SiglaPais;


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
                return Convert.ToInt32(ViewState[Chave.Temporaria.Variavel6.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
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
                bool mostrarOpcaoBoleto = true;

                if (base.PagamentoIdentificadorPlano.HasValue)
                {
                    // carrega plano
                    var objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);
                    UsuarioFilialPerfil objUsuarioFilialPerfil = null;
                    
                    if (objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
                    {
                        TipoPessoaFisica = (int)Enumeradores.PlanoTipo.PessoaFisica;
                        objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoCandidato.Value);
                        var objCurriculo = new Curriculo(base.IdCurriculo.Value);

                        if (!PlanoAdquirido.ExistePlanoAdquiridoAguardandoLiberacao(objCurriculo, objPlano))
                            PlanoAdquirido.CriarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, null, objPlano, false, base.PrazoBoleto.Value);
                        else
                            PlanoAdquirido.AjustarPlanoAdquiridoParcela(objUsuarioFilialPerfil, null, objPlano, false,base.PrazoBoleto.Value);

                        pnlCodigoCredito.Visible = true;

                        mostrarOpcaoBoleto = Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.OpcaoPagamentoPessoaFisicaBoleto));
                    }
                    else if (objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica))
                    {
                        TipoPessoaFisica = (int)Enumeradores.PlanoTipo.PessoaJuridica;
                        objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                        var objFilial = new Filial(base.IdFilial.Value);

                        if (!PlanoAdquirido.ExistePlanoAdquiridoAguardandoLiberacao(objFilial, objPlano))
                            PlanoAdquirido.CriarPlanoAdquiridoParcelaPagamento(objUsuarioFilialPerfil, objFilial, objPlano, false, base.PrazoBoleto.Value);
                        else
                            PlanoAdquirido.AjustarPlanoAdquiridoParcela(objUsuarioFilialPerfil, objFilial, objPlano, false, base.PrazoBoleto.Value);

                        mostrarOpcaoBoleto = Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.OpcaoPagamentoPessoaJuridicaBoleto));
                    }

                    /*Definindo valores para o gaTrackingEcommerce*/
                    IdPlano = objPlano.IdPlano.ToString();
                    NomePlano = objPlano.DescricaoPlano;
                    TipoPlano = objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica) ? "VIP" : "CIA";
                    objUsuarioFilialPerfil.CompleteObject();
                    Endereco end;
                    if(Endereco.CarregarPorPessoaFisica(objUsuarioFilialPerfil.PessoaFisica, out end)){
                        if(end.Cidade.NomeCidade == null) 
                            end.Cidade.CompleteObject();
                        NmeCidade = end.Cidade.NomeCidade;

                        if(end.Cidade.Estado.NomeEstado == null) 
                            end.Cidade.Estado.CompleteObject();
                        NmeEstado = end.Cidade.Estado.NomeEstado;

                        SiglaPais = "BR";
                    }

                }

                mostrarOpcaoBoleto = false; //forçando esconder o boleto

                pnlBoleto.Visible = mostrarOpcaoBoleto;

                if (mostrarOpcaoBoleto)
                    litFormaPagamentoInformacao.Text += "Escolha a bandeira do seu <strong>cartão de crédito</strong> (Visa / Master) ou <strong>boleto bancário</strong>";
                else
                {
                    litFormaPagamentoInformacao.Text = "Escolha a bandeira do seu <strong>cartão de crédito</strong> (Visa / Master)";
                    pnlFormaPagamento.CssClass = "btn_cartoes reduzida";
                }

                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    txtCodigoCredito.Text = CodigoDesconto;
                    btnValidarCodigoCredito_Click(sender, e);   // valida novamente codigo de credito
                }

                // calcula o valor a pagar, usando o desconto
                AtualizaValorPagarNaTela();

                if (Session["MensagemErroPagamento"] != null && !String.IsNullOrEmpty(Session["MensagemErroPagamento"].ToString()))
                {
                    ExibirMensagem(Session["MensagemErroPagamento"].ToString(), TipoMensagem.Erro);
                    Session.Remove("MensagemErroPagamento");
                }
            }

            if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "Pagamento");
            else
                InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "Pagamento");

            txtDataDeValidadeAno.ValorMinimo = DateTime.Now.ToString("yy");
        }
        #endregion

        #region rbMaster_CheckedChanged
        protected void rbMaster_CheckedChanged(object sender, EventArgs e)
        {
            PagamentoCartao();
        }
        #endregion

        #region rbVisa_CheckedChanged
        protected void rbVisa_CheckedChanged(object sender, EventArgs e)
        {
            PagamentoCartao();
        }
        #endregion

        #region rbBoleto_CheckedChanged
        protected void rbBoleto_CheckedChanged(object sender, EventArgs e)
        {
            //Session.Add(Chave.Temporaria.IdPlano.ToString(), base.PagamentoIdentificadorPlano.Value);
            base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.BoletoBancario;
            base.PagamentoValorPago.Value = ValorPagamento;
            base.PagamentoIdentificadorPlanoAdquirido.Value = IdPlanoAdquirido;
            if (TipoPessoaFisica.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
            {
                Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamento.ToString(), null));
            }
            Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamentoCIA.ToString(), null));
        }
        #endregion
        /*
        #region rbDebitoOnlineHSBC_CheckedChanged
        protected void rbDebitoOnlineHSBC_CheckedChanged(object sender, EventArgs e)
        {
            CarregarPlanoAdquirido();

            string Script = String.Format("urlBanco = '/PagamentoRedirecionamentoDebito.aspx?idPagamento={0}&idPlanoAdquirido={1}&banco={2}'", base.PagamentoIdentificadorPagamento.Value, IdPlanoAdquirido, (int)Enumeradores.Banco.HSBC);
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "defineUrlBanco", Script, true);
            mpeModalRedirecionamentoDebito.Show();
        }
        #endregion
        */
        #region btiFechar_Click
        protected void btiFechar_Click(object sender, ImageClickEventArgs e)
        {
            mpeModalRedirecionamentoDebito.Hide();
            /*            rbDebitoOnlineHSBC.Checked = false;*/
            ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "EsconderTooltip", "Inicializar();", true);
            upFormaPagamento.Update();
        }
        #endregion

        #region txtCodigoCredito_TextChanged
        protected void txtCodigoCredito_TextChanged(object sender, EventArgs e)
        {
            ValidarCodigoDesconto();
        }
        #endregion

        #region btnValidarCodigoCredito_Click
        protected void btnValidarCodigoCredito_Click(object sender, EventArgs e)
        {
            ValidarCodigoDesconto();
        }
        #endregion
        /*
        #region btnRedirecionar_Click
        protected void btnRedirecionar_Click(object sender, EventArgs e)
        {
            PagamentoDebito();
        }
        #endregion
        */
        #region btiConcluirCartao_Click
        protected void btiConcluirCartao_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string erro;
                if (ValidarPagamento(out erro))
                {
                    StatusTransacaoCartao = true;

                    base.PagamentoFormaPagamento.Value = (int)Enumeradores.TipoPagamento.CartaoCredito;
                    base.PagamentoValorPago.Value = ValorPagamento;
                    base.PagamentoIdentificadorPlanoAdquirido.Value = IdPlanoAdquirido;
                    if (TipoPessoaFisica.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
                    {
                        Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamento.ToString(), null));
                    }
                    Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.ConfirmacaoPagamentoCIA.ToString(), null));
                }
                else
                {
                    if (string.IsNullOrEmpty(erro))
                        Session.Add("MensagemErroPagamento", "Ocorreu um erro ao tentar validar sua compra, tente novamente!");
                    else
                        Session.Add("MensagemErroPagamento", erro);

                    if (TipoPessoaFisica.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
                    {
                        Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.PagamentoPlano.ToString(), null));
                    }
                    Redirect(GetRouteUrl(BNE.BLL.Enumeradores.RouteCollection.PagamentoPlanoCIA.ToString(), null));
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

        #endregion

        #region Métodos

        #region ValidarCodigoDesconto
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
                        base.IdUsuarioFilialPerfilLogadoCandidato.Value,
                        base.PagamentoIdentificadorPlano.Value,
                        base.PagamentoIdCodigoDesconto.Value,
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

        #region AtualizaValorPagarNaTela
        protected void AtualizaValorPagarNaTela()
        {
            decimal valorPagamento = Decimal.Zero;
            if (base.PagamentoIdentificadorPlano.HasValue)
            {
                Plano objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);

                PlanoAdquirido objPlanoAdquirido;

                if (objPlano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                    PlanoAdquirido.CarregarPlanoAdquiridoAguardandoLiberacao(new Curriculo(base.IdCurriculo.Value), objPlano, out objPlanoAdquirido);
                else if (objPlano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaJuridica)
                    PlanoAdquirido.CarregarPlanoAdquiridoAguardandoLiberacao(new Filial(base.IdFilial.Value), objPlano, out objPlanoAdquirido);
                else
                    throw new InvalidOperationException("Tipo de plano desconhecido, IdPlanoTipo: " + objPlano.PlanoTipo.IdPlanoTipo);

                IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;

                // calcula o valor a pagar, usando o desconto
                valorPagamento = objPlanoAdquirido.RecuperarValor();
                if (base.PagamentoIdCodigoDesconto.HasValue)
                {
                    BLL.CodigoDesconto objCodigoDesconto =
                        new BLL.CodigoDesconto(base.PagamentoIdCodigoDesconto.Value);
                    objCodigoDesconto.CalcularDesconto(ref valorPagamento);
                }
            }
            else if (base.PagamentoAdicionalValorTotal.HasValue && base.PagamentoAdicionalQuantidade.HasValue) //Plano adicional
            {
                valorPagamento = base.PagamentoAdicionalValorTotal.Value;
            }

            litValorPagamento.Text = valorPagamento.ToString("0.00", CultureInfo.CurrentCulture);
            
            //Atualizando valor do TrackingEcommece
            VlrPlano = valorPagamento.ToString("F2", CultureInfo.GetCultureInfo("en-US"));
        }
        #endregion

        #region LimparSessionDesconto
        private void LimparSessionDesconto()
        {
            base.PagamentoIdCodigoDesconto.Clear();
        }
        #endregion

        #region CarregarPlanoAdquirido
        private void CarregarPlanoAdquirido()
        {
            if (base.PagamentoIdentificadorPlano.HasValue)
            {
                var objPlano = Plano.LoadObject(base.PagamentoIdentificadorPlano.Value);

                if (objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica))
                {
                    PlanoAdquirido objPlanoAdquirido;
                    PlanoAdquirido.CarregarPlanoAdquiridoAguardandoLiberacao(new Curriculo(base.IdCurriculo.Value), objPlano, out objPlanoAdquirido);

                    UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoCandidato.Value);

                    PlanoParcela objPlanoParcela = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquirido(objPlanoAdquirido);
                    var objListPagamentosPorParcela = BLL.Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela);

                    if (objListPagamentosPorParcela != null && objListPagamentosPorParcela.Count > 0)
                    {
                        BLL.Pagamento objPagamento = objListPagamentosPorParcela.FirstOrDefault(p => (p.TipoPagamento == null || p.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.CartaoCredito) && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false);

                        // Caso não exista entao cria um novo.
                        if (objPagamento == null)
                            objPagamento = PlanoAdquirido.CriarPagamento(objPlanoParcela, objPlano, null, objUsuarioFilialPerfil, new TipoPagamento((int)Enumeradores.TipoPagamento.CartaoCredito));

                        PlanoAdquirido.AtualizarPagamento(objPagamento, new TipoPagamento((int)Enumeradores.TipoPagamento.CartaoCredito), objPlanoAdquirido, objPlano);

                        IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                        base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                    }
                }
                else if (objPlano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaJuridica))
                {
                    PlanoAdquirido objPlanoAdquirido;
                    PlanoAdquirido.CarregarPlanoAdquiridoAguardandoLiberacao(new Filial(base.IdFilial.Value), objPlano, out objPlanoAdquirido);

                    UsuarioFilialPerfil objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

                    PlanoParcela objPlanoParcela = PlanoParcela.CarregaParcelaAtualEmAbertoPorPlanoAdquirido(objPlanoAdquirido);
                    var objListPagamentosPorParcela = BLL.Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela);

                    if (objListPagamentosPorParcela != null && objListPagamentosPorParcela.Count > 0)
                    {
                        // Se existir algum pagamento com o tipo==null e ativo entao popula o objPagamento
                        BLL.Pagamento objPagamento = objListPagamentosPorParcela.OrderBy(p => p.IdPagamento).FirstOrDefault(p => p.TipoPagamento == null || p.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.CartaoCredito && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto && p.FlagInativo == false);

                        // Atualiza pagamentos selecionados com o tipo == null, caso não exista entao cria um novo.
                        if (objPagamento == null)
                            objPagamento = PlanoAdquirido.CriarPagamento(objPlanoParcela, objPlano, objUsuarioFilialPerfil.Filial, objUsuarioFilialPerfil, new TipoPagamento((int)Enumeradores.TipoPagamento.CartaoCredito));

                        PlanoAdquirido.AtualizarPagamento(objPagamento, new TipoPagamento((int)Enumeradores.TipoPagamento.CartaoCredito), objPlanoAdquirido, objPlano);

                        IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                        base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                        //base.UrlRetornoPagamento.Value = urlRetorno;
                    }
                }
            }
            else if (base.PagamentoAdicionalValorTotal.HasValue && base.PagamentoAdicionalQuantidade.HasValue)
            {
                PlanoAdquirido objPlanoAdquirido;
                PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(new Filial(base.IdFilial.Value), (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido);

                if (objPlanoAdquirido != null)
                {
                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

                    BLL.Pagamento objPagamento;
                    AdicionalPlano.CriarPagamentoEPlanoAdicionalSMS(objPlanoAdquirido, base.PagamentoAdicionalValorTotal.Value, base.PagamentoAdicionalQuantidade.Value, objUsuarioFilialPerfil, Enumeradores.TipoPagamento.CartaoCredito, DateTime.Now, DateTime.Today, out objPagamento);

                    IdPlanoAdquirido = objPlanoAdquirido.IdPlanoAdquirido;
                    base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                }
            }
        }
        #endregion

        #region PagamentoCartao
        private void PagamentoCartao()
        {
            CarregarPlanoAdquirido();

            pnlCartao.Visible = true;
        }
        #endregion

        /*
        #region PagamentoDebito
        private void PagamentoDebito()
        {
            
            String erro = string.Empty;
            if (rbDebitoOnlineHSBC.Checked)
            {
                //String codigoPedidoGateway = Transacao.CriarTransacaoDebitoOnline(objPagamento, IdPlanoAdquirido, Enumeradores.Banco.HSBC, PageHelper.RecuperarIP(), out erro);
                ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "popUpRedirecionamento", 
                        @"newwindow=window.open('http://localhost:2000/teste.aspx','name','height=1024,width=768');
	                      if (window.focus) {newwindow.focus()}", true);
            }
            
            
        }
        #endregion
*/
        #region ValidarPagamento
        private bool ValidarPagamento(out string erro)
        {
            var objPagamento = BLL.Pagamento.LoadObject(base.PagamentoIdentificadorPagamento.Value);

            string numeroCartao = string.Concat(txtNumeroDoCartaoParte1.Valor,
                txtNumeroDoCartaoParte2.Valor,
                txtNumeroDoCartaoParte3.Valor,
                txtNumeroDoCartaoParte4.Valor);
            int mesValidadeCartao = Convert.ToInt32(txtDataDeValidadeMes.Valor);
            int anoValidadeCartao = Convert.ToInt32(txtDataDeValidadeAno.Valor);
            int numeroDigitoVerificador = Convert.ToInt32(txtDigitoVerificador.Valor);

            int bandeiraCartao = 0;
            if (rbVisa.Checked)
                bandeiraCartao = (int)Enumeradores.Operadora.Visa;

            if (rbMaster.Checked)
                bandeiraCartao = (int)Enumeradores.Operadora.Master;

            if (bandeiraCartao.Equals(0))
                erro = "Selecione a bandeira!";
            else
            {
                txtNumeroDoCartaoParte1.Valor = string.Empty;
                txtNumeroDoCartaoParte2.Valor = string.Empty;
                txtNumeroDoCartaoParte3.Valor = string.Empty;
                txtNumeroDoCartaoParte4.Valor = string.Empty;
                txtDataDeValidadeMes.Valor = string.Empty;
                txtDataDeValidadeAno.Valor = string.Empty;
                txtDigitoVerificador.Valor = string.Empty;

                upCartaoDeCredito.Update();

                // verifica e concede desconto
                if (base.PagamentoIdCodigoDesconto.HasValue)
                    objPagamento.ConcederDesconto(new CodigoDesconto(base.PagamentoIdCodigoDesconto.Value));

                return Transacao.ValidarPagamentoCartaoCredito(ref objPagamento,
                    IdPlanoAdquirido, PageHelper.RecuperarIP(), numeroCartao,
                    mesValidadeCartao, anoValidadeCartao, numeroDigitoVerificador, out erro);
            }

            return false;
        }
        #endregion

        #region DeslogarUsuario
        private void DeslogarUsuario()
        {
            if (base.IdPessoaFisicaLogada.HasValue)
                new PessoaFisica(IdPessoaFisicaLogada.Value).ZerarDataInteracaoUsuario();

            base.LimparSession();
        }
        #endregion

        #endregion
    }
}