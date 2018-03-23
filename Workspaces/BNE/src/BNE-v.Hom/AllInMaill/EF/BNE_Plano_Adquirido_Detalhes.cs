namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Plano_Adquirido_Detalhes")]
    public partial class BNE_Plano_Adquirido_Detalhes
    {
        [Key]
        public int Idf_Plano_Adquirido_Detalhes { get; set; }

        public int Idf_Plano_Adquirido { get; set; }

        public bool Flg_Nota_Fiscal { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Res_Plano_Adquirido { get; set; }

        public int? Idf_Funcao { get; set; }

        [Required]
        [StringLength(2)]
        public string Num_Res_DDD_Telefone { get; set; }

        [Required]
        [StringLength(10)]
        public string Num_Res_Telefone { get; set; }

        [Required]
        [StringLength(100)]
        public string Eml_Envio_Boleto { get; set; }

        [StringLength(2000)]
        public string Des_Observacao { get; set; }

        public virtual BNE_Plano_Adquirido BNE_Plano_Adquirido { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
