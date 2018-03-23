using System;
using System.Web.Mvc;
using BNE.Web.LanHouse.Code.Enumeradores;

namespace BNE.Web.LanHouse.Code
{
    //[RequireHttpsOnProduction]
    [Http400ErrorHandler]
    [EntrouOrigem]
    [ValidateAntiForgeryToken]
    public abstract class AbstractController : Controller
    {
        /// <summary>
        /// Serviço de cache
        /// </summary>
        protected ICacheService cache = new InMemoryCache();

        /// <summary>
        /// Devolve a primeira mensagem de erro presente no ModelState
        /// </summary>
        /// <returns>mensagem de erro ou null, caso não tenha nenhuma mensagem</returns>
        protected HttpStatusCodeResult MensagemErro(string mensagem = null)
        {
            if (!String.IsNullOrEmpty(mensagem))
                return new HttpStatusCodeResult(400, mensagem);

            if (ModelState.Count == 0)
                throw new InvalidOperationException(String.Format("{0}.{1}() sem ModelState", GetType().Name, ControllerContext.RouteData.Values["action"]));

            mensagem = null;
            string nomeState = null;

            foreach(var state in ModelState)
            {
                nomeState = state.Key;
                var modelState = state.Value;

                if (modelState.Errors.Count != 0)
                {
                    mensagem = modelState.Errors[0].ErrorMessage;
                    break;
                }
            }

            if (mensagem == null)
                throw new InvalidOperationException("Sem mensagem de erro");

            return new HttpStatusCodeResult(400, String.Format("{0}: {1}", nomeState, mensagem));
        }

        protected int IdPessoaFisica()
        {
            int id = 0;

            if (User != null && User.Identity != null && !String.IsNullOrEmpty(User.Identity.Name))
                id = Convert.ToInt32(User.Identity.Name);

            int sessionId = (int)(Session[Chave.IdPessoaFisica.ToString()] ?? 0);

            if (id != sessionId)
                id = 0;

            return id;
        }

        protected int IdCidadeLAN()
        {
            int id = 0;

            id = (int)(Session[Chave.IdCidadeLAN.ToString()] ?? 0);

            return id;
        }

        protected int IdOrigemLAN()
        {
            int id = 0;

            id = (int)(Session[Chave.IdOrigemLAN.ToString()] ?? 0);

            return id;
        }

        protected decimal CnpjOportunidade()
        {
            decimal cnpj = Decimal.Zero;

            cnpj = (decimal)(Session[Chave.CnpjOportunidade.ToString()] ?? decimal.Zero);

            return cnpj;
        }

        protected int IdFilialOportunidade()
        {
            int id = 0;

            id = (int)(Session[Chave.IdFilialOportunidade.ToString()] ?? 0);

            return id;
        }
    }
}
