namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pessoa_Fisica_Temp")]
    public partial class TAB_Pessoa_Fisica_Temp
    {
        public TAB_Pessoa_Fisica_Temp()
        {
            BNE_Mini_Curriculo = new HashSet<BNE_Mini_Curriculo>();
        }

        [Key]
        public int Idf_Pessoa_Fisica_Temp { get; set; }

        [Column(TypeName = "numeric")]
        public decimal Num_CPF { get; set; }

        public DateTime Dta_Nascimento { get; set; }

        [Required]
        [StringLength(2)]
        public string Num_DDD_Celular { get; set; }

        [Required]
        [StringLength(10)]
        public string Num_Celular { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Mini_Curriculo> BNE_Mini_Curriculo { get; set; }
    }
}
