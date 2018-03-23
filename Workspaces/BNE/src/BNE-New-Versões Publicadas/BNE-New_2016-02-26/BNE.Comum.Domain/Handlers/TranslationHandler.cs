using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BNE.Core.ExtensionsMethods;
using System.Globalization;

namespace BNE.Comum.Domain.Handlers
{
    /// <summary>
    /// Handler responsável por definir a linguagem a ser utilizada nas requisições.
    /// </summary>
    public class TranslationHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            string l = request.GetQueryString("l");
            if (!String.IsNullOrEmpty(l))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(l);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
