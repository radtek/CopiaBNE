using BNE.Components.WebAPI;
using System.Net.Http;
using System.Web.Http.Filters;

namespace BNE.PessoaFisica.WebAPI.Attributes
{
    public class DeflateCompressionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Response == null)
                return;

            var content = actionExecutedContext.Response.Content;
            var bytes = content == null ? null : content.ReadAsByteArrayAsync().Result;
            var zlibbedContent = bytes == null ? new byte[0] : CompressionHelper.DeflateByte(bytes);

            actionExecutedContext.Response.Content = new ByteArrayContent(zlibbedContent);
            actionExecutedContext.Response.Content.Headers.Remove("Content-Type");
            actionExecutedContext.Response.Content.Headers.Add("Content-encoding","deflate");
            actionExecutedContext.Response.Content.Headers.Add("Content-Type", "application/json");

            base.OnActionExecuted(actionExecutedContext);
        }
    }
}