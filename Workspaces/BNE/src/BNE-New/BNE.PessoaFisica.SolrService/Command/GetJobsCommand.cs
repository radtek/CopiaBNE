using System.Collections.Generic;

namespace BNE.PessoaFisica.SolrService.Command
{
    public class GetJobsCommand
    {
        public string Funcao { get; set; }
        public string Cidade { get; set; }
        public int? IdFuncao { get; set; }
        public int? IdCidade { get; set; }
        public int? Escolaridade { get; set; }
        public decimal? SalarioMinimo { get; set; }
        public decimal? SalarioMaximo { get; set; }
        public int? IdadeMinima { get; set; }
        public int? IdadeMaxima { get; set; }
        public int? Sexo { get; set; }
        public string Estado { get; set; }
        public int? AreaBNE { get; set; }
        public string PalavraChave { get; set; }
        public string RazaoSocial { get; set; }
        public int? Deficiencia { get; set; }
        public List<int> Disponibilidades { get; set; }
        public List<int> TiposDeVinculo { get; set; }

        //PaginationHelpers
        public int? JobIndex { get; set; }
    }
}