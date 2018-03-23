using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;

namespace BNE.PessoaFisica.Web.Helpers
{
    public class ApiGatewayToken
    {
        public static DTO.ApiGatewayToken GerarToken()
        {
            IClaimsPrincipal icp = Thread.CurrentPrincipal as IClaimsPrincipal;

            if (icp != null)
            {
                IClaimsIdentity claimsIdentity = (IClaimsIdentity)icp.Identity;

                if (icp.Identity.IsAuthenticated)
                {
                    var service = new Helpers.HttpService();
                    var serializer = new JavaScriptSerializer();

                    Dictionary<string, string> parm = new Dictionary<string, string>();

                    parm.Add("sistema", ConfigurationManager.AppSettings["KeyGatewayPessoaFisica"]);

                    Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
                    var retorno = service.Post(urlApi, "bne/pessoafisica/Token?cpf=" + claimsIdentity.Claims[1].Value + "&dataNascimento=" + claimsIdentity.Claims[2].Value, parm, "");

                     return serializer.Deserialize<DTO.ApiGatewayToken>(retorno.Content.ReadAsStringAsync().Result);
                }
            }

            return null;
        }
    }
}