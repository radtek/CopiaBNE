using System;

namespace BNE.PessoaFisica.Domain.Model
{
    public class Curriculo
    {
        public int Id { get; set; }
        public decimal PretensaoSalarial { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataModificacao { get; set; }
        public bool FlgDisponivelViagem { get; set; }

        public bool FlgVIP { get; set; }
        public bool Ativo { get; set; }
        public string Observacao { get; set; }
        public string Conhecimento { get; set; }

        public virtual PessoaFisica PessoaFisica{ get; set; }
        public virtual TipoCurriculo TipoCurriculo { get; set; }
        public virtual SituacaoCurriculo SituacaoCurriculo { get; set; }
    }
}