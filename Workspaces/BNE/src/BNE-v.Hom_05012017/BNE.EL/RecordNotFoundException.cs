using System;

namespace BNE.EL
{
    public class RecordNotFoundException : System.Exception
    {
        public RecordNotFoundException(Type tipoObjeto)
            : base(string.Format("Registro do tipo {0} não encontrado.", tipoObjeto.Name))
        {
        }
    }
}
