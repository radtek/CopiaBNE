using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Enumerador
{
    public enum SituacaoAtendimento
    {
        Atendimento = 1,
        Negociacao = 2,
        SemAtendimento = 3,
        Venda = 4,
        FinalDoPrazo = 6,
        Prospeccao = 7
    }
}