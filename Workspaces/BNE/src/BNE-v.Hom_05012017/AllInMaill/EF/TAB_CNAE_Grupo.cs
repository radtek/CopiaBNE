namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_CNAE_Grupo")]
    public partial class TAB_CNAE_Grupo
    {
        public TAB_CNAE_Grupo()
        {
            TAB_CNAE_Classe = new HashSet<TAB_CNAE_Classe>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_CNAE_Grupo { get; set; }

        [Required]
        [StringLength(3)]
        public string Cod_CNAE_Grupo { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_CNAE_Grupo { get; set; }

        public int Idf_CNAE_Divisao { get; set; }

        public virtual ICollection<TAB_CNAE_Classe> TAB_CNAE_Classe { get; set; }

        public virtual TAB_CNAE_Divisao TAB_CNAE_Divisao { get; set; }
    }
}
