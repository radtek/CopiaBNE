namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Parceiro_Tecla")]
    public partial class BNE_Parceiro_Tecla
    {
        public BNE_Parceiro_Tecla()
        {
            BNE_Cadastro_Parceiro = new HashSet<BNE_Cadastro_Parceiro>();
            BNE_Curso_Parceiro_Tecla = new HashSet<BNE_Curso_Parceiro_Tecla>();
        }

        [Key]
        public int Idf_Parceiro_Tecla { get; set; }

        [Required]
        [StringLength(200)]
        public string Nme_Parceiro_Tecla { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        [Required]
        [StringLength(300)]
        public string Des_URL_Cadastro { get; set; }

        [Required]
        [StringLength(300)]
        public string Des_URL_Autenticacao { get; set; }

        public virtual ICollection<BNE_Cadastro_Parceiro> BNE_Cadastro_Parceiro { get; set; }

        public virtual ICollection<BNE_Curso_Parceiro_Tecla> BNE_Curso_Parceiro_Tecla { get; set; }
    }
}
