using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutDetalhesVagaDTO
    {
        [DataMember(Name = "d")]
        public string DataPublicacao { get; set; }

        [DataMember(Name = "a")]
        public string Atribuicoes { get; set; }

        [DataMember(Name = "b")]
        public string Beneficios { get; set; }

        [DataMember(Name = "r")]
        public string Requisitos { get; set; }

        [DataMember(Name = "q")]
        public int QuantidadeDeVagas { get; set; }

        [DataMember(Name = "ci")]
        public string Cidade { get; set; }

        [DataMember(Name = "e")]
        public int Id_Empresa { get; set; }

        [DataMember(Name = "f")]
        public string Funcao { get; set; }

        [DataMember(Name = "smin")]
        public string SalarioMin { get; set; }

        [DataMember(Name = "smax")]
        public string SalarioMax { get; set; }

        [DataMember(Name = "s")]
        public int StatusVaga { get; set; }

        [DataMember(Name = "u")]
        public string Url { get; set; }

        [DataMember(Name = "p")]
        public int Perguntas { get; set; }
    }
}
