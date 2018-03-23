using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LanHouse.Business.DTO
{
    public class ResultadoPesquisaSalarial
    {
        [XmlElement("PesquisaSalarial")]
        public List<PesquisaSalarial> pesquisaSalarialList = new List<PesquisaSalarial>();
    }

    public class PesquisaSalarial
    {
        public int Idf_Nivel_Experiencia { get; set; }
        public float Media { get; set; }
        public int Qtde { get; set; }
        public int Idf_Tipo_Porte { get; set; }
    }
}
