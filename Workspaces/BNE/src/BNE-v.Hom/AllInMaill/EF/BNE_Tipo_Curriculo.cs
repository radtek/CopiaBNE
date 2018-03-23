namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Tipo_Curriculo")]
    public partial class BNE_Tipo_Curriculo
    {
        public BNE_Tipo_Curriculo()
        {
            BNE_Curriculo = new HashSet<BNE_Curriculo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Curriculo { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Tipo_Curriculo { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Curriculo> BNE_Curriculo { get; set; }
    }
}
