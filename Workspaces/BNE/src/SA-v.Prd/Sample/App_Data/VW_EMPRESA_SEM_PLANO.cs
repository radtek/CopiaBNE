namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class VW_EMPRESA_SEM_PLANO
    {
        [Key]
        [Column(Order = 0, TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        public DateTime? Data_Ultimo_Atendimento { get; set; }

        public DateTime? Data_Ultima_Acao_Site { get; set; }

        public DateTime? Data_ultimo_ATC_empresa { get; set; }

        public int? Idf_Atendimento { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal Num_CNPJ { get; set; }

        [StringLength(8000)]
        public string Raz_Social { get; set; }

        [StringLength(2)]
        public string Sig_Estado { get; set; }

        [StringLength(100)]
        public string Des_Area_BNE { get; set; }

        [StringLength(80)]
        public string Nme_Cidade { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Fim_Plano { get; set; }

        public int? Total_Acao { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Dta_Cadastro { get; set; }

          [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Retorno { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string Des_Atendimento { get; set; }

        //[Key]
        //[Column(Order = 4)]
        //public DateTime Dta_Inicio { get; set; }

        public DateTime? Dta_Fim { get; set; }

        public int? Vlr_Percentual { get; set; }
        public double? ISS { get; set; }

        public int? qtd_Funcionarios { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? dta_vencimento { get; set; }

        [StringLength(100)]
        public string Ultimo_Plano { get; set; }

        public string Des_Situacao_Filial { get; set; }

        public int Idf_Situacao_Filial { get; set; }
    }
}
