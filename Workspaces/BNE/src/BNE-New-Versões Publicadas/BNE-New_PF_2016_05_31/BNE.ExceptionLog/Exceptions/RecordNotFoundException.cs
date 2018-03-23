using System;

namespace BNE.ExceptionLog.Exceptions
{
    public class RecordNotFoundException : Exception
    {
        public RecordNotFoundException(Type tipoObjeto)
            : base(string.Format("Registro do tipo {0} não encontrado.", tipoObjeto.Name))
        {
        }
    }
}
