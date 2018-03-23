namespace BNE.Mensagem.Domain.Command
{
    public class EnviarEmail
    {

        public string Sistema { get; set; }
        public string Template { get; set; }
        public dynamic Parametros { get; set; }
        public string GuidUsuarioRemetente { get; set; }
        public string GuidUsuarioDestino { get; set; }
        public string EmailRemetente { get; set; }
        public string EmailDestino { get; set; }
        public string Assunto { get; set; }

    }
}
