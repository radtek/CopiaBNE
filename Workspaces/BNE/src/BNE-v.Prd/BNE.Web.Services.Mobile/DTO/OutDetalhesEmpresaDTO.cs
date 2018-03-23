using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutDetalhesEmpresaDTO
    {
        [DataMember(Name = "n")]
        public string Nome_Empresa { get; set; }

        [DataMember(Name = "q")]
        public int QuantidadeDeFuncionarios { get; set; }
        
        [DataMember(Name = "vd")]
        public int NumeroVagasDivulgadas { get; set; }

        [DataMember(Name = "ci")]
        public string Cidade { get; set; }

    }
}
