namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Fulltext")]
    public partial class BNE_Vaga_Fulltext
    {
        [Required]
        public string Des_Metabusca_Rapida { get; set; }

        public DateTime Dta_Criacao { get; set; }

        public DateTime Dta_Atualizacao { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Vaga { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }
    }
}
