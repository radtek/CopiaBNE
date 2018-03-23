using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class VagaDTO
    {
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
        public int StatusVaga { get; set; }

        [DataMember(Name = "a")]
        public string Atribuicoes { get; set; }

        [DataMember(Name = "u")]
        public string Url { get; set; }

        [DataMember(Name = "p")]
        public int Perguntas { get; set; }

    }
}
