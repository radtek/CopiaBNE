using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelAjaxTerceiraTelaValidator))]
    public class ModelAjaxTerceiraTela
    {

        [BindAlias("c")]
        public string Cpf { get; set; }

        [BindAlias("e")]
        public string Email { get; set; }

        [BindAlias("g")]
        public string Cargo { get; set; }

        [BindAlias("s")]
        public string Salario { get; set; }

    }
}