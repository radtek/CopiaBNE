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

            try
            {
                var objMensagemSMS = new MensagemPlugin.MensagemSMSTanque
                {
                    IdCurriculo = idCurriculo.Value,
                    IdUsuarioTanque = idUsuarioTanque,
                    IdMensagem = idMensagemCS.Value,
                    Descricao = mensagem,
                    DDDCelular = dddNumero,
                    NumeroCelular = numero,
                    NomePessoa = nomePessoa
                };

                return new MensagemPlugin(this, new List<MensagemPlugin.MensagemSMSTanque> { objMensagemSMS }, false);
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
