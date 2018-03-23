using System;
using System.ComponentModel.DataAnnotations;

namespace BNE.PessoaFisica.ApplicationService.Curriculo.Model
{
    public sealed class ExperienciaProfissionalResponse
    {
        [Required]
        public string Nome { get; set; }

        [Required]
        public decimal CPF { get; set; }

        [Required]
        public DateTime DataNascimento { get; set; }

        public string Url { get; private set; }

        public ExperienciaProfissionalResponse(string nome, decimal cpf, DateTime dataNascimento)
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = dataNascimento;
        }

        public ExperienciaProfissionalResponse(string nome, decimal cpf, DateTime dataNascimento, string url) : this(nome, cpf, dataNascimento)
        {
            Url = url;
        }
    }
}