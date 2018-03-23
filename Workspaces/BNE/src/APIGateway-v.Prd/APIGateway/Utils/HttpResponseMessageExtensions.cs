using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace APIGateway.Utils
{
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Creates a Task Completion Source. Used to throw erros.
        /// </summary>
        /// <param name="response">HttpResponseMessage to be returned</param>
        /// <returns></returns>
        public static Task<HttpResponseMessage> TaskCompletionResponse(this HttpResponseMessage response)
        {
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(response);
            return tsc.Task;
        }
    }
}