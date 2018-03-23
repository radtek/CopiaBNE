namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Rede_Social_CS")]
    public partial class BNE_Rede_Social_CS
    {
        public BNE_Rede_Social_CS()
        {
            BNE_Mensagem_CS = new HashSet<BNE_Mensagem_CS>();
            BNE_Rede_Social_Conta = new HashSet<BNE_Rede_Social_Conta>();
            BNE_Vaga_Divulgacao = new HashSet<BNE_Vaga_Divulgacao>();
            BNE_Vaga_Rede_Social = new HashSet<BNE_Vaga_Rede_Social>();
            TAB_Pessoa_Fisica_Rede_Social = new HashSet<TAB_Pessoa_Fisica_Rede_Social>();
        }

        [Key]
        public int Idf_Rede_Social_CS { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Rede_Social { get; set; }

        [Required]
        public byte[] Img_Rede_Social { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Max_Caracter { get; set; }

        public virtual ICollection<BNE_Mensagem_CS> BNE_Mensagem_CS { get; set; }

        public virtual ICollection<BNE_Rede_Social_Conta> BNE_Rede_Social_Conta { get; set; }

        public virtual ICollection<BNE_Vaga_Divulgacao> BNE_Vaga_Divulgacao { get; set; }

        public virtual ICollection<BNE_Vaga_Rede_Social> BNE_Vaga_Rede_Social { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Rede_Social> TAB_Pessoa_Fisica_Rede_Social { get; set; }
    }
}
