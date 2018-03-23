using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using WebSupergoo.ABCpdf8;

namespace BNE.BLL.Custom
{
    public class PDF
    {

        #region RecuperarPDF Usando TextSharp
        public static byte[] RecuperarPDFUsandoTextSharp(string HTML)
        {
            try
            {
                //cria documento com o tamanho e margens
                Document document = new Document(PageSize.A4, 40, 40, 40, 40);

                //cria um memory stream para ser usado na conversão e emissão
                MemoryStream ms = new MemoryStream();

                // inicializa o gravador
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                //le string HTML e atribui ao StringReader
                StringReader conteudo = new StringReader(HTML);

                //Objeto de conversão do HTML
                HTMLWorker objeto = new HTMLWorker(document);

                //Abre o documento
                document.Open();

                //Aplica o parse para análise de conversão
                objeto.Parse(conteudo);

                //fecha o documento
                document.Close();

                //retorna o documento pdf em byte array
                return ms.GetBuffer();
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro na Geração de PDF - RecuperarPDFUsandoTextSharp");
                return null;
            }
            
        }
        #endregion

        #region RecuperarPDF Usando TextSharp
        public static byte[] RecuperarPDFUsandoTextSharp(List<Bitmap> images)
        {
            float lm, rm, bm, tm;
            lm = rm = bm = tm = 2;

            //cria documento com o tamanho e margens
            Document document = new Document(PageSize.A4, lm, rm, tm, bm);

            //cria um memory stream para ser usado na conversão e emissão
            MemoryStream ms = new MemoryStream();

            // inicializa o gravador
            PdfWriter writer = PdfWriter.GetInstance(document, ms);

            //Abre o documento
            document.Open();

            foreach (var image in images)
            {
                iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(image, System.Drawing.Imaging.ImageFormat.Bmp);

                var width = document.PageSize.Width - (lm + rm);
                var height = document.PageSize.Height - (bm + tm);

                //pdfImage.SetAbsolutePosition(lm, bm);
                pdfImage.ScaleToFit(width, height);

                document.Add(pdfImage);
            }

            //fecha o documento
            document.Close();

            //retorna o documento pdf em byte array
            return ms.GetBuffer();
        }
        #endregion

        #region RecuperaCurriculosParaEnvio
        public static byte[] GerarPdfAPartirdoHtml(string HTML)
        {
            try
            {
                var htmlToPdf = new NReco.PdfGenerator.HtmlToPdfConverter();
                htmlToPdf.CustomWkHtmlArgs = "--encoding UTF-8";
                return htmlToPdf.GeneratePdf(HTML);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro na criação de pdf - metodo GerarPdfAPartirdoHtml");
                return null;
            }


        }
        #endregion

        #region RecuperarPDF
        public static byte[] RecuperarPDFUsandoAbc(string HTML)
        {
            try
            {
                using (var doc = new Doc())
                {
                    doc.Page = doc.AddPage();

                    int ID = doc.AddImageHtml(HTML);

                    while (true)
                    {
                        doc.FrameRect(); // add a black border
                        if (!doc.Chainable(ID))
                            break;
                        doc.Page = doc.AddPage();
                        ID = doc.AddImageToChain(ID);
                    }

                    for (int i = 1; i <= doc.PageCount; i++)
                    {
                        doc.PageNumber = i;
                        doc.Flatten();
                    }
                    //reset back to page 1 so the pdf starts displaying there
                    if (doc.PageCount > 0)
                        doc.PageNumber = 1;

                    return doc.GetData();
                }
            }
            catch(Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro na Geração de PDF - AbcPDF");
                return null;
            }
        }
        #endregion

        #region RetornarApenasConteudoHtml
        /// <summary>
        /// Método responsável por retornar apenas o conteúdo necessário para renderização 
        /// </summary>
        /// <param name="texto">Texto que será manipulado</param>
        /// <param name="caminhoCssPDF">Parametro opcional: caminho do css para geração de PDF</param>
        /// <returns></returns>
        public static string RetornarApenasConteudoHtml(string texto, string caminhoCssPDF)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(texto);

            if (!string.IsNullOrEmpty(caminhoCssPDF))
            {
                var nodeHead = doc.DocumentNode.SelectNodes("//head").FirstOrDefault();
                if (nodeHead != null)
                {
                    HtmlNode style = doc.CreateElement("link");
                    style.SetAttributeValue("href", caminhoCssPDF);
                    style.SetAttributeValue("rel", "stylesheet");
                    style.SetAttributeValue("type", "text/css");

                    nodeHead.AppendChild(style);
                }
            }

            //Div do topo que deve ser retirada
            //var nodesTopo = doc.DocumentNode.SelectNodes("//div[@id='topo']|//div[@id='rodape']|//div[@id='rodape_botoes']|//div[@id='abas']|//h2[@id='titulo']|//a[@class='editar']");            
            var nodesTopo = doc.DocumentNode.SelectNodes("//noscript|//div[@id='topo']|//div[@class='barra_rodape']|//div[@class='painel_botoes']|//head[@id='Head1']|//div[@id='fb-root']|//script|//table|//div[@id='upgGlobalCarregandoInformacoes']|//div[@id='updAviso']|//input");
            if (nodesTopo != null)
            {
                foreach (var node in nodesTopo)
                {
                    node.Remove();
                }
            }

            return doc.DocumentNode.OuterHtml;
        }
        #endregion

    }
}
