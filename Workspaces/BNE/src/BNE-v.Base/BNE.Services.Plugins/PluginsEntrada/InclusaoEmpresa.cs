using BNE.BLL;
using BNE.BLL.Custom;
using BNE.BLL.Custom.Email;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using Enumeradores = BNE.BLL.AsyncServices.Enumeradores;
using Parametro = BNE.BLL.AsyncServices.Parametro;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "InclusaoEmpresa")]
    public class InclusaoEmpresa : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var idFilial = objParametros["idFilial"].ValorInt;
            try
            {
                if (idFilial.HasValue)
                {
                    var objFilial = Filial.LoadObject(idFilial.Value);

                    string emailRementente = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailMensagens);
                    string emailDestinatario = Parametro.RecuperaValorParametro(Enumeradores.Parametro.EmailInclusaoEmpresa);
                    string mensagem = ModeloEmail.CorpoVisualizacaoCadastroEmpresa(objFilial, true);

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar("Inclusão da empresa " + objFilial.RazaoSocial, mensagem, emailRementente, emailDestinatario);

                    return new MensagemPlugin(this, true);
                }
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
