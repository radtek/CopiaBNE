using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Employer.Componentes.UI.Web.Extensions
{
    /// <summary>
    /// Extensões para o HtmlTextWriter
    /// </summary>
    public static class HtmlTextWriterExtensions
    {
        #region WriteTagWithClass
        /// <summary>
        /// Escreve uma tag com a classe css
        /// </summary>
        /// <param name="instance">A instância a ser extendida</param>
        /// <param name="tagName">O nome da tag</param>
        /// <param name="className">O nome da classe css</param>
        public static void WriteTagWithClass(this HtmlTextWriter instance, String tagName, String className)
        {
            if (String.IsNullOrEmpty(className))
                instance.Write(String.Format("<{0}>", tagName));
            else
                instance.Write(String.Format("<{0} class=\"{1}\">", tagName, className));
        }
        #endregion

        #region WriteSpanWithClass
        /// <summary>
        /// Escreve uma span com a classe css
        /// </summary>
        /// <param name="instance">A instancia a ser extendida</param>
        /// <param name="className">O nome da classe css</param>
        public static void WriteSpanWithClass(this HtmlTextWriter instance, String className)
        {
            instance.WriteTagWithClass("span", className);
        }
        #endregion 
    }
}
