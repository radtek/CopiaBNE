using SolrNet.Attributes;

namespace BNE.PessoaFisica.SolrService.Model
{
    public class Vaga
    {
        [SolrUniqueKey("id")]
        public int Id { get; set; }
        [SolrUniqueKey("Url_Vaga")]
        public string Url { get; set; }
        [SolrUniqueKey("Idf_Funcao")]
        public int IdFuncao { get; set; }
        [SolrUniqueKey("Des_Funcao")]
        public string Funcao { get; set; }
        [SolrUniqueKey("Idf_Cidade")]
        public int IdCidade { get; set; }
        [SolrUniqueKey("Nme_Cidade")]
        public string Cidade { get; set; }
        [SolrUniqueKey("Idf_Escolaridade")]
        public int? IdEscolaridade { get; set; }
        [SolrUniqueKey("Vlr_Salario_De")]
        public decimal? SalarioMinimo { get; set; }
        [SolrUniqueKey("Vlr_Salario_Para")]
        public decimal? SalarioMaximo { get; set; }
        [SolrUniqueKey("Num_Idade_Minima")]
        public int? IdadeMinima { get; set; }
        [SolrUniqueKey("Num_Idade_Maxima")]
        public int? IdadeMaxima { get; set; }
        [SolrUniqueKey("Idf_Sexo")]
        public int? IdSexo { get; set; }

    }
}
