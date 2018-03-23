using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Integracao.DTO
{
    [DataContract]
    public class InUsuarioTanque
    {
        [DataMember(Name = "c")]
        public string cpf { get; set; }

        [DataMember(Name = "i")]
        public int IdUsuarioFilialPerfil { get; set; }
    }
}