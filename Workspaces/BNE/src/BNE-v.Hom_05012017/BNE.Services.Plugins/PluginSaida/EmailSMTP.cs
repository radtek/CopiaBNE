using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System.ComponentModel.Composition;

namespace BNE.Services.Plugins.PluginSaida
{
    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "EmailSMTP")]
    public class EmailSMTP : OutputPlugin
    {
        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            if (!(objResult is MensagemPlugin))
                throw new IncompatiblePluginException(this, objResult);

            var listaEmail = ((MensagemPlugin)objResult).ListaEmail;
            if (listaEmail != null)
            {
                foreach (var objMensagem in listaEmail)
                {
                    bool enviado;
                    
                    if (objMensagem.Saida == MensagemPlugin.MensagemEmail.Provider.SMTPCloud)
                        enviado = Core.SendMailSMTPCloud(objMensagem.From, objMensagem.To, objMensagem.Assunto, objMensagem.Descricao);
                    else if (objMensagem.Saida == MensagemPlugin.MensagemEmail.Provider.SMTPCloudCampanha)
                        enviado = Core.SendMailSMTPCloudCampanha(objMensagem.From, objMensagem.To, objMensagem.Assunto, objMensagem.Descricao);
                    else if (objMensagem.Saida == MensagemPlugin.MensagemEmail.Provider.AmazonSES)
                        enviado = Core.SendMailAmazonSES(objMensagem.From, objMensagem.To, objMensagem.Assunto, objMensagem.Descricao);
                    else
                        enviado = Core.SendMail(objMensagem.From, objMensagem.To, objMensagem.Assunto, objMensagem.Descricao);

                    if (enviado)
                    {
                        new BLL.EmailHotmail().SalvarDataEnvio(objMensagem.IdMensagem);
                    }
                }
            }
        }
    }
}
