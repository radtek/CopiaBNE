namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Home")]
    public partial class BNE_Vaga_Home
    {
        [Key]
        public int Idf_Vaga_Home { get; set; }

        public int Num_Vaga { get; set; }

        [StringLength(80)]
        public string Des_Funcao_Vaga_Home { get; set; }

        [StringLength(80)]
        public string Des_Vaga_Home { get; set; }

        [StringLength(10)]
        public string Cod_Vaga { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Vaga { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }
    }
}
