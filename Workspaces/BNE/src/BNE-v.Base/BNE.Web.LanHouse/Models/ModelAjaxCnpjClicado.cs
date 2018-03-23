using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelAjaxCnpjClicadoValidator))]
    [Serializable]
    public class ModelAjaxCnpjClicado
    {
        [BindAlias("c")]
        public string Cnpj { get; set; }
    }
}