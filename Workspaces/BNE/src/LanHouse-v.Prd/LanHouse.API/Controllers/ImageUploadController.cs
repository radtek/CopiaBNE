using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    [Authorize]
    public class ImageUploadController : LanHouse.API.Code.BaseController
    {
        /// <summary>
        /// Faz o upload de arquivo
        /// </summary>
        /// <returns></returns>
        /// 
        public HttpResponseMessage Post()
        {
            try
            {
                HttpResponseMessage result = null;
                var httpRequest = HttpContext.Current.Request;
                string nomeArquivo = string.Empty;
                string[] extensoesPermitidas = new string[] { ".jpg", ".jpeg", ".png" };
                string[] contentTypes = new string[] { "image/jpg", "image/jpeg", "image/png" };
                System.Drawing.Imaging.ImageFormat formato = System.Drawing.Imaging.ImageFormat.Jpeg;

                if (httpRequest.Files.Count > 0)
                {
                    foreach (string file in httpRequest.Files)
                    {
                        //salvar o arquivo temporariamente na pasta
                        var postedFile = httpRequest.Files[file];
                        string filePath = System.Configuration.ConfigurationManager.AppSettings["PathImages"];

                        string extensao = Path.GetExtension(filePath);

                        //valida a extensão e o contentType do arquivo
                        bool contentTypyValid = contentTypes.Contains(postedFile.ContentType);
                        //bool extensaoValid = extensoesPermitidas.Contains(extensao);

                        if (!contentTypyValid)
                        {
                            return result = Request.CreateResponse(HttpStatusCode.NotAcceptable, "406");
                        }

                        postedFile.SaveAs(filePath + postedFile.FileName);


                        string retorno = resizeImageAndSave(filePath, postedFile.FileName, "_edit", extensao == ".png" ? System.Drawing.Imaging.ImageFormat.Png : System.Drawing.Imaging.ImageFormat.Png);

                        nomeArquivo = postedFile.FileName.Replace(Path.GetFileNameWithoutExtension(postedFile.FileName), Path.GetFileNameWithoutExtension(postedFile.FileName) + "_edit");
                    }
                    result = Request.CreateResponse(HttpStatusCode.Created, nomeArquivo);
                }
                else
                {
                    result = Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                return result;
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Erro no upload da imagem do candidato");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        public static string resizeImageAndSave(string imagePath, string nomeArquivo, string prefixo, System.Drawing.Imaging.ImageFormat formato)
        {
            string pathImage = imagePath + nomeArquivo;
            System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(pathImage);

            var thumbnailImg = new Bitmap(fullSizeImg, new Size(320, 320));

            if (nomeArquivo.StartsWith("candidato_foto"))
                thumbnailImg = new Bitmap(fullSizeImg, new Size(320, 240));

            var thumbGraph = Graphics.FromImage(thumbnailImg);
            thumbGraph.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            thumbGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            thumbGraph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

            var imageRectangle = new Rectangle(0, 0, thumbnailImg.Width, thumbnailImg.Height);

            thumbGraph.DrawImage(fullSizeImg, imageRectangle);
            string targetPath = pathImage.Replace(Path.GetFileNameWithoutExtension(pathImage), Path.GetFileNameWithoutExtension(pathImage) + prefixo);
            thumbnailImg.Save(targetPath, formato);
            thumbnailImg.Dispose();
            return targetPath;
        }
    }
}
