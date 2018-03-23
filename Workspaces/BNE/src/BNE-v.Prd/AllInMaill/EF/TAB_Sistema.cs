namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Sistema")]
    public partial class TAB_Sistema
    {
        public TAB_Sistema()
        {
            GLO_Transacao = new HashSet<GLO_Transacao>();
            TAB_Mensagem = new HashSet<TAB_Mensagem>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Sistema { get; set; }

        [Required]
        [StringLength(50)]
        public string Nme_Sistema { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public short Num_Prioridade { get; set; }

        public bool Flg_Possui_Integracao { get; set; }

        public virtual ICollection<GLO_Transacao> GLO_Transacao { get; set; }

        public virtual ICollection<TAB_Mensagem> TAB_Mensagem { get; set; }
    }
}
