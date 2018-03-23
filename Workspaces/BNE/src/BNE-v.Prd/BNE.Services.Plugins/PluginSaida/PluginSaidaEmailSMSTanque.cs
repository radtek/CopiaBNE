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
            try
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

                var loteSMS = (objResult as MensagemPlugin).ListaSMSTanque;
                // Envia lista com Mensagens do tipo SMS para o Tanque BNE
                if (loteSMS == null || loteSMS.FirstOrDefault().mensagens.Count <= 0)
                {
                    t2 = TaskEx.FromResult(0);
                }
                else
                {
                    t2 = Task.Factory.StartNew(() => EnviarSMSTanque(loteSMS.FirstOrDefault()));
                }

                Task.WaitAll(t1, t2);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "PluginSaidaEmailSMSTanque -> DoExecuteTask");
                throw;
            }
        }
        #endregion

        #region Metodos

        #region EnviarSMSTanque
        public static void EnviarSMSTanque(MensagemPlugin.MensagemSMSTanque loteSMS)
        {
            try
            {
                if (loteSMS.IdUsuarioTanque == "" || loteSMS.IdUsuarioTanque == null) //Envio Candidato Vaga Perfil separando por DDD
                {
                    var listaSMS41 = new List<BLL.BNETanqueService.Mensagem>();
                    var listaSMS11 = new List<BLL.BNETanqueService.Mensagem>();
                    var idUFPTanque41 = BNE.BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUfpEnvioSmsTanqueVaga41);
                    var idUFPTanque11 = BNE.BLL.Parametro.RecuperaValorParametro(BLL.Enumeradores.Parametro.IdUfpEnvioSmsTanqueVaga11);

                    //Grupo de SMS sem usuário do tanque definido.
                    //Varrer a lista para criar os objetos de SMS para o tanque
                    foreach (var sms in loteSMS.mensagens)
                    {
                        var mensagem = new BLL.BNETanqueService.Mensagem
                        {
                            ci = Convert.ToString(sms.IdCurriculo),
                            np = sms.NomePessoa,
                            nc = Convert.ToDecimal(sms.DDDCelular.Trim().Replace(" ", "") + sms.NumeroCelular.Trim().Replace(" ", "")),
                            dm = sms.Descricao,
                            imc = sms.idMensagemCampanha
                        };

                        if (sms.DDDCelular.Trim().Equals("41"))
                            listaSMS41.Add(mensagem);
                        else
                            listaSMS11.Add(mensagem);
                    }

                    // Envia SMS com DDD 11
                    if (listaSMS11.Count > 0)
                        DispararMensagensTanque(listaSMS11, idUFPTanque11, loteSMS.IdUsuarioOrigem, loteSMS.desCidade, loteSMS.desFuncao, loteSMS.idVaga, loteSMS.idCampanha);

                    // Envia SMS com DDD 41
                    if (listaSMS41.Count > 0)
                        DispararMensagensTanque(listaSMS41, idUFPTanque41, loteSMS.IdUsuarioOrigem, loteSMS.desCidade, loteSMS.desFuncao, loteSMS.idVaga, loteSMS.idCampanha);

                }
                else
                {
                    var listaSMSGenerica = new List<BLL.BNETanqueService.Mensagem>();

                    foreach (var sms in loteSMS.mensagens)
                    {
                        var mensagem = new BLL.BNETanqueService.Mensagem
                        {
                            ci = Convert.ToString(sms.IdCurriculo),
                            np = sms.NomePessoa,
                            nc = Convert.ToDecimal(sms.DDDCelular.Trim().Replace(" ", "") + sms.NumeroCelular.Trim().Replace(" ", "")),
                            dm = sms.Descricao,
                            imc = sms.idMensagemCampanha
                        };
                        listaSMSGenerica.Add(mensagem);
                    }

                    // Envia SMS usuário genérico
                    if (listaSMSGenerica.Count > 0)
                        DispararMensagensTanque(listaSMSGenerica, loteSMS.IdUsuarioTanque, loteSMS.IdUsuarioOrigem, loteSMS.desCidade, loteSMS.desFuncao, loteSMS.idVaga, loteSMS.idCampanha);
                }
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "PluginSaidaEmailSMSTanque -> EnviarSMSTanque");
                throw;
            }
        }
        #endregion

        #region DispararMensagensTanque
        public static void DispararMensagensTanque(List<BLL.BNETanqueService.Mensagem> listaSMS, string UsuarioTanque, string UsuarioTanqueOrigem, string desCidade, string desFuncao, int idVaga, int idCampanha)
        {
            using (var objWsTanque = new BLL.BNETanqueService.AppClient())
            {
                try
                {
                    var conversa = new BNE.BLL.BNETanqueService.ConversaDTO()
                    {
                        usuarioOrigem = UsuarioTanqueOrigem,
                        cidade = desCidade,
                        funcao = desFuncao,
                        campanha = idCampanha,
                        vaga = idVaga
                    };

                    var receberMensagem = new BNE.BLL.BNETanqueService.InReceberMensagem
                    {
                        l = listaSMS.ToArray(),
                        cu = UsuarioTanque,
                        conversa = conversa
                    };

                    objWsTanque.ReceberMensagem(receberMensagem);
                }
                catch (Exception ex)
                {
                    EL.GerenciadorException.GravarExcecao(ex, "PluginSaidaEmailSMSTanque -> DispararMensagensTanque");
                    throw;
                }
            }
        }
        #endregion

        #endregion

    }
}
