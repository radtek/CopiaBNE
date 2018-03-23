using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using BNE.Web.Properties;

namespace BNE.Web.Handlers
{
    /// <summary>
    /// Handler para logotipos de pessoa jurídica
    /// </summary>
    public class PessoaJuridicaLogo : IHttpHandler
    {

        private HttpContext _context;

        #region Propriedades

        #region Width
        private int? Width
        {
            get
            {
                if (!string.IsNullOrEmpty(_context.Request.QueryString["width"]))
                    return Convert.ToInt32(_context.Request.QueryString["width"]);
                return null;
            }
        }
        #endregion Width

        #region Height
        private int? Height
        {
            get
            {
                if (!string.IsNullOrEmpty(_context.Request.QueryString["height"]))
                    return Convert.ToInt32(_context.Request.QueryString["height"]);
                return null;
            }
        }
        #endregion Height

        #region MaxSize
        private Size MaxSize
        {
            get
            {
                if (Width != null && Height != null)
                    return new Size(Width.Value, Height.Value);
                return Size.Empty;
            }
        }
        #endregion MaxSize

        #endregion

        #region ProcessRequest
        public void ProcessRequest(HttpContext context)
        {
            _context = context;

            DateTime delay = DateTime.Now.AddMinutes(-Settings.Default.PessoaJuridicaLogoDelayLimparCacheMinutos);

            if (context.Request.QueryString["origem"] == null)
            {
                NaoDisponivel(context);
                return;
            }

            if (context.Request.QueryString["CNPJ"] == null)
            {
                NaoDisponivel(context);
                return;
            }

            String numeroCnpj = context.Request.QueryString["CNPJ"];

            numeroCnpj = (new Regex("[^0-9]")).Replace(numeroCnpj, "");

            if (numeroCnpj.Length > 14)
            {
                NaoDisponivel(context);
                return;
            }

            numeroCnpj = numeroCnpj.PadLeft(14, '0');

            var pathFile = string.Format("/{0}.jpg", numeroCnpj);
            var pathFolder = Settings.Default.PessoaJuridicaLogoCaminho;
            var serverPathFolder = context.Server.MapPath(pathFolder);

            if (!Employer.Plataforma.Web.Componentes.CNPJ.Validar(numeroCnpj))
            {
                NaoDisponivel(context);
                return;
            }

            var dir = new DirectoryInfo(serverPathFolder);
            if (!dir.Exists)
                Directory.CreateDirectory(serverPathFolder);

            //Excluir arquivos antigos
            (new DirectoryInfo(serverPathFolder))
                .GetFiles()
                .Where(file => file.CreationTime < delay)
                .ToList()
                .ForEach(delegate(FileInfo file)
                {
                    if (!file.IsReadOnly)
                        file.Delete();
                });

            if (File.Exists(serverPathFolder + pathFile) && File.GetCreationTime(serverPathFolder + pathFile) > delay)
            {
                Disponivel(context, pathFolder + pathFile);
                return;
            }

            byte[] byteArray = null;

            var origem = context.Request.QueryString["origem"];

            OrigemLogo origemEnum;
            if (Enum.TryParse(origem, false, out origemEnum))
            {
                byteArray = RecuperarLogo(Convert.ToDecimal(numeroCnpj), origemEnum);
            }

            if (byteArray == null || byteArray.Length == 0)
            {
                NaoDisponivel(context);
                return;
            }

            #region Redimendisonando a imagem
            var objImagem = ByteArrayToImage(byteArray);

            objImagem = ResizeImage(objImagem, MaxSize != Size.Empty ? MaxSize : new Size(270, 38));

            using (var ms = new MemoryStream())
            {
                objImagem.Save(ms, ImageFormat.Png);
                byteArray = ms.ToArray();
            }
            #endregion Redimendisonando a imagem

            //Criando novo arquivo
            using (FileStream sw = File.Create(serverPathFolder + pathFile))
            {
                using (var bw = new BinaryWriter(sw))
                {
                    bw.Write(byteArray);
                    bw.Flush();
                    bw.Close();
                }
            }
            Disponivel(context, pathFolder + pathFile);
        }
        #endregion

