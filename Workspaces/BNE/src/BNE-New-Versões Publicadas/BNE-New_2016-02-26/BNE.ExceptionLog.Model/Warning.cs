namespace BNE.ExceptionLog.Model
{
    public class Warning : MessageBase
    {

        public Warning()
        {
            this.TipoMensagem = new TipoMensagem(Tipo.Aviso);
        }

    }
}
