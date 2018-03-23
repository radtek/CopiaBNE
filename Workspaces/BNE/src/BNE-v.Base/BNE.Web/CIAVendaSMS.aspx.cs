using System;
using System.Collections.Generic;
using System.Web.UI;
using BNE.BLL;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using Enumeradores = BNE.BLL.Enumeradores;

namespace BNE.Web
{
    public partial class CIAVendaSMS : BasePage
    {

        #region Propriedades

        #region ValorSMSAvulso - Variável 1
        public decimal ValorSMSAvulso
        {
            get
            {
                return Convert.ToDecimal(ViewState[Chave.Temporaria.Variavel1.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
            }
        }
        #endregion

        #region QuantidadeSMSMinima - Variável 2
        public decimal QuantidadeSMSMinima
        {
            get
            {
                return Convert.ToDecimal(ViewState[Chave.Temporaria.Variavel2.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel2.ToString(), value);
            }
        }
        #endregion

        #region QuantidadeSMSMaxima - Variável 3
        public decimal QuantidadeSMSMaxima
        {
            get
            {
                return Convert.ToDecimal(ViewState[Chave.Temporaria.Variavel3.ToString()]);
            }
            set
            {
                ViewState.Add(Chave.Temporaria.Variavel3.ToString(), value);
            }
        }
        #endregion

        #region UrlOrigem - Variável 6
        /// <summary>
        /// Propriedade que armazena e recupera o enumerador do login origem
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (Session["UrlOrigem"] != null)
                    return Session["UrlOrigem"].ToString();
                return null;
            }
            set
            {
                if (value != null)
                    Session.Add("UrlOrigem", value);
                else
                    Session.Remove("UrlOrigem");
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UrlOrigem))
                Redirect(UrlOrigem);
            else
                Redirect("Default.aspx");
        }
        #endregion

        #region txtQuantidadeSMS_ValorAlterado
        /// <summary>
        /// Evento disparado quando a quantidade de sms é alterada
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void txtQuantidadeSMS_ValorAlterado(object sender, EventArgs e)
        {
            AjustarValorTotal(CalcularValorPlano(), Convert.ToInt32(txtQuantidadeSMS.Valor));
            ValidarValoresInformados();
        }
        #endregion

        #region btnContinuar_Click
        protected void btnContinuar_Click(object sender, ImageClickEventArgs e)
        {
            String url;
            // Seta url para montar retorno da pagina a ser redirecionada a aplicacao após finalizar operações na cielo.
            if (String.IsNullOrEmpty(Request.Url.Query))
                url = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "");
            else
                url = Request.Url.AbsoluteUri.Replace(Request.Url.AbsolutePath, "").Replace(Request.Url.Query, "");

            string urlRetorno = string.Empty;

            if (base.IdUsuarioFilialPerfilLogadoEmpresa.HasValue)
                urlRetorno = url + "/SalaSelecionadorPlanoIlimitado.aspx";

            base.PagamentoUrlRetorno.Value = urlRetorno;
            base.PagamentoAdicionalValorTotal.Value = CalcularValorPlano();
            base.PagamentoAdicionalQuantidade.Value = Convert.ToInt32(txtQuantidadeSMS.Valor);


            var r = Rota.RecuperarURLRota(Enumeradores.RouteCollection.PagamentoPlano);

#if DEBUG
            Redirect(String.Format("http://{0}/{1}", UIHelper.RecuperarURLAmbiente(), r));
#else
            Redirect(String.Format("https://{0}/{1}", UIHelper.RecuperarURLAmbiente(), r));
#endif

        }
        #endregion

        #endregion

        #region Métodos

        #region Inicializar
        public void Inicializar()
        {
            LimparSessionPagamento();

            AjustarURLdeOrigem();
            CarregarParametrosEAjustarQuantidadeSMS();
            AjustarLabelValorSMS(ValorSMSAvulso);
            AjustarValorTotal(CalcularValorPlano(), Convert.ToInt32(txtQuantidadeSMS.Valor));

            /*if (IdPlano.HasValue)
            {
                ucFormaPagamento.IdPlano = IdPlano;
                ucFormaPagamento.Inicializar();
            }*/
            AjustarTituloTela("Pagamento");
            InicializarBarraBusca(TipoBuscaMaster.Vaga, false, "FormaPagamentoSMS");
        }
        #endregion

