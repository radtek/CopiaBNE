using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutTimeLineDTO
    {
        [DataMember(Name = "s")]
        public int Status { get; set; }

        [DataMember(Name = "e")]
        public List<TimeLineEvent> ListaDeEventos { get; set; }

        public OutTimeLineDTO()
        {
            ListaDeEventos = new List<TimeLineEvent>();
        }
    }
}
