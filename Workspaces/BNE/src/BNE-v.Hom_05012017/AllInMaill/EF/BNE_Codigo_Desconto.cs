namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Codigo_Desconto")]
    public partial class BNE_Codigo_Desconto
    {
        public BNE_Codigo_Desconto()
        {
            BNE_Pagamento = new HashSet<BNE_Pagamento>();
        }

        [Key]
        public int Idf_Codigo_Desconto { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_Codigo_Desconto { get; set; }

        public int Idf_Status_Codigo_Desconto { get; set; }

        public int? Idf_Parceiro { get; set; }

        public int? Idf_Tipo_Codigo_Desconto { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Alteracao { get; set; }

        public DateTime? Dta_Utilizacao { get; set; }

        public DateTime? Dta_Validade_Inicio { get; set; }

        public DateTime? Dta_Validade_Fim { get; set; }

        [StringLength(200)]
        public string Des_Identificacao_Codigo { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        public virtual BNE_Parceiro BNE_Parceiro { get; set; }

        public virtual TAB_Status_Codigo_Desconto TAB_Status_Codigo_Desconto { get; set; }

        public virtual BNE_Tipo_Codigo_Desconto BNE_Tipo_Codigo_Desconto { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual ICollection<BNE_Pagamento> BNE_Pagamento { get; set; }
    }
}
