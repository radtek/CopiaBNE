using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Integracao.DTO
{
    [DataContract]
    public class OutLiberaUsuarioTanque
    {
        [DataMember(Name = "status")]
        public bool status { get; set; }
    }
}