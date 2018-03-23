namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Formacao")]
    public partial class BNE_Formacao
    {
        public int Idf_Pessoa_Fisica { get; set; }

        [Key]
        public int Idf_Formacao { get; set; }

        public int Idf_Escolaridade { get; set; }

        public int? Idf_Curso { get; set; }

        public int? Idf_Fonte { get; set; }

        public short? Ano_Conclusao { get; set; }

        public short? Qtd_Carga_Horaria { get; set; }

        public short? Num_Periodo { get; set; }

        public short? Idf_Situacao_Formacao { get; set; }

        public DateTime? Dta_Alteracao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public bool Flg_Nacional { get; set; }

        [StringLength(300)]
        public string Des_Endereco { get; set; }

        [StringLength(200)]
        public string Des_Curso { get; set; }

        public int? Idf_Cidade { get; set; }

        [StringLength(200)]
        public string Des_Fonte { get; set; }

        public virtual BNE_Situacao_Formacao BNE_Situacao_Formacao { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual TAB_Curso TAB_Curso { get; set; }

        public virtual TAB_Escolaridade TAB_Escolaridade { get; set; }

        public virtual TAB_Fonte TAB_Fonte { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }
    }
}
