using APIGateway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace APIGateway.Modules
{
    public class ModuleRewriter : IHttpModule
    {
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Init(HttpApplication application)
        {
            application.BeginRequest +=
            (new EventHandler(this.Application_BeginRequest));
        }

        private void Application_BeginRequest(Object source,
         EventArgs e)
        {
            HttpApplication application = (HttpApplication)source;
            HttpRequest request = application.Request;
            //application.Request.Cookies.Add(new HttpCookie());

            Model.SwaggerConfig sc = Domain.SwaggerConfig.SwaggerConfigs.FirstOrDefault(n => Regex.IsMatch(application.Request.CurrentExecutionFilePath, "^/?"+ Flurl.Url.Combine(n.UrlSuffix, n.UIUrl) + "$"));

            if (sc != null)
            {
                string Theme = sc.Theme;
                string Authentication = sc.Authentication.Interface;

                String filePath = Flurl.Url.Combine(sc.UrlSuffix, sc.FileUrl);

                string FileUrl = Flurl.Url.Combine(request.Url.Scheme + "://" + request.Url.Authority, filePath);

                request.Cookies.Add(new HttpCookie("teste", "testando"));

                RewriterUtils.RewriteUrl(application.Context, Domain.SwaggerConfig.GetUrl(Theme, Authentication, FileUrl));

                //var response = request.CreateResponse(HttpStatusCode.Found);
                //response.Headers.Location = new Uri(Domain.SwaggerConfig.GetUrl(Theme, Authentication, FileUrl));
                //return response;
            }

        }
    }
}