using System;

namespace BNE.BLL.Integracoes.IntegradorCurriculo
{
    public class Curriculo
    {

        public int CodigoCurriculo { get; set; }
        public string NumeroCPF { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string EnderecoNumeroCEP { get; set; }
        public string EnderecoLogradouro { get; set; }
        public string EnderecoNumero { get; set; }
        public string EnderecoComplemento { get; set; }
        public string NumeroCelular { get; set; }
        public string NumeroResidencial { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataAtualizacaoCurriculo { get; set; }

    }
}
