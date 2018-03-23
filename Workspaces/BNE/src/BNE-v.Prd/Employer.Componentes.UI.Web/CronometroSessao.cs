// -----------------------------------------------------------------------
// <copyright file="CronometroSessao.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Employer.Componentes.UI.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI.WebControls;
    using System.Web.UI;
    using System.ServiceModel;
    using System.ServiceModel.Activation;
    using System.Web;
    using System.Web.SessionState;
    using System.Configuration;
    using Employer.Componentes.UI.Web.Extensions;
    using System.Web.Script.Serialization;

    /// <summary>
    /// Componente abre uma modal avisando ao usuário que sua sessão está perto de expirar.
    /// Caso o tempo termine ele clica no botão sair.
    /// <remarks>Este componente não está estável. Não recomendo usar</remarks>
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceContract(Namespace = "")]
    public class CronometroSessao : AjaxClientControlBase
    {
        private UpdatePanel _upAtualiza = new UpdatePanel { ID = "upAtualiza", UpdateMode= UpdatePanelUpdateMode.Conditional };
        private TextBox _txtTempo = new TextBox { ID = "txtTempo" };
        private EmployerModalConfirmacao _Modal = new EmployerModalConfirmacao { ID = "empModal" };
        private System.Web.UI.WebControls.Button _btnFechar = new System.Web.UI.WebControls.Button { ID = "btnFechar" };

        private HttpSessionState Session { get { return HttpContext.Current.Session; } }

        /// <summary>
        /// Evento disparado ao Terminar a sessão
        /// </summary>
        public event EventHandler FecharSessao;

        #region Propriedades
        /// <summary>
        /// Tempo em minutos para disparar aviso de termino de sessão
        /// </summary>
        public int TempoAviso
        {
            get { return ViewState["TempoAviso"] != null ? (int)ViewState["TempoAviso"] : 5; }
            set { ViewState["TempoAviso"] = value; }
        }

        /// <summary>
        /// Classe css do cronômetro
        /// </summary>
        public string CssCronometro
        {
            get
            {
                EnsureChildControls();
                return _txtTempo.CssClass;
            }

            set
            {
                EnsureChildControls();
                _txtTempo.CssClass = value;
            }
        }

        /// <summary>
        /// Indica se o campo texto do cronômetro está visível
        /// </summary>
        public bool CronometroVisivel
        {
            get
            {
                EnsureChildControls();
                return !(_txtTempo.Style[HtmlTextWriterStyle.Display] != null &&
                    _txtTempo.Style[HtmlTextWriterStyle.Display].Equals("none", StringComparison.InvariantCultureIgnoreCase));
            }
            set
            {
                EnsureChildControls();
                if (value)
                    _txtTempo.Style.Remove(HtmlTextWriterStyle.Display);
                else
                    _txtTempo.Style[HtmlTextWriterStyle.Display] = "none";
            }
        }

        /// <summary>
        /// Urls que serão ignoradas no contador de tempo.<br/>
        /// A cada chamada p/ o servidor o contador é zerado exceto as urls que foram informadas nesta propriedade.
        /// </summary>
        public List<string> UrlsIgnorar
        {
            get
            {
                if (ViewState["UrlsIgnorar"] == null)
                    ViewState["UrlsIgnorar"] = new List<string>();
                return ViewState["UrlsIgnorar"] as List<string>;
            }

            set { ViewState["UrlsIgnorar"]  = value; }
        }

        /// <summary>
        /// Título de modal
        /// </summary>
        public string TituloModal
        {
            get 
            {
                EnsureChildControls();
                return _Modal.Titulo;
            }
            set 
            {
                EnsureChildControls();
                _Modal.Titulo = value; 
            }
        }

        /// <summary>
        /// Texto da modal
        /// </summary>
        public string TextoModal
        {
            get { return ViewState["TextoModal"] != null ? (string)ViewState["TextoModal"] : "Falta {0} para expirar a sessão!!<br/>Deseja renovar a sessão?"; }
            set { ViewState["TextoModal"] = value; }
        }

        /// <summary>
        /// Imagem localizada a esquerda da modal.
        /// </summary>
        public string ImagemModal
        {
            get
            {
                EnsureChildControls();
                return _Modal.ImagemModal;
            }
            set
            {
                EnsureChildControls();
                _Modal.ImagemModal = value;
            }
        }
        #endregion

        /// <inheritdoc/>
        protected override void OnInit(EventArgs e)
        {
            _btnFechar.Click +=new EventHandler(_btnFechar_Click);
            base.OnInit(e);
        }

        void  _btnFechar_Click(object sender, EventArgs e)
        {
 	        if (FecharSessao != null)
                FecharSessao(sender, e);
        }

        /// <inheritdoc/>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            _btnFechar.Style[HtmlTextWriterStyle.Display] = "none";

            _Modal.Titulo = "Sua sessão está para expirar!!";
            _Modal.TextoBotaoConfirmar = "Sim";
            _Modal.BotaoConfirmarVisivel = true;
            _Modal.BotaoCancelarVisivel = false;
            _Modal.HabilitarBotaoFechar = true;

            this.Controls.Add(_upAtualiza);
            this.Controls.Add(_Modal);
            this.Controls.Add(_btnFechar);
                        
            _txtTempo.Enabled = false;
            _upAtualiza.ContentTemplateContainer.Controls.Add(_txtTempo);            
        }

        /// <inheritdoc/>
        protected override void Render(HtmlTextWriter writer)
        {
            _Modal.TextoModal = string.Format(TextoModal, TempoAviso);

            base.Render(writer);
        }

        #region AjaxClientControlBase
        /// <inheritdoc/>
        public override IEnumerable<System.Web.UI.ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("Employer.Componentes.UI.Web.CronometroSessao", this.ClientID);
            this.SetScriptDescriptors(descriptor);

            descriptor.AddProperty("CampoId", _txtTempo.ClientID);
            descriptor.AddProperty("Tempo_Aviso", this.TempoAviso);
            descriptor.AddProperty("Tempo_Sessao", Session.Timeout);
            descriptor.AddProperty("IdModal", _Modal.ClientBehaviorID);
            descriptor.AddProperty("IdLabelModal", _Modal.ClientIdlblMensagemModal);
            descriptor.AddProperty("IdBtnFechar", _btnFechar.ClientID);
            descriptor.AddProperty("UrlsIgnorar", new JavaScriptSerializer().Serialize(UrlsIgnorar));
            descriptor.AddProperty("TextoModal", TextoModal);

            return new ScriptControlDescriptor[] { descriptor };
        }

        /// <inheritdoc />
        public override IEnumerable<System.Web.UI.ScriptReference> GetScriptReferences()
        {
            IList<ScriptReference> references = new List<ScriptReference>(20);
            base.SetScriptReferences(references);

            ScriptReference reference = new ScriptReference();
            reference.Assembly = "Employer.Componentes.UI.Web";
            reference.Name = "Employer.Componentes.UI.Web.Content.js.CronometroSessao.js";
            references.Add(reference);

            return references;
        }
        #endregion
    }
}
