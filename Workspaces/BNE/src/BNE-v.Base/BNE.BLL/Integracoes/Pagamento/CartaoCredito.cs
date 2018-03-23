using System;

namespace BNE.BLL.Integracoes.Pagamento
{
    public class CartaoCredito
    {
        public string NumeroDocumento { get; set; }
        public double ValorDocumento { get; set; }
        public int QuantidadeParcelas { get; set; }
        public decimal NumeroCartao { get; set; }
        public int MesValidade { get; set; }
        public int AnoValidade { get; set; }
        public string CodigoSeguranca { get; set; }
        public string EnderecoIPComprador { get; set; }
        public string Bandeira { get; set; }
        public string Adquirente { get; set; }
        public char ParcelamentoAdministradora { get; set; }
        public char PreAutorizacao { get; set; }

        public static RetornoCartaoCredito ValidarPagamento(Transacao objTransacao, out string erro)
        {
            erro = string.Empty;

            objTransacao.Operadora.CompleteObject();

            var objCartaoCredito = new CobreBem.CartaoCredito
                {
                    CodigoSeguranca = objTransacao.NumeroCodigoVerificadorCartaoCredito,
                    NumeroDocumento = objTransacao.IdTransacao.ToString(),
                    ValorDocumento = Convert.ToDouble(objTransacao.ValorDocumento),
                    QuantidadeParcelas = 1,
                    NumeroCartao = Convert.ToDecimal(objTransacao.NumeroCartaoCredito),
                    MesValidade = Convert.ToInt32(objTransacao.NumeroMesValidadeCartaoCredito),
                    AnoValidade = Convert.ToInt32(objTransacao.NumeroAnoValidadeCartaoCredito),
                    EnderecoIPComprador = objTransacao.DescricaoIPComprador,
                    Bandeira = objTransacao.Operadora.DescricaoOperadora.ToUpper()
                };

            return objCartaoCredito.ValidarTransacao(objTransacao, out erro);
        }

        public static RetornoCaptura CapturarPagamento(Transacao transacao, out string erro)
        {
            erro = string.Empty;

            return CobreBem.CartaoCredito.CapturarTransacao(transacao, out erro);
        }

        public class RetornoCartaoCredito
        {
            public bool Aprovado { get; set; }
            public string DescricaoTransacao { get; set; }
            public string DescricaoAutorizacao { get; set; }
            public string CodigoAutorizacao { get; set; }
        }

        public class RetornoCaptura
        {
            public bool Capturado { get; set; }
            public string DescricaoMensagemCaptura { get; set; }
        }

    }
}
