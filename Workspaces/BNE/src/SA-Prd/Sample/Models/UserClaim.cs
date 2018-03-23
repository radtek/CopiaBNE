using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdminLTE_Application.Models
{
    public class UserClaim
    {
        
        public string cpf { get; set; }

        public string nome { get; set; }

        public int tipoVendedor { get; set; }

    }
}