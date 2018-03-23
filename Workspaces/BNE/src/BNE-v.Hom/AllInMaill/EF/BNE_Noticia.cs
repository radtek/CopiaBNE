namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Noticia")]
    public partial class BNE_Noticia
    {
        public BNE_Noticia()
        {
            BNE_Noticia_Visualizacao = new HashSet<BNE_Noticia_Visualizacao>();
        }

        [Key]
        public int Idf_Noticia { get; set; }

        public string Des_Noticia { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [Required]
        [StringLength(100)]
        public string Nme_Titulo_Noticia { get; set; }

        [StringLength(300)]
        public string Nme_Link_Noticia { get; set; }

        public bool Flg_Noticia_BNE { get; set; }

        public bool Flg_Exibicao { get; set; }

        public DateTime? Dta_Publicacao { get; set; }

        public virtual ICollection<BNE_Noticia_Visualizacao> BNE_Noticia_Visualizacao { get; set; }
    }
}
