using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Componentes.EL;
using BNE.Componentes.Extensions;
using BNE.Componentes.Util;
using Image = System.Drawing.Image;

namespace BNE.Componentes
{
    /// <summary>
    ///     Classe de recorte de foto
    /// </summary>
    public class ImageSlicer : WebControl, IScriptControl
    {
        #region EventHandler
        /// <summary>
        ///     Evento disparado quando um thumbnail for criado
        /// </summary>
        public event ThumbnailCreatedEvent ThumbnailCreated;
        #endregion

        #region Fields
        private ScriptManager _sm; // Referencia para o script manager da páginas
        private LiteralControl _literal; // Literal que mostra as tagsl de imagem        
        #endregion

        #region Properties

        #region AcceptedTypes
        /// <summary>
        ///     As extensões aceitas pelo controle
        /// </summary>
        public static string[] AcceptedTypes = { ".jpg", ".jpeg", ".gif", ".png" };
        #endregion

        #region ImageUrl
        /// <summary>
        ///     Url da imagem atualmente no corte
        /// </summary>
        public string ImageUrl
        {
            get { return Convert.ToString(ViewState[Keys.General.ImageUrl.ToString()]); }
            private set { ViewState[Keys.General.ImageUrl.ToString()] = value; }
        }
        #endregion

