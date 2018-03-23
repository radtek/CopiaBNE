using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web.UI;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.BLL.Custom;
using BNE.Web.Master;
using System.Web.Services;
using BNE.BLL.Integracoes.Pagamento;
using System.Configuration;

namespace BNE.Web.Payment
{
    public partial class PaymentMobileFluxoVip : BasePagePagamento
    {
        public static readonly string UtilizaCielo = ConfigurationManager.AppSettings["UtilizaCielo"];
        #region Propriedades

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


        public Boolean PrimeiraGratis
        {
            get
            {
                return Convert.ToBoolean(ViewState[Chave.Permanente.PrimeiraGratis.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Permanente.PrimeiraGratis.ToString(), value);
            }

        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (base.IdPessoaFisicaLogada.HasValue)
                {
                    PessoaFisica objPessoa = PessoaFisica.LoadObject(base.IdPessoaFisicaLogada.Value);
                    litNomeClienteTopo.Text = litNomeCliente.Text = objPessoa.PrimeiroNome;

                    if (PlanoAdquirido.ExisteVipPlanoLiberado(base.IdPessoaFisicaLogada.Value))
                    {
                        Redirect(GetRouteUrl(Enumeradores.RouteCollection.SalaVIP.ToString(), null));
                        return;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["Identificador"]) && !string.IsNullOrEmpty(Request.QueryString["Data"]))
                    {

                        Curriculo objCurriculo = Curriculo.LoadObject(Convert.ToInt32(Request.QueryString["Identificador"]));
                        if (objCurriculo != null)
                        {
                            int idPessoaFisica = PessoaFisica.RecuperarIdPorCurriculo(objCurriculo);
                            PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(idPessoaFisica);
                            if (objPessoaFisica != null && objPessoaFisica.DataNascimento == Convert.ToDateTime(Request.QueryString["Data"].ToString()))
                            {
                                BNE.Auth.BNEAutenticacao.LogarCandidato(objPessoaFisica.NomePessoa, objPessoaFisica.IdPessoaFisica, objPessoaFisica.CPF, objPessoaFisica.DataNascimento, objCurriculo.IdCurriculo);
                                if (BNE.Auth.BNEAutenticacao.User() != null)
                                    Response.Redirect(Request.RawUrl);
                            }
                        }

                    }

                }
                ValidarCriacaoDoPlano();
                PrimeiraGratis = Request.QueryString["primeira"] != null;
                div_finalizar_Cartao1.Visible = !PrimeiraGratis;
                div_finalizar_Cartao2.Visible = PrimeiraGratis;
                primeiraGratis.Visible = PrimeiraGratis;
            }

            Validacoes();

            updTextAcesso.Update();
            updTextAcessoTopo.Update();
        }

