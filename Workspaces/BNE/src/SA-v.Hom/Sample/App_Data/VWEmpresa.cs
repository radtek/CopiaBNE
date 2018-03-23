namespace AdminLTE_Application
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("VWEmpresa")]
    public partial class VWEmpresa
    {
        
        [Column(Order = 0)]
        [StringLength(100)]
        public string Raz_Social { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "numeric")]
        public decimal Num_CNPJ { get; set; }

        [StringLength(100)]
        public string End_Site { get; set; }

        [Column(Order = 2)]
        [StringLength(2)]
        public string Num_DDD_Comercial { get; set; }

        [Column(Order = 3)]
        [StringLength(10)]
        public string Num_Comercial { get; set; }

        [StringLength(100)]
        public string Des_Natureza_Juridica { get; set; }

        [StringLength(100)]
        public string Des_Plano { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Inicio_Plano { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Fim_plano { get; set; }

        [StringLength(2)]
        public string Sig_Estado { get; set; }

        [StringLength(80)]
        public string Nme_Cidade { get; set; }

        [StringLength(50)]
        public string Des_Situacao_Filial { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column(Order = 4)]
        public DateTime? Dta_Cadastro { get; set; }

        [StringLength(120)]
        public string Des_URL { get; set; }

        [StringLength(120)]
        public string Nme_Vendedor { get; set; }

        public decimal? Num_CPF { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column(Order = 5)]
        public DateTime? Dta_Inicio_Carteira { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [Column(Order = 6)]
        public DateTime? Dta_Fim_Carteira { get; set; }

        [StringLength(120)]
        public string Eml_Vendedor { get; set; }

        [StringLength(200)]
        public string Des_CNAE_Sub_Classe { get; set; }

        public int? Qtd_Funcionarios { get; set; }
        
        [StringLength(200)]
        public string Nme_Usuario { get; set; }

        [StringLength(200)]
        public string Des_Funcao_Usuario { get; set; }

        [StringLength(2)]
        public string Num_DDD_Comercial_Usuario { get; set; }

        [StringLength(2)]
        public string Num_Comercial_Usuario { get; set; }

        public int? Idf_Cidade { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? Dta_Retorno { get; set; }

        public int? PlanoBloquedo { get; set; }
    }
}