        #region ImageData
        internal byte[] ImageData
        {
            get
            {
                EnsureChildControls();

                return (byte[])(ViewState[Keys.General.ImageData.ToString()]);
            }
            private set
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

        #region MaxWidth
        /// <summary>
        ///     Largura máximo do retângulo de seleção
        /// </summary>
        public int MaxWidth
        {
            get
            {
                if (ViewState[Keys.General.MaxWidth.ToString()] == null)
                    return 150;
                return Convert.ToInt32(ViewState[Keys.General.MaxWidth.ToString()]);
            }
            set { ViewState[Keys.General.MaxWidth.ToString()] = value; }
        }
        #endregion

        #region MaxHeight
        /// <summary>
        ///     Largura máxima do retângulo de seleção
        /// </summary>
        public int MaxHeight
        {
            get
            {
                if (ViewState[Keys.General.MaxHeight.ToString()] == null)
                    return 200;

                return Convert.ToInt32(ViewState[Keys.General.MaxHeight.ToString()]);
            }
            set { ViewState[Keys.General.MaxHeight.ToString()] = value; }
        }
        #endregion

        #region AspectRatio
        /// <summary>
        ///     Proporção da área de corte da imagem. Deve estar no seguinte formato: "x:y", onde <br />
        ///     x: proporção de largura<br />
        ///     y: proporção de altura<br />
        /// </summary>
        public string AspectRatio
        {
            get { return Convert.ToString(ViewState[Keys.Images.AspectRatio.ToString()]); }
            set { ViewState[Keys.Images.AspectRatio.ToString()] = value; }
        }
        #endregion

        #region InitialSelection
        /// <summary>
        ///     Define a área inicial selecionada no seguinte formato: "x1;y1;x2;y2;" onde:<br />
        ///     x1 e x2: posições x (inicial e final) do controle<br />
        ///     y1 e y2: posições y (inicial e final) do controle<br />
        /// </summary>
        public string InitialSelection
        {
            get { return Convert.ToString(ViewState["initialSelection"]); }
            set { ViewState["initialSelection"] = value; }
        }
        #endregion

        #region InvalidAspectRatioMessage
        /// <summary>
        ///     Mensagem de erro de proporção inválida
        /// </summary>
        private string InvalidAspectRatioMessage
        {
            get
            {
                if (string.IsNullOrEmpty(Convert.ToString(ViewState[Keys.Images.InvalidAspectRatio.ToString()])))
                    return Resources.ImageSlicerInvalidAspectRatio;
                return Convert.ToString(ViewState[Keys.Images.InvalidAspectRatio.ToString()]);
            }
        }
        #endregion

        #region InvalidInitialSelectionMessage
        /// <summary>
        ///     Mensagem de erro de seleção inicial inválida
        /// </summary>
        public string InvalidInitialSelectionMessage
        {
            get
            {
                if (string.IsNullOrEmpty(Convert.ToString(ViewState[Keys.Images.InvalidInitialSelection.ToString()])))
                    return Resources.ImageSlicerInvalidInitialSelection;
                return Convert.ToString(ViewState[Keys.Images.InvalidInitialSelection.ToString()]);
            }
            set { ViewState[Keys.Images.InvalidInitialSelection.ToString()] = value; }
        }
        #endregion

        #region InvalidImageFormatMessage
        /// <summary>
        ///     Mensagem de erro de formato inválido de mensagem
        /// </summary>
        public static string InvalidImageFormatMessage
        {
            get
            {
                var arr = AcceptedTypes.ReplaceAll(".", string.Empty);
                return string.Format(Resources.ImageSlicerErrorImageType, string.Join(", ", arr).ToUpperInvariant());
            }
        }
        #endregion

        #region InvalidImageMinimumSizeMessage
        /// <summary>
        ///     Mensagem de erro de tamanho mínimo inválido
        /// </summary>
        public string InvalidImageMinimumSizeMessage
        {
            get
            {
                if (string.IsNullOrEmpty(Convert.ToString(ViewState[Keys.Images.InvalidMinimumImageSizeMessage.ToString()])))
                    return Resources.InvalidMinSizeImage;
                return Convert.ToString(ViewState[Keys.Images.InvalidMinimumImageSizeMessage.ToString()]);
            }
            set { ViewState[Keys.Images.InvalidMinimumImageSizeMessage.ToString()] = value; }
        }
        #endregion

        #region ImageWidth
        /// <summary>
        ///     Largura real da imagem tratado dentro do controle
        /// </summary>
        protected Unit ImageWidth
        {
            get
            {
                if (ViewState[Keys.Images.ImageWidth.ToString()] == null)
                    return Unit.Empty;
                return (Unit)ViewState[Keys.Images.ImageWidth.ToString()];
            }
            set { ViewState[Keys.Images.ImageWidth.ToString()] = value; }
        }
        #endregion

        #region ImageHeight
        /// <summary>
        ///     Altura real da imagem tratado dentro do controle
        /// </summary>
        protected Unit ImageHeight
        {
            get
            {
                if (ViewState[Keys.Images.ImageHeight.ToString()] == null)
                    return Unit.Empty;
                return (Unit)ViewState[Keys.Images.ImageHeight.ToString()];
            }
            set { ViewState[Keys.Images.ImageHeight.ToString()] = value; }
        }
        #endregion

        #region ResizeImageWidth
        /// <summary>
        ///     Largura que a imagem deve ser gravada.
        /// </summary>
        public int? ResizeImageWidth
        {
            get { return (int?)ViewState["ResizeImageWidth"]; }
            set { ViewState["ResizeImageWidth"] = value; }
        }
        #endregion

        #region ResizeImageHeight
        /// <summary>
        ///     Altura que a imagem deve ser gravada.
        /// </summary>
        public int? ResizeImageHeight
        {
            get { return (int?)ViewState["ResizeImageHeight"]; }
            set { ViewState["ResizeImageHeight"] = value; }
        }
        #endregion

        #region MinAcceptedWidth
        /// <summary>
        ///     Largura mínima aceita pelo controle
        /// </summary>
        public Unit MinAcceptedWidth
        {
            get
            {
                if (ViewState[Keys.Images.MinAcceptedWidth.ToString()] == null)
                    return Unit.Empty;
                return (Unit)ViewState[Keys.Images.MinAcceptedWidth.ToString()];
            }
            set { ViewState[Keys.Images.MinAcceptedWidth.ToString()] = value; }
        }
        #endregion

        #region MinAcceptedHeight
        /// <summary>
        ///     Altura mínima aceita pelo controle
        /// </summary>
        public Unit MinAcceptedHeight
        {
            get
            {
                if (ViewState[Keys.Images.MinAcceptedHeight.ToString()] == null)
                    return Unit.Empty;
                return (Unit)ViewState[Keys.Images.MinAcceptedHeight.ToString()];
            }
            set { ViewState[Keys.Images.MinAcceptedHeight.ToString()] = value; }
        }
        #endregion

        #region RedimencionaExato
        /// <summary>
        ///     Realiza redimencionamento exato da imagem
        /// </summary>
        public bool RedimencionaExato
        {
            get { return (bool?)ViewState["RedimencionaExato"] ?? false; }
            set { ViewState["RedimencionaExato"] = value; }
        }
        #endregion

        #endregion

        #region Methods

        #region GetScriptDescriptors
        /// <summary>
        ///     Retorna os descritores de script
        /// </summary>
        /// <returns>Coleção dos descritores de script utilizados pelo controle</returns>
        public IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            var descriptor = new ScriptControlDescriptor("ImgResizer", ClientID);
            return new[] { descriptor };
        }
        #endregion

