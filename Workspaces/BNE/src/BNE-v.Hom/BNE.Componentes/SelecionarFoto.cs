using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BNE.Componentes.Util;

namespace BNE.Componentes
{
    public delegate void DelegateError(Exception ex);

    public class SelecionarFoto : CompositeControl
    {
        public event DelegateError Error;

        #region Eventos

        #region _mdlFoto_Error
        private void _mdlFoto_Error(Exception ex)
        {
            InformarErro(ex);
        }
        #endregion

        #endregion

        #region ModalFoto
        private class ModalFoto : ControlModal
        {
            #region Event Handlers
            public event DelegateError Error;
            #endregion

            #region Eventos

            #region _imgSlicer_ThumbnailCreated
            private void _imgSlicer_ThumbnailCreated(object sender, ThumbnailCreatedArgs e)
            {
                Owner.ImageData = e.ImageData;
                Owner.ImageUrl = e.ClientUrl;
                Close();
            }
            #endregion

            #endregion

            #region Campos
            private readonly UpdatePanel _upImage = new UpdatePanel { ID = "upImage", UpdateMode = UpdatePanelUpdateMode.Conditional };
            private readonly HtmlGenericControl _pLblInformacao = new HtmlGenericControl("p");
            private readonly Label _lblInformacao = new Label { ID = "lblInformacao", Text = "Selecione a área da imagem e clique em &quot;Confirmar&quot; para salvar." };
            private readonly Panel _pnlConteudo = new Panel { CssClass = "conteudo-modal" };

            private readonly ImageSlicer _imgSlicer = new ImageSlicer
            {
                ID = "imgSlicer",
                InitialSelection = "5;5;75;100",
                MinAcceptedHeight = 100,
                MinAcceptedWidth = 100,
                MaxHeight = 1024,
                MaxWidth = 1280
            };

            private readonly Panel _pnlBotoes = new Panel { ID = "pnlBotoes" };

            private readonly Button _btnConfirmar = new Button
            {
                ID = "btnConfirmar",
                CausesValidation = false,
                Text = "Confirmar",
                PostBackUrl = "javascript:;"
            };
            #endregion

            #region Propriedades

            #region ParagrafoLabelInformacaoCssClass
            public string ParagrafoLabelInformacaoCssClass
            {
                get { return _pLblInformacao.Attributes["class"]; }
                set { _pLblInformacao.Attributes["class"] = value; }
            }
            #endregion

            #region PainelBotoesModalFotoCssClass
            public string PainelBotoesModalFotoCssClass
            {
                get { return _pnlBotoes.CssClass; }
                set { _pnlBotoes.CssClass = value; }
            }
            #endregion

            #region BotaoConfirmarCssClass
            public string BotaoConfirmarCssClass
            {
                get { return _btnConfirmar.CssClass; }
                set { _btnConfirmar.CssClass = value; }
            }
            #endregion

            #region ImageData
            public byte[] ImageData
            {
                get { return _imgSlicer.ImageData; }
                set { _imgSlicer.SetImageData(value); }
            }
            #endregion

            #region ResizeImageWidth
            /// <summary>
            ///     Largura que a imagem deve ser gravada.
            /// </summary>
            public int? ResizeImageWidth
            {
                get { return _imgSlicer.ResizeImageWidth; }
                set { _imgSlicer.ResizeImageWidth = value; }
            }
            #endregion

            #region RedimencionaExato
            public bool RedimencionaExato
            {
                get { return _imgSlicer.RedimencionaExato; }
                set { _imgSlicer.RedimencionaExato = value; }
            }
            #endregion

            #region ResizeImageHeight
            /// <summary>
            ///     Altura que a imagem deve ser gravada.
            /// </summary>
            public int? ResizeImageHeight
            {
                get { return _imgSlicer.ResizeImageHeight; }
                set { _imgSlicer.ResizeImageHeight = value; }
            }
            #endregion

            #region MinAcceptedHeight
            public Unit MinAcceptedHeight
            {
                get { return _imgSlicer.MinAcceptedHeight; }
                set { _imgSlicer.MinAcceptedHeight = value; }
            }
            #endregion

