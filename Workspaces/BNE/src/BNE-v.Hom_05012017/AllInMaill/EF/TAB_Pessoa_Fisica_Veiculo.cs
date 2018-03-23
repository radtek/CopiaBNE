namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pessoa_Fisica_Veiculo")]
    public partial class TAB_Pessoa_Fisica_Veiculo
    {
        public int Idf_Pessoa_Fisica { get; set; }

        [Key]
        public int Idf_Veiculo { get; set; }

        public int Idf_Tipo_Veiculo { get; set; }

        [StringLength(50)]
        public string Des_Modelo { get; set; }

        public short Ano_Veiculo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public bool? Flg_Veiculo_Proprio { get; set; }

        [StringLength(7)]
        public string Des_placa_Veiculo { get; set; }

        [StringLength(15)]
        public string Num_Renavam { get; set; }

        public bool? Flg_Seguro_Veiculo { get; set; }

        public DateTime? Dta_Vencimento_Seguro { get; set; }

        public virtual TAB_Pessoa_Fisica TAB_Pessoa_Fisica { get; set; }

        public virtual TAB_Tipo_Veiculo TAB_Tipo_Veiculo { get; set; }
    }
}
