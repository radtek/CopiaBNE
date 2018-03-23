namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Mensagem_Mailing")]
    public partial class TAB_Mensagem_Mailing
    {
        [Key]
        [Column(Order = 0)]
        public int Idf_Mensagem_CS { get; set; }

        public DateTime? Dta_Envio { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Mensagem { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Status_Mensagem { get; set; }

        [Key]
        [Column(Order = 3)]
        public string Des_Mensagem { get; set; }

        [Key]
        [Column(Order = 4)]
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

        [Key]
        [Column(Order = 5)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Sistema { get; set; }

        [Key]
        [Column(Order = 6)]
        public DateTime Dta_Cadastro { get; set; }

        [Key]
        [Column(Order = 7)]
        public bool Flg_Inativo { get; set; }
    }
}
