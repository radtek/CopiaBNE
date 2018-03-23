namespace BNE.Mensagem.WebAPI.Models
{
    public class EnviarEmail
    {

        public dynamic Parametros { get; set; }
        public dynamic ParametrosAssunto { get; set; }
        public string GuidUsuarioRemetente { get; set; }
        public string GuidUsuarioDestino { get; set; }
        public string EmailRemetente { get; set; }
        public string EmailDestino { get; set; }
        public string Assunto { get; set; }

    }
}