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
    [ExportMetadata("Type", "EnvioSMSTanque")]
    public class EnvioSMSTanque : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idMensagemCS = objParametros["idMensagemCS"].ValorInt;
            var idCurriculo = objParametros["idCurriculo"].ValorInt;
            var idUsuarioTanque = objParametros["idUsuarioTanque"].Valor;
            var dddNumero = objParametros["numeroDDD"].Valor;
            var numero = objParametros["numeroCelular"].Valor;
            var nomePessoa = objParametros["nomePessoa"].Valor;
            var mensagem = objParametros["mensagem"].Valor;

            int? Idf_Mensagem_Campanha = null;

            try 
            {
                if (objParametros["Idf_Mensagem_Campanha"].Valor != "")
                    Idf_Mensagem_Campanha = int.Parse(objParametros["Idf_Mensagem_Campanha"].Valor);
            }
            catch (Exception) { }


            try
            {
                var LoteSMSTanque = new MensagemPlugin.MensagemSMSTanque();
                var objMensagemSMS = new MensagemPlugin.MensagemSMS
                {
                    IdCurriculo = idCurriculo.Value,
                    IdMensagem = idMensagemCS.Value,
                    Descricao = mensagem,
                    DDDCelular = dddNumero,
                    NumeroCelular = numero,
                    NomePessoa = nomePessoa,
                    idMensagemCampanha = Idf_Mensagem_Campanha
                };

                LoteSMSTanque.IdUsuarioTanque = idUsuarioTanque;
                LoteSMSTanque.mensagens.Add(objMensagemSMS);

                return new MensagemPlugin(this, new List<MensagemPlugin.MensagemSMSTanque> { LoteSMSTanque }, false);
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