        #region NaoDisponivel
        private void NaoDisponivel(HttpContext context)
        {
            context.Response.Redirect("~/img/logo_vazio.png", false);
        }
        #endregion

        #region Disponivel
        private void Disponivel(HttpContext context, string pathFolderFile)
        {
            context.Response.Redirect(string.Format("~/{0}?nocache={1}", pathFolderFile, Guid.NewGuid()), false);
        }
        #endregion

        #region IsReusable
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region RemoverLogoCache
        public static void RemoverLogoCache(String numeroCNPJ)
        {
            numeroCNPJ = (new Regex("[^0-9]")).Replace(numeroCNPJ, "");

            if (numeroCNPJ.Length > 14)
                return;

            numeroCNPJ = numeroCNPJ.PadLeft(14, '0');

            String pathFile = String.Format("/{0}.jpg", numeroCNPJ);
            String pathFolder = HttpContext.Current.Server.MapPath(Settings.Default.PessoaJuridicaLogoCaminho);

            if (File.Exists(pathFolder + pathFile))
            {
                File.Delete(pathFolder + pathFile);
            }
        }
        #endregion

        #region RecuperarLogo
        public static byte[] RecuperarLogo(decimal numeroCNPJ, OrigemLogo origemLogo)
        {
            byte[] byteArray = null;
            switch (origemLogo)
            {
                case OrigemLogo.Local:
                    #region Logo Local
                    try
                    {
                        byteArray = BLL.FilialLogo.RecuperarArquivo(numeroCNPJ);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                    #endregion
                    break;
                case OrigemLogo.Plataforma:
                    #region Plataforma
                    try
                    {
                        using (var wsPJ = new WSPessoaJuridica.WSPessoaJuridica())
                        {
                            byteArray = wsPJ.CarregarPessoaJuridicaLogoPrincipalBinario(numeroCNPJ);
                        }
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                    }
                    #endregion Plataforma
                    break;
            }

            return byteArray;
        }
        #endregion RecuperarLogo

        #region ExisteLogo
        public static bool ExisteLogo(decimal numeroCNPJ)
        {
            return RecuperarLogo(numeroCNPJ, OrigemLogo.Local) != null;
        }
        #endregion

        #region OrigemLogo
        public enum OrigemLogo
        {
            Local,
            Plataforma
        }
        #endregion

        #region Base64ToImage
        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);

            Image image;
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                ms.Write(imageBytes, 0, imageBytes.Length);
                image = Image.FromStream(ms, true);
            }

            return image;
        }
        #endregion

        #region ByteArrayToImage
        public Image ByteArrayToImage(byte[] byteArray)
        {
            Image image;
            using (var ms = new MemoryStream(byteArray, 0, byteArray.Length))
            {
                ms.Write(byteArray, 0, byteArray.Length);
                image = Image.FromStream(ms, true);
            }

            return image;
        }
        #endregion ByteArrayToImage

        #region ResizeImage
        private static Image ResizeImage(Image imgToResize, Size size)
        {
            if (imgToResize != null)
            {
                int sourceWidth = imgToResize.Width;
                int sourceHeight = imgToResize.Height;

                float nPercentW = ((float)size.Width / sourceWidth);
                float nPercentH = ((float)size.Height / sourceHeight);
                float nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

                var destWidth = (int)(sourceWidth * nPercent);
                var destHeight = (int)(sourceHeight * nPercent);

                var b = new Bitmap(destWidth, destHeight, PixelFormat.Format32bppArgb);
                b.SetResolution(150, 150);

                Graphics g = Graphics.FromImage(b);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                
                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                g.Dispose();

                return b;
            }
            return null;
        }
        #endregion

    }
}