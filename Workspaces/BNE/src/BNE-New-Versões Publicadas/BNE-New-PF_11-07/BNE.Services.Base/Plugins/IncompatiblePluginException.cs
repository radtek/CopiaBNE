using System;
using BNE.Services.AsyncServices.Plugins.Interface;
using BNE.Services.Base.Plugins.Interface;

namespace BNE.Services.Base.Plugins
{
    /// <summary>
    /// Exceção lançada quando os plugins não são compatíveis
    /// </summary>
    [Serializable]
    public class IncompatiblePluginException : Exception
    {
        /// <summary>
        /// Cria uma nova instância do objeto
        /// </summary>
        /// <param name="objPlugin">O plugin que está lançando a Exceção</param>
        /// <param name="objPluginResult">O resultado que o plugin recebeu</param>
        public IncompatiblePluginException(PluginBase objPlugin, IPluginResult objPluginResult)
            : base(
                String.Format("O plugin \"{0}\" não é compatível com o resultado enviado pelo plugin \"{1}\"",
                objPlugin.MetadataName, objPluginResult.InputPluginName))
        {

        }
    }
}