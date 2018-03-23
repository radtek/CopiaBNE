namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_GPS")]
    public partial class TAB_GPS
    {
        public TAB_GPS()
        {
            TAB_Pessoa_Juridica = new HashSet<TAB_Pessoa_Juridica>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_GPS { get; set; }

        [Required]
        [StringLength(500)]
        public string Des_GPS { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Pessoa_Juridica> TAB_Pessoa_Juridica { get; set; }
    }
}
