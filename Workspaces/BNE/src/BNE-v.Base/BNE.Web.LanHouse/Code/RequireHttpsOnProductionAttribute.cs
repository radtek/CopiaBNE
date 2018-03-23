using System;
using System.Web.Mvc;

namespace BNE.Web.LanHouse.Code
{
    public class RequireHttpsOnProductionAttribute : RequireHttpsAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            
            if (filterContext.HttpContext != null && filterContext.HttpContext.Request.IsLocal)
                return;
            
            base.OnAuthorization(filterContext);
        }
    }
}