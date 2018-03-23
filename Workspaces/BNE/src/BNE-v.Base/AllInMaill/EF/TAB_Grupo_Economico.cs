namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Grupo_Economico")]
    public partial class TAB_Grupo_Economico
    {
        public TAB_Grupo_Economico()
        {
            TAB_Empresa = new HashSet<TAB_Empresa>();
        }

        [Key]
        public int Idf_Grupo_Economico { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Grupo_Economico { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Empresa> TAB_Empresa { get; set; }
    }
}
