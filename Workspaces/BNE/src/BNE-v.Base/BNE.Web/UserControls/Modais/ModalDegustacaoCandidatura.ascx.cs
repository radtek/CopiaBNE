using System;
using BNE.BLL;
using BNE.BLL.Custom;
using BNE.Web.Code;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web.UserControls.Modais
{
    public partial class ModalDegustacaoCandidatura : BaseUserControl
    {

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        #region Inicializar
        public void Inicializar(bool mostrarVoceGanhou, bool mostrarVoceGanhouTextoAdicional, bool mostrarBotoes, int? quantidadeCandidaturasRestantes)
        {
            litQuantidadeDegustacao.Text = string.Empty;
            lbtUsarBonus.Visible = false;
            lbtNaoAgora.Visible = false;
            lbtQueroPassarParaVip.Visible = false;

            if (mostrarVoceGanhou)
            {
                var quantidadeDegustacao = Parametro.RecuperaValorParametro(Enumeradores.Parametro.QuantidadeCandidaturaDegustacao);
                var conteudo = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ModalDegustacaoVoceGanhou);
                var parametros = new
                    {
                        quantidadeCandidaturas = quantidadeDegustacao
                    };
                litInformacao.Text = parametros.ToString(conteudo);
                litQuantidadeDegustacao.Text = quantidadeDegustacao;

                if (!mostrarBotoes)
                    btiFechar.Visible = true;
            }
            if (mostrarVoceGanhouTextoAdicional)
            {
                lblInformacaoAdicional.Visible = true;
                lblInformacaoAdicional.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ModalDegustacaoVoceGanhouTextoAdicional);
            }
            if (mostrarBotoes)
            {
                lbtQueroPassarParaVip.Visible = true;

                if (quantidadeCandidaturasRestantes.HasValue)
                {
                    if (quantidadeCandidaturasRestantes.Value.Equals(-1) || quantidadeCandidaturasRestantes.Value.Equals(0))
                        lbtNaoAgora.Visible = true;
                    else
                        lbtUsarBonus.Visible = true;
                }
                else
                    lbtUsarBonus.Visible = true;
            }

            if (quantidadeCandidaturasRestantes.HasValue)
            {
                btiFechar.Visible = true;

                if (quantidadeCandidaturasRestantes.Value.Equals(-1) || quantidadeCandidaturasRestantes.Value.Equals(0))
                {
                    imgAlerta.Visible = true;
                    imgEnvelope.Visible = false;
                    pnlEnvelope.CssClass = "img_envelope_bne alerta";
                    pnlModalDegustacaoCandidatura.CssClass = "modal_nova_candidatura amarela";
                    pnlInformacao.CssClass = "voce_ganhou que_pena";
                    lbtUsarBonus.CssClass = "alinha_btnUsarBonus complementa_btn_cinza_alinhamento";
                    litInformacao.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ModalDegustacaoAcabou);
                }

                if (quantidadeCandidaturasRestantes.Value.Equals(1))
                {
                    litInformacao.Text = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ModalDegustacaoUma);
                    litQuantidadeDegustacao.Text = quantidadeCandidaturasRestantes.ToString();
                }

                if (quantidadeCandidaturasRestantes.Value >= 2)
                {
                    string texto = ConteudoHTML.RecuperaValorConteudo(Enumeradores.ConteudoHTML.ModalDegustacaoDuasOuMais);
                    var numeroPorExtenso = new NumeroPorExtenso(Convert.ToDecimal(quantidadeCandidaturasRestantes.Value), false);
                    var parametros = new
                    {
                        quantidadeCandidaturas = numeroPorExtenso.ToString()
                    };

                    litInformacao.Text = parametros.ToString(texto);
                    litQuantidadeDegustacao.Text = quantidadeCandidaturasRestantes.ToString();
                }
            }

            upEnvelope.Update();
            upInformacao.Update();
            upBotoes.Update();

            AbrirModal();
        }
        #endregion

        #region btiFechar_Click
        protected void btiFechar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region lbtUsarBonus_Click
        protected void lbtUsarBonus_Click(object sender, EventArgs e)
        {
            FecharModal();

            if (UsarBonus != null)
                UsarBonus();
        }
        #endregion

        #region lbtNaoAgora_Click
        protected void lbtNaoAgora_Click(object sender, EventArgs e)
        {
            FecharModal();
        }
        #endregion

        #region lbtQueroPassarParaVip_Click
        protected void lbtQueroPassarParaVip_Click(object sender, EventArgs e)
        {
            Redirect(GetRouteUrl(Enumeradores.RouteCollection.ProdutoVIP.ToString(), null));
        }
        #endregion

        #region FecharModal
        private void FecharModal()
        {
            mpeModalDegustacaoCandidatura.Hide();
        }
        #endregion

        #region AbrirModal
        private void AbrirModal()
        {
            mpeModalDegustacaoCandidatura.Show();
        }
        #endregion

        #region Delegates
        public delegate void DelegateUsarBonus();
        public event DelegateUsarBonus UsarBonus;
        #endregion

    }
}