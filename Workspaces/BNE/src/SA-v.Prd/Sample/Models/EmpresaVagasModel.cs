using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.Models
{
    public class EmpresaVagasModel
    {
        public IEnumerable<AdminLTE_Application.VWVagas> vagas { get; set; }

        //Filtros
        public bool Ativa { get; set; }
        public bool Inativa { get; set; }
        public bool Oportunidade { get; set; }
        public string FlPeriodo { get; set; }
    }

}