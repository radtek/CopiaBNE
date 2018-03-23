namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Rede_Social")]
    public partial class TAB_Rede_Social
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Rede_Social { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Rede_Social { get; set; }

        [Required]
        public byte[] Img_Rede_Social { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Max_Caracter { get; set; }
    }
}