            #region MinAcceptedWidth
            public Unit MinAcceptedWidth
            {
                get { return _imgSlicer.MinAcceptedWidth; }
                set { _imgSlicer.MinAcceptedWidth = value; }
            }
            #endregion

            #region MaxHeight
            public int MaxHeight
            {
                get { return _imgSlicer.MaxHeight; }
                set { _imgSlicer.MaxHeight = value; }
            }
            #endregion

            #region MaxWidth
            public int MaxWidth
            {
                get { return _imgSlicer.MaxWidth; }
                set { _imgSlicer.MaxWidth = value; }
            }
            #endregion

            #region AspectRatio
            public string AspectRatio
            {
                get { return _imgSlicer.AspectRatio; }
                set { _imgSlicer.AspectRatio = value; }
            }
            #endregion

            #region InitialSelection
            public string InitialSelection
            {
                get { return _imgSlicer.InitialSelection; }
                set { _imgSlicer.InitialSelection = value; }
            }
            #endregion

            #region SlicerWidth
            public Unit SlicerWidth
            {
                get { return _imgSlicer.Width; }
                set { _imgSlicer.Width = value; }
            }
            #endregion

            #region SlicerHeight
            public Unit SlicerHeight
            {
                get { return _imgSlicer.Height; }
                set { _imgSlicer.Height = value; }
            }
            #endregion

            #region Owner
            public SelecionarFoto Owner { get; set; }
            #endregion

            #endregion

            #region Métodos

            #region CreateModalContent
            protected override void CreateModalContent(Panel objPanel)
            {
                Titulo = "Selecione sua Imagem";

                _pLblInformacao.Controls.Add(_lblInformacao);

                _imgSlicer.Style.Add("position", "relative");
                _upImage.ContentTemplateContainer.Controls.Add(_imgSlicer);
                
                _pnlConteudo.Controls.Add(_pLblInformacao);
                _pnlConteudo.Controls.Add(_upImage);
                objPanel.Controls.Add(_pnlConteudo);

                _pnlBotoes.Controls.Add(_btnConfirmar);
                objPanel.Controls.Add(_pnlBotoes);
            }
            #endregion

            #region LimparFoto
            public void LimparFoto()
            {
                ImageData = null;
                _imgSlicer.Clear();
                _upImage.Update();
            }
            #endregion

            #region OnLoad
            protected override void OnLoad(EventArgs e)
            {
                FecharModal += ModalFoto_FecharModal;
                base.OnLoad(e);
                _btnConfirmar.OnClientClick = "btnConfirmar_OnClientClick('" + _imgSlicer.ClientID + "');";
                _imgSlicer.ThumbnailCreated += _imgSlicer_ThumbnailCreated;
            }

            private void ModalFoto_FecharModal(object sender, EventArgs e)
            {
                LimparFoto();
            }
            #endregion

            #region OnPreRender
            protected override void OnPreRender(EventArgs e)
            {
                base.OnPreRender(e);
                ScriptManager.RegisterClientScriptResource(this, typeof(SelecionarFoto),
                    "BNE.Componentes.Content.js.SelecionarFoto.js");
            }
            #endregion

            #region InformarErro
            private void InformarErro(Exception ex)
            {
                if (Error != null)
                    Error(ex);
            }
            #endregion

            #region ValidateImage
            public void ValidateImage(byte[] imageData)
            {
                _imgSlicer.ValidateImage(imageData);
            }
            #endregion

            #endregion
        }
        #endregion

        #region Campos
        private readonly UpdatePanel _upBotaoFoto = new UpdatePanel { ID = "upBotaoFoto", UpdateMode = UpdatePanelUpdateMode.Conditional };
        private readonly Panel _pnlFotoLogo = new Panel { ID = "pnlFotoLogo" };
        private readonly Panel _pnlImagemFotoLogo = new Panel { ID = "pnlImagemFotoLogo" };
        private readonly Image _imgFotoLogo = new Image { ID = "imgFotoLogo" };

