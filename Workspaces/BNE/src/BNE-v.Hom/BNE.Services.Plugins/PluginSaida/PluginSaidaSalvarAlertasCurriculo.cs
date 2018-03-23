using BNE.Services.AsyncServices.Plugins;
using System;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using System.ComponentModel.Composition;

namespace BNE.Services.Plugins.PluginSaida
{
    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PluginSaidaSalvarAlertasCurriculo")]
    public class PluginSaidaSalvarAlertasCurriculo : OutputPlugin
    {
        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            throw new NotImplementedException();
        }
    }
}
