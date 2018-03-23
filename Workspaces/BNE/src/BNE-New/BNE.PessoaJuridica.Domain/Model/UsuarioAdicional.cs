namespace BNE.PessoaJuridica.Domain.Model
{
    public class UsuarioAdicional
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public PessoaJuridica PessoaJuridica { get; set; }

    }
}
