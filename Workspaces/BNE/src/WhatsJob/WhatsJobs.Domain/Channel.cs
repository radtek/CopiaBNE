using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using WhatsAppApi;
using WhatsAppApi.Account;
using WhatsAppApi.Helper;
using WhatsAppApi.Response;
using System.Net;

namespace WhatsJob.Domain
{
    public class Channel
    {
        static BNE.ExceptionLog.DatabaseLogger _logger = new BNE.ExceptionLog.DatabaseLogger();
        private DateTime lastSent = DateTime.MinValue;
        private Random r = new Random();

        WhatsApp _wa;
        Model.Channel _ch;

        /// <summary>
        /// Status of WhatsApp Connection
        /// </summary>
        public ApiBase.CONNECTION_STATUS Status
        {
            get { return wa.ConnectionStatus; }
        }
        
        private WhatsApp wa
        {
            get
            {
                if (_wa == null)
                    _wa = new WhatsApp(_ch.Number, _ch.Password, _ch.NickName);
                return _wa;
            }
        }

        public Channel(Model.Channel ch)
        {
            _ch = ch;
            Configure();
        }

        private void Configure()
        {
            wa.OnConnectFailed += wa_OnConnectFailed;
            wa.OnLoginSuccess += wa_OnLoginSuccess;
            wa.OnLoginFailed += wa_OnLoginFailed;
            wa.OnGetMessage += wa_OnGetMessage;
            wa.OnGetMessageReceivedClient += wa_OnGetMessageReceivedClient;
            wa.OnGetMessageReceivedServer += wa_OnGetMessageReceivedServer;
            wa.OnGetMessageReadClient += wa_OnGetMessageReadClient;
            wa.OnNotificationPicture += wa_OnNotificationPicture;
            wa.OnGetPresence += wa_OnGetPresence;
            wa.OnGetGroupParticipants += wa_OnGetGroupParticipants;
            wa.OnGetLastSeen += wa_OnGetLastSeen;
            wa.OnGetTyping += wa_OnGetTyping;
            wa.OnGetPaused += wa_OnGetPaused;
            wa.OnGetMessageImage += wa_OnGetMessageImage;
            wa.OnGetMessageAudio += wa_OnGetMessageAudio;
            wa.OnGetMessageVideo += wa_OnGetMessageVideo;
            wa.OnGetMessageLocation += wa_OnGetMessageLocation;
            wa.OnGetMessageVcard += wa_OnGetMessageVcard;
            wa.OnGetPhoto += wa_OnGetPhoto;
            wa.OnGetPhotoPreview += wa_OnGetPhotoPreview;
            wa.OnGetGroups += wa_OnGetGroups;
            wa.OnGetSyncResult += wa_OnGetSyncResult;
            wa.OnGetStatus += wa_OnGetStatus;
            wa.OnGetPrivacySettings += wa_OnGetPrivacySettings;
            DebugAdapter.Instance.OnPrintDebug += Instance_OnPrintDebug;
        }

        /// <summary>
        /// Save the next challange to be used in the next loggin
        /// </summary>
        /// <param name="nextChallenge">Challange returned in the LoginSucess event</param>
        private void SaveNextChalenge(string nextChallenge)
        {
            try
            {
                _ch.NextChallenge = nextChallenge;

                using (var db = new Data.WhatsJobsContext())
                {
                    db.Channel.Attach(_ch);
                    var entry = db.Entry(_ch);
                    entry.Property(e => e.NextChallenge).IsModified = true;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Falha ao salvar NextChallang: " + nextChallenge);
            }
        }

        public void SendQueuedMessages()
        {
            using (Data.WhatsJobsContext _ctx = new Data.WhatsJobsContext())
            {
                var messages = from m in _ctx.Message.Include("WhatsChannel").Include("Contact")
                               where !m.Received && !m.SentToServer && m.WhatsChannel.Number == this._ch.Number
                               select m;

                foreach (var m in messages)
                {
                    if (!String.IsNullOrEmpty(this.SendTextMessage(m)))
                        m.SentToServer = true;
                    Thread.Sleep(r.Next(2500, 5000));
                }
                _ctx.SaveChanges();
            }
        }

        public void Login()
        {
            if (wa.ConnectionStatus == ApiBase.CONNECTION_STATUS.DISCONNECTED)
                wa.Connect();

            byte[] nextChallenge = null;
            if (!String.IsNullOrEmpty(_ch.NextChallenge))
            {
                try
                {
                    nextChallenge = Convert.FromBase64String(_ch.NextChallenge);
                }
                catch (Exception) { };
            }
            wa.Login(nextChallenge);
        }

        public void Disconnect()
        {
            if (wa.ConnectionStatus != ApiBase.CONNECTION_STATUS.DISCONNECTED)
                wa.Disconnect();
            _wa = null;
        }

        public void PollMessages()
        {
            if (wa.ConnectionStatus != ApiBase.CONNECTION_STATUS.LOGGEDIN)
                wa.Login();

            wa.PollMessages(true, 5);
        }

        public string SendTextMessage(Model.Contact contact, string message)
        {
            string retorno = null;
            try
            {
                Model.Message objMessage = new Model.Message(Guid.NewGuid().ToString(), _ch, contact, message, false);

                retorno = SendTextMessage(objMessage);

                Domain.Message.Insert(objMessage);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, String.Format("Error sending message \"{0}\" to {1}", message, contact.Number));
            }

            return retorno;
        }

