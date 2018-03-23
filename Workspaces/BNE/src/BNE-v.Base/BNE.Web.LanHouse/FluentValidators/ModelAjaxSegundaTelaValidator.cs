using System;
using System.Globalization;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelAjaxSegundaTelaValidator : AbstractValidator<BNE.Web.LanHouse.Models.ModelAjaxSegundaTela>
    {
        public ModelAjaxSegundaTelaValidator()
        {
            RuleFor(x => x.NomeCompleto)
                .NotEmpty()
                .Must(SerNomeCompleto)
                .WithName("Nome Completo")
                .WithMessage("Digite um <b>nome completo válido</b>");

            RuleFor(x => x.DataNasc)
                .NotEmpty()
                .Length(10)
                .Must(SerDataValida)
                .WithName("Data de nascimento")
                .WithMessage("<b>Data de nascimento inválida</b>, favor corrigir");

            RuleFor(x => x.Sexo)
                .NotEmpty()
                .InclusiveBetween(1, 2)
                .WithName("Sexo")
                .WithMessage("<b>Código do sexo incorreto</b>, deve ser 1 ou 2");

            RuleFor(x => x.DDD)
                .NotEmpty()
                .Must(SerDDDValido)
                .WithName("DDD")
                .WithMessage("<b>DDD inválido</b>, favor corrigir");

            RuleFor(x => x.NumCelular)
                .NotEmpty()
                .Must(SerNumCelularValido)
                .WithName("Número de celular")
                .WithMessage("Digite um <b>número de celular válido</b>, por favor");
        }

        
        public bool SerDDDValido(string ddd)
        {
            return Regex.IsMatch(ddd, @"^\d{2}$");
        }

        public bool SerNumCelularValido(string numCelular)
        {
            return Regex.IsMatch(numCelular, @"^\d{4,5}-\d{4}$");
        }

        private bool SerDataValida(string data)
        {
            DateTime d;
            return DateTime.TryParse(data, CultureInfo.GetCultureInfo("pt-br"), DateTimeStyles.AssumeLocal, out d);
        }

        public bool SerNomeCompleto(string nomeCompleto)
        {
            return Regex.IsMatch(nomeCompleto, @"^[\D]+\s([\D]+\s?)+$");
        }

    }
}