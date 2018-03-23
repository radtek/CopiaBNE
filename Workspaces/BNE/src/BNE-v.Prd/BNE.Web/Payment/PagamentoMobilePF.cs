using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Payment
{
    using BNE.BLL.Integracoes.Pagamento;
    using Enumeradores = BNE.BLL.Enumeradores;

    public abstract class PagamentoMobilePF
    {
        public static bool ProcessoCompra(ref PlanoAdquirido objPlanoAdquirido, Enumeradores.TipoPagamento tipoPagamento)
        {
            // Carrega Plano Adquirido
            objPlanoAdquirido.Plano.CompleteObject();
            objPlanoAdquirido.UsuarioFilialPerfil.CompleteObject();

            if (objPlanoAdquirido.ParaPessoaFisica())
            {
                if (!PlanoParcela.ExisteParcelaCriada(objPlanoAdquirido))
                {
                    objPlanoAdquirido.CriarParcelas(new TipoPagamento((int)tipoPagamento), null, objPlanoAdquirido.QuantidadePrazoBoleto, objPlanoAdquirido.QtdParcela);
                    return true;
                }
                else
                    return objPlanoAdquirido.AjustarParcelas(new TipoPagamento((int)tipoPagamento), null, objPlanoAdquirido.QuantidadePrazoBoleto);
            }
            return false;
        }

    }

}