using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web;
using System.IO;
using System.Web.UI.HtmlControls;

namespace BNE.Componentes
{
    public delegate void DelegateError(Exception ex);

    public class SelecionarFoto : CompositeControl
    {
        #region ModalFoto
        private class ModalFoto : ControlModal
        {
            #region Campos
            private UpdatePanel _upImage = new UpdatePanel() { ID = "upImage", UpdateMode = UpdatePanelUpdateMode.Conditional };
            private HtmlGenericControl _pLblInformacao = new HtmlGenericControl("p");
            private Label _lblInformacao = new Label() { ID = "lblInformacao", Text = "Selecione a área da imagem e clique em &quot;Confirmar&quot; para salvar." };
            private ImageSlicer _imgSlicer = new ImageSlicer()
            {
                ID = "imgSlicer",
                //                AspectRatio = "4:3",
                InitialSelection = "5;5;75;100",
                MinAcceptedHeight = 100,
                MinAcceptedWidth = 100,
                MaxHeight = 1024,
                MaxWidth = 1280
            };
            private Panel _pnlBotoes = new Panel() { ID = "pnlBotoes" };
            private Button _btnConfirmar = new Button()
            {
                ID = "btnConfirmar",
                CausesValidation = false,
                Text = "Confirmar",
                PostBackUrl = "javascript:;"
            };
            #endregion

            #region Event Handlers
            public event DelegateError Error;
            #endregion

            #region Propriedades

            #region ParagrafoLabelInformacaoCssClass
            public String ParagrafoLabelInformacaoCssClass
            {
                get { return _pLblInformacao.Attributes["class"]; }
                set { _pLblInformacao.Attributes["class"] = value; }
            }
            #endregion

            #region PainelBotoesModalFotoCssClass
            public String PainelBotoesModalFotoCssClass
            {
                get { return _pnlBotoes.CssClass; }
                set { _pnlBotoes.CssClass = value; }
            }
            #endregion

            #region BotaoConfirmarCssClass
            public String BotaoConfirmarCssClass
            {
                get { return _btnConfirmar.CssClass; }
                set { _btnConfirmar.CssClass = value; }
            }
            #endregion

            #region ImageUrl
            public String ImageUrl
            {
                get
                {
                    return this._imgSlicer.ImageUrl;
                }
                set
                {
                    this._imgSlicer.ImageUrl = value;
                }
            }
            #endregion

            #region ThumbDir
            public String ThumbDir
            {
                get
                {
                    return this._imgSlicer.ThumbDir;
                }
                set
                {
                    this._imgSlicer.ThumbDir = value;
                }
            }
            #endregion

