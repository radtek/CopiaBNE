namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Curso_Fonte")]
    public partial class TAB_Curso_Fonte
    {
        public TAB_Curso_Fonte()
        {
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
        }

        public int Idf_Fonte { get; set; }

        public int Idf_Curso { get; set; }

        public bool Flg_Manha { get; set; }

        [StringLength(100)]
        public string Des_Curso { get; set; }

        public bool Flg_Tarde { get; set; }

        public bool Flg_Noite { get; set; }

        public long? Qtd_Carga_Horaria { get; set; }

        [StringLength(50)]
        public string Des_Pagamento { get; set; }

        [StringLength(50)]
        public string Des_Contato { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Atualizacao { get; set; }

        [StringLength(1000)]
        public string Des_Obs { get; set; }

        [StringLength(100)]
        public string Des_Duracao { get; set; }

        [Key]
        public int Idf_Curso_Fonte { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual TAB_Curso TAB_Curso { get; set; }

        public virtual TAB_Fonte TAB_Fonte { get; set; }
    }
}
