using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanHouse.Business.DTO
{
    public class Vaga
    {
        public int id { get; set; }
        public int idFuncao { get; set; }
        public int? idTipoVinculo { get; set; }
        public string funcao { get; set; }
        public string atribuicoes { get; set; }
        public string requisitos { get; set; }
        public string beneficios { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime? dataAbertura { get; set; }
        public string codigo { get; set; }
        public string cidade { get; set; }
        public string salario { get; set; }
        public string empresa { get; set; }
        public bool bneRecomenda { get; set; }

        public IList<DTO.SpellCheck> spellChecker {get;set;}
    }
}