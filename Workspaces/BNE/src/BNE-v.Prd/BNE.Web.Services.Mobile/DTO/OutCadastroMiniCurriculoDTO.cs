using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutCadastroMiniCurriculoDTO
    {
        [DataMember(Name = "s")]
        public int Status { get; set; }

        [DataMember(Name = "c")]
        public int Id_Curriculo { get; set; }

        [DataMember(Name = "m")]
        public string Mensagem { get; set; }
    }
}
