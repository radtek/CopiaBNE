namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Regiao_Metropolitana")]
    public partial class TAB_Regiao_Metropolitana
    {
        public TAB_Regiao_Metropolitana()
        {
            TAB_Regiao_Metropolitana_Cidade = new HashSet<TAB_Regiao_Metropolitana_Cidade>();
        }

        [Key]
        public int Idf_Regiao_Metropolitana { get; set; }

        [Required]
        [StringLength(200)]
        public string Nme_Regiao_Metropolitana { get; set; }

        [Required]
        [StringLength(200)]
        public string Nme_Regiao_Metropolitana_Pesquisa { get; set; }

        public int Idf_Cidade { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        [StringLength(2)]
        public string Sig_UF { get; set; }

        [StringLength(200)]
        public string CID_Regiao_Metropolitana { get; set; }

        public virtual TAB_Cidade TAB_Cidade { get; set; }

        public virtual ICollection<TAB_Regiao_Metropolitana_Cidade> TAB_Regiao_Metropolitana_Cidade { get; set; }
    }
}
