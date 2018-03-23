using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebSupergoo.ABCpdf9;
using LanHouse.Business.Custom;
using Newtonsoft.Json.Linq;

namespace LanHouse.API.Controllers
{
    public class EnviarCVEmailController : LanHouse.API.Code.BaseController
    {
        // GET: api/EnviarCVEmail
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/EnviarCVEmail/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/EnviarCVEmail
        [Authorize]
        public void Post(JObject jsonData)
        {
            dynamic json = jsonData;
            string html = json.html;
            string strNomeCandidato = json.nome;
            string strEmail = json.email;
            string strAssunto = json.assunto;
            string strModelo = json.modelo;
            string strMensagem = json.mensagem;
            string strCandidato = json.candidato;

            Html CVHtml = new Html();

            CVHtml.html = html;

            try
            {
                //gerar o PDF para enviar como anexo
                Doc doc = HtmltoPdfController.GerarPDF(CVHtml, strModelo, strCandidato);
                byte[] pdfBytes = doc.GetData();

                Dictionary<string, byte[]> anexo = new Dictionary<string, byte[]> { { string.Format("{0}.pdf", strNomeCandidato.Replace(" ", "_")), pdfBytes } };

                MailController.Send(strEmail, "atendimento@bne.com.br", string.Format("{0} por {1}", strAssunto, strNomeCandidato), strMensagem, anexo, SaidaSMTP.SendGrid);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Enviar CV por e-mail");
            }
        }

        // PUT: api/EnviarCVEmail/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/EnviarCVEmail/5
        public void Delete(int id)
        {
        }
    }
}
