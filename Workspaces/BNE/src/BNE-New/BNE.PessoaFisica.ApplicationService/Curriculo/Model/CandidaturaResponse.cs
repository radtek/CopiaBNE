using System;
using System.ComponentModel.DataAnnotations;

namespace BNE.PessoaFisica.ApplicationService.Curriculo.Model
{
    public class CandidaturaResponse
    {
        [Required]
        public Decimal CPF { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        public string Url { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        public bool Candidatou { get; set; }
        [Required]
        public bool UsuarioInvalido { get; set; }
    }
}