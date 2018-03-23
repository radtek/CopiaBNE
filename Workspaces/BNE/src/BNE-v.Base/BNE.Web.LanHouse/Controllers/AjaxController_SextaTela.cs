using System.Web.Mvc;
using BNE.Web.LanHouse.BLL.Entity;
using BNE.Web.LanHouse.Code.Enumeradores;

namespace BNE.Web.LanHouse.Controllers
{
    public partial class AjaxController
    {

        #region Métodos públicos
        public ActionResult SextaTela(Models.ModelAjaxSextaTela model)
        {
            if (!ModelState.IsValid)
                return MensagemErro();

            Session[Chave.SextaTela.ToString()] = new SextaTela(model);

            var tela6 = Session[Chave.SextaTela.ToString()] as SextaTela;

            if (tela6 == null)
                return Json(false);

            int idPessoaFisica = IdPessoaFisica();
            int idCidade = IdCidadeLAN();

            return Json(MiniCurriculo.Salvar(ref idPessoaFisica, Code.Helper.RecuperarIP(this), idCidade, tela6));
        }
        #endregion Métodos públicos

    }
}