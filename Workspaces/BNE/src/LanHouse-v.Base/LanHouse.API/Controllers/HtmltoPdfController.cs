using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using WebSupergoo.ABCpdf9;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Drawing;
using LanHouse.Business;
using LanHouse.Business.Custom;

namespace LanHouse.API.Controllers
{
    public class HtmltoPdfController : LanHouse.API.Code.BaseController
    {
        //public HttpResponseMessage Post([FromBody]Html html)
        public HttpResponseMessage Post(JObject jsonData)
        {
            try
            {
                dynamic json = jsonData;
                string html = json.html;
                string strModelo = json.modelo;
                string strCandidato = json.candidato;

                Html CVHtml = new Html();
                CVHtml.html = html;

                var doc = GerarPDF(CVHtml, strModelo, strCandidato);

                var pdfBytes = doc.GetData();

                LimparDocPdf(doc);

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                response.Content = new ByteArrayContent(pdfBytes);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");

                return response;
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Gerar PDF");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        #region GerarPDF
        public static Doc GerarPDF([FromBody]Html html, string modelo, string candidato)
        {
            var doc = new Doc();

            doc.HtmlOptions.Engine = EngineType.Gecko;
            doc.HtmlOptions.UseScript = true;

            doc.Rect.Inset(10, 10);
            doc.HtmlOptions.Media = MediaType.Print;
            doc.Page = doc.AddPage();

            Rectangle rc = doc.MediaBox.Rectangle;

            rc.Inflate(-20, -30);
            rc.Height = 725;

            doc.Rect.Rectangle = rc;
            doc.FrameRect();

            string htmlCompleto = "";
            int imgPositionX = 0;
            int imgPositionY = 0;

            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("<img class=\"imgCVE\"(.*)>");

            var v = regex.Match(html.html);

            html.html = System.Text.RegularExpressions.Regex.Replace(html.html, "<img class=\"imgCVE\"(.*)>", "");

            switch(modelo)
            {
                case "CVElegante":
                    htmlCompleto = html.HtmlHeadPadrao + html.HtmlCVElegante + html.html + html.HtmlFooter;
                    imgPositionX = 400;
                    imgPositionY = 575; 
                    break;
                case "CVModerno":
                    htmlCompleto = html.HtmlHeadPadrao + html.HtmlCVModerno + html.html + html.HtmlFooter;
                    imgPositionX = 50;
                    imgPositionY = 580;
                    break;
                default: //BNE
                    htmlCompleto = html.HtmlHeadPadrao + html.HtmlCVBNE + html.html + html.HtmlFooter;
                    imgPositionX = 400;
                    imgPositionY = 520;
                    break;
            }
            
            int theID = doc.AddImageHtml(htmlCompleto);
            int idPessoaFisica = Convert.ToInt32(candidato);

            byte[] imgCandidatoBase = PessoaFisica.CarregarImagemCandidato(idPessoaFisica);

            if (imgCandidatoBase != null)
            {
                XImage imgcandidato = new XImage();
                imgcandidato.SetData(PessoaFisica.CarregarImagemCandidato(idPessoaFisica));

                //definir o tamanho da imagem
                int imgWidth = imgcandidato.Width > imgcandidato.Height ? 260 : 170;
                int imgHeight = 200;

                doc.Rect.String = string.Format("0 0 {0} {1}", imgWidth, imgHeight); // imgcandidato.Selection.String;
                doc.Rect.Magnify(0.5, 0.5);

                doc.Rect.Width = 100;
                doc.Rect.Height = 100;

                //checar orientação da foto para acertar a posição
                imgPositionY = imgcandidato.Width > imgcandidato.Height ? imgPositionY + 40 : imgPositionY;

                doc.Rect.Position(imgPositionX, imgPositionY);

                doc.Rect.String = imgcandidato.Selection.String;
                doc.Rect.Magnify(0.5, 0.5);

                doc.Rect.Position(imgPositionX, imgPositionY);
                doc.AddImageObject(imgcandidato, false);
            }

            doc.Color.String = "255 255 255";

            while (doc.Chainable(theID))
            {
                doc.Page = doc.AddPage();

                Rectangle rc1 = doc.MediaBox.Rectangle;

                rc1.Inflate(-20, -30);
                rc1.Height = 725;

                doc.Rect.Rectangle = rc1;
                doc.FrameRect();

                
                //doc.AddImageObject(imgcandidato);
                theID = doc.AddImageToChain(theID);
            }

            if (doc.PageCount > 0)
                doc.PageNumber = 1;

            return doc;
        }
        #endregion

        #region LimparDocPdf
        /// <summary>
        /// Limpar o doc do pdf depois de gerar
        /// </summary>
        /// <param name="doc"></param>
        private void LimparDocPdf(Doc doc)
        {
            doc.Clear();
        }

        #endregion
    }
}