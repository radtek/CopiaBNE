using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile.DTO.BNEEnvia
{
    [DataContract]
    public class EntradaConfirmaCampanhaEnviada
    {
        [DataMember(Name = "i")]
        public int IdCampanha { get; set; }
    }
}