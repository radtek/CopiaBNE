using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BNE;

namespace BNE.Web.Vip.Controllers
{
    public class BaseMVCController : Controller
    {

        [AllowAnonymous]
        [Route("sair")]
        [Route("logout")]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public RedirectResult Sair()
        {
            BNE.Auth.BNEAutenticacao.DeslogarPadrao();
            var url = string.Concat("http://", BNE.BLL.Custom.Helper.RecuperarURLAmbiente(), "/login.aspx?Deslogar=true");
            return Redirect(url);
        }
    }
}