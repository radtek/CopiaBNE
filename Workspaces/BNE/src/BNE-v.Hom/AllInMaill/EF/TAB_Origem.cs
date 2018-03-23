namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Origem")]
    public partial class TAB_Origem
    {
        public TAB_Origem()
        {
            BNE_Curriculo_Origem = new HashSet<BNE_Curriculo_Origem>();
            BNE_Rastreador = new HashSet<BNE_Rastreador>();
            BNE_Vaga = new HashSet<BNE_Vaga>();
            TAB_Origem_Filial = new HashSet<TAB_Origem_Filial>();
        }

        [Key]
        public int Idf_Origem { get; set; }

        [StringLength(100)]
        public string Des_Origem { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        [StringLength(120)]
        public string Des_URL { get; set; }

        public int Idf_Tipo_Origem { get; set; }

        public virtual ICollection<BNE_Curriculo_Origem> BNE_Curriculo_Origem { get; set; }

        public virtual ICollection<BNE_Rastreador> BNE_Rastreador { get; set; }

        public virtual BNE_Tipo_Origem BNE_Tipo_Origem { get; set; }

        public virtual ICollection<BNE_Vaga> BNE_Vaga { get; set; }

        public virtual ICollection<TAB_Origem_Filial> TAB_Origem_Filial { get; set; }
    }
}
