namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.GLO_Status_Transacao")]
    public partial class GLO_Status_Transacao
    {
        public GLO_Status_Transacao()
        {
            GLO_Transacao = new HashSet<GLO_Transacao>();
        }

        [Key]
        public int Idf_Status_Transacao { get; set; }

        [StringLength(50)]
        public string Des_Status_Transacao { get; set; }

        public virtual ICollection<GLO_Transacao> GLO_Transacao { get; set; }
    }
}
