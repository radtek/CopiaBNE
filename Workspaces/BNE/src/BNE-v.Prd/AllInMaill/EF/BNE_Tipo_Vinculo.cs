namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Tipo_Vinculo")]
    public partial class BNE_Tipo_Vinculo
    {
        public BNE_Tipo_Vinculo()
        {
            BNE_Vaga_Tipo_Vinculo = new HashSet<BNE_Vaga_Tipo_Vinculo>();
            TAB_Pesquisa_Vaga_Tipo_Vinculo = new HashSet<TAB_Pesquisa_Vaga_Tipo_Vinculo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Vinculo { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Tipo_Vinculo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Cod_Grau_Tipo_Vinculo { get; set; }

        public virtual ICollection<BNE_Vaga_Tipo_Vinculo> BNE_Vaga_Tipo_Vinculo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga_Tipo_Vinculo> TAB_Pesquisa_Vaga_Tipo_Vinculo { get; set; }
    }
}
