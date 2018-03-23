using System;
using System.Data;

namespace BNE.BLL.Notificacao.DTO
{
    public class Curriculo
    {
        public int IdCurriculo { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Funcao { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public int QuantidadeQuemMeViu15Dias { get; set; }
        public int QuantidadeQuemMeViu30Dias { get; set; }
        public bool VIP { get; set; }
        public decimal CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public void ReadFromDataReader(IDataReader dr)
        {
            this.IdCurriculo = Convert.ToInt32(dr["Idf_Curriculo"]);
            this.Nome = dr["Nme_Pessoa"].ToString();
            this.Email = dr["Eml_Pessoa"].ToString();
            this.Funcao = dr["Des_Funcao"].ToString();
            this.Cidade = dr["Nme_Cidade"].ToString();
            this.Estado = dr["Sig_Estado"].ToString();
            this.QuantidadeQuemMeViu15Dias = Convert.ToInt32(dr["QtdQuemMeViu15"]);
            this.QuantidadeQuemMeViu30Dias = Convert.ToInt32(dr["QtdQuemMeViu30"]);
            this.VIP = Convert.ToBoolean(dr["Flg_VIP"]);
            this.CPF = Convert.ToDecimal(dr["Num_CPF"]);//dr["Num_CPF"] != DBNull.Value ? Convert.ToDecimal(dr["Num_CPF"]) : decimal.Zero;
            this.DataNascimento = Convert.ToDateTime(dr["Dta_Nascimento"]);
        }
    }
}
