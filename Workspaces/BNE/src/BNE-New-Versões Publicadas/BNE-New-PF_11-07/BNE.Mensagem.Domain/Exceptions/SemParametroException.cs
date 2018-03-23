using System.Collections.Generic;
using System.Linq;

namespace BNE.Mensagem.Domain.Exceptions
{
    public class SemParametroException : System.Exception
    {

        public SemParametroException(List<string> parametros)
            : base(string.Format("Os seguintes parâmetros não foram informados: {0}", parametros.Aggregate((a, b) => a + ", " + b)))
        {
        }

    }
}
