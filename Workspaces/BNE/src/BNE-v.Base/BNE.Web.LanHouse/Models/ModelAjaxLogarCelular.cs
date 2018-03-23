using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BNE.Web.LanHouse.Code;
using FluentValidation.Attributes;

namespace BNE.Web.LanHouse.Models
{
    [Validator(typeof(FluentValidators.ModelAjaxLogarCelularValidator))]
    public class ModelAjaxLogarCelular
    {
        [BindAlias("d")]
        public string DDD { get; set; }

        [BindAlias("n")]
        public string NumCelular { get; set; }
    }
}