using System.Runtime.Serialization;

namespace BNE.Web.Services
{
    [DataContract]
    public class InCampanhaTanqueDTO
    { 
        [DataMember(Name ="v")]
        public int IdVaga { get; set; }
    }
}