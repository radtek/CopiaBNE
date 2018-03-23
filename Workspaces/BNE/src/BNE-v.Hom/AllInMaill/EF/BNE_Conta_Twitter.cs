namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Conta_Twitter")]
    public partial class BNE_Conta_Twitter
    {
        public BNE_Conta_Twitter()
        {
            BNE_Rede_Social_Conta = new HashSet<BNE_Rede_Social_Conta>();
        }

        [Key]
        public int Idf_Conta_Twitter { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Consumer_Key { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Consumer_Secret { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Access_Token { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Access_Token_Secret { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Login { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Senha { get; set; }

        public virtual ICollection<BNE_Rede_Social_Conta> BNE_Rede_Social_Conta { get; set; }
    }
}
