using System;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Web.UI;

namespace Employer.Componentes.UI.Web
{
    /// <summary>
    /// Controle base para os controles de Modal 
    /// </summary>
    public abstract class ControlModal : CompositeControl
    {
        #region Handlers
        /// <summary>
        /// Evento FecharModal
        /// </summary>
        public event EventHandler FecharModal;
        #endregion

        #region Campos
        //Utilizado somente para definir como default da modal
        private HiddenField _hfModal = new HiddenField() { ID = "hfModal" };

        private System.Web.UI.WebControls.Panel _pnlModal = new System.Web.UI.WebControls.Panel() { CssClass = "modal" };
        private System.Web.UI.WebControls.Label _lblTitulo = new System.Web.UI.WebControls.Label();
        private ImageButton _btnFechar = new ImageButton();
        private System.Web.UI.WebControls.Panel _pnlBotoesTopo = new System.Web.UI.WebControls.Panel();
        private ModalPopupExtender _Modal = new ModalPopupExtender();
        #endregion

        #region Propriedades
        /// <inheritdoc/>
        public string ClientBehaviorID
        {
            get 
            {
                EnsureChildControls();
                return _Modal.BehaviorID; 
            }
        }        

        ///
        public System.Web.UI.WebControls.Panel PanelModal
        {
            get
            {
                EnsureChildControls();
                return _pnlModal;
            }
        }

        #region Width
        /// <inheritdoc />
        public override Unit Width
        {
            get 
            {
                EnsureChildControls();
                return _pnlModal.Width; 
            }
            set 
            {
                EnsureChildControls();
                _pnlModal.Width = value; 
            }
        }
        #endregion

        #region Height
        /// <inheritdoc />
        public override Unit Height
        {
            get 
            {
                EnsureChildControls();
                return _pnlModal.Height; 
            }
            set 
            {
                EnsureChildControls();
                _pnlModal.Height = value; 
            }
        }
        #endregion

        #region CssClass
        /// <inheritdoc />
        public override string CssClass
        {
            get 
            {
                EnsureChildControls();
                return _pnlModal.CssClass; 
            }
            set 
            {
                EnsureChildControls();
                _pnlModal.CssClass = value; 
            }
        }
        #endregion

        #region TituloCss
        /// <summary>
        /// Css do título da modal 
        /// </summary>
        public virtual string TituloCss
        {
            get 
            {
                EnsureChildControls();
                return _lblTitulo.CssClass; 
            }
            set 
            {
                EnsureChildControls();
                _lblTitulo.CssClass = value; 
            }
        }
        #endregion

        #region BotaoFechar
        internal ImageButton BotaoFechar
        {
            get { return _btnFechar; }
        }
        #endregion

        #region BotaoFecharCss
        /// <summary>
        /// Classe Css do botão de fechar
        /// </summary>
        public virtual string BotaoFecharCss
        {
            get 
            {
                EnsureChildControls();
                return _btnFechar.CssClass; 
            }
            set 
            {
                EnsureChildControls();
                _btnFechar.CssClass = value; 
            }
        }
        #endregion

        #region BotaoFecharModal
        /// <summary>
        /// Deixa visível ou não o botão fechar
        /// </summary>
        public bool BotaoFecharModal
        {
            get
            {
                EnsureChildControls();
                return this._btnFechar.Visible;
            }
            set
            {
                EnsureChildControls();
                this._btnFechar.Visible = value;
            }
        }
        #endregion

        #region BackgroundCssClass
        /// <summary>
        /// Classe css de fundo da modal
        /// </summary>
        public virtual String BackgroundCssClass
        {
            get
            {
                EnsureChildControls();
                return this._Modal.BackgroundCssClass;
            }
            set
            {
                EnsureChildControls();
                this._Modal.BackgroundCssClass = value;
            }
        }
        #endregion

        #region BotaoFecharImageUrl
        /// <summary>
        /// Caminho da imagem do botão de limpar
        /// </summary>
        public virtual string BotaoFecharImageUrl
        {
            get 
            {
                EnsureChildControls();
                return _btnFechar.ImageUrl; 
            }
            set 
            {
                EnsureChildControls();
                _btnFechar.ImageUrl = value; 
            }
        }
        #endregion

        #region TargetControlID
        /// <summary>
        /// O controle alvo da modal
        /// </summary>
        public String TargetControlID
        {
            get
            {
                EnsureChildControls();
                return this._Modal.TargetControlID;
            }
            set
            {
                EnsureChildControls();
                this._Modal.TargetControlID = value;
            }
        }
        #endregion

