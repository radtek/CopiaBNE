using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class DeficienciaDTO
    {
        [DataMember(Name = "d")]
        public string DescricaoDeficiencia { get; set; }
    }
}