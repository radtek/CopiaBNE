using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Employer.Componentes.UI.Web.EL
{
    /// <summary>
    /// Classe de excessões do componente ImageSlicer
    /// </summary>
    [Serializable]
    public class ImageSlicerException:Exception
    {
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="message">A mensagem de erro</param>
        public ImageSlicerException(String message):base(message)
        {

        }

        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="message">A mensagem de erro</param>
        /// <param name="innerException">A exceção interna</param>
        public ImageSlicerException(String message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
