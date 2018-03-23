using System;
using System.ComponentModel.Composition;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;

namespace BNE.Services.Plugins.PluginSaida
{
    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PluginSaidaArquivo")]
    public class PluginSaidaArquivo : OutputPlugin
    {
        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            if (!(objResult is MensagemPluginArquivo))
                throw new IncompatiblePluginException(this, objResult);

            String Nome = string.Format((objResult as MensagemPluginArquivo).NomeArquivo, (Guid.NewGuid()).ToString());
            this.Core.SaveFile(Nome,
                (objResult as MensagemPluginArquivo).Arquivo);

            this.Core.SetGeneratedFile(Convert.ToInt32(aditionalParameters["IdfAtividade"].Valor), Nome);

        }
    }
}
