namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.BNE_Amplitude_Salarial")]
    public partial class BNE_Amplitude_Salarial
    {
        [Key]
        public int Idf_Amplitude_Salarial { get; set; }

        public int Idf_Funcao { get; set; }

        public decimal? Vlr_Mediana { get; set; }

        public decimal? Vlr_Amplitude_Inferior { get; set; }

        public decimal? Vlr_Amplitude_Superior { get; set; }

        public int? NR_Populacao { get; set; }

        public DateTime? Dta_Amostra { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public decimal? Vlr_Amplitude_Inferior_Alterada { get; set; }

        public decimal? Vlr_Amplitude_Superior_Alterada { get; set; }

        public virtual TAB_Funcao TAB_Funcao { get; set; }
    }
}
