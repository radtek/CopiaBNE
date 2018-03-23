using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class InLoginDTO
    {
        [DataMember(Name = "c")]
        public string Cpf { get; set; }

        [DataMember(Name = "dn")]
        public string Data_Nascimento { get; set; }

        [DataMember(Name = "f")]
        public string UserCodFacebook { get; set; }
    }
}
