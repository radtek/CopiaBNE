using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "EnvioSMS")]
    public class EnvioSMS : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idMensagem = objParametros["idMensagem"].ValorInt;
            var dddNumero = objParametros["numeroDDD"].Valor;
            var numero = objParametros["numeroCelular"].Valor;
            var mensagem = objParametros["mensagem"].Valor;

            try
            {
                var objMensagemEmailSMS = new MensagemPlugin.MensagemSMS
                {
                    Descricao = mensagem,
                    DDDCelular = dddNumero,
                    NumeroCelular = numero,
                    IdMensagem = idMensagem.Value
                };

                return new MensagemPlugin(this, new List<MensagemPlugin.MensagemSMS> { objMensagemEmailSMS }, false);
            }
            catch (Exception ex)
            {
                Core.LogError(ex);
            }

            return new MensagemPlugin(this, true);
        }
        #endregion

    }
}
