using BNE.BLL;
using BNE.BLL.Custom.Email;
using BNE.BLL.Enumeradores;
using System;

namespace BNE.Web
{
    public partial class ReimprimirBoleto : BNE.Web.Code.BasePage
    {

        #region [Page_Load]
        protected void Page_Load(object sender, EventArgs e)
        {
            if (base.IdFilial.HasValue)
            {
                if (!IsPostBack)
                {
                    string urlPdf = string.Empty; var urlImg = string.Empty;
                    byte[] img, boleto;
                    var htmlBoleto = BoletoBancario.RecuperarUltimioBoletoNaoPago(base.IdFilial.Value);
                    BoletoBancario.RetornarBoleto(htmlBoleto, out boleto, out img, out urlPdf, out urlImg);
                    imgBoleto.ImageUrl = urlImg;
                    if(htmlBoleto.Length < 40)
                        Redirect(GetRouteUrl(RouteCollection.EscolhaPlanoCIA.ToString(), null));
                }

                ucConfirmacao.eventVoltar += ucConfirmacao_Voltar;
            }
            else
            {
                Redirect(GetRouteUrl(RouteCollection.SalaSelecionador.ToString(), null));
            }
        }
        #endregion

        #region [ucConfirmacao_Voltar]
        private void ucConfirmacao_Voltar()
        {
            Redirect(GetRouteUrl(RouteCollection.SalaSelecionador.ToString(), null));
        }
        #endregion

        #region [btnEnviarPorEmail_Click]
        protected void btnEnviarPorEmail_Click(object sender, EventArgs e)
        {
            var templateAssunto = string.Empty;
            var EnderecoEmail = BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.EmailMensagens);
            var TemplateEmail = BLL.CartaEmail.RetornarConteudoBNE(BLL.Enumeradores.CartaEmail.BoletosParaPagamento, out templateAssunto);

            Filial objFilial = new Filial(base.IdFilial.Value);
            objFilial.CompleteObject();
            var emailVendedor = objFilial.Vendedor().EmailVendedor;
            var emailUsuarioFilial = new UsuarioFilialPerfil(base.IdUsuarioFilialPerfilLogadoEmpresa.Value).Email();
            templateAssunto = templateAssunto.Replace("Boletos", "Boleto");

            string urlPdf = string.Empty; var urlImg = string.Empty;
            byte[] img, boleto;
            var htmlBoleto = BoletoBancario.RecuperarUltimioBoletoNaoPago(base.IdFilial.Value);
            BoletoBancario.RetornarBoleto(htmlBoleto, out boleto, out img, out urlPdf, out urlImg);

            try
            {
                if (boleto != null)
                {
                    EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(templateAssunto, TemplateEmail, null, emailVendedor, emailUsuarioFilial, "boleto.pdf", boleto);
                    ucConfirmacao.PreencherCampos("Enviado com Sucesso", $"Boleto enviado para o e-mail {emailUsuarioFilial}", false, true);
                    ucConfirmacao.MostrarModal();
                }
                else
                {
                    base.ExibirMensagem("Boleto não foi Gerado.", Code.Enumeradores.TipoMensagem.Aviso);
                }


            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao enviar boleto por e-mail na tela reimprimir boleto");
                base.ExibirMensagemErro(ex, "Ocorreu um erro na geração do boleto, tente novamento mais tarde.");
            }
        }

        #endregion

        #region [btnVoltar_Click]
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(RouteCollection.SalaSelecionador.ToString(), null));
        }
        #endregion
    }
}