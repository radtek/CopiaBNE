namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Operadora_Celular")]
    public partial class TAB_Operadora_Celular
    {
        public TAB_Operadora_Celular()
        {
            TAB_Contato = new HashSet<TAB_Contato>();
            TAB_Pessoa_Fisica = new HashSet<TAB_Pessoa_Fisica>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Operadora_Celular { get; set; }

        [Required]
        [StringLength(50)]
        public string Nme_Operadora_Celular { get; set; }

        public bool Flg_Inativo { get; set; }

        [StringLength(500)]
        public string Des_URL_Logo { get; set; }

        public virtual ICollection<TAB_Contato> TAB_Contato { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }
    }
}
