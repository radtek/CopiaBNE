namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Palavra_Chave")]
    public partial class BNE_Palavra_Chave
    {
        public BNE_Palavra_Chave()
        {
            BNE_Vaga_Palavra_Chave = new HashSet<BNE_Vaga_Palavra_Chave>();
        }

        [Key]
        public int Idf_Palavra_Chave { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Palavra_Chave { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Vaga_Palavra_Chave> BNE_Vaga_Palavra_Chave { get; set; }
    }
}
