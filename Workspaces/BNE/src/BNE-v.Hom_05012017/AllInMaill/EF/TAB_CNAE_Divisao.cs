namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_CNAE_Divisao")]
    public partial class TAB_CNAE_Divisao
    {
        public TAB_CNAE_Divisao()
        {
            TAB_CNAE_Grupo = new HashSet<TAB_CNAE_Grupo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_CNAE_Divisao { get; set; }

        [Required]
        [StringLength(2)]
        public string Cod_CNAE_Divisao { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_CNAE_Divisao { get; set; }

        public int Idf_CNAE_Secao { get; set; }

        public int? Idf_Area_BNE { get; set; }

        public virtual TAB_Area_BNE TAB_Area_BNE { get; set; }

        public virtual ICollection<TAB_CNAE_Grupo> TAB_CNAE_Grupo { get; set; }

        public virtual TAB_CNAE_Secao TAB_CNAE_Secao { get; set; }
    }
}
