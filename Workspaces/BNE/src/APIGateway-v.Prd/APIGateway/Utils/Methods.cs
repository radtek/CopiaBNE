using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace APIGateway.Utils
{
    public class Methods
    {
        /// <summary>
        /// Executa a chamada à um endereço destino
        /// </summary>
        /// <param name="destino">Objeto Uri com o endereço a ser chamado</param>
        /// <param name="headers">Headers a serem inclusos na chamada</param>
        /// <param name="request">Request recebido</param>
        /// <returns></returns>
        public static async Task<HttpResponseMessage> RunAsync(string destino, List<Model.Header> headers, HttpRequestMessage request)
        {
            HttpRequestMessage r = new HttpRequestMessage(request.Method, destino);

            if (request.Method != HttpMethod.Get && request.Method != HttpMethod.Head)
                r.Content = request.Content;

            r.Method = request.Method;

            //Setando headers de identificacao
            if (headers != null)
                foreach (var item in headers)
                    r.Headers.Add(item.Item, item.Value);

            using (var client = new HttpClient())
            {
                return await client.SendAsync(r);
            }
        }
    }
}