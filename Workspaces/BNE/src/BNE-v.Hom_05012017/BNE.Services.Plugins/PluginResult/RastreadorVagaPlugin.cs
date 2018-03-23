using System;
using BNE.Services.AsyncServices.Plugins;
using BNE.Services.AsyncServices.Plugins.Interface;

namespace BNE.Services.Plugins.PluginResult
{
    public class RastreadorVagaPlugin : IPluginResult
    {
        
        #region Implementation of IPluginResult

        /// <summary>
        /// O nome do plugin que originou essa resposta
        /// </summary>
        public string InputPluginName { get; private set; }

        /// <summary>
        /// Se true determina que a exeução do processo pode terminar após a execução deste plugin
        /// </summary>
        public bool FinishTask { get; private set; }

        /// <summary>
        /// Se true, determina que a execução do processo vai ser finalizada pelo ponte azul
        /// </summary>
        public bool FinishWithPonteAzul { get; private set; }

        #endregion

        #region Propriedades

        public Vaga Vaga { get; set; }

        #endregion

        #region Construtor
        public RastreadorVagaPlugin(InputPlugin objInputPlugin, int idVaga, string codigoVaga, Boolean finishTask)
        {
            Vaga = new Vaga
                {
                    CodigoVaga = codigoVaga,
                    IdVaga = idVaga
                };
            InputPluginName = objInputPlugin.MetadataName;
            FinishTask = finishTask;
        }
        #endregion

    }

    public class Vaga
    {
        public int IdVaga { get; set; }
        public string CodigoVaga { get; set; }
    }

}
