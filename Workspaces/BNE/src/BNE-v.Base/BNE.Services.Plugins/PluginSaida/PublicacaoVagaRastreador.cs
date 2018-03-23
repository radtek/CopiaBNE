using System;
using System.ComponentModel.Composition;
using System.Globalization;
using BNE.BLL;
using BNE.BLL.AsyncServices;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;

namespace BNE.Services.Plugins.PluginSaida
{

    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PublicacaoVagaRastreador")]
    public class PublicacaoVagaRastreador : OutputPlugin
    {

        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            if (objResult is RastreadorVagaPlugin)
            {
                var objVaga = (objResult as RastreadorVagaPlugin).Vaga;

                var parametros = new ParametroExecucaoCollection
                    {
                        {"idVaga", "Vaga", objVaga.IdVaga.ToString(CultureInfo.InvariantCulture), objVaga.CodigoVaga}
                    };

                ProcessoAssincrono.IniciarAtividade(
                    TipoAtividade.RastreadorVagas,
                    PluginsCompatibilidade.CarregarPorMetadata("RastreadorVagas", "PluginSaidaEmailSMS"),
                    parametros,
                    null,
                    null,
                    null,
                    null,
                    DateTime.Now);
            }
            else if (objResult is MensagemPlugin)
            {
            }
            else
                throw new IncompatiblePluginException(this, objResult);

        }

    }
}
