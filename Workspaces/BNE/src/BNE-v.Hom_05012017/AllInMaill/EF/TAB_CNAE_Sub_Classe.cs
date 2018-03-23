namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_CNAE_Sub_Classe")]
    public partial class TAB_CNAE_Sub_Classe
    {
        public TAB_CNAE_Sub_Classe()
        {
            TAB_Filial = new HashSet<TAB_Filial>();
            TAB_Pessoa_Juridica = new HashSet<TAB_Pessoa_Juridica>();
            TAB_Pessoa_Juridica1 = new HashSet<TAB_Pessoa_Juridica>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_CNAE_Sub_Classe { get; set; }

        [Required]
        [StringLength(7)]
        public string Cod_CNAE_Sub_Classe { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_CNAE_Sub_Classe { get; set; }

        public int Idf_CNAE_Classe { get; set; }

        public virtual ICollection<TAB_Filial> TAB_Filial { get; set; }

        public virtual TAB_CNAE_Classe TAB_CNAE_Classe { get; set; }

        public virtual ICollection<TAB_Pessoa_Juridica> TAB_Pessoa_Juridica { get; set; }

        public virtual ICollection<TAB_Pessoa_Juridica> TAB_Pessoa_Juridica1 { get; set; }
    }
}
