namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Filial_BNE")]
    public partial class TAB_Filial_BNE
    {
        [Key]
        public int Idf_Filial_BNE { get; set; }

        public int Qtd_Funcionarios { get; set; }

        public int? Mes_Inicio_Sazonalidade { get; set; }

        public int? Mes_Fim_Sazonalidade { get; set; }

        [Required]
        [StringLength(15)]
        public string Des_IP { get; set; }

        public bool Flg_Oferece_Cursos { get; set; }

        public int Idf_Situacao_Filial { get; set; }

        public int Idf_Filial { get; set; }

        public bool Flg_Duplicidade { get; set; }

        public bool Flg_Confidencial { get; set; }

        public DateTime? Dta_Fundacao { get; set; }

        [StringLength(50)]
        public string Sen_BNE_Velho { get; set; }

        public int? Qtd_Usuario_Adicional { get; set; }

        public bool Flg_Auditada { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual TAB_Situacao_Filial TAB_Situacao_Filial { get; set; }
    }
}
