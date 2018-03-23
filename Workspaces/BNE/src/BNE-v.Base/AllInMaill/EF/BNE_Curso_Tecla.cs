namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curso_Tecla")]
    public partial class BNE_Curso_Tecla
    {
        public BNE_Curso_Tecla()
        {
            BNE_Curso_Funcao_Tecla = new HashSet<BNE_Curso_Funcao_Tecla>();
            BNE_Curso_Parceiro_Tecla = new HashSet<BNE_Curso_Parceiro_Tecla>();
        }

        [Key]
        public int Idf_Curso_Tecla { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_Curso_Tecla { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Curso_Funcao_Tecla> BNE_Curso_Funcao_Tecla { get; set; }

        public virtual ICollection<BNE_Curso_Parceiro_Tecla> BNE_Curso_Parceiro_Tecla { get; set; }
    }
}
