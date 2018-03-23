using System;

namespace BNE.PessoaFisica.Model
{
    public class CurriculoParametro
    {
        public int Id { get; set; }
        public string Valor { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }

        public virtual Curriculo Curriculo { get; set; }
        public virtual Parametro Parametro { get; set; }
    }
}