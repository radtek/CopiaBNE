using System;
using System.Collections.Generic;

namespace BNE.PessoaFisica.Model
{
    public class PessoaFisica
    {
        public int Id { get; set; }
        public decimal CPF { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public string RG { get; set; }
        public string OrgaoEmissor { get; set; }
        public bool FlgPossuiFilhos { get; set; }
        public byte? QtdFilhos { get; set; }
        public string CategoriaHabilitacao { get; set; }
        public string CNH { get; set; }

        public virtual Global.Model.Cidade Cidade { get; set; }
        public virtual EstadoCivil EstadoCivil{ get; set; }
        public virtual Global.Model.EscolaridadeGlobal EscolaridadeGlobal { get; set; }
        public virtual Global.Model.Sexo Sexo { get; set; }
        public virtual Global.Model.DeficienciaGlobal DeficienciaGlobal { get; set; }
        public virtual Endereco Endereco { get; set; }
        
    }
}