        public string SendTextMessage(Model.Message message)
        {
            string retorno = null;
            try
            {
                if (wa.ConnectionStatus != ApiBase.CONNECTION_STATUS.LOGGEDIN)
                    Login();

                //Waiting time between sent messages
                if ((DateTime.Now - lastSent).Milliseconds < Settings.Default.TimeMS_BetweenMessages)
                    Thread.Sleep(r.Next(0, (DateTime.Now - lastSent).Milliseconds));

                WhatsUserManager usrMan = new WhatsUserManager();
                var tmpUser = usrMan.CreateUser(message.Contact.Number, message.Contact.NickName);

                #region Emulating user input
                wa.SendComposing(tmpUser.GetFullJid());

                int timeToSleep = 0;
                for (int i = 0; i < message.TextMessage.Length; i++)
                    timeToSleep += r.Next(Settings.Default.PressButtonTimeMS_Min, Settings.Default.PressButtonTimeMS_Max);
                Thread.Sleep(timeToSleep);

                wa.SendPaused(tmpUser.GetFullJid());
                #endregion Emulating user input

                retorno = message.WhatsId = wa.SendMessage(tmpUser.GetFullJid(), message.TextMessage);
                message.SentToServer = true;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, String.Format("Error sending message \"{0}\" to {1}", message, message.Contact.Number));
            }

            return retorno;
        }

        private static void OnGetMedia(string file, string from, string url, byte[] data)
        {
            string path = Path.Combine(Settings.Default.MediaFolderPath, Util.GetNumberOfFrom(from));
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            path = Path.Combine(path, file);

            //save preview
            if (data.Length > 0)
            {
                string previewPath = Path.Combine(Util.GetNumberOfFrom(from), "preview_" + file);
                File.WriteAllBytes(previewPath, data);
            }
            //download
            if (!string.IsNullOrEmpty(url))
            {
                using (WebClient wc = new WebClient())
                {
                    wc.DownloadFileAsync(new Uri(url), path, null);
                }
            }
        }

        #region Events
        void wa_OnConnectFailed(Exception ex)
        {
            _logger.Error(ex, String.Format("Conection failed: {0}", _ch.Number));

            Model.ChannelLog log = new Model.ChannelLog(_ch, Model.FaultType.ConnectFailed, ex.Message); ;

            Domain.ChannelLog.Save(log);
        }

