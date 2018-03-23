namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Tipo_Publicacao")]
    public partial class BNE_Tipo_Publicacao
    {
        public BNE_Tipo_Publicacao()
        {
            BNE_Campo_Publicacao = new HashSet<BNE_Campo_Publicacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Publicacao { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Tipo_Publicacao { get; set; }

        public virtual ICollection<BNE_Campo_Publicacao> BNE_Campo_Publicacao { get; set; }
    }
}
