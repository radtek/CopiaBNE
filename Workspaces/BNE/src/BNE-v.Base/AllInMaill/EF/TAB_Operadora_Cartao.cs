namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Operadora_Cartao")]
    public partial class TAB_Operadora_Cartao
    {
        public TAB_Operadora_Cartao()
        {
            GLO_Cobranca_Cartao = new HashSet<GLO_Cobranca_Cartao>();
        }

        [Key]
        public int Idf_Operadora_Cartao { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Operadora { get; set; }

        public DateTime? Dta_Cadastro { get; set; }

        public bool? Flg_Inativo { get; set; }

        [StringLength(2)]
        public string COD_OPERADORA { get; set; }

        public virtual ICollection<GLO_Cobranca_Cartao> GLO_Cobranca_Cartao { get; set; }
    }
}
