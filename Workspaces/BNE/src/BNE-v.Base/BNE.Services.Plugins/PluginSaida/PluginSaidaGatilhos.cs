using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using BNE.Services.Plugins.PluginSaida.AllInOutput;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BNE.Services.Plugins.PluginSaida
{
    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PluginSaidaGatilhos")]
    public class PluginSaidaGatilhos : OutputPlugin
    {
        private readonly static SemaphoreSlim _semaphoreSlimPainel = new SemaphoreSlim(1, 1);
        private readonly static SemaphoreSlim _semaphoreSlimTransacional = new SemaphoreSlim(1, 1);
        private int TimeoutRequestAllIn = 60000;

        public bool HabilitaEnvioAllIn
        {
            get
            {
                var res = AllInTriggers.Helper.ConfigHelper.GetConfig("HabilitaEnvioEmailGatilhoAllIn", "true");
                bool enabled;
                bool.TryParse(res, out enabled);
                return enabled;
            }
        }
        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            var res = objResult as TriggerPluginResult;

            if (res == null)
                throw new IncompatiblePluginException(this, objResult);

            if (res.ResultType == AllInResultType.None)
                return;

            EnvioSaidaAllInBase saidaAll = FactoryOutputProcess(aditionalParameters, res);

            if (!HabilitaEnvioAllIn)
                return;
            try
            {
                SemaphoreSlim sem;

                if (res.ResultType == AllInResultType.LifeCycle)
                    sem = _semaphoreSlimPainel;
                else
                    sem = _semaphoreSlimTransacional;

                var entry = sem.Wait(TimeoutRequestAllIn);
                try
                {
                    AllInTriggers.Helper.AsyncPump.Run(() => saidaAll.Process());
                    //Task.Factory.StartNew(() => saidaAll.Process()).Unwrap().Wait(TimeoutRequestAllIn);
                }
                finally
                {
                    if (entry)
                        sem.Release();
                }

            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    var agg = (AggregateException)ex;
                    if (agg.InnerExceptions != null && agg.InnerExceptions.Count == 1)
                    {
                        ex = agg.InnerException;
                    }

                    EL.GerenciadorException.GravarExcecao(ex);
                    throw ex;
                }

                EL.GerenciadorException.GravarExcecao(ex);
                throw;

            }
        }

        private EnvioSaidaAllInBase FactoryOutputProcess(ParametroExecucaoCollection aditionalParameters, TriggerPluginResult res)
        {

            EnvioSaidaAllInBase saidaAll;
            switch (res.ResultType)
            {
                case AllInResultType.Transaction:
                    var resTrans = res as TriggerPluginResultEx<AllInTriggers.Model.EnviaEmailTransacaoAllIn>;
                    if (resTrans == null)
                        throw new IncompatiblePluginException(this, res);

                    saidaAll = new EnvioSaidaAllInTransacional(resTrans, aditionalParameters);
                    break;
                case AllInResultType.LifeCycle:
                    var resCycle = res as TriggerPluginResultEx<AllInTriggers.Model.NotificaCicloDeVidaAllIn>;
                    if (resCycle == null)
                        throw new IncompatiblePluginException(this, res);

                    saidaAll = new EnvioSaidaAllInPainelCicloVida(resCycle, aditionalParameters);
                    break;
                case AllInResultType.None:
                default:
                    saidaAll = null;
                    break;
            }

            return saidaAll;
        }
    }
}
