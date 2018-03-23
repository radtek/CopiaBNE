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
