using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutPerguntasVagaDTO
    {
        [DataMember(Name = "s")]
        public int Status { get; set; }

        [DataMember(Name = "p")]
        public List<VagaPerguntaDTO> Perguntas { get; private set; }

        public OutPerguntasVagaDTO()
        {
            Perguntas = new List<VagaPerguntaDTO>();
        }
    }
}
