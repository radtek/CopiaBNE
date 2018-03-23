namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Cidade_Capital")]
    public partial class TAB_Cidade_Capital
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Cidade { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }
    }
}
