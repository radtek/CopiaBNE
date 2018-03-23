using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "CampanhaMensagem")]
    public class CampanhaMensagem : InputPlugin
    {
        public IPluginResult DoExecute(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            return this.DoExecuteTask(objParametros, objAnexos);
        }

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var parametroIdCampanhaMensagem = objParametros["idCampanhaMensagem"];

            if (parametroIdCampanhaMensagem == null || !parametroIdCampanhaMensagem.ValorInt.HasValue)
                throw new ArgumentNullException("parametroIdCampanha");

            var idCampanhaMensagem = parametroIdCampanhaMensagem.ValorInt.Value;

            if (idCampanhaMensagem <= 0)
                throw new ArgumentOutOfRangeException("parametroIdCampanha");

            //Buscar objeto da campanha de mensagens
            BLL.CampanhaMensagem objCampanhaMensagem = BLL.CampanhaMensagem.LoadObject(idCampanhaMensagem);

            //Carrega lista dos destinatarios
            List<BLL.CampanhaMensagemEnvios> lstCampanhaMensagemEnvios = new List<BLL.CampanhaMensagemEnvios>();
            lstCampanhaMensagemEnvios = BLL.CampanhaMensagemEnvios.RetornarPorIdCampanhaMensagem(objCampanhaMensagem.IdCampanhaMensagem);
            
            //Realiza o processamento da campanha de mensagens
            string retorno = "";
            BLL.Custom.EnvioMensagens.EnvioMensagens.EnviarMensagemCV(objCampanhaMensagem, lstCampanhaMensagemEnvios, out retorno, true);

            return new MensagemPlugin(this, true);
        }
        #endregion

        #region Metodos

        

        #region MontarSMS
        private MensagemPlugin.MensagemSMS MontarObjetoSMS(int idCurriculo, string nomePessoa, string numeroDDD, string numeroCelular, string descricao, int? IdMensagemCampanha = null)
        {
            try
            {
                return new MensagemPlugin.MensagemSMS
                {
                    IdCurriculo = idCurriculo,
                    NomePessoa = nomePessoa,
                    DDDCelular = numeroDDD,
                    NumeroCelular = numeroCelular,
                    Descricao = descricao,
                    idMensagemCampanha = IdMensagemCampanha
                };
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro ao montar mensagens SMS.");
                throw;
            }
        }
        #endregion

        #region MontarEmail
        private MensagemPlugin.MensagemEmail MontarEmail(string descricaoAssunto, string descricaoMensagem, string emailRemetente, string emailDestinatario)
        {
            return new MensagemPlugin.MensagemEmail
            {
                Assunto = descricaoAssunto,
                Descricao = descricaoMensagem,
                From = emailRemetente,
                To = emailDestinatario
            };
        }
        #endregion

        #endregion
    }
}
