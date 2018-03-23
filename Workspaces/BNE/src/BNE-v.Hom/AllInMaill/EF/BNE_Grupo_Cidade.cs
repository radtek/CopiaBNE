namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Grupo_Cidade")]
    public partial class BNE_Grupo_Cidade
    {
        public BNE_Grupo_Cidade()
        {
            BNE_Email_Destinatario_Cidade = new HashSet<BNE_Email_Destinatario_Cidade>();
            BNE_Lista_Cidade = new HashSet<BNE_Lista_Cidade>();
        }

        [Key]
        public int Idf_Grupo_Cidade { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Inativo { get; set; }

        public int? Idf_Filial { get; set; }

        public virtual ICollection<BNE_Email_Destinatario_Cidade> BNE_Email_Destinatario_Cidade { get; set; }

        public virtual TAB_Filial TAB_Filial { get; set; }

        public virtual ICollection<BNE_Lista_Cidade> BNE_Lista_Cidade { get; set; }
    }
}
