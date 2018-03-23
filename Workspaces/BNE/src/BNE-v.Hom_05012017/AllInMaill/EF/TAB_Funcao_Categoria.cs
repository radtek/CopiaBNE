namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Funcao_Categoria")]
    public partial class TAB_Funcao_Categoria
    {
        public TAB_Funcao_Categoria()
        {
            BNE_Rede_Social_Conta = new HashSet<BNE_Rede_Social_Conta>();
            TAB_Funcao = new HashSet<TAB_Funcao>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Funcao_Categoria { get; set; }

        [Required]
        [StringLength(50)]
        public string Des_Funcao_Categoria { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [Required]
        [StringLength(1)]
        public string Cod_Funcao_Categoria { get; set; }

        public virtual ICollection<BNE_Rede_Social_Conta> BNE_Rede_Social_Conta { get; set; }

        public virtual ICollection<TAB_Funcao> TAB_Funcao { get; set; }
    }
}
