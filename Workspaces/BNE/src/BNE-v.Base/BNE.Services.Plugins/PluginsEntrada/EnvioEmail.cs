using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "EnvioEmail")]
    public class EnvioEmail : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idMensagem = objParametros["idMensagem"].ValorInt;
            var emailRementente = objParametros["emailRemetente"].Valor;
            var emailDestinatario = objParametros["emailDestinatario"].Valor;
            var assunto = objParametros["assunto"].Valor;
            var mensagem = objParametros["mensagem"].Valor;

            try
            {
                var objMensagemEmail = new MensagemPlugin.MensagemEmail
                {
                    Assunto = assunto,
                    Descricao = mensagem,
                    From = emailRementente,
                    To = emailDestinatario,
                    IdMensagem = idMensagem.Value,
                    Arquivo = objAnexos
                };

                return new MensagemPlugin(this, new List<MensagemPlugin.MensagemEmail> { objMensagemEmail }, false);
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
