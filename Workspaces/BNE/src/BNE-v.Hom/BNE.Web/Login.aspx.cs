using System;
using BNE.Web.Code;
using BNE.BLL.Custom;
using BNE.Auth;
using System.Configuration;
using System.Web;
using JSONSharp;

namespace BNE.Web
{
    public partial class Login : BasePage
    {
        public string RedirectParamArg
        {
            get
            {
                return ConfigurationManager.AppSettings["aspnet:FormsAuthReturnUrlVar"] ?? "ReturnUrl";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ucModalLogin.Fechar += ucModalLogin_Fechar;

            if (IsPostBack)
                return;

            if (Request.QueryString["Deslogar"] != null)
            {
                bool deslogar;
                Boolean.TryParse(Request.QueryString["Deslogar"], out deslogar);
                if (deslogar)
                {
                    BNEAutenticacao.DeslogarPadrao();
                    Redirect("~/");
                    return;
                }
            }

            string redirectURL = PegarUrlParaRedirecionar();
            bool logarVagas = false;
            bool logarVip = false;
            if (Request.QueryString["LogarVagas"] != null)
            {
                Boolean.TryParse(Request.QueryString["LogarVagas"], out logarVagas);
            }
            else if (Request.QueryString["LogarVip"] != null)
            {
                Boolean.TryParse(Request.QueryString["LogarVip"], out logarVip);
            }

            ucModalLogin.Inicializar(logarVagas, logarVip, redirectURL);
            ucModalLogin.Mostrar();
            ucModalLogin.Fechar += ucModalLogin_Fechar;
        }

        private string PegarUrlParaRedirecionar()
        {
            var redirectURL = Request.QueryString["RedirectURL"];
            if (string.IsNullOrWhiteSpace(redirectURL))
            {
                redirectURL = Request.QueryString[RedirectParamArg];

                if (string.IsNullOrWhiteSpace(redirectURL))
                    redirectURL = Request.QueryString["ReferrerURL"] ?? string.Empty;
            }

            return redirectURL;
        }

        void ucModalLogin_Fechar()
        {
            string redirectURL = Request.QueryString["ReferrerURL"] ?? string.Empty;
            if (string.IsNullOrWhiteSpace(redirectURL))
            {
                Redirect("~/");
            }
            else
            {
                Redirect(redirectURL);
            }

        }

        public void ExibirMensagem(string mensagem, Code.Enumeradores.TipoMensagem tipo, bool aumentarTamanhoPainelMensagem = false)
        {
            lblAviso.Text = mensagem;
            switch (tipo)
            {
                case Code.Enumeradores.TipoMensagem.Aviso:
                    lblAviso.CssClass = "texto_avisos_padrao";
                    break;
                case Code.Enumeradores.TipoMensagem.Erro:
                    lblAviso.CssClass = "texto_avisos_erro";
                    break;
            }
            updAviso.Update();

            if (String.IsNullOrEmpty(mensagem))
                System.Web.UI.ScriptManager.RegisterStartupScript(updAviso, updAviso.GetType(), "OcultarAviso", "javaScript:OcultarAviso();", true);
            else
            {
                var parametros = new
                {
                    aumentarPainelMensagem = aumentarTamanhoPainelMensagem
                };
                System.Web.UI.ScriptManager.RegisterStartupScript(updAviso, updAviso.GetType(), "ExibirAviso", "javaScript:ExibirAviso(" + new JSONReflector(parametros) + ");", true);
            }
        }
    }
}