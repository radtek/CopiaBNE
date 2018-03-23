using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Gateway.DTO
{
    public class RemoverModel
    {
        [Required]
        public decimal? Num_CPF { get; set; }

        public int? Idf_Filial { get; set; }

        [Required]
        public string keytoken { get; set; }
    }
}