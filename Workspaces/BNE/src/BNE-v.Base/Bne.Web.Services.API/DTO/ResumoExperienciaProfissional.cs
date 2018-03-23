using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    public class ResumoExperienciaProfissional
    {
        public string razaoSocial { get; set; }
        public string descAreaBNE { get; set; }
        public string dtaAdmissao { get; set; }
        public string dtaDemissao { get; set; }
        public string descFuncao { get; set; }
        public string descAtividade { get; set; }
        public decimal vlrUltimoSalario { get; set; }
        public decimal vlrSalario { get; set; }
        public int ordem { get; set; }
    }
}