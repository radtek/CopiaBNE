using API.GatewayV2.APIModel;
using API.GatewayV2.BNEModel;
using API.GatewayV2.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using WebApiThrottle;

namespace API.GatewayV2.Throttle
{
    public class CustomThrottlingHandler : ThrottlingHandler
    {
        protected override RequestIdentity SetIndentity(HttpRequestMessage request)
        {
            return new RequestIdentity()
            {
                ClientKey = GetAuthId(request),
                ClientIp = base.GetClientIp(request).ToString(),
                Endpoint = request.RequestUri.AbsolutePath.ToLowerInvariant()
            };
        }

        private string GetAuthId(HttpRequestMessage request)
        {
            Usuario usuario = null;
            var cache_key = "";
            UserCredentials credtls;

            if (request.Method == HttpMethod.Options)
            {
                return "allow_options";
            }

            try
            {
                credtls = UsuarioManager.DecodeCredentials(request.Headers.GetValues("apiKey").First());
            }
            catch
            {
                return "";
            }

            if (!credtls.Sistema.HasValue)
                return string.Empty;

            using (var ctx = new APIGatewayContext())
            {
                Cliente cliente = ctx.Cliente.AsNoTracking().Where(c => c.Idf_Cliente == credtls.Sistema.Value).FirstOrDefault();
                if (cliente == null)
                    return "";
            }

            if (credtls == null) return "";

            cache_key = UsuarioManager.CreateUsuarioKey(credtls.CPF, credtls.CNPJ);
            UsuarioManager mngr = new UsuarioManager();
            usuario = mngr.GetCached(cache_key);

            if (usuario != null)
            {
                if (usuario.Dta_Nascimento != credtls.DataNascimento)
                {
                    mngr.Uncache(cache_key);
                    usuario = mngr.ObtainAccess(credtls.CPF, credtls.DataNascimento, credtls.CNPJ);
                    if (usuario != null) mngr.Cache(cache_key, usuario);
                }
            }
            else
            {
                usuario = mngr.ObtainAccess(credtls.CPF, credtls.DataNascimento, credtls.CNPJ);
                if (usuario != null) mngr.Cache(cache_key, usuario);
            }


            if (usuario != null)
            {
                request.Headers.Add("cache_key", cache_key);
                request.Headers.Add("Idf_Cliente", credtls.Sistema.ToString());
                return string.Format("{0}{1}", usuario.Idf_Usuario.ToString(), request.RequestUri.AbsolutePath);
            }
            else
                return "";
        }

    }
}