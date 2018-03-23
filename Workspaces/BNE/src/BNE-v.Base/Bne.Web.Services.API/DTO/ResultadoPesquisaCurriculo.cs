using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    public class ResultadoPesquisaCurriculo
    {
        public int TotalDeRegistros;
        public int RegistrosPorPagina;
        public List<DTO.MiniCurriculo> Curriculos;
    }
}