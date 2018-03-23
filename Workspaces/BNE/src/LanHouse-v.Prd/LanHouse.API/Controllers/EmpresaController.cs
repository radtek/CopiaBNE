using LanHouse.Business;
using LanHouse.Business.Custom;
using LanHouse.Business.EL;
using LanHouse.Entities.BNE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebSupergoo.ABCpdf9;

namespace LanHouse.API.Controllers
{
    public class EmpresaController : ApiController
    {
        // GET: api/Empresa
        [HttpGet]
        public HttpResponseMessage Get(string idFuncao, string filtro, int start, int rows, string geoLocalizacao)
        {
            string[] coordenadas = geoLocalizacao.Split(',');

            double LanLat = Convert.ToDouble(coordenadas[0].Replace(".",","));
            double LanLon = Convert.ToDouble(coordenadas[1].Replace(".", ","));

            try
            {
                if (filtro == null)
                    filtro = "";
                var empresas = Business.Empresa.RecuperarEmpresas(start,rows, LanLat,LanLon);
                //var empresas = Business.Custom.SOLR.PesquisaEmpresaSolr.ObterRegistros(idFuncao, filtro, start, rows, geoLocalizacao);

                return Request.CreateResponse(HttpStatusCode.OK, empresas);
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Listar empresas");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Listar empresas");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        public string VerificaEnvioCurriculo(int idCurriculo, int idEmpresa)
        {
            try
            {
                BNE_Intencao_Filial objIntencao_Filial;
                bool existeCandidatura = Business.EnviarCVEmpresa.VerificaEnvioCurriculo(idCurriculo, idEmpresa, out objIntencao_Filial);
                return existeCandidatura ? string.Format("{0} às {1}", objIntencao_Filial.Dta_Cadastro.ToString("dd/MM/yyyy"), 
                                                                        objIntencao_Filial.Dta_Cadastro.ToString("HH:mm:ss")) : "false";
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - verificar envio CV para empresa");
                return "";
            }
        }

        [HttpPost]
        [Authorize]
        public HttpResponseMessage EfetuarEnvioCV(int idCurriculo, int idEmpresa, int idCandidato, string nomeCandidato)
        {
            Html htmlCV = null;
            bool retorno = false;

            try
            {
                 /*
                 *  PODE ENVIAR O CV PARA EMPRESA SE CASO:
                 * 
                 *  1 - Candidato é vip ou possui candidaturas disponíveis
                 *  
                 */

                if (!Curriculo.ValidaEnvioCVEmpresa(idCurriculo))
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, "");

                //Registrar Intenção do Candidato
                EnviarCVEmpresa.RegistrarIntencaoCandidato(idCurriculo, idEmpresa);

                //atualizar saldo candidaturas
                EnviarCVEmpresa.AtualizarSaldoEnvioEmail(idCurriculo);

                string emailResponsavel = EnviarCVEmpresa.CarregarResponsavelEmpresa(idEmpresa);

                if(emailResponsavel != "")
                {
                    string strAssunto = "Currículo vitae - ";
                    string strMensagem = "Corpo do email aqui";

                    //Gerar PDF do CV
                    htmlCV = Business.EnviarCVEmpresa.GerarHtmlCV(idCurriculo);

                    //Gerar o PDF
                    Doc doc = HtmltoPdfController.GerarPDF(htmlCV, "BNE", idCandidato.ToString());
                    byte[] pdfBytes = doc.GetData();

                    Dictionary<string, byte[]> anexo = new Dictionary<string, byte[]> { { string.Format("{0}.pdf", nomeCandidato.Replace(" ", "_")), pdfBytes } };

                    //Enviar por email
                    MailController.Send(emailResponsavel, "atendimento@bne.com.br", string.Format("{0} {1}", strAssunto, nomeCandidato), strMensagem, anexo, SaidaSMTP.SendGrid);
                }

                retorno = true;

                return Request.CreateResponse(HttpStatusCode.OK, retorno);
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - RNF - Enviar Cv para Empresa");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }
            catch (Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Enviar Cv para Empresa");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }       
        }

        // GET: api/Empresa/5
        [Authorize]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Empresa
        [Authorize]
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Empresa/5
        [Authorize]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Empresa/5
        [Authorize]
        public void Delete(int id)
        {
        }
    }
}
