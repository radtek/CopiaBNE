using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BNE.Componentes.Extensions;
using BNE.Componentes.Util;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using System.Net;
using System.IO;
using System.Collections;
using BNE.Componentes.EL;
using System.Drawing.Drawing2D;

namespace BNE.Componentes
{
    /// <summary>
    /// Classe de recorte de foto
    /// </summary>
    public class ImageSlicer : WebControl, IScriptControl
    {
        #region Fields
        private ScriptManager sm = null; // Referencia para o script manager da páginas
        private LiteralControl literal = null; // Literal que mostra as tagsl de imagem        
        #endregion

        #region Properties

        #region AcceptedTypes
        /// <summary>
        /// As extensões aceitas pelo controle
        /// </summary>
        public static String[] AcceptedTypes = { ".jpg", ".jpeg", ".gif", ".png" };
        #endregion

        #region ImageUrl
        /// <summary>
        /// Url da imagem atualmente no corte
        /// </summary>
        public string ImageUrl
        {
            get
            {
                return Convert.ToString(this.ViewState[Keys.General.ImageUrl.ToString()]);
            }
            set
            {
                // Se a viewstate e o value não estiverem vazios...
                if (!String.IsNullOrEmpty(value))
                {
                    // Verifica se o valor difere do valor que está sendo setado
                    if (!Convert.ToString(this.ViewState[Keys.General.ImageUrl.ToString()])
                        .Equals(value, StringComparison.OrdinalIgnoreCase))
                    {
                        // Reseta o thumbnail atual
                        this.ThumbUrl = String.Empty;

                        String[] arr = ImageSlicer.AcceptedTypes.ReplaceAll(".", "image/");
                        // Sendo diferente valida a imagem
                        try
                        {
                            if (!this.ValidateImage(value))
                                // Lança a excessão
                                throw new ImageSlicerException(ImageSlicer.InvalidImageFormatMessage);
                        }
                        catch (Exception)
                        {
                            // Limpar a ImageUrl
                            this.ViewState[Keys.General.ImageUrl.ToString()] = String.Empty;
                            throw;
                        }
                    }
                }

                // Tudo ocorreu bem.
                this.ViewState[Keys.General.ImageUrl.ToString()] = value;
            }
        }
        #endregion

        #region ThumbUrl
        /// <summary>
        /// Utl da thumbnail atual
        /// </summary>
        public string ThumbUrl
        {
            set { this.ViewState[Keys.General.ThumbUrl.ToString()] = value; }
            get { return Convert.ToString(this.ViewState[Keys.General.ThumbUrl.ToString()]); }
        }
        #endregion

        #region ThumbDir
        /// <summary>
        /// Diretório onde o thumbnail será salvo temporariamente
        /// </summary>
        public String ThumbDir
        {
            get
            {
                String td = Convert.ToString(this.ViewState[Keys.General.ThumbDir.ToString()]);
                if (String.IsNullOrEmpty(td))
                    return "~/thumbs/";
                return td;
            }
            set { this.ViewState[Keys.General.ThumbDir.ToString()] = value; }
        }
        #endregion

        #region MaxWidth
        /// <summary>
        /// Largura máximo do retângulo de seleção
        /// </summary>
        public int MaxWidth
        {
            get
            {
                if (this.ViewState[Keys.General.MaxWidth.ToString()] == null)
                    return 150;
                return Convert.ToInt32(this.ViewState[Keys.General.MaxWidth.ToString()]);
            }
            set { this.ViewState[Keys.General.MaxWidth.ToString()] = value; }
        }
        #endregion

        #region MaxHeight
        /// <summary>
        /// Largura máxima do retângulo de seleção
        /// </summary>
        public int MaxHeight
        {
            get
            {
                if (this.ViewState[Keys.General.MaxHeight.ToString()] == null)
                    return 200;

                return Convert.ToInt32(this.ViewState[Keys.General.MaxHeight.ToString()]);
            }
            set { this.ViewState[Keys.General.MaxHeight.ToString()] = value; }
        }
        #endregion

