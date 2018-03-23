using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BNE.BLL.DTO
{
    public class ExtratoVIP
    {
        public int VisualizacoesCurriculo { get; set; }
        public int VagasVisualizadas { get; set; }
        public int VagasNaoVisualizadas { get; set; }
        public int VagasPeloJornal { get; set; }
        public int ApareceuNasBuscas { get; set; }
        public int BuscaSeuPerfil { get; set; }
        public int VagasCandidatadas { get; set; }
        public int EmpresaBuscouSeuPerfil { get; set; }
        public int VagasNoPerfil { get; set; }
      
    }
}
