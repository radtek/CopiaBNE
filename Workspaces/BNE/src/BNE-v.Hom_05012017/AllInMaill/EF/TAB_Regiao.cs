namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Regiao")]
    public partial class TAB_Regiao
    {
        public TAB_Regiao()
        {
            TAB_Estado = new HashSet<TAB_Estado>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Regiao { get; set; }

        [Required]
        [StringLength(30)]
        public string Nme_Regiao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Estado> TAB_Estado { get; set; }
    }
}
