namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Origem_Filial")]
    public partial class TAB_Origem_Filial
    {
        public TAB_Origem_Filial()
        {
            TAB_Origem_Filial_Funcao = new HashSet<TAB_Origem_Filial_Funcao>();
        }

        public int Idf_Filial { get; set; }

        [Key]
        public int Idf_Origem_Filial { get; set; }

        public int Idf_Origem { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        [Required]
        [StringLength(10)]
        public string Des_IP { get; set; }

        public bool Flg_Inativo { get; set; }

        public bool Flg_Curriculo_Publico { get; set; }

        [StringLength(100)]
        public string Des_Diretorio { get; set; }

        public byte[] Img_Logo { get; set; }

        public int? Idf_Template { get; set; }

        public string Des_Mensagem_Candidato { get; set; }

        public string Des_Pagina_Inicial { get; set; }

        public bool Flg_Todas_Funcoes { get; set; }

        public virtual BNE_Template BNE_Template { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual TAB_Origem TAB_Origem { get; set; }

        public virtual ICollection<TAB_Origem_Filial_Funcao> TAB_Origem_Filial_Funcao { get; set; }
    }
}
