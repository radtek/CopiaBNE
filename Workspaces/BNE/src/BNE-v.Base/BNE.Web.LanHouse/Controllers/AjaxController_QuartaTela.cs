using System;
using System.Web.Mvc;
using BNE.Web.LanHouse.EntityFramework;

namespace BNE.Web.LanHouse.Controllers
{
    public partial class AjaxController
    {

        #region Métodos públicos
        public ActionResult QuartaTela()
        {
            decimal cnpj = CnpjOportunidade();
            int idFilial = IdFilialOportunidade();
            int idPessoaFisica = IdPessoaFisica();

            BNE_Curriculo objCurriculo;
            if (!BLL.Curriculo.CarregarPorPessoaFisica(idPessoaFisica, out objCurriculo))
                throw new InvalidOperationException("Pessoa física não tem currículo!");

            if (BLL.OportunidadeCurriculo.QuantidadeCandidaturas(idPessoaFisica) == 3 && !objCurriculo.Flg_VIP)
                return Json(false);

            bool retorno = BLL.OportunidadeCurriculo.Salvar(idPessoaFisica, cnpj, idFilial, BLL.Enumeradores.FaseCadastro.Minicurriculo);
            return Json(new { t = retorno, c = string.Format("{0}|{1}|{2}", DateTime.Now.ToString("yyyyMMdd"), idPessoaFisica, idFilial) });
        }
        #endregion Métodos públicos

    }
}