using System;

namespace BNE.PessoaJuridica.Domain.Exceptions
{
    public class SemPermissaoParaEditar : Exception
    {
        public SemPermissaoParaEditar()
            : base("Para alterar os dados da empresa é necessário ser usuário MASTER")
        {
        }
    }
}
