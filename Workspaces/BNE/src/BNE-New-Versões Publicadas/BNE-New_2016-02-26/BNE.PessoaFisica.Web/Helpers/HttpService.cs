using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace BNE.PessoaFisica.Web.Helpers
{
    public class HttpService
    {



        public string Get(Uri host, string path, Dictionary<string, string> headers, Dictionary<string, string> parameters)
        {
            try
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
                var url = new Uri(host, path + parameterString.ToString().TrimEnd(new char[] { '&' }));

                var client = new HttpClient();

                foreach (var item in headers)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }

                var response = client.GetAsync(url).Result;

                return response.Content.ReadAsStringAsync().Result;

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Get(Uri host, string path, Dictionary<string,string> parameters)
        {
            try
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
                var url = new Uri(host, path + parameterString.ToString().TrimEnd(new char[] { '&' }));

                var client = new HttpClient();

                

                var response = client.GetAsync(url).Result;

                return response.Content.ReadAsStringAsync().Result;

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public HttpResponseMessage Post(Uri host, string path, Dictionary<string, string> headers, string payload)
        {
            try
            {
                var url = new Uri(host, path);
                var client = new HttpClient();

                if (headers != null)
                {
                    foreach (var item in headers)
                    {
                        client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }
                }

                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                return client.PostAsync(url, content).Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public HttpResponseMessage Post(Uri host, string path, string payload)
        {
            try
            {
                var url = new Uri(host, path);
                var client = new HttpClient();

                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                return client.PostAsync(url, content).Result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}