        #region LatestThumbnails
        /// <summary>
        /// Retorna os ultimos thumbnails criados pelo componente
        /// </summary>
        public StringCollection LatestThumbnails
        {
            get
            {

                if (this.ViewState["lastThumbs"] == null)
                    this.ViewState["lastThumbs"] = new StringCollection();

                return (StringCollection)this.ViewState["lastThumbs"];
            }
        }
        #endregion

        #region AspectRatio
        /// <summary>
        /// Proporção da área de corte da imagem. Deve estar no seguinte formato: "x:y", onde <br/>
        /// x: proporção de largura<br/>
        /// y: proporção de altura<br/>
        /// </summary>
        public String AspectRatio
        {
            get { return Convert.ToString(this.ViewState[Keys.Images.AspectRatio.ToString()]); }
            set { this.ViewState[Keys.Images.AspectRatio.ToString()] = value; }
        }
        #endregion

        #region InitialSelection
        /// <summary>
        /// Define a área inicial selecionada no seguinte formato: "x1;y1;x2;y2;" onde:<br/>
        /// x1 e x2: posições x (inicial e final) do controle<br/> 
        /// y1 e y2: posições y (inicial e final) do controle<br/> 
        /// </summary>
        public String InitialSelection
        {
            get { return Convert.ToString(this.ViewState["initialSelection"]); }
            set { this.ViewState["initialSelection"] = value; }
        }
        #endregion

        #region InvalidAspectRatioMessage
        /// <summary>
        /// Mensagem de erro de proporção inválida
        /// </summary>
        private string InvalidAspectRatioMessage
        {

            get
            {
                if (String.IsNullOrEmpty(Convert.ToString(this.ViewState[Keys.Images.InvalidAspectRatio.ToString()])))
                    return Resources.ImageSlicerInvalidAspectRatio;
                return Convert.ToString(this.ViewState[Keys.Images.InvalidAspectRatio.ToString()]);
            }
            set
            {
                this.ViewState[Keys.Images.InvalidAspectRatio.ToString()] = value;
            }
        }
        #endregion

        #region InvalidInitialSelectionMessage
        /// <summary>
        /// Mensagem de erro de seleção inicial inválida
        /// </summary>
        public string InvalidInitialSelectionMessage
        {
            get
            {
                if (String.IsNullOrEmpty(Convert.ToString(this.ViewState[Keys.Images.InvalidInitialSelection.ToString()])))
                    return Resources.ImageSlicerInvalidInitialSelection;
                return Convert.ToString(this.ViewState[Keys.Images.InvalidInitialSelection.ToString()]);
            }
            set
            {
                this.ViewState[Keys.Images.InvalidInitialSelection.ToString()] = value;
            }
        }
        #endregion

        #region InvalidImageFormatMessage
        /// <summary>
        /// Mensagem de erro de formato inválido de mensagem
        /// </summary>
        public static string InvalidImageFormatMessage
        {
            get
            {
                String[] arr = ImageSlicer.AcceptedTypes.ReplaceAll(".", String.Empty);
                return String.Format(Resources.ImageSlicerErrorImageType, String.Join(", ", arr).ToUpperInvariant());
            }
        }
        #endregion

        #region InvalidImageMinimumSizeMessage
        /// <summary>
        /// Mensagem de erro de tamanho mínimo inválido
        /// </summary>
        public string InvalidImageMinimumSizeMessage
        {
            get
            {
                if (String.IsNullOrEmpty(Convert.ToString(this.ViewState[Keys.Images.InvalidMinimumImageSizeMessage.ToString()])))
                    return Resources.InvalidMinSizeImage;
                return Convert.ToString(this.ViewState[Keys.Images.InvalidMinimumImageSizeMessage.ToString()]);
            }
            set
            {
                this.ViewState[Keys.Images.InvalidMinimumImageSizeMessage.ToString()] = value;
            }


        }

        #endregion

