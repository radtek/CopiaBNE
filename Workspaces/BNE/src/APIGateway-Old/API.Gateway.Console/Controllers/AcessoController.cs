using API.Gateway.Console.BNEModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace API.Gateway.Console.Controllers
{
    public class AcessoController : BNEController
    {

        public ActionResult Entrar()
        {
            return View();
        }

        public ActionResult ValidarAcesso(string cpf, string dta_nascimento, string senha)
        {
            decimal Num_CPF;
            DateTime Dta_Nascimento;
            TAB_Pessoa_Fisica pessoa_fisica;
           
            try
            {
                Num_CPF = Convert.ToDecimal(cpf.Replace(".", "").Replace("-", ""));
                var arr_dta = dta_nascimento.Split('/');
                Dta_Nascimento = new DateTime(int.Parse(arr_dta[2]), int.Parse(arr_dta[1]), int.Parse(arr_dta[0]));
            }
            catch 
            {
                ViewBag.Message = "Dados inválidos";
                return View("Entrar");
            }


            using (var dbo = new BNEContext()) 
            {
                pessoa_fisica = (from ufp in dbo.TAB_Usuario_Filial_Perfil
                          join pf in dbo.TAB_Pessoa_Fisica on ufp.Idf_Pessoa_Fisica equals pf.Idf_Pessoa_Fisica
                          join us in dbo.BNE_Usuario on pf.Idf_Pessoa_Fisica equals us.Idf_Pessoa_Fisica
                          where pf.Num_CPF == Num_CPF  && pf.Dta_Nascimento == Dta_Nascimento && ufp.Idf_Perfil == 1 && us.Sen_Usuario == senha
                          && ufp.Flg_Inativo == false && pf.Flg_Inativo == false && us.Flg_Inativo == false
                          select pf).FirstOrDefault();
            }

            if (pessoa_fisica != null) 
            {
                HttpContext.Session.Add("active_user_key", pessoa_fisica.Nme_Pessoa.Trim().Split(' ')[0]);
                return RedirectToAction("Index", "Usuarios");
            }
            else 
            {
                ViewBag.Message = "Usuário não encontrado.";
                return View("Entrar");
            }
        }

        public ActionResult Sair()
        {
            HttpContext.Session.Remove("active_user_key");
            return View("Entrar");
        }

    }
}