namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Revidor")]
    public partial class BNE_Revidor
    {
        public BNE_Revidor()
        {
            BNE_Curriculo_Fila = new HashSet<BNE_Curriculo_Fila>();
        }

        [Key]
        public int Idf_Revidor { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public int Num_Ordem { get; set; }

        public bool Flg_Inativo { get; set; }

        public virtual ICollection<BNE_Curriculo_Fila> BNE_Curriculo_Fila { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
