using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BNE.Web.Services.Integracao.DTO
{
    [DataContract]
    public class OutAtualizaNumNF
    {
        [DataMember(Name = "r")]
        public bool retorno { get; set; }
    }
}