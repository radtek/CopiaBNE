namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Plano")]
    public partial class BNE_Plano
    {
        public BNE_Plano()
        {
            BNE_Codigo_Desconto_Plano = new HashSet<BNE_Codigo_Desconto_Plano>();
            BNE_Plano_Adquirido = new HashSet<BNE_Plano_Adquirido>();
        }

        [Key]
        public int Idf_Plano { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Plano { get; set; }

        public int Qtd_Dias_Validade { get; set; }

        public int Qtd_SMS { get; set; }

        public int Qtd_Visualizacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public decimal Vlr_Base { get; set; }

        public DateTime? Dta_Inicio { get; set; }

        public DateTime? Dta_Final { get; set; }

        public int Idf_Plano_Tipo { get; set; }

        public int Idf_Plano_Forma_Pagamento { get; set; }

        public int Qtd_Parcela { get; set; }

        public int Vlr_Desconto_Maximo { get; set; }

        public int? Qtd_SMS_Maxima { get; set; }

        public decimal? Vlr_Base_Minimo { get; set; }

        public int? Qtd_Prazo_Boleto_Maxima { get; set; }

        public bool Flg_Boleto_Registrado { get; set; }

        public int? Idf_Tipo_Contrato { get; set; }

        public virtual ICollection<BNE_Codigo_Desconto_Plano> BNE_Codigo_Desconto_Plano { get; set; }

        public virtual BNE_Plano_Forma_Pagamento BNE_Plano_Forma_Pagamento { get; set; }

        public virtual ICollection<BNE_Plano_Adquirido> BNE_Plano_Adquirido { get; set; }

        public virtual BNE_Plano_Tipo BNE_Plano_Tipo { get; set; }

        public virtual BNE_Tipo_Contrato BNE_Tipo_Contrato { get; set; }
    }
}
