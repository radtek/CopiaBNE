using System.Web.Mvc;
using BNE.Web.LanHouse.Code.Enumeradores;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.Controllers
{
    public partial class AjaxController
    {

        #region Métodos públicos
        public ActionResult SegundaTela(Models.ModelAjaxSegundaTela model)
        {
            if (!ModelState.IsValid)
                return MensagemErro();

            var segundaTela = new BLL.Entity.SegundaTela(model);
            Session[Chave.SegundaTela.ToString()] = segundaTela;

            if (!ModelState.IsValid)
                return MensagemErro();

            // se estiver logado, retorna true
            if (IdPessoaFisica() != 0)
                return Json(true);

            return Json(true);
        }
        #endregion Métodos públicos
        
    }
}