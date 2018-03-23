using System;

namespace BNE.BLL.Enumeradores.CustomAttribute
{
    /// <summary>
    /// Chaves de template do Mailsender. Cartas com esse atributo foram migradas com template para o Mailsender (http://mailsender.bne.com.br)
    /// </summary>
    public class MailsenderAttribute : Attribute
    {
        public Guid Id;
        public MailsenderAttribute(string guid)
        {
            Id = Guid.Parse(guid);
        }
    }
}