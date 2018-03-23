using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Services.Plugins.PluginSaida
{
    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PluginSaidaIntegrarCandidaturaSine")]
    public class PluginSaidaIntegrarCandidaturaSine : OutputPlugin
    {
        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
