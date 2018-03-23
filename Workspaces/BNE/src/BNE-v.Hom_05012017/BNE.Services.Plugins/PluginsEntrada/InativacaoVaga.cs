using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using TipoAtividade = BNE.BLL.AsyncServices.Enumeradores.TipoAtividade;
using BNE.BLL;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using BNE.Services.Plugins.PluginResult;

namespace BNE.Services.Plugins.PluginsEntrada
{

    #region Help

    /*
     *      FILA QUE RECEBE O ID DA VAGA CADASTRADA VIA IMPORTAÇÃO, DE ORIGEM SINE, E REALIZA O PROCESSO DE INATIVAÇÃO DA VAGA
     */

    #endregion

    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "InativacaoVaga")]
    public class InativacaoVaga : InputPlugin
    {

        #region DoExecuteTask
        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            try
            {
                #region Parametros Mensagem
                var idIntegrador = objParametros["Integrador"].ValorInt;
                var idVagaSine = objParametros["idVagaSine"].Valor;
                var oportunidade = objParametros != null ? Convert.ToBoolean(objParametros["Oportunidade"].Valor) : false;

                #endregion

                BLL.Custom.Vaga.IntegracaoVaga oIntegradorVaga = new BLL.Custom.Vaga.IntegracaoVaga();

                //Carrega o integrador da vaga
                Integrador oIntegrador = new Integrador();
                oIntegrador = Integrador.LoadObject(idIntegrador.Value);

                //Executa o processo de arquivação da Vaga
                if (oportunidade)
                {
                    oIntegradorVaga.ArquivarVaga(idVagaSine, oIntegrador);
                    
                }
                //Executa o processo de inativação da Vaga
                else
                {
                    oIntegradorVaga.InativaVaga(idVagaSine, oIntegrador);                    
                }

                return new MensagemPlugin(this, true);

            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "appBNE - Plugin InativacaoVaga");
                throw;
            }
            finally
            {

            }
        }
        #endregion

        #region Métodos



        #endregion
    }
}
