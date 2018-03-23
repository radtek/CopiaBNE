namespace BNE.PessoaFisica.Model
{
    public class InstituicaoEnsino
    {
        public int Id { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public bool FlgMEC { get; set; }
        public bool Ativo { get; set; }
    }
}
