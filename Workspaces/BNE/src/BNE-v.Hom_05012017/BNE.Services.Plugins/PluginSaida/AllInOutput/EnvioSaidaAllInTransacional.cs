using AllInTriggers.Model;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BNE.Services.Plugins.PluginSaida.AllInOutput
{
    public class EnvioSaidaAllInTransacional : EnvioSaidaAllInBase
    {

        public EnvioSaidaAllInTransacional(TriggerPluginResultEx<EnviaEmailTransacaoAllIn> objResult, ParametroExecucaoCollection aditionalParameters)
            : base(objResult, aditionalParameters)
        {

        }

        protected override Task<string> ProduceLoginToken()
        {
            return AllInTriggers.AllInTransaction.LoginTransactionCall().ExecuteAsync();
        }

        public override async Task Do(string loginToken)
        {
            var objResult = (TriggerPluginResultEx<EnviaEmailTransacaoAllIn>)ObjResult;

            var respostaTransacaoAllIn = await AllInTriggers.AllInTransaction.EnviarEmailCall(loginToken, objResult.ResultValue).ExecuteAsync();

            if (string.IsNullOrWhiteSpace(respostaTransacaoAllIn))
                throw new NullReferenceException("respostaTransacaoAllIn");

            await ReprocessIfIsAuthenticationFailure(respostaTransacaoAllIn);

            var valid = await ValidateAndProcessAnswer(respostaTransacaoAllIn);

            if (!valid)
                throw new InvalidOperationException(string.Format("Resultado inesperado em TransacaoAllIn ({0})", respostaTransacaoAllIn));
        }

        private Task<bool> ValidateAndProcessAnswer(string answer)
        {
            return Task.Factory.StartNew(() =>
                {
                    var numberResult = answer.SkipWhile(a => !char.IsNumber(a))
                          .TakeWhile(a => char.IsNumber(a)).ToArray();

                    if (numberResult.Length <= 0)
                        return false;

                    var idGenerated = Convert.ToInt64(numberResult);
                    return true;
                });
        }
    }
}
