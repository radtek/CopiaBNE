using System;
using System.Globalization;
using System.Threading;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;
using BNE.Web.Code.Session;
using System.Web.SessionState;

namespace BNE.Web
{
    public partial class PagamentoCartao : BasePagePagamento
    {

        #region Propriedades

        #region IdConteudoHTMLFormaPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o IdConteudoHTML
        /// </summary>
        protected int? IdConteudoHTMLFormaPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdConteudoHTML.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdConteudoHTML.ToString()].ToString());

                if (Session[Chave.Temporaria.IdConteudoHTML.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdConteudoHTML.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdConteudoHTML.ToString(), value);
                Session.Add(Chave.Temporaria.IdConteudoHTML.ToString(), value);
            }
        }
        #endregion

        #region IdPlanoFormaPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        protected int? IdPlanoFormaPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlano.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlano.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlano.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlano.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlano.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlano.ToString(), value);
            }
        }
        #endregion

        #region IdPlanoNormalFormaPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano Extendido
        /// </summary>
        protected int? IdPlanoNormalFormaPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlanoNormal.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlanoNormal.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlanoNormal.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlanoNormal.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlanoNormal.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlanoNormal.ToString(), value);
            }
        }
        #endregion

        #region IdPlanoExtendidoFormaPagamento
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano Extendido
        /// </summary>
        protected int? IdPlanoExtendidoFormaPagamento
        {
            get
            {
                if (ViewState[Chave.Temporaria.IdPlanoExtendido.ToString()] != null)
                    return Int32.Parse(ViewState[Chave.Temporaria.IdPlanoExtendido.ToString()].ToString());

                if (Session[Chave.Temporaria.IdPlanoExtendido.ToString()] != null)
                    return Int32.Parse(Session[Chave.Temporaria.IdPlanoExtendido.ToString()].ToString());

                return null;
            }
            set
            {
                ViewState.Add(Chave.Temporaria.IdPlanoExtendido.ToString(), value);
                Session.Add(Chave.Temporaria.IdPlanoExtendido.ToString(), value);
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        private void Inicializar()
        {
            IdConteudoHTMLFormaPagamento = IdConteudoHTMLFormaPagamento;
            IdPlanoFormaPagamento = IdPlanoFormaPagamento;
            IdPlanoNormalFormaPagamento = IdPlanoNormalFormaPagamento;
            IdPlanoExtendidoFormaPagamento = IdPlanoExtendidoFormaPagamento;

            var objPagamento = new BLL.Pagamento(base.IdPagamento.Value);
            litValorPagamento.Text = objPagamento.RecuperarValor().ToString(CultureInfo.CurrentCulture);

            if (Session["MensagemErroPagamentoCartao"] != null && !String.IsNullOrEmpty(Session["MensagemErroPagamentoCartao"].ToString()))
            {
                ExibirMensagem(Session["MensagemErroPagamentoCartao"].ToString(), TipoMensagem.Erro);
                Session.Remove("MensagemErroPagamentoCartao");
            }
        }
        #endregion

        #region ValidarPagamento
        private bool ValidarPagamento(out string erro)
        {
            erro = string.Empty;

            var idPagamento = base.IdPagamento.Value;
            var idPlanoAdquirido = base.IdPlanoAdquirido.Value;

            var objPagamento = BLL.Pagamento.LoadObject(idPagamento);

            string numeroCartao = string.Concat(txtNumeroDoCartaoParte1.Valor, txtNumeroDoCartaoParte2.Valor, txtNumeroDoCartaoParte3.Valor, txtNumeroDoCartaoParte4.Valor);
            int mesValidadeCartao = Convert.ToInt32(txtDataDeValidadeMes.Valor);
            int anoValidadeCartao = Convert.ToInt32(txtDataDeValidadeAno.Valor);
            int numeroDigitoVerificador = Convert.ToInt32(txtDigitoVerificador.Valor);

            int bandeiraCartao = 0;
            if (rbtVisaCredito.Checked)
                bandeiraCartao = (int)Enumeradores.Operadora.Visa;

            if (rbtMastercardCredito.Checked)
                bandeiraCartao = (int)Enumeradores.Operadora.Master;

            if (bandeiraCartao.Equals(0))
                erro = "Selecione a bandeira!";
            else
            {
                txtNumeroDoCartaoParte1.Valor = "";
                txtNumeroDoCartaoParte2.Valor = "";
                txtNumeroDoCartaoParte3.Valor = "";
                txtNumeroDoCartaoParte4.Valor = "";
                txtDataDeValidadeMes.Valor = "";
                txtDataDeValidadeAno.Valor = "";
                txtDigitoVerificador.Valor = "";

                this.upCartaoDeCredito.Update();
                return Transacao.ValidarPagamentoCartaoCredito(objPagamento, idPlanoAdquirido, PageHelper.RecuperarIP(), Convert.ToDecimal(numeroCartao), mesValidadeCartao, anoValidadeCartao, numeroDigitoVerificador, bandeiraCartao, out erro);
            }

            return false;
        }
        #endregion

        #region Voltar
        private void Voltar()
        {
            Redirect("/FormaPagamento.aspx");
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            if (!base.STC.Value || (base.STC.Value && !base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue))
                InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "PagamentoCartao");
            else
                InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "PagamentoCartao");

            this.txtDataDeValidadeAno.ValorMinimo = DateTime.Now.ToString("yy");
        }
        #endregion

        #region btnConcluirCartaoDeCredito_Click
        protected void btnConcluirCartaoDeCredito_Click(object sender, EventArgs e)
        {
            try
            {
                string erro;
                if (ValidarPagamento(out erro))
                {
                    StatusTransacaoCartao = true;

                    string urlRedirect = base.UrlRetornoPagamento.Value;
                    base.LimparSessionPagamento();

                    Redirect(urlRedirect);
                }
                else
                {
                    if (string.IsNullOrEmpty(erro))
                        Session.Add("MensagemErroPagamentoCartao", "Ocorreu um erro ao tentar validar sua compra, tente novamente!");
                    else
                        Session.Add("MensagemErroPagamentoCartao", erro);
                    Redirect("PagamentoCartao.aspx");
                }
            }
            catch (Exception ex)
            {
                base.ExibirMensagem("Ocorreu um erro ao tentar validar sua compra, tente novamente!", TipoMensagem.Erro);
                if (!(ex is ThreadAbortException))
                    EL.GerenciadorException.GravarExcecao(ex);
            }
        }
        #endregion

        #region lbtVoltarCartaoDeCredito_Click
        protected void lbtVoltarCartaoDeCredito_Click(object sender, EventArgs e)
        {
            Voltar();
        }
        #endregion

        #endregion

    }
}