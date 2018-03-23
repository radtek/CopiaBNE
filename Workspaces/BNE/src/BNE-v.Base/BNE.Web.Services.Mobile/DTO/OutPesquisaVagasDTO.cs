using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutPesquisaVagasDTO
    {
        [DataMember(Name = "v")]
        public List<VagaDTO> ListaDeVagas { get; private set; }

        [DataMember(Name = "p")]
        public bool Paginas { get; set; }

        public OutPesquisaVagasDTO()
        {
            ListaDeVagas = new List<VagaDTO>();
        }
    }
}
