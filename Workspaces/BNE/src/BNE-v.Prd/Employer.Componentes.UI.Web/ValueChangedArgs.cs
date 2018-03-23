using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employer.Componentes.UI.Web
{
    #region ValueChangedArgs
    /// <summary>
    /// Argumentos do evento de mudança de valor
    /// </summary>
    public class ValueChangedArgs : EventArgs
    {
        #region Private
        private Object oldValue; // Valor antigo
        private Object newValue; // Novo valor
        private Object oldDescription; // Descrição antiga
        private Object newDescription; // Nova descrição
        #endregion

        #region Properties
        #region OldValue
        /// <summary>
        /// O valor antigo
        /// </summary>
        public Object OldValue
        {
            get { return oldValue; }
            set { oldValue = value; }
        }
        #endregion
        #region NewValue
        /// <summary>
        /// O novo valor
        /// </summary>
        public Object NewValue
        {
            get { return newValue; }
            set { newValue = value; }
        }
        #endregion
        #region OldDescription
        /// <summary>
        /// O descrição antiga
        /// </summary>
        public Object OldDescription
        {
            get { return oldDescription; }
            set { oldDescription = value; }
        }
        #endregion
        #region NewDescription
        /// <summary>
        /// A nova descrição
        /// </summary>
        public Object NewDescription
        {
            get { return newDescription; }
            set { newDescription = value; }
        }
        #endregion
        #endregion

        #region Ctor
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="value">O valor alterado</param>
        /// <param name="description">A nova descrição</param>
        public ValueChangedArgs(Object value, Object description) : this(null, value, null, description) { }
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="oldValue">O valor antigo</param>
        /// <param name="newValue">O novo valor</param>
        /// <param name="oldDescription">A descrição antiga</param>
        /// <param name="newDescription">A nova descrição</param>
        public ValueChangedArgs(Object oldValue, Object newValue, Object oldDescription, Object newDescription)
        {
            this.oldValue = oldValue;
            this.newValue = newValue;
            this.oldDescription = oldDescription;
            this.newDescription = newDescription;
        }
        #endregion
    }
    #endregion

    #region ValueChangedEvent
    /// <summary>
    /// Evento disparado quando um valor é alterado
    /// </summary>
    /// <param name="sender">O objeto que disparou o evento</param>
    /// <param name="e">Os argumentos do evento</param>
    public delegate void ValueChangedEvent(Object sender, ValueChangedArgs e);
    #endregion
}
