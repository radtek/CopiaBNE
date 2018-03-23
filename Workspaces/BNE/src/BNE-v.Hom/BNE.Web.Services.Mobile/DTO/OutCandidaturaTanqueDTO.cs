using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutCandidaturaTanqueDTO
    {
        [DataMember(Name = "s")]
        public int Status { get; set; }

        [DataMember(Name = "e")]
        public string erro { get; set; }

        [DataMember(Name= "l")]
        public string linkVaga{get;set;}
    }
}