namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Adicional_Plano")]
    public partial class BNE_Adicional_Plano
    {
        public BNE_Adicional_Plano()
        {
            BNE_Pagamento = new HashSet<BNE_Pagamento>();
        }

        [Key]
        public int Idf_Adicional_Plano { get; set; }

        public int Idf_Plano_Adquirido { get; set; }

        public int Idf_Tipo_Adicional { get; set; }

        public int Qtd_Adicional { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        public int? Idf_Adicional_Plano_Situacao { get; set; }

        public virtual ICollection<BNE_Pagamento> BNE_Pagamento { get; set; }

        public virtual BNE_Plano_Adquirido BNE_Plano_Adquirido { get; set; }

        public virtual BNE_Adicional_Plano_Situacao BNE_Adicional_Plano_Situacao { get; set; }

        public virtual BNE_Tipo_Adicional BNE_Tipo_Adicional { get; set; }
    }
}
