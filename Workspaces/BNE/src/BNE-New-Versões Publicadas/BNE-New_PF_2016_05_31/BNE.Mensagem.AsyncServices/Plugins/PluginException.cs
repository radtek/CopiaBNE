using System;

namespace BNE.Services.AsyncServices.Plugins
{
    /// <summary>
    /// Exceção a ser levantada a qualquer momento pelo objPlugin
    /// </summary>
    [Serializable]
    public sealed class PluginException:Exception
    {
        /// <summary>
        /// O nome do plugin que causou a Exceção
        /// </summary>
        public String PluginName { get; private set; }
        /// <summary>
        /// Cria uma nova instância do objeto
        /// </summary>
        /// <param name="message">A mensagem de erro </param>
        /// <param name="plugin">O objPlugin que soltou a Exceção</param>
        public PluginException(String message, PluginBase plugin)
            :base(String.Format("O plugin \"{0}\" disparou a Exceção: \"{1}\"", plugin.MetadataName,message))
        {
            PluginName = plugin.MetadataName;
        }

        /// <summary>
        /// Cria uma nova instância do objeto
        /// </summary>
        /// <param name="message">A mensagem de erro </param>
        /// <param name="plugin">O objPlugin que soltou a Exceção</param>
        /// <param name="innerException">A Exceção interna</param>
        public PluginException(String message, PluginBase plugin, Exception innerException)
            : base(String.Format("O objPlugin \"{0}\" disparou a Exceção: \"{1}\"", plugin.MetadataName, message),innerException)
        {

        }
        
    }
}
