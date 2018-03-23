using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BNE.Web.Services.Integracao.DTO
{
    [DataContract]
    public class InAtualizaNumNF
    {
        [DataMember(Name = "nt")]
        public string desIdentificador { get; set; }

        [DataMember(Name = "nf")]
        public string numeroNF { get; set; }

        [DataMember(Name = "cv")]
        public string codigoVerificacao { get; set; }

        [DataMember(Name = "ln")]
        public string linkNF { get; set; }
    }
}