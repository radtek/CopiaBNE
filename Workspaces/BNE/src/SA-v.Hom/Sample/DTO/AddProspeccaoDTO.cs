using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sample.DTO
{
    public class AddProspeccaoDTO
    {
        public string Num_CNPJ { get; set; }
        public int Idf_Situacao_Atendimento { get; set; }
        public string msg { get; set; }
    }
}