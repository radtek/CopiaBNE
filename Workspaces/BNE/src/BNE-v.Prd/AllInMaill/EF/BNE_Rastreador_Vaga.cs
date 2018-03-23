namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Rastreador_Vaga")]
    public partial class BNE_Rastreador_Vaga
    {
        [Key]
        public int Idf_Rastreador_Vaga { get; set; }

        public int? Idf_Funcao { get; set; }

        public int? Idf_Cidade { get; set; }

        [StringLength(20)]
        public string Des_Palavra_Chave { get; set; }

        public int? Idf_Pessoa_Fisica { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
