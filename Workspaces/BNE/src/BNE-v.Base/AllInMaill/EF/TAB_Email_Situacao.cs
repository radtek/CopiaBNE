namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Email_Situacao")]
    public partial class TAB_Email_Situacao
    {
        public TAB_Email_Situacao()
        {
            BNE_Usuario_Filial = new HashSet<BNE_Usuario_Filial>();
            BNE_Usuario_Filial1 = new HashSet<BNE_Usuario_Filial>();
            BNE_Usuario_Filial2 = new HashSet<BNE_Usuario_Filial>();
            TAB_Pessoa_Fisica = new HashSet<TAB_Pessoa_Fisica>();
            TAB_Pessoa_Fisica1 = new HashSet<TAB_Pessoa_Fisica>();
            TAB_Pessoa_Fisica2 = new HashSet<TAB_Pessoa_Fisica>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Email_Situacao { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Email_Situacao { get; set; }

        public virtual ICollection<BNE_Usuario_Filial> BNE_Usuario_Filial { get; set; }

        public virtual ICollection<BNE_Usuario_Filial> BNE_Usuario_Filial1 { get; set; }

        public virtual ICollection<BNE_Usuario_Filial> BNE_Usuario_Filial2 { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica1 { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica2 { get; set; }
    }
}
