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


            var listaSMS = (objResult as MensagemPlugin).ListaSMSTanque;
            if (listaSMS != null)
            {
                foreach (MensagemPlugin.MensagemSMSTanque objMensagem in listaSMS)
                {
                    var objMensagemCs = new BLL.MensagemCS(objMensagem.IdMensagem);
                    var retorno = EnviarSMSTanque(objMensagem);

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

        #region EnviarSMSTanque
        public static bool EnviarSMSTanque(MensagemPlugin.MensagemSMSTanque objMensagem)
        {
            using (var objWsTanque = new BLL.BNETanqueService.AppClient())
            {
                var mensagens = new List<BLL.BNETanqueService.Mensagem>();
                var mensagem = new BLL.BNETanqueService.Mensagem
                {
                    ci = objMensagem.IdCurriculo.ToString(),
                    np = objMensagem.NomePessoa,
                    nc = Convert.ToDecimal(objMensagem.DDDCelular.Trim() + objMensagem.NumeroCelular.Trim()),
                    dm = objMensagem.Descricao
                };
                mensagens.Add(mensagem);
                var objEnvio = new BNE.BLL.BNETanqueService.InReceberMensagem();
                objEnvio.cu = objMensagem.IdUsuarioTanque;
                objEnvio.l = mensagens.ToArray();

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
            };
        }
        #endregion

    }
}
