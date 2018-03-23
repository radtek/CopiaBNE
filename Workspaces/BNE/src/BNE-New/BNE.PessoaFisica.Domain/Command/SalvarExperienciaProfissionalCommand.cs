using System;

namespace BNE.PessoaFisica.Domain.Command
{
    public class SalvarExperienciaProfissionalCommand
    {
        public int Id { get; set; }
        public int IdPessoa { get; set; }
        public int IdVaga { get; set; }


        public string NomeEmpresa { get; set; }
        public string AtividadesExercidas { get; set; }
        public string FuncaoExercida { get; set; }
        public bool FlgImportado { get; set; }
        public decimal? UltimoSalario { get; set; }
        public int IdRamoEmpresa { get; set; }
        public DateTime DataEntrada { get; set; }
        public DateTime? DataSaida { get; set; }
        public DateTime DataCadastro { get; set; }

        public decimal CPF { get; set; }
        public DateTime DataNascimento { get; set; }
        public int? IdPesquisa { get; set; }
        public string UrlPesquisa { get; set; }
        public string UrlVoltarLogado { get; set; }

        public bool Ativo { get; set; }
        public bool Candidatar { get; set; }
    }
}