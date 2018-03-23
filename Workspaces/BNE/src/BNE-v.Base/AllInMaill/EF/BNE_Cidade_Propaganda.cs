namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Cidade_Propaganda")]
    public partial class BNE_Cidade_Propaganda
    {
        [Key]
        public int Idf_Cidade_Propaganda { get; set; }

        public int Idf_Cidade { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [StringLength(100)]
        public string Eml_Remetente { get; set; }

        public bool Flg_Nao_Envia { get; set; }

        public bool Flg_Inativo { get; set; }

        [StringLength(100)]
        public string Nme_Remetente { get; set; }

        [StringLength(100)]
        public string Des_Cargo_Remetente { get; set; }

        [StringLength(13)]
        public string Num_Fone_Remetente { get; set; }

        [StringLength(13)]
        public string Num_Fone_Geral_Remetente { get; set; }
    }
}
