using APIGateway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using APIGateway.Utils;

namespace APIGateway.Handlers
{
    /// <summary>
    /// Handler to calls for Swagger Ui
    /// </summary>
    public class SwaggerHandler : DelegatingHandler
    {
        public SwaggerHandler(HttpConfiguration httpConfiguration)
        {
            InnerHandler = new HttpControllerDispatcher(httpConfiguration);
        }

        private Regex _regexFiles = new Regex(@"(.ttf|.png|.jpg|.js|.css|.ico)", RegexOptions.IgnoreCase);

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            ////Verifing if the url is about swagger ui
            string urlReferrer = request.RequestUri.AbsolutePath;
            Model.SwaggerConfig sc = Domain.SwaggerConfig.SwaggerConfigs.FirstOrDefault(n => Regex.IsMatch(urlReferrer, "^/?" + Flurl.Url.Combine(n.UrlSuffix, n.UIUrl)));

            ////If is not, returns an Not Found response
            if (sc == null)
                return request.CreateResponse(HttpStatusCode.NotFound).TaskCompletionResponse();

            //Trying to get the path route value.
            //It will be set if a file (image, css, js) was requested
            string path = TryGetPath(request);

            string url;
            if (String.IsNullOrEmpty(path))
            {
                //if NOT is a file, redirects to swagger ui
                if (!request.RequestUri.AbsolutePath.EndsWith("/"))
                {
                    var response = request.CreateResponse(HttpStatusCode.Redirect);
                    response.Headers.Location = new Uri(request.RequestUri.AbsoluteUri + "/");
                    return response.TaskCompletionResponse();
                }
                url = Domain.SwaggerConfig.GetSwaggerUrl();
            }
            else
            {
                ////if is a file, redirects to swagger ui concatenate eith file name
                //url = Regex.Replace(request.RequestUri.AbsolutePath, "^/?" + sc.UrlSuffix, string.Empty, RegexOptions.IgnoreCase);
                //url = Regex.Replace(url, "^/?" + sc.UIUrl, string.Empty, RegexOptions.IgnoreCase);
                url = Flurl.Url.Combine(Domain.SwaggerConfig.GetSwaggerUrl(), path);
            }

            return Utils.Methods.RunAsync(url, null, request).ContinueWith(task =>
                {
                    var response = task.Result;


                    //If is not a file, sets the cookies with information about swagger
                    if (!_regexFiles.IsMatch(request.RequestUri.AbsolutePath))
                    {
                        List<CookieHeaderValue> cookies = new List<CookieHeaderValue>();

                        cookies.Add(new CookieHeaderValue("theme", sc.Theme));
                        cookies.Add(new CookieHeaderValue("authentication", sc.Authentication.Interface));
                        cookies.Add(new CookieHeaderValue("filepath", Flurl.Url.Combine(request.RequestUri.Scheme + "://" + request.RequestUri.Authority, sc.UrlSuffix, sc.FileUrl)));

                        response.Headers.AddCookies(cookies);

                    }

                    return response;
                });
        }

        private string TryGetPath(HttpRequestMessage request)
        {
            int cont = 0;

            foreach (var item in request.GetRouteData().Values.Keys.ToList())
            {
                if (item == "path")
                {
                    return (String) request.GetRouteData().Values.Values.ToList()[cont];
                }

                cont++;
            }

            return String.Empty;
        }
    }
}