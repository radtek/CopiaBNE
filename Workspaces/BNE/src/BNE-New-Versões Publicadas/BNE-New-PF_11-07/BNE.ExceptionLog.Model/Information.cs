namespace BNE.ExceptionLog.Model
{
    public class Information : MessageBase
    {

        public Information()
        {
            this.TipoMensagem = new TipoMensagem(Tipo.Informacao);
        }

    }
}
