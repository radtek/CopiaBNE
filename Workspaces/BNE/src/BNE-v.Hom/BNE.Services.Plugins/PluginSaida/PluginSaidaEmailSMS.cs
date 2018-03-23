using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System.ComponentModel.Composition;

namespace BNE.Services.Plugins.PluginSaida
{
    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PluginSaidaEmailSMS")]
    public class PluginSaidaEmailSMS : OutputPlugin
    {
        public static object syncRoot = new object();

        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            if (!(objResult is MensagemPlugin))
                throw new IncompatiblePluginException(this, objResult);

            var listaEmail = (objResult as MensagemPlugin).ListaEmail;
            if (listaEmail != null)
            {
                foreach (MensagemPlugin.MensagemEmail objMensagem in listaEmail)
                {
                    var diagnostic = new System.Diagnostics.Stopwatch();
                    diagnostic.Start();

                    bool enviado;
                    if (objMensagem.Arquivo != null)
                        enviado = Core.SendMail(objMensagem.From, objMensagem.To, objMensagem.Assunto, objMensagem.Descricao, objMensagem.Arquivo);
                    else
                        enviado = Core.SendMail(objMensagem.From, objMensagem.To, objMensagem.Assunto, objMensagem.Descricao);

                    //Lock da thread, pois está gerando timeout na atualização das mensagens
                    lock (syncRoot)
                    {
                        switch (objMensagem.TabelaOrigem)
                        {
                            case MensagemPlugin.MensagemEmail.Tabela.MensagemCS:
                                if (enviado)
                                    new BLL.MensagemCS(objMensagem.IdMensagem).SalvarDataEnvio();
                                else
                                    new BLL.MensagemCS(objMensagem.IdMensagem).SalvarErro();
                                break;
                            case MensagemPlugin.MensagemEmail.Tabela.MensagemMailing:
                                if (enviado)
                                    new BLL.MensagemMailing().SalvarDataEnvio(objMensagem.IdMensagem);
                                else
                                    new BLL.MensagemMailing().SalvarErro(objMensagem.IdMensagem);
                                break;
                        }
                    }
                    diagnostic.Stop();
                }
            }

            var listaSMS = (objResult as MensagemPlugin).ListaSMS;
            if (listaSMS != null)
            {
                foreach (MensagemPlugin.MensagemSMS objMensagem in listaSMS)
                {
                    var objMensagemCs = new BLL.MensagemCS(objMensagem.IdMensagem);
                    var enviado = Core.SendSMS(objMensagem.DDDCelular + objMensagem.NumeroCelular, objMensagem.Descricao, objMensagem.IdMensagem.ToString());
                    //Lock da thread, pois está gerando timeout na atualização das mensagens
                    lock (syncRoot)
                    {
                        if (enviado)
                            objMensagemCs.SalvarDataEnvio();
                        else
                            objMensagemCs.SalvarErro();
                    }
                }
            }
        }
    }
}