        #region ImageWidth
        /// <summary>
        /// Largura real da imagem tratado dentro do controle
        /// </summary>
        protected Unit ImageWidth
        {
            get
            {
                if (this.ViewState[Keys.Images.ImageWidth.ToString()] == null)
                    return Unit.Empty;
                else
                    return (Unit)this.ViewState[Keys.Images.ImageWidth.ToString()];
            }
            set
            {
                this.ViewState[Keys.Images.ImageWidth.ToString()] = value;
            }
        }
        #endregion

        #region ImageHeight
        /// <summary>
        /// Altura real da imagem tratado dentro do controle
        /// </summary>
        protected Unit ImageHeight
        {
            get
            {
                if (this.ViewState[Keys.Images.ImageHeight.ToString()] == null)
                    return Unit.Empty;
                else
                    return (Unit)this.ViewState[Keys.Images.ImageHeight.ToString()];
            }
            set
            {
                this.ViewState[Keys.Images.ImageHeight.ToString()] = value;
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
                if (this.ViewState["ResizeImageWidth"] == null)
                    return null;

                return (int?)this.ViewState["ResizeImageWidth"];
            }
            set
            {
                this.ViewState["ResizeImageWidth"] = value;
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
                if (this.ViewState["ResizeImageHeight"] == null)
                    return null;

                return (int?)this.ViewState["ResizeImageHeight"];
            }
            set
            {
                this.ViewState["ResizeImageHeight"] = value;
            }
        }
        #endregion

        #region MinAcceptedWidth
        /// <summary>
        /// Largura mínima aceita pelo controle
        /// </summary>
        public Unit MinAcceptedWidth
        {
            get
            {
                if (this.ViewState[Keys.Images.MinAcceptedWidth.ToString()] == null)
                    return Unit.Empty;
                return (Unit)this.ViewState[Keys.Images.MinAcceptedWidth.ToString()];

            }
            set
            {
                this.ViewState[Keys.Images.MinAcceptedWidth.ToString()] = value;
            }
        }
        #endregion

        #region MinAcceptedHeight
        /// <summary>
        /// Altura mínima aceita pelo controle
        /// </summary>
        public Unit MinAcceptedHeight
        {
            get
            {
                if (this.ViewState[Keys.Images.MinAcceptedHeight.ToString()] == null)
                    return Unit.Empty;
                return (Unit)this.ViewState[Keys.Images.MinAcceptedHeight.ToString()];
            }
            set
            {
                this.ViewState[Keys.Images.MinAcceptedHeight.ToString()] = value;
            }
        }
        #endregion

        #region RedimencionaExato
        public bool RedimencionaExato
        {
            get { return ViewState["RedimencionaExato"] != null ? (bool)ViewState["RedimencionaExato"] : false; }
            set { ViewState["RedimencionaExato"] = value; }
        }
        #endregion

        #region Percentual
        /// <summary>
        /// Proporção da imagem original para a área de corte da imagem.
        /// </summary>
        public decimal Percentual
        {
            get { return Convert.ToDecimal(this.ViewState[Keys.Images.Percentual.ToString()]); }
            set { this.ViewState[Keys.Images.Percentual.ToString()] = value; }
        }
        #endregion

        #endregion

        #region Methods

        #region GetScriptDescriptors
        /// <summary>
        /// Retorna os descritores de script
        /// </summary>
        /// <returns>Coleção dos descritores de script utilizados pelo controle</returns>
        public IEnumerable<ScriptDescriptor> GetScriptDescriptors()
        {
            ScriptControlDescriptor descriptor = new ScriptControlDescriptor("ImgResizer", this.ClientID);
            return new ScriptControlDescriptor[] { descriptor };
        }
        #endregion

        #region GetScriptReferences
        /// <summary>
        /// Retorna as referencias aos scripts usados pelo controle
        /// </summary>
        /// <returns>Coleção das referencias de scripts utilizdos pelo controle</returns>
        public IEnumerable<ScriptReference> GetScriptReferences()
        {
            ScriptReference reference = new ScriptReference();
            reference.Assembly = "BNE.Componentes";
            reference.Name = "BNE.Componentes.Content.js.ImgResizer.js";

            return new ScriptReference[] { reference };
        }
        #endregion

