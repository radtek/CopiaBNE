namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Parametro")]
    public partial class TAB_Parametro
    {
        public TAB_Parametro()
        {
            TAB_Parametro_Curriculo = new HashSet<TAB_Parametro_Curriculo>();
            TAB_Parametro_Integrador = new HashSet<TAB_Parametro_Integrador>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Parametro { get; set; }

        [Required]
        [StringLength(70)]
        public string Nme_Parametro { get; set; }

        [Required]
        public string Vlr_Parametro { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Parametro_Curriculo> TAB_Parametro_Curriculo { get; set; }

        public virtual ICollection<TAB_Parametro_Integrador> TAB_Parametro_Integrador { get; set; }
    }
}
