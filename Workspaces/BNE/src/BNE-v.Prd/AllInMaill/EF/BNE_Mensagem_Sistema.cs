namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Mensagem_Sistema")]
    public partial class BNE_Mensagem_Sistema
    {
        public BNE_Mensagem_Sistema()
        {
            BNE_Mensagem_CS = new HashSet<BNE_Mensagem_CS>();
        }

        [Key]
        public int Idf_Mensagem_Sistema { get; set; }

        [Required]
        [StringLength(40)]
        public string Des_Mensagem_Sistema { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Mensagem_CS> BNE_Mensagem_CS { get; set; }
    }
}
