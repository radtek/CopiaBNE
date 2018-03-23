using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BNE.RedesSociais.EL;
using MSNPSharp;

namespace BNE.RedesSociais
{
    /// <summary>
    /// Integração Windows Live Messenger
    /// </summary>
    public class WindowsLiveMessenger : IRedeSocial, IDisposable
    {
        #region Private
        private Messenger wlmsgr = null;
        private EventHandler authenticationCompleteCallback;
        #endregion

        #region Properties
        #region UserID
        /// <summary>
        /// Id do usuário
        /// </summary>
        public string UserID { get; set; }
        #endregion

        #region Password
        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Password { get; set; }
        #endregion
        #endregion

        #region Ctor
        /// <summary>
        /// Inicia a instância do wrapper para o Windows Live
        /// </summary>
        public WindowsLiveMessenger()
            : this(String.Empty, String.Empty)
        {

        }
        /// <summary>
        /// Inicia a instância do wrapper para o Windows Live
        /// </summary>
        /// <param name="userId">User name</param>
        /// <param name="password">Senha</param>
        public WindowsLiveMessenger(String userId, String password)
        {
            this.UserID = userId;
            this.Password = password;

            // Cria o messenger
            this.wlmsgr = new Messenger();
            // Associa os tratadores de evento
            this.wlmsgr.Nameserver.SignedIn += new EventHandler<EventArgs>(Nameserver_SignedIn);
            //this.wlmsgr.NameserverProcessor.ConnectionEstablished += new EventHandler<EventArgs>(NameserverProcessor_ConnectionEstablished);
            this.wlmsgr.NameserverProcessor.ConnectingException += new EventHandler<ExceptionEventArgs>(NameserverProcessor_ConnectingException);
            this.wlmsgr.NameserverProcessor.ConnectionException += new EventHandler<ExceptionEventArgs>(NameserverProcessor_ConnectionException);
        }
        #endregion

        #region Events
        #region NameserverProcessor_ConnectionException
        protected void NameserverProcessor_ConnectionException(object sender, ExceptionEventArgs e)
        {
            throw new RedeSocialException(e.Exception.Message);
        }
        #endregion

        #region NameserverProcessor_ConnectingException
        protected void NameserverProcessor_ConnectingException(object sender, ExceptionEventArgs e)
        {
            throw new RedeSocialException(e.Exception.Message);
        }
        #endregion

        #region NameserverProcessor_ConnectionEstablished
        //protected void NameserverProcessor_ConnectionEstablished(object sender, EventArgs e)
        //{

        //}
        #endregion

        #region Nameserver_SignedIn
        protected void Nameserver_SignedIn(object sender, EventArgs e)
        {
            if (authenticationCompleteCallback != null)
                authenticationCompleteCallback(this, new EventArgs());
        }
        #endregion
        #endregion

        #region Methods
        #region UpdateStatus
        /// <summary>
        /// Atualiza o status da rede social
        /// </summary>
        /// <param name="message">A mensagem de status</param>
        /// <returns></returns>
        public bool UpdateStatus(string message)
        {
            if (String.IsNullOrEmpty(message))
                throw new ArgumentNullException("message");

            if (!this.wlmsgr.Connected)
                throw new NotConnectedInWindowsLiveException();

            try
            {
                this.wlmsgr.Owner.PersonalMessage.Message = message;
                return true;
            }
            catch (Exception ex)
            {
                throw new RedeSocialException(ex.Message);
            }
        }
        #endregion

        #region InviteContact
        /// <summary>
        /// Envia uma solicitação para adicionar um contato
        /// </summary>
        /// <param name="contactAddress"></param>
        /// <returns></returns>
        public bool InviteContact(String contactAddress)
        {
            if (String.IsNullOrEmpty(contactAddress))
                throw new ArgumentNullException("contactAddress");

            if (!this.wlmsgr.Connected)
                throw new NotConnectedInWindowsLiveException();

            try
            {
                wlmsgr.ContactService.AddNewContact(contactAddress, "Convite para adicionar o BNE no msn.");
                return true;
            }
            catch (Exception ex)
            {
                throw new RedeSocialException(ex.Message);
            }
        }
        #endregion

        #region AuthenticateAsync
        /// <summary>
        /// Autentica no windows live messenger, e dispara o callback quando o processo estiver completo
        /// </summary>
        /// <param name="authenticationComplete">Callback a ser disparado quando os processos de autenticação e login
        /// forem concluídos.
        /// </param>
        public void AuthenticateAsync(EventHandler authenticationCompleteCallback)
        {
            if (String.IsNullOrEmpty(this.UserID))
                throw new RedeSocialException("Login do usuário não definido");

            if (String.IsNullOrEmpty(this.Password))
                throw new RedeSocialException("Senha do usuário não definida");

            this.authenticationCompleteCallback = authenticationCompleteCallback;

            this.wlmsgr.Credentials = new Credentials(this.UserID, this.Password);
            this.wlmsgr.Connect();

        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (this.wlmsgr != null)
            {
                if (this.wlmsgr.Connected)
                    this.wlmsgr.Disconnect();

                this.wlmsgr = null;
            }
        }
        #endregion
        #endregion              
    }
}
