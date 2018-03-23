using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.GatewayV2.Security
{
    public class UserCredentials
    {
        public decimal CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public decimal? CNPJ { get; set; }
        public Guid? Sistema { get; set; }
    }
}