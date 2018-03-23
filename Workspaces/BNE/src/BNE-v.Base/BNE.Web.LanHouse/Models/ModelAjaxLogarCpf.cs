using System;
using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelAjaxLogarCpfValidator))]
    public class ModelAjaxLogarCpf
    {
        [BindAlias("c")]
        public string Cpf { get; set; }

        [BindAlias("d")]
        public DateTime DataNascimento { get; set; }
    }
}