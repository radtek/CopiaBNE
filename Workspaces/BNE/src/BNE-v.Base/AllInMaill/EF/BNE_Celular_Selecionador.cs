namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Celular_Selecionador")]
    public partial class BNE_Celular_Selecionador
    {
        public BNE_Celular_Selecionador()
        {
            BNE_Campanha = new HashSet<BNE_Campanha>();
        }

        [Key]
        public int Idf_Celular_Selecionador { get; set; }

        public int Idf_Usuario_Filial_Perfil { get; set; }

        public int Idf_Celular { get; set; }

        public DateTime Dta_Inicio_Utilizacao { get; set; }

        public DateTime? Dta_Fim_Utilizacao { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public bool Flg_Utiliza_Servico_Tanque { get; set; }

        public virtual ICollection<BNE_Campanha> BNE_Campanha { get; set; }

        public virtual BNE_Celular BNE_Celular { get; set; }

        public virtual TAB_Usuario_Filial_Perfil TAB_Usuario_Filial_Perfil { get; set; }
    }
}
