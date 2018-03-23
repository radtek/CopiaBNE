namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pesquisa_Vaga_Disponibilidade")]
    public partial class TAB_Pesquisa_Vaga_Disponibilidade
    {
        [Key]
        public int Idf_Pesquisa_Vaga_Disponibilidade { get; set; }

        public int? Idf_Disponibilidade { get; set; }

        public int? Idf_Pesquisa_Vaga { get; set; }

        public virtual Tab_Disponibilidade Tab_Disponibilidade { get; set; }

        public virtual TAB_Pesquisa_Vaga TAB_Pesquisa_Vaga { get; set; }
    }
}
