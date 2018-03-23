// -----------------------------------------------------------------------
// <copyright file="EmployerModalConfirmacao.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Employer.Componentes.UI.Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Web.UI.HtmlControls;
    using System.ComponentModel;
    using System.Drawing.Design;

    /// <summary>
    /// Modal de confirmação
    /// </summary>
    public class EmployerModalConfirmacao : ControlModal
    {
        #region Atributos
        private bool _Atualizar = false;

        UpdatePanel upnModal = new UpdatePanel { ID = "upnModal", UpdateMode = UpdatePanelUpdateMode.Conditional };
        Image icnAtencao = new Image { ID = "icnAtencao" };
        Label lblMensagem = new Label { ID = "lblMensagem" };
        HtmlGenericControl pLinha = new HtmlGenericControl { ID = "pLinha", TagName = "p" };
        Panel divConteudo = new Panel { ID = "divConteudo" };
        Panel divBotoes = new Panel { ID = "divBotoes" };
        Employer.Componentes.UI.Web.Button btnConfirmar = new Employer.Componentes.UI.Web.Button { ID = "btnConfirmar", CausesValidation = false, ToolTip="Confirmar", Text="Confirmar" };
        Employer.Componentes.UI.Web.Button btnCancelar = new Employer.Componentes.UI.Web.Button { ID = "btnCancelar", CausesValidation = false, ToolTip="Cancelar", Text="Cancelar"};
        #endregion

        #region Eventos
        /// <summary>
        /// Evento disparado ao clicar no botão confirmar
        /// </summary>
        public event EventHandler Confirmar;

        /// <summary>
        /// Evento disparado ao clicar no botão cancelar
        /// </summary>
        public event EventHandler Cancelar;
        #endregion

        #region Propriedades

        internal string ClientIdlblMensagemModal
        {
            get 
            {
                EnsureChildControls();
                return lblMensagem.ClientID;
            }
        }

        /// <summary>
        /// Indica se o botão cancelar está visível
        /// </summary>
        public bool BotaoCancelarVisivel
        {
            get 
            {
                EnsureChildControls();
                return btnCancelar.Visible;
            }
            set
            {
                EnsureChildControls();
                btnCancelar.Visible = value;
            }
        }

        /// <summary>
        /// Indica se o botão confirmar está visível
        /// </summary>
        public bool BotaoConfirmarVisivel
        {
            get
            {
                EnsureChildControls();
                return btnConfirmar.Visible;
            }
            set
            {
                EnsureChildControls();
                btnConfirmar.Visible = value;
            }
        }

        /// <summary>
        /// Texto do botão cancelar
        /// </summary>
        public string TextoBotaoCancelar
        {
            get
            {
                EnsureChildControls();
                return btnCancelar.Text;
            }
            set
            {
                EnsureChildControls();
                btnCancelar.Text = value;

                _Atualizar = true;
            }
        }

        /// <summary>
        /// Texto do botão confirmar
        /// </summary>
        public string TextoBotaoConfirmar
        {
            get
            {
                EnsureChildControls();
                return btnConfirmar.Text;
            }
            set
            {
                EnsureChildControls();
                btnConfirmar.Text = value;

                _Atualizar = true;
            }
        }

        /// <summary>
        /// Texto da modal
        /// </summary>
        public string TextoModal
        {
            get 
            {
                EnsureChildControls();
                return lblMensagem.Text; 
            }
            set
            {
                EnsureChildControls();
                lblMensagem.Text = value;

                _Atualizar = true;
            }
        }

        /// <summary>
        /// Imagem localizada a esquerda.
        /// </summary>
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [UrlProperty]
        [Bindable(true)]
        public string ImagemModal
        {
            get
            {
                EnsureChildControls();
                return icnAtencao.ImageUrl;
            }
            set
            {
                EnsureChildControls();
                icnAtencao.ImageUrl = value;
            }
        }

        /// <summary>
        /// Habilita o botão fechar
        /// </summary>
        public bool HabilitarBotaoFechar
        {
            get { return this.ViewState["HabilitarBotaoFechar"] != null ? (bool)this.ViewState["HabilitarBotaoFechar"] : false; }
            set { this.ViewState["HabilitarBotaoFechar"] = value; }
        }

        #region CSS

        /// <summary>
        /// Classe css da imagem localizada a esquerda.
        /// </summary>
        [CssClassProperty]
        [DefaultValue("")]
        public string ImagemCssClass
        {
            get
            {
                EnsureChildControls();
                return icnAtencao.CssClass;
            }
            set
            {
                EnsureChildControls();
                icnAtencao.CssClass = value;
            }
        }

        /// <summary>
        /// Classe css da linha de menságem
        /// </summary>
        [CssClassProperty]
        [DefaultValue("")]
        public string LinhaCssClass
        {
            get
            {
                EnsureChildControls();
                return pLinha.Attributes["class"];
            }
            set
            {
                EnsureChildControls();
                pLinha.Attributes["class"] = value;
            }
        }

        /// <summary>
        /// Classe css da linha de botões
        /// </summary>
        [CssClassProperty]
        [DefaultValue("")]
        public string LinhaBotoesCssClass
        {
            get
            {
                EnsureChildControls();
                return divBotoes.CssClass;
            }
            set
            {
                EnsureChildControls();
                divBotoes.CssClass = value;
            }
        }

        /// <summary>
        /// Classe css da div de conteúdo
        /// </summary>
        [CssClassProperty]
        [DefaultValue("")]
        public string LinhaTextoCssClass
        {
            get
            {
                EnsureChildControls();
                return divConteudo.CssClass;
            }
            set
            {
                EnsureChildControls();
                divConteudo.CssClass = value;
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region Criar o conteúdo

        private void InicializaBotoes()
        {
            upnModal.ContentTemplateContainer.Controls.Add(divBotoes);
            divBotoes.Controls.Add(btnConfirmar);
            divBotoes.Controls.Add(btnCancelar);
        }

        private void InicializaConteudo()
        {
            upnModal.ContentTemplateContainer.Controls.Add(divConteudo);
            
            divConteudo.Controls.Add(icnAtencao);
            divConteudo.Controls.Add(pLinha);

            pLinha.Controls.Add(lblMensagem);
        }

        /// <inheritdoc/>
        protected override void CreateModalContent(System.Web.UI.WebControls.Panel objPanel)
        {
            objPanel.Controls.Add(upnModal);
                        
            InicializaConteudo();
            InicializaBotoes();
        }

        #endregion

        #region Eventos
        /// <inheritdoc/>
        protected override void OnInit(EventArgs e)
        {
            this.btnConfirmar.Click += new EventHandler(btnConfirmar_Click);
            this.btnCancelar.Click += new EventHandler(btnCancelar_Click);

            base.OnInit(e);
        }

        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (this.HabilitarBotaoFechar)
                BotaoFechar.Style.Remove(HtmlTextWriterStyle.Display);
            else
                BotaoFechar.Style[HtmlTextWriterStyle.Display] = "none";
        }

        /// <inheritdoc/>
        protected override void OnPreRender(EventArgs e)
        {
            if (_Atualizar)
                upnModal.Update();

            base.OnPreRender(e);
        }

        void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();

            if (Cancelar != null)
                Cancelar(sender, e);
        }

        void btnConfirmar_Click(object sender, EventArgs e)
        {
            this.Close();

            if (Confirmar != null)
                Confirmar(sender, e);
        }
        #endregion

        #endregion
    }
}
