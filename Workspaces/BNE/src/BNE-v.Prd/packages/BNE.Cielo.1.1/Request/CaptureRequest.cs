using System;
using System.ComponentModel;
using System.Xml.Serialization;
using BNE.Cielo.Request.Element;

namespace BNE.Cielo.Request
{
	[SerializableAttribute ()]
	[DesignerCategoryAttribute ("code")]
	[XmlTypeAttribute (Namespace = "http://ecommerce.cbmp.com.br")]
	[XmlRootAttribute ("requisicao-captura", Namespace = "http://ecommerce.cbmp.com.br", IsNullable = false)]
	public partial class CaptureRequest :AbstractElement
	{
		[XmlElementAttribute ("tid")]
		public String tid { get; set; }

		[XmlElementAttribute ("dados-ec")]
		public DadosEcElement dadosEc { get; set; }

		public int valor { get; set; }

		public static CaptureRequest create (Transaction transaction)
		{
			return CaptureRequest.create (transaction, transaction.order.total);
		}

		public static CaptureRequest create (Transaction transaction, int total)
		{
			var cancellationRequest = new CaptureRequest {
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
	}
}