        #region GetScriptReferences
        /// <summary>
        ///     Retorna as referencias aos scripts usados pelo controle
        /// </summary>
        /// <returns>Coleção das referencias de scripts utilizdos pelo controle</returns>
        public IEnumerable<ScriptReference> GetScriptReferences()
        {
            var reference = new ScriptReference
            {
                Assembly = "BNE.Componentes",
                Name = "BNE.Componentes.Content.js.ImgResizer.js"
            };

            return new[] { reference };
        }
        #endregion

        #region CreateChildControls
        /// <summary>
        ///     Inicia os controles filhos utilizados dentro deste controle
        /// </summary>
        protected override void CreateChildControls()
        {
            if (ChildControlsCreated)
                return;

            CreateImageTag();


            base.CreateChildControls();
        }
        #endregion

        #region CreateImageTag
        /// <summary>
        ///     Cria a tag de imagem
        /// </summary>
        private void CreateImageTag()
        {
            if (_literal == null)
                _literal = new LiteralControl();

            var style = new StringBuilder();
            style.Append("style=\"");
            if (!ImageWidth.IsEmpty)
            {
                style.Append("max-width:" + ImageWidth);
                style.Append("; ");
            }
            if (!ImageHeight.IsEmpty)
            {
                style.Append("max-height:" + ImageHeight);
                style.Append("; ");
            }
            style.Append("\"");

            var html = "<img id=\"" + ClientID + "_img\" src=\"" + ImageUrl + "\" " + style + " />";
            _literal.Text = html;

            if (Controls.Count == 0)
                Controls.Add(_literal);
        }
        #endregion

