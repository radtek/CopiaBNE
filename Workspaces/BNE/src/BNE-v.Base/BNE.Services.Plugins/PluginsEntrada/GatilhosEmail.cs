using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AllInTriggers.Core;
using AllInTriggers;
using Newtonsoft.Json.Linq;
using AllInTriggers.Helper;
using System.Threading;
using BNE.Services.Plugins.PluginsEntrada.AllInInput;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "GatilhosEmail")]
    public class GatilhosEmail : InputPlugin
    {
        private static int TimeoutProcessTrigger = 180000;

        private static readonly Lazy<TriggerWrapper> _manager;
        static GatilhosEmail()
        {
            _manager = new Lazy<TriggerWrapper>(() => new TriggerWrapper());
        }

        protected override IPluginResult DoExecuteTask(Base.ProcessosAssincronos.ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            if (objParametros == null)
                throw new ArgumentNullException("objParametros");

            var realParams = objParametros.Where(a => a != null && a.Parametro != null).ToArray();

            var gatilhoPair = realParams.FirstOrDefault(a => a.Parametro.Equals("IdTipoGatilho"));
            if (gatilhoPair == null)
                throw new NullReferenceException("gatilho");

            var dynObj = new DynObj();
            foreach (var item in realParams)
            {
                dynObj.Add(item.Parametro, item.Valor);
            }

            if (!gatilhoPair.ValorInt.HasValue || gatilhoPair.ValorInt.Value <= 0)
                throw new NullReferenceException("gatilhoPair.ValorInt");

            try
            {
                var gatilho = (BLL.Enumeradores.TipoGatilho)gatilhoPair.ValorInt.Value;

                var promisse = _manager.Value.Shoot(this, gatilho, dynObj, CancellationToken.None);

                if (promisse == null)
                    throw new NullReferenceException("promisse");

                if (promisse.Wait(TimeoutProcessTrigger))
                {
                    var pluginResult = promisse.Result;

                    if (pluginResult == null)
                        throw new NullReferenceException("pluginResult");

                    return pluginResult;
                }

                throw new TimeoutException("Timeout in TriggerWrapper");
            }
            catch (Exception ex)
            {
                if (ex is AggregateException)
                {
                    var agg = (AggregateException)ex;

                    if (agg.InnerExceptions != null && agg.InnerExceptions.Count == 1)
                    {
                        ex = agg.InnerException;
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw ex;
                    }
                }
                EL.GerenciadorException.GravarExcecao(ex);
                throw;
            }
        }
    }

}
