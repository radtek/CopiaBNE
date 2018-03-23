using System.Web.Mvc;
using BNE.Web.LanHouse.BLL.Entity;
using System;

namespace BNE.Web.LanHouse.Controllers
{
    public partial class AjaxController
    {

        #region Métodos públicos
        [ActionName("vipok")]
        public ActionResult VipTela(string c)
        {
            string codigoDesconto = c;

            try
            {
            //var retorno = Vip.Autorizar(IdPessoaFisica(), codigoDesconto);
                return Json(Vip.Autorizar(IdPessoaFisica(), codigoDesconto));
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }

            return Json(false);
            /*
             * 
             * 
             * 
             * if (!string.IsNullOrWhiteSpace(mensagemErro))
                    
             * 
             * 
             */
        }
        #endregion Métodos públicos

    }
}