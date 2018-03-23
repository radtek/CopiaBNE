namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Vaga_Integracao")]
    public partial class BNE_Vaga_Integracao
    {
        [Key]
        public int Idf_Vaga_Integracao { get; set; }

        public int Idf_Vaga { get; set; }

        public int Idf_Integrador { get; set; }

        [Required]
        [StringLength(50)]
        public string Cod_Vaga_Integrador { get; set; }

        public bool Flg_Inativo { get; set; }

        public bool Flg_Enviada_Para_Auditoria { get; set; }

        public virtual BNE_Vaga BNE_Vaga { get; set; }

        public virtual TAB_Integrador TAB_Integrador { get; set; }
    }
}
