namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curso_Modalidade_Tecla")]
    public partial class BNE_Curso_Modalidade_Tecla
    {
        public BNE_Curso_Modalidade_Tecla()
        {
            BNE_Curso_Parceiro_Tecla = new HashSet<BNE_Curso_Parceiro_Tecla>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Curso_Modalidade_Tecla { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Curso_Modalidade_Tecla { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Curso_Parceiro_Tecla> BNE_Curso_Parceiro_Tecla { get; set; }
    }
}
