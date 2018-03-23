using System;
using System.Configuration;
using System.Net;
using System.Threading.Tasks;
using BNE.Cache;
using BNE.Data.Services.Interfaces;
using IdentityModel.Client;
using IdentityServerAPI;
using IdentityServerAPI.Models;
using log4net;
using Microsoft.Rest;
using SharedKernel.CustomHeader;

namespace BNE.Data.Services
{
    public class IdentityServerService : IIdentityServerService
    {
        private static readonly string Tenant = ConfigurationManager.AppSettings.Get("IS-Tenant");
        private readonly ICachingService _cache;

        private readonly TokenClient _clientToken;

        private readonly ILog _log;

        private string AccessToken
        {
            get
            {
                var key = "AccessToken";
                var accessToken = _cache.GetItem<string>(key);

                if (accessToken == null)
                {
                    _log.Info("Token expirado, gerando outro...");

                    var tokenResponse = _clientToken.RequestClientCredentialsAsync("IdentityServerAPI").Result;

                    var tokenExpireDate = TimeSpan.FromSeconds(tokenResponse.ExpiresIn - tokenResponse.ExpiresIn * 0.1);
                    accessToken = tokenResponse.AccessToken;

                    _cache.AddItem(key, accessToken, tokenExpireDate);

                    _log.Info($"Token criado, vai expirar em {tokenExpireDate}");
                }
                return accessToken;
            }
        }

        private IdentityServerAPIClient Client => new IdentityServerAPIClient(
            new Uri(ConfigurationManager.AppSettings.Get("IS-APIUri")),
            new TokenCredentials(AccessToken));

        public IdentityServerService(TokenClient clientToken, ILog log, ICachingService cache)
        {
            _clientToken = clientToken;
            _log = log;
            _cache = cache;
        }

        public async Task<Guid> CreateUserAccount(string nome, decimal cpf, DateTime dataNascimento)
        {
            var userCommand = new UserCommand
            {
                //TODO: Criação de usuário no Identity Server, mandar email e celular, quando for unico
                Username = nome,
                CPF = cpf.ToString(),
                BirthDate = dataNascimento,
                Tenant = Tenant
            };

            var result = await Client.Account.PostWithHttpMessagesAsync(userCommand, new CorrelationIdCustomHeader().Header());

            if (result.Response.StatusCode == HttpStatusCode.Created && result.Body != null)
            {
                return new Guid(result.Body.ID);
            }

            return Guid.Empty;
        }
    }
}