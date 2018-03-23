namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Status_Mensagem_CS")]
    public partial class BNE_Status_Mensagem_CS
    {
        public BNE_Status_Mensagem_CS()
        {
            BNE_Mensagem_CS = new HashSet<BNE_Mensagem_CS>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Status_Mensagem_CS { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Status_Mensagem { get; set; }

        public virtual ICollection<BNE_Mensagem_CS> BNE_Mensagem_CS { get; set; }
    }
}
