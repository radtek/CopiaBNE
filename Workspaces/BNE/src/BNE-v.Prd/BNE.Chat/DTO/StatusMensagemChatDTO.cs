using System;

namespace BNE.Chat.DTO
{
    public class StatusMensagemChatDTO
    {
        public StatusMensagemChatDTO()
        {
            var now = DateTime.Now;
            StatusData = now.AddMilliseconds(now.Millisecond * -1); // utilizado para arredondar os mili
        }

        public string GuidMensagem { get; set; }
        public DateTime StatusData { get; set; }
        public int TipoStatus { get; set; }
        public int UsuarioFilialPerfilId { get; set; }
        public string GuidChat { get; set; }
        public int CurriculoId { get; set; }
    }
}