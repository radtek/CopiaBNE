using BNE.Common.Enumeradores;
using BNE.Common.Session;
using System;
using System.Collections.Generic;
using BNE.Auth.Core;

namespace BNE.Auth
{
    public class BNECleanPaymentSession : LoginBehaviorBase
    {
        protected override void OnBeforeLogoffSafe(Func<System.Web.HttpContextBase> contextAccessor, LogOffInfo info)
        {
            var context = contextAccessor();
            if (context == null)
                return;

            foreach (var item in SessionVariablesToClean())
            {
                item();
            }
        }

        protected IEnumerable<Action> SessionVariablesToClean()
        {
            yield return () => new SessionVariable<string>(Chave.Permanente.PagamentoUrlRetorno.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPagamento.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPlanoAdquirido.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.PagamentoIdentificadorPlano.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.PagamentoIdCodigoDesconto.ToString()).Clear();
            yield return () => new SessionVariable<decimal>(Chave.Permanente.PagamentoAdicionalValorTotal.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.PagamentoAdicionalQuantidade.ToString()).Clear();
            yield return () => new SessionVariable<int>(Chave.Permanente.PagamentoFormaPagamento.ToString()).Clear();
        }
    }

}
