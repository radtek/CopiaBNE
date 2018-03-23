namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Transacao")]
    public partial class TAB_Tipo_Transacao
    {
        public TAB_Tipo_Transacao()
        {
            GLO_Transacao = new HashSet<GLO_Transacao>();
        }

        [Key]
        public int Idf_Tipo_Transacao { get; set; }

        [StringLength(50)]
        public string Des_Transacao { get; set; }

        public virtual ICollection<GLO_Transacao> GLO_Transacao { get; set; }
    }
}
