namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curriculo_Fulltext")]
    public partial class BNE_Curriculo_Fulltext
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Curriculo { get; set; }

        [Required]
        public string Des_MetaBusca { get; set; }

        public DateTime Dta_Criacao { get; set; }

        public DateTime Dta_Atualizacao { get; set; }

        public string Des_Experiencia_Profissional { get; set; }

        public string Des_Curso { get; set; }

        public string Nme_Fonte { get; set; }

        public string Raz_Social { get; set; }

        public string Des_Metabusca_Rapida { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }
    }
}
