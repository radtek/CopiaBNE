using Bne.Web.Services.API.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Bne.Web.Services.API.DTO.CadastroCV
{
    public class CadastroMiniCurriculoIncompleto : CadastroMiniCurriculo
    {
        /// <summary>
        /// Data dde nascimento do candidato
        /// </summary>
        [DataMember]
        public new DateTime? DataNascimento { get; set; }

        /// <summary>
        /// DDD do celular do candidato
        /// </summary>
        [DataMember]
        public new short? DDDCelular { get; set; }

        /// <summary>
        /// Número do celular do candidato
        /// </summary>
        [DataMember]
        public new decimal? NumeroCelular { get; set; }

        /// <summary>
        /// Sexo do candidato
        /// </summary>
        [DataMember]
        public new Sexo? Sexo { get; set; }
    }
}