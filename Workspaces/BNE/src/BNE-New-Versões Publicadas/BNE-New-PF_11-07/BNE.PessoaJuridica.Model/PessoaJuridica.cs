using System;
using System.Collections.Generic;

namespace BNE.PessoaJuridica.Model
{
    public class PessoaJuridica
    {

        public int Id { get; set; }
        public decimal CNPJ { get; set; }
        public string Site { get; set; }
        public bool Matriz { get; set; }
        public DateTime DataAbertura { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string SituacaoCadastral { get; set; }
        public int QuantidadeFuncionario { get; set; }
        public DateTime DataAlteracao { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
        public string IP { get; set; }
        public Guid Guid { get; set; }

        public virtual CNAE CNAE { get; set; }
        public virtual NaturezaJuridica NaturezaJuridica { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual ICollection<UsuarioPessoaJuridica> UsuarioPessoaJuridica { get; set; }
        public virtual ICollection<Email> Email { get; set; }

    }
}
