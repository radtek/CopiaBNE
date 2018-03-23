using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.DTO
{
    public class Experiencia
    {
        public int idExperienciaProfissional { get; set; }
        public int? idAreaBNE { get; set; }
        public string Razao { get; set; }
        public string DesAtividades { get; set; }
        public string funcaoEmpresa { get; set; }
        public DateTime dataAdmissao { get; set; }
        public DateTime? dataDemissao { get; set; }

    }
}
