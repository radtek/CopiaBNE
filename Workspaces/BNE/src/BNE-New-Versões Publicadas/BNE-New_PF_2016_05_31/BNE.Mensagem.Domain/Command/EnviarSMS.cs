namespace BNE.Mensagem.Domain.Command
{
    public class EnviarSMS
    {

        public string Sistema { get; set; }
        public string Template { get; set; }
        public string Parametros { get; set; }
        public string GuidUsuarioRemetente { get; set; }
        public string GuidUsuarioDestino { get; set; }
        public string NumeroCelular { get; set; }

    }
}
