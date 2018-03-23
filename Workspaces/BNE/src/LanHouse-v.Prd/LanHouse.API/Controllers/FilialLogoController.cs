using LanHouse.Business;
using LanHouse.Business.EL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    public class FilialLogoController : ApiController
    {
        private Code.ICacheService cache = new Code.InMemoryCache();

        /// <summary>
        /// Carregar a logomarca da filial (Lan House)
        /// GET / api/<controller>/nome
        /// </summary>
        /// <param name="cnpj"></param>
        /// <returns></returns>
        /// 
        public HttpResponseMessage Get(string cnpj)
        {
            try
            {
                Image objImagem =  cache.Get(string.Format("logo.{0}", cnpj), () =>
                {
                    decimal cnpjDecimal = Convert.ToDecimal(cnpj);

                    Image retorno;
                    var logo = Companhia.RecuperarLogo(cnpjDecimal);
                    if (logo == null || !logo.Any())
                        retorno = null;//Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/Content/img/logo_vazio.png"));
                    else
                    {
                        using (var mslogo = new MemoryStream(logo, 0, logo.Length))
                        {
                            retorno = Image.FromStream(mslogo, true);
                        }
                    }

                    if(retorno != null)
                        return retorno.Width > retorno.Height ? new Bitmap(retorno, new Size(103, 65)) : new Bitmap(retorno, new Size(65, 65));
                    else
                        return null;
                });

                var ms = new MemoryStream();

                if (objImagem != null)
                {
                    objImagem.Save(ms, ImageFormat.Png);
                    ms.Seek(0, SeekOrigin.Begin);
                }

                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StreamContent(ms);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                return response;
            }
            catch (RecordNotFoundException ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar a logomarca da filial");
                return Request.CreateResponse(HttpStatusCode.NotFound, ex.Message);
            }

            catch(Exception ex)
            {
                string msgErro;
                Business.EL.GerenciadorException.GravarExcecao(ex, out msgErro, "Lan House API - Carregar a logomarca da filial");
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
        
        // POST: api/FilialLogo
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/FilialLogo/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/FilialLogo/5
        public void Delete(int id)
        {
        }
    }
}
