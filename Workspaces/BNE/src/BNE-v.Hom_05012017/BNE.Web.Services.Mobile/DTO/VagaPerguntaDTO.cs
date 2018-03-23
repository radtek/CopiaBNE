using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class VagaPerguntaDTO
    {
        [DataMember(Name = "vp")]
        public int IdfVagaPergunta { get; set; }

        [DataMember(Name = "d")]
        public string DescricaoPergunta { get; set; }
    }
}
