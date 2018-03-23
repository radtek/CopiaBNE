using AllInTriggers.Model;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Threading.Tasks;

namespace BNE.Services.Plugins.PluginSaida.AllInOutput
{
    public class EnvioSaidaAllInPainelCicloVida : EnvioSaidaAllInBase
    {
        private readonly NotificaCicloDeVidaAllIn _valueModelResult;

        public EnvioSaidaAllInPainelCicloVida(NotificaCicloDeVidaAllIn objResult)
        {
            _valueModelResult = objResult;
        }

        public EnvioSaidaAllInPainelCicloVida(TriggerPluginResultEx<NotificaCicloDeVidaAllIn> objResult, ParametroExecucaoCollection aditionalParameters)
            : base(objResult, aditionalParameters)
        {
        }

        public override async Task Process()
        {
            await Do(string.Empty);
        }

        protected override Task<string> ProduceLoginToken()
        {
            return TaskEx.FromResult(string.Empty);
        }

        public override async Task Do(string loginToken)
        {
            var resResult = GetModelDataResult();

            var respostaCicloDeVida = await AllInTriggers.AllInPainel.NotificarCicloDeVidaCall(resResult).ExecuteAsync();

            if (string.IsNullOrWhiteSpace(respostaCicloDeVida))
                throw new NullReferenceException("respostaTransacaoAllIn");

            var valid = await ValidateAndProcessAnswer(respostaCicloDeVida);

            if (!valid)
                throw new InvalidOperationException(string.Format("Resultado inesperado em CicloVidaAlln ({0})", respostaCicloDeVida));
        }

        private NotificaCicloDeVidaAllIn GetModelDataResult()
        {
            if (_valueModelResult != null)
                return _valueModelResult;

            var resResult = (TriggerPluginResultEx<NotificaCicloDeVidaAllIn>)ObjResult;
            return resResult.ResultValue;
        }

        private Task<bool> ValidateAndProcessAnswer(string respostaCicloDeVida)
        {
            return TaskEx.Run(() => respostaCicloDeVida.IndexOf("OK", StringComparison.OrdinalIgnoreCase) > -1);
        }
    }
}
