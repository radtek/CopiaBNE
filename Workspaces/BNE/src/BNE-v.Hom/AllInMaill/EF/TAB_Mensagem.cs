namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Mensagem")]
    public partial class TAB_Mensagem
    {
        [Key]
        public int Idf_Mensagem { get; set; }

        public int Idf_Mensagem_Recebida { get; set; }

        public int Idf_Tipo_Mensagem { get; set; }

        public int Idf_Sistema { get; set; }

        public int Idf_Centro_Servico { get; set; }

        [Required]
        [StringLength(2000)]
        public string Des_Mensagem { get; set; }

        public DateTime? Dta_Envio { get; set; }

        [StringLength(100)]
        public string Des_Email_Destinatario { get; set; }

        [StringLength(100)]
        public string Des_Email_Assunto { get; set; }

        public string Arq_Anexo { get; set; }

        public int Idf_Status_Mensagem { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [StringLength(100)]
        public string Des_Email_Remetente { get; set; }

        [StringLength(2)]
        public string Num_DDD_Celular { get; set; }

        [StringLength(10)]
        public string Num_Celular { get; set; }

        public bool Flg_Inativo { get; set; }

        [StringLength(50)]
        public string Nme_Anexo { get; set; }

        [StringLength(100)]
        public string Des_Obs { get; set; }

        public int? Idf_Rede_Social { get; set; }

        public virtual TAB_Centro_Servico TAB_Centro_Servico { get; set; }

        public virtual TAB_Sistema TAB_Sistema { get; set; }
    }
}