            #region ResizeImageWidth
            /// <summary>
            /// Largura que a imagem deve ser gravada.
            /// </summary>
            public int? ResizeImageWidth
            {
                get
                {
                    return _imgSlicer.ResizeImageWidth;
                }
                set
                {
                    _imgSlicer.ResizeImageWidth = value;
                }
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
            /// Altura que a imagem deve ser gravada.
            /// </summary>
            public int? ResizeImageHeight
            {
                get
                {
                    return _imgSlicer.ResizeImageHeight;
                }
                set
                {
                    _imgSlicer.ResizeImageHeight = value;
                }
            }
            #endregion

            #region MinAcceptedHeight
            public Unit MinAcceptedHeight
            {
                get
                {
                    return this._imgSlicer.MinAcceptedHeight;
                }
                set
                {
                    this._imgSlicer.MinAcceptedHeight = value;
                }
            }
            #endregion

            #region MinAcceptedWidth
            public Unit MinAcceptedWidth
            {
                get
                {
                    return this._imgSlicer.MinAcceptedWidth;
                }
                set
                {
                    this._imgSlicer.MinAcceptedWidth = value;
                }
            }
            #endregion

            #region MaxHeight
            public int MaxHeight
            {
                get
                {
                    return this._imgSlicer.MaxHeight;
                }
                set
                {
                    this._imgSlicer.MaxHeight = value;
                }
            }
            #endregion

            #region MaxWidth
            public int MaxWidth
            {
                get
                {
                    return this._imgSlicer.MaxWidth;
                }
                set
                {
                    this._imgSlicer.MaxWidth = value;
                }
            }
            #endregion

            #region AspectRatio
            public String AspectRatio
            {
                get { return this._imgSlicer.AspectRatio; }
                set { this._imgSlicer.AspectRatio = value; }
            }
            #endregion

            #region InitialSelection
            public String InitialSelection
            {
                get { return this._imgSlicer.InitialSelection; }
                set { this._imgSlicer.InitialSelection = value; }
            }
            #endregion

            #region SlicerWidth
            public Unit SlicerWidth
            {
                get { return this._imgSlicer.Width; }
                set { this._imgSlicer.Width = value; }
            }
            #endregion

            #region SlicerHeight
            public Unit SlicerHeight
            {
                get { return this._imgSlicer.Height; }
                set { this._imgSlicer.Height = value; }
            }
            #endregion

            #region ImageData
            public byte[] ImageData
            {
                get
                {
                    EnsureChildControls();

                    return (byte[])(ViewState["ImageData"]);
                }
                set
                {
                    EnsureChildControls();
                    if (value != null)
                        ViewState.Add("ImageData", value);
                    else
                        ViewState.Remove("ImageData");
                }
            }
            #endregion

            #region Owner
            public SelecionarFoto Owner
            {
                get;
                set;
            }
            #endregion

            #region SlicerID
            public String SlicerID
            {
                get
                {
                    return this._imgSlicer.ClientID;
                }
            }
            #endregion

            #region SlicerPercentual
            public decimal SlicerPercentual
            {
                get { return this._imgSlicer.Percentual; }
                set { this._imgSlicer.Percentual = value; }
            }
            #endregion


            #endregion

            #region Métodos

            #region CreateModalContent
            protected override void CreateModalContent(System.Web.UI.WebControls.Panel objPanel)
            {

                //this.Titulo = "Selecione sua Imagem";

                _pLblInformacao.Controls.Add(_lblInformacao);

                objPanel.Controls.Add(_pLblInformacao);
                _imgSlicer.Style.Add("position", "relative");
                _upImage.ContentTemplateContainer.Controls.Add(_imgSlicer);
                objPanel.Controls.Add(_upImage);

                _pnlBotoes.Controls.Add(_btnConfirmar);
                objPanel.Controls.Add(_pnlBotoes);
            }
            #endregion

            #region LimparFoto
            public void LimparFoto()
            {
                this.ImageData = null;
                this._imgSlicer.Clear();
                this._upImage.Update();
            }
            #endregion

            #region OnLoad
            protected override void OnLoad(EventArgs e)
            {
                base.FecharModal += new EventHandler(ModalFoto_FecharModal);
                base.OnLoad(e);
                this._btnConfirmar.OnClientClick = "btnConfirmar_OnClientClick('" + this._imgSlicer.ClientID + "');";
                _imgSlicer.ThumbnailCreated += new ThumbnailCreatedEvent(_imgSlicer_ThumbnailCreated);
            }

            void ModalFoto_FecharModal(object sender, EventArgs e)
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

            #endregion

            #region Eventos

            #region _imgSlicer_ThumbnailCreated
            private void _imgSlicer_ThumbnailCreated(object sender, ThumbnailCreatedArgs e)
            {
                try
                {
                    string caminhoArquivo = this.Page.Server.MapPath(e.RelativeUrl);

                    FileStream fs = null;
                    BinaryReader br = null;
                    try
                    {
                        fs = File.Open(caminhoArquivo, FileMode.Open);
                        br = new BinaryReader(fs);

                        ImageData = br.ReadBytes((int)fs.Length);

                        fs.Close();
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Dispose();
                    }

                    //Ajustando botão
                    //ANTIGO
                    //btiFoto.ImageUrl = e.RelativeUrl;
                    Owner.ImageUrl = this.Page.ResolveUrl(e.RelativeUrl);
                    this.Close();
                }
                catch (Exception ex)
                {
                    InformarErro(ex);
                }
            }
            #endregion

            #endregion

        }
        #endregion

