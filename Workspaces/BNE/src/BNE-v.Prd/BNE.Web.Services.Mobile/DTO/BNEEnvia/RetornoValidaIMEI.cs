using System.Runtime.Serialization;
using System;

namespace BNE.Web.Services.Mobile.DTO.BNEEnvia
{
    [DataContract]
    public class RetornoValidaIMEI
    {
        [DataMember(Name = "n")]
        public string Nome { get; set; }

        [DataMember(Name = "s")]
        public int Status { get; set; }

        [DataMember(Name = "d")]
        public string DataAtual { get; set; }

        [DataMember(Name = "c")]
        public int Cota { get; set; }

        [DataMember(Name = "e")]
        public bool EnviaCopia { get; set; }

        [DataMember(Name = "v")]
        public String Versao { get; set; }
    }
}