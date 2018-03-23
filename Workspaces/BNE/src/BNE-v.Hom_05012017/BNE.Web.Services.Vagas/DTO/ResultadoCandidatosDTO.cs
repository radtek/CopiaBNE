using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Services.Vagas.DTO
{
    public class ResultadoCandidatosDTO
    {
        public int TotalRegistros {get; set;}
        public int TotalPaginas { get; set; }
        public int Pagina { get; set; }
        public List<CurriculoCurtoDTO> Curriculos { get; set; }
    }
}