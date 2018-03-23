namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Conversas_Ativas")]
    public partial class BNE_Conversas_Ativas
    {
        [Key]
        public int Idf_Conversa_Ativa { get; set; }

        public int Idf_Curriculo { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public bool Flg_Mensagem_Pendente { get; set; }

        public bool? Flg_Armazenado { get; set; }

        public DateTime? Dta_Ultima_Atualizacao { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }

        public virtual BNE_Curriculo BNE_Curriculo { get; set; }
    }
}
