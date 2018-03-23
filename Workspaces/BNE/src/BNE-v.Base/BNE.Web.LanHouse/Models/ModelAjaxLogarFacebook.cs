using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelAjaxLogarFacebookValidator))]
    public class ModelAjaxLogarFacebook
    {
        [BindAlias("l")]
        public string Login { get; set; }
    }
}