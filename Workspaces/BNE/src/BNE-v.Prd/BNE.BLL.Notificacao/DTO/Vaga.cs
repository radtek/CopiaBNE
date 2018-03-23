using System;
using System.Data;

namespace BNE.BLL.Notificacao.DTO
{
    public class Vaga
    {
        public int IdVaga { get; set; }
        public string Codigo { get; set; }
        public string DescricaoFuncao { get; set; }
        public decimal? ValorSalarioInicial { get; set; }
        public decimal? ValorSalarioFinal { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Atribuicoes { get; set; }
        public decimal? ValorMediaInicial { get; set; }
        public decimal? ValorMediaFinal { get; set; }
        public bool TemPlano { get; set; }
        public bool TemDeficiencia { get; set; }
        public string Area { get; set; }

        public string Salario
        {
            get
            {
                if (ValorSalarioInicial.HasValue && ValorSalarioFinal.HasValue)
                {
                    return string.Format("Salário de R$ {0:0.00} até R$ {1:0.00}", ValorSalarioInicial, ValorSalarioFinal);
                }
                if (ValorSalarioInicial.HasValue)
                {
                    return string.Format("Salário de R$ {0:0.00}", ValorSalarioInicial);

                }
                if (ValorSalarioFinal.HasValue)
                {
                    return string.Format("Salário de R$ {0:0.00}", ValorSalarioFinal);

                }

                if (ValorMediaInicial.HasValue && ValorMediaFinal.HasValue)
                {
                    return string.Format("Média salarial do mercado: R$ {0:0.00} a R$ {1:0.00}", ValorMediaInicial, ValorMediaFinal);
                }

                return "A combinar";
            }
        }


        public void ReadFromDataReader(IDataReader dr)
        {
            this.IdVaga = Convert.ToInt32(dr["Idf_Vaga"]);
            this.Codigo = dr["Cod_Vaga"].ToString();
            this.Cidade = dr["Nme_Cidade"].ToString();
            this.Estado = dr["Sig_Estado"].ToString();
            this.Atribuicoes = dr["Des_Atribuicoes"].ToString();
            this.Area = dr["Des_Area_BNE"].ToString();

            if (dr["Vlr_Salario_De"] != DBNull.Value)
            {
                this.ValorSalarioInicial = Convert.ToDecimal(dr["Vlr_Salario_De"]);
            }

            if (dr["Vlr_Salario_Para"] != DBNull.Value)
            {
                this.ValorSalarioFinal = Convert.ToDecimal(dr["Vlr_Salario_Para"]);
            }

            if (dr["Vlr_Junior"] != DBNull.Value)
            {
                this.ValorMediaInicial = Convert.ToDecimal(dr["Vlr_Junior"]);
            }

            if (dr["Vlr_Master"] != DBNull.Value)
            {
                this.ValorMediaFinal = Convert.ToDecimal(dr["Vlr_Master"]);
            }

            if (dr["Idf_Vaga_Tipo_Vinculo"] != DBNull.Value && dr["Des_Funcao"].ToString() != "Estagiário")
            {
                this.DescricaoFuncao = "Estágio para " + dr["Des_Funcao"];
            }
            else
            {
                this.DescricaoFuncao = dr["Des_Funcao"].ToString();
            }

            if (dr["Idf_Plano_Adquirido"] != DBNull.Value)
            {
                this.TemPlano = true;
            }

            if (dr["Idf_Deficiencia"] != DBNull.Value && Convert.ToInt32(dr["Idf_Deficiencia"]) != 0)
            {
                this.TemDeficiencia = true;
            }

        }
    }
}
