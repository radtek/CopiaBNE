using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile.DTO.BNEEnvia
{
    [DataContract]
    public class RetornoConfirmaCampanhaEnviada
    {
        [DataMember(Name = "s")]
        public int Status { get; set; }
    }
}