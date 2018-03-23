using System;

namespace BNE.PessoaFisica.Domain.Model
{
    public class Formacao
    {
        public int Id { get; set; }
        public short? AnoConclusao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public short CargaHoraria { get; set; }
        public bool Ativo { get; set; }
        public string NomeInstituicao { get; set; }
        public string NomeCurso { get; set; }

        public virtual Global.Model.Cidade Cidade { get; set; }
        public virtual InstituicaoEnsino InstituicaoEnsino { get; set; }
        public virtual Curso Curso { get; set; }
        public virtual Global.Model.EscolaridadeGlobal EscolaridadeGlobal { get; set; }
        public virtual Model.PessoaFisica PessoaFisica { get; set; }
    }
}