        private void wa_OnLoginSuccess(string phoneNumber, byte[] data)
        {
            string nextChallenge = Convert.ToBase64String(data);
            try
            {
                
                if (!Directory.Exists(Settings.Default.PasswordFolderPath))
                    Directory.CreateDirectory(Settings.Default.PasswordFolderPath);
                File.WriteAllText(Path.Combine(Settings.Default.PasswordFolderPath, phoneNumber+".next"), nextChallenge);

                SaveNextChalenge(nextChallenge);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, String.Format("PhoneNumber: {0} NextChallenge: {1}", phoneNumber, nextChallenge));
            }
        }

        private void Instance_OnPrintDebug(object value)
        {
            try
            {
                Model.ChannelLog log = new Model.ChannelLog(_ch, Model.FaultType.Debug, value.ToString());
                using (var db = new Data.WhatsJobsContext())
                {
                    db.ChannelLog.Add(log);
                    db.Entry(log.Channel).State = System.Data.Entity.EntityState.Unchanged;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        static void wa_OnGetPrivacySettings(Dictionary<ApiBase.VisibilityCategory, ApiBase.VisibilitySetting> settings)
        {
            //throw new NotImplementedException();
        }

        static void wa_OnGetStatus(string from, string type, string name, string status)
        {
            //Console.WriteLine(String.Format("Got status from {0}: {1}", from, status));
        }

        static void wa_OnGetSyncResult(int index, string sid, Dictionary<string, string> existingUsers, string[] failedNumbers)
        {
            //Console.WriteLine("Sync result for {0}:", sid);
            //foreach (KeyValuePair<string, string> item in existingUsers)
            //{
            //    Console.WriteLine("Existing: {0} (username {1})", item.Key, item.Value);
            //}
            //foreach (string item in failedNumbers)
            //{
            //    Console.WriteLine("Non-Existing: {0}", item);
            //}
        }

        static void wa_OnGetGroups(WaGroupInfo[] groups)
        {
            //Console.WriteLine("Got groups:");
            //foreach (WaGroupInfo info in groups)
            //{
            //    Console.WriteLine("\t{0} {1}", info.subject, info.id);
            //}
        }

        static void wa_OnGetPhotoPreview(string from, string id, byte[] data)
        {
            //Console.WriteLine("Got preview photo for {0}", from);
            //File.WriteAllBytes(string.Format("preview_{0}.jpg", from), data);
            OnGetMedia("photo_preview.jpg", from, null, data);
        }

        static void wa_OnGetPhoto(string from, string id, byte[] data)
        {
            //Console.WriteLine("Got full photo for {0}", from);
            //File.WriteAllBytes(string.Format("{0}.jpg", from), data);
            OnGetMedia("photo.jpg", from, null, data);
        }

        static void wa_OnGetMessageVcard(ProtocolTreeNode vcardNode, string from, string id, string name, byte[] data)
        {
            //Console.WriteLine("Got vcard \"{0}\" from {1}", name, from);
            //File.WriteAllBytes(string.Format("{0}.vcf", name), data);
            OnGetMedia(name + ".vcf", from, null, data);
        }

        static void wa_OnGetMessageLocation(ProtocolTreeNode locationNode, string from, string id, double lon, double lat, string url, string name, byte[] preview)
        {
            //Console.WriteLine("Got location from {0} ({1}, {2})", from, lat, lon);
            //if (!string.IsNullOrEmpty(name))
            //{
            //    Console.WriteLine("\t{0}", name);
            //}
            //File.WriteAllBytes(string.Format("{0}{1}.jpg", lat, lon), preview);
        }

        static void wa_OnGetMessageVideo(ProtocolTreeNode mediaNode, string from, string id, string fileName, int fileSize, string url, byte[] preview)
        {
            //Console.WriteLine("Got video from {0}", from, fileName);
            //OnGetMedia(fileName, url, preview);
            OnGetMedia(fileName, from, url, preview);
        }

        static void wa_OnGetMessageAudio(ProtocolTreeNode mediaNode, string from, string id, string fileName, int fileSize, string url, byte[] preview)
        {
            OnGetMedia(fileName, from, url, preview);
        }

        static void wa_OnGetMessageImage(ProtocolTreeNode mediaNode, string from, string id, string fileName, int size, string url, byte[] preview)
        {
            //Console.WriteLine("Got image from {0}", from, fileName);
            //OnGetMedia(fileName, url, preview);
            OnGetMedia(fileName, from, url, preview);
        }

        static void wa_OnGetPaused(string from)
        {
            //Console.WriteLine("{0} stopped typing", from);
        }

        static void wa_OnGetTyping(string from)
        {
            //Console.WriteLine("{0} is typing...", from);
        }

        static void wa_OnGetLastSeen(string from, DateTime lastSeen)
        {
            //Console.WriteLine("{0} last seen on {1}", from, lastSeen.ToString());
        }

        private void wa_OnGetMessageReceivedServer(string from, string participant, string id)
        {
            using (var db = new Data.WhatsJobsContext())
            {
                var message = db.Message.FirstOrDefault(m => m.WhatsId == id);
                if (message == null)
                {
                    _logger.Error(new Exception("Mensagem não encontrada"), id);
                    return;
                }

                message.ReceivedByServer = DateTime.Now;
                db.SaveChanges();
            }
        }

        private void wa_OnGetMessageReceivedClient(string from, string participant, string id)
        {
            using (var db = new Data.WhatsJobsContext())
            {
                var message = db.Message.FirstOrDefault(m => m.WhatsId == id);
                if (message == null)
                {
                    _logger.Error(new Exception("Mensagem não encontrada"), id);
                    return;
                }

                message.ReceivedByClient = DateTime.Now;
                db.SaveChanges();
            }
        }

        static void wa_OnGetMessageReadClient(string from, string participant, string id)
        {
            using (var db = new Data.WhatsJobsContext())
            {
                var message = db.Message.FirstOrDefault(m => m.WhatsId == id);
                if (message == null)
                {
                    _logger.Error(new Exception("Mensagem não encontrada"), id);
                    return;
                }

                message.ReadByClient = DateTime.Now;
                db.SaveChanges();
            }
        }

        static void wa_OnGetGroupParticipants(string gjid, string[] jids)
        {
            //Console.WriteLine("Got participants from {0}:", gjid);
            //foreach (string jid in jids)
            //{
            //    Console.WriteLine("\t{0}", jid);
            //}
        }

        static void wa_OnGetPresence(string from, string type)
        {
            //Console.WriteLine("Presence from {0}: {1}", from, type);
        }

        static void wa_OnNotificationPicture(string type, string jid, string id)
        {
            //TODO
            //throw new NotImplementedException();
        }

        private void wa_OnGetMessage(ProtocolTreeNode node, string from, string id, string name, string message, bool receipt_sent)
        {
            Model.Contact contact = Contact.GetContact(from, name);
            try
            {
                using (var db = new Data.WhatsJobsContext())
                {
                    var objMessage = new Model.Message(id, _ch, Contact.GetContact(from, name), message, true);
                    db.Message.Add(objMessage);
                    db.Entry(objMessage.WhatsChannel).State = System.Data.Entity.EntityState.Unchanged;
                    db.Entry(objMessage.Contact).State = System.Data.Entity.EntityState.Unchanged;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, String.Format("Error sending message \"{0}\" to {1}", message, contact.Number));
            }
        }

        private static void wa_OnLoginFailed(string data)
        {
            Console.WriteLine("Login failed. Reason: {0}", data);
        }

        #endregion Events


    }
}
