namespace BNE.PessoaFisica.Model
{
    public class Telefone : Comum.Model.TelefoneComum
    {
        public virtual PessoaFisica PessoaFisica { get; set; }
        public string FalarCom { get; set; }
    }
}