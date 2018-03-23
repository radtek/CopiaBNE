using System.ComponentModel.Composition;
using BNE.Services.AsyncServices.Base.Plugins;
using BNE.Services.AsyncServices.Base.Plugins.Interface;
using BNE.Services.AsyncServices.Base.ProcessosAssincronos;

namespace BNE.Mensagem.AsyncExecutor.Plugins.PluginsSaida
{

    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PluginSaidaEmail")]
    public class Email : Plugins.OutputPlugin
    {
        public static object SyncRoot = new object();

        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            var email = DomainEmail;

            if (!(objResult is OutputResult.Mensagem))
                throw new IncompatiblePluginException(this, objResult);

            var listaEmail = ((OutputResult.Mensagem)objResult).ListaEmail;
            if (listaEmail != null)
            {
                foreach (OutputResult.Mensagem.MensagemEmail objMensagem in listaEmail)
                {
                    bool enviado = Core.SendMail(objMensagem.From, objMensagem.To, objMensagem.Assunto, objMensagem.Descricao);

                    lock (SyncRoot)
                    {
                        var objEmail = email.RecuperarEmail(objMensagem.IdMensagem);
                        if (enviado)
                            email.AtualizarDataEnvio(objEmail);
                        else
                            email.AtualizarErro(objEmail);
                    }
                }
            }
        }
    }
}
