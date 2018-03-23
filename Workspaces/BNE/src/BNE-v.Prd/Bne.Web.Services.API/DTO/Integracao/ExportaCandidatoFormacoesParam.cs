using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Integracao
{
    public class ExportaCandidatoFormacoesParam
    {

        public int CodigoEscolaridade { get; set; }

        public string NomeInstituicao { get; set; }

        public string TituloCurso { get; set; }

        public short? Conclusao { get; set; }

        public string Cidade { get; set; }

    }
}