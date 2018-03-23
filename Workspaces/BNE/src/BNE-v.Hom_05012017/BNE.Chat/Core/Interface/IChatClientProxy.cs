namespace BNE.Chat.Core.Interface
{
    public interface IChatClientProxy
    {
        void SendMessage(object mensagemArgs);
        void SendCloseChat(object fimDoChatArgs);
        void SendOnlineContacts(object contatosArgs);
        void SendStatusOfMessage(object mensagemArgs);
        void SendCloseConnection();
        void SendOpossiteTypingSignal();
        void SendDeleteContact(int targetId);
    }
}