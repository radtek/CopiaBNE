using System;

namespace BNE.PessoaJuridica.Domain.Exceptions
{
    public class CNPJInvalido : Exception
    {
        public CNPJInvalido()
            : base("CNPJ inválido")
        {
        }
    }
}
