using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace BNE.BLL.Integracoes.SendGrid
{
    public class HttpClientSendGridApi
    {

        #region Constantes
        private const string URL = "https://api.sendgrid.com/api/{0}.get.json?start_date={1}&end_date={2}&api_user={3}&api_key={4}";
        private const int TIMEOUT = 10000;
        #endregion

        #region Propriedades
        private DateTime StartDate
        {
            get;
            set;
        }

        private string ApiUser
        {
            get;
            set;
        }

        private string ApiKey
        {
            get;
            set;
        }
        #endregion

        #region Construtores
        public HttpClientSendGridApi(string apiUser, string apiKey, string parametroStartDate)
        {
            if (String.IsNullOrEmpty(apiUser) || String.IsNullOrEmpty(apiKey))
                throw new ArgumentNullException("API User e API Key precisam ser especificados.");

            ApiUser = apiUser;
            ApiKey = apiKey;

            DateTime startDate;
            if (!DateTime.TryParse(parametroStartDate, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out startDate))
            {
                throw new ArgumentException("A data de importação de emails inválidos vindos do SendGrid API está em um formato incorreto: '" + parametroStartDate + "', esperado: dd/MM/yyyy", "parametroStartDate");
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
            return GetBlockedEmails()
                .Union(GetBouncedEmails())
                .Union(GetInvalidEmails())
                .AsEnumerable();
        }
        #endregion

        #region GetBlockedEmails
        public IEnumerable<IEmailStatus> GetBlockedEmails()
        {
            return GetStatus("blocks");
        }
        #endregion

        #region GetInvalidEmails
        public IEnumerable<IEmailStatus> GetInvalidEmails()
        {
            return GetStatus("invalidemails");
        }
        #endregion

        #region GetBouncedEmails
        public IEnumerable<IEmailStatus> GetBouncedEmails()
        {
            return GetStatus("bounces");
        }
        #endregion

        #region GetUnsubscribeEmails
        public IEnumerable<IEmailStatus> GetUnsubscribeEmails()
        {
            return GetStatus("unsubscribes");
        }
        #endregion

        #region GetStatus
        private IEnumerable<IEmailStatus> GetStatus(string type)
        {
            string url = BuildApiUrl(type);
            string response = GetResponse(url);

            if (String.IsNullOrEmpty(response))
            {
                throw new InvalidOperationException("Erro no web service, pode ser causado por erro na autenticação ou porque as datas estão no futuro");
            }

            var retorno = JsonConvert.DeserializeObject<EmailStatus[]>(response);

            return retorno;
        }
        #endregion

        #region BuildApiUrl
        private string BuildApiUrl(string operation)
        {
            DateTime endDate = StartDate.AddDays(1);

            string url = String.Format(URL, 
                operation, 
                StartDate.ToString("yyyy-MM-dd"), 
                endDate.ToString("yyyy-MM-dd"), 
                ApiUser, 
                ApiKey);

            return url;
        }
        #endregion

        #region GetResponse
        public string GetResponse(string uri)
        {
            string retorno = null;

            WebRequest request = WebRequest.Create(uri);
            request.Method = "GET";
            request.Timeout = TIMEOUT;

            try
            {
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
        public string status;
        public string reason;
        public string email;

        public string Status
        {
            get { return status; }
        }

        public string Reason
        {
            get { return reason; }
        }

        public string Email
        {
            get { return email; }
        }
    }
}