        #region CreateChildControls
        /// <summary>
        /// Inicia os controles filhos utilizados dentro deste controle
        /// </summary>
        protected override void CreateChildControls()
        {
            if (this.ChildControlsCreated)
                return;

            this.CreateImageTag();


            base.CreateChildControls();
        }
        #endregion

        #region CreateImageTag
        /// <summary>
        /// Cria a tag de imagem
        /// </summary>
        private void CreateImageTag()
        {
            if (this.literal == null)
                this.literal = new LiteralControl();

            String url = this.ImageUrl;

            if (url.IndexOf("http://") == -1 && url.IndexOf("www.") == -1)
            {
                if (url.IndexOf("~") == -1)
                    url = this.Page.ResolveClientUrl(("~/" + url).Replace("//", "/"));
                else
                    url = this.Page.ResolveClientUrl(url);
            }
            else
                if (url.IndexOf("http://") == -1)
                    url = "http://" + url;


            StringBuilder style = new StringBuilder();
            style.Append("style=\"");
            if (!this.ImageWidth.IsEmpty)
            {
                style.Append("max-width:" + this.ImageWidth.ToString());
                style.Append("; ");
            }
            if (!this.ImageHeight.IsEmpty)
            {
                style.Append("max-height:" + this.ImageHeight.ToString());
                style.Append("; ");
            }
            //style.Append("border: 1px double black; ");
            style.Append("\"");

            // Referencia do evento postback            
            String html = "<img id=\"" + this.ClientID + "_img\" src=\"" + url + "\" " + style.ToString() + " />";
            this.literal.Text = html;

            if (this.Controls.Count == 0)
                this.Controls.Add(this.literal);

        }
        #endregion

        /// <summary>
        /// Resize image
        /// </summary>
        /// <param name="imgToResize"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private Bitmap ResizeImage(System.Drawing.Image imgToResize, Size size)
        {
            int sourceWidth = imgToResize.Width;
            int sourceHeight = imgToResize.Height;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)size.Width / (float)sourceWidth);
            nPercentH = ((float)size.Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
                nPercent = nPercentH;
            else
                nPercent = nPercentW;

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap b = new Bitmap(destWidth, destHeight);
            Graphics g = Graphics.FromImage((System.Drawing.Image)b);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
            g.Dispose();

            return b;
        }

        private Bitmap ResizeImageExato(System.Drawing.Image imgToResize, Size size)
        {
            Bitmap b = new Bitmap(size.Width, size.Height);
            using (Graphics g = Graphics.FromImage((System.Drawing.Image)b))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                g.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }

