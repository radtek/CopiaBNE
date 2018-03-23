using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Vagas.Models
{
    public class PessoaIndicada
    {
        public PessoaIndicada(){}
        public string Nome { get; set; }
        public string CelularDDD { get; set; }
        public string CelularNumero { get; set; }
        public string Email { get; set; }
    }
}