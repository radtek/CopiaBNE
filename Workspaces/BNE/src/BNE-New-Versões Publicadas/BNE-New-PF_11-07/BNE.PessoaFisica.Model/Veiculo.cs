namespace BNE.PessoaFisica.Model
{
    public class Veiculo
    {
        public int Id { get; set; }
        public string Modelo { get; set; }
        public short Ano { get; set; }

        public virtual TipoVeiculo TipoVeiculo { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
    }
}