namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Email_Destinatario_Cidade")]
    public partial class BNE_Email_Destinatario_Cidade
    {
        [Key]
        public int Idf_Email_Destinatario_Cidade { get; set; }

        public int Idf_Grupo_Cidade { get; set; }

        public int Idf_Email_Destinatario { get; set; }

        public bool Flg_Responsavel { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public DateTime Dta_Alteracao { get; set; }

        public bool Flg_Inativo { get; set; }

        public int Idf_Usuario_Gerador { get; set; }

        public int? idf_Filial { get; set; }

        public virtual BNE_Email_Destinatario BNE_Email_Destinatario { get; set; }

        public virtual BNE_Grupo_Cidade BNE_Grupo_Cidade { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
