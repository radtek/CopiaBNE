using BNE.Services.AsyncServices.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.ProcessosAssincronos;
using System.ComponentModel.Composition;
using BNE.BLL;
using BNE.Services.Plugins.PluginResult;
using Sine.Integracao.Client;
using Sine.Integracao.Api;

namespace BNE.Services.Plugins.PluginsEntrada
{
    [Export(typeof(IInputPlugin))]
    [ExportMetadata("Type", "IntegrarCandidaturaSine")]
    public class IntegrarCandidaturaSine : InputPlugin
    {

        protected override IPluginResult DoExecuteTask(ParametroExecucaoCollection objParametros, Dictionary<string, byte[]> objAnexos)
        {
            var IdVaga = objParametros["IdVaga"].ValorInt;
            var IdCurriculo = objParametros["IdCurriculo"].ValorInt;

            VagaIntegracao objVagaIntegracao;
            try
            {
                if (VagaIntegracao.RecuperarIntegradorPorVaga(IdVaga.Value, out objVagaIntegracao))
                {
                    var urlCandidatura = objVagaIntegracao.Integrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Url_Retorno_Candidatura);
#if DEBUG
                    urlCandidatura = "http://localhost:51899";
#endif                
                    if (!string.IsNullOrEmpty(urlCandidatura))
                    {
                        var candidaturaSine = new Sine.Integracao.Model.Candidatura { Codigovaga = objVagaIntegracao.CodigoVagaIntegrador };
                        candidaturaSine.Curriculo = new Sine.Integracao.Model.Curriculo { Codigo = IdCurriculo.ToString() };

                        Curriculo objCurriculo = Curriculo.LoadObject(IdCurriculo.Value);
                        var enviaCV = string.Empty;
                        if (objCurriculo.FlagVIP)
                        {
                            enviaCV = objVagaIntegracao.Integrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Flg_Envia_CV_Vip);
                        }
                        else
                        {
                            enviaCV = objVagaIntegracao.Integrador.GetValorParametro(BLL.Enumeradores.Parametro.Integracao_Flg_Envia_CV_Nao_Vip);
                        }
                        if (!string.IsNullOrEmpty(enviaCV) && (enviaCV == "1" || enviaCV.ToLower() == "true"))
                        {
                            objCurriculo.CompletarCurriculoIntegracao(candidaturaSine.Curriculo);
                        }


                        var _client = new ApiClient();
                        var _job = new JobApi(urlCandidatura);

                        _job.Configuration.DefaultHeader.Add("apikey", "3e9ca441-1e58-4a85-90b7-eab561edf25b");
                        _job.JobCandidatar(candidaturaSine);
                       
                    }
                }

                return new MensagemPlugin(this, true);
            }
            catch (Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Erro para integrar candidatura");
                throw new NotImplementedException();
            }

            
        }

    }
}
