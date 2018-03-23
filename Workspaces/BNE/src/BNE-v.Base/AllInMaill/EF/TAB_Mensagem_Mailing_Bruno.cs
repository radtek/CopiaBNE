namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Mensagem_Mailing_Bruno")]
    public partial class TAB_Mensagem_Mailing_Bruno
    {
        [Key]
        public int Idf_Mensagem_CS { get; set; }

        public DateTime? Dta_Envio { get; set; }

        public int Idf_Tipo_Mensagem { get; set; }

        public int Idf_Status_Mensagem { get; set; }

        [Required]
        public string Des_Mensagem { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Email_Remetente { get; set; }

        [StringLength(100)]
        public string Des_Email_Destino { get; set; }

        [StringLength(100)]
        public string Des_Assunto { get; set; }

        [StringLength(50)]
        public string Nme_Anexo { get; set; }

        public byte[] Arq_Anexo { get; set; }

        [StringLength(2)]
        public string Num_DDD_Celular { get; set; }

        [StringLength(10)]
        public string Num_Celular { get; set; }

        public int Idf_Sistema { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }
    }
}