        #region Campos
        private UpdatePanel _upBotaoFoto = new UpdatePanel() { ID = "upBotaoFoto", UpdateMode = UpdatePanelUpdateMode.Conditional };
        private Panel _pnlFotoLogo = new Panel() { ID = "pnlFotoLogo" };
        private Panel _pnlImagemFotoLogo = new Panel() { ID = "pnlImagemFotoLogo" };
        private Image _imgFotoLogo = new Image() { ID = "imgFotoLogo" };

        private Panel _pnlFileUploadFotoLogo = new Panel() { ID = "pnlFileUploadFotoLogo" };
        private FileUpload _fupFotoLogo = new FileUpload() { ID = "fupFotoLogo" };
        private Button _btnEnviarFoto = new Button() { ID = "btnEnviarFoto", CausesValidation = false };

        private Panel _pnlRemoverImagem = new Panel() { ID = "pnlFotoLogoRemoverFoto" };
        //private LinkButton _lbtAlterarFoto = new LinkButton() { ID = "lbtAlterarFoto", Text = "Alterar", CausesValidation = false };
        private HiddenField _hdnModal = new HiddenField() { ID = "hdnModal" };
        private ModalFoto _mdlFoto = new ModalFoto() { ID = "mdlFoto" };
        private int _imgOriginalWidth;
        private int _imgOriginalHeight;
        private int _imgCropHeight;
        private int _imgCropWidth;

        #endregion

        #region Event Handlers
        public event DelegateError Error;
        #endregion

        #region Properties

        #region PainelImagemFotoCssClass
        public String PainelImagemFotoCssClass
        {
            get
            {
                return _pnlImagemFotoLogo.CssClass;
            }
            set
            {
                _pnlImagemFotoLogo.CssClass = value;
            }
        }
        #endregion

        #region PainelFileUploadCssClass
        public String PainelFileUploadCssClass
        {
            get { return _pnlFileUploadFotoLogo.CssClass; }
            set { _pnlFileUploadFotoLogo.CssClass = value; }
        }
        #endregion

        #region SemFotoImagemUrl
        public String SemFotoImagemUrl
        {
            get
            {
                return Convert.ToString(this.ViewState["UrlImagemSemFoto"]);
            }
            set
            {
                this.ViewState["UrlImagemSemFoto"] = value;
            }
        }
        #endregion

        #region PainelBotoesModalFotoCssClass
        public String PainelBotoesModalFotoCssClass
        {
            get { return _mdlFoto.PainelBotoesModalFotoCssClass; }
            set { _mdlFoto.PainelBotoesModalFotoCssClass = value; }
        }
        #endregion

        #region PainelRemoverImagemCssClass
        public String PainelRemoverImagemCssClass
        {
            get { return _pnlRemoverImagem.CssClass; }
            set { _pnlRemoverImagem.CssClass = value; }
        }
        #endregion

        #region ParagrafoLabelInformacaoCssClass
        public String ParagrafoLabelInformacaoCssClass
        {
            get { return this._mdlFoto.ParagrafoLabelInformacaoCssClass; }
            set { this._mdlFoto.ParagrafoLabelInformacaoCssClass = value; }
        }
        #endregion

        #region BotaoConfirmarCssClass
        public String BotaoConfirmarCssClass
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
        /// Css do título da modal 
        /// </summary>
        public string TituloCss
        {
            get { return _mdlFoto.TituloCss; }
            set { _mdlFoto.TituloCss = value; }
        }
        #endregion

        #region BotaoFecharCss
        /// <summary>
        /// Classe Css do botão de fechar
        /// </summary>
        public string BotaoFecharCss
        {
            get { return _mdlFoto.BotaoFecharCss; }
            set { _mdlFoto.BotaoFecharCss = value; }
        }
        #endregion

        #region BackgroundCssClass
        public String BackgroundCssClass
        {
            get
            {
                return _mdlFoto.BackgroundCssClass;
            }
            set
            {
                _mdlFoto.BackgroundCssClass = value;
            }
        }
        #endregion

