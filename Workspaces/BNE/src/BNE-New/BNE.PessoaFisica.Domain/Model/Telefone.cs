namespace BNE.PessoaFisica.Domain.Model
{
    public class Telefone : Comum.Model.TelefoneComum
    {
        public virtual Model.PessoaFisica PessoaFisica { get; set; }
        public string FalarCom { get; set; }
    }
}