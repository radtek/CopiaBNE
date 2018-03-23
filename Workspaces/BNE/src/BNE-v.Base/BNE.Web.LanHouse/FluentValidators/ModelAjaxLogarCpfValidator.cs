using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BNE.Web.LanHouse.FluentValidators
{
    public class ModelAjaxLogarCpfValidator : AbstractValidator<BNE.Web.LanHouse.Models.ModelAjaxLogarCpf>
    {
        public ModelAjaxLogarCpfValidator()
        {
            RuleFor(x => x.Cpf)
                .Must(SerCpfValido)
                .WithName("CPF")
                .WithMessage("<b>CPF inválido</b>, favor corrigir");

            RuleFor(x => x.DataNascimento)
                .NotNull()
                .WithName("Data de nascimento")
                .WithMessage("<b>Data de nascimento não pode ser vazia</b>, favor corrigir");
        }

        private bool SerCpfValido(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            return Regex.IsMatch(cpf, @"^\d{3}\.\d{3}\.\d{3}-\d{2}$") && ValidarDigitoVerificador(cpf);
        }

        private bool ValidarDigitoVerificador(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            cpf = cpf.Replace(".", String.Empty).Replace("-", String.Empty);

            return Code.Helper.VerificarDigitoVerificadorCpf(cpf);
        }
    }
}