        #region ImageUrl
        public String ImageUrl
        {
            get
            {
                return ViewState["ImageUrl"] == null ? String.Empty : ViewState["ImageUrl"].ToString();
                //return this._imgFotoLogo.ImageUrl; 
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    //this._imgFotoLogo.ImageUrl = this.SemFotoImagemUrl;
                    ViewState["ImageUrl"] = this.SemFotoImagemUrl;
                    TemFoto = false;
                }
                else
                {
                    //this._imgFotoLogo.ImageUrl = value;
                    ViewState["ImageUrl"] = value;

                    TemFoto = SemFotoImagemUrl != value;
                    _pnlRemoverImagem.Visible = TemFoto;

                }
                this._imgFotoLogo.ImageUrl = ImageUrl;
                this._upBotaoFoto.Update();
            }
        }
        #endregion

        #region TemFoto
        /// <summary>
        /// Propriedade utilizada para identificar se a foto do componente é padrão.
        /// </summary>
        private Boolean TemFoto
        {
            get
            {
                if (ViewState["TemFoto"] != null)
                    return (Boolean)ViewState["TemFoto"];
                return false;
            }
            set
            {
                ViewState["TemFoto"] = value;
            }
        }
        #endregion

        #region ThumbDir
        public String ThumbDir
        {
            get
            {
                return this._mdlFoto.ThumbDir;
            }
            set
            {
                this._mdlFoto.ThumbDir = value;
            }
        }
        #endregion

        #region MinAcceptedHeight
        public Unit MinAcceptedHeight
        {
            get
            {
                return this._mdlFoto.MinAcceptedHeight;
            }
            set
            {
                this._mdlFoto.MinAcceptedHeight = value;
            }
        }
        #endregion

        #region MinAcceptedWidth
        public Unit MinAcceptedWidth
        {
            get
            {
                return this._mdlFoto.MinAcceptedWidth;
            }
            set
            {
                this._mdlFoto.MinAcceptedWidth = value;
            }
        }
        #endregion

        #region MaxHeight
        public int MaxHeight
        {
            get
            {
                return this._mdlFoto.MaxHeight;
            }
            set
            {
                this._mdlFoto.MaxHeight = value;
            }
        }
        #endregion

        #region MaxWidth
        public int MaxWidth
        {
            get
            {
                return this._mdlFoto.MaxWidth;
            }
            set
            {
                this._mdlFoto.MaxWidth = value;
            }
        }
        #endregion

        #region ResizeImageWidth
        /// <summary>
        /// Largura que a imagem deve ser gravada.
        /// </summary>
        public int? ResizeImageWidth
        {
            get
            {
                return _mdlFoto.ResizeImageWidth;
            }
            set
            {
                _mdlFoto.ResizeImageWidth = value;
            }
        }
        #endregion

        #region ResizeImageHeight
        /// <summary>
        /// Altura que a imagem deve ser gravada.
        /// </summary>
        public int? ResizeImageHeight
        {
            get
            {
                return _mdlFoto.ResizeImageHeight;
            }
            set
            {
                _mdlFoto.ResizeImageHeight = value;
            }
        }
        #endregion

        #region AspectRatio
        public String AspectRatio
        {
            get { return this._mdlFoto.AspectRatio; }
            set { this._mdlFoto.AspectRatio = value; }
        }
        #endregion

        #region InitialSelection
        public String InitialSelection
        {
            get { return this._mdlFoto.InitialSelection; }
            set { this._mdlFoto.InitialSelection = value; }
        }
        #endregion

        #region ImageData
        public byte[] ImageData
        {
            get
            {
                EnsureChildControls();
                return this._mdlFoto.ImageData;
            }
            set
            {
                EnsureChildControls();
                this._mdlFoto.ImageData = value;
            }
        }
        #endregion

        #region PermitirAlterarFoto
        public Boolean PermitirAlterarFoto
        {
            get
            {
                return this._fupFotoLogo.Enabled;
            }
            set
            {
                this._fupFotoLogo.Enabled = false;
            }

        }
        #endregion

