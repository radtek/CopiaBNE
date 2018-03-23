using Microsoft.IdentityModel.Claims;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
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
                    var service = new Core.Helpers.HttpService();
                    var serializer = new JavaScriptSerializer();

                    Dictionary<string, string> parm = new Dictionary<string, string>
                    {
                        {"sistema", ConfigurationManager.AppSettings["KeyGatewayPessoaFisica"]}
                    };

                    Uri urlApi = new Uri(ConfigurationManager.AppSettings["EnderecoApiPessoaFisica"]);
                    var retorno = service.Post(urlApi, "api/pessoafisica/Token?cpf=" + claimsIdentity.Claims[1].Value + "&dataNascimento=" + claimsIdentity.Claims[2].Value, string.Empty, parm);

                    return serializer.Deserialize<DTO.ApiGatewayToken>(retorno.Content.ReadAsStringAsync().Result);
                }
            }
            return null;
        }
    }
}