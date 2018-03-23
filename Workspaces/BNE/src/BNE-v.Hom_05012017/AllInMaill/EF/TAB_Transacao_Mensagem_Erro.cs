namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Transacao_Mensagem_Erro")]
    public partial class TAB_Transacao_Mensagem_Erro
    {
        [Key]
        public int Idf_Transacao_Mensagem_Erro { get; set; }

        [StringLength(10)]
        public string Des_Codigo_Erro { get; set; }

        [StringLength(500)]
        public string Des_Descricao_Erro { get; set; }

        [Required]
        [StringLength(500)]
        public string Des_Mensagem_Amigavel { get; set; }
    }
}
