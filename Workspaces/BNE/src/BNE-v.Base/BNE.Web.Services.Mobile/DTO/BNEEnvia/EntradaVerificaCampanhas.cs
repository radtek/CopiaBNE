using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile.DTO.BNEEnvia
{
    [DataContract]
    public class EntradaVerificaCampanhas
    {
        [DataMember(Name = "i")]
        public string IMEI { get; set; }

        [DataMember(Name = "c")]
        public int idCampanha { get; set; }
    }
}