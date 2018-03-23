namespace BNE.BLL.Mensagem.Mailsender
{
    public struct MailsenderParameters<T, N>
    {
        public T Substitution { get; set; }
        public N Section { get; set; }
    }
}