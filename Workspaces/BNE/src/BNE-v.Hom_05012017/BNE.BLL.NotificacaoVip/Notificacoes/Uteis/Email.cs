using BNE.BLL.Custom.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.NotificacoesVip.Notificacoes.Uteis
{
    internal class Email
    {
        public Curriculo _curriculo { get; set; }
        public UsuarioFilialPerfil _usuarioFilialPerfil { get; set; }

        public BLL.Enumeradores.CartaEmail? _carta {get; set;}
        public string _assunto { get; set; }
        public string _mensagem { get; set; }
        public string _emailRemetente { get; set; }
        public string _emailDestinatario { get; set; }

        public Email(Curriculo objCurriculo, UsuarioFilialPerfil objUsuarioFilialPerfil, string assunto, string mensagem, string emailRemetente, string emailDestinatario, BLL.Enumeradores.CartaEmail? carta = null)
        {
            _curriculo = objCurriculo;
            _usuarioFilialPerfil = objUsuarioFilialPerfil;
            _carta = carta;
            _assunto= assunto;
            _mensagem = mensagem;
            _emailRemetente = emailRemetente;
            _emailDestinatario = emailDestinatario;
        }

        internal void Enviar()
        {
            EmailSenderFactory
                .Create(TipoEnviadorEmail.Fila)
                .Enviar(_curriculo, null, _usuarioFilialPerfil, _assunto, _mensagem, _carta, _emailRemetente, _emailDestinatario);
        }
    }
}
