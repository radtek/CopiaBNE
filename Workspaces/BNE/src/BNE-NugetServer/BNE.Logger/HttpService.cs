using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace BNE.Logger
{
    //TODO Tem um cara desse no bne.core do projeto novo, o ideal seria montar um nuget
    public class HttpService
    {
        public HttpService()
        {
            ServicePointManager.DefaultConnectionLimit = 100; //O padrão no .net é 2, então apenas duas conexões simultâneas poderão ocorrer.
            ServicePointManager.Expect100Continue = false; //Um roundtrip a menos
        }

        public string Get(Uri host, string path, Dictionary<string, string> parameters = default(Dictionary<string, string>), Dictionary<string, string> headers = default(Dictionary<string, string>))
        {
            var parameterString = new StringBuilder();

            if (parameters == null || parameters.Count <= 0 || parameters == default(Dictionary<string, string>))
                parameterString.Clear();
            else
            {
                parameterString.Append("?");
                foreach (KeyValuePair<string, string> parameter in parameters)
                {
                    parameterString.Append(parameter.Key + "=" + parameter.Value + "&");
                }
            }
            var url = new Uri(host, path + parameterString.ToString().TrimEnd('&'));

            var client = new HttpClient();

            if (headers != default(Dictionary<string, string>))
            {
                foreach (var item in headers)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            var response = client.GetAsync(url).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        public HttpResponseMessage Post(Uri host, string path, string payload, Dictionary<string, string> headers = default(Dictionary<string, string>))
        {
            var url = new Uri(host + path);

            var client = new HttpClient();

            if (headers != default(Dictionary<string, string>))
            {
                foreach (var item in headers)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }

            var content = new StringContent(payload, Encoding.UTF8, "application/json");

            return client.PostAsync(url, content).Result;
        }

    }
}