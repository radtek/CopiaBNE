using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    public class Endereco
    {
        private IDataReader dr;

        public Endereco(IDataReader dr)
        {
            CEP = Convert.ToDecimal(dr["Num_CEP"]);
            Logradouro = dr["Des_Logradouro"].ToString();
            Numero = dr["Num_Endereco"].ToString();
            Bairro = dr["Des_Bairro"].ToString();
            Cidade = dr["Nme_Cidade"].ToString();
            UF = dr["Sig_Estado"].ToString();
        }

        public decimal CEP { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
    }
}