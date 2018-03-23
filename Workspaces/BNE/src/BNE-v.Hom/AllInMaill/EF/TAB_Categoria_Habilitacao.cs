namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Categoria_Habilitacao")]
    public partial class TAB_Categoria_Habilitacao
    {
        public TAB_Categoria_Habilitacao()
        {
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Solicitacao_R1 = new HashSet<BNE_Solicitacao_R1>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pessoa_Fisica_Complemento = new HashSet<TAB_Pessoa_Fisica_Complemento>();
            BNE_Integracao = new HashSet<BNE_Integracao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Categoria_Habilitacao { get; set; }

        [Required]
        [StringLength(2)]
        public string Des_Categoria_Habilitacao { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<BNE_Solicitacao_R1> BNE_Solicitacao_R1 { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Complemento> TAB_Pessoa_Fisica_Complemento { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }
    }
}
