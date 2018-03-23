using System;
using System.ComponentModel.DataAnnotations;

namespace BNE.PessoaFisica.ApplicationService.Curriculo.Model
{
    public sealed class FormacaoResponse
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public decimal CPF { get; set; }
        [Required]
        public DateTime DataNascimento { get; set; }
        public string Url { get; set; }
        public FormacaoResponse(string nome, decimal cpf, DateTime dataNascimento)
        {
            Nome = nome;
            CPF = cpf;
            DataNascimento = dataNascimento;
        }
        public FormacaoResponse(string nome, decimal cpf, DateTime dataNascimento, string url) : this(nome, cpf, dataNascimento)
        {
            Url = url;
        }
    }
}