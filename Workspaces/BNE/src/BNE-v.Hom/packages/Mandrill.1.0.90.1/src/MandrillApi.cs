// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MandrillApi.cs" company="">
//   
// </copyright>
// <summary>
//   Core class for using the MandrillApp Api
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Mandrill.Models;
using Mandrill.Requests;
using Mandrill.Utilities;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Mandrill.Requests.Messages;
using Newtonsoft.Json;

namespace Mandrill
{
    /// <summary>
    ///   Core class for using the MandrillApp Api
    /// </summary>
    public partial class MandrillApi
    {
        #region Fields

        private readonly string baseUrl;
        private HttpClient _httpClient;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="MandrillApi" /> class.
        /// </summary>
        /// <param name="apiKey">
        ///   The API Key recieved from MandrillApp
        /// </param>
        /// <param name="useHttps">
        /// </param>
        /// <param name="timeout">
        ///   Timeout in milliseconds to use for requests.
        /// </param>
        public MandrillApi(string apiKey, bool useHttps = true)
        {
            ApiKey = apiKey;

            if (useHttps)
            {
                baseUrl = Configuration.BASE_SECURE_URL;
            }
            else
            {
                baseUrl = Configuration.BASE_URL;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   The Api Key for the project received from the MandrillApp website
        /// </summary>
        public string ApiKey { get; private set; }
        
        #endregion

        #region Public Methods and Operators

        /// <summary>
        ///   Allows overriding the HttpClient which is used in Post()
        /// </summary>
        /// <param name="httpClient">the httpClient to use</param>
        public void SetHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            SetBaseAdress(_httpClient);
        }

        public void SetBaseAdress(HttpClient httpClient)
        {
            httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<T> Post<T>(string path, RequestBase data, bool withHttpWebRequest = false)
        {
            return await PostData<T>(path, data, withHttpWebRequest);
        }

        public async Task<T> Post<T>(string path, RequestBase data)
        {
            return await PostData<T>(path, data);
        }

        /// <summary>
        ///   Execute post to path
        /// </summary>
        /// <param name="path">the path to post to</param>
        /// <param name="data">the payload to send in request body as json</param>
        /// <param name="withHttpWebRequest">send with http web request</param>
        /// <returns></returns>
        public async Task<T> PostData<T>(string path, RequestBase data, bool withHttpWebRequest = false)
        {
            data.Key = ApiKey;
            try
            {
                var dispose = false;

                HttpClient client;
                if (_httpClient != null)
                    client = _httpClient;
                else
                {
                    client = new HttpClient();
                    SetBaseAdress(client);
                    dispose = true;
                }

                if (withHttpWebRequest)
                {
                    byte[] postBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));

                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(_httpClient.BaseAddress + path);
                    httpWebRequest.Method = "POST";
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.ContentLength = postBytes.Length;

                    using (var stream = httpWebRequest.GetRequestStream())
                    {
                        stream.Write(postBytes, 0, postBytes.Length);
                    }

                    var httpWebresponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    var responseString = new StreamReader(httpWebresponse.GetResponseStream()).ReadToEnd();
                    if (httpWebresponse.StatusCode == HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<T>(responseString);
                    }
                    var errorHttpWebresponse = JsonConvert.DeserializeObject<ErrorResponse>(responseString);

                    throw new MandrillException(errorHttpWebresponse, string.Format("Post failed {0} with status {1} and content '{2}'", path, httpWebresponse.StatusCode, data));
                }

                var response = await client.PostAsync(path, new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json")).ConfigureAwait(false);

                if (dispose)
                    client.Dispose();

                if (response.IsSuccessStatusCode)
                    return JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                var error = JsonConvert.DeserializeObject<ErrorResponse>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));

                throw new MandrillException(error, string.Format("Post failed {0} with status {1} and content '{2}'", path, response.StatusCode, data));
            }
            catch (TimeoutException)
            {
                throw new TimeoutException(string.Format("Post timed out to {0}", path));
            }
        }

        #endregion
    }
}