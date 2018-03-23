namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Repositorio_XML")]
    public partial class TAB_Repositorio_XML
    {
        public int id { get; set; }

        public string vaga { get; set; }
    }
}
