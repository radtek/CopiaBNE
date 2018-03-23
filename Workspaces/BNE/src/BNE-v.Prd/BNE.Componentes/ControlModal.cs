using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace BNE.Componentes
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
        private HiddenField _hfModal = new HiddenField{ ID = "hfModal" };

        private readonly Panel _pnlModal = new Panel();
        private readonly Literal _litTitulo = new Literal();
        private readonly Panel _pnlTitulo = new Panel();
        private readonly HtmlGenericControl _pnlTituloH2 = new HtmlGenericControl("H2");
        private readonly LinkButton _btlFechar = new LinkButton { CssClass = "fechar" };

        private ModalPopupExtender _Modal = new ModalPopupExtender();
        #endregion

        #region Propriedades

        public string ClientBehaviorID
        {
            get
            {
                EnsureChildControls();
                return _Modal.BehaviorID;
            }
        }

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
        public string TituloCss
        {
            get
            {
                EnsureChildControls();
                return _pnlTitulo.CssClass;
            }
            set
            {
                EnsureChildControls();
                _pnlTitulo.CssClass = value;
            }
        }
        #endregion

        #region BackgroundCssClass
        public String BackgroundCssClass
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
        public String Titulo
        {
            get
            {
                EnsureChildControls();
                return this._litTitulo.Text;
            }
            set
            {
                EnsureChildControls();
                this._litTitulo.Text = value;
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
        }
        #endregion

        #region IniciaLabel
        /// <summary>
        /// Inicia a label de título
        /// </summary>
        private void IniciaLabel()
        {
            _litTitulo.ID = "Titulo";
            _litTitulo.EnableTheming = false;
        }
        #endregion

        #region IniciaBotoes
        /// <summary>
        /// Inicia todos os botões
        /// </summary>
        private void IniciaBotoes()
        {
            _btlFechar.ID = "btlFechar";
            _btlFechar.EnableTheming = false;
            _btlFechar.CausesValidation = false;
            _btlFechar.Text = @"<span aria-hidden='true'>×</span>";
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

        #region Close

        public void Close()
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

            _pnlTituloH2.Controls.Add(_litTitulo);
            _pnlTitulo.Controls.Add(_pnlTituloH2);
            _pnlTitulo.Controls.Add(_btlFechar);
            _pnlModal.Controls.Add(_pnlTitulo);

            CreateModalContent(this._pnlModal);

            this.Controls.Add(_pnlModal);

            if (String.IsNullOrEmpty(TargetControlID))
            {
                TargetControlID = _hfModal.ID;
            }
        }
        #endregion

        #region OnLoad
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (FecharModal != null)
                this._btlFechar.Click += _btlFechar_Click;

            if (this.FecharModal == null)
                _Modal.CancelControlID = _btlFechar.ID;
        }

        private void _btlFechar_Click(object sender, EventArgs e)
        {
            FecharModal?.Invoke(this, new EventArgs());

            this.Close();
        }
        #endregion

        #endregion
    }
}
