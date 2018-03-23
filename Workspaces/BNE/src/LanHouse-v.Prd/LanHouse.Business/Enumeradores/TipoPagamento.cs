using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.Enumeradores
{
    public enum TipoPagamento
    {
        CartaoCredito = 1,
        BoletoBancario = 2,
        DepositoIdentificado = 3,
        Parceiro = 4,
        DebitoOnline = 5,
        PagSeguro = 6,
        PayPal = 7,
        DebitoRecorrente = 8
    }
}

