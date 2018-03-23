using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.Services.Plugins.PluginResult;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using System.ComponentModel.Composition;

namespace BNE.Services.Plugins.PluginSaida
{
    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PluginSaidaPublicacaoVaga")]
    public class PluginSaidaPublicacaoVaga : OutputPlugin
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
