namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Contato")]
    public partial class TAB_Contato
    {
        public int Idf_Pessoa_Fisica { get; set; }

        [Key]
        public int Idf_Contato { get; set; }

        [StringLength(100)]
        public string Nme_Contato { get; set; }

        [StringLength(30)]
        public string Tip_Contato { get; set; }

        [StringLength(2)]
        public string Num_DDD_Celular { get; set; }

        [StringLength(10)]
        public string Num_Celular { get; set; }

        [StringLength(2)]
        public string Num_DDD_Telefone { get; set; }

        [StringLength(10)]
        public string Num_Telefone { get; set; }

        [StringLength(4)]
        public string Num_Ramal_Telefone { get; set; }

        [StringLength(2)]
        public string Num_DDD_Fax { get; set; }

        [StringLength(10)]
        public string Num_Fax { get; set; }

        [StringLength(100)]
        public string Eml_Contato { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Importado { get; set; }

        public int? Idf_Tipo_Contato { get; set; }

        public int? Idf_Operadora_Celular { get; set; }

        public virtual TAB_Tipo_Contato TAB_Tipo_Contato { get; set; }

        public virtual TAB_Pessoa_Fisica_Complemento TAB_Pessoa_Fisica_Complemento { get; set; }

        public virtual TAB_Operadora_Celular TAB_Operadora_Celular { get; set; }
    }
}
