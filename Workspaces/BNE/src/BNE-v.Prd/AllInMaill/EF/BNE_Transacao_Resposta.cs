namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Transacao_Resposta")]
    public partial class BNE_Transacao_Resposta
    {
        [Key]
        public int Idf_Transacao_Resposta { get; set; }

        public int Idf_Transacao { get; set; }

        public bool Flg_Transacao_Aprovada { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_Resultado_Solicitacao_Aprovacao { get; set; }

        [StringLength(10)]
        public string Des_Codigo_Autorizacao { get; set; }

        [StringLength(14)]
        public string Des_Transacao { get; set; }

        [StringLength(19)]
        public string Des_Cartao_Mascarado { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? Num_Sequencial_Unico { get; set; }

        public string Des_Comprovante_Administradora { get; set; }

        public string Des_Nacionalidade_Emissor { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public virtual BNE_Transacao BNE_Transacao { get; set; }
    }
}
