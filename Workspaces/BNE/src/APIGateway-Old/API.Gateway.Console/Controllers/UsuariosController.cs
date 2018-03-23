using API.Gateway.Console.Business;
using API.Gateway.Console.Filters;
using API.Gateway.Console.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Gateway.Console.Controllers
{
    [AccessControlFilter]
    public class UsuariosController : BNEController
    {

        public ActionResult Index(int Pagina = 1 )
        {
            return View(UsuarioBusiness.Carregar(Pagina));
        }


        public ActionResult Unregister(int Id)
        {
            return View(UsuarioBusiness.CarregarPorId(Id));
        }

        public ActionResult Deactivate(int Id)
        {
            string msg = "";
            bool result = UsuarioBusiness.Desativar(Id, out msg);
            ViewBag.Message = msg;
            return View(result);
        }

        public ActionResult Reactivate(int Id)
        {
            string msg = "";
            var us = UsuarioBusiness.CarregarPorId(Id);
            bool result = UsuarioBusiness.Registar(us.Num_CPF, us.Idf_Filial, us.Senha, us.Dta_Nascimento, us.Idf_Perfil, out msg);
            ViewBag.Message = msg;
            return View(result);
        }

        public ActionResult ConfirmRegister(RegisterModel model)
        {
            string msg = "";
            bool result = false;

            decimal Num_CPF;
            int? Idf_Filial;
            string Senha ;
            DateTime Dta_Nascimento;            
            try
            {
                Num_CPF = Convert.ToDecimal(model.Num_CPF.Replace(".", "").Replace("-", ""));
                var arr_dta = model.Dta_Nascimento.Split('/');
                Dta_Nascimento = new DateTime(int.Parse(arr_dta[2]), int.Parse(arr_dta[1]), int.Parse(arr_dta[0]));
                Idf_Filial = (model.Idf_Filial != null) ? (int?)Convert.ToInt32(model.Idf_Filial) : null;
                Senha = (!string.IsNullOrEmpty(model.Senha)) ? model.Senha : null;

                result = UsuarioBusiness.Registar(Num_CPF, Idf_Filial, Senha, Dta_Nascimento, model.Idf_Perfil, out msg);
            }
            catch
            {
                msg = "Dados inválidos";
            }

            ViewBag.Message = msg;
            return View(result);
           
        }


        public ActionResult View(int Id)
        {
            return View(UsuarioBusiness.CarregarPorId(Id));
        }

        public ActionResult Register()
        {
            @ViewBag.Perfis = UsuarioBusiness.CarregarPerfis();
            return View();
        }

    }
}