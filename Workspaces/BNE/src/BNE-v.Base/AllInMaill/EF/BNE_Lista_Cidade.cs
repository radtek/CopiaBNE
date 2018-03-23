namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Lista_Cidade")]
    public partial class BNE_Lista_Cidade
    {
        [Key]
        public int Idf_Lista_Cidade { get; set; }

        public int Idf_Cidade { get; set; }

        public int Idf_Grupo_Cidade { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual BNE_Grupo_Cidade BNE_Grupo_Cidade { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }
    }
}
