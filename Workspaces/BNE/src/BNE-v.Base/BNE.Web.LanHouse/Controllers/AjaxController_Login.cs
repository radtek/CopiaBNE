using System;
using System.Globalization;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Security;
using BNE.Web.LanHouse.Code;
using BNE.Web.LanHouse.Code.Enumeradores;
using BNE.Web.LanHouse.EntityFramework;
using BNE.Web.LanHouse.Models;

namespace BNE.Web.LanHouse.Controllers
{
    public partial class AjaxController
    {

        #region Métodos públicos

        #region LogarFacebook
        [ActionName("signinf")]
        [HttpPost]
        public ActionResult LogarFacebook(ModelAjaxLogarFacebook model)
        {
            if (!ModelState.IsValid)
                return MensagemErro();

            // se estiver logado, retorna true
            if (IdPessoaFisica() != 0)
                return Json(true);

            TAB_Pessoa_Fisica_Rede_Social objPessoaFisicaRedeSocial;
            if (BLL.PessoaFisicaRedeSocial.CarregarPorFacebook(model.Login, out objPessoaFisicaRedeSocial))
            {
                FormalizarAutenticacao(objPessoaFisicaRedeSocial.Idf_Pessoa_Fisica);
                return View("NovoToken");
            }
            return Json(false);
        }
        #endregion LogarFacebook

        #region LogarCpf
        [ActionName("signinc")]
        [HttpPost]
        public ActionResult LogarCpf(ModelAjaxLogarCpf model)
        {
            if (!ModelState.IsValid)
                return MensagemErro();

            if (IdPessoaFisica() != 0)
                return Json(true);

            decimal cpf = Helper.ConverterCpfParaDecimal(model.Cpf);
            DateTime dataNasc = model.DataNascimento;

            TAB_Pessoa_Fisica objPessoaFisica;
            if (BLL.PessoaFisica.CarregarPorCpfDataNascimento(cpf, dataNasc, out objPessoaFisica))
            {
                FormalizarAutenticacao(objPessoaFisica.Idf_Pessoa_Fisica);
                return View("NovoToken");
            }

            return Json(false);
        }
        #endregion LogarCpf

        #region Deslogar
        [AutorizadoLogado]
        [ActionName("signout")]
        public ActionResult Deslogar()
        {
            try
            {
                // remove os tickets do contexto HTTP
                HttpContext.Request.Cookies.Remove("__RequestVerificationToken_Lw__");
                FormsAuthentication.SignOut();

                // retira o usuário do contexto HTTP para gerar um novo token logo a seguir
                HttpContext.User = new GenericPrincipal(new GenericIdentity(String.Empty), null);

                Session[Chave.IdPessoaFisica.ToString()] = null;
            }
            catch
            {
                return Json(false);
            }

            return View("NovoToken");
        }
        #endregion Deslogar

        #endregion Métodos públicos

        #region Métodos privados

        #region FormalizarAutenticacao
        private void FormalizarAutenticacao(int idPessoaFisica)
        {
            FormsAuthentication.SetAuthCookie(idPessoaFisica.ToString(CultureInfo.InvariantCulture), false);

            // cria temporariamente um usuário nomeado no contexto HTTP para poder gerar um token válido logo adiante
            HttpContext.User = new GenericPrincipal(new GenericIdentity(idPessoaFisica.ToString(CultureInfo.InvariantCulture)), null);

            Session[Chave.IdPessoaFisica.ToString()] = idPessoaFisica;
        }
        #endregion FormalizarAutenticacao

        #endregion Métodos privados

    }
}