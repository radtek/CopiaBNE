using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.PessoaFisica.Web.DTO
{
    public class ClaimsPessoaFisica
    {
        public decimal Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
    }
}