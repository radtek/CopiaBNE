namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_CNAE_Classe")]
    public partial class TAB_CNAE_Classe
    {
        public TAB_CNAE_Classe()
        {
            TAB_CNAE_Sub_Classe = new HashSet<TAB_CNAE_Sub_Classe>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_CNAE_Classe { get; set; }

        [Required]
        [StringLength(5)]
        public string Cod_CNAE_Classe { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_CNAE_Classe { get; set; }

        public int Idf_CNAE_Grupo { get; set; }

        public virtual ICollection<TAB_CNAE_Sub_Classe> TAB_CNAE_Sub_Classe { get; set; }

        public virtual TAB_CNAE_Grupo TAB_CNAE_Grupo { get; set; }
    }
}
