using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract(Name = "Vaga", Namespace = "BNE")]
    public class TimelineVagaDTO : TimeLineEvent
    {
        [DataMember(Name = "d")]
        public string Data_Publicacao { get { return DataHora.ToString("s"); } set { } }

        [DataMember(Name = "v")]
        public int Id_Vaga { get; set; }

        [DataMember(Name = "f")]
        public string Funcao { get; set; }

        [DataMember(Name = "ci")]
        public string Cidade { get; set; }

        [DataMember(Name = "smin")]
        public string SalarioMin { get; set; }

        [DataMember(Name = "smax")]
        public string SalarioMax { get; set; }

        [DataMember(Name = "q")]
        public int QuantidadeDeVagas { get; set; }

        [DataMember(Name = "s")]
        public int Status { get; set; }

        [DataMember(Name = "de")]
        public string Descricao { get; set; }

        [DataMember(Name = "u")]
        public string Url { get; set; }

        [DataMember(Name = "p")]
        public int Perguntas { get; set; }

        [DataMember(Name = "op")]
        public bool Arquivada { get; set; }

        [DataMember(Name = "pcd")]
        public bool PCD { get; set; }

    }
}