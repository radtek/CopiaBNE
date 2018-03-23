using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelHomeIndexValidator))]
    public class ModelHomeIndex
    {
        [BindAlias("t")]
        public string TermoBusca { get; set; }

        public decimal ValorVIP { get; set; }

    }
}