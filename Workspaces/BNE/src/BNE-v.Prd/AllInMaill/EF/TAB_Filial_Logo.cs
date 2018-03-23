namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Filial_Logo")]
    public partial class TAB_Filial_Logo
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Filial { get; set; }

        public byte[] Img_Logo { get; set; }

        [Key]
        [Column(Order = 1)]
        public bool Flg_Inativo { get; set; }

        [Key]
        [Column(Order = 2)]
        public DateTime Dta_Cadastro { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime Dta_Alteracao { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }
    }
}
