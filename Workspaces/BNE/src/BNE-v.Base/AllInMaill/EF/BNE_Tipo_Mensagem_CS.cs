namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Tipo_Mensagem_CS")]
    public partial class BNE_Tipo_Mensagem_CS
    {
        public BNE_Tipo_Mensagem_CS()
        {
            BNE_Mensagem_CS = new HashSet<BNE_Mensagem_CS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Mensagem_CS { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Tipo_Mensagem { get; set; }

        public bool Flg_inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Mensagem_CS> BNE_Mensagem_CS { get; set; }
    }
}
