using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile.DTO.BNEEnvia
{
    [DataContract]
    public class EntradaValidaIMEI
    {
        [DataMember(Name = "t")]
        public string TokenGCM { get; set; }

        [DataMember(Name = "i")]
        public string IMEI { get; set; }
    }
}