        #region Validar - Criacao Do Plano
        private void ValidarCriacaoDoPlano()
        {
            Plano objPlano = null;
            PlanoAdquirido objPlanoAdquirido = null;

            if (!base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.EscolhaPlano.ToString(), null));
            else if (!base.PagamentoIdentificadorPlano.HasValue && string.IsNullOrEmpty(Request.QueryString["IdPlano"]))
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.EscolhaPlano.ToString(), null));

            //Tratamento especial para compra de plano do relatório do salario BR - plano adquirido é criado aqui
            objPlano = Plano.LoadObject(string.IsNullOrEmpty(Request.QueryString["IdPlano"]) ? base.PagamentoIdentificadorPlano.Value : Convert.ToInt32(Request.QueryString["IdPlano"].ToString()));
            if (objPlano == null) Redirect(GetRouteUrl(Enumeradores.RouteCollection.EscolhaPlano.ToString(), null));


            //Caso Pessoa Jurídica redireciona
            if (objPlano.ParaPessoaJuridica())
                Redirect(GetRouteUrl(Enumeradores.RouteCollection.EscolhaPlano.ToString(), null));

            objPlanoAdquirido = PlanoAdquirido.CriarPlanoAdquiridoPF(UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoCandidato.Value), objPlano, 1);

            if (objPlanoAdquirido != null)
            {
                //Grava os Objetos Na Sessao e nas Variaveis Hidden
                base.PagamentoIdentificadorPlano.Value = objPlano.IdPlano;
                base.PagamentoIdentificadorPlanoAdquirido.Value = objPlanoAdquirido.IdPlanoAdquirido;
                txtPlanoAdquirido.Value = objPlanoAdquirido.IdPlanoAdquirido.ToString();
                txtPessoaFisica.Value = base.IdPessoaFisicaLogada.Value.ToString();


                if (objPlano.PlanoTipo.IdPlanoTipo == (int)Enumeradores.PlanoTipo.PessoaFisica)
                {
                    string valor = (base.ValorBasePlano.HasValue == true ? base.ValorBasePlano.Value.ToString("C") : objPlano.ValorBase.ToString("C"));
                    if (objPlano.FlagRecorrente)
                    {
                        if (objPlano.IdPlano == 620)
                        {
                            //ltValorPlanoTopo.Text = string.Format(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CampanhaRecorrenciaBNE), valor);
                            ltValorPlanoTopo.Text = Request.QueryString["primeira"] == null ? string.Format(
                                    Parametro.RecuperaValorParametro(Enumeradores.Parametro.CampanhaRecorrenciaBNE),
                                    valor) : String.Format(Parametro.RecuperaValorParametro(Enumeradores.Parametro.Campanha1Gratis), valor);
                        }
                        else if (objPlano.IdPlano == 644)
                            ltValorPlanoTopo.Text = string.Format(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CampanhaRecorrenciaEstagiario), valor);
                        else if (objPlano.IdPlano == 672)
                            ltValorPlanoTopo.Text = string.Format(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CampanhaBlackFriday), valor);
                        else if (objPlano.IdPlano == 676)
                            ltValorPlanoTopo.Text = string.Format(Parametro.RecuperaValorParametro(Enumeradores.Parametro.SegundoMesFree), valor);
                        hRecorrente.Value = "true";
                        ltValorPlano.Text = (base.ValorBasePlano.HasValue == true ? base.ValorBasePlano.Value.ToString("C") : objPlano.ValorBase.ToString("C"));
                    }
                    else
                        hRecorrente.Value = "false";
                    //else
                    //    ltValorPlanoTopo.Text = string.Format(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CampanhaEmDobroBNE), valor);

                    if (objPlano.QuantidadeDiasValidade == 0)
                        ltNomePlano.Text = string.Empty;
                    else if (objPlano.FlagRecorrente)
                        ltNomePlano.Text = "Assinatura";
                    else
                        ltNomePlano.Text = objPlano.QuantidadeDiasValidade + " dias";
                }
            }
        }
        #endregion

        #region Validacoes
        private void Validacoes()
        {
            bool sucesso = true;
            //Validação se já existe compra
            if (base.PagamentoIdentificadorPlanoAdquirido.HasValue)
            {
                PlanoAdquirido objPlanoAdquiridoValidacao = PlanoAdquirido.LoadObject(base.PagamentoIdentificadorPlanoAdquirido.Value);

                //Trataticas Pessoas Júridicas
                if (objPlanoAdquiridoValidacao.ParaPessoaJuridica())
                {
                    if (PlanoAdquirido.ExistePlanoAdquridoEnviadoOuNaoEnviadoDebitoHSBC(new Filial(base.IdFilial.Value)))
                    {
                        Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileRegistered.aspx"));
                    }
                }
                else
                {
                    if (base.IdPessoaFisicaLogada.HasValue)
                    {
                        if (PlanoAdquirido.ExistePlanoAdquridoEnviadoOuNaoEnviadoDebitoHSBC(new PessoaFisica(base.IdPessoaFisicaLogada.Value)))
                        {
                            Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileRegistered.aspx"));
                        }
                    }
                }
                if (sucesso)
                {
                    if (objPlanoAdquiridoValidacao.FlagRecorrente)
                        TratarVisibilidadeFormasPagamento(true);
                    else
                        HabilitarTodasAsFormasDePagamentos(true);
                }
            }
            else
            {
                if (!base.PagamentoIdentificadorPlano.HasValue)
                {
                    var r = Rota.RecuperarURLRota(Enumeradores.RouteCollection.PagamentoPlano);
                    Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), r));
                }
            }

        }

        #endregion

        #endregion

        #region Evento Click - PayPal
        [WebMethod]
        public static object PagamentoPayPal(int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.PayPal))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        string url = Transacao.CriarTransacaoPayPal(objPagamento, objPlanoAdquirido, idPessoaFisica, PageHelper.RecuperarIP(), UIHelper.GetAbsoluteUrl(string.Format("Confirmacao-de-Pagamento/{0}/{1}", IntermediadorPagamento.PayPal.ToString(), idPlanoAdquirido)));
                        return new { url = url, status = "OK", msg = "" };
                    }
                }
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Falha ao relizar o Pagamento!" };
            }
            catch (Exception ex)
            {
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Falha ao relizar o Pagamento!" };
            }
        }
        #endregion

        #region Evento Click - Cartao Credito
        [WebMethod]
        public static object PagamentoCartaoDeCredito(string numeroCartao, int mesValidade, int anoValidade, string codigoSeguranca, int idPlanoAdquirido, int idPessoaFisica, string nomeCartaoCredito)
        {
            try
            {
                string erro = string.Empty;
                string resultadoTransacaoResposta = string.Empty;
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.CartaoCredito))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        Enumeradores.GerenciadoraTransacao gerenciadoraTransacao;
                        gerenciadoraTransacao = UtilizaCielo == "True" ? Enumeradores.GerenciadoraTransacao.Cielo : Enumeradores.GerenciadoraTransacao.PagarMe;

                        var retornoTransacao = Transacao.ValidarPagamentoCartaoCredito(ref objPagamento, objPlanoAdquirido.IdPlanoAdquirido, PageHelper.RecuperarIP(),
                            numeroCartao, mesValidade, anoValidade, codigoSeguranca, nomeCartaoCredito, null, Enumeradores.GerenciadoraTransacao.Cielo, out erro, out resultadoTransacaoResposta);


                        if (retornoTransacao)
                        {
                            return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileSuccess.aspx"), status = "OK", msg = erro };
                        }
                        else
                        {
                            //Salvar CRM motivo da compra não ser realizada.
                            CurriculoObservacao.SalvarCRM("Pagamento Cartão do plano: " + objPlanoAdquirido.IdPlanoAdquirido +
                                 " não realizado, motivo: " + erro + " - " + resultadoTransacaoResposta, new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(new PessoaFisica(idPessoaFisica))),
                                 "ControleParcelas -> Renovacao Recorrencia");

                            return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = erro };
                        }
                    }
                }
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = erro };
            }
            catch (Exception ex)
            {
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
        }
        #endregion

        public static object PagamentoCartaoDeCreditoGratis(string numeroCartao, int mesValidade, int anoValidade, string codigoSeguranca, int idPlanoAdquirido, int idPessoaFisica, string nomeCartaoCredito)
        {
            try
            {
                string erro = string.Empty;
                string resultadoTransacaoResposta = string.Empty;

                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.CartaoCredito))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        Enumeradores.GerenciadoraTransacao gerenciadoraTransacao;
                        gerenciadoraTransacao = UtilizaCielo == "True" ? Enumeradores.GerenciadoraTransacao.Cielo : Enumeradores.GerenciadoraTransacao.PagarMe;

                        var retornoTransacao = Transacao.ValidarPagamentoCartaoCreditoPrimeiraGratis(ref objPagamento, objPlanoAdquirido.IdPlanoAdquirido, PageHelper.RecuperarIP(), numeroCartao, mesValidade, anoValidade,
                            codigoSeguranca, nomeCartaoCredito, null, Enumeradores.GerenciadoraTransacao.Cielo, out erro, out resultadoTransacaoResposta);

                        if (retornoTransacao)
                        {
                            return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileSuccess.aspx"), status = "OK", msg = erro };
                        }
                        else
                        {
                            //Salvar CRM motivo da compra não ser realizada.
                            CurriculoObservacao.SalvarCRM("Pagamento Cartão do plano: " + objPlanoAdquirido.IdPlanoAdquirido +
                                 " não realizado, motivo: " + erro + " - " + resultadoTransacaoResposta, new Curriculo(Curriculo.RecuperarIdPorPessoaFisica(new PessoaFisica(idPessoaFisica))),
                                 "ControleParcelas -> Renovacao Recorrencia");
                            return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = erro };
                        }
                    }
                }
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = erro };
            }
            catch (Exception ex)
            {
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
        }


        #region Evento Click - Pagamento Boleto
        [WebMethod]
        public static object PagamentoBoleto(int idPessoaFisica, int idPlanoAdquirido, int isEmail)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.BoletoBancario))
                {
                    if (isEmail == 1)
                    {
                        string templateAssunto;
                        string mensagem = CartaEmail.RetornarConteudoBNE(Enumeradores.CartaEmail.BoletosParaPagamento, out templateAssunto);
                        byte[] pdf = BoletoBancario.MontarBoletoBytes(idPlanoAdquirido, Enumeradores.FormatoBoleto.PDF);

                        PessoaFisica objPessoaFisica = PessoaFisica.LoadObject(idPessoaFisica);

                        EmailSenderFactory.Create(TipoEnviadorEmail.Fila)
                            .Enviar(templateAssunto, mensagem, null, "atendimento@bne.com.br", objPessoaFisica.EmailPessoa, "boletos.pdf", pdf);
                        return "Seu email foi Encaminhado com Sucesso!";
                    }
                    else
                    {
                        Pagamento objPagamento = null;
                        BLL.DTO.DTOBoletoPagarMe objBoleto = null;
                        if (!Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(idPlanoAdquirido, out objPagamento))
                            Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento);
                        objBoleto = PagarMeOperacoes.GerarBoleto(objPagamento);

                        if (string.IsNullOrEmpty(objPagamento.DescricaoDescricao))
                            return new { text = String.Join("", System.Text.RegularExpressions.Regex.Split(objBoleto.CodigoDeBarra, @"[^\d]")), status = "OK" };
                        return new { text = objPagamento.DescricaoDescricao, status = "OK" };
                    }
                }
                return new { text = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), string.Format("Payment/PaymentMobileErro.aspx?IdPessoaLogada={0}&IdPlano={1}", idPessoaFisica, objPlanoAdquirido.Plano.IdPlano)), status = "FAILED" };
            }
            catch (Exception ex)
            {
                return new { text = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED" };
            }
        }
        #endregion

        #region Evento Click - Pagamento PagSeguro
        [WebMethod]
        public static object PagamentoPagSeguro(int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.PagSeguro))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        string url = Transacao.CriarTransacaoPagSeguro(objPagamento, objPlanoAdquirido, idPessoaFisica, PageHelper.RecuperarIP(), UIHelper.GetAbsoluteUrl(string.Format("Confirmacao-de-Pagamento/{0}/{1}", IntermediadorPagamento.PagSeguro.ToString(), idPlanoAdquirido)));
                        return new { url = url, status = "OK", msg = "" };
                    }
                }
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
            catch (Exception ex)
            {
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
        }
        #endregion

        #region Evento Click - Debito Recorrente HSBC
        [WebMethod]
        public static object PagamentoDebitoRecorrenteHSBC(string cpfOuCnpj, string agencia, string conta, string digito, int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.DebitoRecorrente))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        string cnpj = null;
                        string cpf = null;
                        string erro = string.Empty;
                        string numeros = String.Join("", System.Text.RegularExpressions.Regex.Split(cpfOuCnpj, @"[^\d]"));

                        string contaDigito = conta + digito;

                        if (numeros.Length == 14)
                            cnpj = numeros;
                        else
                            cpf = numeros;

                        if (Transacao.ValidarPagamentoDebito(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), Enumeradores.Banco.HSBC, agencia, contaDigito, Convert.ToDecimal(cpf), Convert.ToDecimal(cnpj), out erro))
                        {
                            return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileRegistered.aspx"), status = "OK", msg = erro };
                        }
                        else
                        {
                            return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
                        }
                    }
                }
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
            catch (Exception ex)
            {
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
        }
        #endregion

        #region Evento Click - Debito Online Bradesco
        [WebMethod]
        public static object PagamentoBradescoOnline(int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.DebitoOnline))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        string erro = string.Empty;
                        Transacao objTransacao = Transacao.ValidarPagamentoDebitoOnline(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), Enumeradores.Banco.BRADESCO, out erro);
                        objTransacao.PlanoAdquirido = objPlanoAdquirido;

                        if (objTransacao != null)
                        {
                            //criar funcao de retorno
                            return new { url = EnviaPostBradesco(objTransacao, objPagamento), status = "OK", msg = "" };
                        }


                    }
                }
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
            catch (Exception ex)
            {
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
        }
        #endregion

        #region Evento Click - Debito Online Banco do Brasil
        [WebMethod]
        public static object PagamentoBancoDoBrasilOnline(int idPlanoAdquirido, int idPessoaFisica)
        {
            try
            {
                PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
                if (PagamentoMobilePF.ProcessoCompra(ref objPlanoAdquirido, Enumeradores.TipoPagamento.DebitoOnline))
                {
                    Pagamento objPagamento = null;
                    if (Pagamento.CarregarPagamentoPrimeiraParcelaEmAbertoPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido, out objPagamento))
                    {
                        //Se existe transações do cliente dentro de um intervalo, não o deixe criar uma nova
                        if (!Transacao.ExisteTransacaoDebitoOnlineNoIntervalo(idPessoaFisica, Parametro.RecuperaValorParametro(Enumeradores.Parametro.IntervaloTempoSondaDebitoOnlineBB)))
                        {
                            string erro = string.Empty;

                            Transacao objTransacao = Transacao.ValidarPagamentoDebitoOnline(ref objPagamento, objPlanoAdquirido, PageHelper.RecuperarIP(), Enumeradores.Banco.BANCODOBRASIL, out erro);

                            if (objTransacao != null)
                            {
                                return new { url = EnviaPostBancoDoBrasil(objTransacao, objPagamento), status = "OK", msg = "" };
                            }
                        }
                    }
                }
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
            catch (Exception ex)
            {
                return new { url = String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx"), status = "FAILED", msg = "Erro ao realizar Pagamento!" };
            }
        }

        #endregion

        #endregion Eventos

        #region Validacao

        #region Validar - Forma De Pagamento Assinatura
        private bool ValidaFormaDePagamentoAssinatura(int idPlanoAdquirido, int tipoFormaDePagamento)
        {
            PlanoAdquirido objPlanoAdquirido = PlanoAdquirido.LoadObject(idPlanoAdquirido);
            objPlanoAdquirido.Plano.CompleteObject();

            if (objPlanoAdquirido.Plano.FlagRecorrente && (tipoFormaDePagamento == (int)BNE.BLL.Enumeradores.TipoPagamento.CartaoCredito || tipoFormaDePagamento == (int)BNE.BLL.Enumeradores.TipoPagamento.DebitoRecorrente))
                return true;
            else if (!objPlanoAdquirido.Plano.FlagRecorrente)
                return true;
            else
                return false;
        }
        #endregion

        #region Validar - HabilitarTodasAsFormasDePagamentos
        private bool HabilitarTodasAsFormasDePagamentos(bool habilitar)
        {
            updBoleto.Visible = updCartaoDeCredito.Visible = updDebitoEmConta.Visible =
            updDebitoOnline.Visible = updPagseguro.Visible = updPaypal.Visible = habilitar;

            updCartaoDeCredito.Update();
            updDebitoEmConta.Update();
            updBoleto.Update();
            updDebitoOnline.Update();
            updPagseguro.Update();
            updPaypal.Update();

            return habilitar;
        }
        #endregion

        #endregion Validacao

        #region Metodos

        #region TratarVisibilidadeFormasPagamento
        private void TratarVisibilidadeFormasPagamento(bool flgRecorrente)
        {
            try
            {
                //Validação para mostrar apenas cartão de credito quando o plano é de pesquisa salarial do salario br para empresas
                if (flgRecorrente) //Verificar se Plano selecionado é do tipo Recorrente, habilitar apenas Cartões de Crédito e Debito Hsbc
                    HabilitarFormasPagamentoPlanosRecorrentes(true);
                else
                    HabilitarTodasAsFormasDePagamentos(false);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex);
            }
        }

        #endregion

        #region HabilitarFormasPagamentoPlanosRecorrentes
        private void HabilitarFormasPagamentoPlanosRecorrentes(bool visible)
        {
            updCartaoDeCredito.Visible = updDebitoEmConta.Visible = visible;
            updBoleto.Visible = updDebitoOnline.Visible = updPagseguro.Visible = updPaypal.Visible = !visible;

            updCartaoDeCredito.Update();
            updDebitoEmConta.Update();
            updBoleto.Update();
            updDebitoOnline.Update();
            updPagseguro.Update();
            updPaypal.Update();
        }
        #endregion

        #region HabilitarApenasCartaoCredito
        private void HabilitarApenasCartaoCredito(bool visible)
        {
            updCartaoDeCredito.Visible = visible;
            updDebitoEmConta.Visible = updBoleto.Visible = updDebitoOnline.Visible = updPagseguro.Visible = updPaypal.Visible = !visible;

            updCartaoDeCredito.Update();
            updDebitoEmConta.Update();
            updBoleto.Update();
            updDebitoOnline.Update();
            updPagseguro.Update();
            updPaypal.Update();
        }
        #endregion

        #endregion

        #region Pagamentos

        #region Falha Na Compra
        [WebMethod]
        public static string RedirectTelaPaymentMobileErro(int idPessoaFisica, int idPlanoAdquirido)
        {
            return String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), "Payment/PaymentMobileErro.aspx");
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

        #region Debito Online - POST - Bradesco
        private static string EnviaPostBradesco(Transacao objTransacao, Pagamento objPagamento)
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
            return dadosDaRequisicao;
        }
        #endregion

        #endregion

        #region BANCO DO BRASIL

        #region Debito Online - POST - Banco do Brasil
        private static string EnviaPostBancoDoBrasil(Transacao objTransacao, Pagamento objPagamento)
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

            return @"../PagamentoDebitoOnlineBB.aspx?" + dadosDaRequisicao;
        }
        #endregion


        #endregion

        #endregion

        #endregion
    }
}