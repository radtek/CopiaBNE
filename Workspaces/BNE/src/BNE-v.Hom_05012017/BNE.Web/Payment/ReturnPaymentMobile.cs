using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Payment
{
    public class ReturnPaymentMobile
    {
        private object p;

        public string Mensagem { get; set; }

        public string Url { get; set; }

        public bool Falha { get; set; }

        public object Dados { get; set; }



        public ReturnPaymentMobile(bool falha, object dados = null, string mensagem = null, string url = null)
        {
            Mensagem = mensagem;
            Url = url;
            Falha = falha;
            Dados = dados;
        }
    }
}