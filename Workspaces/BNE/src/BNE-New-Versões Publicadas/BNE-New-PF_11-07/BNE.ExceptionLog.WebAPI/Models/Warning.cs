namespace BNE.ExceptionLog.WebAPI.Models
{
    public class Warning
    {

        public string Aplicacao { get; set; }
        public string Usuario { get; set; }

        public string Message { get; set; }
        public string Payload { get; set; }

    }
}