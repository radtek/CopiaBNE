using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanHouse.Business.DTO
{
    public class IdiomaCandidato
    {
        public int idIdiomaCandidato { get; set; }
        public int idIdioma { get; set; }
        public string text { get; set; }
        public int nivel { get; set; }
        public string nivelTexto { get; set; }

    }
}
