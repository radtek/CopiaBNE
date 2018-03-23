using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Comum.Model.Localizable
{
    /// <summary>
    /// Interface utilizada para definir que a entidade é traduzível
    /// </summary>
    public interface ILocalizableEntity
    {
        /// <summary>
        /// Estado atual da tradução da entidade.
        /// </summary>
        Enum.TranslationState TranslationState { get; set; }
    }
}
