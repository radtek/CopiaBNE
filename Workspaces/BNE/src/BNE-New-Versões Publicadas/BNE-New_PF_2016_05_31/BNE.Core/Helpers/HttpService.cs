using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BNE.Core.Helpers
{
    public class HttpService
    {

        public string Get(string uri, Dictionary<string, string> parameters = null)
        {
            var parameterString = new StringBuilder();

            if (parameters == null || parameters.Count <= 0)
                parameterString.Clear();
            else
            {
                parameterString.Append("?");
                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    parameterString.Append(parameter.Key + "=" + parameter.Value + "&");
                }
            }
            var url = new Uri(uri + parameterString.ToString().TrimEnd(new char[] { '&' }));

            var client = new HttpClient();

            var response = client.GetAsync(url).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        public HttpResponseMessage Post(string host, string path, Dictionary<string, string> headers, string payload)
        {
            var url = new Uri(host + path);

            var client = new HttpClient();
            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            return client.PostAsync(url, content).Result;
        }

    }
}