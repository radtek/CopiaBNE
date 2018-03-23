using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO.Empresas
{
    public class Selecionadora
    {
        public Selecionadora(IDataReader dr)
        {
            IdPessoa = Convert.ToInt32(dr["Idf_Pessoa_Fisica"]);
            Cpf = Convert.ToDecimal(dr["Num_CPF"]);
            Nome = dr["Nme_Pessoa"].ToString();
            Sexo = Convert.ToInt32(dr["Idf_Sexo"]) == 1 ? 'M' : 'F';
            Nascimento = Convert.ToDateTime(dr["Dta_Nascimento"]).ToString("yyyy-MM-dd");
            DDDCelular = dr.IsDBNull(dr.GetOrdinal("DDDCelularPessoa"))
                    ? null
                    : (int?)Convert.ToInt32(dr["DDDCelularPessoa"]);
            NumCelular = dr.IsDBNull(dr.GetOrdinal("NumCelularPessoa"))
                    ? null
                    : (int?)Convert.ToInt32(dr["NumCelularPessoa"]);
            DDDTelefone = dr.IsDBNull(dr.GetOrdinal("DDDTelefonePessoa"))
                    ? null
                    : (int?)Convert.ToInt32(dr["DDDTelefonePessoa"]);
            NumTelefone = dr.IsDBNull(dr.GetOrdinal("NumTelefonePessoa"))
                    ? null
                    : (int?)Convert.ToInt32(dr["NumTelefonePessoa"]);
            Email = dr.IsDBNull(dr.GetOrdinal("EmailPessoa"))
                    ? null
                    : dr.GetString(dr.GetOrdinal("EmailPessoa"));

            Empresas = new List<DTO.Empresas.EmpresaSelecionadora>();
        }

        public int IdPessoa { get; set; }
        public decimal Cpf { get; set; }
        public string Nome { get; set; }
        public char Sexo { get; set; }
        public string Nascimento { get; set; }
        public int? DDDCelular { get; set; }
        public int? NumCelular { get; set; }
        public int? DDDTelefone { get; set; }
        public int? NumTelefone { get; set; }
        public string Email { get; set; }
        public List<Empresas.EmpresaSelecionadora> Empresas { get; set; }
    }
}