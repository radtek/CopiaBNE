namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Pesquisa_Vaga_Tipo_Vinculo")]
    public partial class TAB_Pesquisa_Vaga_Tipo_Vinculo
    {
        [Key]
        public int Idf_Pesquisa_Vaga_Tipo_Vinculo { get; set; }

        public int? Idf_Tipo_Vinculo { get; set; }

        public int? Idf_Pesquisa_Vaga { get; set; }

        public virtual BNE_Tipo_Vinculo BNE_Tipo_Vinculo { get; set; }

        public virtual TAB_Pesquisa_Vaga TAB_Pesquisa_Vaga { get; set; }
    }
}