        #region BotaoFecharImageUrl
        public string BotaoFecharImageUrl
        {
            set { _mdlFoto.BotaoFecharImageUrl = value; }
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

            //_pnlRemoverImagem.Controls.Add(_lbtAlterarFoto);
            _upBotaoFoto.ContentTemplateContainer.Controls.Add(_pnlRemoverImagem);
            _upBotaoFoto.Triggers.Add(new PostBackTrigger { ControlID = _btnEnviarFoto.ID });

            this.Controls.Add(_upBotaoFoto);

            this.Controls.Add(_hdnModal);
            this._mdlFoto.TargetControlID = _hdnModal.ID;
            this._mdlFoto.Owner = this;
            this.Controls.Add(_mdlFoto);
        }
        #endregion

        #region UploadNovo
        void _btnEnviarFoto_Click(object sender, EventArgs e)
        {
            try
            {

                if (_fupFotoLogo.HasFile)
                {
                    byte[] imageData = new byte[_fupFotoLogo.FileContent.Length];
                    _fupFotoLogo.FileContent.Read(imageData, 0, (int)imageData.Length);

                    String extensao = System.IO.Path.GetExtension(_fupFotoLogo.FileName).ToLowerInvariant();
                    String mapPath = Page.Server.MapPath(ThumbDir);

                    if (!Directory.Exists(mapPath))
                        Directory.CreateDirectory(mapPath);

                    string caminhoNomeArquivo = this.ThumbDir +
                                                    DateTime.Now.Day + "-" +
                                                    DateTime.Now.Month + "-" +
                                                    DateTime.Now.Year + "_" +
                                                    DateTime.Now.Hour + "." +
                                                    DateTime.Now.Minute + "." +
                                                    DateTime.Now.Second + "." +
                                                    DateTime.Now.Ticks + "_" +
                                                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].Replace(":", "") + "_" +
                                                    this.Page.Session.SessionID + extensao;

                    FileStream fs = null;

                    try
                    {
                        fs = new FileStream(this.Page.Server.MapPath(caminhoNomeArquivo), FileMode.CreateNew, FileAccess.Write);
                        fs.Write(imageData, 0, imageData.Length);
                        fs.Position = 0;
                        fs.Close();
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }

                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(this.Page.Server.MapPath(caminhoNomeArquivo));
                    _imgOriginalWidth = imgOriginal.Width;
                    _imgOriginalHeight = imgOriginal.Height;

                    //Largura maior que altura
                    Decimal porcentagem;
                    if (_imgOriginalWidth > _imgOriginalHeight)
                    {
                        decimal width = 370;

                        porcentagem = (width * 100) / _imgOriginalWidth / 100;
                        _imgCropWidth = (int)width;
                        _imgCropHeight = Convert.ToInt32(Math.Floor(_imgOriginalHeight * porcentagem));
                    }
                    //Altura maior que largura
                    else
                    {
                        decimal height = 300;

                        porcentagem = (height * 100) / _imgOriginalHeight / 100;
                        _imgCropHeight = (int)height;
                        _imgCropWidth = Convert.ToInt32(Math.Floor(_imgOriginalWidth * porcentagem));
                    }

                    this._mdlFoto.SlicerHeight = _imgCropHeight;
                    this._mdlFoto.SlicerWidth = _imgCropWidth;

                    this._mdlFoto.ImageUrl = this.ResolveUrl(caminhoNomeArquivo);

                    this._mdlFoto.SlicerPercentual = porcentagem;

                    _fupFotoLogo.Attributes["Value"] = String.Empty;
                    //_upBotaoFoto.Update();

                    //Exibe a modal para selecionar a área da imagem
                    this._mdlFoto.Show();
                }
            }
            catch (Exception ex)
            {
                InformarErro(ex);
            }
        }
        #endregion

        protected override void OnInit(EventArgs e)
        {
            //this._lbtAlterarFoto.Click += new EventHandler(_lbtAlterarFoto_Click);
            this._mdlFoto.Error += new DelegateError(_mdlFoto_Error);
            this._btnEnviarFoto.Click += new EventHandler(_btnEnviarFoto_Click);
            base.OnInit(e);
        }

        #region OnPreRender
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            this._pnlRemoverImagem.Visible = (this._mdlFoto.ImageData != null || this.TemFoto) && this.PermitirAlterarFoto;
            if (this._mdlFoto.ImageData == null && !this.TemFoto)
            {
                this.ImageUrl = this.SemFotoImagemUrl;
            }
            this._imgFotoLogo.ImageUrl = ImageUrl;

