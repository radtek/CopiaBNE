namespace BNE.Mensagem.Model
{
    public class Email : Mensagem
    {

        public string EmailRemetente { get; set; }
        public string EmailDestinatario { get; set; }

        public virtual Anexo Anexo { get; set; }
        public virtual TemplateEmail TemplateEmail { get; set; }
    }
}
