using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace BNE.BLL.Integracoes.Mandrill
{
    public class HttpClientMandrillApi
    {        
        #region Constantes
        private const string URL = "https://mandrillapp.com/api/1.0/rejects/list.json";
        private const string DATA = @"{{""key"": ""{0}"",""include_expired"": true}}";
        private const int TIMEOUT = 10000;
        #endregion

        #region Propriedades
        private DateTime StartDate
        {
            get;
            set;
        }

        private string ApiKey
        {
            get;
            set;
        }

        private IEnumerable<EmailStatus> Pool
        {
            get;
            set;
        }
        #endregion

        #region Construtores
        public HttpClientMandrillApi(string apiKey, string parametroStartDate)
        {
            if (String.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException("API Key precisa ser especificada.");

            ApiKey = apiKey;

            DateTime startDate;
            if (!DateTime.TryParse(parametroStartDate, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out startDate))
            {
                throw new ArgumentException("A data de importação de emails inválidos vindos do Mandrill API está em um formato incorreto: '" + parametroStartDate + "', esperado: dd/MM/yyyy", "parametroStartDate");
            }

            StartDate = startDate;
        }
        #endregion

        #region Métodos

        #region GetBlackList
        public IEnumerable<string> GetBlackList()
        {
            return GetCompleteBlackList()
                .Select(status => status.Email)
                .Distinct()
                .AsEnumerable();
        }
        #endregion

        #region GetCompleteBlackList
        public IEnumerable<IEmailStatus> GetCompleteBlackList()
        {
            return GetSpammedEmails()
                .Union(GetHardBounceEmails())
                .Union(GetSoftBounceEmails())
                .AsEnumerable();
        }
        #endregion

        #region GetSpammedEmails
        public IEnumerable<IEmailStatus> GetSpammedEmails()
        {
            return GetReason("spam");
        }
        #endregion

        #region GetHardBounceEmails
        public IEnumerable<IEmailStatus> GetHardBounceEmails()
        {
            return GetReason("hard-bounce");
        }
        #endregion

        #region GetSoftBounceEmails
        public IEnumerable<IEmailStatus> GetSoftBounceEmails()
        {
            return GetReason("soft-bounce");
        }
        #endregion

        #region GetBounceEmails
        public IEnumerable<IEmailStatus> GetBounceEmails()
        {
            return GetSoftBounceEmails()
                .Union(GetHardBounceEmails())
                .AsEnumerable();
        }
        #endregion

        #region GetUnsubscribeEmails
        public IEnumerable<IEmailStatus> GetUnsubscribeEmails()
        {
            return GetReason("unsub");
        }
        #endregion

        #region GetReason
        private IEnumerable<IEmailStatus> GetReason(string type)
        {
            if (Pool == null)
            {
                string url = BuildApiUrl();
                string response = GetResponse(url, ApiKey);

                if (String.IsNullOrEmpty(response))
                {
                    throw new InvalidOperationException("Erro durante acesso à API do Mandrill, pode ter sido causado por um erro 500 no servidor ou porque a API Key é inválida");
                }

                Pool = JsonConvert.DeserializeObject<EmailStatus[]>(response);
            }

            return Pool.Where(status => status.Reason.Equals(type)).AsEnumerable();
        }
        #endregion

        #region BuildApiUrl
        private string BuildApiUrl()
        {
            return URL;
        }
        #endregion

        #region GetResponse
        public string GetResponse(string uri, string apiKey)
        {
            string retorno = null;

            string json = String.Format(DATA, apiKey);

            byte[] buffer = Encoding.ASCII.GetBytes(json);

            WebRequest request = WebRequest.Create(uri);
            request.Method = "POST";
            request.Timeout = TIMEOUT;
            request.ContentLength = buffer.Length;

            try
            {
                using (Stream postData = request.GetRequestStream())
                {
                    postData.Write(buffer, 0, buffer.Length);
                }

                using (WebResponse response = request.GetResponse()) // pode dar exceção de timeout WebException
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader sr = new StreamReader(stream))
                        {
                            retorno = sr.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException)
            {
                retorno = null;
            }

            return retorno;
        }
        #endregion

        #endregion
    }

    [Serializable]
    public class EmailStatus : IEmailStatus
    {
        public string email;
        public string reason;
        public string detail;
        public string created_at;
        public string last_event_at;
        public string expires_at;
        public bool expired;
        public EmailSender sender;
        public string subaccount;

        public string Email
        {
            get { return email; }
        }

        public EmailSender Sender
        {
            get { return sender; }
        }

        public string Reason
        {
            get { return reason; }
        }
    }

    [Serializable]
    public class EmailSender
    {
        public string address;
        public string created_at;
        public int sent;
        public int hard_bounces;
        public int soft_bounces;
        public int rejects;
        public int complaints;
        public int unsubs;
        public int opens;
        public int clicks;
        public int unique_opens;
        public int unique_clicks;

        public string Email
        {
            get { return address; }
        }
    }
}
