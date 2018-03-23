namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Consultor_R1")]
    public partial class BNE_Consultor_R1
    {
        public BNE_Consultor_R1()
        {
            BNE_Simulacao_R1 = new HashSet<BNE_Simulacao_R1>();
            BNE_Solicitacao_R1 = new HashSet<BNE_Solicitacao_R1>();
            TAB_Cidade = new HashSet<TAB_Cidade>();
        }

        [Key]
        public int Idf_Consultor_R1 { get; set; }

        [Required]
        [StringLength(150)]
        public string Nme_Consultor { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Email { get; set; }

        public virtual ICollection<BNE_Simulacao_R1> BNE_Simulacao_R1 { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }

        public virtual ICollection<TAB_Cidade> TAB_Cidade { get; set; }
    }
}
