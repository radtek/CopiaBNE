using BNE.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Services.Vagas.DTO
{
    public class CandidatosDTO
    {
        public int CodigoVaga { get; set; }
        public int Pagina { get; set; }
        public List<MiniCurriculo> MiniCurriculos { get; set; }
    }
}