using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIGateway.Model
{
    public class UsuarioPessoaJuridica : Usuario
    {
        public decimal CPF { get; set; }
        public decimal CNPJ { get; set; }

        public UsuarioPessoaJuridica(){}

        public UsuarioPessoaJuridica(decimal CPF, decimal CNPJ)
        {
            this.CPF = CPF;
            this.CNPJ = CNPJ;
        }

        public UsuarioPessoaJuridica(decimal CPF, decimal CNPJ, Model.Perfil perfil)
            : base(perfil)
        {
            this.CPF = CPF;
            this.CNPJ = CNPJ;
        }
    }
}
