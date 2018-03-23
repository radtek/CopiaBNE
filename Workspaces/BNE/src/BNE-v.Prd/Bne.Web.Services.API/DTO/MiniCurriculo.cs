using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    public class MiniCurriculo : Curriculo
    {
        public MiniCurriculo(IDataReader dr)
        {
            if (dr["Dta_Candidatura"] != null)
                DataHoraCandidatura = Convert.ToDateTime(dr["Dta_Candidatura"]);

            Bairro =
                dr.IsDBNull(dr.GetOrdinal("Des_Bairro"))
                    ? ""
                    : dr.GetString(dr.GetOrdinal("Des_Bairro"));
            Carteira =
                dr.IsDBNull(dr.GetOrdinal("Des_Categoria_Habilitacao"))
                    ? ""
                    : dr.GetString(dr.GetOrdinal("Des_Categoria_Habilitacao"));
            Cidade =
                dr.IsDBNull(dr.GetOrdinal("Nme_Cidade"))
                    ? ""
                    : dr.GetString(dr.GetOrdinal("Nme_Cidade"));
            Escolaridade =
                dr.IsDBNull(dr.GetOrdinal("Des_Abreviada"))
                    ? ""
                    : dr.GetString(dr.GetOrdinal("Des_Abreviada"));
            Estado =
                dr.IsDBNull(dr.GetOrdinal("Sig_Estado"))
                    ? ""
                    : dr.GetString(dr.GetOrdinal("Sig_Estado"));
            EstadoCivil =
                dr.IsDBNull(dr.GetOrdinal("Des_Estado_Civil"))
                    ? ""
                    : dr.GetString(dr.GetOrdinal("Des_Estado_Civil"));
            Experiencia =
                dr.IsDBNull(dr.GetOrdinal("Qtd_Experiencia"))
                    ? "0 m"
                    : dr.GetInt32(dr.GetOrdinal("Qtd_Experiencia")).ToString() + " m";
            Funcoes =
                dr.IsDBNull(dr.GetOrdinal("Des_Funcao"))
                    ? ""
                    : dr.GetString(dr.GetOrdinal("Des_Funcao"));
            Idade =
                dr.IsDBNull(dr.GetOrdinal("Num_Idade"))
                    ? 0
                    : dr.GetInt32(dr.GetOrdinal("Num_Idade"));
            IDCurriculo =
                dr.IsDBNull(dr.GetOrdinal("Idf_Curriculo"))
                    ? 0
                    : dr.GetInt32(dr.GetOrdinal("Idf_Curriculo"));
            Nome =
                dr.IsDBNull(dr.GetOrdinal("Nme_Pessoa"))
                    ? ""
                    : dr.GetString(dr.GetOrdinal("Nme_Pessoa"));
            Pretensao =
                dr.IsDBNull(dr.GetOrdinal("Vlr_Pretensao_Salarial"))
                    ? 0m
                    : dr.GetDecimal(dr.GetOrdinal("Vlr_Pretensao_Salarial"));
            Sexo =
                dr.IsDBNull(dr.GetOrdinal("Sig_Sexo"))
                    ? ""
                    : dr.GetString(dr.GetOrdinal("Sig_Sexo"));
            Vip = dr["Flg_Vip"] != DBNull.Value && Convert.ToBoolean(dr["Flg_Vip"]);
        }

        public MiniCurriculo(DataRow row)
        {
            this.Vip = Convert.ToBoolean(row["Vip"]);
            this.Nome = row["Nme_Pessoa"].ToString();
            this.Sexo = row["Des_Genero"].ToString();
            this.EstadoCivil = row["Des_Estado_Civil"].ToString();
            this.Idade = Convert.ToInt32(row["Num_Idade"]);
            this.Escolaridade = row["Sig_Escolaridade"].ToString();
            this.Pretensao = Convert.ToDecimal(row["Vlr_Pretensao_Salarial"]);
            this.Bairro = row["Des_Bairro"].ToString();
            this.Cidade = row["Nme_Cidade"].ToString();
            this.Estado = row["Sig_Estado"].ToString();
            this.Funcoes = row["Des_Funcao"].ToString();
            this.Experiencia = row["Des_Experiencia"].ToString();
            this.Carteira = row["Des_Categoria_Habilitacao"].ToString();
            this.IDCurriculo = Convert.ToInt32(row["Idf_Curriculo"]);
        }

        private string _nome;

        public bool Vip { get; set; }
        public string Nome
        {
            get { return _nome; }
            set
            {
                _nome = (value.IndexOf(" ") > 1) ? value.Substring(0, value.IndexOf(" ")) : value;
            }
        }
        public string Sexo { get; set; }
        public string EstadoCivil { get; set; }
        public int Idade { get; set; }
        public string Escolaridade { get; set; }
        public decimal Pretensao { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Funcoes { get; set; }
        public string Experiencia { get; set; }
        public string Carteira { get; set; }
        public int IDCurriculo { get; set; }
    }
}