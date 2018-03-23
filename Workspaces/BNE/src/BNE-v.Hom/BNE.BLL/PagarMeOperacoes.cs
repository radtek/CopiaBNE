using System;
using System.Configuration;
using PagarMe;

namespace BNE.BLL
{
    public class PagarMeOperacoes
    {
        private static readonly string DefaultApiKeyPagarMe = ConfigurationManager.AppSettings["DefaultApiKeyPagarMe"];
        private static readonly string DefaultEncryptionKeyPagarMe = ConfigurationManager.AppSettings["DefaultEncryptionKeyPagarMe"];
        

        public static Transaction Charge(Transacao transacao, string urlRetorno)
        {
            PagarMeService.DefaultApiKey = DefaultApiKeyPagarMe;
            PagarMeService.DefaultEncryptionKey = DefaultEncryptionKeyPagarMe;
            
            Transaction transaction = new Transaction();
            transaction = CriarTransacao(transacao, urlRetorno);
            return transaction;
        }
        
        public static Transaction CriarTransacao(Transacao transacao, string url)
        {
            Transaction transaction = new Transaction();

            transaction.Amount = Convert.ToInt32(transacao.ValorDocumento * 100);
            transaction.PaymentMethod = PaymentMethod.CreditCard;
            transaction.CardHash = GerarCardHash(transacao);
            
            return transaction;
        }

        
        private static string GerarCardHash(Transacao transacao)
        {

            var creditcard = new CardHash();

            //creditcard.CardHolderName = "Jose da Silva";
            //creditcard.CardNumber = "5433229077370451";
            //creditcard.CardExpirationDate = "1038";
            //creditcard.CardCvv = "018";

            creditcard.CardHolderName = transacao.NomeCartaoCredito;
            creditcard.CardNumber = transacao.NumeroCartaoCredito;
            creditcard.CardExpirationDate =  transacao.NumeroMesValidadeCartaoCredito.PadLeft(2,'0') + transacao.NumeroAnoValidadeCartaoCredito;
            creditcard.CardCvv = transacao.NumeroCodigoVerificadorCartaoCredito;

            return creditcard.Generate();
        }

     
        public static Boolean CancelarTransacaoPagarMe(Pagamento pagamento)
        {
            PagarMeService.DefaultApiKey = DefaultApiKeyPagarMe;
            PagarMeService.DefaultEncryptionKey = DefaultEncryptionKeyPagarMe;

            var transaction = PagarMeService.GetDefaultService().Transactions.Find(pagamento.DescricaoIdentificador);

            transaction.Refund();
            
            return transaction.Status == TransactionStatus.Refunded;
        }

        public static Card RetornarCartao(string idCartao)
        {
            return PagarMeService.GetDefaultService().Cards.Find(idCartao);
        }
    }
}
