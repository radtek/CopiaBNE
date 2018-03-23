namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Tipo_Origem")]
    public partial class BNE_Tipo_Origem
    {
        public BNE_Tipo_Origem()
        {
            TAB_Origem = new HashSet<TAB_Origem>();
        }

        [Key]
        public int Idf_Tipo_Origem { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Tipo_Origem { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<TAB_Origem> TAB_Origem { get; set; }
    }
}
