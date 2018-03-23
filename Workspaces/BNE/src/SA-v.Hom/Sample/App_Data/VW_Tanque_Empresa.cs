namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VW_Tanque_Empresa")]
    public partial class VW_Tanque_Empresa
    {

        public decimal? Num_CPF { get; set; }

        public DateTime? Data_Ultima_Acao_Site { get; set; }

        [Key]
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

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Inicio { get; set; }

        public int qtd_Funcionarios { get; set; }

    }
}
