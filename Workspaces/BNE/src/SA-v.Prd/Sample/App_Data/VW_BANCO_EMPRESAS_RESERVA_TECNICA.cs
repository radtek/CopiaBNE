namespace AdminLTE_Application
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("VW_BANCO_EMPRESAS_RESERVA_TECNICA")]
    public partial class VW_BANCO_EMPRESAS_RESERVA_TECNICA
    {
        public decimal? Num_CPF { get; set; }

        public DateTime? Data_Ultima_Acao_Site { get; set; }

        [Key]
        public decimal Num_CNPJ { get; set; }

        [StringLength(8000)]
        public string Raz_Social { get; set; }

        public DateTime? Dta_Ultimo_Atendimento { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Fim_Plano { get; set; }

        public string Ultimo_Plano { get; set; }

        public int? Total_Acao { get; set; }

        [StringLength(100)]
        public string Des_Area_BNE { get; set; }

        [StringLength(2)]
        public string Sig_Estado { get; set; }

        [StringLength(80)]
        public string Nme_Cidade { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dta_Vencimento { get; set; }

        public int qtd_Funcionarios { get; set; }

        public string Des_Situacao_Filial { get; set; }

        public int Idf_Situacao_Filial { get; set; }



    }
}
