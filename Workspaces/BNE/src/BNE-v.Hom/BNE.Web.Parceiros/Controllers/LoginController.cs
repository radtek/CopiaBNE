using BNE.BLL;
using BNE.Web.Parceiros.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BNE.Web.Parceiros.Business;
using BNE.EL;

namespace BNE.Web.Parceiros.Controllers
{
    public class LoginController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Entrar(string cpf, string nascimento)
        {
            try
            {
                decimal numCPF = Convert.ToDecimal(cpf.Replace(".", "").Replace("-", ""));
                DateTime dtaNasc = DateTime.ParseExact(nascimento, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                Session["ItemSession"] = new PessoaFisica().RecuperarPessoaSTCLanHouse(numCPF, dtaNasc);
                if (Session["ItemSession"] != null)
                    return RedirectToAction("Index", "Perfil");
                else
                    ViewBag.ErrorMessage = "Usuário inexistente.";
            }
            catch (Exception ex) 
            {
                ViewBag.ErrorMessage = "Não foi possível processar os dados de login.";
                GerenciadorException.GravarExcecao(ex);
            }
            
            return View("Index");
        }

    }
}
