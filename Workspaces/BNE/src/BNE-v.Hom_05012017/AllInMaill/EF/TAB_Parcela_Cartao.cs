namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Parcela_Cartao")]
    public partial class TAB_Parcela_Cartao
    {
        public TAB_Parcela_Cartao()
        {
            GLO_Cobranca_Cartao = new HashSet<GLO_Cobranca_Cartao>();
        }

        [Key]
        public int Idf_Parcela_Cartao { get; set; }

        [Required]
        [StringLength(20)]
        public string Des_Parcela { get; set; }

        public virtual ICollection<GLO_Cobranca_Cartao> GLO_Cobranca_Cartao { get; set; }
    }
}
