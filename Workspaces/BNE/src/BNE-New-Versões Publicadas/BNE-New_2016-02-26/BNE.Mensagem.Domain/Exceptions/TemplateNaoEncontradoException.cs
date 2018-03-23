using System;

namespace BNE.Mensagem.Domain.Exceptions
{
    public class TemplateNaoEncontradoException : Exception
    {
        public TemplateNaoEncontradoException(string nomeTemplate)
            : base(string.Format("Template {0} não encontrado.", nomeTemplate))
        {
        }
    }
}
