using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace LanHouse.Business.DTO
{
    public class Empresa
    {
        public int id { get; set; }
        public string nomeFantasia { get; set; }
        public string apresentacao { get; set; }
        public string emailResponsavel { get; set; }
        public double? distance { get; set; }
        public byte[] logoEmpresa { get; set; }
        public decimal? cnpj { get; set; }

        public IList<Vaga> vagas { get; set; }
    }
}