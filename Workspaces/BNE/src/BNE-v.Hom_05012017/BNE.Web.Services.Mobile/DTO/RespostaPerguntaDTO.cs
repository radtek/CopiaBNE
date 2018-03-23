using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class RespostaPerguntaDTO
    {
        [DataMember(Name = "vp")]
        public int IdfVagaPergunta { get; set; }

        [DataMember(Name = "r")]
        public bool RespostaPergunta { get; set; }
    }
}
