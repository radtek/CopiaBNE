namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Palavra_Chave")]
    public partial class BNE_Vaga_Palavra_Chave
    {
        [Key]
        public int Idf_Vaga_Palavra_Chave { get; set; }

        public int Idf_Vaga { get; set; }

        public int Idf_Palavra_Chave { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual BNE_Palavra_Chave BNE_Palavra_Chave { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }
    }
}
