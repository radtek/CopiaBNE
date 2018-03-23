using System;
using System.ComponentModel.DataAnnotations;

namespace BNE.PessoaFisica.ApplicationService.PessoFisica.Model
{
    public class IndicarAmigosResponse
    {
        [Required]
        public decimal CPF { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        public string Nome { get; set; }
        public string Url { get; set; }
    }
}