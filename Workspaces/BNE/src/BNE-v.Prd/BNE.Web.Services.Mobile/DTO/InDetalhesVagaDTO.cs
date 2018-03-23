using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class InDetalhesVagaDTO
    {
        [DataMember(Name = "v")]
        public int Id_Vaga { get; set; }

        [DataMember(Name = "c")]
        public int Id_Curriculo { get; set; }
    }
}