using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Bne.Web.Services.API.DTO
{
    public class MiniCurriculo
    {
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
            set{
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