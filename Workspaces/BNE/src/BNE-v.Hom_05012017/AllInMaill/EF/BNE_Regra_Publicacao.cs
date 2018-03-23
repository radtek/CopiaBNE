namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Regra_Publicacao")]
    public partial class BNE_Regra_Publicacao
    {
        public BNE_Regra_Publicacao()
        {
            BNE_Regra_Campo_Publicacao = new HashSet<BNE_Regra_Campo_Publicacao>();
        }

        [Key]
        public int Idf_Regra_Publicacao { get; set; }

        public int? Idf_Palavra_Publicacao { get; set; }

        public int Idf_Acao_Publicacao { get; set; }

        public bool Flg_Aplicar_Regex { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual BNE_Acao_Publicacao BNE_Acao_Publicacao { get; set; }

        public virtual BNE_Palavra_Publicacao BNE_Palavra_Publicacao { get; set; }

        public virtual ICollection<BNE_Regra_Campo_Publicacao> BNE_Regra_Campo_Publicacao { get; set; }
    }
}