        #region OkControlID
        /// <summary>
        /// Id do botão ok da modal
        /// </summary>
        public String OkControlID
        {
            get
            {
                EnsureChildControls();
                return this._Modal.OkControlID;
            }
            set
            {
                EnsureChildControls();
                this._Modal.OkControlID = value;
            }
        }
        #endregion

        #region CancelControlID
        /// <summary>
        /// Id do botão cancelar da modal
        /// </summary>
        public String CancelControlID
        {
            get
            {
                EnsureChildControls();
                return this._Modal.CancelControlID;
            }
            set
            {
                EnsureChildControls();
                this._Modal.CancelControlID = value;
            }
        }
        #endregion

        #region Titulo
        /// <summary>
        /// Título da modal
        /// </summary>
        public String Titulo
        {
            get
            {
                EnsureChildControls();
                return this._lblTitulo.Text;
            }
            set
            {
                EnsureChildControls();
                this._lblTitulo.Text = value;
            }
        }
        #endregion

        #endregion

        #region Métodos

        #region IniciaPainel
        /// <summary>
        /// Inicia o painel principal
        /// </summary>
        private void IniciaPainel()
        {
            _pnlModal.ID = "pnlModal";
            _pnlModal.Style[HtmlTextWriterStyle.Display] = "none";
            //_pnlModal.EnableTheming = false;            
        }
        #endregion

        #region IniciaLabel
        /// <summary>
        /// Inicia a label de título
        /// </summary>
        private void IniciaLabel()
        {
            _lblTitulo.ID = "Titulo";
            _lblTitulo.EnableTheming = false;
        }
        #endregion

        #region IniciaBotoes
        /// <summary>
        /// Inicia todos os botões
        /// </summary>
        private void IniciaBotoes()
        {
            _btnFechar.ID = "btnFechar";

            _btnFechar.EnableTheming = false;

            _btnFechar.CausesValidation = false;            
        }
        #endregion

        #region IniciaModal
        /// <summary>
        /// Inicia a modal 
        /// </summary>
        private void IniciaModal()
        {
            _Modal.PopupControlID = _pnlModal.ID;

            this.Controls.Add(_Modal);
        }
        #endregion

        #region Show
        /// <summary>
        /// Mostra a Modal
        /// </summary>
        public virtual void Show()
        {
            this._Modal.Show();
        }
        #endregion

        #region OnPreRender
        /// <inheritdoc/>
        protected override void OnPreRender(EventArgs e)
        {
            if (string.IsNullOrEmpty(_btnFechar.ImageUrl))
                _btnFechar.ImageUrl = Page.ClientScript.GetWebResourceUrl(
                    typeof(ControlModal), "Employer.Componentes.UI.Web.Content.Images.botao_padrao_fechar.png");

            base.OnPreRender(e);
        }
        #endregion

        #region Close
        /// <summary>
        /// Fecha a modal
        /// </summary>
        public virtual void Close()
        {
            this._Modal.Hide();
        }
        #endregion

        #region CreateModalContent
        /// <summary>
        /// Cria o conteúdo da modal 
        /// </summary>
        /// <param name="objPanel">O panel em que os controles devem ser inseridos</param>
        protected abstract void CreateModalContent(System.Web.UI.WebControls.Panel objPanel);
        #endregion

        #region CreateChildControls
        /// <inheritdoc/>        
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            this.Controls.Add(_hfModal);

            IniciaPainel();
            IniciaLabel();
            IniciaBotoes();
            IniciaModal();

            _pnlModal.Controls.Add(_lblTitulo);
            _pnlModal.Controls.Add(_btnFechar);

            CreateModalContent(this._pnlModal);

            this.Controls.Add(_pnlModal);

            if (String.IsNullOrEmpty(TargetControlID))
            {
                TargetControlID = _hfModal.ID;
            }
        }
        #endregion

        #region OnLoad
        /// <inheritdoc/>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (FecharModal != null)
                this._btnFechar.Click += new ImageClickEventHandler(_btnFechar_Click);

            if (this.FecharModal == null)
                _Modal.CancelControlID = _btnFechar.ID;
        }
        #endregion

        #endregion
        
        #region Eventos

        #region _btnFechar_Click
        private void _btnFechar_Click(object sender, ImageClickEventArgs e)
        {
            if (FecharModal != null)
                FecharModal(this, new EventArgs());

            this.Close();
        }
        #endregion

        #endregion
    }
}
