using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract(Name = "QuemMeViu", Namespace = "BNE")]
    public class TimelineQuemMeViuDTO : TimeLineEvent
    {
        [DataMember(Name = "d")]
        public string Data_QuemMeViu { get { return DataHora.ToString("s"); } set { } }

        [DataMember(Name = "e")]
        public int Id_Empresa { get; set; }

        [DataMember(Name = "n")]
        public string NomeEmpresa { get; set; }

        [DataMember(Name = "ci")]
        public string Cidade { get; set; }

    }
}