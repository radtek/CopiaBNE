using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class InDetalhesEmpresaDTO
    {
        [DataMember(Name = "e")]
        public int Id_Empresa { get; set; }

        [DataMember(Name = "c")]
        public int Id_Curriculo { get; set; }

        [DataMember(Name = "v")]
        public int? Id_Vaga { get; set; }
    }
}
