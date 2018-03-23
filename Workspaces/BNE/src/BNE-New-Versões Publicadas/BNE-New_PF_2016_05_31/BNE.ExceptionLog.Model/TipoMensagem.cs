namespace BNE.ExceptionLog.Model
{
    public class TipoMensagem
    {

        public Tipo Tipo { get; set; }

        public TipoMensagem(Tipo tipo)
        {
            Tipo = tipo;
        }

    }

    public enum Tipo
    {
        Erro,
        Aviso,
        Informacao
    }
}
