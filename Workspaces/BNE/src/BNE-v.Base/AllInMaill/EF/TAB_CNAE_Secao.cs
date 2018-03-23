namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_CNAE_Secao")]
    public partial class TAB_CNAE_Secao
    {
        public TAB_CNAE_Secao()
        {
            TAB_CNAE_Divisao = new HashSet<TAB_CNAE_Divisao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_CNAE_Secao { get; set; }

        [Required]
        [StringLength(1)]
        public string Cod_CNAE_Secao { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_CNAE_Secao { get; set; }

        public virtual ICollection<TAB_CNAE_Divisao> TAB_CNAE_Divisao { get; set; }
    }
}