        /// <summary>
        ///     Resize image
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private Bitmap ResizeImage(Image imgToResize, Size size)
        {
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;

            float nPercent;

            var nPercentW = (size.Width / (float)sourceWidth);
            var nPercentH = (size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            var destWidth = (int)(sourceWidth * nPercent);
            var destHeight = (int)(sourceHeight * nPercent);

            var b = new Bitmap(destWidth, destHeight);
            var g = Graphics.FromImage(b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        private Bitmap ResizeImageExato(Image imgToResize, Size size)
        {
            var b = new Bitmap(size.Width, size.Height);
            using (var g = Graphics.FromImage(b))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }

            return b;
        }

        #region AddAttributesToRender
        /// <summary>
        ///     Adiciona os atributos necessários para o renderizador
        /// </summary>
        /// <param name="writer">O renderizador</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!string.IsNullOrEmpty(InitialSelection))
            {
                if (InitialSelection.Split(';').Length != 4)
                    throw new ImageSlicerException(InvalidInitialSelectionMessage);
            }

            if (!string.IsNullOrEmpty(AspectRatio))
            {
                if (AspectRatio.Split(':').Length != 2)
                    throw new ImageSlicerException(InvalidAspectRatioMessage);
            }

            if (!string.IsNullOrEmpty(InitialSelection))
            {
                // Tamanho dos atributos
                var arr = InitialSelection.Split(';');
                var rec = new Rectangle(Convert.ToInt32(arr[0]),
                    Convert.ToInt32(arr[1]),
                    Convert.ToInt32(arr[2]) - Convert.ToInt32(arr[0]),
                    Convert.ToInt32(arr[3]) - Convert.ToInt32(arr[1]));

                if (MaxWidth > 0)
                {
                    if (rec.Width > MaxWidth)
                        throw new ImageSlicerException("A largura da seleção inicial excede o largura máxima da área de seleção");
                }
                if (MaxHeight > 0)
                {
                    if (rec.Height > MaxHeight)
                        throw new ImageSlicerException("A altura da seleção inicial excede a altura máxima da área de seleção");
                }
            }


            writer.AddAttribute("maxW", Convert.ToString(MaxWidth));
            writer.AddAttribute("maxH", Convert.ToString(MaxHeight));

            if (!string.IsNullOrEmpty(AspectRatio))
                writer.AddAttribute("aspectRatio", AspectRatio);

            writer.AddAttribute("initialSelection", InitialSelection);
            base.AddAttributesToRender(writer);
        }
        #endregion

        #region Render
        /// <summary>
        ///     Renderiza o controle
        /// </summary>
        /// <param name="writer">O renderizador da página</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!DesignMode)
            {
                // Registra os descritores de script
                if (_sm != null)
                    _sm.RegisterScriptDescriptors(this);
            }

            CreateImageTag();
            base.Render(writer);
        }
        #endregion

        #region RenderContents
        /// <summary>
        ///     Renderiza o connteúdo do controle, comumente os controles filhos
        /// </summary>
        /// <param name="output">O renderizador da página</param>
        protected override void RenderContents(HtmlTextWriter output)
        {
            //output.Write(Text);
            base.RenderContents(output);
        }
        #endregion

        #region ValidateImage
        /// <summary>
        ///     Valida o MimeType de uma imagem, ou se consegue abrir o arquivo corretamente usando o objeto Image
        /// </summary>
        /// <param name="ba">A imagem</param>
        /// <returns>True se a imagem for válida, false do contrário</returns>
        public bool ValidateImage(byte[] ba)
        {
            if (ba == null)
                throw new ArgumentNullException("ba");

            Bitmap image = null;
            try
            {
                try
                {
                    image = Helpers.ByteToImage(ba) as Bitmap;
                }
                catch //Resultado esperando quando não está em um formato válido de imagem.
                {
                    throw new ImageSlicerException(InvalidImageFormatMessage);
                }

                if (image != null)
                {
                    if (MinAcceptedHeight != Unit.Empty)
                        if (image.Height < MinAcceptedHeight.Value)
                            throw new ImageSlicerException(InvalidImageMinimumSizeMessage);

                    if (MinAcceptedWidth != Unit.Empty)
                        if (image.Width < MinAcceptedWidth.Value)
                            throw new ImageSlicerException(InvalidImageMinimumSizeMessage);

                    return true;
                }
            }
            finally
            {
                if (image != null)
                    image.Dispose();
            }

            return false;
        }
        #endregion

