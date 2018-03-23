using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Integracao.DTO
{
    [DataContract]
    public class InLiberaUsuarioTanque
    {
        [DataMember(Name = "ufp")]
        public int usuarioFilialPerfil { get; set; }

        [DataMember(Name = "dtaInicio")]
        public DateTime dataInicio { get; set; }

        [DataMember(Name = "dtaFim")]
        public DateTime? dtaFim { get; set; }

    }
}