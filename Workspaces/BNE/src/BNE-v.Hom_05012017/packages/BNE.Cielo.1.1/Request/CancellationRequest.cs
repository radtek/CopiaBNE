using System;
using System.ComponentModel;
using System.Xml.Serialization;
using BNE.Cielo.Request.Element;

namespace BNE.Cielo.Request
{
	[SerializableAttribute ()]
	[DesignerCategoryAttribute ("code")]
	[XmlTypeAttribute (Namespace = "http://ecommerce.cbmp.com.br")]
	[XmlRootAttribute ("requisicao-cancelamento", Namespace = "http://ecommerce.cbmp.com.br", IsNullable = false)]
	public partial class CancellationRequest :AbstractElement
	{
		[XmlElementAttribute ("tid")]
		public String tid { get; set; }

		[XmlElementAttribute ("dados-ec")]
		public DadosEcElement dadosEc { get; set; }

		public int valor { get; set; }

		public static CancellationRequest create (Transaction transaction)
		{
			return CancellationRequest.create (transaction, transaction.order.total);
		}

		public static CancellationRequest create (Transaction transaction, int total)
		{
			var cancellationRequest = new CancellationRequest {
                id = Guid.NewGuid().ToString(),
				versao = CieloService.VERSION,
				tid = transaction.tid,
				dadosEc = new DadosEcElement {
					numero = transaction.merchant.id,
					chave = transaction.merchant.key
				},
				valor = total
			};

			return cancellationRequest;
		}

        public static CancellationRequest create(string tid, Merchant merchant, int total)
        {
            var cancellationRequest = new CancellationRequest
            {
                id = Guid.NewGuid().ToString(),
                versao = CieloService.VERSION,
                tid = tid,
                dadosEc = new DadosEcElement
                {
                    numero = merchant.id,
                    chave = merchant.key
                },
                valor = total
            };

            return cancellationRequest;
        }
	}
}

