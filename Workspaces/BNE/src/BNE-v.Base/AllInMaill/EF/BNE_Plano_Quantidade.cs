namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Plano_Quantidade")]
    public partial class BNE_Plano_Quantidade
    {
        [Key]
        public int Idf_Plano_Quantidade { get; set; }

        public DateTime Dta_Inicio_Quantidade { get; set; }

        public DateTime Dta_Fim_Quantidade { get; set; }

        public int Qtd_SMS { get; set; }

        public int Qtd_Visualizacao { get; set; }

        public int Qtd_SMS_Utilizado { get; set; }

        public int Qtd_Visualizacao_Utilizado { get; set; }

        public bool? Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Alteracao { get; set; }

        public int? Idf_Plano_Adquirido { get; set; }

        public virtual BNE_Plano_Adquirido BNE_Plano_Adquirido { get; set; }
    }
}
