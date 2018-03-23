using System;
using System.Collections.Generic;

namespace BNE.PessoaJuridica.Domain.Model
{
    public class UsuarioPessoaJuridica
    {

        public int IdUsuario { get; set; }
        public int IdPessoaJuridica { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public Guid Guid { get; set; }
        public string Funcao { get; set; }
        public string IP { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Perfil Perfil { get; set; }
        public virtual PessoaJuridica PessoaJuridica { get; set; }
        public virtual ICollection<Email> Email { get; set; }
        public virtual Global.Model.FuncaoSinonimo FuncaoSinonimo { get; set; }
    }
}
