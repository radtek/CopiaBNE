using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract(Name = "Mensagem", Namespace = "BNE")]
    public class TimelineMensagemDTO : TimeLineEvent
    {
        [DataMember(Name = "d")]
        public string Data_Envio { get { return DataHora.ToString("s"); } set { } }

        [DataMember(Name = "a")]
        public string Assunto { get; set; }

        [DataMember(Name = "m")]
        public string Mensagem { get; set; }

        [DataMember(Name = "t")]
        public int Tipo { get; set; }

    }
}