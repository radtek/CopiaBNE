using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace BNE.Services.Plugins.PluginSaida
{
    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PluginSaidaSMSTanque")]
    public class PluginSaidaSMSTanque : OutputPlugin
    {
        public static object syncRoot = new object();

        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            if (!(objResult is MensagemPlugin))
                throw new IncompatiblePluginException(this, objResult);

            var listaSMSTanque = ((MensagemPlugin)objResult).ListaSMSTanque;
            foreach (var loteSMS in listaSMSTanque)
            {
                if (loteSMS != null)
                {
                    foreach (MensagemPlugin.MensagemSMS objMensagem in loteSMS.mensagens)
                    {
                        var objMensagemCs = new BLL.MensagemCS(objMensagem.IdMensagem);
                        var retorno = EnviarSMSTanque(objMensagem, loteSMS);

                        //Lock da thread, pois está gerando timeout na atualização das mensagens
                        lock (syncRoot)
                        {
                            if (retorno)
                                objMensagemCs.SalvarDataEnvio();
                            else
                                objMensagemCs.SalvarErro();
                        }
                    }
                }
            }

        }

        #region EnviarSMSTanque
        public static bool EnviarSMSTanque(MensagemPlugin.MensagemSMS objMensagem, MensagemPlugin.MensagemSMSTanque dadosEnvio)
        {
            using (var objWsTanque = new BLL.BNETanqueService.AppClient())
            {
                var mensagens = new List<BLL.BNETanqueService.Mensagem>();
                var mensagem = new BLL.BNETanqueService.Mensagem
                {
                    ci = objMensagem.IdCurriculo.ToString(),
                    np = objMensagem.NomePessoa,
                    nc = Convert.ToDecimal(objMensagem.DDDCelular.Trim() + objMensagem.NumeroCelular.Trim()),
                    dm = objMensagem.Descricao,
                    imc = objMensagem.idMensagemCampanha
                };
                mensagens.Add(mensagem);
                var objEnvio = new BLL.BNETanqueService.InReceberMensagem
                {
                    cu = dadosEnvio.IdUsuarioTanque,
                    l = mensagens.ToArray(),
                    idMsgCampanha = mensagem.imc
                };

                try
                {
                    objWsTanque.ReceberMensagem(objEnvio);
                    return true;
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex);
                    return false;
                }
            }
        }
        #endregion

    }
}
