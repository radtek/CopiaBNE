namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pesquisa_Salarial")]
    public partial class TAB_Pesquisa_Salarial
    {
        [Key]
        public int Idf_Pesquisa_Salarial { get; set; }

        public int Idf_Funcao { get; set; }

        public int? Idf_Estado { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Media { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Maximo { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Minimo { get; set; }

        public DateTime Dta_Atualizacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public long Num_Populacao { get; set; }

        public int Num_Amostra { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Junior { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Treinee { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Senior { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Master { get; set; }

        [Column(TypeName = "money")]
        public decimal Vlr_Pleno { get; set; }

        public virtual TAB_Estado TAB_Estado { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
