namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pesquisa_Curriculo_Disponibilidade")]
    public partial class TAB_Pesquisa_Curriculo_Disponibilidade
    {
        [Key]
        public int Idf_Pesquisa_Curriculo_Disponibilidade { get; set; }

        public int Idf_Pesquisa_Curriculo { get; set; }

        public int Idf_Disponibilidade { get; set; }

        public virtual Tab_Disponibilidade Tab_Disponibilidade { get; set; }

        public virtual TAB_Pesquisa_Curriculo TAB_Pesquisa_Curriculo { get; set; }
    }
}
