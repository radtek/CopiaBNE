namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Tipo_Arquivo")]
    public partial class TAB_Tipo_Arquivo
    {
        public TAB_Tipo_Arquivo()
        {
            BNE_Arquivo = new HashSet<BNE_Arquivo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Arquivo { get; set; }

        [Required]
        [StringLength(50)]
        public string Dsc_Tipo_Arquivo { get; set; }

        public bool Flg_Remessa { get; set; }

        public virtual ICollection<BNE_Arquivo> BNE_Arquivo { get; set; }
    }
}
