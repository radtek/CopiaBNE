using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelAjaxBuscarFilialValidator))]
    public class ModelAjaxBuscarFilial
    {
        [BindAlias("t")]
        public string TermoBusca { get; set; }

        [BindAlias("i")]
        public int Index { get; set; }

        [BindAlias("c")]
        public int Count { get; set; }
    }
}