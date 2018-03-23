namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Vinculo_Integracao")]
    public partial class TAB_Tipo_Vinculo_Integracao
    {
        public TAB_Tipo_Vinculo_Integracao()
        {
            BNE_Integracao = new HashSet<BNE_Integracao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Vinculo_Integracao { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Tipo_Vinculo { get; set; }

        public bool Flg_Prazo_Determinado { get; set; }

        public short? Qtd_Prazo_Padrao { get; set; }

        public bool Flg_Prazo_Variavel { get; set; }

        public bool Flg_Experiencia { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public short? Cod_Categoria_Trabalhador { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }
    }
}
