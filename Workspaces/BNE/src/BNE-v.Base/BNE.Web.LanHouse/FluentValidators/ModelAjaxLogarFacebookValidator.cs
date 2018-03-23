using FluentValidation;
using System.Text.RegularExpressions;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelAjaxLogarFacebookValidator : AbstractValidator<BNE.Web.LanHouse.Models.ModelAjaxLogarFacebook>
    {
        public ModelAjaxLogarFacebookValidator()
        {
            RuleFor(x => x.Login)
                .NotEmpty()
                .Must(SerLoginValido)
                .WithName("Login")
                .WithMessage("Login inválido");
        }

        private bool SerLoginValido(string login)
        {
            return Regex.IsMatch(login, @"^*[0-9]{15}\s*$");
        }
    }
}