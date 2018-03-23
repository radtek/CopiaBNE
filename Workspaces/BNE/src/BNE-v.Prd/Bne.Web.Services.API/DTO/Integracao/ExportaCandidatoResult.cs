using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Integracao
{
    public class ExportaCandidatoResult
    {
        public string Message { get; set; }
        public int? Id { get; set; }
        public bool ExistePessoaFisica { get; set; }
    }
}