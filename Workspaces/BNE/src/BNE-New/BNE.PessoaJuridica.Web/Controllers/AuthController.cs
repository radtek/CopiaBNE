using System;
using System.Web.Mvc;

namespace BNE.PessoaJuridica.Web.Controllers
{
    public class AuthController : Controller
    {
        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Teste()
        {
            try
            {
                var u = Auth.NET45.BNEAutenticacao.User();
            }
            catch (Exception)
            {
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string teste)
        {
            SignOut();

            return View();
        }



        public ActionResult SignIn()
        {
            decimal cpf = 04914896923;
            DateTime dataNascimento = new DateTime(1986, 03, 24);
            string nome = " ";

            Auth.NET45.BNEAutenticacao.LogarCandidato(nome, cpf, dataNascimento);

            var user = Auth.NET45.BNEAutenticacao.User();

            return View("Index");
        }

        public ActionResult SignOut()
        {
            Auth.NET45.BNEAutenticacao.Deslogar();
            return View("Index");
        }

    }
}