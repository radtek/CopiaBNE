namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Estado")]
    public partial class TAB_Estado
    {
        public TAB_Estado()
        {
            BNE_Propaganda_Estado = new HashSet<BNE_Propaganda_Estado>();
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            TAB_Feriado = new HashSet<TAB_Feriado>();
            TAB_Feriado_Modelo = new HashSet<TAB_Feriado_Modelo>();
            TAB_Pesquisa_Salarial = new HashSet<TAB_Pesquisa_Salarial>();
            TAB_Inscricao_Estadual = new HashSet<TAB_Inscricao_Estadual>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Estado { get; set; }

        [Required]
        [StringLength(2)]
        public string Sig_Estado { get; set; }

        [Required]
        [StringLength(50)]
        public string Nme_Estado { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int? Idf_Regiao { get; set; }

        public virtual ICollection<BNE_Propaganda_Estado> BNE_Propaganda_Estado { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<TAB_Feriado> TAB_Feriado { get; set; }

        public virtual ICollection<TAB_Feriado_Modelo> TAB_Feriado_Modelo { get; set; }

        public virtual ICollection<TAB_Pesquisa_Salarial> TAB_Pesquisa_Salarial { get; set; }

        public virtual ICollection<TAB_Inscricao_Estadual> TAB_Inscricao_Estadual { get; set; }

        public virtual TAB_Regiao TAB_Regiao { get; set; }
    }
}
