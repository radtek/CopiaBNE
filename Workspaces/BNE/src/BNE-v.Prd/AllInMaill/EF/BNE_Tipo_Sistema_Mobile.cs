namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Tipo_Sistema_Mobile")]
    public partial class BNE_Tipo_Sistema_Mobile
    {
        public BNE_Tipo_Sistema_Mobile()
        {
            BNE_Mobile_Token = new HashSet<BNE_Mobile_Token>();
        }

        [Key]
        public int Idf_Tipo_Sistema_Mobile { get; set; }

        [Required]
        [StringLength(100)]
        public string Des_Tipo_Sistema_Mobile { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Mobile_Token> BNE_Mobile_Token { get; set; }
    }
}
