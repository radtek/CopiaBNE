namespace BNE.Services.AsyncServices.Base.Plugins.Interface
{
    /// <summary>
    /// Interface que descreve o comportamento básico de um objPlugin
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Inicializa o objPlugin
        /// </summary>
        /// <param name="objCoreCapabilities">As capacidades do núcleo que está chamando o objPlugin</param>        
        void InitializeComponent(CoreCapabilities objCoreCapabilities);
    }
}