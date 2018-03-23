using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelAjaxSextaTelaValidator))]
    public class ModelAjaxSextaTela
    {
        [BindAlias("e")]
        public int IdEstadoCivil { get; set; }
        [BindAlias("c")]
        public string Cep { get; set; }
        [BindAlias("t")]
        public string TelefoneRecadoFone { get; set; }
        [BindAlias("d")]
        public string TelefoneRecadoDDD { get; set; }
        [BindAlias("fc")]
        public string FalarCom { get; set; }
    }
}