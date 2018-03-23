using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    [KnownType(typeof(TimelineJaEnvieiDTO))]
    [KnownType(typeof(TimelineMensagemDTO))]
    [KnownType(typeof(TimelineQuemMeViuDTO))]
    [KnownType(typeof(TimelineVagaDTO))]
    public abstract class TimeLineEvent
    {
        public DateTime DataHora { get; set; }
    }
}