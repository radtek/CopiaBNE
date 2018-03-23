using LanHouse.Entities.BNE;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace LanHouse.API.Authentication.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId = string.Empty;
            string clientSecret = string.Empty;
            string cpf = context.Parameters["username"].ToString().Replace(".","").Replace("-","");
            TAB_Pessoa_Fisica objPessoaFisica;
            bool existe = Business.PessoaFisica.CarregarPorCPF(Convert.ToDecimal(cpf), out objPessoaFisica);

            if(!context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.TryGetFormCredentials(out clientId, out clientSecret);
            }

            if(context.ClientId == null)
            {
                context.Validated();

                return Task.FromResult<object>(null);
            }

            if (objPessoaFisica == null)
            {
                context.SetError("invalid_clientId", string.Format("Usuário '{0}' is not registered in the system.", context.ClientId));
                return Task.FromResult<object>(null);
            }

            //context.OwinContext.Set<string>("as:clientAllowedOrigin", client.AllowedOrigin);
            //context.OwinContext.Set<string>("as:clientRefreshTokenLifeTime", client.RefreshTokenLifeTime.ToString());

            context.Validated();
            return Task.FromResult<object>(null);
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin");

            if (allowedOrigin == null) allowedOrigin = "*";

            //context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            string cpf = context.UserName.ToString().Replace(".", "").Replace("-", "");

            TAB_Pessoa_Fisica objPessoaFisica;
            int existe = Business.PessoaFisica.ValidarCPFeDataNascimento(Convert.ToDecimal(cpf), Convert.ToDateTime(context.Password), out objPessoaFisica);

            if (existe == 1)
            {
                context.SetError("invalid_grant", "Dados inválidos.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            var props = new AuthenticationProperties(new Dictionary<string, string>
                {
                    { 
                        "as:client_id", (context.ClientId == null) ? string.Empty : context.ClientId
                    },
                    { 
                        "userName", context.UserName
                    }
                });

            var ticket = new AuthenticationTicket(identity, props);
            context.Validated(identity);

        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

    }
}