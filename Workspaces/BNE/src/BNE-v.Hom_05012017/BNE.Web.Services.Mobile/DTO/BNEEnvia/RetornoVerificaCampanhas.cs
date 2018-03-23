using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile.DTO.BNEEnvia
{
    [DataContract]
    public class RetornoVerificaCampanhas
    {
        [DataMember(Name = "i")]
        public int IdCampanha { get; set; }

        [DataMember(Name = "n")]
        public string nome { get; set; }

        [DataMember(Name = "m")]
        public string Mensagem { get; set; }

        [DataMember(Name = "t")]
        public List<Telefone> Telefones { get; set; }

    }
}