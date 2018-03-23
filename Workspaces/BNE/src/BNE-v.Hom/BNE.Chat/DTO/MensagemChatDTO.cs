using System;

namespace BNE.Chat.DTO
{
    public class MensagemChatDTO
    {
        public MensagemChatDTO()
        {
            GuidMessage = Guid.NewGuid().ToString();
            var now = DateTime.Now;
            CriacaoData = now.AddMilliseconds(now.Millisecond * -1);
            StatusData = now.AddMilliseconds(now.Millisecond * -1); // utilizado para arredondar os mili
        }
        public string GuidMessage { get; set; }

        public string GuidChat { get; set; }

        public string Mensagem { get; set; }

        public string NumeroCelular { get; set; }

        public int UsuarioFilialPerfilId { get; set; }

        public int CurriculoId { get; set; }

        public bool EscritoPorUsuarioFilialPerfil { get; set; }

        public DateTime CriacaoData { get; set; }
        public DateTime StatusData { get; set; }

        public int TipoStatus { get; set; }

        public string Nome { get; set; }
    }
}