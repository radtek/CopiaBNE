using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Vagas.Models
{
    public class AlertaVaga
    {
        public string Cidade { get; set; }
        public string Funcao { get; set; }
        public string Uf { get; set; }
        public int IdCidade { get; set; }
        public int IdFuncao { get; set; }
    }
}