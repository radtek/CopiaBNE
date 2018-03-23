using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Empresas
{
    public class EmpresaSelecionadora
    {
        public EmpresaSelecionadora(IDataReader dr)
        {
            CNPJ = Convert.ToDecimal(dr["Num_CNPJ"]);
            RazaoSocial = dr.IsDBNull(dr.GetOrdinal("Raz_Social"))
                    ? null
                    : dr.GetString(dr.GetOrdinal("Raz_Social"));
            NomeFantasia = dr.IsDBNull(dr.GetOrdinal("Nme_Fantasia"))
                    ? null
                    : dr.GetString(dr.GetOrdinal("Nme_Fantasia"));
            Matriz = Convert.ToBoolean(dr["Flg_Matriz"]);
            Site = dr.IsDBNull(dr.GetOrdinal("End_Site"))
                    ? null
                    : dr.GetString(dr.GetOrdinal("End_Site"));
            DDDTelefone = dr.IsDBNull(dr.GetOrdinal("DDDTelefoneEmpresa"))
                    ? null
                    : (int?)Convert.ToInt32(dr["DDDTelefoneEmpresa"]);
            NumTelefone = dr.IsDBNull(dr.GetOrdinal("NumTelefoneEmpresa"))
                    ? null
                    : (int?)Convert.ToInt32(dr["NumTelefoneEmpresa"]);

            Endereco = new Endereco(dr);
        }

        public decimal CNPJ { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public bool Matriz { get; set; }
        public string Site { get; set; }
        public int? DDDTelefone { get; set; }
        public int? NumTelefone { get; set; }
        public Endereco Endereco { get; set; }
    }
}