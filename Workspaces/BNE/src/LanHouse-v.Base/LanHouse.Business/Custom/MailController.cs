using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.Custom
{
    public enum SaidaSMTP
    {
        SendGrid = 0,
        Mandrill = 1
    }

    public static class MailController
    {

        #region Initialize
        private static void Initialize(SaidaSMTP saidaSMTP = SaidaSMTP.Mandrill)
        {
            Enumeradores.Parametro SMTPAccount;
            Enumeradores.Parametro SMTPPassword;
            Enumeradores.Parametro SMTPServer;
            Enumeradores.Parametro SMTPPort;

            switch (saidaSMTP)
            {
                case SaidaSMTP.SendGrid:
                    SMTPAccount = Enumeradores.Parametro.SMTPAccountSendGrid;
                    SMTPPassword = Enumeradores.Parametro.SMTPPasswordSendGrid;
                    SMTPServer = Enumeradores.Parametro.SMTPServerSendGrid;
                    SMTPPort = Enumeradores.Parametro.SMTPPortSendGrid;
                    break;
                default:
                    SMTPAccount = Enumeradores.Parametro.SMTPAccount;
                    SMTPPassword = Enumeradores.Parametro.SMTPPassword;
                    SMTPServer = Enumeradores.Parametro.SMTPServer;
                    SMTPPort = Enumeradores.Parametro.SMTPPort;
                    break;
            }

            List<Int32> parametros = new List<Int32>{
                Convert.ToInt32(SMTPAccount),
                Convert.ToInt32(SMTPPassword),
                Convert.ToInt32(SMTPServer),
                Convert.ToInt32(SMTPPort)
            };

            var dicionarioParametro = Parametro.ListarParametros(parametros);

            Message.Config = new MailConfiguration
            {
                EMailAccount = dicionarioParametro[SMTPAccount],
                EMailPassword = dicionarioParametro[SMTPPassword],
                SMTPServer = dicionarioParametro[SMTPServer],
                SMTPPort = Convert.ToInt32(dicionarioParametro[SMTPPort])
            };
        }
        #endregion Initialize

        #region Send
        public static bool Send(string to, string from, string subject, string message, SaidaSMTP saidaSMTP = SaidaSMTP.Mandrill)
        {
            return Send(to, from, subject, message, new Dictionary<string, byte[]>(), saidaSMTP);
        }
        public static bool Send(string to, string from, string subject, string message, Dictionary<string, byte[]> attachments, SaidaSMTP saidaSMTP = SaidaSMTP.Mandrill)
        {
            int teste = Convert.ToInt32(Enumeradores.Parametro.ContaPadraoEnvioEmail);

            var contaPadrao = new Business.Parametro().GetById(teste).Vlr_Parametro;

            //Se o remetente for vazio, pega a conta default para p envio de e-mails.
            from = from.Trim();
            if (String.IsNullOrEmpty(from))
            {
                from = contaPadrao;
            }

            //Tratando Subject para evitar assuntos inválidos (com quebra de linhas).
            subject = subject.Replace('\r', ' ').Replace('\n', ' ');

            //tratando destinatários
            to = to.Replace(" ", "").Replace(";", "; ");

            Initialize(saidaSMTP);

            var email = new Message
            {
                MailMessage = new MailMessage(@from, to, subject, message)
                {
                    ReplyTo = new MailAddress(@from), //Ajuste para o BNE. Manda o remetente no reply to. E usamos uma conta do BNE como sender do e-mail
                    From = new MailAddress(contaPadrao, @from),
                    IsBodyHtml = true
                }
            };

            //Anexando arquivo
            MemoryStream stream = null;
            try
            {
                if (!attachments.Equals(default(Dictionary<string, byte[]>)))
                {
                    foreach (var attachment in attachments)
                    {
                        stream = new MemoryStream(attachment.Value);
                        email.MailMessage.Attachments.Add(new Attachment(stream, attachment.Key));
                    }
                }

                //Trantando emails com cópia separados por ';'
                if (to.IndexOf(';') >= 0)
                {
                    foreach (string ccTo in to.Split(new char[] { ';' }))
                    {
                        if (ccTo.Trim().Length > 0)
                        {
                            email.ToRecipients.Add(ccTo);
                        }
                    }
                }
                else
                {
                    email.ToRecipients.Add(to);
                }

                email.SimpleSend();
                return true;
            }
            catch (Exception ex)
            {
                string mensagemErro = string.Format("From: {0} - To {1}", from, to);
                //Business.EL.GerenciadorException.GravarExcecao(ex, mensagemErro);
                return false;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }
        #endregion

        #region Message
        /// <summary>
        /// <para>Classe que controla o envio de e-mail</para>
        /// </summary>
        internal class Message
        {
            #region Eventos

            #region Delegates

            /// <summary>
            /// Evento associado ao envio de uma mensagem
            /// </summary>
            /// <param name="sender">Mensagem a qual foi ordenado o envio.</param>
            /// <param name="e">Status de retorno sobre o envio.</param>
            public delegate void EmailEvent(Message sender, SmtpStatusCode e);

            #endregion

            //Eventos para Sucesso e Falha
            public event EmailEvent OnError;
            public event EmailEvent OnSucess;

            #endregion

            #region Properties

            /// <summary>
            /// <para>Configuração que informa como o e-mail deverá ser enviado.</para>
            /// <para>Aconselha-se o uso do método estático LoadFromXMLConfig para seu carregamento ao invés de um construtor</para>
            /// </summary>
            public static MailConfiguration Config { get; set; }

            /// <summary>
            /// Coleção de Destinatários que irão receber o e-mail
            /// </summary>
            public MailAddressCollection ToRecipients { get; set; }

            /// <summary>
            /// Coleção de Destinatários que irão receber o e-mail em CC (Cópia Carbonada)
            /// </summary>
            public MailAddressCollection CCRecipients { get; set; }

            /// <summary>
            /// Coleção de Destinatários que irão receber o e-mail em CCO (Cópia Carbonada Oculta)
            /// </summary>
            public MailAddressCollection CCORecipients { get; set; }

            /// <summary>
            /// <para>Mensagem de e-mail a ser enviada.</para>
            /// <para>Aconselha-se apenas acessar esta propriedade para configurações avançadas (Atachments, BodyEncode, etc)</para>
            /// </summary>
            public MailMessage MailMessage { get; set; }

            /// <summary>
            /// <para>Prioridade da mensagem de e-mail a ser enviada.</para>
            /// <para>Por padrão o valor desta propriedade é NORMAL</para>
            /// </summary>
            private MailPriority Priority { get; set; }

            /// <summary>
            /// Propriedade que permite carregar o SMTP Client com base no Config
            /// </summary>
            private static SmtpClient SmtpClient
            {
                get
                {
                    var objSmtp = new SmtpClient(Config.SMTPServer, Config.SMTPPort)
                    {
                        Credentials = new NetworkCredential(Config.EMailAccount,
                                                            (Config.CryptographyEnable)
                                                                ? Config.DecryptPassword()
                                                                : Config.EMailPassword),
                        EnableSsl = Config.EnableSSL
                    };

                    return objSmtp;
                }
            }

            #endregion

            #region Constructors

            #region Static Constructor

            /// <summary>
            /// <para>Construtor Estático</para>
            /// <para>Define a propriedade Config de acordo com o construtor padrão da classe EmailConfiguration</para>
            /// </summary>
            static Message()
            {
                if (Config == null)
                    Config = new MailConfiguration();
            }

            #endregion

            #region Default Constructor

            /// <summary>
            /// <para>Construtor Padrão</para>
            /// <para>Cria a mensagem vazia e lista de destinatários vazia.</para>
            /// </summary>
            public Message()
            {
                ToRecipients = new MailAddressCollection();
                CCRecipients = new MailAddressCollection();
                CCORecipients = new MailAddressCollection();
                MailMessage = null;
            }

            #endregion

            #region Simple Constructor

            /// <summary>
            /// <para>Construtor Simples</para>
            /// <para>Constroi a mensagem com titulo e corpo mas mantém a lista de destinatários vazia</para>
            /// </summary>
            /// <param name="messageSubject">Título do E-mail</param>
            /// <param name="messageBody">Corpo do E-mail (suporta HTML)</param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public Message(string messageSubject, string messageBody)
            {
                //Inicialização da lista de Destinatários
                InitializeRecipients();

                //Criação da mensagem padrão
                MailMessage = DefaultMessage(messageSubject, messageBody);
            }

            #endregion

            #region Advanced Constructor to Single Recipient

            /// <summary>
            /// Cria a mensagem de e-mail já com título, corpo e um destinatário
            /// </summary>
            /// <param name="messageSubject">Título do e-mail</param>
            /// <param name="messageBody">Corpo do e-mail (suporta HTML)</param>
            /// <param name="recipientName">Nome do destinatário</param>
            /// <param name="recipientAddress">Endereço de e-mail do destinatário</param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public Message(string messageSubject, string messageBody, string recipientName, string recipientAddress)
            {
                //Inicialização da lista de Destinatários
                InitializeRecipients();

                //Criação da mensagem padrão
                MailMessage = DefaultMessage(messageSubject, messageBody);

                ToRecipients.Add(new MailAddress(recipientAddress, recipientName));
            }

            #endregion

            #region Advanced Constructor to Multiple Recipients

            /// <summary>
            /// Cria a mensagem de e-mail já com título, corpo e uma lista de destinatários
            /// </summary>
            /// <param name="messageSubject">Título do e-mail</param>
            /// <param name="messageBody">Corpo do e-mail (suporta HTML)</param>
            /// <param name="toRecipientsList">Lista de destinatários (To)</param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public Message(string messageSubject, string messageBody, IEnumerable<MailAddress> toRecipientsList)
            {
                //Inicialização da lista de Destinatários
                InitializeRecipients();

                //Criação da mensagem padrão
                MailMessage = DefaultMessage(messageSubject, messageBody);

                foreach (MailAddress mailAdress in toRecipientsList)
                {
                    ToRecipients.Add(mailAdress);
                }
            }

            #endregion

            #region Advanced Constructor to Multiple Recipients

            /// <summary>
            /// Cria a mensagem de e-mail já com título, corpo e uma lista de destinatários
            /// </summary>
            /// <param name="messageSubject">Título do e-mail</param>
            /// <param name="messageBody">Corpo do e-mail (suporta HTML)</param>
            /// <param name="toRecipientsList">Lista de destinatários (To)</param>
            /// <param name="ccRecipientsList">Lista de destinatários (CC)</param>
            /// <param name="ccoRecipientsList">Lista de destinatários (CCO)</param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public Message(string messageSubject, string messageBody, IEnumerable<MailAddress> toRecipientsList,
                                IEnumerable<MailAddress> ccRecipientsList, IEnumerable<MailAddress> ccoRecipientsList)
            {
                //Inicialização da lista de Destinatários
                InitializeRecipients();

                //Criação da mensagem padrão
                MailMessage = DefaultMessage(messageSubject, messageBody);

                //Carregando Lista TO
                foreach (MailAddress mailAdress in toRecipientsList)
                {
                    ToRecipients.Add(mailAdress);
                }

                //Carregando Lista CC
                foreach (MailAddress mailAdress in ccRecipientsList)
                {
                    CCRecipients.Add(mailAdress);
                }

                //Carregango Lista CCO
                foreach (MailAddress mailAdress in ccoRecipientsList)
                {
                    CCORecipients.Add(mailAdress);
                }
            }

            #endregion

            #endregion

            #region Methods

            #region InitializeRecipients

            /// <summary>
            /// Inicializa as listas de destinatários (To, CC, CCO)
            /// </summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public void InitializeRecipients()
            {
                ToRecipients = new MailAddressCollection();
                CCRecipients = new MailAddressCollection();
                CCORecipients = new MailAddressCollection();
            }

            #endregion

            #region DefaultMessage

            /// <summary>
            /// Método que retorna uma mensagem com as propriedades básicas já definidas
            /// </summary>
            /// <param name="messageSubject">Título do e-mail</param>
            /// <param name="messageBody">Corpo do e-mail (suporta HTML)</param>
            /// <returns>Mensagem de e-mail com propriedades básicas já definidas</returns>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            private static MailMessage DefaultMessage(string messageSubject, string messageBody)
            {
                return new MailMessage
                {
                    Subject = messageSubject,
                    Body = messageBody,
                    SubjectEncoding = Encoding.UTF8,
                    BodyEncoding = Encoding.GetEncoding("ISO-8859-1"),
                    IsBodyHtml = true,
                    Priority = MailPriority.Normal
                };
            }

            #endregion

            #region LoadRecipients

            /// <summary>
            /// Carrega os destinatários a partir da lista de destinatários
            /// </summary>
            private void LoadRecipients()
            {
                //Carregar Lista TO para a Mensagem
                MailMessage.To.Clear();
                foreach (MailAddress mailAdress in ToRecipients)
                {
                    MailMessage.To.Add(mailAdress);
                }

                MailMessage.CC.Clear();
                //Carregar Lista CC para a Mensagem
                foreach (MailAddress mailAdress in CCRecipients)
                {
                    MailMessage.CC.Add(mailAdress);
                }

                MailMessage.Bcc.Clear();
                //Carregar Lista CCO para a Mensagem
                foreach (MailAddress mailAdress in CCORecipients)
                {
                    MailMessage.Bcc.Add(mailAdress);
                }
            }

            #endregion

            #region Send Mail

            /// <summary>
            ///     <para>Envia o email de forma sincrona.</para>
            ///     <para>Desta forma o sistema terá que aguardar o término do envio antes de executar o próximo comando.</para>
            /// </summary>
            /// <returns>
            ///     <para>TRUE  - E-mail Enviado com Sucesso</para>
            ///     <para>FALSE - E-maril Não pode ser Enviado</para>
            /// </returns>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public bool SendMail()
            {
                //Se  não houver mensagem ou destinatário cancelar o envio
                if (MailMessage == null || ToRecipients.Count == 0)
                    return false;

                //Carrega todos os destinatários
                LoadRecipients();

                try
                {
                    //Enviar a Mensagem
                    SmtpClient.Send(MailMessage);

                    //Caso tudo ocorra bem disparar o evento se ele estiver registrado
                    if (OnSucess != null)
                        OnSucess(this, SmtpStatusCode.Ok);

                    return true;
                }
                catch (SmtpException ex)
                {
                    //Caso ocorra um erro disparar o evento se ele estiver registrado
                    if (OnError != null)
                        OnError(this, ex.StatusCode);

                    return false;
                }
            }

            /// <summary>
            ///     <para>Envia o email de forma sincrona.</para>
            ///     <para>Desta forma o sistema terá que aguardar o término do envio antes de executar o próximo comando.</para>
            /// </summary>
            /// <returns>
            ///     <para>TRUE  - E-mail Enviado com Sucesso</para>
            ///     <para>FALSE - E-maril Não pode ser Enviado</para>
            /// </returns>
            public void SimpleSend()
            {
                //Se  não houver mensagem ou destinatário cancelar o envio
                if (MailMessage == null || ToRecipients.Count == 0)
                    throw new Exception("Mensagem ou destino não informado.");

                //Carrega todos os destinatários
                LoadRecipients();

                SmtpClient.Timeout = 600000;

                //Enviar a Mensagem
                SmtpClient.Send(MailMessage);
            }

            #endregion

            #region Async Send Mail

            /// <summary>
            /// <para>Envia o e-mail de forma assincrona.</para> 
            /// <para>Desta forma o sistema não aguarda o término do envio para executar o próximo comando.</para>
            /// <para> Dispara: OnSucess</para>
            /// <para> --> Quando o e-mail é enviado com sucesso.</para>
            /// <para> Dispara: OnSucess</para>
            /// <para> --> Quando o e-mail é enviado com sucesso.</para>
            /// </summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public void AsyncSendMail()
            {
                //Se  não houver mensagem ou destinatário cancelar o envio 
                if (MailMessage == null || ToRecipients.Count == 0)
                    return;

                //Carrega todos os destinatários
                LoadRecipients();

                //Registra o evento (occore tanto em envio OK ou com ERRO)
                SmtpClient.SendCompleted += SmtpClientSendCompleted;
                //Envia de forma assincrona o email
                SmtpClient.SendAsync(MailMessage, MailMessage.Subject);
            }

            #endregion

            #region Send Completed

            /// <summary>
            /// Evento disparado ao término do envio do e-mail (tanto em caso de sucesso com em caso de falha)
            /// </summary>
            /// <param name="sender">Mensagem enviada</param>
            /// <param name="e">Argumentos do Evento</param>
            private void SmtpClientSendCompleted(object sender, AsyncCompletedEventArgs e)
            {
                if (e.Error == null)
                {
                    if (OnSucess != null)
                        OnSucess(this, SmtpStatusCode.Ok);
                }
                else
                {
                    if (OnError != null)
                        OnError(this, SmtpStatusCode.GeneralFailure);
                }
            }

            #endregion

            #endregion
        }
        #endregion

        #region MailConfiguration
        /// <summary>
        /// <para>Classe de configuração para envio de e-mail</para>
        /// <para>Aconselha-se o uso do método estático LoadFromXMLConfig para seu carregamento ao invés de um construtor</para>
        /// </summary>
        public class MailConfiguration
        {
            #region Constructors

            #region Default Constructor

            /// <summary>
            /// Construtor Default.
            /// Utiliza por padrão as configurações:
            /// SMTP  : vm-atenas.techresult.com.br
            /// CONTA : techresult_app@techresult.com.br
            /// SENHA : t3ch@app
            /// </summary>
            public MailConfiguration()
                : this(
                    "vm-atenas.techresult.com.br",
                    "techresult_app@techresult.com.br",
                    "t3ch@pp")
            {
                SMTPPort = 25;
                EnableSSL = false;
                CryptographyEnable = false;
            }

            #endregion

            #region Base Constructor

            /// <summary>
            /// Construtor com sobrecarga simples.
            /// </summary>
            /// <param name="smtpServer">Servidor de SMTP</param>
            /// <param name="eMailAccount">Email do Usuário SMTP</param>
            /// <param name="eMailPassword">Senha do Usuário SMTP</param>
            public MailConfiguration(String smtpServer, String eMailAccount, String eMailPassword)
            {
                SMTPServer = smtpServer;
                EMailAccount = eMailAccount;
                EMailPassword = eMailPassword;
            }

            #endregion

            #region Advanced Constructor

            /// <summary>
            /// Construtor com sobrecarga avançada.
            /// </summary>
            /// <param name="smtpServer">Servidor de SMTP</param>
            /// <param name="smtpPort">Porta Servidor SMTP</param>
            /// <param name="eMailAccount">Email do Usuário SMTP</param>
            /// <param name="eMailPassword">Senha do Usuário SMTP</param>
            /// <param name="enableSSL">Usar conexão Segura SSL</param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public MailConfiguration(String smtpServer, int smtpPort, String eMailAccount, String eMailPassword,
                                      Boolean enableSSL)
            {
                SMTPServer = smtpServer;
                SMTPPort = smtpPort;
                EMailAccount = eMailAccount;
                EMailPassword = eMailPassword;
                EnableSSL = enableSSL;
            }

            #endregion

            #region Base Constructor With Cryptography

            /// <summary>
            /// Construtor com sobrecarga simples e criptografia.
            /// </summary>
            /// <param name="smtpServer">Servidor de SMTP</param>
            /// <param name="eMailAccount">Email do Usuário SMTP</param>
            /// <param name="eMailPassword">Senha do Usuário SMTP Criptografada</param>
            /// <param name="cryptographyKey">Chave de Descriptografia</param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public MailConfiguration(String smtpServer, String eMailAccount, String eMailPassword,
                                      String cryptographyKey)
            {
                SMTPServer = smtpServer;
                EMailAccount = eMailAccount;
                EMailPassword = eMailPassword;

                if (String.IsNullOrEmpty(cryptographyKey))
                    return;

                CryptographyKey = cryptographyKey;
                CryptographyEnable = true;
            }

            #endregion

            #region Advanced Constructor With Cryptography

            /// <summary>
            /// Construtor com sobrecarga avançada e criptografia.
            /// </summary>
            /// <param name="smtpServer">Servidor de SMTP</param>
            /// <param name="smtpPort">Porta Servidor SMTP</param>
            /// <param name="eMailAccount">Email do Usuário SMTP</param>
            /// <param name="eMailPassword">Senha do Usuário SMTP Criptografada</param>
            /// <param name="enableSSL">Usar conexão Segura SSL</param>
            /// <param name="cryptographyKey">Chave de Descriptografia</param>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode:Não será alterado o conteudo do MailControler")]
            public MailConfiguration(String smtpServer, int smtpPort, String eMailAccount, String eMailPassword,
                                      Boolean enableSSL, String cryptographyKey)
            {
                SMTPServer = smtpServer;
                SMTPPort = smtpPort;
                EMailAccount = eMailAccount;
                EMailPassword = eMailPassword;
                EnableSSL = enableSSL;

                if (String.IsNullOrEmpty(cryptographyKey))
                    return;

                CryptographyKey = cryptographyKey;
                CryptographyEnable = true;
            }

            #endregion

            #endregion

            #region Properties

            /// <summary>
            /// Servidor de SMTP.
            /// </summary>
            public String SMTPServer { get; set; }

            /// <summary>
            /// Porta Servidor SMTP.
            /// </summary>
            public int SMTPPort { get; set; }

            /// <summary>
            /// Email do Usuário SMTP.
            /// </summary>
            public String EMailAccount { get; set; }

            /// <summary>
            /// Senha do Usuário SMTP.
            /// </summary>
            public String EMailPassword { get; set; }

            /// <summary>
            /// Chave de Descriptografia.
            /// </summary>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
            public String CryptographyKey { get; set; }

            /// <summary>
            /// Usar conexão Segura SSL.
            /// </summary>
            public Boolean EnableSSL { get; set; }

            /// <summary>
            /// Usar senha criptografada.
            /// </summary>
            public Boolean CryptographyEnable { get; set; }

            #endregion

            #region Methods

            #region DecryptPassword

            /// <summary>
            /// Descriptografa o Password
            /// </summary>
            /// <returns>Password Descriptografado</returns>
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
            internal string DecryptPassword()
            {
                byte[] encryptedData = Convert.FromBase64String(EMailPassword);
                byte[] pwdBytes = Encoding.UTF8.GetBytes(CryptographyKey);
                var keyBytes = new byte[16];

                int len = pwdBytes.Length;
                if (len > keyBytes.Length)
                    len = keyBytes.Length;

                Array.Copy(pwdBytes, keyBytes, len);

                var objRijndaelManaged = new RijndaelManaged
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 128,
                    BlockSize = 128,
                    Key = keyBytes,
                    IV = keyBytes
                };

                ICryptoTransform objCrypytoTransform = objRijndaelManaged.CreateDecryptor();
                byte[] decodedPassword = objCrypytoTransform.TransformFinalBlock(encryptedData, 0, encryptedData.Length);

                return Encoding.UTF8.GetString(decodedPassword);
            }

            #endregion

            #endregion
        }
        #endregion

    }
}
