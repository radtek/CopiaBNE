using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LanHouse.API.Controllers
{
    public class MeuHoleriteController : ApiController
    {

        [HttpGet]
        public HttpResponseMessage ObterLoginURL(string cpf) 
        {
            var acesso_url = "";
            try 
            {
                MeuHoleriteService.SiteComercialClient SiteComercialWs = new MeuHoleriteService.SiteComercialClient();
                Dictionary<string, string> empresas = SiteComercialWs.RetornarEmpresasUsuario(Convert.ToDecimal(cpf));

                if (empresas.Count > 0)
                {
                    acesso_url = SiteComercialWs.CarregarUrlAcessoPorID(Convert.ToInt32(empresas.First().Key), Convert.ToDecimal(cpf));
                    acesso_url = acesso_url.Split('-').First();
                    return Request.CreateResponse<string>(HttpStatusCode.OK, String.Format("https://prd.meuholerite.com.br/{0}", acesso_url));
                }
                else
                    return Request.CreateResponse<string>(HttpStatusCode.OK, "");
            }
            catch (Exception ex) 
            {
                return Request.CreateResponse<string>(HttpStatusCode.InternalServerError, ex.Message);
            }
        }



    }
}
