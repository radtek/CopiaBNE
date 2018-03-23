namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Celular")]
    public partial class BNE_Celular
    {
        public BNE_Celular()
        {
            BNE_Celular_Selecionador = new HashSet<BNE_Celular_Selecionador>();
        }

        [Key]
        public int Idf_Celular { get; set; }

        [Required]
        [StringLength(200)]
        public string Cod_Imei_Celular { get; set; }

        [Required]
        [StringLength(200)]
        public string Cod_Token_Celular { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Inativo { get; set; }

        public virtual ICollection<BNE_Celular_Selecionador> BNE_Celular_Selecionador { get; set; }
    }
}
