using System;

namespace BNE.PessoaFisica.Domain.Command
{
    public class SalvarCurriculoCommand
    {
        public int Id { get; set; }
        public decimal CPF { get; set; }
        public string Nome { get; set; }
        public DateTime DataNascimento { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public string RG { get; set; }
        public string OrgaoEmissor { get; set; }
        public bool FlgPossuiFilhos { get; set; }
        public byte? QtdFilhos { get; set; }
        public string CategoriaHabilitacao { get; set; }
        public string CNH { get; set; }


        public string Sexo { get; set; }
        public string Email { get; set; }
        public byte DDDCelular { get; set; }
        public decimal NumeroCelular { get; set; }
        public string Cidade { get; set; }
        public decimal PretensaoSalarial { get; set; }
        public int? TempoExperienciaAnos { get; set; }
        public int? TempoExperienciaMeses { get; set; }
        public int IdEscolaridade { get; set; }
        public int? IdDeficiencia { get; set; }


        public string DescricaoFuncao { get; set; }
        public int IdCidade { get; set; }
        public int IdFuncao { get; set; }
        public int IdSexo { get; set; }

        public int IdTipoCurriculo { get; set; }
        public int IdSituacaoCurriculo { get; set; }
        public int IdOrigem { get; set; }
        public short? TempoExperiencia { get; set; }
        public int? IdVaga { get; set; }
        public bool FlgWhatsApp { get; set; }
    }
}