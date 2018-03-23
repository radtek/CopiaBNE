namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Idioma")]
    public partial class TAB_Idioma
    {
        public TAB_Idioma()
        {
            BNE_Curriculo_Idioma = new HashSet<BNE_Curriculo_Idioma>();
            BNE_Rastreador_Idioma = new HashSet<BNE_Rastreador_Idioma>();
            TAB_Pesquisa_Curriculo_Idioma = new HashSet<TAB_Pesquisa_Curriculo_Idioma>();
            TAB_Pessoa_Fisica_Idioma = new HashSet<TAB_Pessoa_Fisica_Idioma>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
        }

        [Key]
        public int Idf_Idioma { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Idioma { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Curriculo_Idioma> BNE_Curriculo_Idioma { get; set; }

        public virtual ICollection<BNE_Rastreador_Idioma> BNE_Rastreador_Idioma { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo_Idioma> TAB_Pesquisa_Curriculo_Idioma { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Idioma> TAB_Pessoa_Fisica_Idioma { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }
    }
}
