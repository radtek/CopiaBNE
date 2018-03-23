using System;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Chat.Core;
using BNE.Chat.Helper;

namespace BNE.Chat.UserControls
{
    public partial class RenderizarChat : UserControl
    {
        #region [ Const ]
        private const string ChaveDeSessaoControlePermissao = "RenderizarChatStatus";

        public enum ChangeRenderState
        {
            Impossible,
            NextLoad,
            Updated
        }
        #endregion

        #region [ Page Load ]
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #endregion

        public string UrlAplicacao { get; } = ConfigurationManager.AppSettings["urlSite"];

        #region [ Static ]
        private static readonly SetValueOrDefault<string> _pathDefinitionToRenderJs = new SetValueOrDefault<string>();
        public static SetValueOrDefault<string> PathDefinitionToRenderJs
        {
            get { return _pathDefinitionToRenderJs; }
        }

        private static readonly SetValueOrDefault<string> _pathDefinitionToRenderCss = new SetValueOrDefault<string>();
        public static SetValueOrDefault<string> PathDefinitionToRenderCss
        {
            get { return _pathDefinitionToRenderCss; }
        }

        private static readonly SetValueOrDefault<string> _pathDefinitionToRenderImg = new SetValueOrDefault<string>();
        public static SetValueOrDefault<string> PathDefinitionToRenderImg
        {
            get { return _pathDefinitionToRenderImg; }
        }

        private static readonly SetValueOrDefault<string> _pathDefinitionToTargetLink = new SetValueOrDefault<string>();
        public static SetValueOrDefault<string> PathDefinitionToTargetLink
        {
            get { return _pathDefinitionToTargetLink; }
        }

        private static readonly SetValueOrDefault<string> _pathDefinitionToRenderContactThumb = new SetValueOrDefault<string>();
        public static SetValueOrDefault<string> PathDefinitionToRenderContactThumb
        {
            get { return _pathDefinitionToRenderContactThumb; }
        }

        public static ChangeRenderState TryChangeRenderChatStatusBySession(bool habilitado)
        {
            var context = HttpContext.Current;
            if (context == null)
                return ChangeRenderState.Impossible;

            if (context.Session == null)
                return ChangeRenderState.Impossible;

            context.Session[ChaveDeSessaoControlePermissao] = habilitado;

            var controle = (context.CurrentHandler ?? context.Handler) as TemplateControl;

            if (controle == null)
                return ChangeRenderState.NextLoad;

            var page = controle as Page;

            Panel panelChat;
            if (page == null)
            {
                panelChat = controle.FindControl("pnlPrincipalChat") as Panel;
            }
            else
            {
                if (page.Master == null)
                    panelChat = page.FindControl("pnlPrincipalChat") as Panel;
                else
                    panelChat = page.Master.FindControl("pnlPrincipalChat") as Panel;
            }

            if (panelChat == null || panelChat.Parent == null)
                return ChangeRenderState.NextLoad;

            var chat = panelChat.Parent is RenderizarChat
                ? panelChat.Parent as RenderizarChat
                : panelChat.Parent.Parent as RenderizarChat;

            if (chat == null)
                return ChangeRenderState.NextLoad;

            chat.ForceUpdateRender();
            return ChangeRenderState.Updated;
        }
        #endregion

        #region [ Properties ]
        public override bool Visible
        {
            get
            {
                return EnabledToRender;
            }
            set
            {
                base.Visible = value;
            }
        }

        public virtual string PathToRenderJs
        {
            get
            {
                if (!PathDefinitionToRenderJs.IsSet)
                {
                    return TemplateSourceDirectory + "/js";
                }

                if (PathDefinitionToRenderJs.Value == null)
                    return "";

                if (PathDefinitionToRenderJs.Value.EndsWith("/"))
                {
                    return PathDefinitionToRenderJs.Value.Substring(0, PathDefinitionToRenderJs.Value.Length - 1);
                }
                return PathDefinitionToRenderJs.Value;
            }
        }

