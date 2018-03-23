namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Origem_Filial_Funcao")]
    public partial class TAB_Origem_Filial_Funcao
    {
        [Key]
        public int Idf_Origem_Filial_Funcao { get; set; }

        public int Idf_Origem_Filial { get; set; }

        public int Idf_Funcao { get; set; }

        [StringLength(100)]
        public string Des_Funcao { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public DateTime? Dta_Alteracao { get; set; }

        public bool? Flg_Inativo { get; set; }

        public virtual TAB_Origem_Filial TAB_Origem_Filial { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
