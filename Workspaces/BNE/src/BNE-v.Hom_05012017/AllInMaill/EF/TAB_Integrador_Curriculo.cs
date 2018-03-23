namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Integrador_Curriculo")]
    public partial class TAB_Integrador_Curriculo
    {
        public TAB_Integrador_Curriculo()
        {
            TAB_Requisicao_Integrador_Curriculo = new HashSet<TAB_Requisicao_Integrador_Curriculo>();
        }

        [Key]
        public int Idf_Integrador_Curriculo { get; set; }

        public int Idf_Filial { get; set; }

        [Required]
        [StringLength(200)]
        public string Sen_Integrador_Curriculo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual ICollection<TAB_Requisicao_Integrador_Curriculo> TAB_Requisicao_Integrador_Curriculo { get; set; }
    }
}
