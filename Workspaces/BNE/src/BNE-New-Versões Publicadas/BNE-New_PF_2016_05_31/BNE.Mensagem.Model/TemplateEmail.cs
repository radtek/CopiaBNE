namespace BNE.Mensagem.Model
{
    public class TemplateEmail : Template
    {

        public string Assunto { get; set; }
        public virtual TemplateEmail TemplateSistema { get; set; }
    }
}
