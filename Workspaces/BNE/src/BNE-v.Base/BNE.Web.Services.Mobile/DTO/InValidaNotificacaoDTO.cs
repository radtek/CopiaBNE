using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class InValidaNotificacaoDTO
    {
        [DataMember(Name = "n")]
        public int Id_Notificacao { get; set; }

        [DataMember(Name = "c")]
        public int Id_Curriculo { get; set; }

        [DataMember(Name = "e")]
        public bool Entregue { get; set; }

        [DataMember(Name = "a")]
        public bool Aberta { get; set; }
    }
}
