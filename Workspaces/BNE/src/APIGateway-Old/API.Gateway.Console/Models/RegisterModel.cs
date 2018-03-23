using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Gateway.Console.Models
{

    public class RegisterModel
    {
        public string Num_CPF { get; set; }
        public string Idf_Filial { get; set; }
        public string Senha { get; set; }
        public string Dta_Nascimento { get; set; }
        public int Idf_Perfil { get; set; }
    }

}