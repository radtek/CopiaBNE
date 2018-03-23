using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BNE.Core.ExtensionsMethods;
using Newtonsoft.Json.Linq;
using System.Security.Authentication;
using Newtonsoft.Json;

namespace APIGateway.Authentication
{
    public class OAuth_PF : IAuthentication
    {
        private static string _secretKey;

        private String SecretKey
        {
            get
            {
                if (String.IsNullOrEmpty(_secretKey))
                    _secretKey =  Domain.OAuthConfig.ObterSecretKey("OAuth_PF");
                return _secretKey;
            }
        }

        public Model.Usuario Authenticate(System.Net.Http.HttpRequestMessage request, out Model.SistemaCliente sistema)
        {
            //Verificando presença do Authorization no cabeçalho
            if (request.Headers.Authorization == null ||
                request.Headers.Authorization.Scheme != "Bearer" ||
                String.IsNullOrEmpty(request.Headers.Authorization.Parameter))
                throw new AuthenticationException("Cabeçalho da autorização é inválido");

            //Descriptografando token
            JObject jObject = JObject.Parse(request.Headers.Authorization.Parameter.Descriptografa(SecretKey));

            //Verificando se token está expirado
            DateTime expiresIn = Convert.ToDateTime(jObject["expires_in"].ToString());
            if (expiresIn < DateTime.Now)
                throw new AuthenticationException("Token expirado");

            //Definindo sistema
            sistema = Domain.SistemaCliente.Get(new Guid(jObject["sistema"].ToString()));

            
            Model.UsuarioPessoaFisica usuario = new Model.UsuarioPessoaFisica() { 
                PerfilDeAcesso = Model.Perfil.PessoaFisicaBNE,
                CPF = Convert.ToDecimal(jObject["auth_data"]["CPF"]),
                DataNascimento = Convert.ToDateTime(jObject["auth_data"]["DataNascimento"]),
            };

            usuario.ForwardHeaders.Add(new Model.Header() { Item = "authenticarion_data", Value = jObject["auth_data"].ToString(Formatting.None) });

            return usuario;
        }
    }
}