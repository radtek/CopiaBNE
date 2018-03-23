namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.Tab_Disponibilidade")]
    public partial class Tab_Disponibilidade
    {
        public Tab_Disponibilidade()
        {
            BNE_Curriculo_Disponibilidade = new HashSet<BNE_Curriculo_Disponibilidade>();
            BNE_Rastreador_Disponibilidade = new HashSet<BNE_Rastreador_Disponibilidade>();
            BNE_Vaga_Disponibilidade = new HashSet<BNE_Vaga_Disponibilidade>();
            TAB_Pesquisa_Curriculo_Disponibilidade = new HashSet<TAB_Pesquisa_Curriculo_Disponibilidade>();
            TAB_Pesquisa_Vaga_Disponibilidade = new HashSet<TAB_Pesquisa_Vaga_Disponibilidade>();
        }

        [Key]
        public int Idf_Disponibilidade { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Disponibilidade { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Curriculo_Disponibilidade> BNE_Curriculo_Disponibilidade { get; set; }

        public virtual ICollection<BNE_Rastreador_Disponibilidade> BNE_Rastreador_Disponibilidade { get; set; }

        public virtual ICollection<BNE_Vaga_Disponibilidade> BNE_Vaga_Disponibilidade { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo_Disponibilidade> TAB_Pesquisa_Curriculo_Disponibilidade { get; set; }

        public virtual ICollection<TAB_Pesquisa_Vaga_Disponibilidade> TAB_Pesquisa_Vaga_Disponibilidade { get; set; }
    }
}
