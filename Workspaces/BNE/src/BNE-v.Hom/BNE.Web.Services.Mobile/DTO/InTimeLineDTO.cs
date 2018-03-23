using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class InTimeLineDTO
    {
        [DataMember(Name = "c")]
        public int Id_Curriculo { get; set; }

        [DataMember(Name = "q")]
        public bool FiltroQuemMeViu { get; set; }

        [DataMember(Name = "j")]
        public bool FiltroJaEnviei { get; set; }

        [DataMember(Name = "m")]
        public bool FiltroMensagens { get; set; }

        [DataMember(Name = "v")]
        public bool FiltroVagas { get; set; }

        [DataMember(Name = "d")]
        public string DataUltimaAtualizacao { get; set; }

        [DataMember(Name = "p")]
        public int Pagina { get; set; }
        
    }
}
