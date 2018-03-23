using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract(Name = "JaEnviei", Namespace = "BNE")]
    public class TimelineJaEnvieiDTO : TimeLineEvent
    {
        [DataMember(Name = "d")]
        public string Data_Envio { get { return DataHora.ToString("s"); } set { } }

        [DataMember(Name = "v")]
        public int Id_Vaga { get; set; }

        [DataMember(Name = "f")]
        public string Funcao { get; set; }

        [DataMember(Name = "e")]
        public int Id_Empresa { get; set; }

        [DataMember(Name = "n")]
        public string NomeEmpresa { get; set; }

        [DataMember(Name = "s")]
        public string Salario { get; set; }
    }
}