        private readonly Panel _pnlFileUploadFotoLogo = new Panel { ID = "pnlFileUploadFotoLogo" };
        private readonly FileUpload _fupFotoLogo = new FileUpload { ID = "fupFotoLogo" };
        private readonly Button _btnEnviarFoto = new Button { ID = "btnEnviarFoto", CausesValidation = false };

        private readonly Panel _pnlRemoverImagem = new Panel { ID = "pnlFotoLogoRemoverFoto" };
        private readonly Button _btnRemove = new Button { ID = "btnRemove", CausesValidation = false, Text = "Remover Foto" };

        private readonly HiddenField _hdnModal = new HiddenField { ID = "hdnModal" };
        private readonly ModalFoto _mdlFoto = new ModalFoto { ID = "mdlFoto" };
        private int _imgOriginalWidth;
        private int _imgOriginalHeight;
        private int _imgCropHeight;
        private int _imgCropWidth;
        #endregion

        #region Properties

        #region PainelImagemFotoCssClass
        public string PainelImagemFotoCssClass
        {
            get { return _pnlImagemFotoLogo.CssClass; }
            set { _pnlImagemFotoLogo.CssClass = value; }
        }
        #endregion

        #region PainelFileUploadCssClass
        public string PainelFileUploadCssClass
        {
            get { return _pnlFileUploadFotoLogo.CssClass; }
            set { _pnlFileUploadFotoLogo.CssClass = value; }
        }
        #endregion

        #region SemFotoImagemUrl
        public string SemFotoImagemUrl
        {
            get { return Convert.ToString(ViewState["UrlImagemSemFoto"]); }
            set { ViewState["UrlImagemSemFoto"] = value; }
        }
        #endregion

        #region PainelBotoesModalFotoCssClass
        public string PainelBotoesModalFotoCssClass
        {
            get { return _mdlFoto.PainelBotoesModalFotoCssClass; }
            set { _mdlFoto.PainelBotoesModalFotoCssClass = value; }
        }
        #endregion

        #region PainelRemoverImagemCssClass
        public string PainelRemoverImagemCssClass
        {
            get { return _pnlRemoverImagem.CssClass; }
            set { _pnlRemoverImagem.CssClass = value; }
        }
        #endregion

        #region ParagrafoLabelInformacaoCssClass
        public string ParagrafoLabelInformacaoCssClass
        {
            get { return _mdlFoto.ParagrafoLabelInformacaoCssClass; }
            set { _mdlFoto.ParagrafoLabelInformacaoCssClass = value; }
        }
        #endregion

        #region BotaoConfirmarCssClass
        public string BotaoConfirmarCssClass
        {
            get { return _mdlFoto.BotaoConfirmarCssClass; }
            set { _mdlFoto.BotaoConfirmarCssClass = value; }
        }
        #endregion

        #region CssClass
        /// <inheritdoc />
        public string ModalCssClass
        {
            get { return _mdlFoto.CssClass; }
            set { _mdlFoto.CssClass = value; }
        }
        #endregion

        #region TituloCss
        /// <summary>
        ///     Css do título da modal
        /// </summary>
        public string TituloCss
        {
            get { return _mdlFoto.TituloCss; }
            set { _mdlFoto.TituloCss = value; }
        }
        #endregion

        #region BackgroundCssClass
        public string BackgroundCssClass
        {
            get { return _mdlFoto.BackgroundCssClass; }
            set { _mdlFoto.BackgroundCssClass = value; }
        }
        #endregion

