namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Inscricao_Curso")]
    public partial class BNE_Inscricao_Curso
    {
        [Key]
        public int Idf_Inscricao_Curso { get; set; }

        public int Idf_Curriculo { get; set; }

        public int Idf_Curso_Parceiro_Tecla { get; set; }

        public DateTime Dta_Inscricao { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual BNE_Curso_Parceiro_Tecla BNE_Curso_Parceiro_Tecla { get; set; }
    }
}
