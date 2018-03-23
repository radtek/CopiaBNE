using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile.DTO.BNEEnvia
{
    [DataContract]
    public class Telefone
    {
        [DataMember(Name = "n")]
        public string Nome { get; set; }
        
        [DataMember(Name = "t")]
        public string NumeroTelefone { get; set; }
    }
}