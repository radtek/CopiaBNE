using System.Web.Mvc;

namespace BNE.Web.Vagas.Code.ActionFilter
{
    public class FuncaoCidadeActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            filterContext.ActionParameters["funcao"] = "Funcao";
            filterContext.ActionParameters["cidade"] = "Cidade";
        }
    }
}