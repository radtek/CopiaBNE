using API.Gateway.Models;
using API.Gateway.Repository;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace API.Gateway.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            IdentityUser user = null;
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            using (AuthRepository _repo = new AuthRepository())
            { 
                user = await _repo.FindUser(context.UserName, context.Password);

                if (user == null)
                {
                    context.SetError("invalid_grant", "O CPF ou data de nascimento é inválido");
                    return;
                }

                Usuario perfil_usuario = null;
                using (var dbo = new APIGatewayContext()) 
                {
                    perfil_usuario = (from pu in dbo.Usuario where pu.Idf_Usuario.ToString() == user.UserName select pu).FirstOrDefault();
                }
            

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                if(perfil_usuario != null)
                {
                    identity.AddClaim(new Claim("Num_CPF", perfil_usuario.Num_CPF.ToString()));
                    identity.AddClaim(new Claim("Dta_Nascimento", perfil_usuario.Dta_Nascimento.ToString() ));
                    identity.AddClaim(new Claim("Idf_Filial", (perfil_usuario.Idf_Filial.HasValue) ? perfil_usuario.Idf_Filial.ToString() : "" ));
                    identity.AddClaim(new Claim("Idf_Perfil", perfil_usuario.Idf_Perfil.ToString()));
                    identity.AddClaim(new Claim("Idf_Perfil_Usuario", perfil_usuario.Idf_Usuario.ToString()));
                    identity.AddClaim(new Claim("Dta_Inicio_Plano", perfil_usuario.Dta_Inicio_Plano.ToString()));

                    var values = await context.Request.ReadFormAsync();
                    identity.AddClaim(new Claim("Idf_Cliente", values.Get("Idf_Cliente")));
                }

                context.Validated(identity);
            }
            
        }


    }
}