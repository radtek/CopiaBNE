namespace BNE.Mensagem.Model
{
    public abstract class Template
    {

        public int Id { get; set; }
        public string Nome { get; set; }
        public short Versao { get; set; }
        public string Conteudo { get; set; }

        public virtual Sistema Sistema { get; set; }

    }
}
