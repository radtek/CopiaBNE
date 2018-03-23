using BNE.BLL.Custom.Email;

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

        internal bool Enviar()
        {
            try
            {
                //Tratativa de cópia de e-mails enviados para o Marcelo a pedido do Tortato e Gustavo Brasil
                if (_curriculo.IdCurriculo == 1747084)
                {
                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(_curriculo, null, _usuarioFilialPerfil, "Cópia email Marcelo -> " + _assunto, _mensagem, _carta, _emailRemetente, "josetortato@bne.com.br");

                    EmailSenderFactory
                        .Create(TipoEnviadorEmail.Fila)
                        .Enviar(_curriculo, null, _usuarioFilialPerfil, "Cópia email Marcelo -> " + _assunto, _mensagem, _carta, _emailRemetente, "gustavobrasil@bne.com.br");
                }

                EmailSenderFactory
                    .Create(TipoEnviadorEmail.Fila)
                    .Enviar(_curriculo, null, _usuarioFilialPerfil, _assunto, _mensagem, _carta, _emailRemetente, _emailDestinatario);

                return true;
            }
            catch (System.Exception ex)
            {
                EL.GerenciadorException.GravarExcecao(ex, "Notificações VIP: Falha no envio de e-mail. CV: " + _curriculo.IdCurriculo + " Erro: " + ex.Message);
                return false;
            }
        }
    }
}
