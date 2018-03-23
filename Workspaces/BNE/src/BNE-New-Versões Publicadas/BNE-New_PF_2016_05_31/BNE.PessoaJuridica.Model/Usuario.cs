using System;

namespace BNE.PessoaJuridica.Model
{
    public class Usuario
    {

        public int Id { get; set; }
        public DateTime DataNascimento { get; set; }
        public decimal CPF { get; set; }
        public string Nome { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataCadastro { get; set; }
        public Guid Guid { get; set; }
        public string IP { get; set; }

        public virtual Global.Model.Sexo Sexo { get; set; }
    }
}
