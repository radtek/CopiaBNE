using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class InTokenGCMDTO
    {
        [DataMember(Name = "c")]
        public int Id_Curriculo { get; set; }

        [DataMember(Name = "t")]
        public string TokenGCM { get; set; }

        [DataMember(Name = "i")]
        public string IMEI { get; set; }
    }
}
