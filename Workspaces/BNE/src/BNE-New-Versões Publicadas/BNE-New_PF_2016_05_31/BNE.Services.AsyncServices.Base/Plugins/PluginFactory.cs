using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;

namespace BNE.Services.AsyncServices.Base.Plugins
{
    /// <summary>
    /// Classe Factory de Plugins
    /// </summary>
    /// <typeparam name="T">A interface a qual os plugins implementam</typeparam>
    public sealed class PluginFactory<T> : IDisposable
    {

        #region Fields
        /// <summary>
        /// A coleção dos plugins que o componente está servindo
        /// </summary>
        [ImportMany]
        private Lazy<T, IDictionary<String, object>>[] Plugins { get; set; }
        private readonly AggregateCatalog _objCatalog;
        readonly CompositionContainer _objContainer;
        #endregion

        #region Construtor
        /// <summary>
        /// Cria a fábrica e carrega os plugins
        /// </summary>
        /// <param name="catalog">O catálogo de plugins a ser carregado</param>
        public PluginFactory(ComposablePartCatalog catalog)
        {
            if (catalog == null)
                throw new ArgumentNullException("catalog");

            _objCatalog = new AggregateCatalog();
            _objCatalog.Catalogs.Add(catalog);
            _objContainer = new CompositionContainer(_objCatalog);
            _objContainer.ComposeParts(this);
        }
        #endregion

        #region Métodos

        #region GetPlugin
        /// <summary>
        /// Retorna um objPlugin pelo seu tipo de metadado
        /// </summary>
        /// <param name="type">O tipo de metadado do objPlugin</param>
        /// <returns>A instância do objPlugin</returns>
        public T GetPlugin(String type)
        {
            foreach (var objPro in Plugins)
            {
                if ((String)objPro.Metadata["Type"] == type)
                    return objPro.Value;
            }
            return default(T);
        }
        #endregion

        #region GetAllPlugins
        /// <summary>
        /// Retorna a coleção de todos os plugins
        /// </summary>
        /// <returns>A coleção contendo todos os plugins</returns>
        public Collection<T> GetAllPlugins()
        {
            var objPlugins = new Collection<T>();

            foreach (var objPro in Plugins)
                objPlugins.Add(objPro.Value);

            return objPlugins;
        }
        #endregion

        #region Dispose
        /// <inheritdoc />
        public void Dispose()
        {
            if (_objCatalog != null)
                _objCatalog.Dispose();
            if (_objContainer != null)
                _objContainer.Dispose();
        }
        #endregion

        #endregion

    }
}