            return b;
        }

        #region AddAttributesToRender
        /// <summary>
        /// Adiciona os atributos necessários para o renderizador
        /// </summary>
        /// <param name="writer">O renderizador</param>
        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            if (!String.IsNullOrEmpty(this.InitialSelection))
            {
                if (this.InitialSelection.Split(';').Length != 4)
                    throw new ImageSlicerException(this.InvalidInitialSelectionMessage);
            }

            if (!String.IsNullOrEmpty(this.AspectRatio))
            {
                if (this.AspectRatio.Split(':').Length != 2)
                    throw new ImageSlicerException(this.InvalidAspectRatioMessage);
            }

            if (!String.IsNullOrEmpty(this.InitialSelection))
            {
                // Tamanho dos atributos
                String[] arr = this.InitialSelection.Split(';');
                Rectangle rec = new Rectangle(Convert.ToInt32(arr[0]),
                    Convert.ToInt32(arr[1]),
                    Convert.ToInt32(arr[2]) - Convert.ToInt32(arr[0]),
                    Convert.ToInt32(arr[3]) - Convert.ToInt32(arr[1]));

                if (this.MaxWidth > 0)
                {
                    if (rec.Width > this.MaxWidth)
                        throw new ImageSlicerException("A largura da seleção inicial excede o largura máxima da área de seleção");
                }
                if (this.MaxHeight > 0)
                {
                    if (rec.Height > this.MaxHeight)
                        throw new ImageSlicerException("A altura da seleção inicial excede a altura máxima da área de seleção");
                }
            }


            writer.AddAttribute("maxW", Convert.ToString(this.MaxWidth));
            writer.AddAttribute("maxH", Convert.ToString(this.MaxHeight));

            if (!String.IsNullOrEmpty(this.AspectRatio))
                writer.AddAttribute("aspectRatio", this.AspectRatio);

            writer.AddAttribute("initialSelection", this.InitialSelection);
            base.AddAttributesToRender(writer);
        }
        #endregion

        #region Render
        /// <summary>
        /// Renderiza o controle
        /// </summary>
        /// <param name="writer">O renderizador da página</param>
        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.DesignMode)
            {
                // Registra os descritores de script
                if (sm != null)
                    this.sm.RegisterScriptDescriptors(this);
            }

            this.CreateImageTag();
            base.Render(writer);
        }
        #endregion

        #region RenderContents
        /// <summary>
        /// Renderiza o connteúdo do controle, comumente os controles filhos
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
        /// Valida o MimeType de uma imagem, ou se consegue abrir o arquivo corretamente usando o objeto Image
        /// </summary>
        /// <param name="imgUrl">A Url da imagem</param>
        /// <returns>True se a imagem for válida, false do contrário</returns>
        protected bool ValidateImage(String imgUrl)
        {
            if (String.IsNullOrEmpty(imgUrl))
                throw new ArgumentNullException("imgUrl");

            Bitmap image = null;
            try
            {
                // Mapeia o caminho da imagem no servidor
                string src = String.Empty;

                if (imgUrl.IndexOf("http://") == -1 && imgUrl.IndexOf("www.") == -1)
                {
                    if (imgUrl.IndexOf("~") == -1)
                        src = HttpContext.Current.Server.MapPath(("~/" + imgUrl).Replace("//", "/"));
                    else
                        src = HttpContext.Current.Server.MapPath(imgUrl);
                }
                else
                    if (imgUrl.IndexOf("http://") == -1)
                        src = "http://" + imgUrl;
                    else
                        src = imgUrl;

                // Carrega a imagem de origem
                if (imgUrl.IndexOf("http://") == -1 && imgUrl.IndexOf("www.") == -1)
                    try
                    {
                        // Abrir a imagem para ler dados posteriormente
                        image = System.Drawing.Image.FromFile(src) as Bitmap;
                    }
                    catch (Exception)
                    {
                        // Erro ao carregar a imagem
                        return false;
                    }

                else
                {
                    HttpWebRequest req = null;
                    WebResponse resp = null;
                    try
                    {
                        // Cria a request
                        req = (HttpWebRequest)WebRequest.Create(src);
                        // Resposta...
                        resp = req.GetResponse();

                        String[] arr = ImageSlicer.AcceptedTypes.ReplaceAll(".", "image/");

                        //if (!resp.ContentType.ContainsAny(arr))
                        // return false;

                        // Abrir a imagem para ler dados posteriormente
                        image = ImageSlicer.OpenImageFromUrl(src) as Bitmap;
                    }
                    finally
                    {
                        if (resp != null)
                            resp.Close();
                    }
                }

                // A imagem não pode chegar como null aqui.
                // Se chegou houve algum erro então não deve deixar o processo seguir
                if (image == null)
                    throw new ImageSlicerException("Erro desconhecido ao abrir a imagem");

                // Validação do tamanho da imagem

                if (this.MinAcceptedHeight != Unit.Empty)
                    if (image.Height < this.MinAcceptedHeight.Value)
                        throw new ImageSlicerException(this.InvalidImageMinimumSizeMessage);

                if (this.MinAcceptedWidth != Unit.Empty)
                    if (image.Width < this.MinAcceptedWidth.Value)
                        throw new ImageSlicerException(this.InvalidImageMinimumSizeMessage);


                if (this.Width.Value > this.Height.Value)
                {
                    if (image.Height > image.Width)
                    {
                        this.ImageWidth = this.Height;
                        this.ImageHeight = this.Width;
                    }
                    else
                    {
                        this.ImageWidth = this.Width;
                        this.ImageHeight = this.Height;
                    }
                }
                else
                    if (this.Width.Value < this.Height.Value)
                    {
                        if (image.Height < image.Width)
                        {
                            this.ImageWidth = this.Height;
                            this.ImageHeight = this.Width;
                        }
                        else
                        {
                            this.ImageWidth = this.Width;
                            this.ImageHeight = this.Height;
                        }
                    }

                if (image.Height < this.Height.Value)
                    this.ImageHeight = Unit.Pixel(image.Height);
                if (image.Width < this.Width.Value)
                    this.ImageWidth = Unit.Pixel(image.Width);

                return true;
            }
            finally
            {
                if (image != null)
                    image.Dispose();
            }
        }
        #endregion

        #region OpenImageFromUrl
        /// <summary>
        /// Retorna um objeto Image a partir de uma url
        /// </summary>
        /// <param name="url">A url</param>
        /// <returns>Uma instância de System.Drawing.Image contendo a imagem apontada pela url</returns>
        public static System.Drawing.Image OpenImageFromUrl(String url)
        {
            System.Drawing.Image img = null;
            HttpWebRequest req = null;
            WebResponse resp = null;
            try
            {
                // Cria a request
                req = (HttpWebRequest)WebRequest.Create(url);
                // Resposta...
                resp = req.GetResponse();

                String[] arr = ImageSlicer.AcceptedTypes.ReplaceAll(".", "image/");

                //if (!resp.ContentType.ContainsAny(arr))
                // throw new ImageSlicerException(ImageSlicer.InvalidImageFormatMessage);

                // Carrega a imagem da stream
                Stream objStream = resp.GetResponseStream();
                img = System.Drawing.Image.FromStream(objStream);
                return img;
            }
            catch (Exception ex)
            {
                throw new ImageSlicerException("Erro ao carregar a imagem: " + ex.Message, ex);
            }
            finally
            {
                if (resp != null)
                    resp.Close();
            }
        }
        #endregion

        #region GetCurrentThumbnail
        /// <summary>
        /// Retorna um objeto Image a partir da url do thumbnail atual
        /// </summary>
        /// <returns>Uma instãncia de System.Drawing.Image contendo a imagem apontada pelo thumbnail atual</returns>
        public System.Drawing.Image GetCurrentThumbnail()
        {
            return System.Drawing.Image.FromFile(this.Page.MapPath(this.ThumbUrl));
        }
        #endregion

        #region GetCurrentThumbnailStream
        /// <summary>
        /// Retorna um objeto Stream a partir de da url do thumbnail atual
        /// </summary>
        /// <returns>Uma instância de System.IO.Stream contendo a imagem apontada pelo thumbnail atual</returns>
        public Stream GetCurrentThumbnailStream()
        {
            return new FileStream(this.Page.MapPath(this.ThumbUrl), FileMode.Open);
        }
        #endregion

        #region CreateThumbnail
        /// <summary>
        /// Cria o thumbnail
        /// </summary>
        /// <param name="x1">Cordenada x1 da caixa de seleção</param>
        /// <param name="x2">Cordenada x2 da caixa de seleção</param>
        /// <param name="y1">Cordenada y1 da caixa de seleção</param>
        /// <param name="y2">Cordenada y2 da caixa de seleção</param>
        /// <param name="w">Largura da caixa de seleção</param>
        /// <param name="h">Altura da caixa de seleção</param>
        /// <param name="imgUrl">A Url relativa da imagem que originará o thumbnail</param>
        /// <returns>O endereço relativo do thumbnail</returns>
        public String CreateThumbnail(int x1, int x2, int y1, int y2, int w, int h, String imgUrl)
        {
            //TODO: Refatorar essa parte da aplicação do percentual.
            //Ajusta levando em conta o percentual
            x1 = (int)(x1 / Percentual);
            x2 = (int)(x2 / Percentual);
            y1 = (int)(y1 / Percentual);
            y2 = (int)(y2 / Percentual);
            w = (int)(w * Percentual);
            h = (int)(h * Percentual);

            // Mapeia o caminho da imagem no servidor
            string src = String.Empty;

            if (imgUrl.IndexOf("http://") == -1 && imgUrl.IndexOf("www.") == -1)
            {
                if (imgUrl.IndexOf("~") == -1)
                    src = HttpContext.Current.Server.MapPath(("~/" + imgUrl).Replace("//", "/"));
                else
                    src = HttpContext.Current.Server.MapPath(imgUrl);
            }
            else
                if (imgUrl.IndexOf("http://") == -1)
                    src = "http://" + imgUrl;
                else
                    src = imgUrl;

            if (String.IsNullOrEmpty(src))
                return String.Empty;

            Bitmap imgSrc = null;
            Bitmap imgTarget = null;

            try
            {
                // Cria o retângulo de corte
                Rectangle cropRect = new Rectangle(x1, y1, x2 - x1, y2 - y1);
                // Carrega a imagem de origem
                if (imgUrl.IndexOf('~') > -1)
                    imgSrc = System.Drawing.Image.FromFile(src) as Bitmap;
                else
                    if (imgUrl.IndexOf("http://") == -1 && imgUrl.IndexOf("www.") == -1)
                        imgSrc = System.Drawing.Image.FromFile(src) as Bitmap;
                    else
                        imgSrc = ImageSlicer.OpenImageFromUrl(src) as Bitmap;

                if (this.ImageWidth != Unit.Empty || this.ImageHeight != Unit.Empty)
                {
                    double iW = this.ImageWidth.Value / (double)Percentual;
                    double iH = this.ImageHeight.Value / (double)Percentual;
                    double ratio = 1;

                    // Checagem  de segurança
                    if (iW == 0)
                        iW = imgSrc.Width;
                    if (iH == 0)
                        iH = imgSrc.Height;


                    // Checagem de proporção
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

                    // Verificação de tamanho
                    if (iW > this.ImageWidth.Value)
                        iW = this.ImageWidth.Value / (double)Percentual;
                    if (iH > this.Height.Value)
                        iH = this.Height.Value / (double)Percentual;


                    using (Bitmap temp = new Bitmap((int)Math.Round(iW), (int)Math.Round(iH)))
                    {
                        Graphics gr = Graphics.FromImage((temp as System.Drawing.Image));
                        gr.DrawImage(imgSrc, 0, 0, temp.Width, temp.Height);
                        gr.Dispose();
                        //temp.Save(HttpContext.Current.Server.MapPath(this.ThumbDir + "teste.png"));

                        // Correção do retângulo de corte
                        if (cropRect.Right > temp.Width)
                        {
                            cropRect.Width -= (cropRect.Right - temp.Width);
                        }
                        if (cropRect.Bottom > temp.Height)
                            cropRect.Height -= (cropRect.Bottom - temp.Height);

                        // Clona para a imagem de destino utilizando o pixel format                
                        imgTarget = temp.Clone(cropRect, temp.PixelFormat);
                    }
                }
                else
                {
                    // Clona para a imagem de destino utilizando o pixel format                
                    imgTarget = imgSrc.Clone(cropRect, imgSrc.PixelFormat);
                }

                //aumentar ou reduzir a imagem
                if (this.ResizeImageWidth.HasValue && this.ResizeImageHeight.HasValue)
                {
                    Size s = new Size(this.ResizeImageWidth.Value, this.ResizeImageHeight.Value);
                    if (RedimencionaExato)
                        imgTarget = ResizeImageExato(imgTarget, s);
                    else
                        imgTarget = ResizeImage(imgTarget, s);
                }

                // Cria a url relativa onde será salva a imagem
                String nm = "Thumb" + DateTime.Now.ToString("hhmmss") + ".png";
                // Salva mepando o arquivo do servidor
                imgTarget.Save(HttpContext.Current.Server.MapPath(this.ThumbDir + nm));
                // Atualiza o endereço da imagem
                this.ThumbUrl = this.ThumbDir + nm;
            }
            finally
            {
                // Liberar a memória
                if (imgSrc != null)
                    imgSrc.Dispose();
                if (imgTarget != null)
                    imgTarget.Dispose();
            }

            // Histórico de thumbs criados
            this.LatestThumbnails.Add(this.ThumbUrl);
            // Retorna o endereço
            return this.ThumbUrl;
        }
        #endregion

        #region Clear
        /// <summary>
        /// Limpa imagem e thumbnails
        /// </summary>
        public void Clear()
        {
            this.ImageUrl = String.Empty;
            this.ThumbUrl = String.Empty;
            this.LatestThumbnails.Clear();
        }
        #endregion

        #endregion

        #region EventHandler
        /// <summary>
        /// Evento disparado quando um thumbnail for criado
        /// </summary>
        public event ThumbnailCreatedEvent ThumbnailCreated;
        #endregion

        #region Events
        #region OnLoad
        /// <summary>
        /// Fase de load do controle
        /// </summary>        
        protected override void OnLoad(EventArgs e)
        {
            this.Page.ClientScript.RegisterClientScriptInclude("JQueryImageSelectionPlugin", this.Page.ClientScript.GetWebResourceUrl(typeof(ImageSlicer), Resources.JQueryImageSelectionPlugin));
            // Trata os eventos customizados durante o ciclo de postback
            if (this.Page.IsPostBack && this.Visible)
            {
                String pbarg = Convert.ToString(this.Page.Request["__EVENTARGUMENT"]);
                // Verifica o evento setImage
                if (pbarg.IndexOf("setImage") > -1)
                {

                    // Controla para não haver dois pb iguais....
                    String old = Convert.ToString(this.ViewState["odlEvent"]);
                    if (!String.IsNullOrEmpty(old))
                    {
                        if (old.Equals(pbarg))
                            return;
                    }
                    this.ViewState["odlEvent"] = pbarg;

                    // Split para pegar os parâmetros
                    String[] ar = pbarg.Split(';');

                    if (ar.Length != 8)
                        throw new ImageSlicerException("Houve um erro ao processar a requisição: Número inconsistente de argumentos durante a requisição. Número de argumentos: " + ar.Length);

                    // Cria o thumbail
                    // Garantir somente números inteiros do lado do servidor
                    this.CreateThumbnail(Convert.ToInt32(ar[1].Split('.')[0]),
                        Convert.ToInt32(ar[2].Split('.')[0]),
                        Convert.ToInt32(ar[3].Split('.')[0]),
                        Convert.ToInt32(ar[4].Split('.')[0]),
                        Convert.ToInt32(ar[5].Split('.')[0]),
                        Convert.ToInt32(ar[6].Split('.')[0]), this.ImageUrl);

                    // Verifica se o evento foi tratado, em caso afirmativo dispara o evento
                    if (this.ThumbnailCreated != null)
                        this.ThumbnailCreated(this, new ThumbnailCreatedArgs(this.ThumbUrl, this.Page.ResolveClientUrl(this.ThumbUrl)));
                }
            }

            base.OnLoad(e);
        }
        #endregion

        #region OnPreRender
        /// <summary>
        /// Fase de pré-renderização
        /// </summary>        
        protected override void OnPreRender(EventArgs e)
        {
            // Pega o script manager da página atual
            this.sm = ScriptManager.GetCurrent(this.Page);
            if (this.sm == null)
                throw new ImageSlicerException("É necessário ter um ScriptManager ou RadScriptManager registrado na página para poder prosseguir");

            // Registra o controle caso não esteja em tempo de design
            if (!this.DesignMode)
            {
                this.sm.RegisterScriptControl(this);
            }

            base.OnPreRender(e);
        }
        #endregion
        #endregion
    }
}
