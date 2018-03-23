using System.Web.Mvc;

namespace BNE.Web.LanHouse.Code
{
    public class Http400ErrorHandlerAttribute : FilterAttribute, IExceptionFilter
    {
        public virtual void OnException(ExceptionContext filterContext)
        {
            EL.GerenciadorException.GravarExcecao(filterContext.Exception);

            string mensagem = filterContext.Exception.Message;
            filterContext.Result = new HttpStatusCodeResult(400, mensagem);
            filterContext.ExceptionHandled = true;
        }
    }
}