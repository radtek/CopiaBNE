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

        #region ImageByte - Variável 7
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        protected byte[] ImageByte
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel7.ToString()] != null)
                    return (byte[])ViewState[Chave.Temporaria.Variavel7.ToString()];
                return null;
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
                ImageByte = ImageByte;
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
                divBreadCrumb.Visible = objPlanoAdquirido.Plano.PlanoTipo.IdPlanoTipo.Equals((int)Enumeradores.PlanoTipo.PessoaFisica) ? true : false;


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

                if (end != null && end.IdEndereco != null && end.Cidade !=null)//se for compra direto do preCurriculo do bne novo não vai ter endereço
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
                divPlanoLiberadoVip.Visible = false;
                divPlanoLiberadoCia.Visible = false;
                divDebitoRecorrente_AguardandoVIP.Visible = false;
                divDebitoRecorrente_AguardandoCIA.Visible = false;

                if (base.PagamentoFormaPagamento.Value == (int)Enumeradores.TipoPagamento.BoletoBancario)
                {
                    if (ImageByte == null)
                        GerarBoleto(objPlanoAdquirido);
                    else
                        AjustarBoleto(ImageByte);

                    pnlBoleto.Visible = true;
                }
                else if (base.PagamentoFormaPagamento.Value == (int)Enumeradores.TipoPagamento.CartaoCredito)
                {
                    pnlPlanoLiberado.Visible = true;

                    if (TipoPlano == "VIP")
                        divPlanoLiberadoVip.Visible = true;
                    else
                        divPlanoLiberadoCia.Visible = true;

                    if (objPlanoAdquirido.Plano.IdPlano.Equals(Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.PlanoCandidaturaPremium))))
                    {
                        divBreadCrumb.Visible = false;
                        pnlPlanoLiberado.Visible = false;
                        pnlPremiumCandidatura.Visible = true;
                        Vaga objVaga = Vaga.LoadObject(PlanoAdquiridoDetalhes.RecuperarIdVagaPorPlanoAdquirido(objPlanoAdquirido.IdPlanoAdquirido));
                        objVaga.Cidade.CompleteObject();
                        lblCidade.Text = String.Format("{0}/{1}", objVaga.Cidade.NomeCidade, objVaga.Cidade.Estado.SiglaEstado);
                        lblFuncao.Text = objVaga.DescricaoFuncao;
                        lnkVerVaga.PostBackUrl = String.Format("{0}?u={1}",Vaga.MontarUrlVaga(objVaga.IdVaga), "Candidatou");

                        lnkVerVagas.PostBackUrl = String.Format("http://{0}/vagas-de-emprego-para-{1}-em-{2}-{3}", UIHelper.RecuperarURLAmbiente(), objVaga.DescricaoFuncao,
                        objVaga.Cidade.NomeCidade, objVaga.Cidade.Estado.SiglaEstado).ToLower().Replace(" ","-");
                    }
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

                        if (TipoPlano == "VIP")
                            divDebitoRecorrente_AguardandoVIP.Visible = true;
                        else
                            divDebitoRecorrente_AguardandoCIA.Visible = true;
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
            ucEnvioEmail.HFIdPlanoAdquirido = base.PagamentoIdentificadorPlanoAdquirido.Value.ToString();

        }
        #endregion

        #region hlDownload_Click
        protected void hlDownload_Click(object sender, EventArgs e)
        {
            Redirect("DownloadArquivo.aspx?path=" + PDFURL + "&filename=boleto");
        }
        #endregion

        #endregion
        #region Metodos

        #region GerarBoleto
        /// <summary>
        /// Metodo responsável por ajustar o boleto, PF ou PJ
        /// </summary>
        private void GerarBoleto(PlanoAdquirido objPlanoAdquirido)
        {
            string htmlFile = string.Empty;
            List<BLL.DTO.DTOBoletoPagarMe> boletos = null;

            if (objPlanoAdquirido.CompleteObject())
            {
                boletos = BoletoBancario.GerarBoletosNovoPagarme(objPlanoAdquirido);
            }
            else if (base.PagamentoAdicionalValorTotal.HasValue && base.PagamentoAdicionalQuantidade.HasValue) //Quando for compra de SMS Adicional            
            {
                int identificadorPagamento = base.PagamentoIdentificadorPagamento.Value;
                boletos = BoletoBancario.GerarBoletoPagamentoAdicionalNovo(base.PagamentoAdicionalValorTotal.Value, base.PagamentoAdicionalQuantidade.Value, base.IdFilial.Value, base.IdUsuarioFilialPerfilLogadoEmpresa.Value, ref identificadorPagamento);
                base.PagamentoIdentificadorPagamento.Value = identificadorPagamento;
            }

            byte[] pdfArray = null;

            byte[] imgArray = null;

            if (boletos != null)
            {
                BoletoBancario.RetornarBoleto(BoletoBancario.GerarLayoutBoletoHTMLPagarMe(boletos),out pdfArray,out imgArray);
                PDFArray = pdfArray;
                ImageByte = imgArray;

                AjustarBoleto(imgArray);
            }

            if (boletos != null && boletos != null  && pdfArray != null)
            {           
                    string emailDestinatario = string.Empty;

                    if (objPlanoAdquirido.ParaPessoaJuridica())
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
                        MensagemCS.EnvioDeEmailComValidacao(TipoEnviadorEmail.Fila, carta.Assunto, carta.Conteudo,BLL.Enumeradores.CartaEmail.ConteudoBoletoVencimento, emailRemetente, emailDestinatario, "boletos.pdf", PDFArray);
                    }               
            }             
        }
        #endregion

        #region AjustarBoleto
        private void AjustarBoleto(byte[] ImageByte)
        {
            if (string.IsNullOrWhiteSpace(PDFURL))
            {
                hlDownload.Visible = false;
                hlDownloadTopo.Visible = false;
                btnImprimir.Visible = false;
            }

            imgBoleto.ImageUrl = "data:image/jpg;base64," + Convert.ToBase64String(ImageByte);
        }
        #endregion

        #endregion

    }
}