namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Situacao_Filial")]
    public partial class TAB_Situacao_Filial
    {
        public TAB_Situacao_Filial()
        {
            TAB_Filial = new HashSet<TAB_Filial>();
            TAB_Filial_BNE = new HashSet<TAB_Filial_BNE>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Situacao_Filial { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Situacao_Filial { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<TAB_Filial> TAB_Filial { get; set; }

        public virtual ICollection<TAB_Filial_BNE> TAB_Filial_BNE { get; set; }
    }
}
