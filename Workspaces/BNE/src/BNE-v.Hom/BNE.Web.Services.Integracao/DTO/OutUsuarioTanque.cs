using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BNE.Web.Services.Integracao.DTO
{
    [DataContract]
    public class OutUsuarioTanque
    {
        [DataMember(Name = "l")]
        public List<UsuarioDTO> listaUsuarioFilialPerfil { get; set; }

        public OutUsuarioTanque()
        {
            listaUsuarioFilialPerfil = new List<UsuarioDTO>();
        }
    }
    [DataContract]
    public class UsuarioDTO
    {
        [DataMember(Name = "i")]
        public int idfUsuarioFilialPerfil{get; set;}

        [DataMember(Name = "n")]
        public string nome { get; set; }

        [DataMember(Name = "e")]
        public string nomeEmpresa { get; set; }

        [DataMember(Name = "c")]
        public string cnpj { get; set; }

        [DataMember(Name = "t")]
        public string telefone { get; set; }

    }
}