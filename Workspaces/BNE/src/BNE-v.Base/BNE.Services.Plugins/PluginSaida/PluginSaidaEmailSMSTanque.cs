using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using BNE.EL;
using BNE.BLL.Custom.Email;
using System.Threading.Tasks;

namespace BNE.Services.Plugins.PluginSaida
{
    [Export(typeof(IOutputPlugin))]
    [ExportMetadata("Type", "PluginSaidaEmailSMSTanque")]
    public class PluginSaidaEmailSMSTanque : OutputPlugin
    {
        #region DoExecuteTask
        protected override void DoExecuteTask(IPluginResult objResult, ParametroExecucaoCollection aditionalParameters)
        {
            if (!(objResult is MensagemPlugin))
                throw new IncompatiblePluginException(this, objResult);

            Task t1;

            var listaEmail = (objResult as MensagemPlugin).ListaEmail;
            if (listaEmail == null || listaEmail.Count <= 0)
            {
                t1 = TaskEx.FromResult(0);
            }
            else
            {
                t1 = Task.Factory.StartNew(() =>
              {
                  foreach (MensagemPlugin.MensagemEmail objMensagem in listaEmail)
                  {
                      Core.SendMail(objMensagem.From, objMensagem.To, objMensagem.Assunto, objMensagem.Descricao);
                  }
              });
            }


            Task t2;

            var listaSMS = (objResult as MensagemPlugin).ListaSMSTanque;
            // Envia lista com Mensagens do tipo SMS para o Tanque BNE
            if (listaSMS == null || listaSMS.Count <= 0)
            {
                t2 = TaskEx.FromResult(0);
            }
            else
            {
                t2 = Task.Factory.StartNew(() => EnviarSMSTanque(listaSMS));
            }

            Task.WaitAll(t1, t2);
        }
        #endregion

        #region Metodos

        public static void EnviarSMSTanque(List<MensagemPlugin.MensagemSMSTanque> listaSMS)
        {
            using (var objWsTanque = new BLL.BNETanqueService.AppClient())
            {
                var listaSMS41 = new List<BLL.BNETanqueService.Mensagem>();
                var listaSMS11 = new List<BLL.BNETanqueService.Mensagem>();
                var idUFPTanque41 = BNE.BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUfpEnvioSmsTanqueVaga41);
                var idUFPTanque11 = BNE.BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUfpEnvioSmsTanqueVaga11);

                foreach (var objMensagem in listaSMS)
                {
                    var mensagem = new BLL.BNETanqueService.Mensagem
                    {
                        ci = Convert.ToString(objMensagem.IdCurriculo),
                        np = objMensagem.NomePessoa,
                        nc = Convert.ToDecimal(objMensagem.DDDCelular.Trim() + objMensagem.NumeroCelular.Trim()),
                        dm = objMensagem.Descricao
                    };

                    if (objMensagem.DDDCelular.Trim().Equals("41"))
                        listaSMS41.Add(mensagem);
                    else
                        listaSMS11.Add(mensagem);
                }

                // Envia SMS com DDD 11

                if (listaSMS11.Count > 0)
                {
                    var receberMensagem11 = new BNE.BLL.BNETanqueService.InReceberMensagem
                        {
                            l = listaSMS11.ToArray(),
                            cu = idUFPTanque11
                        };

                    try
                    {
                        objWsTanque.ReceberMensagem(receberMensagem11);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                }

                // Envia SMS com DDD 41

                if (listaSMS41.Count > 0)
                {
                    var receberMensagem41 = new BNE.BLL.BNETanqueService.InReceberMensagem
                        {
                            l = listaSMS41.ToArray(),
                            cu = idUFPTanque41
                        };
                    try
                    {
                        objWsTanque.ReceberMensagem(receberMensagem41);
                    }
                    catch (Exception ex)
                    {
                        EL.GerenciadorException.GravarExcecao(ex);
                        throw;
                    }
                }
            };
        }

        #endregion
    }
}