        #region AdjustSize
        /// <summary>
        ///     Ajusta o tamanho height x width da imagem
        /// </summary>
        protected void AdjustSize()
        {
            if (ImageData != null)
            {
                var image = Helpers.ByteToImage(ImageData) as Bitmap;

                if (image != null)
                {
                    if (Width.Value > Height.Value)
                    {
                        if (image.Height > image.Width)
                        {
                            ImageWidth = Height;
                            ImageHeight = Width;
                        }
                        else
                        {
                            ImageWidth = Width;
                            ImageHeight = Height;
                        }
                    }
                    else if (Width.Value < Height.Value)
                    {
                        if (image.Height < image.Width)
                        {
                            ImageWidth = Height;
                            ImageHeight = Width;
                        }
                        else
                        {
                            ImageWidth = Width;
                            ImageHeight = Height;
                        }
                    }

                    if (image.Height < Height.Value)
                        ImageHeight = Unit.Pixel(image.Height);
                    if (image.Width < Width.Value)
                        ImageWidth = Unit.Pixel(image.Width);
                }
            }
        }
        #endregion

        #region SetImageData
        public void SetImageData(byte[] ba, bool validate = true)
        {
            if (validate && ba != null)
            {
                try
                {
                    if (!ValidateImage(ba))
                        throw new ImageSlicerException(InvalidImageFormatMessage);
                }
                catch (Exception)
                {
                    ImageData = null;
                    throw;
                }
            }
            if (ImageData != ba)
            {
                ImageData = ba;
                AdjustSize();
            }
        }
        #endregion

        #region CreateThumbnail
        /// <summary>
        ///     Cria o thumbnail
        /// </summary>
        /// <param name="x1">Cordenada x1 da caixa de seleção</param>
        /// <param name="x2">Cordenada x2 da caixa de seleção</param>
        /// <param name="y1">Cordenada y1 da caixa de seleção</param>
        /// <param name="y2">Cordenada y2 da caixa de seleção</param>
        /// <param name="w">Largura da caixa de seleção</param>
        /// <param name="h">Altura da caixa de seleção</param>
        /// <param name="ba">O byte array da imagem</param>
        /// <returns>O endereço relativo do thumbnail</returns>
        public void CreateThumbnail(int x1, int x2, int y1, int y2, int w, int h, byte[] ba)
        {
            if (ba == null)
                return;

            Image imgSrc = null;
            Image imgTarget = null;

            try
            {
                // Cria o retângulo de corte
                var cropRect = new Rectangle(x1, y1, x2 - x1, y2 - y1);

                imgSrc = Helpers.ByteToImage(ba) as Bitmap;

                if (ImageWidth != Unit.Empty || ImageHeight != Unit.Empty)
                {
                    var iW = ImageWidth.Value;
                    var iH = ImageHeight.Value;

                    // Checagem  de segurança
                    if (imgSrc != null)
                    {
                        if (iW == 0)
                            iW = imgSrc.Width;
                        if (iH == 0)
                            iH = imgSrc.Height;

                        // Checagem de proporção
                        double ratio;
                        if (iW < iH)
                        {
                            ratio = iW / imgSrc.Width;
                            iH = imgSrc.Height * ratio;
                        }
                        else
                        {
                            ratio = iH / imgSrc.Height;
                            iW = imgSrc.Width * ratio;
                        }
                    }

                    // Verificação de tamanho
                    if (iW > ImageWidth.Value)
                        iW = ImageWidth.Value;
                    if (iH > Height.Value)
                        iH = Height.Value;

                    using (var temp = new Bitmap((int)Math.Round(iW), (int)Math.Round(iH)))
                    {
                        var gr = Graphics.FromImage(temp);
                        if (imgSrc != null)
                        {
                            gr.DrawImage(imgSrc, 0, 0, temp.Width, temp.Height);
                        }
                        gr.Dispose();

                        // Correção do retângulo de corte
                        if (cropRect.Right > temp.Width)
                        {
                            cropRect.Width -= (cropRect.Right - temp.Width);
                        }
                        if (cropRect.Bottom > temp.Height)
                            cropRect.Height -= (cropRect.Bottom - temp.Height);

                        // Clona para a imagem de destino utilizando o pixel format
                        imgTarget = Crop(temp, cropRect);
                    }
                }
                else
                {
                    // Clona para a imagem de destino utilizando o pixel format
                    if (imgSrc != null) imgTarget = Crop(imgSrc, cropRect);
                }

                //aumentar ou reduzir a imagem
                if (ResizeImageWidth.HasValue && ResizeImageHeight.HasValue)
                {
                    var s = new Size(ResizeImageWidth.Value, ResizeImageHeight.Value);
                    if (RedimencionaExato)
                        imgTarget = ResizeImageExato(imgTarget, s);
                    else
                        imgTarget = ResizeImage(imgTarget, s);
                }

                SetImageData(Helpers.ImageToByte(imgTarget), false);
            }
            finally
            {
                // Liberar a memória
                if (imgSrc != null)
                    imgSrc.Dispose();
                if (imgTarget != null)
                    imgTarget.Dispose();
            }
        }
        #endregion