        #region AjustarURLdeOrigem
        /// <summary>
        /// Método responsável por identificar a url que chamou a página atual.
        /// </summary>
        private void AjustarURLdeOrigem()
        {
            if (Request.UrlReferrer != null)
            {
                if (string.IsNullOrEmpty(UrlOrigem))
                    UrlOrigem = Request.UrlReferrer.AbsoluteUri.ToString();
            }
        }
        #endregion

        #region CarregarParametrosEAjustarQuantidadeSMS
        /// <summary>
        /// Método responsável por ajustar a quantidade mínima de sms que pode ser adquirida
        /// </summary>
        private void CarregarParametrosEAjustarQuantidadeSMS()
        {
            var parametros = new List<Enumeradores.Parametro>
            {
                Enumeradores.Parametro.ValorSMSAvulso, 
                //Enumeradores.Parametro.CodigoPlanoSMS, 
                Enumeradores.Parametro.QuantidadeSMSMinima, 
                Enumeradores.Parametro.QuantidadeSMSMaxima
            };

            Dictionary<Enumeradores.Parametro, string> valoresParametros = Parametro.ListarParametros(parametros);

            txtQuantidadeSMS.Valor = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.QuantidadeSMSMinima]);
            QuantidadeSMSMinima = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.QuantidadeSMSMinima]);
            QuantidadeSMSMaxima = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.QuantidadeSMSMaxima]);

            //base.PagamentoIdentificadorPlano.Value = Convert.ToInt32(valoresParametros[Enumeradores.Parametro.CodigoPlanoSMS]);
            ValorSMSAvulso = Convert.ToDecimal(valoresParametros[Enumeradores.Parametro.ValorSMSAvulso]);

            //txtQuantidadeSMS.MensagemErroIntervalo = string.Empty;
            //txtQuantidadeSMS.ValorMinimo = QuantidadeSMSMinima;
            //txtQuantidadeSMS.ValorMaximo = QuantidadeSMSMaxima;
        }
        #endregion

        #region AjustarValorSMS
        /// <summary>
        /// Ajusta o texto do label do valor do SMS
        /// </summary>
        /// <param name="valorSMS"></param>
        private void AjustarLabelValorSMS(decimal valorSMS)
        {
            lblValorSMS.Text = String.Format("R$ {0}", valorSMS.ToString("N2"));
        }
        #endregion

        #region CalcularValorPlano
        private decimal CalcularValorPlano()
        {
            if (!txtQuantidadeSMS.Valor.HasValue)
                return decimal.Zero;

            return Convert.ToInt32(txtQuantidadeSMS.Valor) * ValorSMSAvulso;
        }
        #endregion

        #region AjustarValorTotal
        /// <summary>
        /// Ajusta o a textbox com o valor total o controle do pagamento.
        /// </summary>
        /// <param name="valorTotalPlano"></param>
        /// <param name="quantidadeAdquirida"></param>
        private void AjustarValorTotal(decimal valorTotalPlano, int quantidadeAdquirida)
        {
            lblValorSMSTotal.Text = valorTotalPlano.ToString("N2");

            //ucFormaPagamento.ValorPlanoSMS = valorTotalPlano;
            //ucFormaPagamento.ValorQuantidadeSMS = quantidadeAdquirida;

            upValorTotal.Update();
        }
        #endregion

        #region ValidarValoresInformados
        private void ValidarValoresInformados()
        {
            txtQuantidadeSMS.ValorMinimo = Decimal.Zero;
            txtQuantidadeSMS.ValorMaximo = 999999999M;
            if (txtQuantidadeSMS.Valor < QuantidadeSMSMinima)
            {
                txtQuantidadeSMS.ValorMinimo = QuantidadeSMSMinima;
                txtQuantidadeSMS.ValorMaximo = 999999999M;
                txtQuantidadeSMS.MensagemErroIntervalo = String.Format("Quantidade mínima é de {0}", QuantidadeSMSMinima);
            }
            else if (txtQuantidadeSMS.Valor > QuantidadeSMSMaxima)
            {
                txtQuantidadeSMS.ValorMinimo = Decimal.Zero;
                txtQuantidadeSMS.ValorMaximo = QuantidadeSMSMaxima;
                txtQuantidadeSMS.MensagemErroIntervalo = String.Format("Quantidade máxima é de {0}", QuantidadeSMSMaxima);
            }
        }
        #endregion

        #endregion

    }
}