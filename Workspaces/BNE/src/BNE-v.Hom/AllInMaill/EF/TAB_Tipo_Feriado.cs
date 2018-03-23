namespace AllInMail
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BNE.TAB_Tipo_Feriado")]
    public partial class TAB_Tipo_Feriado
    {
        public TAB_Tipo_Feriado()
        {
            TAB_Feriado = new HashSet<TAB_Feriado>();
            TAB_Feriado_Modelo = new HashSet<TAB_Feriado_Modelo>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Idf_Tipo_Feriado { get; set; }

        [Required]
        [StringLength(30)]
        public string Des_Tipo_Feriado { get; set; }

        public bool Flg_Inativo { get; set; }

        public DateTime Dta_Cadastro { get; set; }

        public virtual ICollection<TAB_Feriado> TAB_Feriado { get; set; }

        public virtual ICollection<TAB_Feriado_Modelo> TAB_Feriado_Modelo { get; set; }
    }
}
