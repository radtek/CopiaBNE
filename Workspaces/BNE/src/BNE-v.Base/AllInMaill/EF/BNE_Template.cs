namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Template")]
    public partial class BNE_Template
    {
        public BNE_Template()
        {
            TAB_Origem_Filial = new HashSet<TAB_Origem_Filial>();
        }

        [Key]
        public int Idf_Template { get; set; }

        [Required]
        [StringLength(20)]
        public string Nme_Template { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<TAB_Origem_Filial> TAB_Origem_Filial { get; set; }
    }
}
