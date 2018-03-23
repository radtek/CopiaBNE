namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Email_Destinatario")]
    public partial class BNE_Email_Destinatario
    {
        public BNE_Email_Destinatario()
        {
            BNE_Email_Destinatario_Cidade = new HashSet<BNE_Email_Destinatario_Cidade>();
        }

        [Key]
        public int Idf_Email_Destinatario { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Email { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Usuario_Gerador { get; set; }

        [Required]
        [StringLength(50)]
        public string Nme_Pessoa { get; set; }

        [StringLength(2)]
        public string Num_DDD_Telefone { get; set; }

        [StringLength(10)]
        public string Num_Telefone { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual ICollection<BNE_Email_Destinatario_Cidade> BNE_Email_Destinatario_Cidade { get; set; }
    }
}
