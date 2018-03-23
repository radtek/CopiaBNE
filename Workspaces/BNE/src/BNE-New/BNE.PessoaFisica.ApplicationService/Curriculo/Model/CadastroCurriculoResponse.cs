using System;
using System.ComponentModel.DataAnnotations;

namespace BNE.PessoaFisica.ApplicationService.Curriculo.Model
{
    public class CadastroCurriculoResponse
    {
        [Required]
        public decimal CPF { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        public string Nome { get; set; }
        public bool Candidatou { get; set; }
    }
}