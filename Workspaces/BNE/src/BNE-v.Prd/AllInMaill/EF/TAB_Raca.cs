namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Raca")]
    public partial class TAB_Raca
    {
        public TAB_Raca()
        {
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pessoa_Fisica = new HashSet<TAB_Pessoa_Fisica>();
            BNE_Integracao = new HashSet<BNE_Integracao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Raca { get; set; }

        [Required]
        [StringLength(20)]
        public string Des_Raca { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [StringLength(20)]
        public string Des_Raca_BNE { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica> TAB_Pessoa_Fisica { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }
    }
}
