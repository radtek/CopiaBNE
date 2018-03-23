namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Pagamento")]
    public partial class BNE_Pagamento
    {
        [Key]
        public int Idf_Pagamento { get; set; }

        public int? Idf_Filial { get; set; }

        public int? Idf_Tipo_Pagamento { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public DateTime? Dta_Emissao { get; set; }

        public DateTime? Dta_Vencimento { get; set; }

        [StringLength(50)]
        public string Des_Identificador { get; set; }

        [StringLength(100)]
        public string Des_Descricao { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Vlr_Pagamento { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Avulso { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Pagamento_Situacao { get; set; }

        public int? Idf_Operadora { get; set; }

        public int? Idf_Plano_Parcela { get; set; }

        public int? IDF_Usuario_Gerador { get; set; }

        [StringLength(100)]
        public string Cod_Guid { get; set; }

        public int? Idf_Adicional_Plano { get; set; }

        [StringLength(10)]
        public string Num_Nota_Fiscal { get; set; }

        public int? Idf_Codigo_Desconto { get; set; }

        public int? Idf_Transacao { get; set; }

        public bool Flg_baixado { get; set; }

        public virtual BNE_Adicional_Plano BNE_Adicional_Plano { get; set; }

        public virtual BNE_Codigo_Desconto BNE_Codigo_Desconto { get; set; }

        public virtual BNE_Operadora BNE_Operadora { get; set; }

        public virtual BNE_Transacao BNE_Transacao { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual BNE_Plano_Parcela BNE_Plano_Parcela { get; set; }

        public virtual BNE_Pagamento_Situacao BNE_Pagamento_Situacao { get; set; }

        public virtual BNE_Tipo_Pagamento BNE_Tipo_Pagamento { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil1 { get; set; }
    }
}
