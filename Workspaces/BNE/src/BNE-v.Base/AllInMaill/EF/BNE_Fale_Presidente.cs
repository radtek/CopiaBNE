namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Fale_Presidente")]
    public partial class BNE_Fale_Presidente
    {
        [Key]
        public int Idf_Fale_Presidente { get; set; }

        [StringLength(100)]
        public string Nme_Usuario { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_CPF { get; set; }

        [StringLength(100)]
        public string Eml_Pessoa { get; set; }

        [StringLength(100)]
        public string Des_Assunto { get; set; }

        [StringLength(500)]
        public string Des_Mensagem { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public bool? Flg_Inativo { get; set; }
    }
}
