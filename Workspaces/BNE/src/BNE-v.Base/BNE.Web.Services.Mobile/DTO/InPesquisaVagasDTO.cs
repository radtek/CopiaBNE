using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class InPesquisaVagasDTO
    {
        [DataMember(Name = "c")]
        public int? Id_Curriculo { get; set; }

        [DataMember(Name = "ci")]
        public string Cidade { get; set; }

        [DataMember(Name = "f")]
        public string Funcao { get; set; }

        [DataMember(Name = "e")]
        public int? Id_Empresa { get; set; }

        [DataMember(Name = "p")]
        public int? Pagina { get; set; }
    }
}
