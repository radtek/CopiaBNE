using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace APIGateway.Model
{
    public class UsuarioPessoaFisica : Usuario
    {
        public decimal CPF { get; set; }
        public DateTime DataNascimento { get; set; }

        public UsuarioPessoaFisica() { }

        public UsuarioPessoaFisica(decimal CPF)
        {
            this.CPF = CPF;
        }

        public UsuarioPessoaFisica(decimal CPF, Model.Perfil perfil)
            : base(perfil)
        {
            this.CPF = CPF;
        }
    }
}
