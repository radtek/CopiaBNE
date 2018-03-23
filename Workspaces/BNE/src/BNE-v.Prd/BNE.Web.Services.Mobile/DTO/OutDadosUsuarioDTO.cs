using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Mobile
{
    [DataContract]
    public class OutDadosUsuarioDTO
    {
        [DataMember(Name = "v")]
        public bool FlagVip { get; set; }

        [DataMember(Name = "q")]
        public int QtdeEmpresasQuemMeViu { get; set; }

        [DataMember(Name="m")]
        public int Mensagens { get; set; }

        [DataMember(Name = "c")]
        public string Cpf { get; set; }

        [DataMember(Name = "dn")]
        public string Nascimento { get; set; }

        [DataMember(Name = "n")]
        public string Nome { get; set; }

        [DataMember(Name = "s")]
        public int Sexo { get; set; }

        [DataMember(Name = "p")]
        public string Celular { get; set; }

        [DataMember(Name = "f")]
        public string Funcao { get; set; }

        [DataMember(Name = "ps")]
        public string PretensaoSalarial { get; set; }

        [DataMember(Name = "ci")]
        public string Cidade { get; set; }

        [DataMember(Name = "e")]
        public string Email { get; set; }

        [DataMember(Name = "qca")]
        public bool Candidaturas { get; set; }

        [DataMember(Name = "qc")]
        public int QuantidadeCandidaturas { get; set; }

        [DataMember(Name = "pm")]
        public int IdPlanoMensal { get; set; }

        [DataMember(Name = "pmd")]
        public String VlrPlanoMensalSemDesconto { get; set; }

        [DataMember(Name = "pt")]
        public int IdPlanoTri { get; set; }

        [DataMember(Name = "ptd")]
        public String VlrPlanoTrimestralSemDesconto { get; set; }

        [DataMember(Name = "fo")]
        public String Foto { get; set; }
    }
}
