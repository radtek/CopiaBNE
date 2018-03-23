using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.UserControls.Modais;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class ConfirmacaoPagamento : BasePage
    {

        #region Propriedades

        public string IdPlanoAdquirido;
        public string IdPlano;
        public string NomePlano;
        public string TipoPlano;
        public string VlrPagamento;
        public string FormaPagamento;
        public string NmeCidade;
        public string NmeEstado;
        public string SiglaPais;

        #region UrlOrigem - Variável 5
        /// <summary>
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel5.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel5.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                {
                    Session.Add(Chave.Temporaria.Variavel5.ToString(), value);
                    ViewState.Add(Chave.Temporaria.Variavel5.ToString(), value);
                }
                else
                    ViewState.Remove(Chave.Temporaria.Variavel5.ToString());
            }
        }
        #endregion

        #region UrlRedirect - Variável 6
        /// <summary>
        /// </summary>
        public string UrlRedirect
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel6.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel6.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                {
                    Session.Add(Chave.Temporaria.Variavel6.ToString(), value);
                    ViewState.Add(Chave.Temporaria.Variavel6.ToString(), value);
                }
                else
                    ViewState.Remove(Chave.Temporaria.Variavel6.ToString());
            }
        }
        #endregion

        #region ImageUrl - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        protected string ImageUrl
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel7.ToString()].ToString();
                return String.Empty;
            }
            set
            {
                Session.Add(Chave.Temporaria.Variavel7.ToString(), value);
                ViewState.Add(Chave.Temporaria.Variavel7.ToString(), value);
            }
        }
        #endregion

        #region PDFArray - Variável 8
        /// <summary>
        /// Propriedade que armazena e recupera o array com o PDF
        /// </summary>
        protected byte[] PDFArray
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel8.ToString()] != null)
                    return (byte[])ViewState[Chave.Temporaria.Variavel8.ToString()];
                return null;
            }
            set
            {
                Session.Add(Chave.Temporaria.Variavel8.ToString(), value);
                ViewState.Add(Chave.Temporaria.Variavel8.ToString(), value);
            }
        }
        #endregion

        #region PdfURL - Variável 9
        /// <summary>
        /// Propriedade que armazena e recupera o url do pdf
        /// </summary>
        protected string PDFURL
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel9.ToString()] != null)
                    return ViewState[Chave.Temporaria.Variavel9.ToString()].ToString();
                return String.Empty;
            }
            set
            {
                Session.Add(Chave.Temporaria.Variavel9.ToString(), value);
                ViewState.Add(Chave.Temporaria.Variavel9.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //Acumulando o valor das propriedades para evitar erro ao dar um F5 e efetuar alguma ação na tela.
                UrlOrigem = UrlOrigem;
                ImageUrl = ImageUrl;
                PDFArray = PDFArray;
                PDFURL = PDFURL;

                if (Request.UrlReferrer != null)
                    UrlOrigem = Request.UrlReferrer.AbsoluteUri;

                PlanoAdquirido objPlanoAdquirido = new PlanoAdquirido(base.PagamentoIdentificadorPlanoAdquirido.Value);
                objPlanoAdquirido.CompleteObject();
                objPlanoAdquirido.Plano.CompleteObject();

                #region Definindo valores para o gaTrackingEcommerce
                IdPlano = objPlanoAdquirido.Plano.IdPlano.ToString();
                IdPlanoAdquirido = base.PagamentoIdentificadorPlanoAdquirido.Value.ToString();
                NomePlano = objPlanoAdquirido.Plano.DescricaoPlano;
                TipoPlano = objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica) ? "VIP" : "CIA";
                VlrPagamento = base.PagamentoValorPago.Value.ToString("F2", CultureInfo.GetCultureInfo("en-US"));

                switch ((Enumeradores.TipoPagamento)base.PagamentoFormaPagamento.Value)
                {
                    case Enumeradores.TipoPagamento.BoletoBancario:
                        FormaPagamento = "Boleto";
                        break;
                    case Enumeradores.TipoPagamento.CartaoCredito:
                        FormaPagamento = "Cartão de Crédito";
                        break;
                    default:
                        if (base.PagamentoFormaPagamento.HasValue && base.PagamentoFormaPagamento.Value > 0)
                            FormaPagamento = ((Enumeradores.TipoPagamento)base.PagamentoFormaPagamento.Value).ToString();
                        else
                            FormaPagamento = "Não Definida";
                        break;
                }

                UrlRedirect = objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica) ? "SalaVip.aspx" : "SalaSelecionador.aspx";
                btn_sala_vip.Text = objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica) ? "Ir para minha sala VIP" : "Ir para a sala da selecionadora";

                UsuarioFilialPerfil objUsuarioFilialPerfil;
                Endereco end;
                if (base.IdUsuarioFilialPerfilLogadoCandidato.HasValue)
                {
                    objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoCandidato.Value);
                    objUsuarioFilialPerfil.CompleteObject();
                    Endereco.CarregarPorPessoaFisica(objUsuarioFilialPerfil.PessoaFisica, out end);
                }
                else
                {
                    objUsuarioFilialPerfil = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);
                    objUsuarioFilialPerfil.CompleteObject();
                    Endereco.CarregarPorFilial(objUsuarioFilialPerfil.Filial, out end);
                }

                if (end != null && end.IdEndereco != null)
                {
                    if (end.Cidade.NomeCidade == null)
                        end.Cidade.CompleteObject();
                    NmeCidade = end.Cidade.NomeCidade;

                    if (end.Cidade.Estado.NomeEstado == null)
                        end.Cidade.Estado.CompleteObject();
                    NmeEstado = end.Cidade.Estado.NomeEstado;

                    SiglaPais = "BR";
                }
                #endregion Definindo valores para o gaTrackingEcommerce

                pnlBoleto.Visible = false;
                pnlPlanoLiberado.Visible = false;
                pnlAguardandoIntermediador.Visible = false;
                pnlDebitoRecorrente_Aguardando.Visible = false;
                pnlDebitoRecorrente_Liberado.Visible = false;
                if (base.PagamentoFormaPagamento.Value == (int)Enumeradores.TipoPagamento.BoletoBancario)
                {
                    if (String.IsNullOrEmpty(ImageUrl))
                        GerarBoleto();
                    else
                        AjustarBoleto();

                    pnlBoleto.Visible = true;
                }
                else if (base.PagamentoFormaPagamento.Value == (int)Enumeradores.TipoPagamento.CartaoCredito)
                {
                    pnlPlanoLiberado.Visible = true;
                }
                else if (base.PagamentoFormaPagamento.Value == (int)Enumeradores.TipoPagamento.PagSeguro)
                {
                    pnlAguardandoIntermediador.Visible = true;
                    lblNomeIntermediador.Text = "PagSeguro";
                }
                else if (base.PagamentoFormaPagamento.Value == (int)Enumeradores.TipoPagamento.PayPal)
                {
                    pnlAguardandoIntermediador.Visible = true;
                    lblNomeIntermediador.Text = "PayPal";
                }
                else if (base.PagamentoFormaPagamento.Value == (int)Enumeradores.TipoPagamento.DebitoRecorrente)
                {
                    if (Convert.ToBoolean(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PagamentoDebitoLiberacaoNaConfirmacaoDoDebito)))
                    {
                        pnlDebitoRecorrente_Aguardando.Visible = true;
                    }
                    else
                    {
                        pnlDebitoRecorrente_Liberado.Visible = true;
                    }
                }
            }

            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "FormaPagamento");
        }
        #endregion

        #region BtnVoltarClick
        protected void BtnVoltarClick(object sender, EventArgs e)
        {
            Redirect(!string.IsNullOrEmpty(UrlOrigem) ? UrlOrigem : "Default.aspx");
        }
        #endregion

        #region BtnIrParaSalaVipClick
        protected void BtnIrParaSalaVipClick(object sender, EventArgs e)
        {
            Redirect(!string.IsNullOrEmpty(UrlRedirect) ? UrlRedirect : "Default.aspx");
        }
        #endregion

        #region btnEnviarPorEmail_Click
        protected void btnEnviarPorEmail_Click(object sender, EventArgs e)
        {
            ucEnvioEmail.ArquivoAnexo = new Dictionary<string, byte[]> { { "boletos.pdf", PDFArray } };
            ucEnvioEmail.MostrarModal(EnvioEmail.TipoEnvioEmail.BoletoPagamento);
        }
        #endregion

        #endregion

        #region Metodos

        #region GerarBoleto
        /// <summary>
        /// Metodo responsável por ajustar o boleto, PF ou PJ
        /// </summary>
        private void GerarBoleto()
        {
            if (base.PagamentoIdentificadorPlanoAdquirido.HasValue)
            {
                var objPlanoAdquirido = PlanoAdquirido.LoadObject(base.PagamentoIdentificadorPlanoAdquirido.Value);

                if (objPlanoAdquirido != null)
                {
                    var parcelas = PlanoParcela.RecuperarParcelasPlanoAdquirido(objPlanoAdquirido);
                    var listaPagamento = new List<Pagamento>();

                    foreach (var objPlanoParcela in parcelas)
                    {
                        List<BLL.Pagamento> objListPagamentosEmAberto = BLL.Pagamento.CarregaPagamentosPorPlanoParcela(objPlanoParcela.IdPlanoParcela);
                        var listaPagamentoParcela = objListPagamentosEmAberto.Where(p => p.TipoPagamento.IdTipoPagamento == (int)Enumeradores.TipoPagamento.BoletoBancario
                                        && p.PagamentoSituacao.IdPagamentoSituacao == (int)Enumeradores.PagamentoSituacao.EmAberto
                                        && p.FlagInativo == false).ToList();

                        listaPagamento = listaPagamento.Concat(listaPagamentoParcela).ToList();
                    }

                    AjustarCriacaoAtualizacaoBoleto(listaPagamento, objPlanoAdquirido.ParaPessoaJuridica());
                }
            }
            else if (base.PagamentoAdicionalValorTotal.HasValue && base.PagamentoAdicionalQuantidade.HasValue) //Quando for compra de SMS Adicional
            {
                var objFilial = new Filial(base.IdFilial.Value);

                PlanoAdquirido objPlanoAdquirido;
                PlanoAdquirido.CarregarPlanoAdquiridoPorSituacao(objFilial, (int)Enumeradores.PlanoSituacao.Liberado, out objPlanoAdquirido);

                if (objPlanoAdquirido != null)
                {
                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(base.IdUsuarioFilialPerfilLogadoEmpresa.Value);

                    BLL.Pagamento objPagamento;
                    AdicionalPlano.CriarPagamentoEPlanoAdicionalSMS(objPlanoAdquirido, base.PagamentoAdicionalValorTotal.Value, base.PagamentoAdicionalQuantidade.Value, objUsuarioFilialPerfil, Enumeradores.TipoPagamento.BoletoBancario, DateTime.Now, DateTime.Today, out objPagamento);

                    base.PagamentoIdentificadorPagamento.Value = objPagamento.IdPagamento;
                    var listaPagamento = new List<Pagamento> { objPagamento };
                    AjustarCriacaoAtualizacaoBoleto(listaPagamento, objPlanoAdquirido.ParaPessoaJuridica());
                }
            }
        }
        #endregion

        #region AjustarCriacaoAtualizacaoBoleto
        /// <summary>
        /// Metodo responsavel por ajustar a criação/atualizacao do boleto e imprimir na tela e manda e-mail
        /// </summary>
        /// <param name="pagamentos">Object</param>
        private void AjustarCriacaoAtualizacaoBoleto(List<BLL.Pagamento> pagamentos, bool pessoaJuridica)
        {
            byte[] pdfArray = null;
            string pdfURL = string.Empty;
            //Se tiver vários pagamentos gera uma imagem apenas.
            var retorno = pagamentos.Count >= 1 ? CobrancaBoleto.GerarBoleto(pagamentos, out pdfArray, out pdfURL) : CobrancaBoleto.GerarBoleto(pagamentos.First());

            if (!string.IsNullOrEmpty(retorno))
            {
                PDFArray = pdfArray;
                PDFURL = pdfURL;
                ImageUrl = retorno;

                AjustarBoleto();
            }

            if (pagamentos.Count > 1 && pdfArray != null)
            {
                string emailDestinatario = string.Empty;

                if (pessoaJuridica)
                {
                    UsuarioFilial objUsuarioFilial;
                    if (UsuarioFilial.CarregarUsuarioFilialPorUsuarioFilialPerfil(IdUsuarioFilialPerfilLogadoEmpresa.Value, out objUsuarioFilial))
                        emailDestinatario = objUsuarioFilial.EmailComercial;
                }
                else
                {
                    var objUsuarioFilialPerfil = UsuarioFilialPerfil.LoadObject(IdUsuarioFilialPerfilLogadoCandidato.Value);
                    emailDestinatario = objUsuarioFilialPerfil.PessoaFisica.EmailPessoa;
                }

                if (!string.IsNullOrWhiteSpace(emailDestinatario))
                {
                    var emailRemetente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.ControleParcelasRemetente);
                    var carta = CartaEmail.RecuperarCarta(BLL.Enumeradores.CartaEmail.ConteudoBoletoVencimento);

                    EmailSenderFactory.Create(TipoEnviadorEmail.Fila).Enviar(carta.Assunto, carta.Conteudo, emailRemetente, emailDestinatario.Trim(), "boletos.pdf", PDFArray);
                }
            }
        }
        #endregion

        #region AjustarBoleto
        private void AjustarBoleto()
        {
            btnImprimir.Visible = string.IsNullOrWhiteSpace(PDFURL);

            if (string.IsNullOrWhiteSpace(PDFURL))
                hlDownload.Visible = false;
            else
                hlDownload.HRef = PDFURL;

            imgBoleto.ImageUrl = ImageUrl;
        }
        #endregion

        #endregion

    }
}