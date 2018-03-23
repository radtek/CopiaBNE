using System;
using System.Globalization;
using System.Text.RegularExpressions;
using FluentValidation;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelAjaxTerceiraTelaValidator : AbstractValidator<Models.ModelAjaxTerceiraTela>
    {
        public ModelAjaxTerceiraTelaValidator()
        {
            RuleFor(x => x.Cpf)
                .NotEmpty()
                .Length(14)
                .Must(SerCpfValido)
                .WithName("CPF")
                .WithMessage("<b>O CPF informado é inválido</b>, favor corrigir");

            RuleFor(x => x.Email)
                .Length(0, 100)
                .Must(SerEmailValidoOuVazio)
                .WithName("E-mail")
                .WithMessage("<b>E-mail inválido</b>, favor corrigir");

            RuleFor(x => x.Cargo)
                .NotEmpty()
                .Must(SerTextoValido)
                .WithName("Cargo")
                .WithMessage("<b>Cargo inválido</b>, favor corrigir");

            var salarioMinimo = BLL.Parametro.SalarioMinimo();

            RuleFor(x => x.Salario)
                .NotEmpty()
                .Must((salario, minimo) => SerSalarioValido(salario.Salario, salarioMinimo))
                .WithName("Salário")
                .WithMessage(string.Format("<b>Salário inválido</b>, deve ser numérico e maior que {0}, favor corrigir", salarioMinimo.ToString("C", CultureInfo.GetCultureInfo("pt-br"))));
        }

        private bool SerSalarioValido(string salario, decimal salarioMinimo)
        {
            decimal d;
            return Decimal.TryParse(salario, NumberStyles.Number, CultureInfo.GetCultureInfo("pt-br"), out d) && (d - salarioMinimo) >= 0.00m;
        }

        private bool SerTextoValido(string texto)
        {
            return Regex.IsMatch(texto, @"^(\w+\s?)+$");
        }

        private bool SerEmailValidoOuVazio(string email)
        {
            return String.IsNullOrEmpty(email) || Regex.IsMatch(email, @"^[_a-z0-9-]+(\.[_a-z0-9-]+)*@[a-z0-9-]+(\.[a-z0-9-]+)*(\.[a-z]{2,4})$");
        }

        private bool SerCpfValido(string cpf)
        {
            return Regex.IsMatch(cpf, @"^\d{3}\.\d{3}\.\d{3}-\d{2}$") && Code.Helper.VerificarDigitoVerificadorCpf(cpf);
        }

    }
}