using SolrNet.Attributes;

namespace BNE.Infrastructure.Services.SolrService.Model
{
    public class Vaga
    {
        [SolrUniqueKey("id")]
        public int Id { get; set; }
        [SolrUniqueKey("Cod_Vaga")]
        public string Codigo { get; set; }
        [SolrUniqueKey("Flg_Plano")]
        public bool TemPlano { get; set; }
        [SolrUniqueKey("Des_Funcao")]
        public string DescricaoFuncao { get; set; }
        [SolrUniqueKey("Flg_Deficiencia")]
        public bool TemDeficiencia { get; set; }
        [SolrUniqueKey("Des_Area_BNE")]
        public string Area { get; set; }
        [SolrUniqueKey("Des_Atribuicoes")]
        public string Atribuicoes { get; set; }
        [SolrUniqueKey("Nme_Cidade")]
        public string NomeCidade { get; set; }
        [SolrUniqueKey("Sig_Estado")]
        public string SiglaEstado { get; set; }
        [SolrUniqueKey("Idf_Cidade")]
        public string IdCidade { get; set; }
        [SolrUniqueKey("Idf_Funcao")]
        public string IdFuncao { get; set; }

        public string Salario
        {
            get
            {
                if (SalarioDe.HasValue && SalarioPara.HasValue)
                {
                    return $"Salário de R$ {SalarioDe:0.00} até R$ {SalarioPara:0.00}";
                }
                if (SalarioDe.HasValue)
                {
                    return $"Salário de R$ {SalarioDe:0.00}";
                }
                if (SalarioPara.HasValue)
                {
                    return $"Salário de R$ {SalarioPara:0.00}";
                }

                if (ValorMediaInicial.HasValue && ValorMediaFinal.HasValue)
                {
                    return $"Média salarial do mercado: R$ {ValorMediaInicial:0.00} a R$ {ValorMediaFinal:0.00}";
                }

                return "A combinar";
            }
        }

        [SolrUniqueKey("Vlr_Salario_De")]
        private double? SalarioDe { get; set; }

        [SolrUniqueKey("Vlr_Salario_Para")]
        private double? SalarioPara { get; set; }

        [SolrUniqueKey("Vlr_Junior_Funcao_Estado")]
        private double? ValorMediaInicial { get; set; }

        [SolrUniqueKey("Vlr_Master_Funcao_Estado")]
        private double? ValorMediaFinal { get; set; }

    }
}
