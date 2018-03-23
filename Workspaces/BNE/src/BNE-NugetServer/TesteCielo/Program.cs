using BNE.Cielo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteCielo
{
    class Program
    {
        static void Main(string[] args)
        {
            String mid = "1006993069";
            String key = "25fbb99741c739dd84d7b06ec78c9bac718838630f30b112d033ce2e621b34f3";

            CieloService cielo = new CieloService(mid, key, CieloService.TEST);

            Token token = cielo.tokenRequest(cielo.holder("4012001038443335", "2018", "05", "124443"));
            Holder holder = cielo.holder(token.code);

            Random randomOrder = new Random();

            Order order = cielo.order(randomOrder.Next(1000, 10000).ToString(), 10000);
            PaymentMethod paymentMethod = cielo.paymentMethod(PaymentMethod.VISA, PaymentMethod.CREDITO_A_VISTA);
            Transaction transaction = cielo.transactionRequest(holder, order, paymentMethod, "www.bne.com.br/Confirmacao", Transaction.AuthorizationMethod.RECURRENCE, true);
            Transaction transactionC = cielo.cancellationRequest(transaction.tid, 10000);
        }   
    }
}
