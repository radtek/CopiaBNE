using System;

namespace BNE.EL
{
    public class EmailInvalidoException : System.Exception
    {
        public EmailInvalidoException() : base("Email inválido.")
        {
        }

        public EmailInvalidoException(string mensagem) : base(mensagem)
        {
        }
    }
}
