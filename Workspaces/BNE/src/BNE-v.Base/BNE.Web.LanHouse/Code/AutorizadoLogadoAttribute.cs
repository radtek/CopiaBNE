using System.Web.Mvc;

namespace BNE.Web.LanHouse.Code
{
    public class AutorizadoLogadoAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);

            if (filterContext.HttpContext.Request.IsAjaxRequest() && filterContext.Result is HttpUnauthorizedResult)
            {
                // envia valor falso
                JsonResult json = new JsonResult();
                json.Data = false;
                json.ContentType = "application/json";
                filterContext.Result = json;
            }
        }
    }
}