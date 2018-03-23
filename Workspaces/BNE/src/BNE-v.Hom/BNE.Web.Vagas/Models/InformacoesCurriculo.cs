using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BNE.Web.Vagas.Models
{
    public class InformacoesCurriculo
    {
        public bool EhVip { get; set; }
        public int idCurriculo { get; set; }
        public bool EmpresaBloqueada { get; set; }
        public bool JaEnvioCvParaVaga { get; set; }
        public bool EstaNaRegiaoBH { get; set; }
        public bool TemExperienciaProfissional { get; set; }
        public bool TemFormacao { get; set; }
        public int SaldoCandidatura { get; set; }
        public bool DisseQueNaoTemExperiencia { get; set; }
        public DateTime DataNaoTemExperiencia { get; set; }
    }
}