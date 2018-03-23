using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.Enumeradores
{
    public enum PlanoSituacao
    {
        AguardandoLiberacao = 0,
        Liberado = 1,
        Encerrado = 2,
        Cancelado = 3,
        Bloqueado = 4,
        /*Indica que a venda do plano já está concretizada e que será liberado em um momento futuro
         *Utilizado na renovação e nos casos de compra com plano já liberado 
         */
        LiberacaoFutura = 5
    }
}
