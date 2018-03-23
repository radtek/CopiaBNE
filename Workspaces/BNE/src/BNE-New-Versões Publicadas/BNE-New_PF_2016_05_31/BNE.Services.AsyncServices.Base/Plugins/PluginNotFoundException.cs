using System;

namespace BNE.Services.AsyncServices.Base.Plugins
{
    /// <summary>
    /// Exceção a ser disparada quando os plugins não forem encontrados
    /// </summary>
    [Serializable]
    public sealed class PluginNotFoundException : Exception
    {
        /// <summary>
        /// Inicia a instância do objeto
        /// </summary>
        /// <param name="pluginName">O nome do objPlugin que não foi encontrado</param>
        public PluginNotFoundException(String pluginName)
            : base(String.Format("O plugin \"{0}\" não foi encontrado", pluginName))
        {

        }
    }
}