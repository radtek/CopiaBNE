using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutStatusDTO
    {
        [DataMember(Name = "s")]
        public int Status { get; set; }

        [DataMember(Name = "e")]
        public string erro { get; set; }
    }
}