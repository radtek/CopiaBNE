namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Campo_Publicacao")]
    public partial class BNE_Campo_Publicacao
    {
        public BNE_Campo_Publicacao()
        {
            BNE_Regra_Campo_Publicacao = new HashSet<BNE_Regra_Campo_Publicacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Campo_Publicacao { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Campo_Publicacao { get; set; }

        public int Idf_Tipo_Publicacao { get; set; }

        public virtual ICollection<BNE_Regra_Campo_Publicacao> BNE_Regra_Campo_Publicacao { get; set; }

        public virtual BNE_Tipo_Publicacao BNE_Tipo_Publicacao { get; set; }
    }
}
