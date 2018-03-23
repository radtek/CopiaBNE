using System;
using System.ComponentModel.Composition;
using BNE.Services.AsyncServices.Base.Plugins.Interface;

namespace BNE.Services.AsyncServices.Base.Plugins
{
    /// <summary>
    /// Classe base para todos os plugins
    /// </summary>
    public abstract class PluginBase : IPlugin
    {

        #region Campos
        /// <summary>
        /// As capacidades do núcleo que está chamando o objPlugin
        /// </summary>
        protected CoreCapabilities Core { get; private set; }
        #endregion

        #region Propriedades

        #region MetadataName
        /// <summary>
        /// Recupera o valor da metadata Type do atributo ExportMetadata
        /// </summary>
        public String MetadataName
        {
            get
            {
                Attribute[] atrs = Attribute.GetCustomAttributes(GetType());

                foreach (Attribute a in atrs)
                {
                    if (a is ExportMetadataAttribute)
                    {
                        if ("Type".Equals((a as ExportMetadataAttribute).Name, StringComparison.OrdinalIgnoreCase))
                            return Convert.ToString((a as ExportMetadataAttribute).Value);
                    }
                }

                return String.Empty;
            }
        }
        #endregion

        #endregion

        #region Eventos

        #region Initialize
        /// <summary>
        /// Evento disparado ao inicializar o objPlugin
        /// </summary>
        public event EventHandler Initialize;
        #endregion

        #endregion

        #region Métodos

        #region InitializeComponent
        /// <summary>
        /// Inicia o objPlugin passando as capacidades do núcleo do sistema
        /// </summary>
        /// <param name="objCoreCapabilities">As capacidades do núcleo do sistema</param>
        public void InitializeComponent(CoreCapabilities objCoreCapabilities)
        {
            Core = objCoreCapabilities;
            if (Initialize != null)
                Initialize(this, new EventArgs());
        }
        #endregion

        #endregion

    }
}
