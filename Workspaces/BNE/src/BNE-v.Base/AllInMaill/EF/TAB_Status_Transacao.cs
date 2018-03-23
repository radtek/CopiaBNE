namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Status_Transacao")]
    public partial class TAB_Status_Transacao
    {
        public TAB_Status_Transacao()
        {
            BNE_Transacao = new HashSet<BNE_Transacao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Status_Transacao { get; set; }

        [Required]
        [StringLength(150)]
        public string Dsc_Status_Transacao { get; set; }

        public virtual ICollection<BNE_Transacao> BNE_Transacao { get; set; }
    }
}
