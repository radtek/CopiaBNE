using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelAjaxBuscarFilialValidator : AbstractValidator<BNE.Web.LanHouse.Models.ModelAjaxBuscarFilial>
    {
        public ModelAjaxBuscarFilialValidator()
        {
            RuleFor(x => x.TermoBusca)
                .Length(0, 100)
                .Must(SerVazioOuAlfanumerico)
                .WithName("Termo de busca")
                .WithMessage("O termo de busca deve ser composto de letras ou números, no máximo 100 caracteres");

            RuleFor(x => x.Count)
                .InclusiveBetween(1, 30)
                .NotEmpty()
                .WithName("Número de itens")
                .WithMessage("O número de itens deve estar entre 1 e 30");

            RuleFor(x => x.Index)
                .GreaterThanOrEqualTo(0)
                .NotEmpty()
                .WithName("Índice")
                .WithMessage("O índice deve ser não-negativo");
        }

        private bool SerVazioOuAlfanumerico(string termoBusca)
        {
            return Regex.IsMatch(termoBusca ?? String.Empty, @"\w*");
        }
    }
}