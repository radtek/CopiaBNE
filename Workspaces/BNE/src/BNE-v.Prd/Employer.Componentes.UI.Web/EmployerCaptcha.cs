using Employer.Componentes.UI.Web.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Componente de Captcha.<br/>
    /// Para usar é necessário adinionar no web.config:<br/>
    /// <code language="xml">
    ///     &lt;appSettings&gt;
    ///        &lt;add key=&quot;Employer.Componentes:HandlerCaptch&quot; value=&quot;Componentes/HandlerCaptch&quot;/&gt;
    ///     &lt;/appSettings&gt;
    ///     &lt;system.webServer&gt;
    ///         &lt;handlers&gt;
    ///             &lt;add name=&quot;EmployerHandlerCaptch&quot; verb=&quot;GET&quot; path=&quot;Componentes/HandlerCaptch&quot; type=&quot;Employer.Componentes.UI.Web.Handlers.HandlerCaptch,Employer.Componentes.UI.Web&quot;/&gt;
    ///         &lt;/handlers&gt;
    ///     &lt;/system.webServer&gt;
    ///  </code>
    /// </summary>
    public class EmployerCaptcha : CompositeControl
    {
        #region Atributos
        Image _imgCaptcha = new Image { ID = "imgCaptcha" };
        ControlAlfaNumerico _txtCaptcha = new ControlAlfaNumerico {
            ID = "txtCaptcha",
            Tipo = ControlBaseTextBox.TipoAlfaNumerico.Numeros,
            Tamanho = 6,
            ValorMinimo = "100000"
        };
        #endregion

        #region Propriedades
        #region PublicCode
        internal string PublicCode
        {
            get { return ViewState["PublicCode"] as string; }
            set { ViewState["PublicCode"] = value; }
        }
        #endregion

        #region Image
        public Unit ImageWidth
        {
            get { EnsureChildControls(); return _imgCaptcha.Width; }
            set { EnsureChildControls(); _imgCaptcha.Width = value; }
        }

        public Unit ImageHeight
        {
            get { EnsureChildControls(); return _imgCaptcha.Height; }
            set { EnsureChildControls(); _imgCaptcha.Height = value; }
        }

        [CssClassProperty]
        public string ImageCssClass
        {
            get { EnsureChildControls(); return _imgCaptcha.CssClass; }
            set { EnsureChildControls(); _imgCaptcha.CssClass = value; }
        }
        #endregion

        #region TextBox
        public Unit TextBoxWidth
        {
            get { EnsureChildControls(); return _txtCaptcha.Width; }
            set { EnsureChildControls(); _txtCaptcha.Width = value; }
        }

        public bool Obrigatorio
        {
            get { EnsureChildControls(); return _txtCaptcha.Obrigatorio; }
            set { EnsureChildControls(); _txtCaptcha.Obrigatorio = value; }
        }

        public Unit TextBoxHeight
        {
            get { EnsureChildControls(); return _txtCaptcha.Height; }
            set { EnsureChildControls(); _txtCaptcha.Height = value; }
        }

        public string ValidationGroup
        {
            get { EnsureChildControls(); return _txtCaptcha.ValidationGroup; }
            set { EnsureChildControls(); _txtCaptcha.ValidationGroup = value; }
        }

        [CssClassProperty]
        public string TextCssClass
        {
            get { EnsureChildControls(); return _txtCaptcha.CssClass; }
            set { EnsureChildControls(); _txtCaptcha.CssClass = value; }
        }

        public string TextBoxMascaraMensagemErroMinimoSummary
        {
            get { EnsureChildControls(); return _txtCaptcha.MascaraMensagemErroMinimoSummary; }
            set { EnsureChildControls(); _txtCaptcha.MascaraMensagemErroMinimoSummary = value; }
        }

        public string TextBoxMascaraMensagemErroMinimo
        {
            get { EnsureChildControls(); return _txtCaptcha.MascaraMensagemErroMinimo; }
            set { EnsureChildControls(); _txtCaptcha.MascaraMensagemErroMinimo = value; }
        }

        public string TextBoxMensagemErroObrigatorio
        {
            get { EnsureChildControls(); return _txtCaptcha.MensagemErroObrigatorio; }
            set { EnsureChildControls(); _txtCaptcha.MensagemErroObrigatorio = value; }
        }

        public string TextBoxMensagemErroObrigatorioSummary
        {
            get { EnsureChildControls(); return _txtCaptcha.MensagemErroObrigatorioSummary; }
            set { EnsureChildControls(); _txtCaptcha.MensagemErroObrigatorioSummary = value; }
        }

        public string TextBoxMensagemErroFormato
        {
            get { EnsureChildControls(); return _txtCaptcha.MensagemErroFormato; }
            set { EnsureChildControls(); _txtCaptcha.MensagemErroFormato = value; }
        }

        public string TextBoxMensagemErroFormatoSummary
        {
            get { EnsureChildControls(); return _txtCaptcha.MensagemErroFormatoSummary; }
            set { EnsureChildControls(); _txtCaptcha.MensagemErroFormatoSummary = value; }
        }
        #endregion

        #endregion

        /// <inheritdoc/>
        protected override void CreateChildControls()
        {
            this.Controls.Add(_imgCaptcha);
            this.Controls.Add(_txtCaptcha);

            base.CreateChildControls();
        }

        /// <summary>
        /// Valida o texto digitado com a imagem do Captcha
        /// </summary>
        /// <returns></returns>
        public bool ValidarCaptcha()
        {
            return ValidarCaptchaEncript(PublicCode, _txtCaptcha.Text);
        }

        /// <summary>
        /// Troca a imagem do Captcha
        /// </summary>
        public void TrocarCaptcha()
        {
            GeraCaptchaText();
        }

        internal static bool ValidarCaptchaEncript(string s, string sTexto)
        {
            try
            {
                var code = new CryptUtil().ActionDecrypt(s);
                var parans = code.Split('&');
                var horario = DateTime.FromFileTime(long.Parse(parans[0]));
                if (horario.Add(new TimeSpan(24, 0, 0)) < DateTime.Now)
                    return false; //throw new SecurityException("Token expirou");

                return parans[1] == sTexto;
            }            
            catch { }

            return false;
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (string.IsNullOrEmpty(PublicCode))
            {
                GeraCaptchaText();
            }
        }

        private void GeraCaptchaText()
        {
            Random oRandom = new Random();
            int iNumber = oRandom.Next(100000, 999999);
            var pKey = string.Format("{0}&{1}", DateTime.Now.Ticks, iNumber);
            PublicCode = new CryptUtil().ActionEncrypt(pKey);

            _imgCaptcha.ImageUrl = ConfigurationManager.AppSettings["Employer.Componentes:HandlerCaptch"]
                    + "?CaptchaCode=" + PublicCode;
        }
    }
}
