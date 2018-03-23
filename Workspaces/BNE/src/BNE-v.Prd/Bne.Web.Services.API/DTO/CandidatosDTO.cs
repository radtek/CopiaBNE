using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    public class CandidatosDTO
    {
        public int CodigoVaga { get; set; }
        public int Pagina { get; set; }
        public List<Curriculo> Curriculos { get; set; }
    }
}