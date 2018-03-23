using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.Componentes.EL
{
    public class ImageSlicerException : Exception
    {
        public ImageSlicerException()
            : base()
        {

        }

        public ImageSlicerException(string message)
            : base(message)
        {

        }

        public ImageSlicerException(string message, Exception ex)
            : base(message, ex)
        { 
        
        }
    }
}
