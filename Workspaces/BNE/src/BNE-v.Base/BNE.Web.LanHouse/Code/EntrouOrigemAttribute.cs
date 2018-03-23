using System.Web;
using System.Web.Mvc;
using BNE.Web.LanHouse.Code.Enumeradores;

namespace BNE.Web.LanHouse.Code
{
    public class EntrouOrigemAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            object session = filterContext.HttpContext.Session[Chave.IdFilialLAN.ToString()];
            HttpCookie cookie = filterContext.HttpContext.Request.Cookies[Chave.CookieOrigem.ToString()];

            // a sessao está vazia ou o cookie não contém o mesmo idFilial da sessao?
            if (session == null || cookie == null || cookie.Value != session.ToString())
            {
                // precisa fazer refresh na sessao
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                    filterContext.Result =
                        new HttpStatusCodeResult(400, "Sessão expirada, favor recarregar a origem");
                else
                    filterContext.Result =
                        new RedirectResult("~/Filial/RecarregarOrigem");
            }
        }
    }
}