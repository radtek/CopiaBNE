namespace BNE.BLL.Enumeradores
{
    public enum StatusTransacaoGlobal
    {
        SolicitacaoDePagamento = 1,
        AguardandoPagamento = 2,
        Pago = 3,
        NaoPago = 4,
        Cancelado = 5,
        AguardandoLiberacao = 6
    }
}
