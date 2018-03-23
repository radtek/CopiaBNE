using System;

namespace BNE.PessoaJuridica.Domain.Exceptions
{
    public class CPFInvalido : Exception
    {
        public CPFInvalido()
            : base("CPF Inválido")
        {
        }
    }
}
