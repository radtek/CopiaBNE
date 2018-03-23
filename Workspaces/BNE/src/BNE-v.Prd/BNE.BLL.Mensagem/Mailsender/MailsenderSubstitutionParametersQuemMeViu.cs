using System.Collections.Generic;

namespace BNE.BLL.Mensagem.Mailsender
{
    public class MailsenderSubstitutionParametersQuemMeViu
    {
        public MailsenderSubstitutionParametersQuemMeViu()
        {
            nome = new List<string>();
            link = new List<string>();
        }

        public IList<string> nome { get; set; }
        public IList<string> link { get; set; }
    }
}