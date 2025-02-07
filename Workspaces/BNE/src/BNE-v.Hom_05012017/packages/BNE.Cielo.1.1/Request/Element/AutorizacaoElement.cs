﻿using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace BNE.Cielo.Request.Element
{
	[SerializableAttribute ()]
	[XmlTypeAttribute (Namespace = "http://ecommerce.cbmp.com.br")]
	public partial class AutorizacaoElement :AbstractElement
	{
		public int codigo { get; set; }

		public String mensagem { get; set; }

		[XmlElementAttribute ("data-hora")]
		public String dataHora { get; set; }

		public int valor { get; set; }

		public int lr { get; set; }

		public int arp { get; set; }

		public int nsu { get; set; }

		public Authorization ToAuthorization ()
		{
			Authorization authorization = new Authorization ();

			authorization.code = codigo;
			authorization.message = mensagem;
			authorization.dateTime = dataHora;
			authorization.total = valor;
			authorization.lr = lr;
			authorization.arp = arp;
			authorization.nsu = nsu;

			return authorization;
		}
	}
}