using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Sample.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
       {
           try
           {
               var userWithClaims = (ClaimsPrincipal)User;
               var cpf = userWithClaims.Claims.First(c => c.Type == "cpf");
               var tipoVendedor = userWithClaims.Claims.First(c => c.Type == "tipoVendedor");
            

               if (tipoVendedor.Value.ToString() == "4")
               {
                   return View();
               }
               else
               {
                   return RedirectToAction("../Vendedor/Details/" + cpf.Value.ToString());                  
               }


           }
           catch (Exception e)
           {
                return View();
           }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}