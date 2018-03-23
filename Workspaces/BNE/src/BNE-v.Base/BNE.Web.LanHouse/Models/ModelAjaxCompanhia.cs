using System;
using BNE.Web.LanHouse.Code;

namespace BNE.Web.LanHouse.Models
{
    [Serializable]
    public class ModelAjaxCompanhia
    {

        [BindAlias("i")]
        public int IdCompanhia { get; set; }
        [BindAlias("l")]
        public string Logo { get; set; }
        [BindAlias("n")]
        public string NomeCompanhia { get; set; }
        [BindAlias("c")]
        public decimal NumeroCNPJ { get; set; }

        /*
        
        [BindAlias("di")]
        public DateTime DataInicioVigencia { get; set; }
        [BindAlias("df")]
        public DateTime? DataFimVigencia { get; set; }
        */

    }
}