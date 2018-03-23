using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using FluentValidation;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelAjaxCnpjClicadoValidator : AbstractValidator<BNE.Web.LanHouse.Models.ModelAjaxCnpjClicado>
    {
        public ModelAjaxCnpjClicadoValidator()
        {
            RuleFor(x => x.Cnpj)
                .NotEmpty()
                .Must(SerCnpjValido)
                .WithName("CNPJ clicado")
                .WithName("O CNPJ da empresa clicada não é válido");
        }

        private bool SerCnpjValido(string cnpj)
        {
            return !String.IsNullOrEmpty(cnpj) && Regex.IsMatch(cnpj, @"\d{14}");
        }
    }
}