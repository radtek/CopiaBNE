using System;

namespace BNE.BLL.Custom.Email
{
    public static class EmailSenderFactory
    {
        public static ComponenteEnviadorEmail Create(TipoEnviadorEmail tipo)
        {
            switch (tipo)
            {
                case TipoEnviadorEmail.Fila:
                case TipoEnviadorEmail.FilaComVerificacao:
                    return new EnviadorEmailComVerificacao(new EnviadorEmailPorFila());

                case TipoEnviadorEmail.Smtp:
                case TipoEnviadorEmail.SmtpComVerificacao:
                    return new EnviadorEmailComVerificacao(new EnviadorEmailPorFila()); //return new EnviadorEmailComVerificacao(new EnviadorEmailPorSmtp());

                case TipoEnviadorEmail.FilaSemVerificacao:
                    return new EnviadorEmailPorFila();
                case TipoEnviadorEmail.SmtpSemVerificacao:
                    return new EnviadorEmailComVerificacao(new EnviadorEmailPorFila()); //return new EnviadorEmailPorSmtp();

                default:
                    throw new ArgumentException("Tipo de Enviador de Email inválido", "tipo");
            }
        }
    }

    public enum TipoEnviadorEmail
    {
        FilaComVerificacao,
        FilaSemVerificacao,
        SmtpComVerificacao,
        SmtpSemVerificacao,
        Fila,
        Smtp
    }
}
