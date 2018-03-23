using System.Web.Mvc;

namespace BNE.Web.LanHouse.Code
{
    public class AutorizadoQCAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Result is HttpUnauthorizedResult)
            {
                // envia valor 1 do qc
                filterContext.Result = new JsonResult { Data = 0, ContentType = "application/json" };
            }
        }
    }
}