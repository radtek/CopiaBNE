namespace BNE.PessoaJuridica.Domain.Model
{
    public class Telefone : Comum.Model.TelefoneComum
    {
        
        public virtual PessoaJuridica PessoaJuridica { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual UsuarioPessoaJuridica UsuarioPessoaJuridica { get; set; }

    }
}
