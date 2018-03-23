namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Campanha")]
    public partial class BNE_Campanha
    {
        public BNE_Campanha()
        {
            BNE_Campanha_Curriculo = new HashSet<BNE_Campanha_Curriculo>();
        }

        [Key]
        public int Idf_Campanha { get; set; }

        public int Idf_Celular_Selecionador { get; set; }

        [Required]
        [StringLength(200)]
        public string Nme_Campanha { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime? Dta_Envio { get; set; }

        [Required]
        [StringLength(200)]
        public string Des_Mensagem { get; set; }

        public virtual BNE_Celular_Selecionador BNE_Celular_Selecionador { get; set; }

        public virtual ICollection<BNE_Campanha_Curriculo> BNE_Campanha_Curriculo { get; set; }
    }
}
