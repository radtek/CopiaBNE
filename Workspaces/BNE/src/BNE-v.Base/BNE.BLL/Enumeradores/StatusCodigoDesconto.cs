using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BNE.BLL.Enumeradores
{
    public enum StatusCodigoDesconto : int
    {
        Inativo = 1,
        Ativo = 2,
        Utilizado = 3,
        Cancelado = 4,
        Reutilizavel = 5
    }
}
