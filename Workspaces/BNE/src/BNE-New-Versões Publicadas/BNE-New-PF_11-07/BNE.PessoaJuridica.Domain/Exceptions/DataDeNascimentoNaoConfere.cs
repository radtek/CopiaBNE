using System;

namespace BNE.PessoaJuridica.Domain.Exceptions
{
    public class DataDeNascimentoNaoConfere : Exception
    {
        public DataDeNascimentoNaoConfere()
            : base("Data de nascimento não confere")
        {
        }
    }
}
