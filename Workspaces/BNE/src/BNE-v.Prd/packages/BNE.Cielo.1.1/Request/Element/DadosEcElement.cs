﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BNE.Cielo.Request.Element
{
	[SerializableAttribute ()]
	[XmlTypeAttribute (Namespace = "http://ecommerce.cbmp.com.br")]
	public partial class DadosEcElement :AbstractElement
	{
		public String numero { get; set; }

		public String chave { get; set; }

		public Merchant ToMerchant ()
		{
			return new Merchant (numero, chave);
		}
	}
}