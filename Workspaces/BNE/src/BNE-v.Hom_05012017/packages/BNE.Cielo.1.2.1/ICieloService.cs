using BNE.Cielo.Requests;
using BNE.Cielo.Responses;

namespace BNE.Cielo
{
    public interface ICieloService
    {
        CreateTransactionResponse CreateTransaction(CreateTransactionRequest request);
        CheckTransactionResponse CheckTransaction(CheckTransactionRequest request);
    }
}