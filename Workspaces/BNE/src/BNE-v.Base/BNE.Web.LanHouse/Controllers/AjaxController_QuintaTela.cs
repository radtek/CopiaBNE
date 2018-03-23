using System;
using System.Web.Mvc;
using BNE.Web.LanHouse.BLL.Entity;
using BNE.Web.LanHouse.Code.Enumeradores;

namespace BNE.Web.LanHouse.Controllers
{
    public partial class AjaxController
    {

        #region Métodos públicos
        public ActionResult QuintaTela(Models.ModelAjaxQuintaTela model)
        {
            if (!ModelState.IsValid)
                return MensagemErro();

            Session[Chave.QuintaTela.ToString()] = new QuintaTela(model);

            var tela5 = Session[Chave.QuintaTela.ToString()] as QuintaTela;

            if (tela5 == null)
                return Json(false);

            int idPessoaFisica = IdPessoaFisica();

            return Json(MiniCurriculo.Salvar(ref idPessoaFisica, Code.Helper.RecuperarIP(this), tela5));
        }
        #endregion Métodos públicos

    }
}