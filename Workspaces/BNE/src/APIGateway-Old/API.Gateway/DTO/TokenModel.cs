using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Gateway.DTO
{
    public class TokenModel
    {
        [Required]
        public decimal? Num_CPF { get; set; }
        public int? Idf_Filial { get; set; }
        public string Senha { get; set; }
        [Required]
        public DateTime? Dta_Nascimento { get; set; }
        [Required]
        public string Idf_Cliente { get; set; }
    }
}