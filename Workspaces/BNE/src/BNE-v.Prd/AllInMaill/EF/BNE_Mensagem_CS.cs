namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Mensagem_CS")]
    public partial class BNE_Mensagem_CS
    {
        [Key]
        public int Idf_Mensagem_CS { get; set; }

        public int? Idf_Usuario_Filial_Perfil { get; set; }

        public int? Idf_Curriculo { get; set; }

        [Required]
        public string Des_Mensagem { get; set; }

        public DateTime? Dta_Envio { get; set; }

        public int? Idf_Rede_Social_CS { get; set; }

        public int Idf_Tipo_Mensagem_CS { get; set; }

        [StringLength(100)]
        public string Des_Email_Destinatario { get; set; }

        [StringLength(100)]
        public string Des_Email_Assunto { get; set; }

        public int Idf_Status_Mensagem_CS { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [Required]
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

        public int Idf_Sistema { get; set; }

        public int Idf_Centro_Servico { get; set; }

        public byte[] Arq_Anexo { get; set; }

        public int? Idf_Mensagem_Sistema { get; set; }

        public int? Idf_Usuario_Filial_Des { get; set; }

        public bool Flg_Lido { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }

        public virtual BNE_Mensagem_Sistema BNE_Mensagem_Sistema { get; set; }

        public virtual BNE_Status_Mensagem_CS BNE_Status_Mensagem_CS { get; set; }

        public virtual BNE_Tipo_Mensagem_CS BNE_Tipo_Mensagem_CS { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil1 { get; set; }

        public virtual BNE_Rede_Social_CS BNE_Rede_Social_CS { get; set; }
    }
}
