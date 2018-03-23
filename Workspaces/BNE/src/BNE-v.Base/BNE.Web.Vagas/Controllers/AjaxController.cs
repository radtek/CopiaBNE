using System.Linq;
using System.Web.Mvc;

namespace BNE.Web.Vagas.Controllers
{
    public class AjaxController : BaseController
    {
        public JsonResult Cidade(string query, int limit)
        {
            var data = BLL.Cidade.RecuperarNomesCidadesEstado(query, string.Empty, limit).ToDictionary(d => d.Key.ToString(), d => d.Value.ToString());
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Funcao(string query, int limit)
        {
            int? origem = null;

            if (base.STC.ValueOrDefault)
                origem = base.IdOrigem.Value;

            var data = BLL.Funcao.RecuperarFuncoes(query, limit, origem).ToDictionary(d => d.ToString(), d => d.ToString());
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