        public virtual string PathToRenderCss
        {
            get
            {
                if (!PathDefinitionToRenderCss.IsSet)
                {
                    return TemplateSourceDirectory + "/css";
                }

                if (PathDefinitionToRenderCss.Value == null)
                    return "";

                if (PathDefinitionToRenderCss.Value.EndsWith("/"))
                {
                    return PathDefinitionToRenderCss.Value.Substring(0, PathDefinitionToRenderCss.Value.Length - 1);
                }
                return PathDefinitionToRenderCss.Value;
            }
        }

        public virtual string PathToRenderThumb
        {
            get { return PathDefinitionToRenderContactThumb.Value; }
        }

        public virtual string PathToRenderImg
        {
            get
            {
                if (!PathDefinitionToRenderImg.IsSet)
                {
                    return TemplateSourceDirectory + "/img";
                }

                if (PathDefinitionToRenderImg.Value == null)
                    return "";

                if (PathDefinitionToRenderImg.Value.EndsWith("/"))
                {
                    return PathDefinitionToRenderImg.Value.Substring(0, PathDefinitionToRenderImg.Value.Length - 1);
                }
                return PathDefinitionToRenderImg.Value;
            }
        }

        public virtual string PathToTargetLink
        {
            get
            {
                if (!PathDefinitionToTargetLink.IsSet)
                    return "";

                if (PathDefinitionToTargetLink.Value == null)
                    return "";

                if (PathDefinitionToTargetLink.Value.EndsWith("/"))
                    return PathDefinitionToTargetLink.Value.Substring(0, PathDefinitionToTargetLink.Value.Length - 1);

                return PathDefinitionToTargetLink.Value;
            }
        }

        public virtual string DashboardLink
        {
            get
            {
                return ChatService.Instance.ChatConsumer.GetDashboardLink();
            }
        }

        public virtual string UsuarioFilialPerfilLogado
        {
            get
            {
                return ChatService.Instance.ChatConsumer.GetUsuarioFilialPerfil().ToString();
                //return Session["IdUsuarioFilialPerfilLogadoEmpresa"].ToString();
                //session[typeof(SessionVariable<int>) + Chave.Permanente.IdUsuarioFilialPerfilLogadoEmpresa.ToString()];
            }
        }

        public virtual bool EnabledToRender
        {
            get
            {
                if (Session == null)
                    return base.Visible;

                var obj = Session[ChaveDeSessaoControlePermissao];
                if (obj == null)
                    return base.Visible && ChatService.SimpleSecurity.Evaluate(this.Context);

                bool value;
                if (bool.TryParse(obj.ToString(), out value))
                {
                    return base.Visible && value;
                }
                return base.Visible && ChatService.SimpleSecurity.Evaluate(this.Context);
            }
            protected set
            {
                var ultimoValor = EnabledToRender;
                try
                {
                    if (Session == null)
                    {
                        Visible = value;
                        return;
                    }

                    Session[ChaveDeSessaoControlePermissao] = value;
                }
                finally
                {
                    if (ultimoValor != value)
                        UpdatePermission(value);
                }
            }
        }
        #endregion

        #region [ Public Methods ]
        private void UpdatePermission(bool permitirChat)
        {
            if (permitirChat)
            {
                if (!Visible)
                {
                    Visible = true;
                }
            }
            pnlPrincipalChat.Visible = permitirChat;
            UpdateComponent();
        }

        public void ForceUpdateRender()
        {
            pnlPrincipalChat.Visible = EnabledToRender;
            UpdateComponent();
        }
        #endregion

        #region [ Private/Protected Methods ]
        protected override void OnPreRender(EventArgs e)
        {
            if (pnlPrincipalChat != null)
                pnlPrincipalChat.Visible = EnabledToRender;
            base.OnPreRender(e);
        }

        private void UpdateComponent()
        {
            if (upPrincipalChat.UpdateMode == UpdatePanelUpdateMode.Conditional)
                upPrincipalChat.Update();
        }
        #endregion
    }
}