            this._fupFotoLogo.Attributes.Add("OnChange",
                "window.setTimeout(function() { $(\"input[id*='btnEnviarFoto']\").trigger(\"click\") }, 500)");

            this._upBotaoFoto.Update();
        }
        #endregion

        #region LimparFoto
        public void LimparFoto()
        {
            try
            {
                this.ImageUrl = this.SemFotoImagemUrl;
                this._mdlFoto.ImageData = null;
                this._mdlFoto.LimparFoto();
                this._pnlRemoverImagem.Visible = false;
                this._upBotaoFoto.Update();
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

        #region Eventos

        #region _lbtAlterarFoto_Click
        void _lbtAlterarFoto_Click(object sender, EventArgs e)
        {
            try
            {

                if (_fupFotoLogo.HasFile)
                {
                    byte[] imageData = new byte[_fupFotoLogo.FileContent.Length];
                    _fupFotoLogo.FileContent.Read(imageData, 0, (int)imageData.Length);

                    String extensao = System.IO.Path.GetExtension(_fupFotoLogo.FileName).ToLowerInvariant();
                    String mapPath = Page.Server.MapPath(ThumbDir);

                    if (!Directory.Exists(mapPath))
                        Directory.CreateDirectory(mapPath);

                    string caminhoNomeArquivo = this.ThumbDir +
                                                    DateTime.Now.Day + "-" +
                                                    DateTime.Now.Month + "-" +
                                                    DateTime.Now.Year + "_" +
                                                    DateTime.Now.Hour + "." +
                                                    DateTime.Now.Minute + "." +
                                                    DateTime.Now.Second + "." +
                                                    DateTime.Now.Ticks + "_" +
                                                    HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].Replace(":", "") + "_" +
                                                    this.Page.Session.SessionID + extensao;

                    FileStream fs = null;

                    try
                    {
                        fs = new FileStream(this.Page.Server.MapPath(caminhoNomeArquivo), FileMode.CreateNew, FileAccess.Write);
                        fs.Write(imageData, 0, imageData.Length);
                        fs.Position = 0;
                        fs.Close();
                    }
                    finally
                    {
                        if (fs != null)
                            fs.Close();
                    }

                    System.Drawing.Image imgOriginal = System.Drawing.Image.FromFile(this.Page.Server.MapPath(caminhoNomeArquivo));
                    _imgOriginalWidth = imgOriginal.Width;
                    _imgOriginalHeight = imgOriginal.Height;

                    //Largura maior que altura
                    Decimal porcentagem;
                    if (_imgOriginalWidth > _imgOriginalHeight)
                    {
                        Decimal width = 370;

                        porcentagem = (width * 100) / _imgOriginalWidth / 100;

                        _imgCropWidth = (int)width;
                        _imgCropHeight = Convert.ToInt32(Math.Floor(_imgOriginalHeight * porcentagem));
                    }
                    //Altura maior que largura
                    else
                    {
                        Decimal height = 300;

                        porcentagem = (height * 100) / _imgOriginalHeight / 100;

                        _imgCropHeight = (int)height;
                        _imgCropWidth = Convert.ToInt32(Math.Floor(_imgOriginalWidth * porcentagem));
                    }

                    this._mdlFoto.SlicerHeight = _imgCropHeight;
                    this._mdlFoto.SlicerWidth = _imgCropWidth;

                    this._mdlFoto.ImageUrl = this.ResolveUrl(caminhoNomeArquivo);

                    this._mdlFoto.SlicerPercentual = porcentagem;

                    _fupFotoLogo.Attributes["Value"] = String.Empty;
                    //_upBotaoFoto.Update();

                    //Exibe a modal para selecionar a área da imagem
                    this._mdlFoto.Show();
                }
            }
            catch (Exception ex)
            {
                InformarErro(ex);
            }
            //this.LimparFoto();
        }
        #endregion

        #region _mdlFoto_Error
        private void _mdlFoto_Error(Exception ex)
        {
            InformarErro(ex);
        }
        #endregion

        #endregion

    }
}
