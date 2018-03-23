namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Curso_Funcao_Tecla")]
    public partial class BNE_Curso_Funcao_Tecla
    {
        [Key]
        public int Idf_Curso_Funcao_Tecla { get; set; }

        public int Idf_Curso_Tecla { get; set; }

        public int Idf_Funcao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual BNE_Curso_Tecla BNE_Curso_Tecla { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
