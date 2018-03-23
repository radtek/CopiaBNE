using System.Web;
using System.Web.Mvc;
using BNE.Web.LanHouse.Code.Enumeradores;

namespace BNE.Web.LanHouse.Code
{
    public static class EmitirTicketOrigem
    {
        public static bool Emitir(Controller controller)
        {
            object session = controller.ControllerContext.HttpContext.Session[Chave.IdFilialLAN.ToString()];

            if (session != null)
            {
                var cookie = new HttpCookie(Chave.CookieOrigem.ToString(), session.ToString()) { HttpOnly = true };
                controller.ControllerContext.HttpContext.Response.Cookies.Add(cookie);

                return true;
            }
            return false;
        }
    }
}