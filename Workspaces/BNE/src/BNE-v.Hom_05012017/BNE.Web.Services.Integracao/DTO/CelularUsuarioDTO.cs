using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BNE.Web.Services.Integracao.DTO
{
    [DataContract]
    public class CelularUsuarioDTO
    {
        [DataMember(Name = "ics")]
        public int IdCelularSelecionador { get; set; }

        [DataMember(Name = "iufp")]
        public int IdUsuarioFilialPerfil { get; set; }

        [DataMember(Name = "ic")]
        public int IdCelular { get; set; }

        [DataMember(Name = "di")]
        public DateTime? DataInicio { get; set; }

        [DataMember(Name = "df")]
        public DateTime? DataFim { get; set; }
    }
}