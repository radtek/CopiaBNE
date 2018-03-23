using System;

namespace BNE.PessoaFisica.Domain.Aggregates
{
    public sealed class Candidatura
    {
        public decimal CPF { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public string URL { get; private set; }
        public string Nome { get; private set; }
        public bool Candidatou { get; private set; }
        public bool UsuarioInvalido { get; private set; }

        public Candidatura(string nome, decimal cpf, DateTime dataNascimento, string url, bool candidatou, bool usuarioInvalido)
        {
            CPF = cpf;
            DataNascimento = dataNascimento;
            URL = url;
            Nome = nome;
            Candidatou = candidatou;
            UsuarioInvalido = usuarioInvalido;
        }
    }
}
