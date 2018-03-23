namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Arquivo")]
    public partial class BNE_Arquivo
    {
        public BNE_Arquivo()
        {
            BNE_Linha_Arquivo = new HashSet<BNE_Linha_Arquivo>();
        }

        [Key]
        public int Idf_Arquivo { get; set; }

        public DateTime Dta_Remessa { get; set; }

        [Required]
        [StringLength(50)]
        public string Nme_Arquivo { get; set; }

        public int Idf_Tipo_Arquivo { get; set; }

        [Required]
        public string Dsc_Conteudo_Arquivo { get; set; }

        public virtual TAB_Tipo_Arquivo TAB_Tipo_Arquivo { get; set; }

        public virtual ICollection<BNE_Linha_Arquivo> BNE_Linha_Arquivo { get; set; }
    }
}
