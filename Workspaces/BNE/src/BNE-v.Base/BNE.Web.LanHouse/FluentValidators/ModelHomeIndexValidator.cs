using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelHomeIndexValidator : AbstractValidator<BNE.Web.LanHouse.Models.ModelHomeIndex>
    {
        public ModelHomeIndexValidator()
        {
            RuleFor(x => x.TermoBusca)
                .Length(2, 100)
                .Must(SerAlfanumerico)
                .WithName("Termo de busca")
                .WithMessage("Digite letras ou números, de 2 até 100 caracteres");
        }

        public bool SerAlfanumerico(string termoBusca)
        {
            return Regex.IsMatch(termoBusca, @"\w*");
        }
    }
}