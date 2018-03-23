using APIGateway.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;

namespace APIGateway.Handlers
{
    /// <summary>
    /// Handler created to handler Swagger images, css and javascript
    /// </summary>
    public class ReverseProxy : IHttpHandler
    {

        private Regex _regexFiles = new Regex(@"(.ttf|.png|.jpg|.js|.css|.ico)", RegexOptions.IgnoreCase);

        public bool IsReusable
        {
            get { return true; }
        }

        /// <summary>
        /// Method calls when client request the server
        /// </summary>
        /// <param name="context">HTTP context for client</param>
        public void ProcessRequest(HttpContext context)
        {
            //Verifing if the url is about swagger ui
            string urlReferrer = context.Request.UrlReferrer == null ? context.Request.CurrentExecutionFilePath : context.Request.UrlReferrer.AbsolutePath;
            Model.SwaggerConfig sc = Domain.SwaggerConfig.SwaggerConfigs.FirstOrDefault(n => Regex.IsMatch(urlReferrer, "^/?" + Flurl.Url.Combine(n.UrlSuffix, n.UIUrl)));

            //If is not, returns an empty response
            if (sc == null)
            {
                // Send 404 to client 
                context.Response.StatusCode = 404;
                context.Response.StatusDescription = "Page Not Found";
                context.Response.Write("Page not found");
                context.Response.End();
            }

            RemoteServer server;
            if (!_regexFiles.IsMatch(context.Request.CurrentExecutionFilePath))
            {
                //if NOT is a file, redirects to swagger ui
                if (!context.Request.CurrentExecutionFilePath.EndsWith("/"))
                {
                    context.Response.Redirect(context.Request.CurrentExecutionFilePath + "/");
                    context.Response.End();
                }
                server = new RemoteServer(context, Domain.SwaggerConfig.GetSwaggerUrl());
            }
            else
            {
                //if is a file, redirects to swagger ui concatenate eith file name
                string url = Regex.Replace(context.Request.CurrentExecutionFilePath, "^/?" + sc.UrlSuffix, string.Empty, RegexOptions.IgnoreCase);
                url = Regex.Replace(url, "^/?" + sc.UIUrl, string.Empty, RegexOptions.IgnoreCase);
                server = new RemoteServer(context, Flurl.Url.Combine(Domain.SwaggerConfig.GetSwaggerUrl(), url));
            }

            // Create a request with same data in navigator request
            HttpWebRequest request = server.GetRequest();

            // Send the request to the remote server and return the response
            HttpWebResponse response = server.GetResponse(request);
            byte[] responseData = server.GetResponseStreamBytes(response);

            // Send the response to client
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = response.ContentType;
            context.Response.OutputStream.Write(responseData, 0,
                             responseData.Length);

            // Handle cookies to navigator
            server.SetContextCookies(response);

            //If is not a file, sets the cookies with information about swagger
            if (!_regexFiles.IsMatch(context.Request.CurrentExecutionFilePath))
            {
                Dictionary<string, string> entries = new Dictionary<string, string>();
                entries.Add("theme", sc.Theme);
                entries.Add("authentication", sc.Authentication.Interface);
                entries.Add("filepath", Flurl.Url.Combine(context.Request.Url.Scheme + "://" + context.Request.Url.Authority, sc.UrlSuffix, sc.FileUrl));
                server.AddCookie("SwaggerSettings", entries);
            }

            // Close streams
            response.Close();
            context.Response.End();
        }
    }
}