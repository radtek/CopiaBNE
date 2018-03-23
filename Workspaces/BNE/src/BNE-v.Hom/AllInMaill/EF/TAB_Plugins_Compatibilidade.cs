namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("plataforma.TAB_Plugins_Compatibilidade")]
    public partial class TAB_Plugins_Compatibilidade
    {
        public TAB_Plugins_Compatibilidade()
        {
            TAB_Atividade = new HashSet<TAB_Atividade>();
        }

        [Key]
        public int Idf_Plugins_Compatibilidade { get; set; }

        public int Idf_Plugin_Entrada { get; set; }

        public int Idf_Plugin_Saida { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Atividade> TAB_Atividade { get; set; }

        public virtual TAB_Plugin TAB_Plugin { get; set; }

        public virtual TAB_Plugin TAB_Plugin1 { get; set; }
    }
}
