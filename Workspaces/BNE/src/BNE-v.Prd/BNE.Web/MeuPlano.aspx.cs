using System;
using BNE.Web.Code;
using BNE.Web.Code.Enumeradores;
using BNE.Web.Code.Session;
using Enumeradores = BNE.BLL.Enumeradores;
using System.Web.UI;
using BNE.BLL;

namespace BNE.Web
{
    public partial class MeuPlano : BasePage
    {

        #region Propriedades

        #region UrlOrigem - Variável 1
        /// <summary>
        /// </summary>
        public string UrlOrigem
        {
            get
            {
                if (ViewState[Chave.Temporaria.Variavel1.ToString()] != null)
                    return (ViewState[Chave.Temporaria.Variavel1.ToString()]).ToString();
                return null;
            }
            set
            {
                if (value != null)
                    ViewState.Add(Chave.Temporaria.Variavel1.ToString(), value);
                else
                    ViewState.Remove(Chave.Temporaria.Variavel1.ToString());
            }
        }
        #endregion


        #region IdPlano
        /// <summary>
        /// Propriedade que armazena e recupera o IdPlano
        /// </summary>
        protected int? IdPlano
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

        #region IdConteudoHTML
        /// <summary>
        /// Propriedade que armazena e recupera o IdConteudoHTML
        /// </summary>
        protected int? IdConteudoHTML
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

        #endregion

        #region Eventos

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                Inicializar();

            InicializarBarraBusca(TipoBuscaMaster.Curriculo, false, "MeuPlano");
        }
        #endregion

        #region btn_EuQuero_Click
        protected void btn_EuQuero_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            IdConteudoHTML =  (int)Enumeradores.ConteudoHTML.ConteudoTelaFormaPagamentoEmpresaAcessoIlimitado;
            IdPlano = Convert.ToInt32(Parametro.RecuperaValorParametro(Enumeradores.Parametro.CodigoPlanoEmpresa));

            Redirect("FormaPagamento.aspx");
        }
        #endregion

        #region btnVoltar_Click
        protected void btnVoltar_Click(object sender, EventArgs e)
        {
            if (base.UrlDestinoPagamento.HasValue)
            {
                string paginaRedirect = base.UrlDestinoPagamento.Value;
                base.UrlDestinoPagamento.Clear();
                Redirect(paginaRedirect);
            }
            else
            {
                if (!string.IsNullOrEmpty(UrlOrigem))
                    Redirect(UrlOrigem);
                else
                    Redirect("Default.aspx");
            }
        }
        #endregion

        #region lnkImprimirProposta_Click
        protected void lnkImprimirProposta_Click(object sender, EventArgs e)
        {
            string serverName = Request.ServerVariables["HTTP_HOST"];
            ScriptManager.RegisterStartupScript(this, GetType(), "AbrirPopup", string.Format("AbrirPopup('http://{0}/ImprimirProposta.aspx', 600, 800);", serverName), true);
        }
        #endregion

        #endregion

        #region Metodos

        #region Inicializar
        public void Inicializar()
        {
            AjustarTituloTela("Meu Plano");
            ExibirMenuSecaoEmpresa();

            if (Request.UrlReferrer != null)
                UrlOrigem = Request.UrlReferrer.AbsoluteUri;
        }
        #endregion

        #endregion
    }
}