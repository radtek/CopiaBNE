namespace BNE.Mensagem.Model
{
    public class SMS : Mensagem
    {

        public byte DDD { get; set; }
        public decimal Numero { get; set; }

        public virtual TemplateSMS TemplateSMS { get; set; }

    }
}