        private static Image Crop(Image imgSrc, Rectangle cropRectangle)
        {
            var src = new Bitmap(imgSrc);
            var target = src.Clone(cropRectangle, src.PixelFormat);
            return target;
        }

        #region Clear
        /// <summary>
        ///     Limpa imagem e thumbnails
        /// </summary>
        public void Clear()
        {
            ImageData = null;
        }
        #endregion

        #endregion

        #region Events

        #region OnLoad
        /// <summary>
        ///     Fase de load do controle
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude("JQueryImageSelectionPlugin", Page.ClientScript.GetWebResourceUrl(typeof(ImageSlicer), Resources.JQueryImageSelectionPlugin));
            // Trata os eventos customizados durante o ciclo de postback
            if (Page.IsPostBack && Visible)
            {
                var pbarg = Convert.ToString(Page.Request["__EVENTARGUMENT"]);
                // Verifica o evento setImage
                if (!string.IsNullOrWhiteSpace(pbarg) && pbarg.IndexOf("setImage", StringComparison.Ordinal) > -1)
                {
                    // Controla para não haver dois pb iguais
                    var old = Convert.ToString(ViewState["odlEvent"]);
                    if (!string.IsNullOrEmpty(old))
                    {
                        if (old.Equals(pbarg))
                            return;
                    }
                    ViewState["odlEvent"] = pbarg;

                    // Split para pegar os parâmetros
                    var ar = pbarg.Split(';');

                    if (ar.Length != 8)
                        throw new ImageSlicerException("Houve um erro ao processar a requisição: Número inconsistente de argumentos durante a requisição. Número de argumentos: " + ar.Length);

                    // Cria o thumbail
                    // Garantir somente números inteiros do lado do servidor
                    CreateThumbnail(Convert.ToInt32(ar[1].Split('.')[0]),
                        Convert.ToInt32(ar[2].Split('.')[0]),
                        Convert.ToInt32(ar[3].Split('.')[0]),
                        Convert.ToInt32(ar[4].Split('.')[0]),
                        Convert.ToInt32(ar[5].Split('.')[0]),
                        Convert.ToInt32(ar[6].Split('.')[0]), ImageData);

                    // Verifica se o evento foi tratado, em caso afirmativo dispara o evento
                    if (ThumbnailCreated != null)
                        ThumbnailCreated(this, new ThumbnailCreatedArgs(ImageData, ImageUrl));
                }
            }

            base.OnLoad(e);
        }
        #endregion

        #region OnPreRender
        /// <summary>
        ///     Fase de pré-renderização
        /// </summary>
        protected override void OnPreRender(EventArgs e)
        {
            // Pega o script manager da página atual
            _sm = ScriptManager.GetCurrent(Page);
            if (_sm == null)
                throw new ImageSlicerException("É necessário ter um ScriptManager ou RadScriptManager registrado na página para poder prosseguir");

            // Registra o controle caso não esteja em tempo de design
            if (!DesignMode)
            {
                _sm.RegisterScriptControl(this);
            }

            base.OnPreRender(e);
        }
        #endregion

        #endregion
    }
}