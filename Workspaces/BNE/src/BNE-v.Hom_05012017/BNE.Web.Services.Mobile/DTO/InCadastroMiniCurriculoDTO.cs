using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class InCadastroMiniCurriculoDTO
    {
        [DataMember(Name = "c")]
        public string Cpf { get; set; }

        [DataMember(Name = "dn")]
        public string Nascimento { get; set; }

        [DataMember(Name = "cv")]
        public int? Id_Curriculo { get; set; }

        [DataMember(Name = "n")]
        public string Nome { get; set; }

        [DataMember(Name = "s")]
        public int Sexo { get; set; }

        [DataMember(Name = "p")]
        public string Celular { get; set; }

        [DataMember(Name = "f")]
        public string Funcao { get; set; }

        [DataMember(Name = "ft")]
        public int TempoExperiencia { get; set; }

        [DataMember(Name = "ps")]
        public string PretensaoSalarial { get; set; }

        [DataMember(Name = "ci")]
        public string Cidade { get; set; }

        [DataMember(Name = "fb")]
        public string CodUserFacebook { get; set; }

        [DataMember(Name = "e")]
        public string Email { get; set; }
    }
}
