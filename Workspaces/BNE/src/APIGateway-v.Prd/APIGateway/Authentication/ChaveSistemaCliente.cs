using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using BNE.Core.ExtensionsMethods;
using System.Security.Authentication;
using APIGateway.Model;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using APIGateway.Authentication.Classes;

namespace APIGateway.Authentication
{
    /// <summary>
    /// Implementação para permitir autenticação apenas com a chave do sistema cliente.
    /// Pode ser informada através:
    /// - Query string: apiKey
    /// - Header: apiKey
    /// </summary>
    public class ChaveSistemaCliente : IAuthentication
    {
        private const String key = "apiKey";

        public Usuario Authenticate(System.Net.Http.HttpRequestMessage request, out Model.SistemaCliente sistema)
        {
            String chave = null;

            //Verifica presenca da apiKey no cabecalho
            if (request.Headers.Contains("apiKey"))
               chave  = request.Headers.GetValues("apiKey").First();


            if (string.IsNullOrEmpty(chave)) {
                var qs = request.GetQueryNameValuePairs();
                if(qs.Any(q => q.Key.ToLower() == key.ToLower()))
                    chave = qs.First(q => q.Key.ToLower() == key.ToLower()).Value;
            }

            if (string.IsNullOrEmpty(chave))
                throw new InvalidCredentialException("ApiKey não informada");

            Guid chaveSistema;
            if (!Guid.TryParse(chave, out chaveSistema))
            {
                // Se a chave não é guid, tenta obter no objeto codificado
                try
                {
                    chaveSistema = DecodeCredentials(chave).Sistema;
                }
                catch (Exception ex)
                {
                    throw new InvalidCredentialException("Não foi possível recuperar as informações de autenticação do cabeçalho", ex);
                }
            }

            UsuarioSistemaCliente usuario = Domain.UsuarioSistemaCliente.Obter(chaveSistema);
            if (usuario == null)
                throw new InvalidCredentialException("Nenhum usuário encontrado para a ApiKey informada");

            usuario.ForwardHeaders = usuario.Headers;
            usuario.PerfilDeAcesso = usuario.Perfil;
            sistema = usuario.SistemaCliente;

            return usuario;
        }

        internal static UserCredentials DecodeCredentials(string base64)
        {
            Regex reRetiraZerosEsquerda = new Regex("(?<=\"(CPF|CNPJ)\": *)0+", RegexOptions.IgnoreCase | RegexOptions.Multiline);

            UserCredentials credentials = null;
            try
            {
                var json = Encoding.UTF8.GetString(Convert.FromBase64String(base64));
                json = reRetiraZerosEsquerda.Replace(json, String.Empty);
                credentials = JsonConvert.DeserializeObject<UserCredentials>(json);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return credentials;
        }
    }
}