using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "RemoverVagaEmpresaBloqueada")]
    public class RemoverVagaEmpresaBloqueada : InputPlugin
    {
        public IPluginResult DoExecute(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            return this.DoExecuteTask(objParametros, objAnexos);
        }

        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            int idFilial = (int)objParametros["Idf_Filial"].ValorInt;
            bool status = objParametros["Status"].ValorBool;
            BLL.Vaga.RemoverVagasFilial(idFilial, status);
            return new MensagemPlugin(this, true);
        }

    }
}