        #region ImageUrl
        public string ImageUrl
        {
            get { return ViewState["ImageUrl"] == null ? string.Empty : ViewState["ImageUrl"].ToString(); }
            private set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ViewState["ImageUrl"] = SemFotoImagemUrl;
                    TemFoto = false;
                }
                else
                {
                    ViewState["ImageUrl"] = value;

                    TemFoto = SemFotoImagemUrl != value;
                    _pnlRemoverImagem.Visible = TemFoto;
                }
                _imgFotoLogo.ImageUrl = ImageUrl;
                _upBotaoFoto.Update();
            }
        }
        #endregion

        #region TemFoto
        /// <summary>
        ///     Propriedade utilizada para identificar se a foto do componente é padrão.
        /// </summary>
        private bool TemFoto
        {
            get
            {
                if (ViewState["TemFoto"] != null)
                    return (bool)ViewState["TemFoto"];
                return false;
            }
            set { ViewState["TemFoto"] = value; }
        }
        #endregion

        #region MinAcceptedHeight
        public Unit MinAcceptedHeight
        {
            get { return _mdlFoto.MinAcceptedHeight; }
            set { _mdlFoto.MinAcceptedHeight = value; }
        }
        #endregion

        #region MinAcceptedWidth
        public Unit MinAcceptedWidth
        {
            get { return _mdlFoto.MinAcceptedWidth; }
            set { _mdlFoto.MinAcceptedWidth = value; }
        }
        #endregion

        #region MaxHeight
        public int MaxHeight
        {
            get { return _mdlFoto.MaxHeight; }
            set { _mdlFoto.MaxHeight = value; }
        }
        #endregion

        #region MaxWidth
        public int MaxWidth
        {
            get { return _mdlFoto.MaxWidth; }
            set { _mdlFoto.MaxWidth = value; }
        }
        #endregion

        #region ResizeImageWidth
        /// <summary>
        ///     Largura que a imagem deve ser gravada.
        /// </summary>
        public int? ResizeImageWidth
        {
            get { return _mdlFoto.ResizeImageWidth; }
            set { _mdlFoto.ResizeImageWidth = value; }
        }
        #endregion

        #region ResizeImageHeight
        /// <summary>
        ///     Altura que a imagem deve ser gravada.
        /// </summary>
        public int? ResizeImageHeight
        {
            get { return _mdlFoto.ResizeImageHeight; }
            set { _mdlFoto.ResizeImageHeight = value; }
        }
        #endregion

        #region AspectRatio
        public string AspectRatio
        {
            get { return _mdlFoto.AspectRatio; }
            set { _mdlFoto.AspectRatio = value; }
        }
        #endregion

        #region InitialSelection
        public string InitialSelection
        {
            get { return _mdlFoto.InitialSelection; }
            set { _mdlFoto.InitialSelection = value; }
        }
        #endregion

        #region ImageData
        public byte[] ImageData
        {
            get
            {
                EnsureChildControls();

                return (byte[])(ViewState[Keys.General.ImageData.ToString()]);
            }
            set
            {
                EnsureChildControls();
                if (value != null)
                {
                    ViewState.Add(Keys.General.ImageData.ToString(), value);
                    ImageUrl = string.Format("data:image/gif;base64,{0}", Convert.ToBase64String(value));
                }
                else
                {
                    ViewState.Remove(Keys.General.ImageData.ToString());
                    ImageUrl = string.Empty;
                }
            }
        }
        #endregion

        #region PermitirAlterarFoto
        public bool PermitirAlterarFoto
        {
            get { return _fupFotoLogo.Enabled; }
            set { _fupFotoLogo.Enabled = false; }
        }
        #endregion

        #region RedimencionaExato
        public bool RedimencionaExato
        {
            get { return _mdlFoto.RedimencionaExato; }
            set { _mdlFoto.RedimencionaExato = value; }
        }
        #endregion

        #endregion

        #region Métodos

        #region CreateChildControls
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            _btnEnviarFoto.Style[HtmlTextWriterStyle.Display] = "none";

            _pnlImagemFotoLogo.Controls.Add(_imgFotoLogo);
            _pnlFotoLogo.Controls.Add(_pnlImagemFotoLogo);

            _pnlFileUploadFotoLogo.Controls.Add(_fupFotoLogo);
            _pnlFileUploadFotoLogo.Controls.Add(_btnEnviarFoto);
            _pnlFotoLogo.Controls.Add(_pnlFileUploadFotoLogo);

            _upBotaoFoto.ContentTemplateContainer.Controls.Add(_pnlFotoLogo);
            _pnlRemoverImagem.Controls.Add(_btnRemove);

            _upBotaoFoto.ContentTemplateContainer.Controls.Add(_pnlRemoverImagem);
            _upBotaoFoto.Triggers.Add(new PostBackTrigger { ControlID = _btnEnviarFoto.ID });

            Controls.Add(_upBotaoFoto);

            Controls.Add(_hdnModal);
            _mdlFoto.TargetControlID = _hdnModal.ID;
            _mdlFoto.Owner = this;
            Controls.Add(_mdlFoto);
        }
        #endregion

        #region _btnEnviarFoto_Click
        private void _btnEnviarFoto_Click(object sender, EventArgs e)
        {
            try
            {
                if (_fupFotoLogo.HasFile)
                {
                    var imageData = new byte[_fupFotoLogo.FileContent.Length];
                    _fupFotoLogo.FileContent.Read(imageData, 0, imageData.Length);

                    _mdlFoto.ValidateImage(imageData);

                    using (var imgOriginal = Helpers.ByteToImage(imageData))
                    {
                        _imgOriginalWidth = imgOriginal.Width;
                        _imgOriginalHeight = imgOriginal.Height;
                    }

                    //Largura maior que altura
                    decimal porcentagem;
                    if (_imgOriginalWidth > _imgOriginalHeight)
                    {
                        porcentagem = Convert.ToDecimal(370 * 100 / _imgOriginalWidth);

                        _imgCropWidth = 370;
                        _imgCropHeight = Convert.ToInt32(Math.Floor(_imgOriginalHeight * (porcentagem / 100)));
                    }
                    else //Altura maior que largura
                    {
                        porcentagem = Convert.ToDecimal(300 * 100 / _imgOriginalHeight);

                        _imgCropHeight = 300;
                        _imgCropWidth = Convert.ToInt32(Math.Floor(_imgOriginalWidth * (porcentagem / 100)));
                    }

                    _mdlFoto.SlicerHeight = _imgCropHeight;
                    _mdlFoto.SlicerWidth = _imgCropWidth;
                    _mdlFoto.ImageData = imageData;

                    _fupFotoLogo.Attributes["Value"] = string.Empty;

                    _mdlFoto.Show();
                }
            }
            catch (Exception ex)
            {
                InformarErro(ex);
            }
        }
        #endregion


        #region MyRegion

        #region _btnRemove_Click
        private void _btnRemove_Click(object sender, EventArgs e)
        {
            LimparFoto();
        }

        #endregion

        #endregion
        #region OnPreRender OnInit
        protected override void OnInit(EventArgs e)
        {
            _mdlFoto.Error += _mdlFoto_Error;
            _btnEnviarFoto.Click += _btnEnviarFoto_Click;
            _btnRemove.Click += _btnRemove_Click;
            base.OnInit(e);
        }
        #endregion


        #region OnPreRender
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            _pnlRemoverImagem.Visible = (_mdlFoto.ImageData != null || TemFoto) && PermitirAlterarFoto;
            if (_mdlFoto.ImageData == null && !TemFoto)
            {
                ImageUrl = SemFotoImagemUrl;
                _btnRemove.Visible = false;
                _fupFotoLogo.Visible = true;

            }
            else
            {
                _btnRemove.Visible = true;
                _fupFotoLogo.Visible = false;
            }

            _imgFotoLogo.ImageUrl = ImageUrl;
            _fupFotoLogo.Attributes.Add("OnChange", "window.setTimeout(function() { $(\"input[id*='btnEnviarFoto']\").trigger(\"click\") }, 500)");
            _upBotaoFoto.Update();
        }
        #endregion

        #region LimparFoto
        public void LimparFoto()
        {
            try
            {
                ImageData = null;
                _mdlFoto.ImageData = null;
                _mdlFoto.LimparFoto();
                _pnlRemoverImagem.Visible = false;
                _upBotaoFoto.Update();
            }
            catch (Exception ex)
            {
                InformarErro(ex);
            }
        }
        #endregion

        #region InformarErro
        private void InformarErro(Exception ex)
        {
            if (Error != null)
                Error(ex);
        }
        #endregion

        #endregion
    }
}