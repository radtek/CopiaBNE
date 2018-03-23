namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Tipo_Veiculo")]
    public partial class TAB_Tipo_Veiculo
    {
        public TAB_Tipo_Veiculo()
        {
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            TAB_Pesquisa_Curriculo = new HashSet<TAB_Pesquisa_Curriculo>();
            TAB_Pessoa_Fisica_Complemento = new HashSet<TAB_Pessoa_Fisica_Complemento>();
            TAB_Pessoa_Fisica_Veiculo = new HashSet<TAB_Pessoa_Fisica_Veiculo>();
            BNE_Integracao = new HashSet<BNE_Integracao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Veiculo { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Tipo_Veiculo { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual ICollection<TAB_Pesquisa_Curriculo> TAB_Pesquisa_Curriculo { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Complemento> TAB_Pessoa_Fisica_Complemento { get; set; }

        public virtual ICollection<TAB_Pessoa_Fisica_Veiculo> TAB_Pessoa_Fisica_Veiculo { get; set; }

        public virtual ICollection<BNE_Integracao> BNE_Integracao { get; set; }
    }
}
