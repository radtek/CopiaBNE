using FluentValidation;
using System.Text.RegularExpressions;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelAjaxLogarCelularValidator : AbstractValidator<BNE.Web.LanHouse.Models.ModelAjaxLogarCelular>
    {
        public ModelAjaxLogarCelularValidator()
        {
            RuleFor(x => x.DDD)
                .NotNull()
                .Must(SerDDDValido)
                .WithName("DDD")
                .WithMessage("<b>DDD inválido</b>, favor corrigir");

            RuleFor(x => x.NumCelular)
                .NotNull()
                .Must(SerNumCelularValido)
                .WithName("Número de celular")
                .WithMessage("<b>Número de celular inválido</b>, favor corrigir");
        }

        private bool SerDDDValido(string ddd)
        {
            return Regex.IsMatch(ddd, @"^\d{2}$");
        }

        private bool SerNumCelularValido(string numCelular)
        {
            return Regex.IsMatch(numCelular, @"^\d{4,5}-\d{4}$");
        }
    }
}