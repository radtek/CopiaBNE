using System.Web.Mvc;
using BNE.BLL.Integracoes.Facebook;
using BNE.Web.LanHouse.Code.Enumeradores;
using Newtonsoft.Json;

namespace BNE.Web.LanHouse.Controllers
{
    public partial class AjaxController
    {

        #region Métodos públicos

        #region ArmazenarDadosFacebook
        [HttpPost]
        [ActionName("salvarfb")]
        public ActionResult ArmazenarDadosFacebook(string m, string p)
        {
            var dadosFacebook = JsonConvert.DeserializeObject<ProfileFacebook.DadosFacebook>(m);
            var dadosFoto = JsonConvert.DeserializeObject<ProfileFacebook.FotoFacebook>(p);

            Session[Chave.DadosFacebook.ToString()] = dadosFacebook;
            Session[Chave.FotoFacebook.ToString()] = dadosFoto;

            return Json(true);
        }
        #endregion ArmazenarDadosFacebook

        #region ArmazenarDadosFacebook
        [HttpPost]
        [ActionName("fb")]
        public ActionResult RecuperarDadosFacebook()
        {
            if (Session[Chave.DadosFacebook.ToString()] != null)
            {
                var dadosFacebook = (ProfileFacebook.DadosFacebook)Session[Chave.DadosFacebook.ToString()];

                return Json(new { cp = dadosFacebook.UltimaFuncao, ec = dadosFacebook.IdEstadoCivil });
            }
            return Json(false);
        }
        #endregion ArmazenarDadosFacebook

        #endregion Métodos públicos

    }
}