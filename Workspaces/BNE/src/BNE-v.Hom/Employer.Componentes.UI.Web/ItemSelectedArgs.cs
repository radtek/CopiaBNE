using System;

namespace Employer.Componentes.UI.Web
{
    #region ItemSelectedArgs
    /// <summary>
    /// Os argumentos do evento ItemSelectedEvent
    /// </summary>
    public class ItemSelectedArgs : EventArgs
    {
        #region Private
        private Object value;
        private Object text;
        private Object dataItem;
        #endregion

        #region Properties
        #region Value
        /// <summary>
        /// O valor do ítem selecionado
        /// </summary>
        public Object Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        #endregion
        #region Value
        /// <summary>
        /// O texto de descrição do ítem selecionado
        /// </summary>
        public Object Text
        {
            get { return this.text; }
            set { this.text = value; }
        }
        #endregion
#region DataItem
        /// <summary>
        /// O ítem selecionado vindo do Data Source
        /// </summary>
        public Object DataItem
        {
            get { return this.dataItem; }
            set { this.dataItem = value; }
        }
#endregion 
        #endregion

        #region Ctor
        /// <summary>
        /// Construtor
        /// </summary>
        public ItemSelectedArgs() : this(null, null,null) { }
        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="value">O valor do ítem selecionado</param>        
        /// <param name="text">O texto do ítem selecionado</param>        
        /// <param name="dataItem">O item selecionado vindo do Data Source</param>
        public ItemSelectedArgs(Object value, Object text, Object dataItem)
        {
            this.value = value;
            this.text = text;
            this.dataItem = dataItem;
        }
        #endregion
    }
    #endregion

    #region ItemSelectedEvent
    /// <summary>
    /// Delegate que identifica o evento de Item selecionado
    /// </summary>
    /// <param name="sender">O objeto que disparou o evento</param>
    /// <param name="e">Os argumentos do evento</param>
    public delegate void ItemSelectedEvent(Object sender, ItemSelectedArgs e);
    #endregion 
}
