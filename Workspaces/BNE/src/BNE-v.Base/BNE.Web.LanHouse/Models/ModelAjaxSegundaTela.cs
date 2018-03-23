using System;
using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelAjaxSegundaTelaValidator))]
    [Serializable]
    public class ModelAjaxSegundaTela
    {

        [BindAlias("n")]
        public string NomeCompleto { get; set; }

        [BindAlias("dn")]
        public string DataNasc { get; set; }

        [BindAlias("s")]
        public int Sexo { get; set; }

        [BindAlias("d")]
        public string DDD { get; set; }

        [BindAlias("m")]
        public string NumCelular { get; set; }

        [BindAlias("cv")]
        public string CodigoValidacaoCelular { get; set; }

    }
}