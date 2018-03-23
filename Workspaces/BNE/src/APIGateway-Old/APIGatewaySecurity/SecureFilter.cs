using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace APIGatewaySecurity
{
    public class SecureFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext ctx)
        {
            var secured_controller = ctx.ControllerContext.Controller as SecureController;
            try
            {
                secured_controller.ApiKey = ctx.Request.Headers.GetValues("apiKey").First();
            }
            catch { }
            base.OnActionExecuting(ctx);
        }
    }
}
