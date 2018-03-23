using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Globalization;
using BNE.Cielo;

namespace BNE.NugetServer.Test
{
    [TestClass]
    public class UnitTest2
    {

        [TestMethod]
        public void TestMethod1()
        {
            String mid = "1006993069";
            String key = "25fbb99741c739dd84d7b06ec78c9bac718838630f30b112d033ce2e621b34f3";

            CieloService cielo = new CieloService(mid, key, CieloService.TEST);

            Holder holder = cielo.holder("4012001038443335", "2018", "05", "124443");
            holder.name = "Fulano Portador da Silva";

            Random randomOrder = new Random();

            Order order = cielo.order(randomOrder.Next(1000, 10000).ToString(), 10000);
            PaymentMethod paymentMethod = cielo.paymentMethod(PaymentMethod.VISA, PaymentMethod.CREDITO_A_VISTA);
            Transaction transaction = cielo.transactionRequest(holder, order, paymentMethod, "", Transaction.AuthorizationMethod.AUTHORIZE_WITHOUT_AUTHENTICATION, true);
            Transaction transactionC = cielo.cancellationRequest(transaction.tid, 10000);
        }
    }
}
