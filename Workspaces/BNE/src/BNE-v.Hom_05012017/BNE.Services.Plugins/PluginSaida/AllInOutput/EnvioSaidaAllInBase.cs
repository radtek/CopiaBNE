using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace BNE.Services.Plugins.PluginSaida.AllInOutput
{
    public abstract class EnvioSaidaAllInBase
    {
        private static ImmutableDictionary<Type, KeyValuePair<string, DateTime>> _loginCache = ImmutableDictionary.Create<Type, KeyValuePair<string, DateTime>>();

        private readonly TriggerPluginResult objResult;
        private readonly ParametroExecucaoCollection aditionalParameters;
        private ThreadLocal<int> loginAttempt = new ThreadLocal<int>();
        public virtual TimeSpan LoginExpiration
        {
            get
            {
                return TimeSpan.FromMinutes(5);
            }
        }

        public TriggerPluginResult ObjResult
        {
            get { return objResult; }
        }

        public ParametroExecucaoCollection AditionalParameters
        {
            get { return aditionalParameters; }
        }

        public EnvioSaidaAllInBase()
        {

        }

        public EnvioSaidaAllInBase(TriggerPluginResult objResult, ParametroExecucaoCollection aditionalParameters = null)
        {
            if (objResult == null)
                throw new ArgumentNullException("objResult");

            this.objResult = objResult;
            this.aditionalParameters = aditionalParameters;
        }

        public virtual async Task Process()
        {
            if (loginAttempt == null)
                loginAttempt = new ThreadLocal<int>();
            else
                loginAttempt.Value = 0;

            var token = await GetLoginToken();
            await Do(token);
        }

        protected async Task ReprocessIfIsAuthenticationFailure(string answer)
        {
            if (answer.IndexOf("token", StringComparison.OrdinalIgnoreCase) > -1)
            {
                var lowerRes = answer.ToLower();
                if (lowerRes.Contains("expirado") || lowerRes.Contains("inválido") || lowerRes.Contains("invalido") || lowerRes.Contains("inexistente"))
                {
                    if (loginAttempt.Value > 2)
                        throw new InvalidOperationException(string.Format("Problema no token de autenticação ({0})", answer));

                    loginAttempt.Value = loginAttempt.Value + 1;
                    var newToken = await GetLoginToken(true);

                    if (string.IsNullOrWhiteSpace(newToken))
                        throw new NullReferenceException("newToken");

                    await Do(newToken);
                }
            }
        }

        protected virtual async Task<string> GetLoginToken(bool forceProduceOrRenew = false)
        {
            if (forceProduceOrRenew)
            {
                var newLogin = await ProduceLoginToken();
                Interlocked.Exchange(ref _loginCache, _loginCache.SetItem(this.GetType(), new KeyValuePair<string, DateTime>(newLogin, DateTime.Now)));
                return newLogin;
            }

            KeyValuePair<string, DateTime> token;
            if (_loginCache.TryGetValue(this.GetType(), out token))
            {
                if (!IsExpired(token))
                    return token.Key;
            }

            return await GetLoginToken(true);
        }

        protected virtual bool IsExpired(KeyValuePair<string, DateTime> tokenPair)
        {
            if (DateTime.Now - tokenPair.Value >= LoginExpiration)
            {
                return true;
            }

            return false;
        }

        protected abstract Task<string> ProduceLoginToken();

        public abstract Task Do(string loginToken);
    }
}
