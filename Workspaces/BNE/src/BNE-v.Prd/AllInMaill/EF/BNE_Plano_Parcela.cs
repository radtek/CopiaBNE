namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Plano_Parcela")]
    public partial class BNE_Plano_Parcela
    {
        public BNE_Plano_Parcela()
        {
            BNE_Pagamento = new HashSet<BNE_Pagamento>();
        }

        public int Idf_Plano_Adquirido { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Pagamento { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Vlr_Parcela { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Plano_Parcela_Situacao { get; set; }

        [Key]
        public int Idf_Plano_Parcela { get; set; }

        public int? Num_Desconto { get; set; }

        public int Qtd_SMS_Total { get; set; }

        public int? Qtd_SMS_Liberada { get; set; }

        public virtual ICollection<BNE_Pagamento> BNE_Pagamento { get; set; }

        public virtual BNE_Plano_Adquirido BNE_Plano_Adquirido { get; set; }

        public virtual BNE_Plano_Parcela_Situacao BNE_Plano_Parcela_Situacao { get; set; }
    }
}
