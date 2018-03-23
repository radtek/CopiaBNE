namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Tipo_Integrador")]
    public partial class TAB_Tipo_Integrador
    {
        public TAB_Tipo_Integrador()
        {
            TAB_Integrador = new HashSet<TAB_Integrador>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Integrador { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Tipo_Integrador { get; set; }

        public virtual ICollection<TAB_Integrador> TAB_Integrador { get; set; }
    }
}
